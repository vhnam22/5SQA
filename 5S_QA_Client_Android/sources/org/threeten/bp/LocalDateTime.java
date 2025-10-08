package org.threeten.bp;

import java.io.DataInput;
import java.io.DataOutput;
import java.io.IOException;
import java.io.InvalidObjectException;
import java.io.ObjectStreamException;
import java.io.Serializable;
import org.threeten.bp.chrono.ChronoLocalDateTime;
import org.threeten.bp.format.DateTimeFormatter;
import org.threeten.bp.jdk8.Jdk8Methods;
import org.threeten.bp.temporal.ChronoField;
import org.threeten.bp.temporal.ChronoUnit;
import org.threeten.bp.temporal.Temporal;
import org.threeten.bp.temporal.TemporalAccessor;
import org.threeten.bp.temporal.TemporalAdjuster;
import org.threeten.bp.temporal.TemporalAmount;
import org.threeten.bp.temporal.TemporalField;
import org.threeten.bp.temporal.TemporalQueries;
import org.threeten.bp.temporal.TemporalQuery;
import org.threeten.bp.temporal.TemporalUnit;
import org.threeten.bp.temporal.UnsupportedTemporalTypeException;
import org.threeten.bp.temporal.ValueRange;

public final class LocalDateTime extends ChronoLocalDateTime<LocalDate> implements Temporal, TemporalAdjuster, Serializable {
    public static final TemporalQuery<LocalDateTime> FROM = new TemporalQuery<LocalDateTime>() {
        public LocalDateTime queryFrom(TemporalAccessor temporal) {
            return LocalDateTime.from(temporal);
        }
    };
    public static final LocalDateTime MAX = of(LocalDate.MAX, LocalTime.MAX);
    public static final LocalDateTime MIN = of(LocalDate.MIN, LocalTime.MIN);
    private static final long serialVersionUID = 6207766400415563566L;
    private final LocalDate date;
    private final LocalTime time;

    public static LocalDateTime now() {
        return now(Clock.systemDefaultZone());
    }

    public static LocalDateTime now(ZoneId zone) {
        return now(Clock.system(zone));
    }

    public static LocalDateTime now(Clock clock) {
        Jdk8Methods.requireNonNull(clock, "clock");
        Instant now = clock.instant();
        return ofEpochSecond(now.getEpochSecond(), now.getNano(), clock.getZone().getRules().getOffset(now));
    }

    public static LocalDateTime of(int year, Month month, int dayOfMonth, int hour, int minute) {
        return new LocalDateTime(LocalDate.of(year, month, dayOfMonth), LocalTime.of(hour, minute));
    }

    public static LocalDateTime of(int year, Month month, int dayOfMonth, int hour, int minute, int second) {
        return new LocalDateTime(LocalDate.of(year, month, dayOfMonth), LocalTime.of(hour, minute, second));
    }

    public static LocalDateTime of(int year, Month month, int dayOfMonth, int hour, int minute, int second, int nanoOfSecond) {
        return new LocalDateTime(LocalDate.of(year, month, dayOfMonth), LocalTime.of(hour, minute, second, nanoOfSecond));
    }

    public static LocalDateTime of(int year, int month, int dayOfMonth, int hour, int minute) {
        return new LocalDateTime(LocalDate.of(year, month, dayOfMonth), LocalTime.of(hour, minute));
    }

    public static LocalDateTime of(int year, int month, int dayOfMonth, int hour, int minute, int second) {
        return new LocalDateTime(LocalDate.of(year, month, dayOfMonth), LocalTime.of(hour, minute, second));
    }

    public static LocalDateTime of(int year, int month, int dayOfMonth, int hour, int minute, int second, int nanoOfSecond) {
        return new LocalDateTime(LocalDate.of(year, month, dayOfMonth), LocalTime.of(hour, minute, second, nanoOfSecond));
    }

    public static LocalDateTime of(LocalDate date2, LocalTime time2) {
        Jdk8Methods.requireNonNull(date2, "date");
        Jdk8Methods.requireNonNull(time2, "time");
        return new LocalDateTime(date2, time2);
    }

    public static LocalDateTime ofInstant(Instant instant, ZoneId zone) {
        Jdk8Methods.requireNonNull(instant, "instant");
        Jdk8Methods.requireNonNull(zone, "zone");
        return ofEpochSecond(instant.getEpochSecond(), instant.getNano(), zone.getRules().getOffset(instant));
    }

    public static LocalDateTime ofEpochSecond(long epochSecond, int nanoOfSecond, ZoneOffset offset) {
        Jdk8Methods.requireNonNull(offset, "offset");
        long localSecond = ((long) offset.getTotalSeconds()) + epochSecond;
        return new LocalDateTime(LocalDate.ofEpochDay(Jdk8Methods.floorDiv(localSecond, 86400)), LocalTime.ofSecondOfDay((long) Jdk8Methods.floorMod(localSecond, 86400), nanoOfSecond));
    }

    public static LocalDateTime from(TemporalAccessor temporal) {
        if (temporal instanceof LocalDateTime) {
            return (LocalDateTime) temporal;
        }
        if (temporal instanceof ZonedDateTime) {
            return ((ZonedDateTime) temporal).toLocalDateTime();
        }
        try {
            return new LocalDateTime(LocalDate.from(temporal), LocalTime.from(temporal));
        } catch (DateTimeException e) {
            throw new DateTimeException("Unable to obtain LocalDateTime from TemporalAccessor: " + temporal + ", type " + temporal.getClass().getName());
        }
    }

    public static LocalDateTime parse(CharSequence text) {
        return parse(text, DateTimeFormatter.ISO_LOCAL_DATE_TIME);
    }

    public static LocalDateTime parse(CharSequence text, DateTimeFormatter formatter) {
        Jdk8Methods.requireNonNull(formatter, "formatter");
        return (LocalDateTime) formatter.parse(text, FROM);
    }

    private LocalDateTime(LocalDate date2, LocalTime time2) {
        this.date = date2;
        this.time = time2;
    }

    private LocalDateTime with(LocalDate newDate, LocalTime newTime) {
        if (this.date == newDate && this.time == newTime) {
            return this;
        }
        return new LocalDateTime(newDate, newTime);
    }

    public boolean isSupported(TemporalField field) {
        if (field instanceof ChronoField) {
            if (field.isDateBased() || field.isTimeBased()) {
                return true;
            }
            return false;
        } else if (field == null || !field.isSupportedBy(this)) {
            return false;
        } else {
            return true;
        }
    }

    public boolean isSupported(TemporalUnit unit) {
        if (unit instanceof ChronoUnit) {
            if (unit.isDateBased() || unit.isTimeBased()) {
                return true;
            }
            return false;
        } else if (unit == null || !unit.isSupportedBy(this)) {
            return false;
        } else {
            return true;
        }
    }

    public ValueRange range(TemporalField field) {
        if (field instanceof ChronoField) {
            return field.isTimeBased() ? this.time.range(field) : this.date.range(field);
        }
        return field.rangeRefinedBy(this);
    }

    public int get(TemporalField field) {
        if (field instanceof ChronoField) {
            return field.isTimeBased() ? this.time.get(field) : this.date.get(field);
        }
        return super.get(field);
    }

    public long getLong(TemporalField field) {
        if (field instanceof ChronoField) {
            return field.isTimeBased() ? this.time.getLong(field) : this.date.getLong(field);
        }
        return field.getFrom(this);
    }

    public int getYear() {
        return this.date.getYear();
    }

    public int getMonthValue() {
        return this.date.getMonthValue();
    }

    public Month getMonth() {
        return this.date.getMonth();
    }

    public int getDayOfMonth() {
        return this.date.getDayOfMonth();
    }

    public int getDayOfYear() {
        return this.date.getDayOfYear();
    }

    public DayOfWeek getDayOfWeek() {
        return this.date.getDayOfWeek();
    }

    public int getHour() {
        return this.time.getHour();
    }

    public int getMinute() {
        return this.time.getMinute();
    }

    public int getSecond() {
        return this.time.getSecond();
    }

    public int getNano() {
        return this.time.getNano();
    }

    public LocalDateTime with(TemporalAdjuster adjuster) {
        if (adjuster instanceof LocalDate) {
            return with((LocalDate) adjuster, this.time);
        }
        if (adjuster instanceof LocalTime) {
            return with(this.date, (LocalTime) adjuster);
        }
        if (adjuster instanceof LocalDateTime) {
            return (LocalDateTime) adjuster;
        }
        return (LocalDateTime) adjuster.adjustInto(this);
    }

    public LocalDateTime with(TemporalField field, long newValue) {
        if (!(field instanceof ChronoField)) {
            return (LocalDateTime) field.adjustInto(this, newValue);
        }
        if (field.isTimeBased()) {
            return with(this.date, this.time.with(field, newValue));
        }
        return with(this.date.with(field, newValue), this.time);
    }

    public LocalDateTime withYear(int year) {
        return with(this.date.withYear(year), this.time);
    }

    public LocalDateTime withMonth(int month) {
        return with(this.date.withMonth(month), this.time);
    }

    public LocalDateTime withDayOfMonth(int dayOfMonth) {
        return with(this.date.withDayOfMonth(dayOfMonth), this.time);
    }

    public LocalDateTime withDayOfYear(int dayOfYear) {
        return with(this.date.withDayOfYear(dayOfYear), this.time);
    }

    public LocalDateTime withHour(int hour) {
        return with(this.date, this.time.withHour(hour));
    }

    public LocalDateTime withMinute(int minute) {
        return with(this.date, this.time.withMinute(minute));
    }

    public LocalDateTime withSecond(int second) {
        return with(this.date, this.time.withSecond(second));
    }

    public LocalDateTime withNano(int nanoOfSecond) {
        return with(this.date, this.time.withNano(nanoOfSecond));
    }

    public LocalDateTime truncatedTo(TemporalUnit unit) {
        return with(this.date, this.time.truncatedTo(unit));
    }

    public LocalDateTime plus(TemporalAmount amount) {
        return (LocalDateTime) amount.addTo(this);
    }

    public LocalDateTime plus(long amountToAdd, TemporalUnit unit) {
        if (!(unit instanceof ChronoUnit)) {
            return (LocalDateTime) unit.addTo(this, amountToAdd);
        }
        switch (AnonymousClass2.$SwitchMap$org$threeten$bp$temporal$ChronoUnit[((ChronoUnit) unit).ordinal()]) {
            case 1:
                return plusNanos(amountToAdd);
            case 2:
                return plusDays(amountToAdd / 86400000000L).plusNanos((amountToAdd % 86400000000L) * 1000);
            case 3:
                return plusDays(amountToAdd / 86400000).plusNanos((amountToAdd % 86400000) * 1000000);
            case 4:
                return plusSeconds(amountToAdd);
            case 5:
                return plusMinutes(amountToAdd);
            case 6:
                return plusHours(amountToAdd);
            case 7:
                return plusDays(amountToAdd / 256).plusHours((amountToAdd % 256) * 12);
            default:
                return with(this.date.plus(amountToAdd, unit), this.time);
        }
    }

    /* renamed from: org.threeten.bp.LocalDateTime$2  reason: invalid class name */
    static /* synthetic */ class AnonymousClass2 {
        static final /* synthetic */ int[] $SwitchMap$org$threeten$bp$temporal$ChronoUnit;

        static {
            int[] iArr = new int[ChronoUnit.values().length];
            $SwitchMap$org$threeten$bp$temporal$ChronoUnit = iArr;
            try {
                iArr[ChronoUnit.NANOS.ordinal()] = 1;
            } catch (NoSuchFieldError e) {
            }
            try {
                $SwitchMap$org$threeten$bp$temporal$ChronoUnit[ChronoUnit.MICROS.ordinal()] = 2;
            } catch (NoSuchFieldError e2) {
            }
            try {
                $SwitchMap$org$threeten$bp$temporal$ChronoUnit[ChronoUnit.MILLIS.ordinal()] = 3;
            } catch (NoSuchFieldError e3) {
            }
            try {
                $SwitchMap$org$threeten$bp$temporal$ChronoUnit[ChronoUnit.SECONDS.ordinal()] = 4;
            } catch (NoSuchFieldError e4) {
            }
            try {
                $SwitchMap$org$threeten$bp$temporal$ChronoUnit[ChronoUnit.MINUTES.ordinal()] = 5;
            } catch (NoSuchFieldError e5) {
            }
            try {
                $SwitchMap$org$threeten$bp$temporal$ChronoUnit[ChronoUnit.HOURS.ordinal()] = 6;
            } catch (NoSuchFieldError e6) {
            }
            try {
                $SwitchMap$org$threeten$bp$temporal$ChronoUnit[ChronoUnit.HALF_DAYS.ordinal()] = 7;
            } catch (NoSuchFieldError e7) {
            }
        }
    }

    public LocalDateTime plusYears(long years) {
        return with(this.date.plusYears(years), this.time);
    }

    public LocalDateTime plusMonths(long months) {
        return with(this.date.plusMonths(months), this.time);
    }

    public LocalDateTime plusWeeks(long weeks) {
        return with(this.date.plusWeeks(weeks), this.time);
    }

    public LocalDateTime plusDays(long days) {
        return with(this.date.plusDays(days), this.time);
    }

    public LocalDateTime plusHours(long hours) {
        return plusWithOverflow(this.date, hours, 0, 0, 0, 1);
    }

    public LocalDateTime plusMinutes(long minutes) {
        return plusWithOverflow(this.date, 0, minutes, 0, 0, 1);
    }

    public LocalDateTime plusSeconds(long seconds) {
        return plusWithOverflow(this.date, 0, 0, seconds, 0, 1);
    }

    public LocalDateTime plusNanos(long nanos) {
        return plusWithOverflow(this.date, 0, 0, 0, nanos, 1);
    }

    public LocalDateTime minus(TemporalAmount amount) {
        return (LocalDateTime) amount.subtractFrom(this);
    }

    public LocalDateTime minus(long amountToSubtract, TemporalUnit unit) {
        return amountToSubtract == Long.MIN_VALUE ? plus(Long.MAX_VALUE, unit).plus(1, unit) : plus(-amountToSubtract, unit);
    }

    public LocalDateTime minusYears(long years) {
        return years == Long.MIN_VALUE ? plusYears(Long.MAX_VALUE).plusYears(1) : plusYears(-years);
    }

    public LocalDateTime minusMonths(long months) {
        return months == Long.MIN_VALUE ? plusMonths(Long.MAX_VALUE).plusMonths(1) : plusMonths(-months);
    }

    public LocalDateTime minusWeeks(long weeks) {
        return weeks == Long.MIN_VALUE ? plusWeeks(Long.MAX_VALUE).plusWeeks(1) : plusWeeks(-weeks);
    }

    public LocalDateTime minusDays(long days) {
        return days == Long.MIN_VALUE ? plusDays(Long.MAX_VALUE).plusDays(1) : plusDays(-days);
    }

    public LocalDateTime minusHours(long hours) {
        return plusWithOverflow(this.date, hours, 0, 0, 0, -1);
    }

    public LocalDateTime minusMinutes(long minutes) {
        return plusWithOverflow(this.date, 0, minutes, 0, 0, -1);
    }

    public LocalDateTime minusSeconds(long seconds) {
        return plusWithOverflow(this.date, 0, 0, seconds, 0, -1);
    }

    public LocalDateTime minusNanos(long nanos) {
        return plusWithOverflow(this.date, 0, 0, 0, nanos, -1);
    }

    private LocalDateTime plusWithOverflow(LocalDate newDate, long hours, long minutes, long seconds, long nanos, int sign) {
        LocalDate localDate = newDate;
        int i = sign;
        if ((hours | minutes | seconds | nanos) == 0) {
            return with(localDate, this.time);
        }
        long curNoD = this.time.toNanoOfDay();
        long totNanos = (((long) i) * ((nanos % 86400000000000L) + ((seconds % 86400) * 1000000000) + ((minutes % 1440) * 60000000000L) + ((hours % 24) * 3600000000000L))) + curNoD;
        long totDays = (((nanos / 86400000000000L) + (seconds / 86400) + (minutes / 1440) + (hours / 24)) * ((long) i)) + Jdk8Methods.floorDiv(totNanos, 86400000000000L);
        long newNoD = Jdk8Methods.floorMod(totNanos, 86400000000000L);
        return with(localDate.plusDays(totDays), newNoD == curNoD ? this.time : LocalTime.ofNanoOfDay(newNoD));
    }

    public <R> R query(TemporalQuery<R> query) {
        if (query == TemporalQueries.localDate()) {
            return toLocalDate();
        }
        return super.query(query);
    }

    public Temporal adjustInto(Temporal temporal) {
        return super.adjustInto(temporal);
    }

    public long until(Temporal endExclusive, TemporalUnit unit) {
        LocalDateTime end = from(endExclusive);
        if (!(unit instanceof ChronoUnit)) {
            return unit.between(this, end);
        }
        ChronoUnit f = (ChronoUnit) unit;
        if (f.isTimeBased()) {
            long daysUntil = this.date.daysUntil(end.date);
            long timeUntil = end.time.toNanoOfDay() - this.time.toNanoOfDay();
            if (daysUntil > 0 && timeUntil < 0) {
                daysUntil--;
                timeUntil += 86400000000000L;
            } else if (daysUntil < 0 && timeUntil > 0) {
                daysUntil++;
                timeUntil -= 86400000000000L;
            }
            long amount = daysUntil;
            switch (AnonymousClass2.$SwitchMap$org$threeten$bp$temporal$ChronoUnit[f.ordinal()]) {
                case 1:
                    return Jdk8Methods.safeAdd(Jdk8Methods.safeMultiply(amount, 86400000000000L), timeUntil);
                case 2:
                    return Jdk8Methods.safeAdd(Jdk8Methods.safeMultiply(amount, 86400000000L), timeUntil / 1000);
                case 3:
                    return Jdk8Methods.safeAdd(Jdk8Methods.safeMultiply(amount, 86400000), timeUntil / 1000000);
                case 4:
                    return Jdk8Methods.safeAdd(Jdk8Methods.safeMultiply(amount, 86400), timeUntil / 1000000000);
                case 5:
                    return Jdk8Methods.safeAdd(Jdk8Methods.safeMultiply(amount, 1440), timeUntil / 60000000000L);
                case 6:
                    return Jdk8Methods.safeAdd(Jdk8Methods.safeMultiply(amount, 24), timeUntil / 3600000000000L);
                case 7:
                    return Jdk8Methods.safeAdd(Jdk8Methods.safeMultiply(amount, 2), timeUntil / 43200000000000L);
                default:
                    throw new UnsupportedTemporalTypeException("Unsupported unit: " + unit);
            }
        } else {
            LocalDate endDate = end.date;
            if (endDate.isAfter(this.date) && end.time.isBefore(this.time)) {
                endDate = endDate.minusDays(1);
            } else if (endDate.isBefore(this.date) && end.time.isAfter(this.time)) {
                endDate = endDate.plusDays(1);
            }
            return this.date.until(endDate, unit);
        }
    }

    public OffsetDateTime atOffset(ZoneOffset offset) {
        return OffsetDateTime.of(this, offset);
    }

    public ZonedDateTime atZone(ZoneId zone) {
        return ZonedDateTime.of(this, zone);
    }

    public LocalDate toLocalDate() {
        return this.date;
    }

    public LocalTime toLocalTime() {
        return this.time;
    }

    public int compareTo(ChronoLocalDateTime<?> other) {
        if (other instanceof LocalDateTime) {
            return compareTo0((LocalDateTime) other);
        }
        return super.compareTo(other);
    }

    private int compareTo0(LocalDateTime other) {
        int cmp = this.date.compareTo0(other.toLocalDate());
        if (cmp == 0) {
            return this.time.compareTo(other.toLocalTime());
        }
        return cmp;
    }

    public boolean isAfter(ChronoLocalDateTime<?> other) {
        if (other instanceof LocalDateTime) {
            return compareTo0((LocalDateTime) other) > 0;
        }
        return super.isAfter(other);
    }

    public boolean isBefore(ChronoLocalDateTime<?> other) {
        if (other instanceof LocalDateTime) {
            return compareTo0((LocalDateTime) other) < 0;
        }
        return super.isBefore(other);
    }

    public boolean isEqual(ChronoLocalDateTime<?> other) {
        if (other instanceof LocalDateTime) {
            return compareTo0((LocalDateTime) other) == 0;
        }
        return super.isEqual(other);
    }

    public boolean equals(Object obj) {
        if (this == obj) {
            return true;
        }
        if (!(obj instanceof LocalDateTime)) {
            return false;
        }
        LocalDateTime other = (LocalDateTime) obj;
        if (!this.date.equals(other.date) || !this.time.equals(other.time)) {
            return false;
        }
        return true;
    }

    public int hashCode() {
        return this.date.hashCode() ^ this.time.hashCode();
    }

    public String toString() {
        return this.date.toString() + 'T' + this.time.toString();
    }

    public String format(DateTimeFormatter formatter) {
        return super.format(formatter);
    }

    private Object writeReplace() {
        return new Ser((byte) 4, this);
    }

    private Object readResolve() throws ObjectStreamException {
        throw new InvalidObjectException("Deserialization via serialization delegate");
    }

    /* access modifiers changed from: package-private */
    public void writeExternal(DataOutput out) throws IOException {
        this.date.writeExternal(out);
        this.time.writeExternal(out);
    }

    static LocalDateTime readExternal(DataInput in) throws IOException {
        return of(LocalDate.readExternal(in), LocalTime.readExternal(in));
    }
}
