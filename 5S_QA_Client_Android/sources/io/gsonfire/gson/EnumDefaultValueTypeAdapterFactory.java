package io.gsonfire.gson;

import com.google.gson.Gson;
import com.google.gson.TypeAdapter;
import com.google.gson.TypeAdapterFactory;
import com.google.gson.reflect.TypeToken;
import com.google.gson.stream.JsonReader;
import com.google.gson.stream.JsonWriter;
import java.io.IOException;
import java.lang.Enum;

public final class EnumDefaultValueTypeAdapterFactory<T extends Enum> implements TypeAdapterFactory {
    private final Class<T> clazz;
    /* access modifiers changed from: private */
    public final T defaultValue;

    public EnumDefaultValueTypeAdapterFactory(Class<T> clazz2, T defaultValue2) {
        this.clazz = clazz2;
        this.defaultValue = defaultValue2;
    }

    public <T> TypeAdapter<T> create(Gson gson, TypeToken<T> type) {
        if (!this.clazz.isAssignableFrom(type.getRawType())) {
            return null;
        }
        final TypeAdapter<T> originalTypeAdapter = gson.getDelegateAdapter(this, type);
        return new NullableTypeAdapter(new TypeAdapter<T>() {
            public void write(JsonWriter jsonWriter, T o) throws IOException {
                originalTypeAdapter.write(jsonWriter, o);
            }

            public T read(JsonReader jsonReader) throws IOException {
                T result = originalTypeAdapter.read(jsonReader);
                if (result == null) {
                    return EnumDefaultValueTypeAdapterFactory.this.defaultValue;
                }
                return result;
            }
        });
    }
}
