package io.gsonfire.postprocessors.methodinvoker;

import io.gsonfire.annotations.ExposeMethodResult;
import java.lang.reflect.Method;

public final class MappedMethod {
    private final ExposeMethodResult.ConflictResolutionStrategy conflictResolutionStrategy;
    private final Method method;
    private final String serializedName;

    public Method getMethod() {
        return this.method;
    }

    public String getSerializedName() {
        return this.serializedName;
    }

    public ExposeMethodResult.ConflictResolutionStrategy getConflictResolutionStrategy() {
        return this.conflictResolutionStrategy;
    }

    public MappedMethod(Method method2, String serializedName2, ExposeMethodResult.ConflictResolutionStrategy conflictResolutionStrategy2) {
        this.method = method2;
        this.serializedName = serializedName2;
        this.conflictResolutionStrategy = conflictResolutionStrategy2;
    }
}
