package io.gsonfire.gson;

import com.google.gson.TypeAdapter;
import com.google.gson.stream.JsonReader;
import com.google.gson.stream.JsonWriter;
import java.io.IOException;
import java.util.Date;

abstract class DateUnixtimeTypeAdapter extends TypeAdapter<Date> {
    private final boolean allowNegativeTimestamp;

    /* access modifiers changed from: protected */
    public abstract Date fromTimestamp(long j);

    /* access modifiers changed from: protected */
    public abstract long toTimestamp(Date date);

    public DateUnixtimeTypeAdapter(boolean allowNegativeTimestamp2) {
        this.allowNegativeTimestamp = allowNegativeTimestamp2;
    }

    public final void write(JsonWriter out, Date value) throws IOException {
        if (value.getTime() >= 0 || this.allowNegativeTimestamp) {
            out.value(toTimestamp(value));
        } else {
            out.nullValue();
        }
    }

    public final Date read(JsonReader in) throws IOException {
        long time = in.nextLong();
        if (time >= 0 || this.allowNegativeTimestamp) {
            return fromTimestamp(time);
        }
        return null;
    }
}
