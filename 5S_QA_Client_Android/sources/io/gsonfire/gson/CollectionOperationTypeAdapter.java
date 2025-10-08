package io.gsonfire.gson;

import com.google.gson.JsonArray;
import com.google.gson.JsonElement;
import com.google.gson.TypeAdapter;
import com.google.gson.stream.JsonReader;
import com.google.gson.stream.JsonToken;
import com.google.gson.stream.JsonWriter;
import java.io.IOException;
import java.util.Collection;

final class CollectionOperationTypeAdapter extends TypeAdapter<Collection> {
    private static final JsonElement EMPTY_ARRAY = new JsonArray();
    private final TypeAdapter<Collection> collectionTypeAdapter;

    private enum Operator {
        $add {
            public void apply(Collection to, Collection from) {
                to.addAll(from);
            }
        },
        $remove {
            public void apply(Collection to, Collection from) {
                to.removeAll(from);
            }
        },
        $clear {
            public void apply(Collection to, Collection from) {
                to.clear();
            }
        };

        public abstract void apply(Collection collection, Collection collection2);
    }

    public CollectionOperationTypeAdapter(TypeAdapter<Collection> collectionTypeAdapter2) {
        this.collectionTypeAdapter = collectionTypeAdapter2;
    }

    public void write(JsonWriter out, Collection value) throws IOException {
        this.collectionTypeAdapter.write(out, value);
    }

    public Collection read(JsonReader in) throws IOException {
        Collection operand;
        if (in.peek() != JsonToken.BEGIN_OBJECT) {
            return this.collectionTypeAdapter.read(in);
        }
        Collection res = this.collectionTypeAdapter.fromJsonTree(EMPTY_ARRAY);
        in.beginObject();
        while (in.hasNext()) {
            Operator op = Operator.valueOf(in.nextName());
            if (op == Operator.$clear) {
                operand = null;
            } else {
                operand = this.collectionTypeAdapter.read(in);
            }
            op.apply(res, operand);
        }
        in.endObject();
        return res;
    }
}
