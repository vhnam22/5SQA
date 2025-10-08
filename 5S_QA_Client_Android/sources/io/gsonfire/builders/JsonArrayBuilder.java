package io.gsonfire.builders;

import com.google.gson.JsonArray;
import com.google.gson.JsonElement;
import io.gsonfire.util.JsonUtils;

public final class JsonArrayBuilder implements JsonElementBuilder<JsonArray> {
    private final JsonArray array = new JsonArray();

    public JsonArrayBuilder add(JsonElement element) {
        this.array.add(element);
        return this;
    }

    public JsonArrayBuilder add(JsonElementBuilder builder) {
        this.array.add(builder.build());
        return this;
    }

    public JsonArrayBuilder add(Boolean bool) {
        this.array.add(bool);
        return this;
    }

    public JsonArrayBuilder add(Character character) {
        this.array.add(character);
        return this;
    }

    public JsonArrayBuilder add(Number number) {
        this.array.add(number);
        return this;
    }

    public JsonArrayBuilder add(String string) {
        this.array.add(string);
        return this;
    }

    public JsonArrayBuilder addAll(JsonArray jsonArray) {
        this.array.addAll(jsonArray);
        return this;
    }

    public JsonArray build() {
        return JsonUtils.deepCopy(this.array).getAsJsonArray();
    }

    public static JsonArrayBuilder start() {
        return new JsonArrayBuilder();
    }
}
