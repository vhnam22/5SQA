package io.gsonfire.util;

import com.google.gson.JsonArray;
import com.google.gson.JsonElement;
import com.google.gson.JsonNull;
import com.google.gson.JsonObject;
import com.google.gson.TypeAdapter;
import com.google.gson.internal.bind.JsonTreeReader;
import com.google.gson.internal.bind.JsonTreeWriter;
import com.google.gson.stream.JsonReader;
import com.google.gson.stream.JsonWriter;
import java.io.IOException;
import java.util.Iterator;
import java.util.Map;

public class JsonUtils {
    private JsonUtils() {
    }

    public static JsonElement deepCopy(JsonElement from) {
        if (from.isJsonObject()) {
            JsonObject result = new JsonObject();
            for (Map.Entry<String, JsonElement> entry : from.getAsJsonObject().entrySet()) {
                result.add(entry.getKey(), deepCopy(entry.getValue()));
            }
            return result;
        } else if (from.isJsonArray()) {
            JsonArray result2 = new JsonArray();
            Iterator<JsonElement> it = from.getAsJsonArray().iterator();
            while (it.hasNext()) {
                result2.add(it.next());
            }
            return result2;
        } else if (!from.isJsonPrimitive() && !from.isJsonNull()) {
            return JsonNull.INSTANCE;
        } else {
            return from;
        }
    }

    public static JsonElement toJsonTree(TypeAdapter typeAdapter, JsonWriter optionsFrom, Object value) throws IOException {
        JsonTreeWriter jsonTreeWriter = new JsonTreeWriter();
        jsonTreeWriter.setLenient(optionsFrom.isLenient());
        jsonTreeWriter.setHtmlSafe(optionsFrom.isHtmlSafe());
        jsonTreeWriter.setSerializeNulls(optionsFrom.getSerializeNulls());
        typeAdapter.write(jsonTreeWriter, value);
        return jsonTreeWriter.get();
    }

    public static <T> T fromJsonTree(TypeAdapter<T> typeAdapter, JsonReader originalReader, JsonElement element) throws IOException {
        JsonTreeReader jsonTreeReader = new JsonTreeReader(element);
        jsonTreeReader.setLenient(originalReader.isLenient());
        return typeAdapter.read(jsonTreeReader);
    }
}
