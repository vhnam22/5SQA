package org.threeten.bp.jdk8;

import org.threeten.bp.temporal.Temporal;
import org.threeten.bp.temporal.TemporalAdjuster;
import org.threeten.bp.temporal.TemporalAmount;
import org.threeten.bp.temporal.TemporalUnit;

public abstract class DefaultInterfaceTemporal extends DefaultInterfaceTemporalAccessor implements Temporal {
    public Temporal with(TemporalAdjuster adjuster) {
        return adjuster.adjustInto(this);
    }

    public Temporal plus(TemporalAmount amount) {
        return amount.addTo(this);
    }

    public Temporal minus(TemporalAmount amount) {
        return amount.subtractFrom(this);
    }

    public Temporal minus(long amountToSubtract, TemporalUnit unit) {
        return amountToSubtract == Long.MIN_VALUE ? plus(Long.MAX_VALUE, unit).plus(1, unit) : plus(-amountToSubtract, unit);
    }
}
