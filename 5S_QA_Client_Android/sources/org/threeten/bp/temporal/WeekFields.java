package org.threeten.bp.temporal;

import java.io.InvalidObjectException;
import java.io.Serializable;
import java.util.GregorianCalendar;
import java.util.Locale;
import java.util.Map;
import java.util.concurrent.ConcurrentHashMap;
import java.util.concurrent.ConcurrentMap;
import org.threeten.bp.DateTimeException;
import org.threeten.bp.DayOfWeek;
import org.threeten.bp.Year;
import org.threeten.bp.chrono.ChronoLocalDate;
import org.threeten.bp.chrono.Chronology;
import org.threeten.bp.format.ResolverStyle;
import org.threeten.bp.jdk8.Jdk8Methods;

public final class WeekFields implements Serializable {
    private static final ConcurrentMap<String, WeekFields> CACHE = new ConcurrentHashMap(4, 0.75f, 2);
    public static final WeekFields ISO = new WeekFields(DayOfWeek.MONDAY, 4);
    public static final WeekFields SUNDAY_START = of(DayOfWeek.SUNDAY, 1);
    private static final long serialVersionUID = -1177360819670808121L;
    private final transient TemporalField dayOfWeek = ComputedDayOfField.ofDayOfWeekField(this);
    private final DayOfWeek firstDayOfWeek;
    private final int minimalDays;
    private final transient TemporalField weekBasedYear = ComputedDayOfField.ofWeekBasedYearField(this);
    private final transient TemporalField weekOfMonth = ComputedDayOfField.ofWeekOfMonthField(this);
    /* access modifiers changed from: private */
    public final transient TemporalField weekOfWeekBasedYear = ComputedDayOfField.ofWeekOfWeekBasedYearField(this);
    private final transient TemporalField weekOfYear = ComputedDayOfField.ofWeekOfYearField(this);

    public static WeekFields of(Locale locale) {
        Jdk8Methods.requireNonNull(locale, "locale");
        GregorianCalendar gcal = new GregorianCalendar(new Locale(locale.getLanguage(), locale.getCountry()));
        return of(DayOfWeek.SUNDAY.plus((long) (gcal.getFirstDayOfWeek() - 1)), gcal.getMinimalDaysInFirstWeek());
    }

    public static WeekFields of(DayOfWeek firstDayOfWeek2, int minimalDaysInFirstWeek) {
        String key = firstDayOfWeek2.toString() + minimalDaysInFirstWeek;
        ConcurrentMap<String, WeekFields> concurrentMap = CACHE;
        WeekFields rules = (WeekFields) concurrentMap.get(key);
        if (rules != null) {
            return rules;
        }
        concurrentMap.putIfAbsent(key, new WeekFields(firstDayOfWeek2, minimalDaysInFirstWeek));
        return (WeekFields) concurrentMap.get(key);
    }

    private WeekFields(DayOfWeek firstDayOfWeek2, int minimalDaysInFirstWeek) {
        Jdk8Methods.requireNonNull(firstDayOfWeek2, "firstDayOfWeek");
        if (minimalDaysInFirstWeek < 1 || minimalDaysInFirstWeek > 7) {
            throw new IllegalArgumentException("Minimal number of days is invalid");
        }
        this.firstDayOfWeek = firstDayOfWeek2;
        this.minimalDays = minimalDaysInFirstWeek;
    }

    private Object readResolve() throws InvalidObjectException {
        try {
            return of(this.firstDayOfWeek, this.minimalDays);
        } catch (IllegalArgumentException ex) {
            throw new InvalidObjectException("Invalid WeekFields" + ex.getMessage());
        }
    }

    public DayOfWeek getFirstDayOfWeek() {
        return this.firstDayOfWeek;
    }

    public int getMinimalDaysInFirstWeek() {
        return this.minimalDays;
    }

    public TemporalField dayOfWeek() {
        return this.dayOfWeek;
    }

    public TemporalField weekOfMonth() {
        return this.weekOfMonth;
    }

    public TemporalField weekOfYear() {
        return this.weekOfYear;
    }

    public TemporalField weekOfWeekBasedYear() {
        return this.weekOfWeekBasedYear;
    }

    public TemporalField weekBasedYear() {
        return this.weekBasedYear;
    }

    public boolean equals(Object object) {
        if (this == object) {
            return true;
        }
        if (!(object instanceof WeekFields) || hashCode() != object.hashCode()) {
            return false;
        }
        return true;
    }

    public int hashCode() {
        return (this.firstDayOfWeek.ordinal() * 7) + this.minimalDays;
    }

    public String toString() {
        return "WeekFields[" + this.firstDayOfWeek + ',' + this.minimalDays + ']';
    }

    static class ComputedDayOfField implements TemporalField {
        private static final ValueRange DAY_OF_WEEK_RANGE = ValueRange.of(1, 7);
        private static final ValueRange WEEK_BASED_YEAR_RANGE = ChronoField.YEAR.range();
        private static final ValueRange WEEK_OF_MONTH_RANGE = ValueRange.of(0, 1, 4, 6);
        private static final ValueRange WEEK_OF_WEEK_BASED_YEAR_RANGE = ValueRange.of(1, 52, 53);
        private static final ValueRange WEEK_OF_YEAR_RANGE = ValueRange.of(0, 1, 52, 54);
        private final TemporalUnit baseUnit;
        private final String name;
        private final ValueRange range;
        private final TemporalUnit rangeUnit;
        private final WeekFields weekDef;

        static ComputedDayOfField ofDayOfWeekField(WeekFields weekDef2) {
            return new ComputedDayOfField("DayOfWeek", weekDef2, ChronoUnit.DAYS, ChronoUnit.WEEKS, DAY_OF_WEEK_RANGE);
        }

        static ComputedDayOfField ofWeekOfMonthField(WeekFields weekDef2) {
            return new ComputedDayOfField("WeekOfMonth", weekDef2, ChronoUnit.WEEKS, ChronoUnit.MONTHS, WEEK_OF_MONTH_RANGE);
        }

        static ComputedDayOfField ofWeekOfYearField(WeekFields weekDef2) {
            return new ComputedDayOfField("WeekOfYear", weekDef2, ChronoUnit.WEEKS, ChronoUnit.YEARS, WEEK_OF_YEAR_RANGE);
        }

        static ComputedDayOfField ofWeekOfWeekBasedYearField(WeekFields weekDef2) {
            return new ComputedDayOfField("WeekOfWeekBasedYear", weekDef2, ChronoUnit.WEEKS, IsoFields.WEEK_BASED_YEARS, WEEK_OF_WEEK_BASED_YEAR_RANGE);
        }

        static ComputedDayOfField ofWeekBasedYearField(WeekFields weekDef2) {
            return new ComputedDayOfField("WeekBasedYear", weekDef2, IsoFields.WEEK_BASED_YEARS, ChronoUnit.FOREVER, WEEK_BASED_YEAR_RANGE);
        }

        private ComputedDayOfField(String name2, WeekFields weekDef2, TemporalUnit baseUnit2, TemporalUnit rangeUnit2, ValueRange range2) {
            this.name = name2;
            this.weekDef = weekDef2;
            this.baseUnit = baseUnit2;
            this.rangeUnit = rangeUnit2;
            this.range = range2;
        }

        public long getFrom(TemporalAccessor temporal) {
            int dow = Jdk8Methods.floorMod(temporal.get(ChronoField.DAY_OF_WEEK) - this.weekDef.getFirstDayOfWeek().getValue(), 7) + 1;
            if (this.rangeUnit == ChronoUnit.WEEKS) {
                return (long) dow;
            }
            if (this.rangeUnit == ChronoUnit.MONTHS) {
                int dom = temporal.get(ChronoField.DAY_OF_MONTH);
                return (long) computeWeek(startOfWeekOffset(dom, dow), dom);
            } else if (this.rangeUnit == ChronoUnit.YEARS) {
                int doy = temporal.get(ChronoField.DAY_OF_YEAR);
                return (long) computeWeek(startOfWeekOffset(doy, dow), doy);
            } else if (this.rangeUnit == IsoFields.WEEK_BASED_YEARS) {
                return (long) localizedWOWBY(temporal);
            } else {
                if (this.rangeUnit == ChronoUnit.FOREVER) {
                    return (long) localizedWBY(temporal);
                }
                throw new IllegalStateException("unreachable");
            }
        }

        private int localizedDayOfWeek(TemporalAccessor temporal, int sow) {
            return Jdk8Methods.floorMod(temporal.get(ChronoField.DAY_OF_WEEK) - sow, 7) + 1;
        }

        private long localizedWeekOfMonth(TemporalAccessor temporal, int dow) {
            int dom = temporal.get(ChronoField.DAY_OF_MONTH);
            return (long) computeWeek(startOfWeekOffset(dom, dow), dom);
        }

        private long localizedWeekOfYear(TemporalAccessor temporal, int dow) {
            int doy = temporal.get(ChronoField.DAY_OF_YEAR);
            return (long) computeWeek(startOfWeekOffset(doy, dow), doy);
        }

        private int localizedWOWBY(TemporalAccessor temporal) {
            int dow = Jdk8Methods.floorMod(temporal.get(ChronoField.DAY_OF_WEEK) - this.weekDef.getFirstDayOfWeek().getValue(), 7) + 1;
            long woy = localizedWeekOfYear(temporal, dow);
            if (woy == 0) {
                return ((int) localizedWeekOfYear(Chronology.from(temporal).date(temporal).minus(1, (TemporalUnit) ChronoUnit.WEEKS), dow)) + 1;
            }
            if (woy >= 53) {
                int weekIndexOfFirstWeekNextYear = computeWeek(startOfWeekOffset(temporal.get(ChronoField.DAY_OF_YEAR), dow), this.weekDef.getMinimalDaysInFirstWeek() + (Year.isLeap((long) temporal.get(ChronoField.YEAR)) ? 366 : 365));
                if (woy >= ((long) weekIndexOfFirstWeekNextYear)) {
                    return (int) (woy - ((long) (weekIndexOfFirstWeekNextYear - 1)));
                }
            }
            return (int) woy;
        }

        private int localizedWBY(TemporalAccessor temporal) {
            int dow = Jdk8Methods.floorMod(temporal.get(ChronoField.DAY_OF_WEEK) - this.weekDef.getFirstDayOfWeek().getValue(), 7) + 1;
            int year = temporal.get(ChronoField.YEAR);
            long woy = localizedWeekOfYear(temporal, dow);
            if (woy == 0) {
                return year - 1;
            }
            if (woy < 53) {
                return year;
            }
            if (woy >= ((long) computeWeek(startOfWeekOffset(temporal.get(ChronoField.DAY_OF_YEAR), dow), this.weekDef.getMinimalDaysInFirstWeek() + (Year.isLeap((long) year) ? 366 : 365)))) {
                return year + 1;
            }
            return year;
        }

        private int startOfWeekOffset(int day, int dow) {
            int weekStart = Jdk8Methods.floorMod(day - dow, 7);
            int offset = -weekStart;
            if (weekStart + 1 > this.weekDef.getMinimalDaysInFirstWeek()) {
                return 7 - weekStart;
            }
            return offset;
        }

        private int computeWeek(int offset, int day) {
            return ((offset + 7) + (day - 1)) / 7;
        }

        public <R extends Temporal> R adjustInto(R temporal, long newValue) {
            int newVal = this.range.checkValidIntValue(newValue, this);
            int currentVal = temporal.get(this);
            if (newVal == currentVal) {
                return temporal;
            }
            if (this.rangeUnit != ChronoUnit.FOREVER) {
                return temporal.plus((long) (newVal - currentVal), this.baseUnit);
            }
            int baseWowby = temporal.get(this.weekDef.weekOfWeekBasedYear);
            Temporal result = temporal.plus((long) (((double) (newValue - ((long) currentVal))) * 52.1775d), ChronoUnit.WEEKS);
            if (result.get(this) > newVal) {
                return result.minus((long) result.get(this.weekDef.weekOfWeekBasedYear), ChronoUnit.WEEKS);
            }
            if (result.get(this) < newVal) {
                result = result.plus(2, ChronoUnit.WEEKS);
            }
            Temporal result2 = result.plus((long) (baseWowby - result.get(this.weekDef.weekOfWeekBasedYear)), ChronoUnit.WEEKS);
            if (result2.get(this) > newVal) {
                return result2.minus(1, ChronoUnit.WEEKS);
            }
            return result2;
        }

        public TemporalAccessor resolve(Map<TemporalField, Long> fieldValues, TemporalAccessor partialTemporal, ResolverStyle resolverStyle) {
            long days;
            long days2;
            ChronoLocalDate date;
            long days3;
            ChronoLocalDate date2;
            Map<TemporalField, Long> map = fieldValues;
            ResolverStyle resolverStyle2 = resolverStyle;
            int sow = this.weekDef.getFirstDayOfWeek().getValue();
            if (this.rangeUnit == ChronoUnit.WEEKS) {
                map.put(ChronoField.DAY_OF_WEEK, Long.valueOf((long) (Jdk8Methods.floorMod((sow - 1) + (this.range.checkValidIntValue(map.remove(this).longValue(), this) - 1), 7) + 1)));
                return null;
            } else if (!map.containsKey(ChronoField.DAY_OF_WEEK)) {
                return null;
            } else {
                if (this.rangeUnit == ChronoUnit.FOREVER) {
                    if (!map.containsKey(this.weekDef.weekOfWeekBasedYear)) {
                        return null;
                    }
                    Chronology chrono = Chronology.from(partialTemporal);
                    int isoDow = ChronoField.DAY_OF_WEEK.checkValidIntValue(map.get(ChronoField.DAY_OF_WEEK).longValue());
                    int dow = Jdk8Methods.floorMod(isoDow - sow, 7) + 1;
                    int wby = range().checkValidIntValue(map.get(this).longValue(), this);
                    if (resolverStyle2 == ResolverStyle.LENIENT) {
                        date2 = chrono.date(wby, 1, this.weekDef.getMinimalDaysInFirstWeek());
                        long wowby = map.get(this.weekDef.weekOfWeekBasedYear).longValue();
                        int dateDow = localizedDayOfWeek(date2, sow);
                        int i = isoDow;
                        long j = wowby;
                        days3 = (7 * (wowby - localizedWeekOfYear(date2, dateDow))) + ((long) (dow - dateDow));
                        Chronology chronology = chrono;
                    } else {
                        date2 = chrono.date(wby, 1, this.weekDef.getMinimalDaysInFirstWeek());
                        long wowby2 = (long) this.weekDef.weekOfWeekBasedYear.range().checkValidIntValue(map.get(this.weekDef.weekOfWeekBasedYear).longValue(), this.weekDef.weekOfWeekBasedYear);
                        int dateDow2 = localizedDayOfWeek(date2, sow);
                        Chronology chronology2 = chrono;
                        int i2 = dateDow2;
                        days3 = (7 * (wowby2 - localizedWeekOfYear(date2, dateDow2))) + ((long) (dow - dateDow2));
                    }
                    ChronoLocalDate date3 = date2.plus(days3, (TemporalUnit) ChronoUnit.DAYS);
                    if (resolverStyle2 != ResolverStyle.STRICT || date3.getLong(this) == map.get(this).longValue()) {
                        map.remove(this);
                        map.remove(this.weekDef.weekOfWeekBasedYear);
                        map.remove(ChronoField.DAY_OF_WEEK);
                        return date3;
                    }
                    throw new DateTimeException("Strict mode rejected date parsed to a different year");
                } else if (!map.containsKey(ChronoField.YEAR)) {
                    return null;
                } else {
                    int isoDow2 = ChronoField.DAY_OF_WEEK.checkValidIntValue(map.get(ChronoField.DAY_OF_WEEK).longValue());
                    int dow2 = Jdk8Methods.floorMod(isoDow2 - sow, 7) + 1;
                    int year = ChronoField.YEAR.checkValidIntValue(map.get(ChronoField.YEAR).longValue());
                    Chronology chrono2 = Chronology.from(partialTemporal);
                    if (this.rangeUnit == ChronoUnit.MONTHS) {
                        if (!map.containsKey(ChronoField.MONTH_OF_YEAR)) {
                            return null;
                        }
                        long value = map.remove(this).longValue();
                        if (resolverStyle2 == ResolverStyle.LENIENT) {
                            ChronoLocalDate date4 = chrono2.date(year, 1, 1).plus(map.get(ChronoField.MONTH_OF_YEAR).longValue() - 1, (TemporalUnit) ChronoUnit.MONTHS);
                            int dateDow3 = localizedDayOfWeek(date4, sow);
                            int i3 = dateDow3;
                            long month = value;
                            days2 = (7 * (value - localizedWeekOfMonth(date4, dateDow3))) + ((long) (dow2 - dateDow3));
                            date = date4;
                        } else {
                            int month2 = ChronoField.MONTH_OF_YEAR.checkValidIntValue(map.get(ChronoField.MONTH_OF_YEAR).longValue());
                            ChronoLocalDate date5 = chrono2.date(year, month2, 8);
                            int dateDow4 = localizedDayOfWeek(date5, sow);
                            long j2 = value;
                            int i4 = month2;
                            ChronoLocalDate date6 = date5;
                            days2 = (7 * (((long) this.range.checkValidIntValue(value, this)) - localizedWeekOfMonth(date5, dateDow4))) + ((long) (dow2 - dateDow4));
                            date = date6;
                        }
                        ChronoLocalDate date7 = date.plus(days2, (TemporalUnit) ChronoUnit.DAYS);
                        if (resolverStyle2 != ResolverStyle.STRICT || date7.getLong(ChronoField.MONTH_OF_YEAR) == map.get(ChronoField.MONTH_OF_YEAR).longValue()) {
                            map.remove(this);
                            map.remove(ChronoField.YEAR);
                            map.remove(ChronoField.MONTH_OF_YEAR);
                            map.remove(ChronoField.DAY_OF_WEEK);
                            return date7;
                        }
                        throw new DateTimeException("Strict mode rejected date parsed to a different month");
                    } else if (this.rangeUnit == ChronoUnit.YEARS) {
                        long value2 = map.remove(this).longValue();
                        ChronoLocalDate date8 = chrono2.date(year, 1, 1);
                        if (resolverStyle2 == ResolverStyle.LENIENT) {
                            int dateDow5 = localizedDayOfWeek(date8, sow);
                            long weeks = value2 - localizedWeekOfYear(date8, dateDow5);
                            int i5 = isoDow2;
                            Chronology chronology3 = chrono2;
                            long j3 = weeks;
                            int i6 = sow;
                            days = (7 * weeks) + ((long) (dow2 - dateDow5));
                        } else {
                            Chronology chronology4 = chrono2;
                            int isoDow3 = localizedDayOfWeek(date8, sow);
                            int i7 = sow;
                            int i8 = isoDow3;
                            days = ((long) (dow2 - isoDow3)) + (7 * (((long) this.range.checkValidIntValue(value2, this)) - localizedWeekOfYear(date8, isoDow3)));
                        }
                        ChronoLocalDate date9 = date8.plus(days, (TemporalUnit) ChronoUnit.DAYS);
                        if (resolverStyle2 != ResolverStyle.STRICT || date9.getLong(ChronoField.YEAR) == map.get(ChronoField.YEAR).longValue()) {
                            map.remove(this);
                            map.remove(ChronoField.YEAR);
                            map.remove(ChronoField.DAY_OF_WEEK);
                            return date9;
                        }
                        throw new DateTimeException("Strict mode rejected date parsed to a different year");
                    } else {
                        int i9 = isoDow2;
                        throw new IllegalStateException("unreachable");
                    }
                }
            }
        }

        public TemporalUnit getBaseUnit() {
            return this.baseUnit;
        }

        public TemporalUnit getRangeUnit() {
            return this.rangeUnit;
        }

        public ValueRange range() {
            return this.range;
        }

        public boolean isDateBased() {
            return true;
        }

        public boolean isTimeBased() {
            return false;
        }

        public boolean isSupportedBy(TemporalAccessor temporal) {
            if (!temporal.isSupported(ChronoField.DAY_OF_WEEK)) {
                return false;
            }
            if (this.rangeUnit == ChronoUnit.WEEKS) {
                return true;
            }
            if (this.rangeUnit == ChronoUnit.MONTHS) {
                return temporal.isSupported(ChronoField.DAY_OF_MONTH);
            }
            if (this.rangeUnit == ChronoUnit.YEARS) {
                return temporal.isSupported(ChronoField.DAY_OF_YEAR);
            }
            if (this.rangeUnit == IsoFields.WEEK_BASED_YEARS) {
                return temporal.isSupported(ChronoField.EPOCH_DAY);
            }
            if (this.rangeUnit == ChronoUnit.FOREVER) {
                return temporal.isSupported(ChronoField.EPOCH_DAY);
            }
            return false;
        }

        public ValueRange rangeRefinedBy(TemporalAccessor temporal) {
            TemporalField field;
            if (this.rangeUnit == ChronoUnit.WEEKS) {
                return this.range;
            }
            if (this.rangeUnit == ChronoUnit.MONTHS) {
                field = ChronoField.DAY_OF_MONTH;
            } else if (this.rangeUnit == ChronoUnit.YEARS) {
                field = ChronoField.DAY_OF_YEAR;
            } else if (this.rangeUnit == IsoFields.WEEK_BASED_YEARS) {
                return rangeWOWBY(temporal);
            } else {
                if (this.rangeUnit == ChronoUnit.FOREVER) {
                    return temporal.range(ChronoField.YEAR);
                }
                throw new IllegalStateException("unreachable");
            }
            int sow = this.weekDef.getFirstDayOfWeek().getValue();
            int offset = startOfWeekOffset(temporal.get(field), Jdk8Methods.floorMod(temporal.get(ChronoField.DAY_OF_WEEK) - sow, 7) + 1);
            ValueRange fieldRange = temporal.range(field);
            return ValueRange.of((long) computeWeek(offset, (int) fieldRange.getMinimum()), (long) computeWeek(offset, (int) fieldRange.getMaximum()));
        }

        private ValueRange rangeWOWBY(TemporalAccessor temporal) {
            int dow = Jdk8Methods.floorMod(temporal.get(ChronoField.DAY_OF_WEEK) - this.weekDef.getFirstDayOfWeek().getValue(), 7) + 1;
            long woy = localizedWeekOfYear(temporal, dow);
            if (woy == 0) {
                return rangeWOWBY(Chronology.from(temporal).date(temporal).minus(2, (TemporalUnit) ChronoUnit.WEEKS));
            }
            int weekIndexOfFirstWeekNextYear = computeWeek(startOfWeekOffset(temporal.get(ChronoField.DAY_OF_YEAR), dow), this.weekDef.getMinimalDaysInFirstWeek() + (Year.isLeap((long) temporal.get(ChronoField.YEAR)) ? 366 : 365));
            if (woy >= ((long) weekIndexOfFirstWeekNextYear)) {
                return rangeWOWBY(Chronology.from(temporal).date(temporal).plus(2, (TemporalUnit) ChronoUnit.WEEKS));
            }
            return ValueRange.of(1, (long) (weekIndexOfFirstWeekNextYear - 1));
        }

        public String getDisplayName(Locale locale) {
            Jdk8Methods.requireNonNull(locale, "locale");
            if (this.rangeUnit == ChronoUnit.YEARS) {
                return "Week";
            }
            return toString();
        }

        public String toString() {
            return this.name + "[" + this.weekDef.toString() + "]";
        }
    }
}
