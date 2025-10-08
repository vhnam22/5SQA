package io.gsonfire;

import java.util.ArrayList;
import java.util.Collection;

public final class ClassConfig<T> {
    private Class<T> clazz;
    private boolean hooksEnabled;
    private Collection<PostProcessor<T>> postProcessors;
    private Collection<PreProcessor<T>> preProcessors;
    private TypeSelector<? super T> typeSelector;

    public ClassConfig(Class<T> clazz2) {
        this.clazz = clazz2;
    }

    public Class<T> getConfiguredClass() {
        return this.clazz;
    }

    public TypeSelector<? super T> getTypeSelector() {
        return this.typeSelector;
    }

    public void setTypeSelector(TypeSelector<? super T> typeSelector2) {
        this.typeSelector = typeSelector2;
    }

    public Collection<PostProcessor<T>> getPostProcessors() {
        if (this.postProcessors == null) {
            this.postProcessors = new ArrayList();
        }
        return this.postProcessors;
    }

    public Collection<PreProcessor<T>> getPreProcessors() {
        if (this.preProcessors == null) {
            this.preProcessors = new ArrayList();
        }
        return this.preProcessors;
    }

    public boolean isHooksEnabled() {
        return this.hooksEnabled;
    }

    public void setHooksEnabled(boolean hooksEnabled2) {
        this.hooksEnabled = hooksEnabled2;
    }
}
