package io.gsonfire.gson;

import com.google.gson.Gson;
import com.google.gson.TypeAdapter;
import com.google.gson.TypeAdapterFactory;
import com.google.gson.reflect.TypeToken;
import io.gsonfire.util.SimpleIterable;
import java.lang.reflect.ParameterizedType;

public final class SimpleIterableTypeAdapterFactory implements TypeAdapterFactory {
    public TypeAdapter create(Gson gson, TypeToken type) {
        if (type.getRawType() != SimpleIterable.class) {
            return null;
        }
        if (type.getType() instanceof ParameterizedType) {
            return new SimpleIterableTypeAdapter(gson, ((ParameterizedType) type.getType()).getActualTypeArguments()[0]);
        }
        return new SimpleIterableTypeAdapter(gson, Object.class);
    }
}
