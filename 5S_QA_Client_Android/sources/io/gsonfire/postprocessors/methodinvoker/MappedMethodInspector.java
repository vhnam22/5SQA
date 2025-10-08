package io.gsonfire.postprocessors.methodinvoker;

import io.gsonfire.annotations.ExposeMethodResult;
import io.gsonfire.util.reflection.AnnotationInspector;
import java.lang.reflect.Method;

final class MappedMethodInspector extends AnnotationInspector<Method, MappedMethod> {
    MappedMethodInspector() {
    }

    /* access modifiers changed from: protected */
    public Method[] getDeclaredMembers(Class clazz) {
        return clazz.getDeclaredMethods();
    }

    /* access modifiers changed from: protected */
    public MappedMethod map(Method member) {
        if (member.getParameterTypes().length <= 0) {
            ExposeMethodResult exposeMethodResult = (ExposeMethodResult) member.getAnnotation(ExposeMethodResult.class);
            return new MappedMethod(member, exposeMethodResult.value(), exposeMethodResult.conflictResolution());
        }
        throw new IllegalArgumentException("The methods annotated with ExposeMethodResult should have no arguments");
    }
}
