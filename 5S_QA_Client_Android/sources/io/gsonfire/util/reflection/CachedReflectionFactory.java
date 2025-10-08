package io.gsonfire.util.reflection;

import java.lang.ref.SoftReference;
import java.util.concurrent.ConcurrentHashMap;
import java.util.concurrent.ConcurrentMap;

public class CachedReflectionFactory implements Factory {
    private final ConcurrentMap<Class, SoftReference<Object>> cache = new ConcurrentHashMap();

    public <T> T get(Class<T> clazz) {
        T result;
        SoftReference<T> resultRef = (SoftReference) this.cache.get(clazz);
        if (resultRef != null && (result = resultRef.get()) != null) {
            return result;
        }
        try {
            T result2 = clazz.newInstance();
            this.cache.putIfAbsent(clazz, new SoftReference(result2));
            return result2;
        } catch (InstantiationException e) {
            throw new RuntimeException(e);
        } catch (IllegalAccessException e2) {
            throw new RuntimeException(e2);
        }
    }
}
