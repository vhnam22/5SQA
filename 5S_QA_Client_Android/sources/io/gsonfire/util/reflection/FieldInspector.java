package io.gsonfire.util.reflection;

import java.lang.reflect.Field;

public class FieldInspector extends AnnotationInspector<Field, Field> {
    /* access modifiers changed from: protected */
    public Field[] getDeclaredMembers(Class clazz) {
        return clazz.getDeclaredFields();
    }

    /* access modifiers changed from: protected */
    public Field map(Field member) {
        return member;
    }
}
