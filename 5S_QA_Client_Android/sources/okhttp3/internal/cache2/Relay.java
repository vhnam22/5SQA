package okhttp3.internal.cache2;

import java.io.Closeable;
import java.io.File;
import java.io.IOException;
import java.io.RandomAccessFile;
import okhttp3.internal.Util;
import okio.Buffer;
import okio.ByteString;
import okio.Source;
import okio.Timeout;

final class Relay {
    private static final long FILE_HEADER_SIZE = 32;
    static final ByteString PREFIX_CLEAN = ByteString.encodeUtf8("OkHttp cache v1\n");
    static final ByteString PREFIX_DIRTY = ByteString.encodeUtf8("OkHttp DIRTY :(\n");
    private static final int SOURCE_FILE = 2;
    private static final int SOURCE_UPSTREAM = 1;
    final Buffer buffer = new Buffer();
    final long bufferMaxSize;
    boolean complete;
    RandomAccessFile file;
    private final ByteString metadata;
    int sourceCount;
    Source upstream;
    final Buffer upstreamBuffer = new Buffer();
    long upstreamPos;
    Thread upstreamReader;

    private Relay(RandomAccessFile file2, Source upstream2, long upstreamPos2, ByteString metadata2, long bufferMaxSize2) {
        this.file = file2;
        this.upstream = upstream2;
        this.complete = upstream2 == null;
        this.upstreamPos = upstreamPos2;
        this.metadata = metadata2;
        this.bufferMaxSize = bufferMaxSize2;
    }

    public static Relay edit(File file2, Source upstream2, ByteString metadata2, long bufferMaxSize2) throws IOException {
        File file3 = file2;
        RandomAccessFile randomAccessFile = new RandomAccessFile(file2, "rw");
        Relay relay = new Relay(randomAccessFile, upstream2, 0, metadata2, bufferMaxSize2);
        randomAccessFile.setLength(0);
        relay.writeHeader(PREFIX_DIRTY, -1, -1);
        return relay;
    }

    public static Relay read(File file2) throws IOException {
        RandomAccessFile randomAccessFile = new RandomAccessFile(file2, "rw");
        FileOperator fileOperator = new FileOperator(randomAccessFile.getChannel());
        Buffer header = new Buffer();
        fileOperator.read(0, header, 32);
        ByteString byteString = PREFIX_CLEAN;
        if (header.readByteString((long) byteString.size()).equals(byteString)) {
            long upstreamSize = header.readLong();
            long metadataSize = header.readLong();
            Buffer metadataBuffer = new Buffer();
            fileOperator.read(upstreamSize + 32, metadataBuffer, metadataSize);
            return new Relay(randomAccessFile, (Source) null, upstreamSize, metadataBuffer.readByteString(), 0);
        }
        throw new IOException("unreadable cache file");
    }

    private void writeHeader(ByteString prefix, long upstreamSize, long metadataSize) throws IOException {
        Buffer header = new Buffer();
        header.write(prefix);
        header.writeLong(upstreamSize);
        header.writeLong(metadataSize);
        if (header.size() == 32) {
            new FileOperator(this.file.getChannel()).write(0, header, 32);
            return;
        }
        throw new IllegalArgumentException();
    }

    private void writeMetadata(long upstreamSize) throws IOException {
        Buffer metadataBuffer = new Buffer();
        metadataBuffer.write(this.metadata);
        new FileOperator(this.file.getChannel()).write(32 + upstreamSize, metadataBuffer, (long) this.metadata.size());
    }

    /* access modifiers changed from: package-private */
    public void commit(long upstreamSize) throws IOException {
        writeMetadata(upstreamSize);
        this.file.getChannel().force(false);
        writeHeader(PREFIX_CLEAN, upstreamSize, (long) this.metadata.size());
        this.file.getChannel().force(false);
        synchronized (this) {
            this.complete = true;
        }
        Util.closeQuietly((Closeable) this.upstream);
        this.upstream = null;
    }

    /* access modifiers changed from: package-private */
    public boolean isClosed() {
        return this.file == null;
    }

    public ByteString metadata() {
        return this.metadata;
    }

    public Source newSource() {
        synchronized (this) {
            if (this.file == null) {
                return null;
            }
            this.sourceCount++;
            return new RelaySource();
        }
    }

    class RelaySource implements Source {
        private FileOperator fileOperator;
        private long sourcePos;
        private final Timeout timeout = new Timeout();

        RelaySource() {
            this.fileOperator = new FileOperator(Relay.this.file.getChannel());
        }

        /*  JADX ERROR: IndexOutOfBoundsException in pass: RegionMakerVisitor
            java.lang.IndexOutOfBoundsException: Index: 0, Size: 0
            	at java.util.ArrayList.rangeCheck(ArrayList.java:659)
            	at java.util.ArrayList.get(ArrayList.java:435)
            	at jadx.core.dex.nodes.InsnNode.getArg(InsnNode.java:101)
            	at jadx.core.dex.visitors.regions.RegionMaker.traverseMonitorExits(RegionMaker.java:611)
            	at jadx.core.dex.visitors.regions.RegionMaker.traverseMonitorExits(RegionMaker.java:619)
            	at jadx.core.dex.visitors.regions.RegionMaker.traverseMonitorExits(RegionMaker.java:619)
            	at jadx.core.dex.visitors.regions.RegionMaker.traverseMonitorExits(RegionMaker.java:619)
            	at jadx.core.dex.visitors.regions.RegionMaker.processMonitorEnter(RegionMaker.java:561)
            	at jadx.core.dex.visitors.regions.RegionMaker.traverse(RegionMaker.java:133)
            	at jadx.core.dex.visitors.regions.RegionMaker.makeRegion(RegionMaker.java:86)
            	at jadx.core.dex.visitors.regions.RegionMaker.processExcHandler(RegionMaker.java:1043)
            	at jadx.core.dex.visitors.regions.RegionMaker.processTryCatchBlocks(RegionMaker.java:975)
            	at jadx.core.dex.visitors.regions.RegionMakerVisitor.visit(RegionMakerVisitor.java:52)
            */
        public long read(okio.Buffer r22, long r23) throws java.io.IOException {
            /*
                r21 = this;
                r1 = r21
                r2 = r23
                okhttp3.internal.cache2.FileOperator r0 = r1.fileOperator
                if (r0 == 0) goto L_0x014f
                okhttp3.internal.cache2.Relay r4 = okhttp3.internal.cache2.Relay.this
                monitor-enter(r4)
            L_0x000b:
                long r5 = r1.sourcePos     // Catch:{ all -> 0x014c }
                okhttp3.internal.cache2.Relay r0 = okhttp3.internal.cache2.Relay.this     // Catch:{ all -> 0x014c }
                long r7 = r0.upstreamPos     // Catch:{ all -> 0x014c }
                r9 = r7
                r11 = -1
                int r0 = (r5 > r7 ? 1 : (r5 == r7 ? 0 : -1))
                if (r0 != 0) goto L_0x003a
                okhttp3.internal.cache2.Relay r0 = okhttp3.internal.cache2.Relay.this     // Catch:{ all -> 0x014c }
                boolean r0 = r0.complete     // Catch:{ all -> 0x014c }
                if (r0 == 0) goto L_0x0020
                monitor-exit(r4)     // Catch:{ all -> 0x014c }
                return r11
            L_0x0020:
                okhttp3.internal.cache2.Relay r0 = okhttp3.internal.cache2.Relay.this     // Catch:{ all -> 0x014c }
                java.lang.Thread r0 = r0.upstreamReader     // Catch:{ all -> 0x014c }
                if (r0 == 0) goto L_0x002e
                okio.Timeout r0 = r1.timeout     // Catch:{ all -> 0x014c }
                okhttp3.internal.cache2.Relay r5 = okhttp3.internal.cache2.Relay.this     // Catch:{ all -> 0x014c }
                r0.waitUntilNotified(r5)     // Catch:{ all -> 0x014c }
                goto L_0x000b
            L_0x002e:
                okhttp3.internal.cache2.Relay r0 = okhttp3.internal.cache2.Relay.this     // Catch:{ all -> 0x014c }
                java.lang.Thread r5 = java.lang.Thread.currentThread()     // Catch:{ all -> 0x014c }
                r0.upstreamReader = r5     // Catch:{ all -> 0x014c }
                r0 = 1
                monitor-exit(r4)     // Catch:{ all -> 0x014c }
                r5 = r0
                goto L_0x004d
            L_0x003a:
                okhttp3.internal.cache2.Relay r0 = okhttp3.internal.cache2.Relay.this     // Catch:{ all -> 0x014c }
                okio.Buffer r0 = r0.buffer     // Catch:{ all -> 0x014c }
                long r5 = r0.size()     // Catch:{ all -> 0x014c }
                long r5 = r9 - r5
                long r7 = r1.sourcePos     // Catch:{ all -> 0x014c }
                int r0 = (r7 > r5 ? 1 : (r7 == r5 ? 0 : -1))
                if (r0 >= 0) goto L_0x0131
                r0 = 2
                monitor-exit(r4)     // Catch:{ all -> 0x014c }
                r5 = r0
            L_0x004d:
                r0 = 2
                r6 = 32
                if (r5 != r0) goto L_0x006c
                long r11 = r1.sourcePos
                long r11 = r9 - r11
                long r11 = java.lang.Math.min(r2, r11)
                okhttp3.internal.cache2.FileOperator r13 = r1.fileOperator
                long r14 = r1.sourcePos
                long r14 = r14 + r6
                r16 = r22
                r17 = r11
                r13.read(r14, r16, r17)
                long r6 = r1.sourcePos
                long r6 = r6 + r11
                r1.sourcePos = r6
                return r11
            L_0x006c:
                r4 = 0
                okhttp3.internal.cache2.Relay r0 = okhttp3.internal.cache2.Relay.this     // Catch:{ all -> 0x011d }
                okio.Source r0 = r0.upstream     // Catch:{ all -> 0x011d }
                okhttp3.internal.cache2.Relay r8 = okhttp3.internal.cache2.Relay.this     // Catch:{ all -> 0x011d }
                okio.Buffer r8 = r8.upstreamBuffer     // Catch:{ all -> 0x011d }
                okhttp3.internal.cache2.Relay r13 = okhttp3.internal.cache2.Relay.this     // Catch:{ all -> 0x011d }
                long r13 = r13.bufferMaxSize     // Catch:{ all -> 0x011d }
                long r13 = r0.read(r8, r13)     // Catch:{ all -> 0x011d }
                int r0 = (r13 > r11 ? 1 : (r13 == r11 ? 0 : -1))
                if (r0 != 0) goto L_0x009c
                okhttp3.internal.cache2.Relay r0 = okhttp3.internal.cache2.Relay.this     // Catch:{ all -> 0x0098 }
                r0.commit(r9)     // Catch:{ all -> 0x0098 }
                okhttp3.internal.cache2.Relay r6 = okhttp3.internal.cache2.Relay.this
                monitor-enter(r6)
                okhttp3.internal.cache2.Relay r0 = okhttp3.internal.cache2.Relay.this     // Catch:{ all -> 0x0095 }
                r0.upstreamReader = r4     // Catch:{ all -> 0x0095 }
                okhttp3.internal.cache2.Relay r0 = okhttp3.internal.cache2.Relay.this     // Catch:{ all -> 0x0095 }
                r0.notifyAll()     // Catch:{ all -> 0x0095 }
                monitor-exit(r6)     // Catch:{ all -> 0x0095 }
                return r11
            L_0x0095:
                r0 = move-exception
                monitor-exit(r6)     // Catch:{ all -> 0x0095 }
                throw r0
            L_0x0098:
                r0 = move-exception
                r15 = r5
                goto L_0x011f
            L_0x009c:
                long r11 = java.lang.Math.min(r13, r2)     // Catch:{ all -> 0x011d }
                okhttp3.internal.cache2.Relay r0 = okhttp3.internal.cache2.Relay.this     // Catch:{ all -> 0x011d }
                okio.Buffer r15 = r0.upstreamBuffer     // Catch:{ all -> 0x011d }
                r17 = 0
                r16 = r22
                r19 = r11
                r15.copyTo((okio.Buffer) r16, (long) r17, (long) r19)     // Catch:{ all -> 0x011d }
                r8 = r5
                long r4 = r1.sourcePos     // Catch:{ all -> 0x011a }
                long r4 = r4 + r11
                r1.sourcePos = r4     // Catch:{ all -> 0x011a }
                okhttp3.internal.cache2.FileOperator r15 = r1.fileOperator     // Catch:{ all -> 0x011a }
                long r16 = r9 + r6
                okhttp3.internal.cache2.Relay r0 = okhttp3.internal.cache2.Relay.this     // Catch:{ all -> 0x011a }
                okio.Buffer r0 = r0.upstreamBuffer     // Catch:{ all -> 0x011a }
                okio.Buffer r18 = r0.clone()     // Catch:{ all -> 0x011a }
                r19 = r13
                r15.write(r16, r18, r19)     // Catch:{ all -> 0x011a }
                okhttp3.internal.cache2.Relay r4 = okhttp3.internal.cache2.Relay.this     // Catch:{ all -> 0x011a }
                monitor-enter(r4)     // Catch:{ all -> 0x011a }
                okhttp3.internal.cache2.Relay r0 = okhttp3.internal.cache2.Relay.this     // Catch:{ all -> 0x0112 }
                okio.Buffer r0 = r0.buffer     // Catch:{ all -> 0x0112 }
                okhttp3.internal.cache2.Relay r5 = okhttp3.internal.cache2.Relay.this     // Catch:{ all -> 0x0112 }
                okio.Buffer r5 = r5.upstreamBuffer     // Catch:{ all -> 0x0112 }
                r0.write((okio.Buffer) r5, (long) r13)     // Catch:{ all -> 0x0112 }
                okhttp3.internal.cache2.Relay r0 = okhttp3.internal.cache2.Relay.this     // Catch:{ all -> 0x0112 }
                okio.Buffer r0 = r0.buffer     // Catch:{ all -> 0x0112 }
                long r5 = r0.size()     // Catch:{ all -> 0x0112 }
                okhttp3.internal.cache2.Relay r0 = okhttp3.internal.cache2.Relay.this     // Catch:{ all -> 0x0112 }
                r15 = r8
                long r7 = r0.bufferMaxSize     // Catch:{ all -> 0x0118 }
                int r0 = (r5 > r7 ? 1 : (r5 == r7 ? 0 : -1))
                if (r0 <= 0) goto L_0x00f7
                okhttp3.internal.cache2.Relay r0 = okhttp3.internal.cache2.Relay.this     // Catch:{ all -> 0x0118 }
                okio.Buffer r0 = r0.buffer     // Catch:{ all -> 0x0118 }
                okhttp3.internal.cache2.Relay r5 = okhttp3.internal.cache2.Relay.this     // Catch:{ all -> 0x0118 }
                okio.Buffer r5 = r5.buffer     // Catch:{ all -> 0x0118 }
                long r5 = r5.size()     // Catch:{ all -> 0x0118 }
                okhttp3.internal.cache2.Relay r7 = okhttp3.internal.cache2.Relay.this     // Catch:{ all -> 0x0118 }
                long r7 = r7.bufferMaxSize     // Catch:{ all -> 0x0118 }
                long r5 = r5 - r7
                r0.skip(r5)     // Catch:{ all -> 0x0118 }
            L_0x00f7:
                okhttp3.internal.cache2.Relay r0 = okhttp3.internal.cache2.Relay.this     // Catch:{ all -> 0x0118 }
                long r5 = r0.upstreamPos     // Catch:{ all -> 0x0118 }
                long r5 = r5 + r13
                r0.upstreamPos = r5     // Catch:{ all -> 0x0118 }
                monitor-exit(r4)     // Catch:{ all -> 0x0118 }
                okhttp3.internal.cache2.Relay r5 = okhttp3.internal.cache2.Relay.this
                monitor-enter(r5)
                okhttp3.internal.cache2.Relay r0 = okhttp3.internal.cache2.Relay.this     // Catch:{ all -> 0x010f }
                r4 = 0
                r0.upstreamReader = r4     // Catch:{ all -> 0x010f }
                okhttp3.internal.cache2.Relay r0 = okhttp3.internal.cache2.Relay.this     // Catch:{ all -> 0x010f }
                r0.notifyAll()     // Catch:{ all -> 0x010f }
                monitor-exit(r5)     // Catch:{ all -> 0x010f }
                return r11
            L_0x010f:
                r0 = move-exception
                monitor-exit(r5)     // Catch:{ all -> 0x010f }
                throw r0
            L_0x0112:
                r0 = move-exception
                r15 = r8
            L_0x0114:
                monitor-exit(r4)     // Catch:{ all -> 0x0118 }
                throw r0     // Catch:{ all -> 0x0116 }
            L_0x0116:
                r0 = move-exception
                goto L_0x011f
            L_0x0118:
                r0 = move-exception
                goto L_0x0114
            L_0x011a:
                r0 = move-exception
                r15 = r8
                goto L_0x011f
            L_0x011d:
                r0 = move-exception
                r15 = r5
            L_0x011f:
                okhttp3.internal.cache2.Relay r5 = okhttp3.internal.cache2.Relay.this
                monitor-enter(r5)
                okhttp3.internal.cache2.Relay r4 = okhttp3.internal.cache2.Relay.this     // Catch:{ all -> 0x012e }
                r6 = 0
                r4.upstreamReader = r6     // Catch:{ all -> 0x012e }
                okhttp3.internal.cache2.Relay r4 = okhttp3.internal.cache2.Relay.this     // Catch:{ all -> 0x012e }
                r4.notifyAll()     // Catch:{ all -> 0x012e }
                monitor-exit(r5)     // Catch:{ all -> 0x012e }
                throw r0
            L_0x012e:
                r0 = move-exception
                monitor-exit(r5)     // Catch:{ all -> 0x012e }
                throw r0
            L_0x0131:
                long r7 = r9 - r7
                long r7 = java.lang.Math.min(r2, r7)     // Catch:{ all -> 0x014c }
                okhttp3.internal.cache2.Relay r0 = okhttp3.internal.cache2.Relay.this     // Catch:{ all -> 0x014c }
                okio.Buffer r11 = r0.buffer     // Catch:{ all -> 0x014c }
                long r12 = r1.sourcePos     // Catch:{ all -> 0x014c }
                long r13 = r12 - r5
                r12 = r22
                r15 = r7
                r11.copyTo((okio.Buffer) r12, (long) r13, (long) r15)     // Catch:{ all -> 0x014c }
                long r11 = r1.sourcePos     // Catch:{ all -> 0x014c }
                long r11 = r11 + r7
                r1.sourcePos = r11     // Catch:{ all -> 0x014c }
                monitor-exit(r4)     // Catch:{ all -> 0x014c }
                return r7
            L_0x014c:
                r0 = move-exception
                monitor-exit(r4)     // Catch:{ all -> 0x014c }
                throw r0
            L_0x014f:
                java.lang.IllegalStateException r0 = new java.lang.IllegalStateException
                java.lang.String r4 = "closed"
                r0.<init>(r4)
                throw r0
            */
            throw new UnsupportedOperationException("Method not decompiled: okhttp3.internal.cache2.Relay.RelaySource.read(okio.Buffer, long):long");
        }

        public Timeout timeout() {
            return this.timeout;
        }

        public void close() throws IOException {
            if (this.fileOperator != null) {
                this.fileOperator = null;
                RandomAccessFile fileToClose = null;
                synchronized (Relay.this) {
                    Relay relay = Relay.this;
                    relay.sourceCount--;
                    if (Relay.this.sourceCount == 0) {
                        fileToClose = Relay.this.file;
                        Relay.this.file = null;
                    }
                }
                if (fileToClose != null) {
                    Util.closeQuietly((Closeable) fileToClose);
                }
            }
        }
    }
}
