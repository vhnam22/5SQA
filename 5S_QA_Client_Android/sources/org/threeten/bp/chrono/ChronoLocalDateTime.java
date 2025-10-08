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

public abstract class ChronoLocalDateTime<D extends ChronoLocalDate> extends DefaultInterfaceTemporal implements Temporal, TemporalAdjuster, Comparable<ChronoLocalDateTime<?>> {
    private static final Comparator<ChronoLocalDateTime<?>> DATE_TIME_COMPARATOR = new Comparator<ChronoLocalDateTime<?>>() {
        /* JADX WARNING: type inference failed for: r6v0, types: [org.threeten.bp.chrono.ChronoLocalDateTime, org.threeten.bp.chrono.ChronoLocalDateTime<?>] */
        /* JADX WARNING: type inference failed for: r7v0, types: [org.threeten.bp.chrono.ChronoLocalDateTime, org.threeten.bp.chrono.ChronoLocalDateTime<?>] */
        /* JADX WARNING: Unknown variable types count: 2 */
        /* Code decompiled incorrectly, please refer to instructions dump. */
        public int compare(org.threeten.bp.chrono.ChronoLocalDateTime<?> r6, org.threeten.bp.chrono.ChronoLocalDateTime<?> r7) {
            /*
                r5 = this;
                org.threeten.bp.chrono.ChronoLocalDate r0 = r6.toLocalDate()
                long r0 = r0.toEpochDay()
                org.threeten.bp.chrono.ChronoLocalDate r2 = r7.toLocalDate()
                long r2 = r2.toEpochDay()
                int r0 = org.threeten.bp.jdk8.Jdk8Methods.compareLongs(r0, r2)
                if (r0 != 0) goto L_0x002a
                org.threeten.bp.LocalTime r1 = r6.toLocalTime()
                long r1 = r1.toNanoOfDay()
                org.threeten.bp.LocalTime r3 = r7.toLocalTime()
                long r3 = r3.toNanoOfDay()
                int r0 = org.threeten.bp.jdk8.Jdk8Methods.compareLongs(r1, r3)
            L_0x002a:
                return r0
            */
            throw new UnsupportedOperationException("Method not decompiled: org.threeten.bp.chrono.ChronoLocalDateTime.AnonymousClass1.compare(org.threeten.bp.chrono.ChronoLocalDateTime, org.threeten.bp.chrono.ChronoLocalDateTime):int");
        }
    };

    public abstract ChronoZonedDateTime<D> atZone(ZoneId zoneId);

    public abstract ChronoLocalDateTime<D> plus(long j, TemporalUnit temporalUnit);

    public abstract D toLocalDate();

    public abstract LocalTime toLocalTime();

    public abstract ChronoLocalDateTime<D> with(TemporalField temporalField, long j);

    public static Comparator<ChronoLocalDateTime<?>> timeLineOrder() {
        return DATE_TIME_COMPARATOR;
    }

    public static ChronoLocalDateTime<?> from(TemporalAccessor temporal) {
        Jdk8Methods.requireNonNull(temporal, "temporal");
        if (temporal instanceof ChronoLocalDateTime) {
            return (ChronoLocalDateTime) temporal;
        }
        Chronology chrono = (Chronology) temporal.query(TemporalQueries.chronology());
        if (chrono != null) {
            return chrono.localDateTime(temporal);
        }
        throw new DateTimeException("No Chronology found to create ChronoLocalDateTime: " + temporal.getClass());
    }

    public Chronology getChronology() {
        return toLocalDate().getChronology();
    }

    public ChronoLocalDateTime<D> with(TemporalAdjuster adjuster) {
        return toLocalDate().getChronology().ensureChronoLocalDateTime(super.with(adjuster));
    }

    public ChronoLocalDateTime<D> plus(TemporalAmount amount) {
        return toLocalDate().getChronology().ensureChronoLocalDateTime(super.plus(amount));
    }

    public ChronoLocalDateTime<D> minus(TemporalAmount amount) {
        return toLocalDate().getChronology().ensureChronoLocalDateTime(super.minus(amount));
    }

    public ChronoLocalDateTime<D> minus(long amountToSubtract, TemporalUnit unit) {
        return toLocalDate().getChronology().ensureChronoLocalDateTime(super.minus(amountToSubtract, unit));
    }

    public <R> R query(TemporalQuery<R> query) {
        if (query == TemporalQueries.chronology()) {
            return getChronology();
        }
        if (query == TemporalQueries.precision()) {
            return ChronoUnit.NANOS;
        }
        if (query == TemporalQueries.localDate()) {
            return LocalDate.ofEpochDay(toLocalDate().toEpochDay());
        }
        if (query == TemporalQueries.localTime()) {
            return toLocalTime();
        }
        if (query == TemporalQueries.zone() || query == TemporalQueries.zoneId() || query == TemporalQueries.offset()) {
            return null;
        }
        return super.query(query);
    }

    public Temporal adjustInto(Temporal temporal) {
        return temporal.with(ChronoField.EPOCH_DAY, toLocalDate().toEpochDay()).with(ChronoField.NANO_OF_DAY, toLocalTime().toNanoOfDay());
    }

    public String format(DateTimeFormatter formatter) {
        Jdk8Methods.requireNonNull(formatter, "formatter");
        return formatter.format(this);
    }

    public Instant toInstant(ZoneOffset offset) {
        return Instant.ofEpochSecond(toEpochSecond(offset), (long) toLocalTime().getNano());
    }

    public long toEpochSecond(ZoneOffset offset) {
        Jdk8Methods.requireNonNull(offset, "offset");
        return ((86400 * toLocalDate().toEpochDay()) + ((long) toLocalTime().toSecondOfDay())) - ((long) offset.getTotalSeconds());
    }

    /* JADX WARNING: type inference failed for: r4v0, types: [org.threeten.bp.chrono.ChronoLocalDateTime, org.threeten.bp.chrono.ChronoLocalDateTime<?>] */
    /* JADX WARNING: Unknown variable types count: 1 */
    /* Code decompiled incorrectly, please refer to instructions dump. */
    public int compareTo(org.threeten.bp.chrono.ChronoLocalDateTime<?> r4) {
        /*
            r3 = this;
            org.threeten.bp.chrono.ChronoLocalDate r0 = r3.toLocalDate()
            org.threeten.bp.chrono.ChronoLocalDate r1 = r4.toLocalDate()
            int r0 = r0.compareTo((org.threeten.bp.chrono.ChronoLocalDate) r1)
            if (r0 != 0) goto L_0x0028
            org.threeten.bp.LocalTime r1 = r3.toLocalTime()
            org.threeten.bp.LocalTime r2 = r4.toLocalTime()
            int r0 = r1.compareTo((org.threeten.bp.LocalTime) r2)
            if (r0 != 0) goto L_0x0028
            org.threeten.bp.chrono.Chronology r1 = r3.getChronology()
            org.threeten.bp.chrono.Chronology r2 = r4.getChronology()
            int r0 = r1.compareTo((org.threeten.bp.chrono.Chronology) r2)
        L_0x0028:
            return r0
        */
        throw new UnsupportedOperationException("Method not decompiled: org.threeten.bp.chrono.ChronoLocalDateTime.compareTo(org.threeten.bp.chrono.ChronoLocalDateTime):int");
    }

    /* JADX WARNING: type inference failed for: r10v0, types: [org.threeten.bp.chrono.ChronoLocalDateTime, org.threeten.bp.chrono.ChronoLocalDateTime<?>] */
    /* JADX WARNING: Unknown variable types count: 1 */
    /* Code decompiled incorrectly, please refer to instructions dump. */
    public boolean isAfter(org.threeten.bp.chrono.ChronoLocalDateTime<?> r10) {
        /*
            r9 = this;
            org.threeten.bp.chrono.ChronoLocalDate r0 = r9.toLocalDate()
            long r0 = r0.toEpochDay()
            org.threeten.bp.chrono.ChronoLocalDate r2 = r10.toLocalDate()
            long r2 = r2.toEpochDay()
            int r4 = (r0 > r2 ? 1 : (r0 == r2 ? 0 : -1))
            if (r4 > 0) goto L_0x002f
            int r4 = (r0 > r2 ? 1 : (r0 == r2 ? 0 : -1))
            if (r4 != 0) goto L_0x002d
            org.threeten.bp.LocalTime r4 = r9.toLocalTime()
            long r4 = r4.toNanoOfDay()
            org.threeten.bp.LocalTime r6 = r10.toLocalTime()
            long r6 = r6.toNanoOfDay()
            int r8 = (r4 > r6 ? 1 : (r4 == r6 ? 0 : -1))
            if (r8 <= 0) goto L_0x002d
            goto L_0x002f
        L_0x002d:
            r4 = 0
            goto L_0x0030
        L_0x002f:
            r4 = 1
        L_0x0030:
            return r4
        */
        throw new UnsupportedOperationException("Method not decompiled: org.threeten.bp.chrono.ChronoLocalDateTime.isAfter(org.threeten.bp.chrono.ChronoLocalDateTime):boolean");
    }

    /* JADX WARNING: type inference failed for: r10v0, types: [org.threeten.bp.chrono.ChronoLocalDateTime, org.threeten.bp.chrono.ChronoLocalDateTime<?>] */
    /* JADX WARNING: Unknown variable types count: 1 */
    /* Code decompiled incorrectly, please refer to instructions dump. */
    public boolean isBefore(org.threeten.bp.chrono.ChronoLocalDateTime<?> r10) {
        /*
            r9 = this;
            org.threeten.bp.chrono.ChronoLocalDate r0 = r9.toLocalDate()
            long r0 = r0.toEpochDay()
            org.threeten.bp.chrono.ChronoLocalDate r2 = r10.toLocalDate()
            long r2 = r2.toEpochDay()
            int r4 = (r0 > r2 ? 1 : (r0 == r2 ? 0 : -1))
            if (r4 < 0) goto L_0x002f
            int r4 = (r0 > r2 ? 1 : (r0 == r2 ? 0 : -1))
            if (r4 != 0) goto L_0x002d
            org.threeten.bp.LocalTime r4 = r9.toLocalTime()
            long r4 = r4.toNanoOfDay()
            org.threeten.bp.LocalTime r6 = r10.toLocalTime()
            long r6 = r6.toNanoOfDay()
            int r8 = (r4 > r6 ? 1 : (r4 == r6 ? 0 : -1))
            if (r8 >= 0) goto L_0x002d
            goto L_0x002f
        L_0x002d:
            r4 = 0
            goto L_0x0030
        L_0x002f:
            r4 = 1
        L_0x0030:
            return r4
        */
        throw new UnsupportedOperationException("Method not decompiled: org.threeten.bp.chrono.ChronoLocalDateTime.isBefore(org.threeten.bp.chrono.ChronoLocalDateTime):boolean");
    }

    /* JADX WARNING: type inference failed for: r6v0, types: [org.threeten.bp.chrono.ChronoLocalDateTime, org.threeten.bp.chrono.ChronoLocalDateTime<?>] */
    /* JADX WARNING: Unknown variable types count: 1 */
    /* Code decompiled incorrectly, please refer to instructions dump. */
    public boolean isEqual(org.threeten.bp.chrono.ChronoLocalDateTime<?> r6) {
        /*
            r5 = this;
            org.threeten.bp.LocalTime r0 = r5.toLocalTime()
            long r0 = r0.toNanoOfDay()
            org.threeten.bp.LocalTime r2 = r6.toLocalTime()
            long r2 = r2.toNanoOfDay()
            int r4 = (r0 > r2 ? 1 : (r0 == r2 ? 0 : -1))
            if (r4 != 0) goto L_0x002a
            org.threeten.bp.chrono.ChronoLocalDate r0 = r5.toLocalDate()
            long r0 = r0.toEpochDay()
            org.threeten.bp.chrono.ChronoLocalDate r2 = r6.toLocalDate()
            long r2 = r2.toEpochDay()
            int r4 = (r0 > r2 ? 1 : (r0 == r2 ? 0 : -1))
            if (r4 != 0) goto L_0x002a
            r0 = 1
            goto L_0x002b
        L_0x002a:
            r0 = 0
        L_0x002b:
            return r0
        */
        throw new UnsupportedOperationException("Method not decompiled: org.threeten.bp.chrono.ChronoLocalDateTime.isEqual(org.threeten.bp.chrono.ChronoLocalDateTime):boolean");
    }

    public boolean equals(Object obj) {
        if (this == obj) {
            return true;
        }
        if (!(obj instanceof ChronoLocalDateTime) || compareTo((ChronoLocalDateTime<?>) (ChronoLocalDateTime) obj) != 0) {
            return false;
        }
        return true;
    }

    public int hashCode() {
        return toLocalDate().hashCode() ^ toLocalTime().hashCode();
    }

    public String toString() {
        return toLocalDate().toString() + 'T' + toLocalTime().toString();
    }
}
