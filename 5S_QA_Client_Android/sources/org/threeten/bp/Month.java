package org.threeten.bp;

import java.util.Locale;
import org.threeten.bp.chrono.Chronology;
import org.threeten.bp.chrono.IsoChronology;
import org.threeten.bp.format.DateTimeFormatterBuilder;
import org.threeten.bp.format.TextStyle;
import org.threeten.bp.temporal.ChronoField;
import org.threeten.bp.temporal.ChronoUnit;
import org.threeten.bp.temporal.Temporal;
import org.threeten.bp.temporal.TemporalAccessor;
import org.threeten.bp.temporal.TemporalAdjuster;
import org.threeten.bp.temporal.TemporalField;
import org.threeten.bp.temporal.TemporalQueries;
import org.threeten.bp.temporal.TemporalQuery;
import org.threeten.bp.temporal.UnsupportedTemporalTypeException;
import org.threeten.bp.temporal.ValueRange;

public enum Month implements TemporalAccessor, TemporalAdjuster {
    JANUARY,
    FEBRUARY,
    MARCH,
    APRIL,
    MAY,
    JUNE,
    JULY,
    AUGUST,
    SEPTEMBER,
    OCTOBER,
    NOVEMBER,
    DECEMBER;
    
    private static final Month[] ENUMS = null;
    public static final TemporalQuery<Month> FROM = null;

    static {
        FROM = new TemporalQuery<Month>() {
            public Month queryFrom(TemporalAccessor temporal) {
                return Month.from(temporal);
            }
        };
        ENUMS = values();
    }

    public static Month of(int month) {
        if (month >= 1 && month <= 12) {
            return ENUMS[month - 1];
        }
        throw new DateTimeException("Invalid value for MonthOfYear: " + month);
    }

    public static Month from(TemporalAccessor temporal) {
        if (temporal instanceof Month) {
            return (Month) temporal;
        }
        try {
            if (!IsoChronology.INSTANCE.equals(Chronology.from(temporal))) {
                temporal = LocalDate.from(temporal);
            }
            return of(temporal.get(ChronoField.MONTH_OF_YEAR));
        } catch (DateTimeException ex) {
            throw new DateTimeException("Unable to obtain Month from TemporalAccessor: " + temporal + ", type " + temporal.getClass().getName(), ex);
        }
    }

    public int getValue() {
        return ordinal() + 1;
    }

    public String getDisplayName(TextStyle style, Locale locale) {
        return new DateTimeFormatterBuilder().appendText((TemporalField) ChronoField.MONTH_OF_YEAR, style).toFormatter(locale).format(this);
    }

    public boolean isSupported(TemporalField field) {
        if (field instanceof ChronoField) {
            if (field == ChronoField.MONTH_OF_YEAR) {
                return true;
            }
            return false;
        } else if (field == null || !field.isSupportedBy(this)) {
            return false;
        } else {
            return true;
        }
    }

    public ValueRange range(TemporalField field) {
        if (field == ChronoField.MONTH_OF_YEAR) {
            return field.range();
        }
        if (!(field instanceof ChronoField)) {
            return field.rangeRefinedBy(this);
        }
        throw new UnsupportedTemporalTypeException("Unsupported field: " + field);
    }

    public int get(TemporalField field) {
        if (field == ChronoField.MONTH_OF_YEAR) {
            return getValue();
        }
        return range(field).checkValidIntValue(getLong(field), field);
    }

    public long getLong(TemporalField field) {
        if (field == ChronoField.MONTH_OF_YEAR) {
            return (long) getValue();
        }
        if (!(field instanceof ChronoField)) {
            return field.getFrom(this);
        }
        throw new UnsupportedTemporalTypeException("Unsupported field: " + field);
    }

    public Month plus(long months) {
        return ENUMS[(ordinal() + (((int) (months % 12)) + 12)) % 12];
    }

    public Month minus(long months) {
        return plus(-(months % 12));
    }

    /* renamed from: org.threeten.bp.Month$2  reason: invalid class name */
    static /* synthetic */ class AnonymousClass2 {
        static final /* synthetic */ int[] $SwitchMap$org$threeten$bp$Month = null;

        static {
            int[] iArr = new int[Month.values().length];
            $SwitchMap$org$threeten$bp$Month = iArr;
            try {
                iArr[Month.FEBRUARY.ordinal()] = 1;
            } catch (NoSuchFieldError e) {
            }
            try {
                $SwitchMap$org$threeten$bp$Month[Month.APRIL.ordinal()] = 2;
            } catch (NoSuchFieldError e2) {
            }
            try {
                $SwitchMap$org$threeten$bp$Month[Month.JUNE.ordinal()] = 3;
            } catch (NoSuchFieldError e3) {
            }
            try {
                $SwitchMap$org$threeten$bp$Month[Month.SEPTEMBER.ordinal()] = 4;
            } catch (NoSuchFieldError e4) {
            }
            try {
                $SwitchMap$org$threeten$bp$Month[Month.NOVEMBER.ordinal()] = 5;
            } catch (NoSuchFieldError e5) {
            }
            try {
                $SwitchMap$org$threeten$bp$Month[Month.JANUARY.ordinal()] = 6;
            } catch (NoSuchFieldError e6) {
            }
            try {
                $SwitchMap$org$threeten$bp$Month[Month.MARCH.ordinal()] = 7;
            } catch (NoSuchFieldError e7) {
            }
            try {
                $SwitchMap$org$threeten$bp$Month[Month.MAY.ordinal()] = 8;
            } catch (NoSuchFieldError e8) {
            }
            try {
                $SwitchMap$org$threeten$bp$Month[Month.JULY.ordinal()] = 9;
            } catch (NoSuchFieldError e9) {
            }
            try {
                $SwitchMap$org$threeten$bp$Month[Month.AUGUST.ordinal()] = 10;
            } catch (NoSuchFieldError e10) {
            }
            try {
                $SwitchMap$org$threeten$bp$Month[Month.OCTOBER.ordinal()] = 11;
            } catch (NoSuchFieldError e11) {
            }
            try {
                $SwitchMap$org$threeten$bp$Month[Month.DECEMBER.ordinal()] = 12;
            } catch (NoSuchFieldError e12) {
            }
        }
    }

    public int length(boolean leapYear) {
        switch (AnonymousClass2.$SwitchMap$org$threeten$bp$Month[ordinal()]) {
            case 1:
                return leapYear ? 29 : 28;
            case 2:
            case 3:
            case 4:
            case 5:
                return 30;
            default:
                return 31;
        }
    }

    public int minLength() {
        switch (AnonymousClass2.$SwitchMap$org$threeten$bp$Month[ordinal()]) {
            case 1:
                return 28;
            case 2:
            case 3:
            case 4:
            case 5:
                return 30;
            default:
                return 31;
        }
    }

    public int maxLength() {
        switch (AnonymousClass2.$SwitchMap$org$threeten$bp$Month[ordinal()]) {
            case 1:
                return 29;
            case 2:
            case 3:
            case 4:
            case 5:
                return 30;
            default:
                return 31;
        }
    }

    public int firstDayOfYear(boolean leapYear) {
        int leap = leapYear;
        switch (AnonymousClass2.$SwitchMap$org$threeten$bp$Month[ordinal()]) {
            case 1:
                return 32;
            case 2:
                return ((int) leap) + true;
            case 3:
                return leap + 152;
            case 4:
                return leap + 244;
            case 5:
                return leap + 305;
            case 6:
                return 1;
            case 7:
                return leap + 60;
            case 8:
                return leap + 121;
            case 9:
                return leap + 182;
            case 10:
                return leap + 213;
            case 11:
                return leap + 274;
            default:
                return leap + 335;
        }
    }

    public Month firstMonthOfQuarter() {
        return ENUMS[(ordinal() / 3) * 3];
    }

    public <R> R query(TemporalQuery<R> query) {
        if (query == TemporalQueries.chronology()) {
            return IsoChronology.INSTANCE;
        }
        if (query == TemporalQueries.precision()) {
            return ChronoUnit.MONTHS;
        }
        if (query == TemporalQueries.localDate() || query == TemporalQueries.localTime() || query == TemporalQueries.zone() || query == TemporalQueries.zoneId() || query == TemporalQueries.offset()) {
            return null;
        }
        return query.queryFrom(this);
    }

    public Temporal adjustInto(Temporal temporal) {
        if (Chronology.from(temporal).equals(IsoChronology.INSTANCE)) {
            return temporal.with(ChronoField.MONTH_OF_YEAR, (long) getValue());
        }
        throw new DateTimeException("Adjustment only supported on ISO date-time");
    }
}
