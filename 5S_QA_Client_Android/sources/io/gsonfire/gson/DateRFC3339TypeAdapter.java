package io.gsonfire.gson;

import com.google.gson.TypeAdapter;
import com.google.gson.stream.JsonReader;
import com.google.gson.stream.JsonWriter;
import io.gsonfire.util.RFC3339DateFormat;
import java.io.IOException;
import java.text.DateFormat;
import java.text.ParseException;
import java.util.Date;
import java.util.TimeZone;

public final class DateRFC3339TypeAdapter extends TypeAdapter<Date> {
    private final ThreadLocal<DateFormat> dateFormatThreadLocal;
    private final TimeZone serializationTimezone;
    private final boolean serializeTime;

    public DateRFC3339TypeAdapter(boolean serializeTime2) {
        this(TimeZone.getDefault(), serializeTime2);
    }

    public DateRFC3339TypeAdapter(TimeZone serializationTimezone2, boolean serializeTime2) {
        this.dateFormatThreadLocal = new ThreadLocal<>();
        this.serializationTimezone = serializationTimezone2;
        this.serializeTime = serializeTime2;
    }

    private DateFormat getDateFormat() {
        DateFormat existingDateFormat = this.dateFormatThreadLocal.get();
        if (existingDateFormat != null) {
            return existingDateFormat;
        }
        DateFormat newDateFormat = new RFC3339DateFormat(this.serializationTimezone, this.serializeTime);
        this.dateFormatThreadLocal.set(newDateFormat);
        return newDateFormat;
    }

    public void write(JsonWriter out, Date value) throws IOException {
        out.value(getDateFormat().format(value));
    }

    public Date read(JsonReader in) throws IOException {
        String dateStr = in.nextString();
        try {
            return getDateFormat().parse(dateStr);
        } catch (ParseException e) {
            throw new IOException("Could not parse date " + dateStr, e);
        }
    }
}
