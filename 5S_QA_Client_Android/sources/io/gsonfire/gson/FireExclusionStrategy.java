package io.gsonfire.gson;

import io.gsonfire.postprocessors.methodinvoker.MappedMethod;

public interface FireExclusionStrategy {
    boolean shouldSkipMethod(MappedMethod mappedMethod);
}
