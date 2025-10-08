package io.gsonfire.gson;

import io.gsonfire.postprocessors.methodinvoker.MappedMethod;
import java.util.ArrayList;
import java.util.Arrays;
import java.util.Collection;

public final class FireExclusionStrategyComposite implements FireExclusionStrategy {
    private final Collection<FireExclusionStrategy> strategies;

    public FireExclusionStrategyComposite(FireExclusionStrategy... strategies2) {
        this((Collection<FireExclusionStrategy>) Arrays.asList(strategies2));
    }

    public FireExclusionStrategyComposite(Collection<FireExclusionStrategy> strategies2) {
        this.strategies = new ArrayList(strategies2);
    }

    public boolean shouldSkipMethod(MappedMethod method) {
        for (FireExclusionStrategy strategy : this.strategies) {
            if (strategy.shouldSkipMethod(method)) {
                return true;
            }
        }
        return false;
    }
}
