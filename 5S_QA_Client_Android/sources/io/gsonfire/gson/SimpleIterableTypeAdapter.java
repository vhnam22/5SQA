package io.gsonfire.gson;

import com.google.gson.Gson;
import com.google.gson.TypeAdapter;
import com.google.gson.stream.JsonReader;
import com.google.gson.stream.JsonToken;
import com.google.gson.stream.JsonWriter;
import io.gsonfire.util.SimpleIterable;
import java.io.IOException;
import java.lang.reflect.Type;
import java.util.ArrayList;
import java.util.Collection;
import java.util.Iterator;

public final class SimpleIterableTypeAdapter extends TypeAdapter<SimpleIterable<?>> {
    private final Gson gson;
    private final Type type;

    public SimpleIterableTypeAdapter(Gson gson2, Type type2) {
        this.gson = gson2;
        this.type = type2;
    }

    public void write(JsonWriter out, SimpleIterable<?> iterable) throws IOException {
        if (iterable != null) {
            out.beginArray();
            Iterator<?> it = iterable.iterator();
            while (it.hasNext()) {
                Object v = it.next();
                this.gson.toJson(v, (Type) v.getClass(), out);
            }
            out.endArray();
            return;
        }
        out.nullValue();
    }

    public SimpleIterable<?> read(JsonReader in) throws IOException {
        if (in.peek() == JsonToken.NULL) {
            return null;
        }
        Collection result = new ArrayList();
        in.beginArray();
        while (in.hasNext()) {
            result.add(this.gson.fromJson(in, this.type));
        }
        in.endArray();
        return SimpleIterable.of(result);
    }
}
