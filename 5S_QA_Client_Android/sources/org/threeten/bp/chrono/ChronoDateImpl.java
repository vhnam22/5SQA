package org.threeten.bp.chrono;

import java.io.Serializable;
import org.threeten.bp.DateTimeException;
import org.threeten.bp.LocalDate;
import org.threeten.bp.LocalTime;
import org.threeten.bp.chrono.ChronoLocalDate;
import org.threeten.bp.jdk8.Jdk8Methods;
import org.threeten.bp.temporal.ChronoUnit;
import org.threeten.bp.temporal.Temporal;
import org.threeten.bp.temporal.TemporalAdjuster;
import org.threeten.bp.temporal.TemporalUnit;

abstract class ChronoDateImpl<D extends ChronoLocalDate> extends ChronoLocalDate implements Temporal, TemporalAdjuster, Serializable {
    private static final long serialVersionUID = 6282433883239719096L;

    /* access modifiers changed from: package-private */
    public abstract ChronoDateImpl<D> plusDays(long j);

    /* access modifiers changed from: package-private */
    public abstract ChronoDateImpl<D> plusMonths(long j);

    /* access modifiers changed from: package-private */
    public abstract ChronoDateImpl<D> plusYears(long j);

    ChronoDateImpl() {
    }

    public ChronoDateImpl<D> plus(long amountToAdd, TemporalUnit unit) {
        if (!(unit instanceof ChronoUnit)) {
            return (ChronoDateImpl) getChronology().ensureChronoLocalDate(unit.addTo(this, amountToAdd));
        }
        switch (AnonymousClass1.$SwitchMap$org$threeten$bp$temporal$ChronoUnit[((ChronoUnit) unit).ordinal()]) {
            case 1:
                return plusDays(amountToAdd);
            case 2:
                return plusDays(Jdk8Methods.safeMultiply(amountToAdd, 7));
            case 3:
                return plusMonths(amountToAdd);
            case 4:
                return plusYears(amountToAdd);
            case 5:
                return plusYears(Jdk8Methods.safeMultiply(amountToAdd, 10));
            case 6:
                return plusYears(Jdk8Methods.safeMultiply(amountToAdd, 100));
            case 7:
                return plusYears(Jdk8Methods.safeMultiply(amountToAdd, 1000));
            default:
                throw new DateTimeException(unit + " not valid for chronology " + getChronology().getId());
        }
    }

    /* renamed from: org.threeten.bp.chrono.ChronoDateImpl$1  reason: invalid class name */
    static /* synthetic */ class AnonymousClass1 {
        static final /* synthetic */ int[] $SwitchMap$org$threeten$bp$temporal$ChronoUnit;

        static {
            int[] iArr = new int[ChronoUnit.values().length];
            $SwitchMap$org$threeten$bp$temporal$ChronoUnit = iArr;
            try {
                iArr[ChronoUnit.DAYS.ordinal()] = 1;
            } catch (NoSuchFieldError e) {
            }
            try {
                $SwitchMap$org$threeten$bp$temporal$ChronoUnit[ChronoUnit.WEEKS.ordinal()] = 2;
            } catch (NoSuchFieldError e2) {
            }
            try {
                $SwitchMap$org$threeten$bp$temporal$ChronoUnit[ChronoUnit.MONTHS.ordinal()] = 3;
            } catch (NoSuchFieldError e3) {
            }
            try {
                $SwitchMap$org$threeten$bp$temporal$ChronoUnit[ChronoUnit.YEARS.ordinal()] = 4;
            } catch (NoSuchFieldError e4) {
            }
            try {
                $SwitchMap$org$threeten$bp$temporal$ChronoUnit[ChronoUnit.DECADES.ordinal()] = 5;
            } catch (NoSuchFieldError e5) {
            }
            try {
                $SwitchMap$org$threeten$bp$temporal$ChronoUnit[ChronoUnit.CENTURIES.ordinal()] = 6;
            } catch (NoSuchFieldError e6) {
            }
            try {
                $SwitchMap$org$threeten$bp$temporal$ChronoUnit[ChronoUnit.MILLENNIA.ordinal()] = 7;
            } catch (NoSuchFieldError e7) {
            }
        }
    }

    /* access modifiers changed from: package-private */
    public ChronoDateImpl<D> plusWeeks(long weeksToAdd) {
        return plusDays(Jdk8Methods.safeMultiply(weeksToAdd, 7));
    }

    /* access modifiers changed from: package-private */
    public ChronoDateImpl<D> minusYears(long yearsToSubtract) {
        return yearsToSubtract == Long.MIN_VALUE ? plusYears(Long.MAX_VALUE).plusYears(1) : plusYears(-yearsToSubtract);
    }

    /* access modifiers changed from: package-private */
    public ChronoDateImpl<D> minusMonths(long monthsToSubtract) {
        return monthsToSubtract == Long.MIN_VALUE ? plusMonths(Long.MAX_VALUE).plusMonths(1) : plusMonths(-monthsToSubtract);
    }

    /* access modifiers changed from: package-private */
    public ChronoDateImpl<D> minusWeeks(long weeksToSubtract) {
        return weeksToSubtract == Long.MIN_VALUE ? plusWeeks(Long.MAX_VALUE).plusWeeks(1) : plusWeeks(-weeksToSubtract);
    }

    /* access modifiers changed from: package-private */
    public ChronoDateImpl<D> minusDays(long daysToSubtract) {
        return daysToSubtract == Long.MIN_VALUE ? plusDays(Long.MAX_VALUE).plusDays(1) : plusDays(-daysToSubtract);
    }

    public ChronoLocalDateTime<?> atTime(LocalTime localTime) {
        return ChronoLocalDateTimeImpl.of(this, localTime);
    }

    public long until(Temporal endExclusive, TemporalUnit unit) {
        ChronoLocalDate end = getChronology().date(endExclusive);
        if (unit instanceof ChronoUnit) {
            return LocalDate.from(this).until(end, unit);
        }
        return unit.between(this, end);
    }

    public ChronoPeriod until(ChronoLocalDate endDate) {
        throw new UnsupportedOperationException("Not supported in ThreeTen backport");
    }
}
