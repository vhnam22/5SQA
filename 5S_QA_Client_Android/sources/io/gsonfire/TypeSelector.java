package io.gsonfire;

import com.google.gson.JsonElement;

public interface TypeSelector<T> {
    Class<? extends T> getClassForElement(JsonElement jsonElement);
}
