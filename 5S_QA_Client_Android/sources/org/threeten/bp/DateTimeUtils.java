package org.threeten.bp;

import java.sql.Time;
import java.sql.Timestamp;
import java.util.Calendar;
import java.util.Date;
import java.util.GregorianCalendar;
import java.util.TimeZone;

public final class DateTimeUtils {
    private DateTimeUtils() {
    }

    public static Instant toInstant(Date utilDate) {
        return Instant.ofEpochMilli(utilDate.getTime());
    }

    public static Date toDate(Instant instant) {
        try {
            return new Date(instant.toEpochMilli());
        } catch (ArithmeticException ex) {
            throw new IllegalArgumentException(ex);
        }
    }

    public static Instant toInstant(Calendar calendar) {
        return Instant.ofEpochMilli(calendar.getTimeInMillis());
    }

    public static ZonedDateTime toZonedDateTime(Calendar calendar) {
        return ZonedDateTime.ofInstant(Instant.ofEpochMilli(calendar.getTimeInMillis()), toZoneId(calendar.getTimeZone()));
    }

    public static GregorianCalendar toGregorianCalendar(ZonedDateTime zdt) {
        GregorianCalendar cal = new GregorianCalendar(toTimeZone(zdt.getZone()));
        cal.setGregorianChange(new Date(Long.MIN_VALUE));
        cal.setFirstDayOfWeek(2);
        cal.setMinimalDaysInFirstWeek(4);
        try {
            cal.setTimeInMillis(zdt.toInstant().toEpochMilli());
            return cal;
        } catch (ArithmeticException ex) {
            throw new IllegalArgumentException(ex);
        }
    }

    public static ZoneId toZoneId(TimeZone timeZone) {
        return ZoneId.of(timeZone.getID(), ZoneId.SHORT_IDS);
    }

    public static TimeZone toTimeZone(ZoneId zoneId) {
        String tzid = zoneId.getId();
        if (tzid.startsWith("+") || tzid.startsWith("-")) {
            tzid = "GMT" + tzid;
        } else if (tzid.equals("Z")) {
            tzid = "UTC";
        }
        return TimeZone.getTimeZone(tzid);
    }

    public static LocalDate toLocalDate(java.sql.Date sqlDate) {
        return LocalDate.of(sqlDate.getYear() + 1900, sqlDate.getMonth() + 1, sqlDate.getDate());
    }

    public static java.sql.Date toSqlDate(LocalDate date) {
        return new java.sql.Date(date.getYear() - 1900, date.getMonthValue() - 1, date.getDayOfMonth());
    }

    public static LocalTime toLocalTime(Time sqlTime) {
        return LocalTime.of(sqlTime.getHours(), sqlTime.getMinutes(), sqlTime.getSeconds());
    }

    public static Time toSqlTime(LocalTime time) {
        return new Time(time.getHour(), time.getMinute(), time.getSecond());
    }

    public static Timestamp toSqlTimestamp(LocalDateTime dateTime) {
        return new Timestamp(dateTime.getYear() - 1900, dateTime.getMonthValue() - 1, dateTime.getDayOfMonth(), dateTime.getHour(), dateTime.getMinute(), dateTime.getSecond(), dateTime.getNano());
    }

    public static LocalDateTime toLocalDateTime(Timestamp sqlTimestamp) {
        return LocalDateTime.of(sqlTimestamp.getYear() + 1900, sqlTimestamp.getMonth() + 1, sqlTimestamp.getDate(), sqlTimestamp.getHours(), sqlTimestamp.getMinutes(), sqlTimestamp.getSeconds(), sqlTimestamp.getNanos());
    }

    public static Timestamp toSqlTimestamp(Instant instant) {
        try {
            Timestamp ts = new Timestamp(instant.getEpochSecond() * 1000);
            ts.setNanos(instant.getNano());
            return ts;
        } catch (ArithmeticException ex) {
            throw new IllegalArgumentException(ex);
        }
    }

    public static Instant toInstant(Timestamp sqlTimestamp) {
        return Instant.ofEpochSecond(sqlTimestamp.getTime() / 1000, (long) sqlTimestamp.getNanos());
    }
}
