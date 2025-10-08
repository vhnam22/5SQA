package org.threeten.bp.format;

import java.text.DateFormat;
import java.text.SimpleDateFormat;
import java.util.Locale;
import java.util.concurrent.ConcurrentHashMap;
import java.util.concurrent.ConcurrentMap;
import org.threeten.bp.chrono.Chronology;

final class SimpleDateTimeFormatStyleProvider extends DateTimeFormatStyleProvider {
    private static final ConcurrentMap<String, Object> FORMATTER_CACHE = new ConcurrentHashMap(16, 0.75f, 2);

    SimpleDateTimeFormatStyleProvider() {
    }

    public Locale[] getAvailableLocales() {
        return DateFormat.getAvailableLocales();
    }

    public DateTimeFormatter getFormatter(FormatStyle dateStyle, FormatStyle timeStyle, Chronology chrono, Locale locale) {
        DateFormat dateFormat;
        if (dateStyle == null && timeStyle == null) {
            throw new IllegalArgumentException("Date and Time style must not both be null");
        }
        String key = chrono.getId() + '|' + locale.toString() + '|' + dateStyle + timeStyle;
        ConcurrentMap<String, Object> concurrentMap = FORMATTER_CACHE;
        Object cached = concurrentMap.get(key);
        if (cached == null) {
            if (dateStyle == null) {
                dateFormat = DateFormat.getTimeInstance(convertStyle(timeStyle), locale);
            } else if (timeStyle != null) {
                dateFormat = DateFormat.getDateTimeInstance(convertStyle(dateStyle), convertStyle(timeStyle), locale);
            } else {
                dateFormat = DateFormat.getDateInstance(convertStyle(dateStyle), locale);
            }
            if (dateFormat instanceof SimpleDateFormat) {
                DateTimeFormatter formatter = new DateTimeFormatterBuilder().appendPattern(((SimpleDateFormat) dateFormat).toPattern()).toFormatter(locale);
                concurrentMap.putIfAbsent(key, formatter);
                return formatter;
            }
            concurrentMap.putIfAbsent(key, "");
            throw new IllegalArgumentException("Unable to convert DateFormat to DateTimeFormatter");
        } else if (!cached.equals("")) {
            return (DateTimeFormatter) cached;
        } else {
            throw new IllegalArgumentException("Unable to convert DateFormat to DateTimeFormatter");
        }
    }

    private int convertStyle(FormatStyle style) {
        return style.ordinal();
    }
}
