package io.gsonfire.gson;

import com.google.gson.Gson;
import com.google.gson.JsonElement;
import com.google.gson.JsonObject;
import com.google.gson.TypeAdapter;
import com.google.gson.TypeAdapterFactory;
import com.google.gson.reflect.TypeToken;
import com.google.gson.stream.JsonReader;
import com.google.gson.stream.JsonWriter;
import io.gsonfire.util.JsonUtils;
import io.gsonfire.util.Mapper;
import java.io.IOException;
import java.util.Map;

public class WrapTypeAdapterFactory<T> implements TypeAdapterFactory {
    private final Map<Class<T>, Mapper<T, String>> wrappedClasses;

    public WrapTypeAdapterFactory(Map<Class<T>, Mapper<T, String>> wrappedClasses2) {
        this.wrappedClasses = wrappedClasses2;
    }

    public <T> TypeAdapter<T> create(Gson gson, TypeToken<T> type) {
        TypeAdapter<T> originalTypeAdapter = gson.getDelegateAdapter(this, type);
        Mapper<T, String> mapper = getMostSpecificMapper(type.getRawType());
        if (mapper == null) {
            return originalTypeAdapter;
        }
        return new NullableTypeAdapter(new WrapperTypeAdapter(mapper, gson, originalTypeAdapter));
    }

    private Mapper<T, String> getMostSpecificMapper(Class clazz) {
        for (Class mostSpecificClass = clazz; mostSpecificClass != null; mostSpecificClass = mostSpecificClass.getSuperclass()) {
            Mapper<T, String> mostSpecificMapper = this.wrappedClasses.get(mostSpecificClass);
            if (mostSpecificMapper != null) {
                return mostSpecificMapper;
            }
        }
        return null;
    }

    private class WrapperTypeAdapter<T> extends TypeAdapter<T> {
        private final Gson gson;
        private final Mapper<T, String> mapper;
        private final TypeAdapter<T> originalTypeAdapter;

        public WrapperTypeAdapter(Mapper<T, String> mapper2, Gson gson2, TypeAdapter<T> originalTypeAdapter2) {
            this.mapper = mapper2;
            this.gson = gson2;
            this.originalTypeAdapter = originalTypeAdapter2;
        }

        public void write(JsonWriter out, T src) throws IOException {
            if (src == null) {
                this.originalTypeAdapter.write(out, src);
                return;
            }
            JsonElement unwrappedObj = JsonUtils.toJsonTree(this.originalTypeAdapter, out, src);
            JsonObject wrappedObj = new JsonObject();
            wrappedObj.add(this.mapper.map(src), unwrappedObj);
            this.gson.toJson((JsonElement) wrappedObj, out);
        }

        public T read(JsonReader in) throws IOException {
            in.beginObject();
            in.nextName();
            T unwrappedObj = this.originalTypeAdapter.read(in);
            in.endObject();
            return unwrappedObj;
        }
    }
}
