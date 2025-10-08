package io.gsonfire.gson;

import java.util.Date;

public final class DateUnixtimeSecondsTypeAdapter extends DateUnixtimeTypeAdapter {
    public DateUnixtimeSecondsTypeAdapter(boolean allowNegativeTimestamp) {
        super(allowNegativeTimestamp);
    }

    /* access modifiers changed from: protected */
    public long toTimestamp(Date date) {
        return date.getTime() / 1000;
    }

    /* access modifiers changed from: protected */
    public Date fromTimestamp(long timestamp) {
        return new Date(1000 * timestamp);
    }
}
