package okhttp3.internal.ws;

import android.support.v4.media.session.PlaybackStateCompat;
import java.io.IOException;
import java.util.Random;
import kotlinx.coroutines.scheduling.WorkQueueKt;
import okio.Buffer;
import okio.BufferedSink;
import okio.ByteString;
import okio.Sink;
import okio.Timeout;

final class WebSocketWriter {
    static final /* synthetic */ boolean $assertionsDisabled = false;
    boolean activeWriter;
    final Buffer buffer = new Buffer();
    final FrameSink frameSink = new FrameSink();
    final boolean isClient;
    final byte[] maskBuffer;
    final byte[] maskKey;
    final Random random;
    final BufferedSink sink;
    boolean writerClosed;

    WebSocketWriter(boolean isClient2, BufferedSink sink2, Random random2) {
        if (sink2 == null) {
            throw new NullPointerException("sink == null");
        } else if (random2 != null) {
            this.isClient = isClient2;
            this.sink = sink2;
            this.random = random2;
            byte[] bArr = null;
            this.maskKey = isClient2 ? new byte[4] : null;
            this.maskBuffer = isClient2 ? new byte[8192] : bArr;
        } else {
            throw new NullPointerException("random == null");
        }
    }

    /* access modifiers changed from: package-private */
    public void writePing(ByteString payload) throws IOException {
        synchronized (this) {
            writeControlFrameSynchronized(9, payload);
        }
    }

    /* access modifiers changed from: package-private */
    public void writePong(ByteString payload) throws IOException {
        synchronized (this) {
            writeControlFrameSynchronized(10, payload);
        }
    }

    /* access modifiers changed from: package-private */
    public void writeClose(int code, ByteString reason) throws IOException {
        ByteString payload = ByteString.EMPTY;
        if (!(code == 0 && reason == null)) {
            if (code != 0) {
                WebSocketProtocol.validateCloseCode(code);
            }
            Buffer buffer2 = new Buffer();
            buffer2.writeShort(code);
            if (reason != null) {
                buffer2.write(reason);
            }
            payload = buffer2.readByteString();
        }
        synchronized (this) {
            try {
                writeControlFrameSynchronized(8, payload);
                this.writerClosed = true;
            } catch (Throwable th) {
                throw th;
            }
        }
    }

    private void writeControlFrameSynchronized(int opcode, ByteString payload) throws IOException {
        if (!Thread.holdsLock(this)) {
            throw new AssertionError();
        } else if (!this.writerClosed) {
            int length = payload.size();
            if (((long) length) <= 125) {
                this.sink.writeByte(opcode | 128);
                int b1 = length;
                if (this.isClient) {
                    this.sink.writeByte(b1 | 128);
                    this.random.nextBytes(this.maskKey);
                    this.sink.write(this.maskKey);
                    byte[] bytes = payload.toByteArray();
                    WebSocketProtocol.toggleMask(bytes, (long) bytes.length, this.maskKey, 0);
                    this.sink.write(bytes);
                } else {
                    this.sink.writeByte(b1);
                    this.sink.write(payload);
                }
                this.sink.flush();
                return;
            }
            throw new IllegalArgumentException("Payload size must be less than or equal to 125");
        } else {
            throw new IOException("closed");
        }
    }

    /* access modifiers changed from: package-private */
    public Sink newMessageSink(int formatOpcode, long contentLength) {
        if (!this.activeWriter) {
            this.activeWriter = true;
            this.frameSink.formatOpcode = formatOpcode;
            this.frameSink.contentLength = contentLength;
            this.frameSink.isFirstFrame = true;
            this.frameSink.closed = false;
            return this.frameSink;
        }
        throw new IllegalStateException("Another message writer is active. Did you call close()?");
    }

    /* access modifiers changed from: package-private */
    public void writeMessageFrameSynchronized(int formatOpcode, long byteCount, boolean isFirstFrame, boolean isFinal) throws IOException {
        long j = byteCount;
        if (!Thread.holdsLock(this)) {
            throw new AssertionError();
        } else if (!this.writerClosed) {
            int b0 = isFirstFrame ? formatOpcode : 0;
            if (isFinal) {
                b0 |= 128;
            }
            this.sink.writeByte(b0);
            int b1 = 0;
            if (this.isClient) {
                b1 = 0 | 128;
            }
            if (j <= 125) {
                this.sink.writeByte(b1 | ((int) j));
            } else if (j <= 65535) {
                this.sink.writeByte(b1 | 126);
                this.sink.writeShort((int) j);
            } else {
                this.sink.writeByte(b1 | WorkQueueKt.MASK);
                this.sink.writeLong(j);
            }
            if (this.isClient) {
                this.random.nextBytes(this.maskKey);
                this.sink.write(this.maskKey);
                long written = 0;
                while (written < j) {
                    int read = this.buffer.read(this.maskBuffer, 0, (int) Math.min(j, (long) this.maskBuffer.length));
                    if (read != -1) {
                        WebSocketProtocol.toggleMask(this.maskBuffer, (long) read, this.maskKey, written);
                        this.sink.write(this.maskBuffer, 0, read);
                        written += (long) read;
                    } else {
                        throw new AssertionError();
                    }
                }
            } else {
                this.sink.write(this.buffer, j);
            }
            this.sink.emit();
        } else {
            throw new IOException("closed");
        }
    }

    final class FrameSink implements Sink {
        boolean closed;
        long contentLength;
        int formatOpcode;
        boolean isFirstFrame;

        FrameSink() {
        }

        public void write(Buffer source, long byteCount) throws IOException {
            if (!this.closed) {
                WebSocketWriter.this.buffer.write(source, byteCount);
                boolean deferWrite = this.isFirstFrame && this.contentLength != -1 && WebSocketWriter.this.buffer.size() > this.contentLength - PlaybackStateCompat.ACTION_PLAY_FROM_URI;
                long emitCount = WebSocketWriter.this.buffer.completeSegmentByteCount();
                if (emitCount > 0 && !deferWrite) {
                    synchronized (WebSocketWriter.this) {
                        WebSocketWriter.this.writeMessageFrameSynchronized(this.formatOpcode, emitCount, this.isFirstFrame, false);
                    }
                    this.isFirstFrame = false;
                    return;
                }
                return;
            }
            throw new IOException("closed");
        }

        public void flush() throws IOException {
            if (!this.closed) {
                synchronized (WebSocketWriter.this) {
                    WebSocketWriter webSocketWriter = WebSocketWriter.this;
                    webSocketWriter.writeMessageFrameSynchronized(this.formatOpcode, webSocketWriter.buffer.size(), this.isFirstFrame, false);
                }
                this.isFirstFrame = false;
                return;
            }
            throw new IOException("closed");
        }

        public Timeout timeout() {
            return WebSocketWriter.this.sink.timeout();
        }

        public void close() throws IOException {
            if (!this.closed) {
                synchronized (WebSocketWriter.this) {
                    WebSocketWriter webSocketWriter = WebSocketWriter.this;
                    webSocketWriter.writeMessageFrameSynchronized(this.formatOpcode, webSocketWriter.buffer.size(), this.isFirstFrame, true);
                }
                this.closed = true;
                WebSocketWriter.this.activeWriter = false;
                return;
            }
            throw new IOException("closed");
        }
    }
}
