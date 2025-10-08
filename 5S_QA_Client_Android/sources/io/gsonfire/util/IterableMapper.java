package io.gsonfire.util;

import java.util.Iterator;

public class IterableMapper<F, T> implements Iterable<T> {
    /* access modifiers changed from: private */
    public final Mapper<F, T> mapper;
    private final Iterable<F> source;

    private IterableMapper(Iterable<F> source2, Mapper<F, T> mapper2) {
        this.source = source2;
        this.mapper = mapper2;
    }

    public Iterator<T> iterator() {
        final Iterator<F> sourceIterator = this.source.iterator();
        return new Iterator<T>() {
            public boolean hasNext() {
                return sourceIterator.hasNext();
            }

            public T next() {
                return IterableMapper.this.mapper.map(sourceIterator.next());
            }

            public void remove() {
            }
        };
    }

    /* JADX WARNING: Removed duplicated region for block: B:7:0x0018  */
    /* Code decompiled incorrectly, please refer to instructions dump. */
    public boolean equals(java.lang.Object r7) {
        /*
            r6 = this;
            r0 = 0
            if (r7 == 0) goto L_0x0036
            boolean r1 = r7 instanceof java.lang.Iterable
            if (r1 == 0) goto L_0x0036
            r1 = r7
            java.lang.Iterable r1 = (java.lang.Iterable) r1
            java.util.Iterator r1 = r1.iterator()
            java.util.Iterator r2 = r6.iterator()
        L_0x0012:
            boolean r3 = r2.hasNext()
            if (r3 == 0) goto L_0x002f
            boolean r3 = r1.hasNext()
            if (r3 == 0) goto L_0x002e
            java.lang.Object r3 = r2.next()
            java.lang.Object r4 = r1.next()
            boolean r5 = areObjectEquals(r3, r4)
            if (r5 != 0) goto L_0x002d
            return r0
        L_0x002d:
            goto L_0x0012
        L_0x002e:
            return r0
        L_0x002f:
            boolean r0 = r1.hasNext()
            r0 = r0 ^ 1
            return r0
        L_0x0036:
            return r0
        */
        throw new UnsupportedOperationException("Method not decompiled: io.gsonfire.util.IterableMapper.equals(java.lang.Object):boolean");
    }

    private static boolean areObjectEquals(Object a, Object b) {
        return a == b || (a != null && a.equals(b));
    }

    public static <F, T> Iterable<T> create(Iterable<F> source2, Mapper<F, T> mapper2) {
        return new IterableMapper(source2, mapper2);
    }
}
