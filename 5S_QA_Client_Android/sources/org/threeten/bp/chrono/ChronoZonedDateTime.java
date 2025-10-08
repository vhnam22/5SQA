package org.threeten.bp.chrono;

import java.util.Comparator;
import org.threeten.bp.DateTimeException;
import org.threeten.bp.Instant;
import org.threeten.bp.LocalDate;
import org.threeten.bp.LocalTime;
import org.threeten.bp.ZoneId;
import org.threeten.bp.ZoneOffset;
import org.threeten.bp.chrono.ChronoLocalDate;
import org.threeten.bp.format.DateTimeFormatter;
import org.threeten.bp.jdk8.DefaultInterfaceTemporal;
import org.threeten.bp.jdk8.Jdk8Methods;
import org.threeten.bp.temporal.ChronoField;
import org.threeten.bp.temporal.ChronoUnit;
import org.threeten.bp.temporal.Temporal;
import org.threeten.bp.temporal.TemporalAccessor;
import org.threeten.bp.temporal.TemporalAdjuster;
import org.threeten.bp.temporal.TemporalAmount;
import org.threeten.bp.temporal.TemporalField;
import org.threeten.bp.temporal.TemporalQueries;
import org.threeten.bp.temporal.TemporalQuery;
import org.threeten.bp.temporal.TemporalUnit;
import org.threeten.bp.temporal.UnsupportedTemporalTypeException;
import org.threeten.bp.temporal.ValueRange;

public abstract class ChronoZonedDateTime<D extends ChronoLocalDate> extends DefaultInterfaceTemporal implements Temporal, Comparable<ChronoZonedDateTime<?>> {
    private static Comparator<ChronoZonedDateTime<?>> INSTANT_COMPARATOR = new Comparator<ChronoZonedDateTime<?>>() {
        public int compare(ChronoZonedDateTime<?> datetime1, ChronoZonedDateTime<?> datetime2) {
            int cmp = Jdk8Methods.compareLongs(datetime1.toEpochSecond(), datetime2.toEpochSecond());
            if (cmp == 0) {
                return Jdk8Methods.compareLongs(datetime1.toLocalTime().toNanoOfDay(), datetime2.toLocalTime().toNanoOfDay());
            }
            return cmp;
        }
    };

    public abstract ZoneOffset getOffset();

    public abstract ZoneId getZone();

    public abstract ChronoZonedDateTime<D> plus(long j, TemporalUnit temporalUnit);

    public abstract ChronoLocalDateTime<D> toLocalDateTime();

    public abstract ChronoZonedDateTime<D> with(TemporalField temporalField, long j);

    public abstract ChronoZonedDateTime<D> withEarlierOffsetAtOverlap();

    public abstract ChronoZonedDateTime<D> withLaterOffsetAtOverlap();

    public abstract ChronoZonedDateTime<D> withZoneSameInstant(ZoneId zoneId);

    public abstract ChronoZonedDateTime<D> withZoneSameLocal(ZoneId zoneId);

    public static Comparator<ChronoZonedDateTime<?>> timeLineOrder() {
        return INSTANT_COMPARATOR;
    }

    public static ChronoZonedDateTime<?> from(TemporalAccessor temporal) {
        Jdk8Methods.requireNonNull(temporal, "temporal");
        if (temporal instanceof ChronoZonedDateTime) {
            return (ChronoZonedDateTime) temporal;
        }
        Chronology chrono = (Chronology) temporal.query(TemporalQueries.chronology());
        if (chrono != null) {
            return chrono.zonedDateTime(temporal);
        }
        throw new DateTimeException("No Chronology found to create ChronoZonedDateTime: " + temporal.getClass());
    }

    public ValueRange range(TemporalField field) {
        if (!(field instanceof ChronoField)) {
            return field.rangeRefinedBy(this);
        }
        if (field == ChronoField.INSTANT_SECONDS || field == ChronoField.OFFSET_SECONDS) {
            return field.range();
        }
        return toLocalDateTime().range(field);
    }

    /* renamed from: org.threeten.bp.chrono.ChronoZonedDateTime$2  reason: invalid class name */
    static /* synthetic */ class AnonymousClass2 {
        static final /* synthetic */ int[] $SwitchMap$org$threeten$bp$temporal$ChronoField;

        static {
            int[] iArr = new int[ChronoField.values().length];
            $SwitchMap$org$threeten$bp$temporal$ChronoField = iArr;
            try {
                iArr[ChronoField.INSTANT_SECONDS.ordinal()] = 1;
            } catch (NoSuchFieldError e) {
            }
            try {
                $SwitchMap$org$threeten$bp$temporal$ChronoField[ChronoField.OFFSET_SECONDS.ordinal()] = 2;
            } catch (NoSuchFieldError e2) {
            }
        }
    }

    public int get(TemporalField field) {
        if (!(field instanceof ChronoField)) {
            return super.get(field);
        }
        switch (AnonymousClass2.$SwitchMap$org$threeten$bp$temporal$ChronoField[((ChronoField) field).ordinal()]) {
            case 1:
                throw new UnsupportedTemporalTypeException("Field too large for an int: " + field);
            case 2:
                return getOffset().getTotalSeconds();
            default:
                return toLocalDateTime().get(field);
        }
    }

    public long getLong(TemporalField field) {
        if (!(field instanceof ChronoField)) {
            return field.getFrom(this);
        }
        switch (AnonymousClass2.$SwitchMap$org$threeten$bp$temporal$ChronoField[((ChronoField) field).ordinal()]) {
            case 1:
                return toEpochSecond();
            case 2:
                return (long) getOffset().getTotalSeconds();
            default:
                return toLocalDateTime().getLong(field);
        }
    }

    public D toLocalDate() {
        return toLocalDateTime().toLocalDate();
    }

    public LocalTime toLocalTime() {
        return toLocalDateTime().toLocalTime();
    }

    public Chronology getChronology() {
        return toLocalDate().getChronology();
    }

    public ChronoZonedDateTime<D> with(TemporalAdjuster adjuster) {
        return toLocalDate().getChronology().ensureChronoZonedDateTime(super.with(adjuster));
    }

    public ChronoZonedDateTime<D> plus(TemporalAmount amount) {
        return toLocalDate().getChronology().ensureChronoZonedDateTime(super.plus(amount));
    }

    public ChronoZonedDateTime<D> minus(TemporalAmount amount) {
        return toLocalDate().getChronology().ensureChronoZonedDateTime(super.minus(amount));
    }

    public ChronoZonedDateTime<D> minus(long amountToSubtract, TemporalUnit unit) {
        return toLocalDate().getChronology().ensureChronoZonedDateTime(super.minus(amountToSubtract, unit));
    }

    public <R> R query(TemporalQuery<R> query) {
        if (query == TemporalQueries.zoneId() || query == TemporalQueries.zone()) {
            return getZone();
        }
        if (query == TemporalQueries.chronology()) {
            return toLocalDate().getChronology();
        }
        if (query == TemporalQueries.precision()) {
            return ChronoUnit.NANOS;
        }
        if (query == TemporalQueries.offset()) {
            return getOffset();
        }
        if (query == TemporalQueries.localDate()) {
            return LocalDate.ofEpochDay(toLocalDate().toEpochDay());
        }
        if (query == TemporalQueries.localTime()) {
            return toLocalTime();
        }
        return super.query(query);
    }

    public String format(DateTimeFormatter formatter) {
        Jdk8Methods.requireNonNull(formatter, "formatter");
        return formatter.format(this);
    }

    public Instant toInstant() {
        return Instant.ofEpochSecond(toEpochSecond(), (long) toLocalTime().getNano());
    }

    public long toEpochSecond() {
        return ((86400 * toLocalDate().toEpochDay()) + ((long) toLocalTime().toSecondOfDay())) - ((long) getOffset().getTotalSeconds());
    }

    /* JADX WARNING: type inference failed for: r5v0, types: [org.threeten.bp.chrono.ChronoZonedDateTime, org.threeten.bp.chrono.ChronoZonedDateTime<?>] */
    /* JADX WARNING: Unknown variable types count: 1 */
    /* Code decompiled incorrectly, please refer to instructions dump. */
    public int compareTo(org.threeten.bp.chrono.ChronoZonedDateTime<?> r5) {
        /*
            r4 = this;
            long r0 = r4.toEpochSecond()
            long r2 = r5.toEpochSecond()
            int r0 = org.threeten.bp.jdk8.Jdk8Methods.compareLongs(r0, r2)
            if (r0 != 0) goto L_0x005a
            org.threeten.bp.LocalTime r1 = r4.toLocalTime()
            int r1 = r1.getNano()
            org.threeten.bp.LocalTime r2 = r5.toLocalTime()
            int r2 = r2.getNano()
            int r0 = r1 - r2
            if (r0 != 0) goto L_0x005a
            org.threeten.bp.chrono.ChronoLocalDateTime r1 = r4.toLocalDateTime()
            org.threeten.bp.chrono.ChronoLocalDateTime r2 = r5.toLocalDateTime()
            int r0 = r1.compareTo((org.threeten.bp.chrono.ChronoLocalDateTime<?>) r2)
            if (r0 != 0) goto L_0x005a
            org.threeten.bp.ZoneId r1 = r4.getZone()
            java.lang.String r1 = r1.getId()
            org.threeten.bp.ZoneId r2 = r5.getZone()
            java.lang.String r2 = r2.getId()
            int r0 = r1.compareTo(r2)
            if (r0 != 0) goto L_0x005a
            org.threeten.bp.chrono.ChronoLocalDate r1 = r4.toLocalDate()
            org.threeten.bp.chrono.Chronology r1 = r1.getChronology()
            org.threeten.bp.chrono.ChronoLocalDate r2 = r5.toLocalDate()
            org.threeten.bp.chrono.Chronology r2 = r2.getChronology()
            int r0 = r1.compareTo((org.threeten.bp.chrono.Chronology) r2)
        L_0x005a:
            return r0
        */
        throw new UnsupportedOperationException("Method not decompiled: org.threeten.bp.chrono.ChronoZonedDateTime.compareTo(org.threeten.bp.chrono.ChronoZonedDateTime):int");
    }

    public boolean isAfter(ChronoZonedDateTime<?> other) {
        long thisEpochSec = toEpochSecond();
        long otherEpochSec = other.toEpochSecond();
        return thisEpochSec > otherEpochSec || (thisEpochSec == otherEpochSec && toLocalTime().getNano() > other.toLocalTime().getNano());
    }

    public boolean isBefore(ChronoZonedDateTime<?> other) {
        long thisEpochSec = toEpochSecond();
        long otherEpochSec = other.toEpochSecond();
        return thisEpochSec < otherEpochSec || (thisEpochSec == otherEpochSec && toLocalTime().getNano() < other.toLocalTime().getNano());
    }

    public boolean isEqual(ChronoZonedDateTime<?> other) {
        return toEpochSecond() == other.toEpochSecond() && toLocalTime().getNano() == other.toLocalTime().getNano();
    }

    public boolean equals(Object obj) {
        if (this == obj) {
            return true;
        }
        if (!(obj instanceof ChronoZonedDateTime) || compareTo((ChronoZonedDateTime<?>) (ChronoZonedDateTime) obj) != 0) {
            return false;
        }
        return true;
    }

    public int hashCode() {
        return (toLocalDateTime().hashCode() ^ getOffset().hashCode()) ^ Integer.rotateLeft(getZone().hashCode(), 3);
    }

    public String toString() {
        String str = toLocalDateTime().toString() + getOffset().toString();
        if (getOffset() != getZone()) {
            return str + '[' + getZone().toString() + ']';
        }
        return str;
    }
}
