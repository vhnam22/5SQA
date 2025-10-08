package okhttp3.internal.connection;

import java.io.IOException;
import java.lang.ref.WeakReference;
import java.net.Socket;
import okhttp3.Address;
import okhttp3.ConnectionPool;
import okhttp3.OkHttpClient;
import okhttp3.Route;
import okhttp3.internal.Internal;
import okhttp3.internal.Util;
import okhttp3.internal.http.HttpCodec;
import okhttp3.internal.http2.ConnectionShutdownException;
import okhttp3.internal.http2.ErrorCode;
import okhttp3.internal.http2.StreamResetException;

public final class StreamAllocation {
    static final /* synthetic */ boolean $assertionsDisabled = false;
    public final Address address;
    private final Object callStackTrace;
    private boolean canceled;
    private HttpCodec codec;
    private RealConnection connection;
    private final ConnectionPool connectionPool;
    private int refusedStreamCount;
    private boolean released;
    private Route route;
    private final RouteSelector routeSelector;

    public StreamAllocation(ConnectionPool connectionPool2, Address address2, Object callStackTrace2) {
        this.connectionPool = connectionPool2;
        this.address = address2;
        this.routeSelector = new RouteSelector(address2, routeDatabase());
        this.callStackTrace = callStackTrace2;
    }

    public HttpCodec newStream(OkHttpClient client, boolean doExtensiveHealthChecks) {
        try {
            HttpCodec resultCodec = findHealthyConnection(client.connectTimeoutMillis(), client.readTimeoutMillis(), client.writeTimeoutMillis(), client.retryOnConnectionFailure(), doExtensiveHealthChecks).newCodec(client, this);
            synchronized (this.connectionPool) {
                this.codec = resultCodec;
            }
            return resultCodec;
        } catch (IOException e) {
            throw new RouteException(e);
        }
    }

    /* JADX WARNING: Code restructure failed: missing block: B:11:0x0018, code lost:
        return r0;
     */
    /* JADX WARNING: Code restructure failed: missing block: B:9:0x0012, code lost:
        if (r0.isHealthy(r8) != false) goto L_0x0018;
     */
    /* Code decompiled incorrectly, please refer to instructions dump. */
    private okhttp3.internal.connection.RealConnection findHealthyConnection(int r4, int r5, int r6, boolean r7, boolean r8) throws java.io.IOException {
        /*
            r3 = this;
        L_0x0000:
            okhttp3.internal.connection.RealConnection r0 = r3.findConnection(r4, r5, r6, r7)
            okhttp3.ConnectionPool r1 = r3.connectionPool
            monitor-enter(r1)
            int r2 = r0.successCount     // Catch:{ all -> 0x0019 }
            if (r2 != 0) goto L_0x000d
            monitor-exit(r1)     // Catch:{ all -> 0x0019 }
            return r0
        L_0x000d:
            monitor-exit(r1)     // Catch:{ all -> 0x0019 }
            boolean r1 = r0.isHealthy(r8)
            if (r1 != 0) goto L_0x0018
            r3.noNewStreams()
            goto L_0x0000
        L_0x0018:
            return r0
        L_0x0019:
            r2 = move-exception
            monitor-exit(r1)     // Catch:{ all -> 0x0019 }
            throw r2
        */
        throw new UnsupportedOperationException("Method not decompiled: okhttp3.internal.connection.StreamAllocation.findHealthyConnection(int, int, int, boolean, boolean):okhttp3.internal.connection.RealConnection");
    }

    /* JADX WARNING: Code restructure failed: missing block: B:21:0x002d, code lost:
        if (r1 != null) goto L_0x0035;
     */
    /* JADX WARNING: Code restructure failed: missing block: B:22:0x002f, code lost:
        r1 = r7.routeSelector.next();
     */
    /* JADX WARNING: Code restructure failed: missing block: B:23:0x0035, code lost:
        r2 = r7.connectionPool;
     */
    /* JADX WARNING: Code restructure failed: missing block: B:24:0x0037, code lost:
        monitor-enter(r2);
     */
    /* JADX WARNING: Code restructure failed: missing block: B:27:0x003a, code lost:
        if (r7.canceled != false) goto L_0x0090;
     */
    /* JADX WARNING: Code restructure failed: missing block: B:28:0x003c, code lost:
        okhttp3.internal.Internal.instance.get(r7.connectionPool, r7.address, r7, r1);
        r0 = r7.connection;
     */
    /* JADX WARNING: Code restructure failed: missing block: B:29:0x0047, code lost:
        if (r0 == null) goto L_0x004b;
     */
    /* JADX WARNING: Code restructure failed: missing block: B:30:0x0049, code lost:
        monitor-exit(r2);
     */
    /* JADX WARNING: Code restructure failed: missing block: B:31:0x004a, code lost:
        return r0;
     */
    /* JADX WARNING: Code restructure failed: missing block: B:32:0x004b, code lost:
        r7.route = r1;
        r7.refusedStreamCount = 0;
        r0 = new okhttp3.internal.connection.RealConnection(r7.connectionPool, r1);
        acquire(r0);
     */
    /* JADX WARNING: Code restructure failed: missing block: B:33:0x005a, code lost:
        monitor-exit(r2);
     */
    /* JADX WARNING: Code restructure failed: missing block: B:34:0x005b, code lost:
        r0.connect(r8, r9, r10, r11);
        routeDatabase().connected(r0.route());
        r2 = null;
        r3 = r7.connectionPool;
     */
    /* JADX WARNING: Code restructure failed: missing block: B:35:0x006c, code lost:
        monitor-enter(r3);
     */
    /* JADX WARNING: Code restructure failed: missing block: B:37:?, code lost:
        okhttp3.internal.Internal.instance.put(r7.connectionPool, r0);
     */
    /* JADX WARNING: Code restructure failed: missing block: B:38:0x0078, code lost:
        if (r0.isMultiplexed() == false) goto L_0x0088;
     */
    /* JADX WARNING: Code restructure failed: missing block: B:39:0x007a, code lost:
        r2 = okhttp3.internal.Internal.instance.deduplicate(r7.connectionPool, r7.address, r7);
        r0 = r7.connection;
     */
    /* JADX WARNING: Code restructure failed: missing block: B:40:0x0088, code lost:
        monitor-exit(r3);
     */
    /* JADX WARNING: Code restructure failed: missing block: B:41:0x0089, code lost:
        okhttp3.internal.Util.closeQuietly(r2);
     */
    /* JADX WARNING: Code restructure failed: missing block: B:42:0x008c, code lost:
        return r0;
     */
    /* JADX WARNING: Code restructure failed: missing block: B:49:0x0097, code lost:
        throw new java.io.IOException("Canceled");
     */
    /* Code decompiled incorrectly, please refer to instructions dump. */
    private okhttp3.internal.connection.RealConnection findConnection(int r8, int r9, int r10, boolean r11) throws java.io.IOException {
        /*
            r7 = this;
            okhttp3.ConnectionPool r0 = r7.connectionPool
            monitor-enter(r0)
            boolean r1 = r7.released     // Catch:{ all -> 0x00b3 }
            if (r1 != 0) goto L_0x00ab
            okhttp3.internal.http.HttpCodec r1 = r7.codec     // Catch:{ all -> 0x00b3 }
            if (r1 != 0) goto L_0x00a3
            boolean r1 = r7.canceled     // Catch:{ all -> 0x00b3 }
            if (r1 != 0) goto L_0x009b
            okhttp3.internal.connection.RealConnection r1 = r7.connection     // Catch:{ all -> 0x00b3 }
            if (r1 == 0) goto L_0x0019
            boolean r2 = r1.noNewStreams     // Catch:{ all -> 0x00b3 }
            if (r2 != 0) goto L_0x0019
            monitor-exit(r0)     // Catch:{ all -> 0x00b3 }
            return r1
        L_0x0019:
            okhttp3.internal.Internal r2 = okhttp3.internal.Internal.instance     // Catch:{ all -> 0x00b3 }
            okhttp3.ConnectionPool r3 = r7.connectionPool     // Catch:{ all -> 0x00b3 }
            okhttp3.Address r4 = r7.address     // Catch:{ all -> 0x00b3 }
            r5 = 0
            r2.get(r3, r4, r7, r5)     // Catch:{ all -> 0x00b3 }
            okhttp3.internal.connection.RealConnection r2 = r7.connection     // Catch:{ all -> 0x00b3 }
            if (r2 == 0) goto L_0x0029
            monitor-exit(r0)     // Catch:{ all -> 0x00b3 }
            return r2
        L_0x0029:
            okhttp3.Route r2 = r7.route     // Catch:{ all -> 0x00b3 }
            r1 = r2
            monitor-exit(r0)     // Catch:{ all -> 0x00b3 }
            if (r1 != 0) goto L_0x0035
            okhttp3.internal.connection.RouteSelector r0 = r7.routeSelector
            okhttp3.Route r1 = r0.next()
        L_0x0035:
            okhttp3.ConnectionPool r2 = r7.connectionPool
            monitor-enter(r2)
            boolean r0 = r7.canceled     // Catch:{ all -> 0x0098 }
            if (r0 != 0) goto L_0x0090
            okhttp3.internal.Internal r0 = okhttp3.internal.Internal.instance     // Catch:{ all -> 0x0098 }
            okhttp3.ConnectionPool r3 = r7.connectionPool     // Catch:{ all -> 0x0098 }
            okhttp3.Address r4 = r7.address     // Catch:{ all -> 0x0098 }
            r0.get(r3, r4, r7, r1)     // Catch:{ all -> 0x0098 }
            okhttp3.internal.connection.RealConnection r0 = r7.connection     // Catch:{ all -> 0x0098 }
            if (r0 == 0) goto L_0x004b
            monitor-exit(r2)     // Catch:{ all -> 0x0098 }
            return r0
        L_0x004b:
            r7.route = r1     // Catch:{ all -> 0x0098 }
            r0 = 0
            r7.refusedStreamCount = r0     // Catch:{ all -> 0x0098 }
            okhttp3.internal.connection.RealConnection r0 = new okhttp3.internal.connection.RealConnection     // Catch:{ all -> 0x0098 }
            okhttp3.ConnectionPool r3 = r7.connectionPool     // Catch:{ all -> 0x0098 }
            r0.<init>(r3, r1)     // Catch:{ all -> 0x0098 }
            r7.acquire(r0)     // Catch:{ all -> 0x0098 }
            monitor-exit(r2)     // Catch:{ all -> 0x0098 }
            r0.connect(r8, r9, r10, r11)
            okhttp3.internal.connection.RouteDatabase r2 = r7.routeDatabase()
            okhttp3.Route r3 = r0.route()
            r2.connected(r3)
            r2 = 0
            okhttp3.ConnectionPool r3 = r7.connectionPool
            monitor-enter(r3)
            okhttp3.internal.Internal r4 = okhttp3.internal.Internal.instance     // Catch:{ all -> 0x008d }
            okhttp3.ConnectionPool r5 = r7.connectionPool     // Catch:{ all -> 0x008d }
            r4.put(r5, r0)     // Catch:{ all -> 0x008d }
            boolean r4 = r0.isMultiplexed()     // Catch:{ all -> 0x008d }
            if (r4 == 0) goto L_0x0088
            okhttp3.internal.Internal r4 = okhttp3.internal.Internal.instance     // Catch:{ all -> 0x008d }
            okhttp3.ConnectionPool r5 = r7.connectionPool     // Catch:{ all -> 0x008d }
            okhttp3.Address r6 = r7.address     // Catch:{ all -> 0x008d }
            java.net.Socket r4 = r4.deduplicate(r5, r6, r7)     // Catch:{ all -> 0x008d }
            r2 = r4
            okhttp3.internal.connection.RealConnection r4 = r7.connection     // Catch:{ all -> 0x008d }
            r0 = r4
        L_0x0088:
            monitor-exit(r3)     // Catch:{ all -> 0x008d }
            okhttp3.internal.Util.closeQuietly((java.net.Socket) r2)
            return r0
        L_0x008d:
            r4 = move-exception
            monitor-exit(r3)     // Catch:{ all -> 0x008d }
            throw r4
        L_0x0090:
            java.io.IOException r0 = new java.io.IOException     // Catch:{ all -> 0x0098 }
            java.lang.String r3 = "Canceled"
            r0.<init>(r3)     // Catch:{ all -> 0x0098 }
            throw r0     // Catch:{ all -> 0x0098 }
        L_0x0098:
            r0 = move-exception
            monitor-exit(r2)     // Catch:{ all -> 0x0098 }
            throw r0
        L_0x009b:
            java.io.IOException r1 = new java.io.IOException     // Catch:{ all -> 0x00b3 }
            java.lang.String r2 = "Canceled"
            r1.<init>(r2)     // Catch:{ all -> 0x00b3 }
            throw r1     // Catch:{ all -> 0x00b3 }
        L_0x00a3:
            java.lang.IllegalStateException r1 = new java.lang.IllegalStateException     // Catch:{ all -> 0x00b3 }
            java.lang.String r2 = "codec != null"
            r1.<init>(r2)     // Catch:{ all -> 0x00b3 }
            throw r1     // Catch:{ all -> 0x00b3 }
        L_0x00ab:
            java.lang.IllegalStateException r1 = new java.lang.IllegalStateException     // Catch:{ all -> 0x00b3 }
            java.lang.String r2 = "released"
            r1.<init>(r2)     // Catch:{ all -> 0x00b3 }
            throw r1     // Catch:{ all -> 0x00b3 }
        L_0x00b3:
            r1 = move-exception
            monitor-exit(r0)     // Catch:{ all -> 0x00b3 }
            throw r1
        */
        throw new UnsupportedOperationException("Method not decompiled: okhttp3.internal.connection.StreamAllocation.findConnection(int, int, int, boolean):okhttp3.internal.connection.RealConnection");
    }

    public void streamFinished(boolean noNewStreams, HttpCodec codec2) {
        Socket socket;
        synchronized (this.connectionPool) {
            if (codec2 != null) {
                if (codec2 == this.codec) {
                    if (!noNewStreams) {
                        this.connection.successCount++;
                    }
                    socket = deallocate(noNewStreams, false, true);
                }
            }
            throw new IllegalStateException("expected " + this.codec + " but was " + codec2);
        }
        Util.closeQuietly(socket);
    }

    public HttpCodec codec() {
        HttpCodec httpCodec;
        synchronized (this.connectionPool) {
            httpCodec = this.codec;
        }
        return httpCodec;
    }

    private RouteDatabase routeDatabase() {
        return Internal.instance.routeDatabase(this.connectionPool);
    }

    public synchronized RealConnection connection() {
        return this.connection;
    }

    public void release() {
        Socket socket;
        synchronized (this.connectionPool) {
            socket = deallocate(false, true, false);
        }
        Util.closeQuietly(socket);
    }

    public void noNewStreams() {
        Socket socket;
        synchronized (this.connectionPool) {
            socket = deallocate(true, false, false);
        }
        Util.closeQuietly(socket);
    }

    private Socket deallocate(boolean noNewStreams, boolean released2, boolean streamFinished) {
        if (Thread.holdsLock(this.connectionPool)) {
            if (streamFinished) {
                this.codec = null;
            }
            if (released2) {
                this.released = true;
            }
            Socket socket = null;
            RealConnection realConnection = this.connection;
            if (realConnection != null) {
                if (noNewStreams) {
                    realConnection.noNewStreams = true;
                }
                if (this.codec == null && (this.released || this.connection.noNewStreams)) {
                    release(this.connection);
                    if (this.connection.allocations.isEmpty()) {
                        this.connection.idleAtNanos = System.nanoTime();
                        if (Internal.instance.connectionBecameIdle(this.connectionPool, this.connection)) {
                            socket = this.connection.socket();
                        }
                    }
                    this.connection = null;
                }
            }
            return socket;
        }
        throw new AssertionError();
    }

    public void cancel() {
        HttpCodec codecToCancel;
        RealConnection connectionToCancel;
        synchronized (this.connectionPool) {
            this.canceled = true;
            codecToCancel = this.codec;
            connectionToCancel = this.connection;
        }
        if (codecToCancel != null) {
            codecToCancel.cancel();
        } else if (connectionToCancel != null) {
            connectionToCancel.cancel();
        }
    }

    public void streamFailed(IOException e) {
        Socket socket;
        boolean noNewStreams = false;
        synchronized (this.connectionPool) {
            if (e instanceof StreamResetException) {
                StreamResetException streamResetException = (StreamResetException) e;
                if (streamResetException.errorCode == ErrorCode.REFUSED_STREAM) {
                    this.refusedStreamCount++;
                }
                if (streamResetException.errorCode != ErrorCode.REFUSED_STREAM || this.refusedStreamCount > 1) {
                    noNewStreams = true;
                    this.route = null;
                }
            } else {
                RealConnection realConnection = this.connection;
                if (realConnection != null) {
                    if (!realConnection.isMultiplexed() || (e instanceof ConnectionShutdownException)) {
                        noNewStreams = true;
                        if (this.connection.successCount == 0) {
                            Route route2 = this.route;
                            if (!(route2 == null || e == null)) {
                                this.routeSelector.connectFailed(route2, e);
                            }
                            this.route = null;
                        }
                    }
                    socket = deallocate(noNewStreams, false, true);
                }
            }
            socket = deallocate(noNewStreams, false, true);
        }
        Util.closeQuietly(socket);
    }

    public void acquire(RealConnection connection2) {
        if (!Thread.holdsLock(this.connectionPool)) {
            throw new AssertionError();
        } else if (this.connection == null) {
            this.connection = connection2;
            connection2.allocations.add(new StreamAllocationReference(this, this.callStackTrace));
        } else {
            throw new IllegalStateException();
        }
    }

    private void release(RealConnection connection2) {
        int size = connection2.allocations.size();
        for (int i = 0; i < size; i++) {
            if (connection2.allocations.get(i).get() == this) {
                connection2.allocations.remove(i);
                return;
            }
        }
        throw new IllegalStateException();
    }

    public Socket releaseAndAcquire(RealConnection newConnection) {
        if (!Thread.holdsLock(this.connectionPool)) {
            throw new AssertionError();
        } else if (this.codec == null && this.connection.allocations.size() == 1) {
            Socket socket = deallocate(true, false, false);
            this.connection = newConnection;
            newConnection.allocations.add(this.connection.allocations.get(0));
            return socket;
        } else {
            throw new IllegalStateException();
        }
    }

    public boolean hasMoreRoutes() {
        return this.route != null || this.routeSelector.hasNext();
    }

    public String toString() {
        RealConnection connection2 = connection();
        return connection2 != null ? connection2.toString() : this.address.toString();
    }

    public static final class StreamAllocationReference extends WeakReference<StreamAllocation> {
        public final Object callStackTrace;

        StreamAllocationReference(StreamAllocation referent, Object callStackTrace2) {
            super(referent);
            this.callStackTrace = callStackTrace2;
        }
    }
}
