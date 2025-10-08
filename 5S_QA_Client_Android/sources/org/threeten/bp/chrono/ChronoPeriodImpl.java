package org.threeten.bp.chrono;

import java.io.Serializable;
import java.util.Arrays;
import java.util.Collections;
import java.util.List;
import org.threeten.bp.DateTimeException;
import org.threeten.bp.jdk8.Jdk8Methods;
import org.threeten.bp.temporal.ChronoField;
import org.threeten.bp.temporal.ChronoUnit;
import org.threeten.bp.temporal.Temporal;
import org.threeten.bp.temporal.TemporalAmount;
import org.threeten.bp.temporal.TemporalQueries;
import org.threeten.bp.temporal.TemporalUnit;
import org.threeten.bp.temporal.UnsupportedTemporalTypeException;

final class ChronoPeriodImpl extends ChronoPeriod implements Serializable {
    private static final long serialVersionUID = 275618735781L;
    private final Chronology chronology;
    private final int days;
    private final int months;
    private final int years;

    public ChronoPeriodImpl(Chronology chronology2, int years2, int months2, int days2) {
        this.chronology = chronology2;
        this.years = years2;
        this.months = months2;
        this.days = days2;
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

    public List<TemporalUnit> getUnits() {
        return Collections.unmodifiableList(Arrays.asList(new TemporalUnit[]{ChronoUnit.YEARS, ChronoUnit.MONTHS, ChronoUnit.DAYS}));
    }

    public Chronology getChronology() {
        return this.chronology;
    }

    public ChronoPeriod plus(TemporalAmount amountToAdd) {
        if (amountToAdd instanceof ChronoPeriodImpl) {
            ChronoPeriodImpl amount = (ChronoPeriodImpl) amountToAdd;
            if (amount.getChronology().equals(getChronology())) {
                return new ChronoPeriodImpl(this.chronology, Jdk8Methods.safeAdd(this.years, amount.years), Jdk8Methods.safeAdd(this.months, amount.months), Jdk8Methods.safeAdd(this.days, amount.days));
            }
        }
        throw new DateTimeException("Unable to add amount: " + amountToAdd);
    }

    public ChronoPeriod minus(TemporalAmount amountToSubtract) {
        if (amountToSubtract instanceof ChronoPeriodImpl) {
            ChronoPeriodImpl amount = (ChronoPeriodImpl) amountToSubtract;
            if (amount.getChronology().equals(getChronology())) {
                return new ChronoPeriodImpl(this.chronology, Jdk8Methods.safeSubtract(this.years, amount.years), Jdk8Methods.safeSubtract(this.months, amount.months), Jdk8Methods.safeSubtract(this.days, amount.days));
            }
        }
        throw new DateTimeException("Unable to subtract amount: " + amountToSubtract);
    }

    public ChronoPeriod multipliedBy(int scalar) {
        return new ChronoPeriodImpl(this.chronology, Jdk8Methods.safeMultiply(this.years, scalar), Jdk8Methods.safeMultiply(this.months, scalar), Jdk8Methods.safeMultiply(this.days, scalar));
    }

    public ChronoPeriod normalized() {
        if (!this.chronology.range(ChronoField.MONTH_OF_YEAR).isFixed()) {
            return this;
        }
        long monthLength = (this.chronology.range(ChronoField.MONTH_OF_YEAR).getMaximum() - this.chronology.range(ChronoField.MONTH_OF_YEAR).getMinimum()) + 1;
        long total = (((long) this.years) * monthLength) + ((long) this.months);
        return new ChronoPeriodImpl(this.chronology, Jdk8Methods.safeToInt(total / monthLength), Jdk8Methods.safeToInt(total % monthLength), this.days);
    }

    public Temporal addTo(Temporal temporal) {
        Jdk8Methods.requireNonNull(temporal, "temporal");
        Chronology temporalChrono = (Chronology) temporal.query(TemporalQueries.chronology());
        if (temporalChrono == null || this.chronology.equals(temporalChrono)) {
            int i = this.years;
            if (i != 0) {
                temporal = temporal.plus((long) i, ChronoUnit.YEARS);
            }
            int i2 = this.months;
            if (i2 != 0) {
                temporal = temporal.plus((long) i2, ChronoUnit.MONTHS);
            }
            int i3 = this.days;
            if (i3 != 0) {
                return temporal.plus((long) i3, ChronoUnit.DAYS);
            }
            return temporal;
        }
        throw new DateTimeException("Invalid chronology, required: " + this.chronology.getId() + ", but was: " + temporalChrono.getId());
    }

    public Temporal subtractFrom(Temporal temporal) {
        Jdk8Methods.requireNonNull(temporal, "temporal");
        Chronology temporalChrono = (Chronology) temporal.query(TemporalQueries.chronology());
        if (temporalChrono == null || this.chronology.equals(temporalChrono)) {
            int i = this.years;
            if (i != 0) {
                temporal = temporal.minus((long) i, ChronoUnit.YEARS);
            }
            int i2 = this.months;
            if (i2 != 0) {
                temporal = temporal.minus((long) i2, ChronoUnit.MONTHS);
            }
            int i3 = this.days;
            if (i3 != 0) {
                return temporal.minus((long) i3, ChronoUnit.DAYS);
            }
            return temporal;
        }
        throw new DateTimeException("Invalid chronology, required: " + this.chronology.getId() + ", but was: " + temporalChrono.getId());
    }

    public boolean equals(Object obj) {
        if (this == obj) {
            return true;
        }
        if (!(obj instanceof ChronoPeriodImpl)) {
            return false;
        }
        ChronoPeriodImpl other = (ChronoPeriodImpl) obj;
        if (this.years == other.years && this.months == other.months && this.days == other.days && this.chronology.equals(other.chronology)) {
            return true;
        }
        return false;
    }

    public int hashCode() {
        return this.chronology.hashCode() + Integer.rotateLeft(this.years, 16) + Integer.rotateLeft(this.months, 8) + this.days;
    }

    public String toString() {
        if (isZero()) {
            return this.chronology + " P0D";
        }
        StringBuilder buf = new StringBuilder();
        buf.append(this.chronology).append(' ').append('P');
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
