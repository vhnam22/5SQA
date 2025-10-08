package io.gsonfire.gson;

import com.google.gson.Gson;
import com.google.gson.JsonElement;
import com.google.gson.JsonObject;
import com.google.gson.TypeAdapter;
import com.google.gson.TypeAdapterFactory;
import com.google.gson.reflect.TypeToken;
import com.google.gson.stream.JsonReader;
import com.google.gson.stream.JsonWriter;
import io.gsonfire.annotations.ExcludeByValue;
import io.gsonfire.util.FieldNameResolver;
import io.gsonfire.util.JsonUtils;
import io.gsonfire.util.reflection.Factory;
import io.gsonfire.util.reflection.FieldInspector;
import java.io.IOException;
import java.lang.reflect.Field;
import java.util.Iterator;

public final class ExcludeByValueTypeAdapterFactory implements TypeAdapterFactory {
    /* access modifiers changed from: private */
    public final Factory factory;
    /* access modifiers changed from: private */
    public final FieldInspector fieldInspector;
    /* access modifiers changed from: private */
    public FieldNameResolver fieldNameResolver = null;

    public ExcludeByValueTypeAdapterFactory(FieldInspector fieldInspector2, Factory factory2) {
        this.fieldInspector = fieldInspector2;
        this.factory = factory2;
    }

    public <T> TypeAdapter<T> create(Gson gson, TypeToken<T> type) {
        if (this.fieldNameResolver == null) {
            this.fieldNameResolver = new FieldNameResolver(gson);
        }
        return new ExcludeByValueTypeAdapter(gson, gson.getDelegateAdapter(this, type));
    }

    private class ExcludeByValueTypeAdapter extends TypeAdapter {
        private final Gson gson;
        private final TypeAdapter originalTypeAdapter;

        public ExcludeByValueTypeAdapter(Gson gson2, TypeAdapter originalTypeAdapter2) {
            this.gson = gson2;
            this.originalTypeAdapter = originalTypeAdapter2;
        }

        public void write(JsonWriter out, Object src) throws IOException {
            String fieldName;
            if (src == null) {
                this.originalTypeAdapter.write(out, src);
                return;
            }
            JsonObject postProcessedObject = null;
            Iterator<M> it = ExcludeByValueTypeAdapterFactory.this.fieldInspector.getAnnotatedMembers(src.getClass(), ExcludeByValue.class).iterator();
            while (true) {
                if (!it.hasNext()) {
                    break;
                }
                Field f = (Field) it.next();
                try {
                    if (((ExclusionByValueStrategy) ExcludeByValueTypeAdapterFactory.this.factory.get(((ExcludeByValue) f.getAnnotation(ExcludeByValue.class)).value())).shouldSkipField(f.get(src)) && (fieldName = ExcludeByValueTypeAdapterFactory.this.fieldNameResolver.getFieldName(f)) != null) {
                        if (postProcessedObject == null) {
                            JsonElement originalResult = JsonUtils.toJsonTree(this.originalTypeAdapter, out, src);
                            if (originalResult == null || originalResult.isJsonNull()) {
                                break;
                            } else if (!originalResult.isJsonObject()) {
                                break;
                            } else {
                                postProcessedObject = originalResult.getAsJsonObject();
                            }
                        }
                        postProcessedObject.remove(fieldName);
                    }
                } catch (IllegalAccessException e) {
                    throw new RuntimeException(e);
                }
            }
            if (postProcessedObject != null) {
                this.gson.toJson((JsonElement) postProcessedObject, out);
            } else {
                this.originalTypeAdapter.write(out, src);
            }
        }

        public Object read(JsonReader in) throws IOException {
            return this.originalTypeAdapter.read(in);
        }
    }
}
