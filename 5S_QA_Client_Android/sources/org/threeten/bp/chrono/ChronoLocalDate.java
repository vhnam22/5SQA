package org.threeten.bp.chrono;

import java.util.Comparator;
import org.threeten.bp.DateTimeException;
import org.threeten.bp.LocalDate;
import org.threeten.bp.LocalTime;
import org.threeten.bp.format.DateTimeFormatter;
import org.threeten.bp.jdk8.DefaultInterfaceTemporal;
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

public abstract class ChronoLocalDate extends DefaultInterfaceTemporal implements Temporal, TemporalAdjuster, Comparable<ChronoLocalDate> {
    private static final Comparator<ChronoLocalDate> DATE_COMPARATOR = new Comparator<ChronoLocalDate>() {
        public int compare(ChronoLocalDate date1, ChronoLocalDate date2) {
            return Jdk8Methods.compareLongs(date1.toEpochDay(), date2.toEpochDay());
        }
    };

    public abstract Chronology getChronology();

    public abstract int lengthOfMonth();

    public abstract ChronoLocalDate plus(long j, TemporalUnit temporalUnit);

    public abstract ChronoPeriod until(ChronoLocalDate chronoLocalDate);

    public abstract ChronoLocalDate with(TemporalField temporalField, long j);

    public static Comparator<ChronoLocalDate> timeLineOrder() {
        return DATE_COMPARATOR;
    }

    public static ChronoLocalDate from(TemporalAccessor temporal) {
        Jdk8Methods.requireNonNull(temporal, "temporal");
        if (temporal instanceof ChronoLocalDate) {
            return (ChronoLocalDate) temporal;
        }
        Chronology chrono = (Chronology) temporal.query(TemporalQueries.chronology());
        if (chrono != null) {
            return chrono.date(temporal);
        }
        throw new DateTimeException("No Chronology found to create ChronoLocalDate: " + temporal.getClass());
    }

    public Era getEra() {
        return getChronology().eraOf(get(ChronoField.ERA));
    }

    public boolean isLeapYear() {
        return getChronology().isLeapYear(getLong(ChronoField.YEAR));
    }

    public int lengthOfYear() {
        return isLeapYear() ? 366 : 365;
    }

    public boolean isSupported(TemporalField field) {
        if (field instanceof ChronoField) {
            return field.isDateBased();
        }
        return field != null && field.isSupportedBy(this);
    }

    public boolean isSupported(TemporalUnit unit) {
        if (unit instanceof ChronoUnit) {
            return unit.isDateBased();
        }
        return unit != null && unit.isSupportedBy(this);
    }

    public ChronoLocalDate with(TemporalAdjuster adjuster) {
        return getChronology().ensureChronoLocalDate(super.with(adjuster));
    }

    public ChronoLocalDate plus(TemporalAmount amount) {
        return getChronology().ensureChronoLocalDate(super.plus(amount));
    }

    public ChronoLocalDate minus(TemporalAmount amount) {
        return getChronology().ensureChronoLocalDate(super.minus(amount));
    }

    public ChronoLocalDate minus(long amountToSubtract, TemporalUnit unit) {
        return getChronology().ensureChronoLocalDate(super.minus(amountToSubtract, unit));
    }

    public <R> R query(TemporalQuery<R> query) {
        if (query == TemporalQueries.chronology()) {
            return getChronology();
        }
        if (query == TemporalQueries.precision()) {
            return ChronoUnit.DAYS;
        }
        if (query == TemporalQueries.localDate()) {
            return LocalDate.ofEpochDay(toEpochDay());
        }
        if (query == TemporalQueries.localTime() || query == TemporalQueries.zone() || query == TemporalQueries.zoneId() || query == TemporalQueries.offset()) {
            return null;
        }
        return super.query(query);
    }

    public Temporal adjustInto(Temporal temporal) {
        return temporal.with(ChronoField.EPOCH_DAY, toEpochDay());
    }

    public String format(DateTimeFormatter formatter) {
        Jdk8Methods.requireNonNull(formatter, "formatter");
        return formatter.format(this);
    }

    public ChronoLocalDateTime<?> atTime(LocalTime localTime) {
        return ChronoLocalDateTimeImpl.of(this, localTime);
    }

    public long toEpochDay() {
        return getLong(ChronoField.EPOCH_DAY);
    }

    public int compareTo(ChronoLocalDate other) {
        int cmp = Jdk8Methods.compareLongs(toEpochDay(), other.toEpochDay());
        if (cmp == 0) {
            return getChronology().compareTo(other.getChronology());
        }
        return cmp;
    }

    public boolean isAfter(ChronoLocalDate other) {
        return toEpochDay() > other.toEpochDay();
    }

    public boolean isBefore(ChronoLocalDate other) {
        return toEpochDay() < other.toEpochDay();
    }

    public boolean isEqual(ChronoLocalDate other) {
        return toEpochDay() == other.toEpochDay();
    }

    public boolean equals(Object obj) {
        if (this == obj) {
            return true;
        }
        if (!(obj instanceof ChronoLocalDate) || compareTo((ChronoLocalDate) obj) != 0) {
            return false;
        }
        return true;
    }

    public int hashCode() {
        long epDay = toEpochDay();
        return getChronology().hashCode() ^ ((int) ((epDay >>> 32) ^ epDay));
    }

    public String toString() {
        long yoe = getLong(ChronoField.YEAR_OF_ERA);
        long moy = getLong(ChronoField.MONTH_OF_YEAR);
        long dom = getLong(ChronoField.DAY_OF_MONTH);
        StringBuilder buf = new StringBuilder(30);
        String str = "-0";
        StringBuilder append = buf.append(getChronology().toString()).append(" ").append(getEra()).append(" ").append(yoe).append(moy < 10 ? str : "-").append(moy);
        if (dom >= 10) {
            str = "-";
        }
        append.append(str).append(dom);
        return buf.toString();
    }
}
