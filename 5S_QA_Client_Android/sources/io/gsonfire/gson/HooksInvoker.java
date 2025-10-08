package io.gsonfire.gson;

import com.google.gson.Gson;
import com.google.gson.JsonElement;
import io.gsonfire.annotations.PostDeserialize;
import io.gsonfire.annotations.PreSerialize;
import io.gsonfire.util.reflection.AbstractMethodInspector;
import io.gsonfire.util.reflection.MethodInvoker;
import java.lang.annotation.Annotation;
import java.lang.reflect.InvocationTargetException;
import java.lang.reflect.Method;
import java.util.Arrays;
import java.util.HashSet;
import java.util.Set;

public final class HooksInvoker {
    /* access modifiers changed from: private */
    public static final Set<Class> SUPPORTED_TYPES = new HashSet(Arrays.asList(new Class[]{JsonElement.class, Gson.class}));
    private AbstractMethodInspector<MethodInvoker> inspector = new AbstractMethodInspector<MethodInvoker>() {
        /* access modifiers changed from: protected */
        public MethodInvoker map(Method member) {
            return new MethodInvoker(member, HooksInvoker.SUPPORTED_TYPES);
        }
    };

    public void preSerialize(Object obj) {
        invokeAll(obj, PreSerialize.class, (JsonElement) null, (Gson) null);
    }

    public void postDeserialize(Object obj, JsonElement jsonElement, Gson gson) {
        invokeAll(obj, PostDeserialize.class, jsonElement, gson);
    }

    private void invokeAll(Object obj, Class<? extends Annotation> annotation, JsonElement jsonElement, Gson gson) {
        if (obj != null) {
            for (M m : this.inspector.getAnnotatedMembers(obj.getClass(), annotation)) {
                try {
                    m.invoke(obj, new HooksInvokerValueSupplier(jsonElement, gson));
                } catch (IllegalAccessException e) {
                    throw new HookInvocationException("Exception during hook invocation: " + annotation.getSimpleName(), e);
                } catch (InvocationTargetException e2) {
                    throw new HookInvocationException("Exception during hook invocation: " + annotation.getSimpleName(), e2.getTargetException());
                }
            }
        }
    }

    private static class HooksInvokerValueSupplier implements MethodInvoker.ValueSupplier {
        private final Gson gson;
        private final JsonElement jsonElement;

        private HooksInvokerValueSupplier(JsonElement jsonElement2, Gson gson2) {
            this.jsonElement = jsonElement2;
            this.gson = gson2;
        }

        public Object getValueForType(Class type) {
            if (type == JsonElement.class) {
                return this.jsonElement;
            }
            if (type == Gson.class) {
                return this.gson;
            }
            return null;
        }
    }
}
