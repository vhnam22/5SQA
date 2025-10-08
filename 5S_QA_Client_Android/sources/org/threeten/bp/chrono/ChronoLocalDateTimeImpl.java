package org.threeten.bp.chrono;

import java.io.IOException;
import java.io.ObjectInput;
import java.io.ObjectOutput;
import java.io.Serializable;
import org.threeten.bp.LocalTime;
import org.threeten.bp.ZoneId;
import org.threeten.bp.ZoneOffset;
import org.threeten.bp.chrono.ChronoLocalDate;
import org.threeten.bp.jdk8.Jdk8Methods;
import org.threeten.bp.temporal.ChronoField;
import org.threeten.bp.temporal.ChronoUnit;
import org.threeten.bp.temporal.Temporal;
import org.threeten.bp.temporal.TemporalAdjuster;
import org.threeten.bp.temporal.TemporalField;
import org.threeten.bp.temporal.TemporalUnit;
import org.threeten.bp.temporal.ValueRange;

final class ChronoLocalDateTimeImpl<D extends ChronoLocalDate> extends ChronoLocalDateTime<D> implements Temporal, TemporalAdjuster, Serializable {
    private static final int HOURS_PER_DAY = 24;
    private static final long MICROS_PER_DAY = 86400000000L;
    private static final long MILLIS_PER_DAY = 86400000;
    private static final int MINUTES_PER_DAY = 1440;
    private static final int MINUTES_PER_HOUR = 60;
    private static final long NANOS_PER_DAY = 86400000000000L;
    private static final long NANOS_PER_HOUR = 3600000000000L;
    private static final long NANOS_PER_MINUTE = 60000000000L;
    private static final long NANOS_PER_SECOND = 1000000000;
    private static final int SECONDS_PER_DAY = 86400;
    private static final int SECONDS_PER_HOUR = 3600;
    private static final int SECONDS_PER_MINUTE = 60;
    private static final long serialVersionUID = 4556003607393004514L;
    private final D date;
    private final LocalTime time;

    static <R extends ChronoLocalDate> ChronoLocalDateTimeImpl<R> of(R date2, LocalTime time2) {
        return new ChronoLocalDateTimeImpl<>(date2, time2);
    }

    private ChronoLocalDateTimeImpl(D date2, LocalTime time2) {
        Jdk8Methods.requireNonNull(date2, "date");
        Jdk8Methods.requireNonNull(time2, "time");
        this.date = date2;
        this.time = time2;
    }

    private ChronoLocalDateTimeImpl<D> with(Temporal newDate, LocalTime newTime) {
        D d = this.date;
        if (d == newDate && this.time == newTime) {
            return this;
        }
        return new ChronoLocalDateTimeImpl<>(d.getChronology().ensureChronoLocalDate(newDate), newTime);
    }

    public D toLocalDate() {
        return this.date;
    }

    public LocalTime toLocalTime() {
        return this.time;
    }

    public boolean isSupported(TemporalField field) {
        if (field instanceof ChronoField) {
            if (field.isDateBased() || field.isTimeBased()) {
                return true;
            }
            return false;
        } else if (field == null || !field.isSupportedBy(this)) {
            return false;
        } else {
            return true;
        }
    }

    public boolean isSupported(TemporalUnit unit) {
        if (unit instanceof ChronoUnit) {
            if (unit.isDateBased() || unit.isTimeBased()) {
                return true;
            }
            return false;
        } else if (unit == null || !unit.isSupportedBy(this)) {
            return false;
        } else {
            return true;
        }
    }

    public ValueRange range(TemporalField field) {
        if (field instanceof ChronoField) {
            return field.isTimeBased() ? this.time.range(field) : this.date.range(field);
        }
        return field.rangeRefinedBy(this);
    }

    public int get(TemporalField field) {
        if (field instanceof ChronoField) {
            return field.isTimeBased() ? this.time.get(field) : this.date.get(field);
        }
        return range(field).checkValidIntValue(getLong(field), field);
    }

    public long getLong(TemporalField field) {
        if (field instanceof ChronoField) {
            return field.isTimeBased() ? this.time.getLong(field) : this.date.getLong(field);
        }
        return field.getFrom(this);
    }

    public ChronoLocalDateTimeImpl<D> with(TemporalAdjuster adjuster) {
        if (adjuster instanceof ChronoLocalDate) {
            return with((Temporal) (ChronoLocalDate) adjuster, this.time);
        }
        if (adjuster instanceof LocalTime) {
            return with((Temporal) this.date, (LocalTime) adjuster);
        }
        if (adjuster instanceof ChronoLocalDateTimeImpl) {
            return this.date.getChronology().ensureChronoLocalDateTime((ChronoLocalDateTimeImpl) adjuster);
        }
        return this.date.getChronology().ensureChronoLocalDateTime((ChronoLocalDateTimeImpl) adjuster.adjustInto(this));
    }

    public ChronoLocalDateTimeImpl<D> with(TemporalField field, long newValue) {
        if (!(field instanceof ChronoField)) {
            return this.date.getChronology().ensureChronoLocalDateTime(field.adjustInto(this, newValue));
        }
        if (field.isTimeBased()) {
            return with((Temporal) this.date, this.time.with(field, newValue));
        }
        return with((Temporal) this.date.with(field, newValue), this.time);
    }

    public ChronoLocalDateTimeImpl<D> plus(long amountToAdd, TemporalUnit unit) {
        if (!(unit instanceof ChronoUnit)) {
            return this.date.getChronology().ensureChronoLocalDateTime(unit.addTo(this, amountToAdd));
        }
        switch (AnonymousClass1.$SwitchMap$org$threeten$bp$temporal$ChronoUnit[((ChronoUnit) unit).ordinal()]) {
            case 1:
                return plusNanos(amountToAdd);
            case 2:
                return plusDays(amountToAdd / MICROS_PER_DAY).plusNanos((amountToAdd % MICROS_PER_DAY) * 1000);
            case 3:
                return plusDays(amountToAdd / MILLIS_PER_DAY).plusNanos((amountToAdd % MILLIS_PER_DAY) * 1000000);
            case 4:
                return plusSeconds(amountToAdd);
            case 5:
                return plusMinutes(amountToAdd);
            case 6:
                return plusHours(amountToAdd);
            case 7:
                return plusDays(amountToAdd / 256).plusHours((amountToAdd % 256) * 12);
            default:
                return with((Temporal) this.date.plus(amountToAdd, unit), this.time);
        }
    }

    /* renamed from: org.threeten.bp.chrono.ChronoLocalDateTimeImpl$1  reason: invalid class name */
    static /* synthetic */ class AnonymousClass1 {
        static final /* synthetic */ int[] $SwitchMap$org$threeten$bp$temporal$ChronoUnit;

        static {
            int[] iArr = new int[ChronoUnit.values().length];
            $SwitchMap$org$threeten$bp$temporal$ChronoUnit = iArr;
            try {
                iArr[ChronoUnit.NANOS.ordinal()] = 1;
            } catch (NoSuchFieldError e) {
            }
            try {
                $SwitchMap$org$threeten$bp$temporal$ChronoUnit[ChronoUnit.MICROS.ordinal()] = 2;
            } catch (NoSuchFieldError e2) {
            }
            try {
                $SwitchMap$org$threeten$bp$temporal$ChronoUnit[ChronoUnit.MILLIS.ordinal()] = 3;
            } catch (NoSuchFieldError e3) {
            }
            try {
                $SwitchMap$org$threeten$bp$temporal$ChronoUnit[ChronoUnit.SECONDS.ordinal()] = 4;
            } catch (NoSuchFieldError e4) {
            }
            try {
                $SwitchMap$org$threeten$bp$temporal$ChronoUnit[ChronoUnit.MINUTES.ordinal()] = 5;
            } catch (NoSuchFieldError e5) {
            }
            try {
                $SwitchMap$org$threeten$bp$temporal$ChronoUnit[ChronoUnit.HOURS.ordinal()] = 6;
            } catch (NoSuchFieldError e6) {
            }
            try {
                $SwitchMap$org$threeten$bp$temporal$ChronoUnit[ChronoUnit.HALF_DAYS.ordinal()] = 7;
            } catch (NoSuchFieldError e7) {
            }
        }
    }

    private ChronoLocalDateTimeImpl<D> plusDays(long days) {
        return with((Temporal) this.date.plus(days, (TemporalUnit) ChronoUnit.DAYS), this.time);
    }

    private ChronoLocalDateTimeImpl<D> plusHours(long hours) {
        return plusWithOverflow(this.date, hours, 0, 0, 0);
    }

    private ChronoLocalDateTimeImpl<D> plusMinutes(long minutes) {
        return plusWithOverflow(this.date, 0, minutes, 0, 0);
    }

    /* access modifiers changed from: package-private */
    public ChronoLocalDateTimeImpl<D> plusSeconds(long seconds) {
        return plusWithOverflow(this.date, 0, 0, seconds, 0);
    }

    private ChronoLocalDateTimeImpl<D> plusNanos(long nanos) {
        return plusWithOverflow(this.date, 0, 0, 0, nanos);
    }

    private ChronoLocalDateTimeImpl<D> plusWithOverflow(D newDate, long hours, long minutes, long seconds, long nanos) {
        D d = newDate;
        if ((hours | minutes | seconds | nanos) == 0) {
            return with((Temporal) d, this.time);
        }
        long totDays = (nanos / NANOS_PER_DAY) + (seconds / 86400) + (minutes / 1440) + (hours / 24);
        long totNanos = (nanos % NANOS_PER_DAY) + ((seconds % 86400) * NANOS_PER_SECOND) + ((minutes % 1440) * NANOS_PER_MINUTE) + ((hours % 24) * NANOS_PER_HOUR);
        long curNoD = this.time.toNanoOfDay();
        long totNanos2 = totNanos + curNoD;
        long totDays2 = totDays + Jdk8Methods.floorDiv(totNanos2, (long) NANOS_PER_DAY);
        long newNoD = Jdk8Methods.floorMod(totNanos2, (long) NANOS_PER_DAY);
        return with((Temporal) d.plus(totDays2, (TemporalUnit) ChronoUnit.DAYS), newNoD == curNoD ? this.time : LocalTime.ofNanoOfDay(newNoD));
    }

    public ChronoZonedDateTime<D> atZone(ZoneId zoneId) {
        return ChronoZonedDateTimeImpl.ofBest(this, zoneId, (ZoneOffset) null);
    }

    public long until(Temporal endExclusive, TemporalUnit unit) {
        ChronoLocalDateTime localDateTime = toLocalDate().getChronology().localDateTime(endExclusive);
        if (!(unit instanceof ChronoUnit)) {
            return unit.between(this, localDateTime);
        }
        ChronoUnit f = (ChronoUnit) unit;
        if (f.isTimeBased()) {
            long amount = localDateTime.getLong(ChronoField.EPOCH_DAY) - this.date.getLong(ChronoField.EPOCH_DAY);
            switch (AnonymousClass1.$SwitchMap$org$threeten$bp$temporal$ChronoUnit[f.ordinal()]) {
                case 1:
                    amount = Jdk8Methods.safeMultiply(amount, (long) NANOS_PER_DAY);
                    break;
                case 2:
                    amount = Jdk8Methods.safeMultiply(amount, (long) MICROS_PER_DAY);
                    break;
                case 3:
                    amount = Jdk8Methods.safeMultiply(amount, (long) MILLIS_PER_DAY);
                    break;
                case 4:
                    amount = Jdk8Methods.safeMultiply(amount, (int) SECONDS_PER_DAY);
                    break;
                case 5:
                    amount = Jdk8Methods.safeMultiply(amount, (int) MINUTES_PER_DAY);
                    break;
                case 6:
                    amount = Jdk8Methods.safeMultiply(amount, 24);
                    break;
                case 7:
                    amount = Jdk8Methods.safeMultiply(amount, 2);
                    break;
            }
            return Jdk8Methods.safeAdd(amount, this.time.until(localDateTime.toLocalTime(), unit));
        }
        ChronoLocalDate endDate = localDateTime.toLocalDate();
        if (localDateTime.toLocalTime().isBefore(this.time)) {
            endDate = endDate.minus(1, (TemporalUnit) ChronoUnit.DAYS);
        }
        return this.date.until(endDate, unit);
    }

    private Object writeReplace() {
        return new Ser((byte) 12, this);
    }

    /* access modifiers changed from: package-private */
    public void writeExternal(ObjectOutput out) throws IOException {
        out.writeObject(this.date);
        out.writeObject(this.time);
    }

    static ChronoLocalDateTime<?> readExternal(ObjectInput in) throws IOException, ClassNotFoundException {
        return ((ChronoLocalDate) in.readObject()).atTime((LocalTime) in.readObject());
    }
}
