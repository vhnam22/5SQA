package io.gsonfire;

import com.google.gson.Gson;
import com.google.gson.JsonElement;

public interface PostProcessor<T> {
    void postDeserialize(T t, JsonElement jsonElement, Gson gson);

    void postSerialize(JsonElement jsonElement, T t, Gson gson);
}
