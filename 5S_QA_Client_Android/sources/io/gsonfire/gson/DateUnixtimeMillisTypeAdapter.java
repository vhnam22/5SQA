package io.gsonfire.gson;

import java.util.Date;

public class DateUnixtimeMillisTypeAdapter extends DateUnixtimeTypeAdapter {
    public DateUnixtimeMillisTypeAdapter(boolean allowNegativeTimestamp) {
        super(allowNegativeTimestamp);
    }

    /* access modifiers changed from: protected */
    public long toTimestamp(Date date) {
        return date.getTime();
    }

    /* access modifiers changed from: protected */
    public Date fromTimestamp(long timestamp) {
        return new Date(timestamp);
    }
}
