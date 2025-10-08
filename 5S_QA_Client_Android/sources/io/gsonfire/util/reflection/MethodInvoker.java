package io.gsonfire.util.reflection;

import java.lang.reflect.InvocationTargetException;
import java.lang.reflect.Method;
import java.util.ArrayList;
import java.util.List;
import java.util.Set;

public class MethodInvoker {
    private final List<Class> argsOrder;
    private final Method method;

    public interface ValueSupplier {
        Object getValueForType(Class cls);
    }

    public MethodInvoker(Method method2, Set<Class> supportedInjectionTypes) {
        this.method = method2;
        this.argsOrder = new ArrayList(supportedInjectionTypes.size());
        Class[] parameterTypes = method2.getParameterTypes();
        int length = parameterTypes.length;
        int i = 0;
        while (i < length) {
            Class parameterType = parameterTypes[i];
            if (supportedInjectionTypes.contains(parameterType)) {
                this.argsOrder.add(parameterType);
                i++;
            } else {
                throw new IllegalArgumentException("Cannot auto inject type: " + parameterType);
            }
        }
    }

    public void invoke(Object obj, ValueSupplier supplier) throws InvocationTargetException, IllegalAccessException {
        Object[] args = new Object[this.method.getParameterTypes().length];
        for (int i = 0; i < args.length; i++) {
            args[i] = supplier.getValueForType(this.argsOrder.get(i));
        }
        this.method.invoke(obj, args);
    }
}
