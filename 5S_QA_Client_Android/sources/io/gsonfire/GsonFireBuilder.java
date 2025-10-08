package io.gsonfire;

import com.google.gson.Gson;
import com.google.gson.GsonBuilder;
import com.google.gson.reflect.TypeToken;
import io.gsonfire.gson.EnumDefaultValueTypeAdapterFactory;
import io.gsonfire.gson.ExcludeByValueTypeAdapterFactory;
import io.gsonfire.gson.FireExclusionStrategy;
import io.gsonfire.gson.FireExclusionStrategyComposite;
import io.gsonfire.gson.HooksTypeAdapterFactory;
import io.gsonfire.gson.SimpleIterableTypeAdapterFactory;
import io.gsonfire.gson.TypeSelectorTypeAdapterFactory;
import io.gsonfire.gson.WrapTypeAdapterFactory;
import io.gsonfire.postprocessors.MergeMapPostProcessor;
import io.gsonfire.postprocessors.methodinvoker.MethodInvokerPostProcessor;
import io.gsonfire.util.Mapper;
import io.gsonfire.util.reflection.CachedReflectionFactory;
import io.gsonfire.util.reflection.Factory;
import io.gsonfire.util.reflection.FieldInspector;
import java.util.ArrayList;
import java.util.Collection;
import java.util.Collections;
import java.util.Date;
import java.util.HashMap;
import java.util.List;
import java.util.Map;
import java.util.Set;
import java.util.TimeZone;
import java.util.concurrent.ConcurrentHashMap;

public final class GsonFireBuilder {
    private final Map<Class, ClassConfig> classConfigMap = new HashMap();
    private boolean dateDeserializationStrict = true;
    private DateSerializationPolicy dateSerializationPolicy;
    private boolean enableExclusionByValueStrategies = false;
    private boolean enableExposeMethodResults = false;
    private final Map<Class, Enum> enumDefaultValues = new HashMap();
    private final Factory factory = new CachedReflectionFactory();
    private final FieldInspector fieldInspector = new FieldInspector();
    private final List<Class> orderedClasses = new ArrayList();
    private final List<FireExclusionStrategy> serializationExclusions = new ArrayList();
    private TimeZone serializeTimeZone = TimeZone.getDefault();
    private final Map<Class, Mapper> wrappedClasses = new HashMap();

    private ClassConfig getClassConfig(Class clazz) {
        ClassConfig result = this.classConfigMap.get(clazz);
        if (result != null) {
            return result;
        }
        ClassConfig result2 = new ClassConfig(clazz);
        this.classConfigMap.put(clazz, result2);
        insertOrdered(this.orderedClasses, clazz);
        return result2;
    }

    private static void insertOrdered(List<Class> classes, Class clazz) {
        for (int i = classes.size() - 1; i >= 0; i--) {
            if (classes.get(i).isAssignableFrom(clazz)) {
                classes.add(i + 1, clazz);
                return;
            }
        }
        classes.add(0, clazz);
    }

    public <T> GsonFireBuilder registerTypeSelector(Class<T> clazz, TypeSelector<T> factory2) {
        getClassConfig(clazz).setTypeSelector(factory2);
        return this;
    }

    public <T> GsonFireBuilder registerPostProcessor(Class<T> clazz, PostProcessor<? super T> postProcessor) {
        getClassConfig(clazz).getPostProcessors().add(postProcessor);
        return this;
    }

    public <T> GsonFireBuilder registerPreProcessor(Class<T> clazz, PreProcessor<? super T> preProcessor) {
        getClassConfig(clazz).getPreProcessors().add(preProcessor);
        return this;
    }

    public GsonFireBuilder dateSerializationPolicy(DateSerializationPolicy policy) {
        this.dateSerializationPolicy = policy;
        return this;
    }

    public <T> GsonFireBuilder wrap(Class<T> clazz, final String name) {
        wrap(clazz, new Mapper<T, String>() {
            public String map(Object from) {
                return name;
            }
        });
        return this;
    }

    public <T> GsonFireBuilder wrap(Class<T> clazz, Mapper<T, String> mapper) {
        this.wrappedClasses.put(clazz, mapper);
        return this;
    }

    public GsonFireBuilder enableExposeMethodResult() {
        this.enableExposeMethodResults = true;
        return this;
    }

    public GsonFireBuilder enableExclusionByValue() {
        this.enableExclusionByValueStrategies = true;
        return this;
    }

    public GsonFireBuilder enableHooks(Class clazz) {
        getClassConfig(clazz).setHooksEnabled(true);
        return this;
    }

    @Deprecated
    public GsonFireBuilder enableMergeMaps(Class clazz) {
        registerPostProcessor(clazz, new MergeMapPostProcessor(this.fieldInspector));
        return this;
    }

    public GsonFireBuilder serializeTimeZone(TimeZone timeZone) {
        this.serializeTimeZone = timeZone;
        return this;
    }

    public <T extends Enum> GsonFireBuilder enumDefaultValue(Class<T> enumClass, T defaultValue) {
        this.enumDefaultValues.put(enumClass, defaultValue);
        return this;
    }

    public GsonFireBuilder addSerializationExclusionStrategy(FireExclusionStrategy exclusionStrategy) {
        this.serializationExclusions.add(exclusionStrategy);
        return this;
    }

    public GsonBuilder createGsonBuilder() {
        Set<TypeToken> alreadyResolvedTypeTokensRegistry = Collections.newSetFromMap(new ConcurrentHashMap());
        GsonBuilder builder = new GsonBuilder();
        if (this.enableExposeMethodResults) {
            registerPostProcessor(Object.class, new MethodInvokerPostProcessor(new FireExclusionStrategyComposite((Collection<FireExclusionStrategy>) this.serializationExclusions)));
        }
        if (this.enableExclusionByValueStrategies) {
            builder.registerTypeAdapterFactory(new ExcludeByValueTypeAdapterFactory(this.fieldInspector, this.factory));
        }
        for (Class clazz : this.orderedClasses) {
            ClassConfig config = this.classConfigMap.get(clazz);
            if (config.getTypeSelector() != null) {
                builder.registerTypeAdapterFactory(new TypeSelectorTypeAdapterFactory(config, alreadyResolvedTypeTokensRegistry));
            }
            builder.registerTypeAdapterFactory(new HooksTypeAdapterFactory(config));
        }
        for (Map.Entry<Class, Enum> enumDefault : this.enumDefaultValues.entrySet()) {
            builder.registerTypeAdapterFactory(new EnumDefaultValueTypeAdapterFactory(enumDefault.getKey(), enumDefault.getValue()));
        }
        DateSerializationPolicy dateSerializationPolicy2 = this.dateSerializationPolicy;
        if (dateSerializationPolicy2 != null) {
            builder.registerTypeAdapter(Date.class, dateSerializationPolicy2.createTypeAdapter(this.serializeTimeZone));
        }
        builder.registerTypeAdapterFactory(new SimpleIterableTypeAdapterFactory());
        builder.registerTypeAdapterFactory(new WrapTypeAdapterFactory(this.wrappedClasses));
        return builder;
    }

    public Gson createGson() {
        return createGsonBuilder().create();
    }
}
