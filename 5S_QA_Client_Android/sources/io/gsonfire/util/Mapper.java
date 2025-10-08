package io.gsonfire.util;

public interface Mapper<F, T> {
    T map(F f);
}
