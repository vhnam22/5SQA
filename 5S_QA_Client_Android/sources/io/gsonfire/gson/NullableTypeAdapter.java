package io.gsonfire.gson;

import com.google.gson.TypeAdapter;
import com.google.gson.stream.JsonReader;
import com.google.gson.stream.JsonToken;
import com.google.gson.stream.JsonWriter;
import java.io.IOException;

public final class NullableTypeAdapter<T> extends TypeAdapter<T> {
    private final TypeAdapter<T> nullable;

    public NullableTypeAdapter(TypeAdapter<T> nullable2) {
        this.nullable = nullable2;
    }

    public void write(JsonWriter out, T value) throws IOException {
        if (value == null) {
            out.nullValue();
        } else {
            this.nullable.write(out, value);
        }
    }

    public T read(JsonReader in) throws IOException {
        if (in.peek() != JsonToken.NULL) {
            return this.nullable.read(in);
        }
        in.nextNull();
        return null;
    }
}
