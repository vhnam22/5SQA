package org.threeten.bp.chrono;

import java.io.BufferedReader;
import java.io.DataInput;
import java.io.DataOutput;
import java.io.File;
import java.io.FileInputStream;
import java.io.IOException;
import java.io.InputStream;
import java.io.InputStreamReader;
import java.io.Serializable;
import java.text.ParseException;
import java.util.HashMap;
import java.util.StringTokenizer;
import java.util.zip.ZipEntry;
import java.util.zip.ZipFile;
import org.threeten.bp.Clock;
import org.threeten.bp.DateTimeException;
import org.threeten.bp.DayOfWeek;
import org.threeten.bp.LocalDate;
import org.threeten.bp.LocalTime;
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

public final class HijrahDate extends ChronoDateImpl<HijrahDate> implements Serializable {
    private static final Long[] ADJUSTED_CYCLES = new Long[MAX_ADJUSTED_CYCLE];
    private static final HashMap<Integer, Integer[]> ADJUSTED_CYCLE_YEARS = new HashMap<>();
    private static final Integer[] ADJUSTED_LEAST_MAX_VALUES = new Integer[LEAST_MAX_VALUES.length];
    private static final Integer[] ADJUSTED_MAX_VALUES = new Integer[MAX_VALUES.length];
    private static final Integer[] ADJUSTED_MIN_VALUES = new Integer[MIN_VALUES.length];
    private static final HashMap<Integer, Integer[]> ADJUSTED_MONTH_DAYS = new HashMap<>();
    private static final HashMap<Integer, Integer[]> ADJUSTED_MONTH_LENGTHS = new HashMap<>();
    private static final int[] CYCLEYEAR_START_DATE = {0, 354, 709, 1063, 1417, 1772, 2126, 2481, 2835, 3189, 3544, 3898, 4252, 4607, 4961, 5315, 5670, 6024, 6379, 6733, 7087, 7442, 7796, 8150, 8505, 8859, 9214, 9568, 9922, 10277};
    private static final String DEFAULT_CONFIG_FILENAME = "hijrah_deviation.cfg";
    private static final String DEFAULT_CONFIG_PATH;
    private static final Integer[] DEFAULT_CYCLE_YEARS = new Integer[CYCLEYEAR_START_DATE.length];
    private static final Integer[] DEFAULT_LEAP_MONTH_DAYS = new Integer[LEAP_NUM_DAYS.length];
    private static final Integer[] DEFAULT_LEAP_MONTH_LENGTHS = new Integer[LEAP_MONTH_LENGTH.length];
    private static final Integer[] DEFAULT_MONTH_DAYS;
    private static final Integer[] DEFAULT_MONTH_LENGTHS = new Integer[MONTH_LENGTH.length];
    private static final char FILE_SEP;
    private static final int HIJRAH_JAN_1_1_GREGORIAN_DAY = -492148;
    private static final int[] LEAP_MONTH_LENGTH = {30, 29, 30, 29, 30, 29, 30, 29, 30, 29, 30, 30};
    private static final int[] LEAP_NUM_DAYS = {0, 30, 59, 89, 118, 148, 177, 207, 236, 266, 295, 325};
    private static final int[] LEAST_MAX_VALUES = {1, MAX_VALUE_OF_ERA, 11, 51, 5, 29, 354};
    private static final int MAX_ADJUSTED_CYCLE = 334;
    private static final int[] MAX_VALUES = {1, MAX_VALUE_OF_ERA, 11, 52, 6, 30, 355};
    public static final int MAX_VALUE_OF_ERA = 9999;
    private static final int[] MIN_VALUES = {0, 1, 0, 1, 0, 1, 1};
    public static final int MIN_VALUE_OF_ERA = 1;
    private static final int[] MONTH_LENGTH = {30, 29, 30, 29, 30, 29, 30, 29, 30, 29, 30, 29};
    private static final int[] NUM_DAYS;
    private static final String PATH_SEP = File.pathSeparator;
    private static final int POSITION_DAY_OF_MONTH = 5;
    private static final int POSITION_DAY_OF_YEAR = 6;
    private static final long serialVersionUID = -5207853542612002020L;
    private final transient int dayOfMonth;
    private final transient DayOfWeek dayOfWeek;
    private final transient int dayOfYear;
    private final transient HijrahEra era;
    private final long gregorianEpochDay;
    private final transient boolean isLeapYear;
    private final transient int monthOfYear;
    private final transient int yearOfEra;

    public /* bridge */ /* synthetic */ long until(Temporal x0, TemporalUnit x1) {
        return super.until(x0, x1);
    }

    public /* bridge */ /* synthetic */ ChronoPeriod until(ChronoLocalDate x0) {
        return super.until(x0);
    }

    static {
        int[] iArr = {0, 30, 59, 89, 118, 148, 177, 207, 236, 266, 295, 325};
        NUM_DAYS = iArr;
        char c = File.separatorChar;
        FILE_SEP = c;
        DEFAULT_CONFIG_PATH = "org" + c + "threeten" + c + "bp" + c + "chrono";
        DEFAULT_MONTH_DAYS = new Integer[iArr.length];
        int i = 0;
        while (true) {
            int[] iArr2 = NUM_DAYS;
            if (i >= iArr2.length) {
                break;
            }
            DEFAULT_MONTH_DAYS[i] = new Integer(iArr2[i]);
            i++;
        }
        int i2 = 0;
        while (true) {
            int[] iArr3 = LEAP_NUM_DAYS;
            if (i2 >= iArr3.length) {
                break;
            }
            DEFAULT_LEAP_MONTH_DAYS[i2] = new Integer(iArr3[i2]);
            i2++;
        }
        int i3 = 0;
        while (true) {
            int[] iArr4 = MONTH_LENGTH;
            if (i3 >= iArr4.length) {
                break;
            }
            DEFAULT_MONTH_LENGTHS[i3] = new Integer(iArr4[i3]);
            i3++;
        }
        int i4 = 0;
        while (true) {
            int[] iArr5 = LEAP_MONTH_LENGTH;
            if (i4 >= iArr5.length) {
                break;
            }
            DEFAULT_LEAP_MONTH_LENGTHS[i4] = new Integer(iArr5[i4]);
            i4++;
        }
        int i5 = 0;
        while (true) {
            int[] iArr6 = CYCLEYEAR_START_DATE;
            if (i5 >= iArr6.length) {
                break;
            }
            DEFAULT_CYCLE_YEARS[i5] = new Integer(iArr6[i5]);
            i5++;
        }
        int i6 = 0;
        while (true) {
            Long[] lArr = ADJUSTED_CYCLES;
            if (i6 >= lArr.length) {
                break;
            }
            lArr[i6] = new Long((long) (i6 * 10631));
            i6++;
        }
        int i7 = 0;
        while (true) {
            int[] iArr7 = MIN_VALUES;
            if (i7 >= iArr7.length) {
                break;
            }
            ADJUSTED_MIN_VALUES[i7] = new Integer(iArr7[i7]);
            i7++;
        }
        int i8 = 0;
        while (true) {
            int[] iArr8 = LEAST_MAX_VALUES;
            if (i8 >= iArr8.length) {
                break;
            }
            ADJUSTED_LEAST_MAX_VALUES[i8] = new Integer(iArr8[i8]);
            i8++;
        }
        int i9 = 0;
        while (true) {
            int[] iArr9 = MAX_VALUES;
            if (i9 < iArr9.length) {
                ADJUSTED_MAX_VALUES[i9] = new Integer(iArr9[i9]);
                i9++;
            } else {
                try {
                    readDeviationConfig();
                    return;
                } catch (IOException | ParseException e) {
                    return;
                }
            }
        }
    }

    public static HijrahDate now() {
        return now(Clock.systemDefaultZone());
    }

    public static HijrahDate now(ZoneId zone) {
        return now(Clock.system(zone));
    }

    public static HijrahDate now(Clock clock) {
        return HijrahChronology.INSTANCE.dateNow(clock);
    }

    public static HijrahDate of(int prolepticYear, int monthOfYear2, int dayOfMonth2) {
        return prolepticYear >= 1 ? of(HijrahEra.AH, prolepticYear, monthOfYear2, dayOfMonth2) : of(HijrahEra.BEFORE_AH, 1 - prolepticYear, monthOfYear2, dayOfMonth2);
    }

    static HijrahDate of(HijrahEra era2, int yearOfEra2, int monthOfYear2, int dayOfMonth2) {
        Jdk8Methods.requireNonNull(era2, "era");
        checkValidYearOfEra(yearOfEra2);
        checkValidMonth(monthOfYear2);
        checkValidDayOfMonth(dayOfMonth2);
        return new HijrahDate(getGregorianEpochDay(era2.prolepticYear(yearOfEra2), monthOfYear2, dayOfMonth2));
    }

    private static void checkValidYearOfEra(int yearOfEra2) {
        if (yearOfEra2 < 1 || yearOfEra2 > 9999) {
            throw new DateTimeException("Invalid year of Hijrah Era");
        }
    }

    private static void checkValidDayOfYear(int dayOfYear2) {
        if (dayOfYear2 < 1 || dayOfYear2 > getMaximumDayOfYear()) {
            throw new DateTimeException("Invalid day of year of Hijrah date");
        }
    }

    private static void checkValidMonth(int month) {
        if (month < 1 || month > 12) {
            throw new DateTimeException("Invalid month of Hijrah date");
        }
    }

    private static void checkValidDayOfMonth(int dayOfMonth2) {
        if (dayOfMonth2 < 1 || dayOfMonth2 > getMaximumDayOfMonth()) {
            throw new DateTimeException("Invalid day of month of Hijrah date, day " + dayOfMonth2 + " greater than " + getMaximumDayOfMonth() + " or less than 1");
        }
    }

    static HijrahDate of(LocalDate date) {
        return new HijrahDate(date.toEpochDay());
    }

    static HijrahDate ofEpochDay(long epochDay) {
        return new HijrahDate(epochDay);
    }

    public static HijrahDate from(TemporalAccessor temporal) {
        return HijrahChronology.INSTANCE.date(temporal);
    }

    private HijrahDate(long gregorianDay) {
        int[] dateInfo = getHijrahDateInfo(gregorianDay);
        checkValidYearOfEra(dateInfo[1]);
        checkValidMonth(dateInfo[2]);
        checkValidDayOfMonth(dateInfo[3]);
        checkValidDayOfYear(dateInfo[4]);
        this.era = HijrahEra.of(dateInfo[0]);
        int i = dateInfo[1];
        this.yearOfEra = i;
        this.monthOfYear = dateInfo[2];
        this.dayOfMonth = dateInfo[3];
        this.dayOfYear = dateInfo[4];
        this.dayOfWeek = DayOfWeek.of(dateInfo[5]);
        this.gregorianEpochDay = gregorianDay;
        this.isLeapYear = isLeapYear((long) i);
    }

    private Object readResolve() {
        return new HijrahDate(this.gregorianEpochDay);
    }

    public HijrahChronology getChronology() {
        return HijrahChronology.INSTANCE;
    }

    public HijrahEra getEra() {
        return this.era;
    }

    public ValueRange range(TemporalField field) {
        if (!(field instanceof ChronoField)) {
            return field.rangeRefinedBy(this);
        }
        if (isSupported(field)) {
            ChronoField f = (ChronoField) field;
            switch (AnonymousClass1.$SwitchMap$org$threeten$bp$temporal$ChronoField[f.ordinal()]) {
                case 1:
                    return ValueRange.of(1, (long) lengthOfMonth());
                case 2:
                    return ValueRange.of(1, (long) lengthOfYear());
                case 3:
                    return ValueRange.of(1, 5);
                case 4:
                    return ValueRange.of(1, 1000);
                default:
                    return getChronology().range(f);
            }
        } else {
            throw new UnsupportedTemporalTypeException("Unsupported field: " + field);
        }
    }

    /* renamed from: org.threeten.bp.chrono.HijrahDate$1  reason: invalid class name */
    static /* synthetic */ class AnonymousClass1 {
        static final /* synthetic */ int[] $SwitchMap$org$threeten$bp$temporal$ChronoField;

        static {
            int[] iArr = new int[ChronoField.values().length];
            $SwitchMap$org$threeten$bp$temporal$ChronoField = iArr;
            try {
                iArr[ChronoField.DAY_OF_MONTH.ordinal()] = 1;
            } catch (NoSuchFieldError e) {
            }
            try {
                $SwitchMap$org$threeten$bp$temporal$ChronoField[ChronoField.DAY_OF_YEAR.ordinal()] = 2;
            } catch (NoSuchFieldError e2) {
            }
            try {
                $SwitchMap$org$threeten$bp$temporal$ChronoField[ChronoField.ALIGNED_WEEK_OF_MONTH.ordinal()] = 3;
            } catch (NoSuchFieldError e3) {
            }
            try {
                $SwitchMap$org$threeten$bp$temporal$ChronoField[ChronoField.YEAR_OF_ERA.ordinal()] = 4;
            } catch (NoSuchFieldError e4) {
            }
            try {
                $SwitchMap$org$threeten$bp$temporal$ChronoField[ChronoField.DAY_OF_WEEK.ordinal()] = 5;
            } catch (NoSuchFieldError e5) {
            }
            try {
                $SwitchMap$org$threeten$bp$temporal$ChronoField[ChronoField.ALIGNED_DAY_OF_WEEK_IN_MONTH.ordinal()] = 6;
            } catch (NoSuchFieldError e6) {
            }
            try {
                $SwitchMap$org$threeten$bp$temporal$ChronoField[ChronoField.ALIGNED_DAY_OF_WEEK_IN_YEAR.ordinal()] = 7;
            } catch (NoSuchFieldError e7) {
            }
            try {
                $SwitchMap$org$threeten$bp$temporal$ChronoField[ChronoField.EPOCH_DAY.ordinal()] = 8;
            } catch (NoSuchFieldError e8) {
            }
            try {
                $SwitchMap$org$threeten$bp$temporal$ChronoField[ChronoField.ALIGNED_WEEK_OF_YEAR.ordinal()] = 9;
            } catch (NoSuchFieldError e9) {
            }
            try {
                $SwitchMap$org$threeten$bp$temporal$ChronoField[ChronoField.MONTH_OF_YEAR.ordinal()] = 10;
            } catch (NoSuchFieldError e10) {
            }
            try {
                $SwitchMap$org$threeten$bp$temporal$ChronoField[ChronoField.YEAR.ordinal()] = 11;
            } catch (NoSuchFieldError e11) {
            }
            try {
                $SwitchMap$org$threeten$bp$temporal$ChronoField[ChronoField.ERA.ordinal()] = 12;
            } catch (NoSuchFieldError e12) {
            }
        }
    }

    public long getLong(TemporalField field) {
        if (!(field instanceof ChronoField)) {
            return field.getFrom(this);
        }
        switch (AnonymousClass1.$SwitchMap$org$threeten$bp$temporal$ChronoField[((ChronoField) field).ordinal()]) {
            case 1:
                return (long) this.dayOfMonth;
            case 2:
                return (long) this.dayOfYear;
            case 3:
                return (long) (((this.dayOfMonth - 1) / 7) + 1);
            case 4:
                return (long) this.yearOfEra;
            case 5:
                return (long) this.dayOfWeek.getValue();
            case 6:
                return (long) (((this.dayOfMonth - 1) % 7) + 1);
            case 7:
                return (long) (((this.dayOfYear - 1) % 7) + 1);
            case 8:
                return toEpochDay();
            case 9:
                return (long) (((this.dayOfYear - 1) / 7) + 1);
            case 10:
                return (long) this.monthOfYear;
            case 11:
                return (long) this.yearOfEra;
            case 12:
                return (long) this.era.getValue();
            default:
                throw new UnsupportedTemporalTypeException("Unsupported field: " + field);
        }
    }

    public HijrahDate with(TemporalAdjuster adjuster) {
        return (HijrahDate) super.with(adjuster);
    }

    public HijrahDate with(TemporalField field, long newValue) {
        if (!(field instanceof ChronoField)) {
            return (HijrahDate) field.adjustInto(this, newValue);
        }
        ChronoField f = (ChronoField) field;
        f.checkValidValue(newValue);
        int nvalue = (int) newValue;
        switch (AnonymousClass1.$SwitchMap$org$threeten$bp$temporal$ChronoField[f.ordinal()]) {
            case 1:
                return resolvePreviousValid(this.yearOfEra, this.monthOfYear, nvalue);
            case 2:
                return resolvePreviousValid(this.yearOfEra, ((nvalue - 1) / 30) + 1, ((nvalue - 1) % 30) + 1);
            case 3:
                return plusDays((newValue - getLong(ChronoField.ALIGNED_WEEK_OF_MONTH)) * 7);
            case 4:
                return resolvePreviousValid(this.yearOfEra >= 1 ? nvalue : 1 - nvalue, this.monthOfYear, this.dayOfMonth);
            case 5:
                return plusDays(newValue - ((long) this.dayOfWeek.getValue()));
            case 6:
                return plusDays(newValue - getLong(ChronoField.ALIGNED_DAY_OF_WEEK_IN_MONTH));
            case 7:
                return plusDays(newValue - getLong(ChronoField.ALIGNED_DAY_OF_WEEK_IN_YEAR));
            case 8:
                return new HijrahDate((long) nvalue);
            case 9:
                return plusDays((newValue - getLong(ChronoField.ALIGNED_WEEK_OF_YEAR)) * 7);
            case 10:
                return resolvePreviousValid(this.yearOfEra, nvalue, this.dayOfMonth);
            case 11:
                return resolvePreviousValid(nvalue, this.monthOfYear, this.dayOfMonth);
            case 12:
                return resolvePreviousValid(1 - this.yearOfEra, this.monthOfYear, this.dayOfMonth);
            default:
                throw new UnsupportedTemporalTypeException("Unsupported field: " + field);
        }
    }

    private static HijrahDate resolvePreviousValid(int yearOfEra2, int month, int day) {
        int monthDays = getMonthDays(month - 1, yearOfEra2);
        if (day > monthDays) {
            day = monthDays;
        }
        return of(yearOfEra2, month, day);
    }

    public HijrahDate plus(TemporalAmount amount) {
        return (HijrahDate) super.plus(amount);
    }

    public HijrahDate plus(long amountToAdd, TemporalUnit unit) {
        return (HijrahDate) super.plus(amountToAdd, unit);
    }

    public HijrahDate minus(TemporalAmount amount) {
        return (HijrahDate) super.minus(amount);
    }

    public HijrahDate minus(long amountToAdd, TemporalUnit unit) {
        return (HijrahDate) super.minus(amountToAdd, unit);
    }

    public final ChronoLocalDateTime<HijrahDate> atTime(LocalTime localTime) {
        return super.atTime(localTime);
    }

    public long toEpochDay() {
        return getGregorianEpochDay(this.yearOfEra, this.monthOfYear, this.dayOfMonth);
    }

    public boolean isLeapYear() {
        return this.isLeapYear;
    }

    /* access modifiers changed from: package-private */
    public HijrahDate plusYears(long years) {
        if (years == 0) {
            return this;
        }
        return of(this.era, Jdk8Methods.safeAdd(this.yearOfEra, (int) years), this.monthOfYear, this.dayOfMonth);
    }

    /* access modifiers changed from: package-private */
    public HijrahDate plusMonths(long months) {
        if (months == 0) {
            return this;
        }
        int newMonth = (this.monthOfYear - 1) + ((int) months);
        int years = newMonth / 12;
        int newMonth2 = newMonth % 12;
        while (newMonth2 < 0) {
            newMonth2 += 12;
            years = Jdk8Methods.safeSubtract(years, 1);
        }
        return of(this.era, Jdk8Methods.safeAdd(this.yearOfEra, years), newMonth2 + 1, this.dayOfMonth);
    }

    /* access modifiers changed from: package-private */
    public HijrahDate plusDays(long days) {
        return new HijrahDate(this.gregorianEpochDay + days);
    }

    private static int[] getHijrahDateInfo(long gregorianDays) {
        int era2;
        int date;
        int month;
        int year;
        int dayOfYear2;
        long epochDay = gregorianDays - -492148;
        if (epochDay >= 0) {
            int cycleNumber = getCycleNumber(epochDay);
            int dayOfCycle = getDayOfCycle(epochDay, cycleNumber);
            int yearInCycle = getYearInCycle(cycleNumber, (long) dayOfCycle);
            dayOfYear2 = getDayOfYear(cycleNumber, dayOfCycle, yearInCycle);
            year = (cycleNumber * 30) + yearInCycle + 1;
            month = getMonthOfYear(dayOfYear2, year);
            date = getDayOfMonth(dayOfYear2, month, year) + 1;
            era2 = HijrahEra.AH.getValue();
        } else {
            int cycleNumber2 = ((int) epochDay) / 10631;
            int dayOfCycle2 = ((int) epochDay) % 10631;
            if (dayOfCycle2 == 0) {
                dayOfCycle2 = -10631;
                cycleNumber2++;
            }
            int yearInCycle2 = getYearInCycle(cycleNumber2, (long) dayOfCycle2);
            int dayOfYear3 = getDayOfYear(cycleNumber2, dayOfCycle2, yearInCycle2);
            year = 1 - ((cycleNumber2 * 30) - yearInCycle2);
            dayOfYear2 = isLeapYear((long) year) ? dayOfYear3 + 355 : dayOfYear3 + 354;
            month = getMonthOfYear(dayOfYear2, year);
            date = getDayOfMonth(dayOfYear2, month, year) + 1;
            era2 = HijrahEra.BEFORE_AH.getValue();
        }
        int dayOfWeek2 = (int) ((5 + epochDay) % 7);
        return new int[]{era2, year, month + 1, date, dayOfYear2 + 1, dayOfWeek2 + (dayOfWeek2 <= 0 ? 7 : 0)};
    }

    private static long getGregorianEpochDay(int prolepticYear, int monthOfYear2, int dayOfMonth2) {
        return yearToGregorianEpochDay(prolepticYear) + ((long) getMonthDays(monthOfYear2 - 1, prolepticYear)) + ((long) dayOfMonth2);
    }

    private static long yearToGregorianEpochDay(int prolepticYear) {
        Long cycleDays;
        int cycleNumber = (prolepticYear - 1) / 30;
        int yearInCycle = (prolepticYear - 1) % 30;
        int dayInCycle = getAdjustedCycle(cycleNumber)[Math.abs(yearInCycle)].intValue();
        if (yearInCycle < 0) {
            dayInCycle = -dayInCycle;
        }
        try {
            cycleDays = ADJUSTED_CYCLES[cycleNumber];
        } catch (ArrayIndexOutOfBoundsException e) {
            cycleDays = null;
        }
        if (cycleDays == null) {
            cycleDays = new Long((long) (cycleNumber * 10631));
        }
        return ((cycleDays.longValue() + ((long) dayInCycle)) - 492148) - 1;
    }

    private static int getCycleNumber(long epochDay) {
        Long[] days = ADJUSTED_CYCLES;
        int i = 0;
        while (i < days.length) {
            try {
                if (epochDay < days[i].longValue()) {
                    return i - 1;
                }
                i++;
            } catch (ArrayIndexOutOfBoundsException e) {
                return ((int) epochDay) / 10631;
            }
        }
        return ((int) epochDay) / 10631;
    }

    private static int getDayOfCycle(long epochDay, int cycleNumber) {
        Long day;
        try {
            day = ADJUSTED_CYCLES[cycleNumber];
        } catch (ArrayIndexOutOfBoundsException e) {
            day = null;
        }
        if (day == null) {
            day = new Long((long) (cycleNumber * 10631));
        }
        return (int) (epochDay - day.longValue());
    }

    private static int getYearInCycle(int cycleNumber, long dayOfCycle) {
        Integer[] cycles = getAdjustedCycle(cycleNumber);
        if (dayOfCycle == 0) {
            return 0;
        }
        if (dayOfCycle > 0) {
            for (int i = 0; i < cycles.length; i++) {
                if (dayOfCycle < ((long) cycles[i].intValue())) {
                    return i - 1;
                }
            }
            return 29;
        }
        long dayOfCycle2 = -dayOfCycle;
        for (int i2 = 0; i2 < cycles.length; i2++) {
            if (dayOfCycle2 <= ((long) cycles[i2].intValue())) {
                return i2 - 1;
            }
        }
        return 29;
    }

    private static Integer[] getAdjustedCycle(int cycleNumber) {
        Integer[] cycles;
        try {
            cycles = ADJUSTED_CYCLE_YEARS.get(new Integer(cycleNumber));
        } catch (ArrayIndexOutOfBoundsException e) {
            cycles = null;
        }
        if (cycles == null) {
            return DEFAULT_CYCLE_YEARS;
        }
        return cycles;
    }

    private static Integer[] getAdjustedMonthDays(int year) {
        Integer[] newMonths;
        try {
            newMonths = ADJUSTED_MONTH_DAYS.get(new Integer(year));
        } catch (ArrayIndexOutOfBoundsException e) {
            newMonths = null;
        }
        if (newMonths != null) {
            return newMonths;
        }
        if (isLeapYear((long) year)) {
            return DEFAULT_LEAP_MONTH_DAYS;
        }
        return DEFAULT_MONTH_DAYS;
    }

    private static Integer[] getAdjustedMonthLength(int year) {
        Integer[] newMonths;
        try {
            newMonths = ADJUSTED_MONTH_LENGTHS.get(new Integer(year));
        } catch (ArrayIndexOutOfBoundsException e) {
            newMonths = null;
        }
        if (newMonths != null) {
            return newMonths;
        }
        if (isLeapYear((long) year)) {
            return DEFAULT_LEAP_MONTH_LENGTHS;
        }
        return DEFAULT_MONTH_LENGTHS;
    }

    private static int getDayOfYear(int cycleNumber, int dayOfCycle, int yearInCycle) {
        Integer[] cycles = getAdjustedCycle(cycleNumber);
        if (dayOfCycle > 0) {
            return dayOfCycle - cycles[yearInCycle].intValue();
        }
        return cycles[yearInCycle].intValue() + dayOfCycle;
    }

    private static int getMonthOfYear(int dayOfYear2, int year) {
        Integer[] newMonths = getAdjustedMonthDays(year);
        if (dayOfYear2 >= 0) {
            for (int i = 0; i < newMonths.length; i++) {
                if (dayOfYear2 < newMonths[i].intValue()) {
                    return i - 1;
                }
            }
            return 11;
        }
        int dayOfYear3 = isLeapYear((long) year) ? dayOfYear2 + 355 : dayOfYear2 + 354;
        for (int i2 = 0; i2 < newMonths.length; i2++) {
            if (dayOfYear3 < newMonths[i2].intValue()) {
                return i2 - 1;
            }
        }
        return 11;
    }

    private static int getDayOfMonth(int dayOfYear2, int month, int year) {
        Integer[] newMonths = getAdjustedMonthDays(year);
        if (dayOfYear2 < 0) {
            int dayOfYear3 = isLeapYear((long) year) ? dayOfYear2 + 355 : dayOfYear2 + 354;
            if (month > 0) {
                return dayOfYear3 - newMonths[month].intValue();
            }
            return dayOfYear3;
        } else if (month > 0) {
            return dayOfYear2 - newMonths[month].intValue();
        } else {
            return dayOfYear2;
        }
    }

    static boolean isLeapYear(long year) {
        return ((((year > 0 ? 1 : (year == 0 ? 0 : -1)) > 0 ? year : -year) * 11) + 14) % 30 < 11;
    }

    private static int getMonthDays(int month, int year) {
        return getAdjustedMonthDays(year)[month].intValue();
    }

    static int getMonthLength(int month, int year) {
        return getAdjustedMonthLength(year)[month].intValue();
    }

    public int lengthOfMonth() {
        return getMonthLength(this.monthOfYear - 1, this.yearOfEra);
    }

    static int getYearLength(int year) {
        Integer[] cycleYears;
        int cycleNumber = (year - 1) / 30;
        try {
            cycleYears = ADJUSTED_CYCLE_YEARS.get(Integer.valueOf(cycleNumber));
        } catch (ArrayIndexOutOfBoundsException e) {
            cycleYears = null;
        }
        if (cycleYears == null) {
            return isLeapYear((long) year) ? 355 : 354;
        }
        int yearInCycle = (year - 1) % 30;
        if (yearInCycle != 29) {
            return cycleYears[yearInCycle + 1].intValue() - cycleYears[yearInCycle].intValue();
        }
        Long[] lArr = ADJUSTED_CYCLES;
        return (lArr[cycleNumber + 1].intValue() - lArr[cycleNumber].intValue()) - cycleYears[yearInCycle].intValue();
    }

    public int lengthOfYear() {
        return getYearLength(this.yearOfEra);
    }

    static int getMaximumDayOfMonth() {
        return ADJUSTED_MAX_VALUES[5].intValue();
    }

    static int getSmallestMaximumDayOfMonth() {
        return ADJUSTED_LEAST_MAX_VALUES[5].intValue();
    }

    static int getMaximumDayOfYear() {
        return ADJUSTED_MAX_VALUES[6].intValue();
    }

    static int getSmallestMaximumDayOfYear() {
        return ADJUSTED_LEAST_MAX_VALUES[6].intValue();
    }

    private static void addDeviationAsHijrah(int startYear, int startMonth, int endYear, int endMonth, int offset) {
        int i = startYear;
        int i2 = startMonth;
        int i3 = endYear;
        int i4 = endMonth;
        int i5 = offset;
        if (i < 1) {
            throw new IllegalArgumentException("startYear < 1");
        } else if (i3 < 1) {
            throw new IllegalArgumentException("endYear < 1");
        } else if (i2 < 0 || i2 > 11) {
            throw new IllegalArgumentException("startMonth < 0 || startMonth > 11");
        } else if (i4 < 0 || i4 > 11) {
            throw new IllegalArgumentException("endMonth < 0 || endMonth > 11");
        } else if (i3 > 9999) {
            throw new IllegalArgumentException("endYear > 9999");
        } else if (i3 < i) {
            throw new IllegalArgumentException("startYear > endYear");
        } else if (i3 != i || i4 >= i2) {
            boolean isStartYLeap = isLeapYear((long) i);
            Integer[] orgStartMonthNums = ADJUSTED_MONTH_DAYS.get(new Integer(i));
            if (orgStartMonthNums == null) {
                if (!isStartYLeap) {
                    orgStartMonthNums = new Integer[NUM_DAYS.length];
                    int l = 0;
                    while (true) {
                        int[] iArr = NUM_DAYS;
                        if (l >= iArr.length) {
                            break;
                        }
                        orgStartMonthNums[l] = new Integer(iArr[l]);
                        l++;
                    }
                } else {
                    orgStartMonthNums = new Integer[LEAP_NUM_DAYS.length];
                    int l2 = 0;
                    while (true) {
                        int[] iArr2 = LEAP_NUM_DAYS;
                        if (l2 >= iArr2.length) {
                            break;
                        }
                        orgStartMonthNums[l2] = new Integer(iArr2[l2]);
                        l2++;
                    }
                }
            }
            Integer[] newStartMonthNums = new Integer[orgStartMonthNums.length];
            for (int month = 0; month < 12; month++) {
                if (month > i2) {
                    newStartMonthNums[month] = new Integer(orgStartMonthNums[month].intValue() - i5);
                } else {
                    newStartMonthNums[month] = new Integer(orgStartMonthNums[month].intValue());
                }
            }
            ADJUSTED_MONTH_DAYS.put(new Integer(i), newStartMonthNums);
            Integer[] orgStartMonthLengths = ADJUSTED_MONTH_LENGTHS.get(new Integer(i));
            if (orgStartMonthLengths == null) {
                if (!isStartYLeap) {
                    orgStartMonthLengths = new Integer[MONTH_LENGTH.length];
                    int l3 = 0;
                    while (true) {
                        int[] iArr3 = MONTH_LENGTH;
                        if (l3 >= iArr3.length) {
                            break;
                        }
                        orgStartMonthLengths[l3] = new Integer(iArr3[l3]);
                        l3++;
                    }
                } else {
                    orgStartMonthLengths = new Integer[LEAP_MONTH_LENGTH.length];
                    int l4 = 0;
                    while (true) {
                        int[] iArr4 = LEAP_MONTH_LENGTH;
                        if (l4 >= iArr4.length) {
                            break;
                        }
                        orgStartMonthLengths[l4] = new Integer(iArr4[l4]);
                        l4++;
                    }
                }
            }
            Integer[] newStartMonthLengths = new Integer[orgStartMonthLengths.length];
            for (int month2 = 0; month2 < 12; month2++) {
                if (month2 == i2) {
                    newStartMonthLengths[month2] = new Integer(orgStartMonthLengths[month2].intValue() - i5);
                } else {
                    newStartMonthLengths[month2] = new Integer(orgStartMonthLengths[month2].intValue());
                }
            }
            ADJUSTED_MONTH_LENGTHS.put(new Integer(i), newStartMonthLengths);
            if (i != i3) {
                int sCycleNumber = (i - 1) / 30;
                int sYearInCycle = (i - 1) % 30;
                Integer[] startCycles = ADJUSTED_CYCLE_YEARS.get(new Integer(sCycleNumber));
                if (startCycles == null) {
                    startCycles = new Integer[CYCLEYEAR_START_DATE.length];
                    for (int j = 0; j < startCycles.length; j++) {
                        startCycles[j] = new Integer(CYCLEYEAR_START_DATE[j]);
                    }
                }
                for (int j2 = sYearInCycle + 1; j2 < CYCLEYEAR_START_DATE.length; j2++) {
                    startCycles[j2] = new Integer(startCycles[j2].intValue() - i5);
                }
                ADJUSTED_CYCLE_YEARS.put(new Integer(sCycleNumber), startCycles);
                int sYearInMaxY = (i - 1) / 30;
                int sEndInMaxY = (i3 - 1) / 30;
                if (sYearInMaxY != sEndInMaxY) {
                    int j3 = sYearInMaxY + 1;
                    while (true) {
                        int sYearInMaxY2 = sYearInMaxY;
                        Long[] lArr = ADJUSTED_CYCLES;
                        boolean isStartYLeap2 = isStartYLeap;
                        if (j3 >= lArr.length) {
                            break;
                        }
                        lArr[j3] = new Long(lArr[j3].longValue() - ((long) i5));
                        j3++;
                        sYearInMaxY = sYearInMaxY2;
                        isStartYLeap = isStartYLeap2;
                        orgStartMonthNums = orgStartMonthNums;
                        newStartMonthNums = newStartMonthNums;
                    }
                    Integer[] numArr = newStartMonthNums;
                    int j4 = sEndInMaxY + 1;
                    while (true) {
                        Long[] lArr2 = ADJUSTED_CYCLES;
                        if (j4 >= lArr2.length) {
                            break;
                        }
                        lArr2[j4] = new Long(lArr2[j4].longValue() + ((long) i5));
                        j4++;
                        orgStartMonthLengths = orgStartMonthLengths;
                    }
                } else {
                    boolean z = isStartYLeap;
                    Integer[] numArr2 = orgStartMonthNums;
                    Integer[] numArr3 = newStartMonthNums;
                    Integer[] numArr4 = orgStartMonthLengths;
                }
                int eCycleNumber = (i3 - 1) / 30;
                int sEndInCycle = (i3 - 1) % 30;
                Integer[] endCycles = ADJUSTED_CYCLE_YEARS.get(new Integer(eCycleNumber));
                if (endCycles == null) {
                    endCycles = new Integer[CYCLEYEAR_START_DATE.length];
                    int j5 = 0;
                    while (j5 < endCycles.length) {
                        endCycles[j5] = new Integer(CYCLEYEAR_START_DATE[j5]);
                        j5++;
                        sEndInMaxY = sEndInMaxY;
                    }
                }
                for (int j6 = sEndInCycle + 1; j6 < CYCLEYEAR_START_DATE.length; j6++) {
                    endCycles[j6] = new Integer(endCycles[j6].intValue() + i5);
                }
                ADJUSTED_CYCLE_YEARS.put(new Integer(eCycleNumber), endCycles);
            } else {
                Integer[] numArr5 = orgStartMonthNums;
                Integer[] numArr6 = newStartMonthNums;
                Integer[] numArr7 = orgStartMonthLengths;
            }
            boolean isEndYLeap = isLeapYear((long) i3);
            Integer[] orgEndMonthDays = ADJUSTED_MONTH_DAYS.get(new Integer(i3));
            if (orgEndMonthDays == null) {
                if (!isEndYLeap) {
                    orgEndMonthDays = new Integer[NUM_DAYS.length];
                    int l5 = 0;
                    while (true) {
                        int[] iArr5 = NUM_DAYS;
                        if (l5 >= iArr5.length) {
                            break;
                        }
                        orgEndMonthDays[l5] = new Integer(iArr5[l5]);
                        l5++;
                    }
                } else {
                    orgEndMonthDays = new Integer[LEAP_NUM_DAYS.length];
                    int l6 = 0;
                    while (true) {
                        int[] iArr6 = LEAP_NUM_DAYS;
                        if (l6 >= iArr6.length) {
                            break;
                        }
                        orgEndMonthDays[l6] = new Integer(iArr6[l6]);
                        l6++;
                    }
                }
            }
            Integer[] newEndMonthDays = new Integer[orgEndMonthDays.length];
            for (int month3 = 0; month3 < 12; month3++) {
                if (month3 > i4) {
                    newEndMonthDays[month3] = new Integer(orgEndMonthDays[month3].intValue() + i5);
                } else {
                    newEndMonthDays[month3] = new Integer(orgEndMonthDays[month3].intValue());
                }
            }
            ADJUSTED_MONTH_DAYS.put(new Integer(i3), newEndMonthDays);
            Integer[] orgEndMonthLengths = ADJUSTED_MONTH_LENGTHS.get(new Integer(i3));
            if (orgEndMonthLengths == null) {
                if (!isEndYLeap) {
                    orgEndMonthLengths = new Integer[MONTH_LENGTH.length];
                    int l7 = 0;
                    while (true) {
                        int[] iArr7 = MONTH_LENGTH;
                        if (l7 >= iArr7.length) {
                            break;
                        }
                        orgEndMonthLengths[l7] = new Integer(iArr7[l7]);
                        l7++;
                    }
                } else {
                    orgEndMonthLengths = new Integer[LEAP_MONTH_LENGTH.length];
                    int l8 = 0;
                    while (true) {
                        int[] iArr8 = LEAP_MONTH_LENGTH;
                        if (l8 >= iArr8.length) {
                            break;
                        }
                        orgEndMonthLengths[l8] = new Integer(iArr8[l8]);
                        l8++;
                    }
                }
            }
            Integer[] newEndMonthLengths = new Integer[orgEndMonthLengths.length];
            for (int month4 = 0; month4 < 12; month4++) {
                if (month4 == i4) {
                    newEndMonthLengths[month4] = new Integer(orgEndMonthLengths[month4].intValue() + i5);
                } else {
                    newEndMonthLengths[month4] = new Integer(orgEndMonthLengths[month4].intValue());
                }
            }
            HashMap<Integer, Integer[]> hashMap = ADJUSTED_MONTH_LENGTHS;
            hashMap.put(new Integer(i3), newEndMonthLengths);
            Integer[] startMonthLengths = hashMap.get(new Integer(i));
            Integer[] endMonthLengths = hashMap.get(new Integer(i3));
            HashMap<Integer, Integer[]> hashMap2 = ADJUSTED_MONTH_DAYS;
            Integer[] endMonthDays = hashMap2.get(new Integer(i3));
            int startMonthLength = startMonthLengths[i2].intValue();
            int endMonthLength = endMonthLengths[i4].intValue();
            int startMonthDay = hashMap2.get(new Integer(i))[11].intValue() + startMonthLengths[11].intValue();
            Integer[] numArr8 = endMonthDays;
            int endMonthDay = endMonthDays[11].intValue() + endMonthLengths[11].intValue();
            Integer[] numArr9 = ADJUSTED_MAX_VALUES;
            int maxMonthLength = numArr9[5].intValue();
            Integer[] numArr10 = ADJUSTED_LEAST_MAX_VALUES;
            int leastMaxMonthLength = numArr10[5].intValue();
            if (maxMonthLength < startMonthLength) {
                maxMonthLength = startMonthLength;
            }
            if (maxMonthLength < endMonthLength) {
                maxMonthLength = endMonthLength;
            }
            boolean z2 = isEndYLeap;
            numArr9[5] = new Integer(maxMonthLength);
            if (leastMaxMonthLength > startMonthLength) {
                leastMaxMonthLength = startMonthLength;
            }
            if (leastMaxMonthLength > endMonthLength) {
                leastMaxMonthLength = endMonthLength;
            }
            numArr10[5] = new Integer(leastMaxMonthLength);
            int i6 = endMonthLength;
            int maxMonthDay = numArr9[6].intValue();
            int leastMaxMonthDay = numArr10[6].intValue();
            if (maxMonthDay < startMonthDay) {
                maxMonthDay = startMonthDay;
            }
            if (maxMonthDay < endMonthDay) {
                maxMonthDay = endMonthDay;
            }
            int i7 = maxMonthLength;
            numArr9[6] = new Integer(maxMonthDay);
            if (leastMaxMonthDay > startMonthDay) {
                leastMaxMonthDay = startMonthDay;
            }
            if (leastMaxMonthDay > endMonthDay) {
                leastMaxMonthDay = endMonthDay;
            }
            numArr10[6] = new Integer(leastMaxMonthDay);
        } else {
            throw new IllegalArgumentException("startYear == endYear && endMonth < startMonth");
        }
    }

    private static void readDeviationConfig() throws IOException, ParseException {
        InputStream is = getConfigFileInputStream();
        if (is != null) {
            BufferedReader br = null;
            try {
                BufferedReader br2 = new BufferedReader(new InputStreamReader(is));
                int num = 0;
                while (true) {
                    String readLine = br2.readLine();
                    String line = readLine;
                    if (readLine != null) {
                        num++;
                        parseLine(line.trim(), num);
                    } else {
                        br2.close();
                        return;
                    }
                }
            } catch (Throwable th) {
                if (br != null) {
                    br.close();
                }
                throw th;
            }
        }
    }

    private static void parseLine(String line, int num) throws ParseException {
        int endYear;
        int i = num;
        StringTokenizer st = new StringTokenizer(line, ";");
        int i2 = 0;
        int offset = 0;
        while (st.hasMoreTokens()) {
            String deviationElement = st.nextToken();
            int offsetIndex = deviationElement.indexOf(58);
            if (offsetIndex != -1) {
                try {
                    offset = Integer.parseInt(deviationElement.substring(offsetIndex + 1, deviationElement.length()));
                    int separatorIndex = deviationElement.indexOf(45);
                    if (separatorIndex != -1) {
                        String startDateStg = deviationElement.substring(i2, separatorIndex);
                        String endDateStg = deviationElement.substring(separatorIndex + 1, offsetIndex);
                        int startDateYearSepIndex = startDateStg.indexOf(47);
                        int endDateYearSepIndex = endDateStg.indexOf(47);
                        if (startDateYearSepIndex != -1) {
                            String startYearStg = startDateStg.substring(i2, startDateYearSepIndex);
                            String startMonthStg = startDateStg.substring(startDateYearSepIndex + 1, startDateStg.length());
                            try {
                                int startYear = Integer.parseInt(startYearStg);
                                try {
                                    int startMonth = Integer.parseInt(startMonthStg);
                                    if (endDateYearSepIndex != -1) {
                                        String endYearStg = endDateStg.substring(0, endDateYearSepIndex);
                                        StringTokenizer st2 = st;
                                        String endMonthStg = endDateStg.substring(endDateYearSepIndex + 1, endDateStg.length());
                                        try {
                                            int endYear2 = Integer.parseInt(endYearStg);
                                            try {
                                                int endMonth = Integer.parseInt(endMonthStg);
                                                if (startYear != -1) {
                                                    int startMonth2 = startMonth;
                                                    if (startMonth2 != -1) {
                                                        endYear = endYear2;
                                                        if (!(endYear == -1 || endMonth == -1)) {
                                                            addDeviationAsHijrah(startYear, startMonth2, endYear, endMonth, offset);
                                                            i2 = 0;
                                                            String str = line;
                                                            st = st2;
                                                        }
                                                    } else {
                                                        endYear = endYear2;
                                                    }
                                                } else {
                                                    endYear = endYear2;
                                                    int i3 = startMonth;
                                                }
                                                int i4 = endMonth;
                                                int i5 = endYear;
                                                throw new ParseException("Unknown error at line " + i + ".", i);
                                            } catch (NumberFormatException e) {
                                                int startMonth3 = startMonth;
                                                int startMonth4 = endYear2;
                                                NumberFormatException numberFormatException = e;
                                                String str2 = endMonthStg;
                                                int i6 = startMonth3;
                                                throw new ParseException("End month is not properly set at line " + i + ".", i);
                                            }
                                        } catch (NumberFormatException e2) {
                                            String str3 = endMonthStg;
                                            int i7 = startMonth;
                                            NumberFormatException numberFormatException2 = e2;
                                            throw new ParseException("End year is not properly set at line " + i + ".", i);
                                        }
                                    } else {
                                        throw new ParseException("End year/month has incorrect format at line " + i + ".", i);
                                    }
                                } catch (NumberFormatException e3) {
                                    StringTokenizer stringTokenizer = st;
                                    NumberFormatException numberFormatException3 = e3;
                                    throw new ParseException("Start month is not properly set at line " + i + ".", i);
                                }
                            } catch (NumberFormatException e4) {
                                StringTokenizer stringTokenizer2 = st;
                                NumberFormatException numberFormatException4 = e4;
                                throw new ParseException("Start year is not properly set at line " + i + ".", i);
                            }
                        } else {
                            throw new ParseException("Start year/month has incorrect format at line " + i + ".", i);
                        }
                    } else {
                        throw new ParseException("Start and end year/month has incorrect format at line " + i + ".", i);
                    }
                } catch (NumberFormatException e5) {
                    StringTokenizer stringTokenizer3 = st;
                    NumberFormatException numberFormatException5 = e5;
                    int i8 = offset;
                    throw new ParseException("Offset is not properly set at line " + i + ".", i);
                }
            } else {
                throw new ParseException("Offset has incorrect format at line " + i + ".", i);
            }
        }
    }

    private static InputStream getConfigFileInputStream() throws IOException {
        ZipFile zip;
        String fileName = System.getProperty("org.threeten.bp.i18n.HijrahDate.deviationConfigFile");
        if (fileName == null) {
            fileName = DEFAULT_CONFIG_FILENAME;
        }
        String dir = System.getProperty("org.threeten.bp.i18n.HijrahDate.deviationConfigDir");
        if (dir != null) {
            if (dir.length() != 0 || !dir.endsWith(System.getProperty("file.separator"))) {
                dir = dir + System.getProperty("file.separator");
            }
            File file = new File(dir + FILE_SEP + fileName);
            if (!file.exists()) {
                return null;
            }
            try {
                return new FileInputStream(file);
            } catch (IOException ioe) {
                throw ioe;
            }
        } else {
            StringTokenizer st = new StringTokenizer(System.getProperty("java.class.path"), PATH_SEP);
            while (st.hasMoreTokens()) {
                String path = st.nextToken();
                File file2 = new File(path);
                if (file2.exists()) {
                    if (file2.isDirectory()) {
                        StringBuilder append = new StringBuilder().append(path);
                        char c = FILE_SEP;
                        StringBuilder append2 = append.append(c);
                        String str = DEFAULT_CONFIG_PATH;
                        if (new File(append2.append(str).toString(), fileName).exists()) {
                            try {
                                return new FileInputStream(path + c + str + c + fileName);
                            } catch (IOException ioe2) {
                                throw ioe2;
                            }
                        }
                    } else {
                        try {
                            zip = new ZipFile(file2);
                        } catch (IOException e) {
                            zip = null;
                        }
                        if (zip != null) {
                            StringBuilder append3 = new StringBuilder().append(DEFAULT_CONFIG_PATH);
                            char c2 = FILE_SEP;
                            String targetFile = append3.append(c2).append(fileName).toString();
                            ZipEntry entry = zip.getEntry(targetFile);
                            if (entry == null) {
                                if (c2 == '/') {
                                    targetFile = targetFile.replace('/', '\\');
                                } else if (c2 == '\\') {
                                    targetFile = targetFile.replace('\\', '/');
                                }
                                entry = zip.getEntry(targetFile);
                            }
                            if (entry != null) {
                                try {
                                    return zip.getInputStream(entry);
                                } catch (IOException ioe3) {
                                    throw ioe3;
                                }
                            }
                        } else {
                            continue;
                        }
                    }
                }
            }
            return null;
        }
    }

    private Object writeReplace() {
        return new Ser((byte) 3, this);
    }

    /* access modifiers changed from: package-private */
    public void writeExternal(DataOutput out) throws IOException {
        out.writeInt(get(ChronoField.YEAR));
        out.writeByte(get(ChronoField.MONTH_OF_YEAR));
        out.writeByte(get(ChronoField.DAY_OF_MONTH));
    }

    static ChronoLocalDate readExternal(DataInput in) throws IOException {
        return HijrahChronology.INSTANCE.date(in.readInt(), in.readByte(), in.readByte());
    }
}
