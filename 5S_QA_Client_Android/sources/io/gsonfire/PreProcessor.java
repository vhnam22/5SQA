package io.gsonfire;

import com.google.gson.Gson;
import com.google.gson.JsonElement;

public interface PreProcessor<T> {
    void preDeserialize(Class<? extends T> cls, JsonElement jsonElement, Gson gson);
}
