package io.gsonfire.util;

import java.util.ArrayList;
import java.util.Arrays;
import java.util.Collection;
import java.util.Iterator;
import java.util.List;

public final class SimpleIterable<T> implements Iterable<T> {
    private final Iterable<T> iterable;

    private SimpleIterable(Iterable<T> iterable2) {
        this.iterable = iterable2;
    }

    public Iterator<T> iterator() {
        return this.iterable.iterator();
    }

    public final Collection<T> toCollection() {
        List<T> list = new ArrayList<>();
        addTo(list);
        return list;
    }

    private final void addTo(Collection<T> collection) {
        Iterator it = iterator();
        while (it.hasNext()) {
            collection.add(it.next());
        }
    }

    public static <T> SimpleIterable<T> of(Iterable<T> iterable2) {
        if (iterable2 != null) {
            return new SimpleIterable<>(iterable2);
        }
        throw new NullPointerException("The iterable parameter cannot be null");
    }

    public static <T> SimpleIterable<T> of(T... array) {
        return of(Arrays.asList(array));
    }

    public boolean equals(Object o) {
        if (this == o) {
            return true;
        }
        if (o == null || getClass() != o.getClass()) {
            return false;
        }
        SimpleIterable<?> that = (SimpleIterable) o;
        Iterable<T> iterable2 = this.iterable;
        if (iterable2 != null) {
            if (!iterable2.equals(that.iterable)) {
                return false;
            }
            return true;
        } else if (that.iterable == null) {
            return true;
        }
        return false;
    }

    public int hashCode() {
        Iterable<T> iterable2 = this.iterable;
        if (iterable2 != null) {
            return iterable2.hashCode();
        }
        return 0;
    }

    public String toString() {
        return this.iterable.toString();
    }
}
