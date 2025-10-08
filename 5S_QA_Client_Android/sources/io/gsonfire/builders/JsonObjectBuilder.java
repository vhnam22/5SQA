package io.gsonfire.builders;

import com.google.gson.JsonElement;
import com.google.gson.JsonNull;
import com.google.gson.JsonObject;
import io.gsonfire.util.JsonUtils;
import java.util.Map;

public final class JsonObjectBuilder implements JsonElementBuilder<JsonObject> {
    private final JsonObject object = new JsonObject();

    public JsonObjectBuilder set(String property, String value) {
        this.object.addProperty(property, value);
        return this;
    }

    public JsonObjectBuilder set(String property, Number value) {
        this.object.addProperty(property, value);
        return this;
    }

    public JsonObjectBuilder set(String property, Boolean value) {
        this.object.addProperty(property, value);
        return this;
    }

    public JsonObjectBuilder set(String property, JsonElement value) {
        this.object.add(property, value);
        return this;
    }

    public JsonObjectBuilder set(String property, JsonElementBuilder builder) {
        this.object.add(property, builder.build());
        return this;
    }

    public JsonObjectBuilder setNull(String property) {
        this.object.add(property, JsonNull.INSTANCE);
        return this;
    }

    public JsonObjectBuilder merge(JsonObject jsonObject) {
        for (Map.Entry<String, JsonElement> entry : JsonUtils.deepCopy(jsonObject).getAsJsonObject().entrySet()) {
            this.object.add(entry.getKey(), entry.getValue());
        }
        return this;
    }

    public JsonObject build() {
        return JsonUtils.deepCopy(this.object).getAsJsonObject();
    }
}
