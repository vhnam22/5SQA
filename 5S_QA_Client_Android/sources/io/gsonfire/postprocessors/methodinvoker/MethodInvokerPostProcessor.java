package io.gsonfire.postprocessors.methodinvoker;

import com.google.gson.Gson;
import com.google.gson.JsonElement;
import com.google.gson.JsonObject;
import io.gsonfire.PostProcessor;
import io.gsonfire.annotations.ExposeMethodResult;
import io.gsonfire.gson.FireExclusionStrategy;
import io.gsonfire.gson.FireExclusionStrategyComposite;
import java.lang.reflect.InvocationTargetException;

public final class MethodInvokerPostProcessor<T> implements PostProcessor<T> {
    private static MappedMethodInspector methodInspector = new MappedMethodInspector();
    private final FireExclusionStrategy serializationExclusionStrategy;

    public MethodInvokerPostProcessor() {
        this(new FireExclusionStrategyComposite(new FireExclusionStrategy[0]));
    }

    public MethodInvokerPostProcessor(FireExclusionStrategy serializationExclusionStrategy2) {
        this.serializationExclusionStrategy = serializationExclusionStrategy2;
    }

    public void postDeserialize(T t, JsonElement src, Gson gson) {
    }

    public void postSerialize(JsonElement result, T src, Gson gson) {
        if (result.isJsonObject()) {
            JsonObject jsonObject = result.getAsJsonObject();
            for (M m : methodInspector.getAnnotatedMembers(src.getClass(), ExposeMethodResult.class)) {
                if (!this.serializationExclusionStrategy.shouldSkipMethod(m)) {
                    try {
                        if (m.getConflictResolutionStrategy() == ExposeMethodResult.ConflictResolutionStrategy.OVERWRITE || (m.getConflictResolutionStrategy() == ExposeMethodResult.ConflictResolutionStrategy.SKIP && !jsonObject.has(m.getSerializedName()))) {
                            jsonObject.add(m.getSerializedName(), gson.toJsonTree(m.getMethod().invoke(src, new Object[0])));
                        }
                    } catch (IllegalAccessException e) {
                        e.printStackTrace();
                    } catch (InvocationTargetException e2) {
                        e2.printStackTrace();
                    }
                }
            }
        }
    }
}
