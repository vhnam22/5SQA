package org.threeten.bp.temporal;

import org.threeten.bp.DayOfWeek;
import org.threeten.bp.jdk8.Jdk8Methods;

public final class TemporalAdjusters {
    private TemporalAdjusters() {
    }

    public static TemporalAdjuster firstDayOfMonth() {
        return Impl.FIRST_DAY_OF_MONTH;
    }

    public static TemporalAdjuster lastDayOfMonth() {
        return Impl.LAST_DAY_OF_MONTH;
    }

    public static TemporalAdjuster firstDayOfNextMonth() {
        return Impl.FIRST_DAY_OF_NEXT_MONTH;
    }

    public static TemporalAdjuster firstDayOfYear() {
        return Impl.FIRST_DAY_OF_YEAR;
    }

    public static TemporalAdjuster lastDayOfYear() {
        return Impl.LAST_DAY_OF_YEAR;
    }

    public static TemporalAdjuster firstDayOfNextYear() {
        return Impl.FIRST_DAY_OF_NEXT_YEAR;
    }

    private static class Impl implements TemporalAdjuster {
        /* access modifiers changed from: private */
        public static final Impl FIRST_DAY_OF_MONTH = new Impl(0);
        /* access modifiers changed from: private */
        public static final Impl FIRST_DAY_OF_NEXT_MONTH = new Impl(2);
        /* access modifiers changed from: private */
        public static final Impl FIRST_DAY_OF_NEXT_YEAR = new Impl(5);
        /* access modifiers changed from: private */
        public static final Impl FIRST_DAY_OF_YEAR = new Impl(3);
        /* access modifiers changed from: private */
        public static final Impl LAST_DAY_OF_MONTH = new Impl(1);
        /* access modifiers changed from: private */
        public static final Impl LAST_DAY_OF_YEAR = new Impl(4);
        private final int ordinal;

        private Impl(int ordinal2) {
            this.ordinal = ordinal2;
        }

        public Temporal adjustInto(Temporal temporal) {
            switch (this.ordinal) {
                case 0:
                    return temporal.with(ChronoField.DAY_OF_MONTH, 1);
                case 1:
                    return temporal.with(ChronoField.DAY_OF_MONTH, temporal.range(ChronoField.DAY_OF_MONTH).getMaximum());
                case 2:
                    return temporal.with(ChronoField.DAY_OF_MONTH, 1).plus(1, ChronoUnit.MONTHS);
                case 3:
                    return temporal.with(ChronoField.DAY_OF_YEAR, 1);
                case 4:
                    return temporal.with(ChronoField.DAY_OF_YEAR, temporal.range(ChronoField.DAY_OF_YEAR).getMaximum());
                case 5:
                    return temporal.with(ChronoField.DAY_OF_YEAR, 1).plus(1, ChronoUnit.YEARS);
                default:
                    throw new IllegalStateException("Unreachable");
            }
        }
    }

    public static TemporalAdjuster firstInMonth(DayOfWeek dayOfWeek) {
        Jdk8Methods.requireNonNull(dayOfWeek, "dayOfWeek");
        return new DayOfWeekInMonth(1, dayOfWeek);
    }

    public static TemporalAdjuster lastInMonth(DayOfWeek dayOfWeek) {
        Jdk8Methods.requireNonNull(dayOfWeek, "dayOfWeek");
        return new DayOfWeekInMonth(-1, dayOfWeek);
    }

    public static TemporalAdjuster dayOfWeekInMonth(int ordinal, DayOfWeek dayOfWeek) {
        Jdk8Methods.requireNonNull(dayOfWeek, "dayOfWeek");
        return new DayOfWeekInMonth(ordinal, dayOfWeek);
    }

    private static final class DayOfWeekInMonth implements TemporalAdjuster {
        private final int dowValue;
        private final int ordinal;

        private DayOfWeekInMonth(int ordinal2, DayOfWeek dow) {
            this.ordinal = ordinal2;
            this.dowValue = dow.getValue();
        }

        public Temporal adjustInto(Temporal temporal) {
            if (this.ordinal >= 0) {
                Temporal temp = temporal.with(ChronoField.DAY_OF_MONTH, 1);
                return temp.plus((long) ((int) (((long) (((this.dowValue - temp.get(ChronoField.DAY_OF_WEEK)) + 7) % 7)) + ((((long) this.ordinal) - 1) * 7))), ChronoUnit.DAYS);
            }
            Temporal temp2 = temporal.with(ChronoField.DAY_OF_MONTH, temporal.range(ChronoField.DAY_OF_MONTH).getMaximum());
            int daysDiff = this.dowValue - temp2.get(ChronoField.DAY_OF_WEEK);
            return temp2.plus((long) ((int) (((long) (daysDiff == 0 ? 0 : daysDiff > 0 ? daysDiff - 7 : daysDiff)) - ((((long) (-this.ordinal)) - 1) * 7))), ChronoUnit.DAYS);
        }
    }

    public static TemporalAdjuster next(DayOfWeek dayOfWeek) {
        return new RelativeDayOfWeek(2, dayOfWeek);
    }

    public static TemporalAdjuster nextOrSame(DayOfWeek dayOfWeek) {
        return new RelativeDayOfWeek(0, dayOfWeek);
    }

    public static TemporalAdjuster previous(DayOfWeek dayOfWeek) {
        return new RelativeDayOfWeek(3, dayOfWeek);
    }

    public static TemporalAdjuster previousOrSame(DayOfWeek dayOfWeek) {
        return new RelativeDayOfWeek(1, dayOfWeek);
    }

    private static final class RelativeDayOfWeek implements TemporalAdjuster {
        private final int dowValue;
        private final int relative;

        private RelativeDayOfWeek(int relative2, DayOfWeek dayOfWeek) {
            Jdk8Methods.requireNonNull(dayOfWeek, "dayOfWeek");
            this.relative = relative2;
            this.dowValue = dayOfWeek.getValue();
        }

        public Temporal adjustInto(Temporal temporal) {
            int calDow = temporal.get(ChronoField.DAY_OF_WEEK);
            int i = this.relative;
            if (i < 2 && calDow == this.dowValue) {
                return temporal;
            }
            if ((i & 1) == 0) {
                int daysDiff = calDow - this.dowValue;
                return temporal.plus((long) (daysDiff >= 0 ? 7 - daysDiff : -daysDiff), ChronoUnit.DAYS);
            }
            int daysDiff2 = this.dowValue - calDow;
            return temporal.minus((long) (daysDiff2 >= 0 ? 7 - daysDiff2 : -daysDiff2), ChronoUnit.DAYS);
        }
    }
}
