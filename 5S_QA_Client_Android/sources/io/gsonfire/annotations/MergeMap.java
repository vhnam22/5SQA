package io.gsonfire.annotations;

import java.lang.annotation.ElementType;
import java.lang.annotation.Retention;
import java.lang.annotation.RetentionPolicy;
import java.lang.annotation.Target;

@Deprecated
@Target({ElementType.FIELD})
@Retention(RetentionPolicy.RUNTIME)
public @interface MergeMap {
}
