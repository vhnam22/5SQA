package io.gsonfire.gson;

import com.google.gson.Gson;
import com.google.gson.JsonElement;
import com.google.gson.JsonParser;
import com.google.gson.TypeAdapter;
import com.google.gson.internal.bind.JsonTreeReader;
import com.google.gson.stream.JsonReader;
import com.google.gson.stream.JsonWriter;
import io.gsonfire.ClassConfig;
import io.gsonfire.PostProcessor;
import io.gsonfire.PreProcessor;
import io.gsonfire.util.JsonUtils;
import java.io.IOException;

public final class HooksTypeAdapter<T> extends TypeAdapter<T> {
    private final ClassConfig<? super T> classConfig;
    private final Class<T> clazz;
    private final Gson gson;
    private final HooksInvoker hooksInvoker = new HooksInvoker();
    private final TypeAdapter<T> originalTypeAdapter;

    public HooksTypeAdapter(Class<T> classAdapter, ClassConfig<? super T> classConfig2, TypeAdapter<T> originalTypeAdapter2, Gson gson2) {
        this.classConfig = classConfig2;
        this.gson = gson2;
        this.originalTypeAdapter = originalTypeAdapter2;
        this.clazz = classAdapter;
    }

    public void write(JsonWriter out, T value) throws IOException {
        if (this.classConfig.isHooksEnabled()) {
            this.hooksInvoker.preSerialize(value);
        }
        JsonElement res = JsonUtils.toJsonTree(this.originalTypeAdapter, out, value);
        runPostSerialize(res, value);
        this.gson.toJson(res, out);
    }

    public T read(JsonReader in) throws IOException {
        JsonElement json = new JsonParser().parse(in);
        runPreDeserialize(json);
        T result = deserialize(json, in.isLenient());
        if (this.classConfig.isHooksEnabled()) {
            this.hooksInvoker.postDeserialize(result, json, this.gson);
        }
        runPostDeserialize(result, json);
        return result;
    }

    private void runPostSerialize(JsonElement json, T src) {
        for (PostProcessor<? super T> postProcessor : this.classConfig.getPostProcessors()) {
            postProcessor.postSerialize(json, src, this.gson);
        }
    }

    private void runPostDeserialize(T res, JsonElement src) {
        for (PostProcessor<? super T> postProcessor : this.classConfig.getPostProcessors()) {
            postProcessor.postDeserialize(res, src, this.gson);
        }
    }

    private void runPreDeserialize(JsonElement json) {
        for (PreProcessor<? super T> preProcessor : this.classConfig.getPreProcessors()) {
            preProcessor.preDeserialize(this.clazz, json, this.gson);
        }
    }

    private T deserialize(JsonElement json, boolean lenient) throws IOException {
        JsonReader jsonReader = new JsonTreeReader(json);
        jsonReader.setLenient(lenient);
        return this.originalTypeAdapter.read(jsonReader);
    }
}
