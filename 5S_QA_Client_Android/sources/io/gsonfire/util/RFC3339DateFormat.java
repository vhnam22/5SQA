package io.gsonfire.util;

import java.text.DateFormat;
import java.text.FieldPosition;
import java.text.ParseException;
import java.text.ParsePosition;
import java.text.SimpleDateFormat;
import java.util.Date;
import java.util.TimeZone;
import java.util.regex.Matcher;
import java.util.regex.Pattern;

public final class RFC3339DateFormat extends DateFormat {
    private static final Pattern DATE_ONLY_PATTERN = Pattern.compile("^[0-9]{1,4}-[0-1][0-9]-[0-3][0-9]$");
    private static final Pattern MILLISECONDS_PATTERN = Pattern.compile("(.*)\\.([0-9]+)(.*)");
    private static final Pattern TIMEZONE_PATTERN = Pattern.compile("(.*)([+-][0-9][0-9])\\:?([0-9][0-9])$");
    private final SimpleDateFormat rfc3339Formatter;
    private final SimpleDateFormat rfc3339Parser;
    private final boolean serializeTime;

    public RFC3339DateFormat(TimeZone serializationTimezone, boolean serializeTime2) {
        this.rfc3339Parser = new SimpleDateFormat("yyyy-MM-dd'T'HH:mm:ssZ");
        if (serializeTime2) {
            this.rfc3339Formatter = new SimpleDateFormat("yyyy-MM-dd'T'HH:mm:ss");
        } else {
            this.rfc3339Formatter = new SimpleDateFormat("yyyy-MM-dd");
        }
        this.serializeTime = serializeTime2;
        this.rfc3339Formatter.setTimeZone(serializationTimezone);
    }

    public RFC3339DateFormat(TimeZone serializationTimezone) {
        this(serializationTimezone, true);
    }

    public RFC3339DateFormat(boolean serializeTime2) {
        this(TimeZone.getTimeZone("UTC"), serializeTime2);
    }

    public RFC3339DateFormat() {
        this(true);
    }

    private String generateTimezone(long time, TimeZone serializationTimezone) {
        if (serializationTimezone.getOffset(time) == 0) {
            return "Z";
        }
        int offset = (int) (((long) serializationTimezone.getOffset(time)) / 1000);
        int hours = offset / 3600;
        return (hours >= 0 ? "+" : "-") + String.format("%02d", new Object[]{Integer.valueOf(Math.abs(hours))}) + ":" + String.format("%02d", new Object[]{Integer.valueOf(Math.abs((offset - (hours * 3600)) / 60))});
    }

    public StringBuffer format(Date date, StringBuffer toAppendTo, FieldPosition fieldPosition) {
        StringBuffer formatted = new StringBuffer();
        formatted.append(this.rfc3339Formatter.format(date).toString());
        if (this.serializeTime) {
            long time = date.getTime();
            if (time % 1000 != 0) {
                formatted.append("." + Long.toString(time % 1000));
            }
            formatted.append(generateTimezone(time, this.rfc3339Formatter.getTimeZone()));
        }
        return formatted;
    }

    public Date parse(String source, ParsePosition pos) {
        String source2;
        if (DATE_ONLY_PATTERN.matcher(source).matches()) {
            source = source + "T00:00:00-0000";
        } else if (source.charAt(10) == 't') {
            source = source.substring(0, 10) + "T" + source.substring(12);
        }
        long millis = 0;
        if (source.contains(".")) {
            Matcher matcher = MILLISECONDS_PATTERN.matcher(source);
            millis = Long.parseLong(matcher.replaceAll("$2"));
            source = matcher.replaceAll("$1") + matcher.replaceAll("$3");
        }
        if (source.endsWith("Z") || source.endsWith("z")) {
            source2 = source.substring(0, source.length() - 1) + "-0000";
        } else {
            Matcher matcher2 = TIMEZONE_PATTERN.matcher(source);
            if (matcher2.matches()) {
                source2 = matcher2.replaceAll("$1") + matcher2.replaceAll("$2") + matcher2.replaceAll("$3");
            } else {
                source2 = source + "-0000";
            }
        }
        try {
            Date res = this.rfc3339Parser.parse(source2);
            if (millis > 0) {
                res = new Date(res.getTime() + millis);
            }
            pos.setIndex(source2.length());
            return res;
        } catch (ParseException e) {
            throw new RuntimeException(e);
        }
    }
}
