package io.gsonfire.postprocessors;

import com.google.gson.Gson;
import com.google.gson.JsonElement;
import com.google.gson.JsonObject;
import io.gsonfire.PostProcessor;
import io.gsonfire.annotations.MergeMap;
import io.gsonfire.util.reflection.FieldInspector;
import java.util.Map;

@Deprecated
public final class MergeMapPostProcessor implements PostProcessor {
    private final FieldInspector fieldInspector;

    public MergeMapPostProcessor(FieldInspector fieldInspector2) {
        this.fieldInspector = fieldInspector2;
    }

    public void postDeserialize(Object result, JsonElement src, Gson gson) {
    }

    public void postSerialize(JsonElement result, Object src, Gson gson) {
        if (src != null) {
            for (M f : this.fieldInspector.getAnnotatedMembers(src.getClass(), MergeMap.class)) {
                try {
                    JsonObject resultJsonObject = result.getAsJsonObject();
                    for (Map.Entry<String, JsonElement> entry : gson.toJsonTree((Map) f.get(src)).getAsJsonObject().entrySet()) {
                        resultJsonObject.add(entry.getKey(), entry.getValue());
                    }
                } catch (IllegalAccessException e) {
                    throw new RuntimeException(e);
                }
            }
        }
    }
}
