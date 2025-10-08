package io.gsonfire.gson;

import com.google.gson.Gson;
import com.google.gson.TypeAdapter;
import com.google.gson.TypeAdapterFactory;
import com.google.gson.reflect.TypeToken;
import io.gsonfire.ClassConfig;

public final class HooksTypeAdapterFactory<T> implements TypeAdapterFactory {
    private final ClassConfig<T> classConfig;

    public HooksTypeAdapterFactory(ClassConfig<T> classConfig2) {
        this.classConfig = classConfig2;
    }

    public <T> TypeAdapter<T> create(Gson gson, TypeToken<T> type) {
        if (!this.classConfig.getConfiguredClass().isAssignableFrom(type.getRawType())) {
            return null;
        }
        return new HooksTypeAdapter<>(type.getRawType(), this.classConfig, gson.getDelegateAdapter(this, type), gson);
    }
}
