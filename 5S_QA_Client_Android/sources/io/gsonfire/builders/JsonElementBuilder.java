package io.gsonfire.builders;

import com.google.gson.JsonElement;

public interface JsonElementBuilder<T extends JsonElement> {
    T build();
}
