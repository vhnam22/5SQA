package io.gsonfire;

import com.google.gson.TypeAdapter;
import io.gsonfire.gson.DateRFC3339TypeAdapter;
import io.gsonfire.gson.DateUnixtimeMillisTypeAdapter;
import io.gsonfire.gson.DateUnixtimeSecondsTypeAdapter;
import io.gsonfire.gson.NullableTypeAdapter;
import java.util.Date;
import java.util.TimeZone;

public enum DateSerializationPolicy {
    unixTimeMillis {
        /* access modifiers changed from: package-private */
        public TypeAdapter<Date> createTypeAdapter(TimeZone serializeTimezone) {
            return new NullableTypeAdapter(new DateUnixtimeMillisTypeAdapter(true));
        }
    },
    unixTimeSeconds {
        /* access modifiers changed from: package-private */
        public TypeAdapter<Date> createTypeAdapter(TimeZone serializeTimezone) {
            return new NullableTypeAdapter(new DateUnixtimeSecondsTypeAdapter(true));
        }
    },
    unixTimePositiveMillis {
        /* access modifiers changed from: package-private */
        public TypeAdapter<Date> createTypeAdapter(TimeZone serializeTimezone) {
            return new NullableTypeAdapter(new DateUnixtimeMillisTypeAdapter(false));
        }
    },
    unixTimePositiveSeconds {
        /* access modifiers changed from: package-private */
        public TypeAdapter<Date> createTypeAdapter(TimeZone serializeTimezone) {
            return new NullableTypeAdapter(new DateUnixtimeSecondsTypeAdapter(false));
        }
    },
    rfc3339 {
        /* access modifiers changed from: package-private */
        public TypeAdapter<Date> createTypeAdapter(TimeZone serializeTimezone) {
            return new NullableTypeAdapter(new DateRFC3339TypeAdapter(serializeTimezone, true));
        }
    },
    rfc3339Date {
        /* access modifiers changed from: package-private */
        public TypeAdapter<Date> createTypeAdapter(TimeZone serializeTimezone) {
            return new NullableTypeAdapter(new DateRFC3339TypeAdapter(serializeTimezone, false));
        }
    };

    /* access modifiers changed from: package-private */
    public abstract TypeAdapter<Date> createTypeAdapter(TimeZone timeZone);
}
