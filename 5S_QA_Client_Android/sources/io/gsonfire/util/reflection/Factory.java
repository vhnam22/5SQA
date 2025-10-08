package io.gsonfire.util.reflection;

public interface Factory {
    <T> T get(Class<T> cls);
}
