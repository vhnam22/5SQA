package okhttp3;

import java.util.concurrent.TimeUnit;
import javax.annotation.Nullable;

public final class CacheControl {
    public static final CacheControl FORCE_CACHE = new Builder().onlyIfCached().maxStale(Integer.MAX_VALUE, TimeUnit.SECONDS).build();
    public static final CacheControl FORCE_NETWORK = new Builder().noCache().build();
    @Nullable
    String headerValue;
    private final boolean immutable;
    private final boolean isPrivate;
    private final boolean isPublic;
    private final int maxAgeSeconds;
    private final int maxStaleSeconds;
    private final int minFreshSeconds;
    private final boolean mustRevalidate;
    private final boolean noCache;
    private final boolean noStore;
    private final boolean noTransform;
    private final boolean onlyIfCached;
    private final int sMaxAgeSeconds;

    private CacheControl(boolean noCache2, boolean noStore2, int maxAgeSeconds2, int sMaxAgeSeconds2, boolean isPrivate2, boolean isPublic2, boolean mustRevalidate2, int maxStaleSeconds2, int minFreshSeconds2, boolean onlyIfCached2, boolean noTransform2, boolean immutable2, @Nullable String headerValue2) {
        this.noCache = noCache2;
        this.noStore = noStore2;
        this.maxAgeSeconds = maxAgeSeconds2;
        this.sMaxAgeSeconds = sMaxAgeSeconds2;
        this.isPrivate = isPrivate2;
        this.isPublic = isPublic2;
        this.mustRevalidate = mustRevalidate2;
        this.maxStaleSeconds = maxStaleSeconds2;
        this.minFreshSeconds = minFreshSeconds2;
        this.onlyIfCached = onlyIfCached2;
        this.noTransform = noTransform2;
        this.immutable = immutable2;
        this.headerValue = headerValue2;
    }

    CacheControl(Builder builder) {
        this.noCache = builder.noCache;
        this.noStore = builder.noStore;
        this.maxAgeSeconds = builder.maxAgeSeconds;
        this.sMaxAgeSeconds = -1;
        this.isPrivate = false;
        this.isPublic = false;
        this.mustRevalidate = false;
        this.maxStaleSeconds = builder.maxStaleSeconds;
        this.minFreshSeconds = builder.minFreshSeconds;
        this.onlyIfCached = builder.onlyIfCached;
        this.noTransform = builder.noTransform;
        this.immutable = builder.immutable;
    }

    public boolean noCache() {
        return this.noCache;
    }

    public boolean noStore() {
        return this.noStore;
    }

    public int maxAgeSeconds() {
        return this.maxAgeSeconds;
    }

    public int sMaxAgeSeconds() {
        return this.sMaxAgeSeconds;
    }

    public boolean isPrivate() {
        return this.isPrivate;
    }

    public boolean isPublic() {
        return this.isPublic;
    }

    public boolean mustRevalidate() {
        return this.mustRevalidate;
    }

    public int maxStaleSeconds() {
        return this.maxStaleSeconds;
    }

    public int minFreshSeconds() {
        return this.minFreshSeconds;
    }

    public boolean onlyIfCached() {
        return this.onlyIfCached;
    }

    public boolean noTransform() {
        return this.noTransform;
    }

    public boolean immutable() {
        return this.immutable;
    }

    /* JADX WARNING: Removed duplicated region for block: B:29:0x00b3  */
    /* JADX WARNING: Removed duplicated region for block: B:30:0x00be  */
    /* Code decompiled incorrectly, please refer to instructions dump. */
    public static okhttp3.CacheControl parse(okhttp3.Headers r32) {
        /*
            r0 = r32
            r1 = 0
            r2 = 0
            r3 = -1
            r4 = -1
            r5 = 0
            r6 = 0
            r7 = 0
            r8 = -1
            r9 = -1
            r10 = 0
            r11 = 0
            r12 = 0
            r13 = 1
            r14 = 0
            r15 = 0
            r16 = r1
            int r1 = r32.size()
        L_0x0017:
            if (r15 >= r1) goto L_0x0197
            r17 = r1
            java.lang.String r1 = r0.name(r15)
            r31 = r12
            java.lang.String r12 = r0.value(r15)
            java.lang.String r0 = "Cache-Control"
            boolean r0 = r1.equalsIgnoreCase(r0)
            if (r0 == 0) goto L_0x0033
            if (r14 == 0) goto L_0x0031
            r13 = 0
            goto L_0x003c
        L_0x0031:
            r14 = r12
            goto L_0x003c
        L_0x0033:
            java.lang.String r0 = "Pragma"
            boolean r0 = r1.equalsIgnoreCase(r0)
            if (r0 == 0) goto L_0x018b
            r13 = 0
        L_0x003c:
            r0 = 0
        L_0x003d:
            r18 = r1
            int r1 = r12.length()
            if (r0 >= r1) goto L_0x0184
            r1 = r0
            r19 = r2
            java.lang.String r2 = "=,;"
            int r0 = okhttp3.internal.http.HttpHeaders.skipUntil(r12, r0, r2)
            java.lang.String r2 = r12.substring(r1, r0)
            java.lang.String r2 = r2.trim()
            r20 = r1
            int r1 = r12.length()
            if (r0 == r1) goto L_0x00a6
            char r1 = r12.charAt(r0)
            r21 = r3
            r3 = 44
            if (r1 == r3) goto L_0x00a8
            char r1 = r12.charAt(r0)
            r3 = 59
            if (r1 != r3) goto L_0x0071
            goto L_0x00a8
        L_0x0071:
            int r0 = r0 + 1
            int r0 = okhttp3.internal.http.HttpHeaders.skipWhitespace(r12, r0)
            int r1 = r12.length()
            if (r0 >= r1) goto L_0x0096
            char r1 = r12.charAt(r0)
            r3 = 34
            if (r1 != r3) goto L_0x0096
            int r0 = r0 + 1
            r1 = r0
            java.lang.String r3 = "\""
            int r0 = okhttp3.internal.http.HttpHeaders.skipUntil(r12, r0, r3)
            java.lang.String r3 = r12.substring(r1, r0)
            int r0 = r0 + 1
            goto L_0x00ab
        L_0x0096:
            r1 = r0
            java.lang.String r3 = ",;"
            int r0 = okhttp3.internal.http.HttpHeaders.skipUntil(r12, r0, r3)
            java.lang.String r3 = r12.substring(r1, r0)
            java.lang.String r3 = r3.trim()
            goto L_0x00ab
        L_0x00a6:
            r21 = r3
        L_0x00a8:
            int r0 = r0 + 1
            r3 = 0
        L_0x00ab:
            java.lang.String r1 = "no-cache"
            boolean r1 = r1.equalsIgnoreCase(r2)
            if (r1 == 0) goto L_0x00be
            r1 = 1
            r22 = r0
            r16 = r1
            r2 = r19
            r3 = r21
            goto L_0x017e
        L_0x00be:
            java.lang.String r1 = "no-store"
            boolean r1 = r1.equalsIgnoreCase(r2)
            if (r1 == 0) goto L_0x00ce
            r1 = 1
            r22 = r0
            r2 = r1
            r3 = r21
            goto L_0x017e
        L_0x00ce:
            java.lang.String r1 = "max-age"
            boolean r1 = r1.equalsIgnoreCase(r2)
            r22 = r0
            r0 = -1
            if (r1 == 0) goto L_0x00e2
            int r0 = okhttp3.internal.http.HttpHeaders.parseSeconds(r3, r0)
            r3 = r0
            r2 = r19
            goto L_0x017e
        L_0x00e2:
            java.lang.String r1 = "s-maxage"
            boolean r1 = r1.equalsIgnoreCase(r2)
            if (r1 == 0) goto L_0x00f5
            int r0 = okhttp3.internal.http.HttpHeaders.parseSeconds(r3, r0)
            r4 = r0
            r2 = r19
            r3 = r21
            goto L_0x017e
        L_0x00f5:
            java.lang.String r1 = "private"
            boolean r1 = r1.equalsIgnoreCase(r2)
            if (r1 == 0) goto L_0x0105
            r0 = 1
            r5 = r0
            r2 = r19
            r3 = r21
            goto L_0x017e
        L_0x0105:
            java.lang.String r1 = "public"
            boolean r1 = r1.equalsIgnoreCase(r2)
            if (r1 == 0) goto L_0x0115
            r0 = 1
            r6 = r0
            r2 = r19
            r3 = r21
            goto L_0x017e
        L_0x0115:
            java.lang.String r1 = "must-revalidate"
            boolean r1 = r1.equalsIgnoreCase(r2)
            if (r1 == 0) goto L_0x0125
            r0 = 1
            r7 = r0
            r2 = r19
            r3 = r21
            goto L_0x017e
        L_0x0125:
            java.lang.String r1 = "max-stale"
            boolean r1 = r1.equalsIgnoreCase(r2)
            if (r1 == 0) goto L_0x013a
            r0 = 2147483647(0x7fffffff, float:NaN)
            int r0 = okhttp3.internal.http.HttpHeaders.parseSeconds(r3, r0)
            r8 = r0
            r2 = r19
            r3 = r21
            goto L_0x017e
        L_0x013a:
            java.lang.String r1 = "min-fresh"
            boolean r1 = r1.equalsIgnoreCase(r2)
            if (r1 == 0) goto L_0x014c
            int r0 = okhttp3.internal.http.HttpHeaders.parseSeconds(r3, r0)
            r9 = r0
            r2 = r19
            r3 = r21
            goto L_0x017e
        L_0x014c:
            java.lang.String r0 = "only-if-cached"
            boolean r0 = r0.equalsIgnoreCase(r2)
            if (r0 == 0) goto L_0x015b
            r0 = 1
            r10 = r0
            r2 = r19
            r3 = r21
            goto L_0x017e
        L_0x015b:
            java.lang.String r0 = "no-transform"
            boolean r0 = r0.equalsIgnoreCase(r2)
            if (r0 == 0) goto L_0x016a
            r0 = 1
            r11 = r0
            r2 = r19
            r3 = r21
            goto L_0x017e
        L_0x016a:
            java.lang.String r0 = "immutable"
            boolean r0 = r0.equalsIgnoreCase(r2)
            if (r0 == 0) goto L_0x017a
            r0 = 1
            r31 = r0
            r2 = r19
            r3 = r21
            goto L_0x017e
        L_0x017a:
            r2 = r19
            r3 = r21
        L_0x017e:
            r1 = r18
            r0 = r22
            goto L_0x003d
        L_0x0184:
            r19 = r2
            r21 = r3
            r12 = r31
            goto L_0x018f
        L_0x018b:
            r18 = r1
            r12 = r31
        L_0x018f:
            int r15 = r15 + 1
            r0 = r32
            r1 = r17
            goto L_0x0017
        L_0x0197:
            r17 = r1
            r31 = r12
            if (r13 != 0) goto L_0x019e
            r14 = 0
        L_0x019e:
            okhttp3.CacheControl r0 = new okhttp3.CacheControl
            r17 = r0
            r18 = r16
            r19 = r2
            r20 = r3
            r21 = r4
            r22 = r5
            r23 = r6
            r24 = r7
            r25 = r8
            r26 = r9
            r27 = r10
            r28 = r11
            r29 = r31
            r30 = r14
            r17.<init>(r18, r19, r20, r21, r22, r23, r24, r25, r26, r27, r28, r29, r30)
            return r0
        */
        throw new UnsupportedOperationException("Method not decompiled: okhttp3.CacheControl.parse(okhttp3.Headers):okhttp3.CacheControl");
    }

    public String toString() {
        String result = this.headerValue;
        if (result != null) {
            return result;
        }
        String headerValue2 = headerValue();
        this.headerValue = headerValue2;
        return headerValue2;
    }

    private String headerValue() {
        StringBuilder result = new StringBuilder();
        if (this.noCache) {
            result.append("no-cache, ");
        }
        if (this.noStore) {
            result.append("no-store, ");
        }
        if (this.maxAgeSeconds != -1) {
            result.append("max-age=").append(this.maxAgeSeconds).append(", ");
        }
        if (this.sMaxAgeSeconds != -1) {
            result.append("s-maxage=").append(this.sMaxAgeSeconds).append(", ");
        }
        if (this.isPrivate) {
            result.append("private, ");
        }
        if (this.isPublic) {
            result.append("public, ");
        }
        if (this.mustRevalidate) {
            result.append("must-revalidate, ");
        }
        if (this.maxStaleSeconds != -1) {
            result.append("max-stale=").append(this.maxStaleSeconds).append(", ");
        }
        if (this.minFreshSeconds != -1) {
            result.append("min-fresh=").append(this.minFreshSeconds).append(", ");
        }
        if (this.onlyIfCached) {
            result.append("only-if-cached, ");
        }
        if (this.noTransform) {
            result.append("no-transform, ");
        }
        if (this.immutable) {
            result.append("immutable, ");
        }
        if (result.length() == 0) {
            return "";
        }
        result.delete(result.length() - 2, result.length());
        return result.toString();
    }

    public static final class Builder {
        boolean immutable;
        int maxAgeSeconds = -1;
        int maxStaleSeconds = -1;
        int minFreshSeconds = -1;
        boolean noCache;
        boolean noStore;
        boolean noTransform;
        boolean onlyIfCached;

        public Builder noCache() {
            this.noCache = true;
            return this;
        }

        public Builder noStore() {
            this.noStore = true;
            return this;
        }

        public Builder maxAge(int maxAge, TimeUnit timeUnit) {
            int i;
            if (maxAge >= 0) {
                long maxAgeSecondsLong = timeUnit.toSeconds((long) maxAge);
                if (maxAgeSecondsLong > 2147483647L) {
                    i = Integer.MAX_VALUE;
                } else {
                    i = (int) maxAgeSecondsLong;
                }
                this.maxAgeSeconds = i;
                return this;
            }
            throw new IllegalArgumentException("maxAge < 0: " + maxAge);
        }

        public Builder maxStale(int maxStale, TimeUnit timeUnit) {
            int i;
            if (maxStale >= 0) {
                long maxStaleSecondsLong = timeUnit.toSeconds((long) maxStale);
                if (maxStaleSecondsLong > 2147483647L) {
                    i = Integer.MAX_VALUE;
                } else {
                    i = (int) maxStaleSecondsLong;
                }
                this.maxStaleSeconds = i;
                return this;
            }
            throw new IllegalArgumentException("maxStale < 0: " + maxStale);
        }

        public Builder minFresh(int minFresh, TimeUnit timeUnit) {
            int i;
            if (minFresh >= 0) {
                long minFreshSecondsLong = timeUnit.toSeconds((long) minFresh);
                if (minFreshSecondsLong > 2147483647L) {
                    i = Integer.MAX_VALUE;
                } else {
                    i = (int) minFreshSecondsLong;
                }
                this.minFreshSeconds = i;
                return this;
            }
            throw new IllegalArgumentException("minFresh < 0: " + minFresh);
        }

        public Builder onlyIfCached() {
            this.onlyIfCached = true;
            return this;
        }

        public Builder noTransform() {
            this.noTransform = true;
            return this;
        }

        public Builder immutable() {
            this.immutable = true;
            return this;
        }

        public CacheControl build() {
            return new CacheControl(this);
        }
    }
}
