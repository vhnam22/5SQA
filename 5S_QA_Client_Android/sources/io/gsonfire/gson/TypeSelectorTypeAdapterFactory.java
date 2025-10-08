package io.gsonfire.gson;

import com.google.gson.Gson;
import com.google.gson.JsonElement;
import com.google.gson.JsonParser;
import com.google.gson.TypeAdapter;
import com.google.gson.TypeAdapterFactory;
import com.google.gson.reflect.TypeToken;
import com.google.gson.stream.JsonReader;
import com.google.gson.stream.JsonWriter;
import io.gsonfire.ClassConfig;
import io.gsonfire.TypeSelector;
import io.gsonfire.util.JsonUtils;
import java.io.IOException;
import java.util.Set;

public class TypeSelectorTypeAdapterFactory<T> implements TypeAdapterFactory {
    /* access modifiers changed from: private */
    public final Set<TypeToken> alreadyResolvedTypeTokensRegistry;
    private final ClassConfig<T> classConfig;

    public TypeSelectorTypeAdapterFactory(ClassConfig<T> classConfig2, Set<TypeToken> alreadyResolvedTypeTokensRegistry2) {
        this.classConfig = classConfig2;
        this.alreadyResolvedTypeTokensRegistry = alreadyResolvedTypeTokensRegistry2;
    }

    public <T> TypeAdapter<T> create(Gson gson, TypeToken<T> type) {
        if (this.alreadyResolvedTypeTokensRegistry.contains(type) || !this.classConfig.getConfiguredClass().isAssignableFrom(type.getRawType())) {
            return null;
        }
        return new NullableTypeAdapter<>(new TypeSelectorTypeAdapter(type.getRawType(), this.classConfig.getTypeSelector(), gson));
    }

    private class TypeSelectorTypeAdapter<T> extends TypeAdapter<T> {
        private final Gson gson;
        private final Class superClass;
        private final TypeSelector typeSelector;

        private TypeSelectorTypeAdapter(Class superClass2, TypeSelector typeSelector2, Gson gson2) {
            this.superClass = superClass2;
            this.typeSelector = typeSelector2;
            this.gson = gson2;
        }

        public void write(JsonWriter out, T value) throws IOException {
            this.gson.getDelegateAdapter(TypeSelectorTypeAdapterFactory.this, TypeToken.get(value.getClass())).write(out, value);
        }

        /* JADX INFO: finally extract failed */
        public T read(JsonReader in) throws IOException {
            TypeAdapter<T> otherTypeAdapter;
            JsonElement json = new JsonParser().parse(in);
            Class deserialize = this.typeSelector.getClassForElement(json);
            if (deserialize == null) {
                deserialize = this.superClass;
            }
            TypeToken typeToken = TypeToken.get(deserialize);
            TypeSelectorTypeAdapterFactory.this.alreadyResolvedTypeTokensRegistry.add(typeToken);
            try {
                if (deserialize != this.superClass) {
                    otherTypeAdapter = this.gson.getAdapter(typeToken);
                } else {
                    otherTypeAdapter = this.gson.getDelegateAdapter(TypeSelectorTypeAdapterFactory.this, typeToken);
                }
                TypeSelectorTypeAdapterFactory.this.alreadyResolvedTypeTokensRegistry.remove(typeToken);
                return JsonUtils.fromJsonTree(otherTypeAdapter, in, json);
            } catch (Throwable th) {
                TypeSelectorTypeAdapterFactory.this.alreadyResolvedTypeTokensRegistry.remove(typeToken);
                throw th;
            }
        }
    }
}
