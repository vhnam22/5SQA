package io.gsonfire.util.reflection;

import java.lang.reflect.Method;

public abstract class AbstractMethodInspector<M> extends AnnotationInspector<Method, M> {
    /* access modifiers changed from: protected */
    public Method[] getDeclaredMembers(Class clazz) {
        return clazz.getDeclaredMethods();
    }
}
