package org.threeten.bp;

import java.io.Serializable;
import java.util.Arrays;
import java.util.Collections;
import java.util.List;
import java.util.regex.Matcher;
import java.util.regex.Pattern;
import org.threeten.bp.chrono.ChronoLocalDate;
import org.threeten.bp.chrono.ChronoPeriod;
import org.threeten.bp.chrono.Chronology;
import org.threeten.bp.chrono.IsoChronology;
import org.threeten.bp.format.DateTimeParseException;
import org.threeten.bp.jdk8.Jdk8Methods;
import org.threeten.bp.temporal.ChronoUnit;
import org.threeten.bp.temporal.Temporal;
import org.threeten.bp.temporal.TemporalAmount;
import org.threeten.bp.temporal.TemporalUnit;
import org.threeten.bp.temporal.UnsupportedTemporalTypeException;

public final class Period extends ChronoPeriod implements Serializable {
    private static final Pattern PATTERN = Pattern.compile("([-+]?)P(?:([-+]?[0-9]+)Y)?(?:([-+]?[0-9]+)M)?(?:([-+]?[0-9]+)W)?(?:([-+]?[0-9]+)D)?", 2);
    public static final Period ZERO = new Period(0, 0, 0);
    private static final long serialVersionUID = -8290556941213247973L;
    private final int days;
    private final int months;
    private final int years;

    public static Period ofYears(int years2) {
        return create(years2, 0, 0);
    }

    public static Period ofMonths(int months2) {
        return create(0, months2, 0);
    }

    public static Period ofWeeks(int weeks) {
        return create(0, 0, Jdk8Methods.safeMultiply(weeks, 7));
    }

    public static Period ofDays(int days2) {
        return create(0, 0, days2);
    }

    public static Period of(int years2, int months2, int days2) {
        return create(years2, months2, days2);
    }

    public static Period from(TemporalAmount amount) {
        if (amount instanceof Period) {
            return (Period) amount;
        }
        if (!(amount instanceof ChronoPeriod) || IsoChronology.INSTANCE.equals(((ChronoPeriod) amount).getChronology())) {
            Jdk8Methods.requireNonNull(amount, "amount");
            int years2 = 0;
            int months2 = 0;
            int days2 = 0;
            for (TemporalUnit unit : amount.getUnits()) {
                long unitAmount = amount.get(unit);
                if (unit == ChronoUnit.YEARS) {
                    years2 = Jdk8Methods.safeToInt(unitAmount);
                } else if (unit == ChronoUnit.MONTHS) {
                    months2 = Jdk8Methods.safeToInt(unitAmount);
                } else if (unit == ChronoUnit.DAYS) {
                    days2 = Jdk8Methods.safeToInt(unitAmount);
                } else {
                    throw new DateTimeException("Unit must be Years, Months or Days, but was " + unit);
                }
            }
            return create(years2, months2, days2);
        }
        throw new DateTimeException("Period requires ISO chronology: " + amount);
    }

    public static Period between(LocalDate startDate, LocalDate endDate) {
        return startDate.until((ChronoLocalDate) endDate);
    }

    public static Period parse(CharSequence text) {
        Jdk8Methods.requireNonNull(text, "text");
        Matcher matcher = PATTERN.matcher(text);
        if (matcher.matches()) {
            int negate = 1;
            if ("-".equals(matcher.group(1))) {
                negate = -1;
            }
            String yearMatch = matcher.group(2);
            String monthMatch = matcher.group(3);
            String weekMatch = matcher.group(4);
            String dayMatch = matcher.group(5);
            if (!(yearMatch == null && monthMatch == null && weekMatch == null && dayMatch == null)) {
                try {
                    return create(parseNumber(text, yearMatch, negate), parseNumber(text, monthMatch, negate), Jdk8Methods.safeAdd(parseNumber(text, dayMatch, negate), Jdk8Methods.safeMultiply(parseNumber(text, weekMatch, negate), 7)));
                } catch (NumberFormatException ex) {
                    throw ((DateTimeParseException) new DateTimeParseException("Text cannot be parsed to a Period", text, 0).initCause(ex));
                }
            }
        }
        throw new DateTimeParseException("Text cannot be parsed to a Period", text, 0);
    }

    private static int parseNumber(CharSequence text, String str, int negate) {
        if (str == null) {
            return 0;
        }
        try {
            return Jdk8Methods.safeMultiply(Integer.parseInt(str), negate);
        } catch (ArithmeticException ex) {
            throw ((DateTimeParseException) new DateTimeParseException("Text cannot be parsed to a Period", text, 0).initCause(ex));
        }
    }

    private static Period create(int years2, int months2, int days2) {
        if ((years2 | months2 | days2) == 0) {
            return ZERO;
        }
        return new Period(years2, months2, days2);
    }

    private Period(int years2, int months2, int days2) {
        this.years = years2;
        this.months = months2;
        this.days = days2;
    }

    private Object readResolve() {
        if ((this.years | this.months | this.days) == 0) {
            return ZERO;
        }
        return this;
    }

    public List<TemporalUnit> getUnits() {
        return Collections.unmodifiableList(Arrays.asList(new ChronoUnit[]{ChronoUnit.YEARS, ChronoUnit.MONTHS, ChronoUnit.DAYS}));
    }

    public Chronology getChronology() {
        return IsoChronology.INSTANCE;
    }

    public long get(TemporalUnit unit) {
        if (unit == ChronoUnit.YEARS) {
            return (long) this.years;
        }
        if (unit == ChronoUnit.MONTHS) {
            return (long) this.months;
        }
        if (unit == ChronoUnit.DAYS) {
            return (long) this.days;
        }
        throw new UnsupportedTemporalTypeException("Unsupported unit: " + unit);
    }

    public boolean isZero() {
        return this == ZERO;
    }

    public boolean isNegative() {
        return this.years < 0 || this.months < 0 || this.days < 0;
    }

    public int getYears() {
        return this.years;
    }

    public int getMonths() {
        return this.months;
    }

    public int getDays() {
        return this.days;
    }

    public Period withYears(int years2) {
        if (years2 == this.years) {
            return this;
        }
        return create(years2, this.months, this.days);
    }

    public Period withMonths(int months2) {
        if (months2 == this.months) {
            return this;
        }
        return create(this.years, months2, this.days);
    }

    public Period withDays(int days2) {
        if (days2 == this.days) {
            return this;
        }
        return create(this.years, this.months, days2);
    }

    public Period plus(TemporalAmount amountToAdd) {
        Period amount = from(amountToAdd);
        return create(Jdk8Methods.safeAdd(this.years, amount.years), Jdk8Methods.safeAdd(this.months, amount.months), Jdk8Methods.safeAdd(this.days, amount.days));
    }

    public Period plusYears(long yearsToAdd) {
        if (yearsToAdd == 0) {
            return this;
        }
        return create(Jdk8Methods.safeToInt(Jdk8Methods.safeAdd((long) this.years, yearsToAdd)), this.months, this.days);
    }

    public Period plusMonths(long monthsToAdd) {
        if (monthsToAdd == 0) {
            return this;
        }
        return create(this.years, Jdk8Methods.safeToInt(Jdk8Methods.safeAdd((long) this.months, monthsToAdd)), this.days);
    }

    public Period plusDays(long daysToAdd) {
        if (daysToAdd == 0) {
            return this;
        }
        return create(this.years, this.months, Jdk8Methods.safeToInt(Jdk8Methods.safeAdd((long) this.days, daysToAdd)));
    }

    public Period minus(TemporalAmount amountToSubtract) {
        Period amount = from(amountToSubtract);
        return create(Jdk8Methods.safeSubtract(this.years, amount.years), Jdk8Methods.safeSubtract(this.months, amount.months), Jdk8Methods.safeSubtract(this.days, amount.days));
    }

    public Period minusYears(long yearsToSubtract) {
        return yearsToSubtract == Long.MIN_VALUE ? plusYears(Long.MAX_VALUE).plusYears(1) : plusYears(-yearsToSubtract);
    }

    public Period minusMonths(long monthsToSubtract) {
        return monthsToSubtract == Long.MIN_VALUE ? plusMonths(Long.MAX_VALUE).plusMonths(1) : plusMonths(-monthsToSubtract);
    }

    public Period minusDays(long daysToSubtract) {
        return daysToSubtract == Long.MIN_VALUE ? plusDays(Long.MAX_VALUE).plusDays(1) : plusDays(-daysToSubtract);
    }

    public Period multipliedBy(int scalar) {
        if (this == ZERO || scalar == 1) {
            return this;
        }
        return create(Jdk8Methods.safeMultiply(this.years, scalar), Jdk8Methods.safeMultiply(this.months, scalar), Jdk8Methods.safeMultiply(this.days, scalar));
    }

    public Period negated() {
        return multipliedBy(-1);
    }

    public Period normalized() {
        long totalMonths = toTotalMonths();
        long splitYears = totalMonths / 12;
        int splitMonths = (int) (totalMonths % 12);
        if (splitYears == ((long) this.years) && splitMonths == this.months) {
            return this;
        }
        return create(Jdk8Methods.safeToInt(splitYears), splitMonths, this.days);
    }

    public long toTotalMonths() {
        return (((long) this.years) * 12) + ((long) this.months);
    }

    public Temporal addTo(Temporal temporal) {
        Jdk8Methods.requireNonNull(temporal, "temporal");
        int i = this.years;
        if (i == 0) {
            int i2 = this.months;
            if (i2 != 0) {
                temporal = temporal.plus((long) i2, ChronoUnit.MONTHS);
            }
        } else if (this.months != 0) {
            temporal = temporal.plus(toTotalMonths(), ChronoUnit.MONTHS);
        } else {
            temporal = temporal.plus((long) i, ChronoUnit.YEARS);
        }
        int i3 = this.days;
        if (i3 != 0) {
            return temporal.plus((long) i3, ChronoUnit.DAYS);
        }
        return temporal;
    }

    public Temporal subtractFrom(Temporal temporal) {
        Jdk8Methods.requireNonNull(temporal, "temporal");
        int i = this.years;
        if (i == 0) {
            int i2 = this.months;
            if (i2 != 0) {
                temporal = temporal.minus((long) i2, ChronoUnit.MONTHS);
            }
        } else if (this.months != 0) {
            temporal = temporal.minus(toTotalMonths(), ChronoUnit.MONTHS);
        } else {
            temporal = temporal.minus((long) i, ChronoUnit.YEARS);
        }
        int i3 = this.days;
        if (i3 != 0) {
            return temporal.minus((long) i3, ChronoUnit.DAYS);
        }
        return temporal;
    }

    public boolean equals(Object obj) {
        if (this == obj) {
            return true;
        }
        if (!(obj instanceof Period)) {
            return false;
        }
        Period other = (Period) obj;
        if (this.years == other.years && this.months == other.months && this.days == other.days) {
            return true;
        }
        return false;
    }

    public int hashCode() {
        return this.years + Integer.rotateLeft(this.months, 8) + Integer.rotateLeft(this.days, 16);
    }

    public String toString() {
        if (this == ZERO) {
            return "P0D";
        }
        StringBuilder buf = new StringBuilder();
        buf.append('P');
        int i = this.years;
        if (i != 0) {
            buf.append(i).append('Y');
        }
        int i2 = this.months;
        if (i2 != 0) {
            buf.append(i2).append('M');
        }
        int i3 = this.days;
        if (i3 != 0) {
            buf.append(i3).append('D');
        }
        return buf.toString();
    }
}
