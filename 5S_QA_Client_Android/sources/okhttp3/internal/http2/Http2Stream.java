package okhttp3.internal.http2;

import java.io.EOFException;
import java.io.IOException;
import java.io.InterruptedIOException;
import java.net.SocketTimeoutException;
import java.util.ArrayList;
import java.util.List;
import okio.AsyncTimeout;
import okio.Buffer;
import okio.BufferedSource;
import okio.Sink;
import okio.Source;
import okio.Timeout;

public final class Http2Stream {
    static final /* synthetic */ boolean $assertionsDisabled = false;
    long bytesLeftInWriteWindow;
    final Http2Connection connection;
    ErrorCode errorCode = null;
    private boolean hasResponseHeaders;
    final int id;
    final StreamTimeout readTimeout = new StreamTimeout();
    private final List<Header> requestHeaders;
    private List<Header> responseHeaders;
    final FramingSink sink;
    private final FramingSource source;
    long unacknowledgedBytesRead = 0;
    final StreamTimeout writeTimeout = new StreamTimeout();

    Http2Stream(int id2, Http2Connection connection2, boolean outFinished, boolean inFinished, List<Header> requestHeaders2) {
        if (connection2 == null) {
            throw new NullPointerException("connection == null");
        } else if (requestHeaders2 != null) {
            this.id = id2;
            this.connection = connection2;
            this.bytesLeftInWriteWindow = (long) connection2.peerSettings.getInitialWindowSize();
            FramingSource framingSource = new FramingSource((long) connection2.okHttpSettings.getInitialWindowSize());
            this.source = framingSource;
            FramingSink framingSink = new FramingSink();
            this.sink = framingSink;
            framingSource.finished = inFinished;
            framingSink.finished = outFinished;
            this.requestHeaders = requestHeaders2;
        } else {
            throw new NullPointerException("requestHeaders == null");
        }
    }

    public int getId() {
        return this.id;
    }

    public synchronized boolean isOpen() {
        if (this.errorCode != null) {
            return false;
        }
        if ((this.source.finished || this.source.closed) && ((this.sink.finished || this.sink.closed) && this.hasResponseHeaders)) {
            return false;
        }
        return true;
    }

    public boolean isLocallyInitiated() {
        if (this.connection.client == ((this.id & 1) == 1)) {
            return true;
        }
        return false;
    }

    public Http2Connection getConnection() {
        return this.connection;
    }

    public List<Header> getRequestHeaders() {
        return this.requestHeaders;
    }

    /* JADX WARNING: Code restructure failed: missing block: B:22:0x0031, code lost:
        r0 = th;
     */
    /* JADX WARNING: Exception block dominator not found, dom blocks: [] */
    /* Code decompiled incorrectly, please refer to instructions dump. */
    public synchronized java.util.List<okhttp3.internal.http2.Header> takeResponseHeaders() throws java.io.IOException {
        /*
            r3 = this;
            monitor-enter(r3)
            boolean r0 = r3.isLocallyInitiated()     // Catch:{ all -> 0x0040 }
            if (r0 == 0) goto L_0x0038
            okhttp3.internal.http2.Http2Stream$StreamTimeout r0 = r3.readTimeout     // Catch:{ all -> 0x0040 }
            r0.enter()     // Catch:{ all -> 0x0040 }
        L_0x000c:
            java.util.List<okhttp3.internal.http2.Header> r0 = r3.responseHeaders     // Catch:{ all -> 0x0031 }
            if (r0 != 0) goto L_0x001a
            okhttp3.internal.http2.ErrorCode r0 = r3.errorCode     // Catch:{ all -> 0x0018 }
            if (r0 != 0) goto L_0x001a
            r3.waitForIo()     // Catch:{ all -> 0x0018 }
            goto L_0x000c
        L_0x0018:
            r0 = move-exception
            goto L_0x0032
        L_0x001a:
            okhttp3.internal.http2.Http2Stream$StreamTimeout r0 = r3.readTimeout     // Catch:{ all -> 0x0040 }
            r0.exitAndThrowIfTimedOut()     // Catch:{ all -> 0x0040 }
            java.util.List<okhttp3.internal.http2.Header> r0 = r3.responseHeaders     // Catch:{ all -> 0x0040 }
            if (r0 == 0) goto L_0x0029
            r1 = 0
            r3.responseHeaders = r1     // Catch:{ all -> 0x0040 }
            monitor-exit(r3)
            return r0
        L_0x0029:
            okhttp3.internal.http2.StreamResetException r1 = new okhttp3.internal.http2.StreamResetException     // Catch:{ all -> 0x0040 }
            okhttp3.internal.http2.ErrorCode r2 = r3.errorCode     // Catch:{ all -> 0x0040 }
            r1.<init>(r2)     // Catch:{ all -> 0x0040 }
            throw r1     // Catch:{ all -> 0x0040 }
        L_0x0031:
            r0 = move-exception
        L_0x0032:
            okhttp3.internal.http2.Http2Stream$StreamTimeout r1 = r3.readTimeout     // Catch:{ all -> 0x0040 }
            r1.exitAndThrowIfTimedOut()     // Catch:{ all -> 0x0040 }
            throw r0     // Catch:{ all -> 0x0040 }
        L_0x0038:
            java.lang.IllegalStateException r0 = new java.lang.IllegalStateException     // Catch:{ all -> 0x0040 }
            java.lang.String r1 = "servers cannot read response headers"
            r0.<init>(r1)     // Catch:{ all -> 0x0040 }
            throw r0     // Catch:{ all -> 0x0040 }
        L_0x0040:
            r0 = move-exception
            monitor-exit(r3)
            throw r0
        */
        throw new UnsupportedOperationException("Method not decompiled: okhttp3.internal.http2.Http2Stream.takeResponseHeaders():java.util.List");
    }

    public synchronized ErrorCode getErrorCode() {
        return this.errorCode;
    }

    public void sendResponseHeaders(List<Header> responseHeaders2, boolean out) throws IOException {
        if (Thread.holdsLock(this)) {
            throw new AssertionError();
        } else if (responseHeaders2 != null) {
            boolean outFinished = false;
            synchronized (this) {
                this.hasResponseHeaders = true;
                if (!out) {
                    this.sink.finished = true;
                    outFinished = true;
                }
            }
            this.connection.writeSynReply(this.id, outFinished, responseHeaders2);
            if (outFinished) {
                this.connection.flush();
            }
        } else {
            throw new NullPointerException("responseHeaders == null");
        }
    }

    public Timeout readTimeout() {
        return this.readTimeout;
    }

    public Timeout writeTimeout() {
        return this.writeTimeout;
    }

    public Source getSource() {
        return this.source;
    }

    public Sink getSink() {
        synchronized (this) {
            if (!this.hasResponseHeaders) {
                if (!isLocallyInitiated()) {
                    throw new IllegalStateException("reply before requesting the sink");
                }
            }
        }
        return this.sink;
    }

    public void close(ErrorCode rstStatusCode) throws IOException {
        if (closeInternal(rstStatusCode)) {
            this.connection.writeSynReset(this.id, rstStatusCode);
        }
    }

    public void closeLater(ErrorCode errorCode2) {
        if (closeInternal(errorCode2)) {
            this.connection.writeSynResetLater(this.id, errorCode2);
        }
    }

    private boolean closeInternal(ErrorCode errorCode2) {
        if (!Thread.holdsLock(this)) {
            synchronized (this) {
                if (this.errorCode != null) {
                    return false;
                }
                if (this.source.finished && this.sink.finished) {
                    return false;
                }
                this.errorCode = errorCode2;
                notifyAll();
                this.connection.removeStream(this.id);
                return true;
            }
        }
        throw new AssertionError();
    }

    /* access modifiers changed from: package-private */
    public void receiveHeaders(List<Header> headers) {
        if (!Thread.holdsLock(this)) {
            boolean open = true;
            synchronized (this) {
                this.hasResponseHeaders = true;
                if (this.responseHeaders == null) {
                    this.responseHeaders = headers;
                    open = isOpen();
                    notifyAll();
                } else {
                    List<Header> newHeaders = new ArrayList<>();
                    newHeaders.addAll(this.responseHeaders);
                    newHeaders.add((Object) null);
                    newHeaders.addAll(headers);
                    this.responseHeaders = newHeaders;
                }
            }
            if (!open) {
                this.connection.removeStream(this.id);
                return;
            }
            return;
        }
        throw new AssertionError();
    }

    /* access modifiers changed from: package-private */
    public void receiveData(BufferedSource in, int length) throws IOException {
        if (!Thread.holdsLock(this)) {
            this.source.receive(in, (long) length);
            return;
        }
        throw new AssertionError();
    }

    /* access modifiers changed from: package-private */
    public void receiveFin() {
        boolean open;
        if (!Thread.holdsLock(this)) {
            synchronized (this) {
                this.source.finished = true;
                open = isOpen();
                notifyAll();
            }
            if (!open) {
                this.connection.removeStream(this.id);
                return;
            }
            return;
        }
        throw new AssertionError();
    }

    /* access modifiers changed from: package-private */
    public synchronized void receiveRstStream(ErrorCode errorCode2) {
        if (this.errorCode == null) {
            this.errorCode = errorCode2;
            notifyAll();
        }
    }

    private final class FramingSource implements Source {
        static final /* synthetic */ boolean $assertionsDisabled = false;
        boolean closed;
        boolean finished;
        private final long maxByteCount;
        private final Buffer readBuffer = new Buffer();
        private final Buffer receiveBuffer = new Buffer();

        static {
            Class<Http2Stream> cls = Http2Stream.class;
        }

        FramingSource(long maxByteCount2) {
            this.maxByteCount = maxByteCount2;
        }

        /* JADX WARNING: Code restructure failed: missing block: B:14:0x005b, code lost:
            r5 = r10.this$0.connection;
         */
        /* JADX WARNING: Code restructure failed: missing block: B:15:0x005f, code lost:
            monitor-enter(r5);
         */
        /* JADX WARNING: Code restructure failed: missing block: B:17:?, code lost:
            r10.this$0.connection.unacknowledgedBytesRead += r3;
         */
        /* JADX WARNING: Code restructure failed: missing block: B:18:0x007e, code lost:
            if (r10.this$0.connection.unacknowledgedBytesRead < ((long) (r10.this$0.connection.okHttpSettings.getInitialWindowSize() / 2))) goto L_0x0094;
         */
        /* JADX WARNING: Code restructure failed: missing block: B:19:0x0080, code lost:
            r10.this$0.connection.writeWindowUpdateLater(0, r10.this$0.connection.unacknowledgedBytesRead);
            r10.this$0.connection.unacknowledgedBytesRead = 0;
         */
        /* JADX WARNING: Code restructure failed: missing block: B:20:0x0094, code lost:
            monitor-exit(r5);
         */
        /* JADX WARNING: Code restructure failed: missing block: B:21:0x0095, code lost:
            return r3;
         */
        /* Code decompiled incorrectly, please refer to instructions dump. */
        public long read(okio.Buffer r11, long r12) throws java.io.IOException {
            /*
                r10 = this;
                r0 = 0
                int r2 = (r12 > r0 ? 1 : (r12 == r0 ? 0 : -1))
                if (r2 < 0) goto L_0x009c
                okhttp3.internal.http2.Http2Stream r2 = okhttp3.internal.http2.Http2Stream.this
                monitor-enter(r2)
                r10.waitUntilReadable()     // Catch:{ all -> 0x0099 }
                r10.checkNotClosed()     // Catch:{ all -> 0x0099 }
                okio.Buffer r3 = r10.readBuffer     // Catch:{ all -> 0x0099 }
                long r3 = r3.size()     // Catch:{ all -> 0x0099 }
                int r5 = (r3 > r0 ? 1 : (r3 == r0 ? 0 : -1))
                if (r5 != 0) goto L_0x001d
                r0 = -1
                monitor-exit(r2)     // Catch:{ all -> 0x0099 }
                return r0
            L_0x001d:
                okio.Buffer r3 = r10.readBuffer     // Catch:{ all -> 0x0099 }
                long r4 = r3.size()     // Catch:{ all -> 0x0099 }
                long r4 = java.lang.Math.min(r12, r4)     // Catch:{ all -> 0x0099 }
                long r3 = r3.read(r11, r4)     // Catch:{ all -> 0x0099 }
                okhttp3.internal.http2.Http2Stream r5 = okhttp3.internal.http2.Http2Stream.this     // Catch:{ all -> 0x0099 }
                long r6 = r5.unacknowledgedBytesRead     // Catch:{ all -> 0x0099 }
                long r6 = r6 + r3
                r5.unacknowledgedBytesRead = r6     // Catch:{ all -> 0x0099 }
                okhttp3.internal.http2.Http2Stream r5 = okhttp3.internal.http2.Http2Stream.this     // Catch:{ all -> 0x0099 }
                long r5 = r5.unacknowledgedBytesRead     // Catch:{ all -> 0x0099 }
                okhttp3.internal.http2.Http2Stream r7 = okhttp3.internal.http2.Http2Stream.this     // Catch:{ all -> 0x0099 }
                okhttp3.internal.http2.Http2Connection r7 = r7.connection     // Catch:{ all -> 0x0099 }
                okhttp3.internal.http2.Settings r7 = r7.okHttpSettings     // Catch:{ all -> 0x0099 }
                int r7 = r7.getInitialWindowSize()     // Catch:{ all -> 0x0099 }
                int r7 = r7 / 2
                long r7 = (long) r7     // Catch:{ all -> 0x0099 }
                int r9 = (r5 > r7 ? 1 : (r5 == r7 ? 0 : -1))
                if (r9 < 0) goto L_0x005a
                okhttp3.internal.http2.Http2Stream r5 = okhttp3.internal.http2.Http2Stream.this     // Catch:{ all -> 0x0099 }
                okhttp3.internal.http2.Http2Connection r5 = r5.connection     // Catch:{ all -> 0x0099 }
                okhttp3.internal.http2.Http2Stream r6 = okhttp3.internal.http2.Http2Stream.this     // Catch:{ all -> 0x0099 }
                int r6 = r6.id     // Catch:{ all -> 0x0099 }
                okhttp3.internal.http2.Http2Stream r7 = okhttp3.internal.http2.Http2Stream.this     // Catch:{ all -> 0x0099 }
                long r7 = r7.unacknowledgedBytesRead     // Catch:{ all -> 0x0099 }
                r5.writeWindowUpdateLater(r6, r7)     // Catch:{ all -> 0x0099 }
                okhttp3.internal.http2.Http2Stream r5 = okhttp3.internal.http2.Http2Stream.this     // Catch:{ all -> 0x0099 }
                r5.unacknowledgedBytesRead = r0     // Catch:{ all -> 0x0099 }
            L_0x005a:
                monitor-exit(r2)     // Catch:{ all -> 0x0099 }
                okhttp3.internal.http2.Http2Stream r2 = okhttp3.internal.http2.Http2Stream.this
                okhttp3.internal.http2.Http2Connection r5 = r2.connection
                monitor-enter(r5)
                okhttp3.internal.http2.Http2Stream r2 = okhttp3.internal.http2.Http2Stream.this     // Catch:{ all -> 0x0096 }
                okhttp3.internal.http2.Http2Connection r2 = r2.connection     // Catch:{ all -> 0x0096 }
                long r6 = r2.unacknowledgedBytesRead     // Catch:{ all -> 0x0096 }
                long r6 = r6 + r3
                r2.unacknowledgedBytesRead = r6     // Catch:{ all -> 0x0096 }
                okhttp3.internal.http2.Http2Stream r2 = okhttp3.internal.http2.Http2Stream.this     // Catch:{ all -> 0x0096 }
                okhttp3.internal.http2.Http2Connection r2 = r2.connection     // Catch:{ all -> 0x0096 }
                long r6 = r2.unacknowledgedBytesRead     // Catch:{ all -> 0x0096 }
                okhttp3.internal.http2.Http2Stream r2 = okhttp3.internal.http2.Http2Stream.this     // Catch:{ all -> 0x0096 }
                okhttp3.internal.http2.Http2Connection r2 = r2.connection     // Catch:{ all -> 0x0096 }
                okhttp3.internal.http2.Settings r2 = r2.okHttpSettings     // Catch:{ all -> 0x0096 }
                int r2 = r2.getInitialWindowSize()     // Catch:{ all -> 0x0096 }
                int r2 = r2 / 2
                long r8 = (long) r2     // Catch:{ all -> 0x0096 }
                int r2 = (r6 > r8 ? 1 : (r6 == r8 ? 0 : -1))
                if (r2 < 0) goto L_0x0094
                okhttp3.internal.http2.Http2Stream r2 = okhttp3.internal.http2.Http2Stream.this     // Catch:{ all -> 0x0096 }
                okhttp3.internal.http2.Http2Connection r2 = r2.connection     // Catch:{ all -> 0x0096 }
                r6 = 0
                okhttp3.internal.http2.Http2Stream r7 = okhttp3.internal.http2.Http2Stream.this     // Catch:{ all -> 0x0096 }
                okhttp3.internal.http2.Http2Connection r7 = r7.connection     // Catch:{ all -> 0x0096 }
                long r7 = r7.unacknowledgedBytesRead     // Catch:{ all -> 0x0096 }
                r2.writeWindowUpdateLater(r6, r7)     // Catch:{ all -> 0x0096 }
                okhttp3.internal.http2.Http2Stream r2 = okhttp3.internal.http2.Http2Stream.this     // Catch:{ all -> 0x0096 }
                okhttp3.internal.http2.Http2Connection r2 = r2.connection     // Catch:{ all -> 0x0096 }
                r2.unacknowledgedBytesRead = r0     // Catch:{ all -> 0x0096 }
            L_0x0094:
                monitor-exit(r5)     // Catch:{ all -> 0x0096 }
                return r3
            L_0x0096:
                r0 = move-exception
                monitor-exit(r5)     // Catch:{ all -> 0x0096 }
                throw r0
            L_0x0099:
                r0 = move-exception
                monitor-exit(r2)     // Catch:{ all -> 0x0099 }
                throw r0
            L_0x009c:
                java.lang.IllegalArgumentException r0 = new java.lang.IllegalArgumentException
                java.lang.StringBuilder r1 = new java.lang.StringBuilder
                r1.<init>()
                java.lang.String r2 = "byteCount < 0: "
                java.lang.StringBuilder r1 = r1.append(r2)
                java.lang.StringBuilder r1 = r1.append(r12)
                java.lang.String r1 = r1.toString()
                r0.<init>(r1)
                throw r0
            */
            throw new UnsupportedOperationException("Method not decompiled: okhttp3.internal.http2.Http2Stream.FramingSource.read(okio.Buffer, long):long");
        }

        private void waitUntilReadable() throws IOException {
            Http2Stream.this.readTimeout.enter();
            while (this.readBuffer.size() == 0 && !this.finished && !this.closed && Http2Stream.this.errorCode == null) {
                try {
                    Http2Stream.this.waitForIo();
                } finally {
                    Http2Stream.this.readTimeout.exitAndThrowIfTimedOut();
                }
            }
        }

        /* access modifiers changed from: package-private */
        public void receive(BufferedSource in, long byteCount) throws IOException {
            boolean finished2;
            boolean z;
            boolean flowControlError;
            if (!Thread.holdsLock(Http2Stream.this)) {
                while (byteCount > 0) {
                    synchronized (Http2Stream.this) {
                        finished2 = this.finished;
                        z = true;
                        flowControlError = this.readBuffer.size() + byteCount > this.maxByteCount;
                    }
                    if (flowControlError) {
                        in.skip(byteCount);
                        Http2Stream.this.closeLater(ErrorCode.FLOW_CONTROL_ERROR);
                        return;
                    } else if (finished2) {
                        in.skip(byteCount);
                        return;
                    } else {
                        long read = in.read(this.receiveBuffer, byteCount);
                        if (read != -1) {
                            long byteCount2 = byteCount - read;
                            synchronized (Http2Stream.this) {
                                if (this.readBuffer.size() != 0) {
                                    z = false;
                                }
                                boolean wasEmpty = z;
                                this.readBuffer.writeAll(this.receiveBuffer);
                                if (wasEmpty) {
                                    Http2Stream.this.notifyAll();
                                }
                            }
                            byteCount = byteCount2;
                        } else {
                            throw new EOFException();
                        }
                    }
                }
                return;
            }
            throw new AssertionError();
        }

        public Timeout timeout() {
            return Http2Stream.this.readTimeout;
        }

        public void close() throws IOException {
            synchronized (Http2Stream.this) {
                this.closed = true;
                this.readBuffer.clear();
                Http2Stream.this.notifyAll();
            }
            Http2Stream.this.cancelStreamIfNecessary();
        }

        private void checkNotClosed() throws IOException {
            if (this.closed) {
                throw new IOException("stream closed");
            } else if (Http2Stream.this.errorCode != null) {
                throw new StreamResetException(Http2Stream.this.errorCode);
            }
        }
    }

    /* access modifiers changed from: package-private */
    public void cancelStreamIfNecessary() throws IOException {
        boolean cancel;
        boolean open;
        if (!Thread.holdsLock(this)) {
            synchronized (this) {
                cancel = !this.source.finished && this.source.closed && (this.sink.finished || this.sink.closed);
                open = isOpen();
            }
            if (cancel) {
                close(ErrorCode.CANCEL);
            } else if (!open) {
                this.connection.removeStream(this.id);
            }
        } else {
            throw new AssertionError();
        }
    }

    final class FramingSink implements Sink {
        static final /* synthetic */ boolean $assertionsDisabled = false;
        private static final long EMIT_BUFFER_SIZE = 16384;
        boolean closed;
        boolean finished;
        private final Buffer sendBuffer = new Buffer();

        static {
            Class<Http2Stream> cls = Http2Stream.class;
        }

        FramingSink() {
        }

        public void write(Buffer source, long byteCount) throws IOException {
            if (!Thread.holdsLock(Http2Stream.this)) {
                this.sendBuffer.write(source, byteCount);
                while (this.sendBuffer.size() >= 16384) {
                    emitFrame(false);
                }
                return;
            }
            throw new AssertionError();
        }

        /* JADX INFO: finally extract failed */
        private void emitFrame(boolean outFinished) throws IOException {
            long toWrite;
            synchronized (Http2Stream.this) {
                Http2Stream.this.writeTimeout.enter();
                while (Http2Stream.this.bytesLeftInWriteWindow <= 0 && !this.finished && !this.closed && Http2Stream.this.errorCode == null) {
                    try {
                        Http2Stream.this.waitForIo();
                    } catch (Throwable th) {
                        Http2Stream.this.writeTimeout.exitAndThrowIfTimedOut();
                        throw th;
                    }
                }
                Http2Stream.this.writeTimeout.exitAndThrowIfTimedOut();
                Http2Stream.this.checkOutNotClosed();
                toWrite = Math.min(Http2Stream.this.bytesLeftInWriteWindow, this.sendBuffer.size());
                Http2Stream.this.bytesLeftInWriteWindow -= toWrite;
            }
            Http2Stream.this.writeTimeout.enter();
            try {
                Http2Stream.this.connection.writeData(Http2Stream.this.id, outFinished && toWrite == this.sendBuffer.size(), this.sendBuffer, toWrite);
            } finally {
                Http2Stream.this.writeTimeout.exitAndThrowIfTimedOut();
            }
        }

        public void flush() throws IOException {
            if (!Thread.holdsLock(Http2Stream.this)) {
                synchronized (Http2Stream.this) {
                    Http2Stream.this.checkOutNotClosed();
                }
                while (this.sendBuffer.size() > 0) {
                    emitFrame(false);
                    Http2Stream.this.connection.flush();
                }
                return;
            }
            throw new AssertionError();
        }

        public Timeout timeout() {
            return Http2Stream.this.writeTimeout;
        }

        /* JADX WARNING: Code restructure failed: missing block: B:11:0x0019, code lost:
            if (r8.this$0.sink.finished != false) goto L_0x0044;
         */
        /* JADX WARNING: Code restructure failed: missing block: B:13:0x0025, code lost:
            if (r8.sendBuffer.size() <= 0) goto L_0x0035;
         */
        /* JADX WARNING: Code restructure failed: missing block: B:15:0x002f, code lost:
            if (r8.sendBuffer.size() <= 0) goto L_0x0044;
         */
        /* JADX WARNING: Code restructure failed: missing block: B:16:0x0031, code lost:
            emitFrame(true);
         */
        /* JADX WARNING: Code restructure failed: missing block: B:17:0x0035, code lost:
            r8.this$0.connection.writeData(r8.this$0.id, true, (okio.Buffer) null, 0);
         */
        /* JADX WARNING: Code restructure failed: missing block: B:18:0x0044, code lost:
            r2 = r8.this$0;
         */
        /* JADX WARNING: Code restructure failed: missing block: B:19:0x0046, code lost:
            monitor-enter(r2);
         */
        /* JADX WARNING: Code restructure failed: missing block: B:21:?, code lost:
            r8.closed = true;
         */
        /* JADX WARNING: Code restructure failed: missing block: B:22:0x0049, code lost:
            monitor-exit(r2);
         */
        /* JADX WARNING: Code restructure failed: missing block: B:23:0x004a, code lost:
            r8.this$0.connection.flush();
            r8.this$0.cancelStreamIfNecessary();
         */
        /* JADX WARNING: Code restructure failed: missing block: B:24:0x0056, code lost:
            return;
         */
        /* Code decompiled incorrectly, please refer to instructions dump. */
        public void close() throws java.io.IOException {
            /*
                r8 = this;
                okhttp3.internal.http2.Http2Stream r0 = okhttp3.internal.http2.Http2Stream.this
                boolean r0 = java.lang.Thread.holdsLock(r0)
                if (r0 != 0) goto L_0x005d
                okhttp3.internal.http2.Http2Stream r0 = okhttp3.internal.http2.Http2Stream.this
                monitor-enter(r0)
                boolean r1 = r8.closed     // Catch:{ all -> 0x005a }
                if (r1 == 0) goto L_0x0011
                monitor-exit(r0)     // Catch:{ all -> 0x005a }
                return
            L_0x0011:
                monitor-exit(r0)     // Catch:{ all -> 0x005a }
                okhttp3.internal.http2.Http2Stream r0 = okhttp3.internal.http2.Http2Stream.this
                okhttp3.internal.http2.Http2Stream$FramingSink r0 = r0.sink
                boolean r0 = r0.finished
                r1 = 1
                if (r0 != 0) goto L_0x0044
                okio.Buffer r0 = r8.sendBuffer
                long r2 = r0.size()
                r4 = 0
                int r0 = (r2 > r4 ? 1 : (r2 == r4 ? 0 : -1))
                if (r0 <= 0) goto L_0x0035
            L_0x0027:
                okio.Buffer r0 = r8.sendBuffer
                long r2 = r0.size()
                int r0 = (r2 > r4 ? 1 : (r2 == r4 ? 0 : -1))
                if (r0 <= 0) goto L_0x0044
                r8.emitFrame(r1)
                goto L_0x0027
            L_0x0035:
                okhttp3.internal.http2.Http2Stream r0 = okhttp3.internal.http2.Http2Stream.this
                okhttp3.internal.http2.Http2Connection r2 = r0.connection
                okhttp3.internal.http2.Http2Stream r0 = okhttp3.internal.http2.Http2Stream.this
                int r3 = r0.id
                r4 = 1
                r5 = 0
                r6 = 0
                r2.writeData(r3, r4, r5, r6)
            L_0x0044:
                okhttp3.internal.http2.Http2Stream r2 = okhttp3.internal.http2.Http2Stream.this
                monitor-enter(r2)
                r8.closed = r1     // Catch:{ all -> 0x0057 }
                monitor-exit(r2)     // Catch:{ all -> 0x0057 }
                okhttp3.internal.http2.Http2Stream r0 = okhttp3.internal.http2.Http2Stream.this
                okhttp3.internal.http2.Http2Connection r0 = r0.connection
                r0.flush()
                okhttp3.internal.http2.Http2Stream r0 = okhttp3.internal.http2.Http2Stream.this
                r0.cancelStreamIfNecessary()
                return
            L_0x0057:
                r0 = move-exception
                monitor-exit(r2)     // Catch:{ all -> 0x0057 }
                throw r0
            L_0x005a:
                r1 = move-exception
                monitor-exit(r0)     // Catch:{ all -> 0x005a }
                throw r1
            L_0x005d:
                java.lang.AssertionError r0 = new java.lang.AssertionError
                r0.<init>()
                throw r0
            */
            throw new UnsupportedOperationException("Method not decompiled: okhttp3.internal.http2.Http2Stream.FramingSink.close():void");
        }
    }

    /* access modifiers changed from: package-private */
    public void addBytesToWriteWindow(long delta) {
        this.bytesLeftInWriteWindow += delta;
        if (delta > 0) {
            notifyAll();
        }
    }

    /* access modifiers changed from: package-private */
    public void checkOutNotClosed() throws IOException {
        if (this.sink.closed) {
            throw new IOException("stream closed");
        } else if (this.sink.finished) {
            throw new IOException("stream finished");
        } else if (this.errorCode != null) {
            throw new StreamResetException(this.errorCode);
        }
    }

    /* access modifiers changed from: package-private */
    public void waitForIo() throws InterruptedIOException {
        try {
            wait();
        } catch (InterruptedException e) {
            throw new InterruptedIOException();
        }
    }

    class StreamTimeout extends AsyncTimeout {
        StreamTimeout() {
        }

        /* access modifiers changed from: protected */
        public void timedOut() {
            Http2Stream.this.closeLater(ErrorCode.CANCEL);
        }

        /* access modifiers changed from: protected */
        public IOException newTimeoutException(IOException cause) {
            SocketTimeoutException socketTimeoutException = new SocketTimeoutException("timeout");
            if (cause != null) {
                socketTimeoutException.initCause(cause);
            }
            return socketTimeoutException;
        }

        public void exitAndThrowIfTimedOut() throws IOException {
            if (exit()) {
                throw newTimeoutException((IOException) null);
            }
        }
    }
}
