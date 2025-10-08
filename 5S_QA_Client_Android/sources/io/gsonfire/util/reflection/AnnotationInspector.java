package io.gsonfire.util.reflection;

import java.lang.annotation.Annotation;
import java.lang.reflect.AccessibleObject;
import java.util.Collection;
import java.util.Collections;
import java.util.LinkedHashSet;
import java.util.Map;
import java.util.Set;
import java.util.concurrent.ConcurrentHashMap;
import java.util.concurrent.ConcurrentMap;

public abstract class AnnotationInspector<T extends AccessibleObject, M> {
    private final ConcurrentMap<Class, ConcurrentMap<Class<? extends Annotation>, Collection<M>>> cache = new ConcurrentHashMap();

    /* access modifiers changed from: protected */
    public abstract T[] getDeclaredMembers(Class cls);

    /* access modifiers changed from: protected */
    public abstract M map(T t);

    public Collection<M> getAnnotatedMembers(Class clazz, Class<? extends Annotation> annotation) {
        if (clazz != null) {
            Collection<M> members = getFromCache(clazz, annotation);
            if (members != null) {
                return members;
            }
            if (getFromCache(clazz, annotation) == null) {
                Set<M> memberList = new LinkedHashSet<>();
                for (T m : getDeclaredMembers(clazz)) {
                    if (m.isAnnotationPresent(annotation)) {
                        m.setAccessible(true);
                        memberList.add(map(m));
                    }
                }
                memberList.addAll(getAnnotatedMembers(clazz.getSuperclass(), annotation));
                for (Class interfaceClass : clazz.getInterfaces()) {
                    memberList.addAll(getAnnotatedMembers(interfaceClass, annotation));
                }
                ConcurrentMap<Class<? extends Annotation>, Collection<M>> concurrentHashMap = new ConcurrentHashMap<>();
                ConcurrentMap<Class<? extends Annotation>, Collection<M>> storedAnnotationMap = this.cache.putIfAbsent(clazz, concurrentHashMap);
                (storedAnnotationMap == null ? concurrentHashMap : storedAnnotationMap).put(annotation, memberList);
                return memberList;
            }
        }
        return Collections.emptyList();
    }

    private Collection<M> getFromCache(Class clazz, Class<? extends Annotation> annotation) {
        Collection<M> methods;
        Map<Class<? extends Annotation>, Collection<M>> annotationMap = (Map) this.cache.get(clazz);
        if (annotationMap == null || (methods = annotationMap.get(annotation)) == null) {
            return null;
        }
        return methods;
    }
}
