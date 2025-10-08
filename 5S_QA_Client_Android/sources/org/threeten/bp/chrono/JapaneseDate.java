package org.threeten.bp.chrono;

import java.io.DataInput;
import java.io.DataOutput;
import java.io.IOException;
import java.io.ObjectInputStream;
import java.io.Serializable;
import java.util.Calendar;
import org.threeten.bp.Clock;
import org.threeten.bp.DateTimeException;
import org.threeten.bp.LocalDate;
import org.threeten.bp.LocalTime;
import org.threeten.bp.Period;
import org.threeten.bp.ZoneId;
import org.threeten.bp.jdk8.Jdk8Methods;
import org.threeten.bp.temporal.ChronoField;
import org.threeten.bp.temporal.Temporal;
import org.threeten.bp.temporal.TemporalAccessor;
import org.threeten.bp.temporal.TemporalAdjuster;
import org.threeten.bp.temporal.TemporalAmount;
import org.threeten.bp.temporal.TemporalField;
import org.threeten.bp.temporal.TemporalUnit;
import org.threeten.bp.temporal.UnsupportedTemporalTypeException;
import org.threeten.bp.temporal.ValueRange;

public final class JapaneseDate extends ChronoDateImpl<JapaneseDate> implements Serializable {
    static final LocalDate MIN_DATE = LocalDate.of(1873, 1, 1);
    private static final long serialVersionUID = -305327627230580483L;
    private transient JapaneseEra era;
    private final LocalDate isoDate;
    private transient int yearOfEra;

    public /* bridge */ /* synthetic */ long until(Temporal x0, TemporalUnit x1) {
        return super.until(x0, x1);
    }

    public static JapaneseDate now() {
        return now(Clock.systemDefaultZone());
    }

    public static JapaneseDate now(ZoneId zone) {
        return now(Clock.system(zone));
    }

    public static JapaneseDate now(Clock clock) {
        return new JapaneseDate(LocalDate.now(clock));
    }

    public static JapaneseDate of(JapaneseEra era2, int yearOfEra2, int month, int dayOfMonth) {
        Jdk8Methods.requireNonNull(era2, "era");
        if (yearOfEra2 >= 1) {
            LocalDate eraStartDate = era2.startDate();
            LocalDate eraEndDate = era2.endDate();
            LocalDate date = LocalDate.of(yearOfEra2 + (eraStartDate.getYear() - 1), month, dayOfMonth);
            if (!date.isBefore(eraStartDate) && !date.isAfter(eraEndDate)) {
                return new JapaneseDate(era2, yearOfEra2, date);
            }
            throw new DateTimeException("Requested date is outside bounds of era " + era2);
        }
        throw new DateTimeException("Invalid YearOfEra: " + yearOfEra2);
    }

    static JapaneseDate ofYearDay(JapaneseEra era2, int yearOfEra2, int dayOfYear) {
        Jdk8Methods.requireNonNull(era2, "era");
        if (yearOfEra2 >= 1) {
            LocalDate eraStartDate = era2.startDate();
            LocalDate eraEndDate = era2.endDate();
            if (yearOfEra2 != 1 || (dayOfYear = dayOfYear + (eraStartDate.getDayOfYear() - 1)) <= eraStartDate.lengthOfYear()) {
                LocalDate isoDate2 = LocalDate.ofYearDay(yearOfEra2 + (eraStartDate.getYear() - 1), dayOfYear);
                if (!isoDate2.isBefore(eraStartDate) && !isoDate2.isAfter(eraEndDate)) {
                    return new JapaneseDate(era2, yearOfEra2, isoDate2);
                }
                throw new DateTimeException("Requested date is outside bounds of era " + era2);
            }
            throw new DateTimeException("DayOfYear exceeds maximum allowed in the first year of era " + era2);
        }
        throw new DateTimeException("Invalid YearOfEra: " + yearOfEra2);
    }

    public static JapaneseDate of(int prolepticYear, int month, int dayOfMonth) {
        return new JapaneseDate(LocalDate.of(prolepticYear, month, dayOfMonth));
    }

    public static JapaneseDate from(TemporalAccessor temporal) {
        return JapaneseChronology.INSTANCE.date(temporal);
    }

    JapaneseDate(LocalDate isoDate2) {
        if (!isoDate2.isBefore(MIN_DATE)) {
            JapaneseEra from = JapaneseEra.from(isoDate2);
            this.era = from;
            this.yearOfEra = isoDate2.getYear() - (from.startDate().getYear() - 1);
            this.isoDate = isoDate2;
            return;
        }
        throw new DateTimeException("Minimum supported date is January 1st Meiji 6");
    }

    JapaneseDate(JapaneseEra era2, int year, LocalDate isoDate2) {
        if (!isoDate2.isBefore(MIN_DATE)) {
            this.era = era2;
            this.yearOfEra = year;
            this.isoDate = isoDate2;
            return;
        }
        throw new DateTimeException("Minimum supported date is January 1st Meiji 6");
    }

    private void readObject(ObjectInputStream stream) throws IOException, ClassNotFoundException {
        stream.defaultReadObject();
        JapaneseEra from = JapaneseEra.from(this.isoDate);
        this.era = from;
        this.yearOfEra = this.isoDate.getYear() - (from.startDate().getYear() - 1);
    }

    public JapaneseChronology getChronology() {
        return JapaneseChronology.INSTANCE;
    }

    public JapaneseEra getEra() {
        return this.era;
    }

    public int lengthOfMonth() {
        return this.isoDate.lengthOfMonth();
    }

    public int lengthOfYear() {
        Calendar jcal = Calendar.getInstance(JapaneseChronology.LOCALE);
        jcal.set(0, this.era.getValue() + 2);
        jcal.set(this.yearOfEra, this.isoDate.getMonthValue() - 1, this.isoDate.getDayOfMonth());
        return jcal.getActualMaximum(6);
    }

    public boolean isSupported(TemporalField field) {
        if (field == ChronoField.ALIGNED_DAY_OF_WEEK_IN_MONTH || field == ChronoField.ALIGNED_DAY_OF_WEEK_IN_YEAR || field == ChronoField.ALIGNED_WEEK_OF_MONTH || field == ChronoField.ALIGNED_WEEK_OF_YEAR) {
            return false;
        }
        return super.isSupported(field);
    }

    public ValueRange range(TemporalField field) {
        if (!(field instanceof ChronoField)) {
            return field.rangeRefinedBy(this);
        }
        if (isSupported(field)) {
            ChronoField f = (ChronoField) field;
            switch (AnonymousClass1.$SwitchMap$org$threeten$bp$temporal$ChronoField[f.ordinal()]) {
                case 1:
                    return actualRange(6);
                case 2:
                    return actualRange(1);
                default:
                    return getChronology().range(f);
            }
        } else {
            throw new UnsupportedTemporalTypeException("Unsupported field: " + field);
        }
    }

    /* renamed from: org.threeten.bp.chrono.JapaneseDate$1  reason: invalid class name */
    static /* synthetic */ class AnonymousClass1 {
        static final /* synthetic */ int[] $SwitchMap$org$threeten$bp$temporal$ChronoField;

        static {
            int[] iArr = new int[ChronoField.values().length];
            $SwitchMap$org$threeten$bp$temporal$ChronoField = iArr;
            try {
                iArr[ChronoField.DAY_OF_YEAR.ordinal()] = 1;
            } catch (NoSuchFieldError e) {
            }
            try {
                $SwitchMap$org$threeten$bp$temporal$ChronoField[ChronoField.YEAR_OF_ERA.ordinal()] = 2;
            } catch (NoSuchFieldError e2) {
            }
            try {
                $SwitchMap$org$threeten$bp$temporal$ChronoField[ChronoField.ALIGNED_DAY_OF_WEEK_IN_MONTH.ordinal()] = 3;
            } catch (NoSuchFieldError e3) {
            }
            try {
                $SwitchMap$org$threeten$bp$temporal$ChronoField[ChronoField.ALIGNED_DAY_OF_WEEK_IN_YEAR.ordinal()] = 4;
            } catch (NoSuchFieldError e4) {
            }
            try {
                $SwitchMap$org$threeten$bp$temporal$ChronoField[ChronoField.ALIGNED_WEEK_OF_MONTH.ordinal()] = 5;
            } catch (NoSuchFieldError e5) {
            }
            try {
                $SwitchMap$org$threeten$bp$temporal$ChronoField[ChronoField.ALIGNED_WEEK_OF_YEAR.ordinal()] = 6;
            } catch (NoSuchFieldError e6) {
            }
            try {
                $SwitchMap$org$threeten$bp$temporal$ChronoField[ChronoField.ERA.ordinal()] = 7;
            } catch (NoSuchFieldError e7) {
            }
        }
    }

    private ValueRange actualRange(int calendarField) {
        Calendar jcal = Calendar.getInstance(JapaneseChronology.LOCALE);
        jcal.set(0, this.era.getValue() + 2);
        jcal.set(this.yearOfEra, this.isoDate.getMonthValue() - 1, this.isoDate.getDayOfMonth());
        return ValueRange.of((long) jcal.getActualMinimum(calendarField), (long) jcal.getActualMaximum(calendarField));
    }

    public long getLong(TemporalField field) {
        if (!(field instanceof ChronoField)) {
            return field.getFrom(this);
        }
        switch (AnonymousClass1.$SwitchMap$org$threeten$bp$temporal$ChronoField[((ChronoField) field).ordinal()]) {
            case 1:
                return getDayOfYear();
            case 2:
                return (long) this.yearOfEra;
            case 3:
            case 4:
            case 5:
            case 6:
                throw new UnsupportedTemporalTypeException("Unsupported field: " + field);
            case 7:
                return (long) this.era.getValue();
            default:
                return this.isoDate.getLong(field);
        }
    }

    private long getDayOfYear() {
        if (this.yearOfEra == 1) {
            return (long) ((this.isoDate.getDayOfYear() - this.era.startDate().getDayOfYear()) + 1);
        }
        return (long) this.isoDate.getDayOfYear();
    }

    public JapaneseDate with(TemporalAdjuster adjuster) {
        return (JapaneseDate) super.with(adjuster);
    }

    public JapaneseDate with(TemporalField field, long newValue) {
        if (!(field instanceof ChronoField)) {
            return (JapaneseDate) field.adjustInto(this, newValue);
        }
        ChronoField f = (ChronoField) field;
        if (getLong(f) == newValue) {
            return this;
        }
        switch (AnonymousClass1.$SwitchMap$org$threeten$bp$temporal$ChronoField[f.ordinal()]) {
            case 1:
            case 2:
            case 7:
                int nvalue = getChronology().range(f).checkValidIntValue(newValue, f);
                switch (AnonymousClass1.$SwitchMap$org$threeten$bp$temporal$ChronoField[f.ordinal()]) {
                    case 1:
                        return with(this.isoDate.plusDays(((long) nvalue) - getDayOfYear()));
                    case 2:
                        return withYear(nvalue);
                    case 7:
                        return withYear(JapaneseEra.of(nvalue), this.yearOfEra);
                }
        }
        return with(this.isoDate.with(field, newValue));
    }

    public JapaneseDate plus(TemporalAmount amount) {
        return (JapaneseDate) super.plus(amount);
    }

    public JapaneseDate plus(long amountToAdd, TemporalUnit unit) {
        return (JapaneseDate) super.plus(amountToAdd, unit);
    }

    public JapaneseDate minus(TemporalAmount amount) {
        return (JapaneseDate) super.minus(amount);
    }

    public JapaneseDate minus(long amountToAdd, TemporalUnit unit) {
        return (JapaneseDate) super.minus(amountToAdd, unit);
    }

    private JapaneseDate withYear(JapaneseEra era2, int yearOfEra2) {
        return with(this.isoDate.withYear(JapaneseChronology.INSTANCE.prolepticYear(era2, yearOfEra2)));
    }

    private JapaneseDate withYear(int year) {
        return withYear(getEra(), year);
    }

    /* access modifiers changed from: package-private */
    public JapaneseDate plusYears(long years) {
        return with(this.isoDate.plusYears(years));
    }

    /* access modifiers changed from: package-private */
    public JapaneseDate plusMonths(long months) {
        return with(this.isoDate.plusMonths(months));
    }

    /* access modifiers changed from: package-private */
    public JapaneseDate plusDays(long days) {
        return with(this.isoDate.plusDays(days));
    }

    private JapaneseDate with(LocalDate newDate) {
        return newDate.equals(this.isoDate) ? this : new JapaneseDate(newDate);
    }

    public final ChronoLocalDateTime<JapaneseDate> atTime(LocalTime localTime) {
        return super.atTime(localTime);
    }

    public ChronoPeriod until(ChronoLocalDate endDate) {
        Period period = this.isoDate.until(endDate);
        return getChronology().period(period.getYears(), period.getMonths(), period.getDays());
    }

    public long toEpochDay() {
        return this.isoDate.toEpochDay();
    }

    public boolean equals(Object obj) {
        if (this == obj) {
            return true;
        }
        if (obj instanceof JapaneseDate) {
            return this.isoDate.equals(((JapaneseDate) obj).isoDate);
        }
        return false;
    }

    public int hashCode() {
        return getChronology().getId().hashCode() ^ this.isoDate.hashCode();
    }

    private Object writeReplace() {
        return new Ser((byte) 1, this);
    }

    /* access modifiers changed from: package-private */
    public void writeExternal(DataOutput out) throws IOException {
        out.writeInt(get(ChronoField.YEAR));
        out.writeByte(get(ChronoField.MONTH_OF_YEAR));
        out.writeByte(get(ChronoField.DAY_OF_MONTH));
    }

    static ChronoLocalDate readExternal(DataInput in) throws IOException {
        return JapaneseChronology.INSTANCE.date(in.readInt(), in.readByte(), in.readByte());
    }
}
