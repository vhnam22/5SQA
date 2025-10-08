package org.threeten.bp.format;

import java.util.Locale;
import org.threeten.bp.DateTimeException;
import org.threeten.bp.Instant;
import org.threeten.bp.ZoneId;
import org.threeten.bp.ZoneOffset;
import org.threeten.bp.chrono.ChronoLocalDate;
import org.threeten.bp.chrono.Chronology;
import org.threeten.bp.chrono.IsoChronology;
import org.threeten.bp.jdk8.DefaultInterfaceTemporalAccessor;
import org.threeten.bp.jdk8.Jdk8Methods;
import org.threeten.bp.temporal.ChronoField;
import org.threeten.bp.temporal.TemporalAccessor;
import org.threeten.bp.temporal.TemporalField;
import org.threeten.bp.temporal.TemporalQueries;
import org.threeten.bp.temporal.TemporalQuery;
import org.threeten.bp.temporal.ValueRange;

final class DateTimePrintContext {
    private Locale locale;
    private int optional;
    private DecimalStyle symbols;
    private TemporalAccessor temporal;

    DateTimePrintContext(TemporalAccessor temporal2, DateTimeFormatter formatter) {
        this.temporal = adjust(temporal2, formatter);
        this.locale = formatter.getLocale();
        this.symbols = formatter.getDecimalStyle();
    }

    DateTimePrintContext(TemporalAccessor temporal2, Locale locale2, DecimalStyle symbols2) {
        this.temporal = temporal2;
        this.locale = locale2;
        this.symbols = symbols2;
    }

    private static TemporalAccessor adjust(final TemporalAccessor temporal2, DateTimeFormatter formatter) {
        final ChronoLocalDate effectiveDate;
        Chronology overrideChrono = formatter.getChronology();
        ZoneId overrideZone = formatter.getZone();
        if (overrideChrono == null && overrideZone == null) {
            return temporal2;
        }
        Chronology temporalChrono = (Chronology) temporal2.query(TemporalQueries.chronology());
        ZoneId temporalZone = (ZoneId) temporal2.query(TemporalQueries.zoneId());
        if (Jdk8Methods.equals(temporalChrono, overrideChrono)) {
            overrideChrono = null;
        }
        if (Jdk8Methods.equals(temporalZone, overrideZone)) {
            overrideZone = null;
        }
        if (overrideChrono == null && overrideZone == null) {
            return temporal2;
        }
        final Chronology effectiveChrono = overrideChrono != null ? overrideChrono : temporalChrono;
        final ZoneId effectiveZone = overrideZone != null ? overrideZone : temporalZone;
        if (overrideZone != null) {
            if (temporal2.isSupported(ChronoField.INSTANT_SECONDS)) {
                return (effectiveChrono != null ? effectiveChrono : IsoChronology.INSTANCE).zonedDateTime(Instant.from(temporal2), overrideZone);
            }
            ZoneId normalizedOffset = overrideZone.normalized();
            ZoneOffset temporalOffset = (ZoneOffset) temporal2.query(TemporalQueries.offset());
            if ((normalizedOffset instanceof ZoneOffset) && temporalOffset != null && !normalizedOffset.equals(temporalOffset)) {
                throw new DateTimeException("Invalid override zone for temporal: " + overrideZone + " " + temporal2);
            }
        }
        if (overrideChrono == null) {
            effectiveDate = null;
        } else if (temporal2.isSupported(ChronoField.EPOCH_DAY)) {
            effectiveDate = effectiveChrono.date(temporal2);
        } else {
            if (!(overrideChrono == IsoChronology.INSTANCE && temporalChrono == null)) {
                ChronoField[] arr$ = ChronoField.values();
                int len$ = arr$.length;
                int i$ = 0;
                while (i$ < len$) {
                    ChronoField f = arr$[i$];
                    if (!f.isDateBased() || !temporal2.isSupported(f)) {
                        i$++;
                    } else {
                        throw new DateTimeException("Invalid override chronology for temporal: " + overrideChrono + " " + temporal2);
                    }
                }
            }
            effectiveDate = null;
        }
        return new DefaultInterfaceTemporalAccessor() {
            public boolean isSupported(TemporalField field) {
                if (effectiveDate == null || !field.isDateBased()) {
                    return temporal2.isSupported(field);
                }
                return effectiveDate.isSupported(field);
            }

            public ValueRange range(TemporalField field) {
                if (effectiveDate == null || !field.isDateBased()) {
                    return temporal2.range(field);
                }
                return effectiveDate.range(field);
            }

            public long getLong(TemporalField field) {
                if (effectiveDate == null || !field.isDateBased()) {
                    return temporal2.getLong(field);
                }
                return effectiveDate.getLong(field);
            }

            public <R> R query(TemporalQuery<R> query) {
                if (query == TemporalQueries.chronology()) {
                    return effectiveChrono;
                }
                if (query == TemporalQueries.zoneId()) {
                    return effectiveZone;
                }
                if (query == TemporalQueries.precision()) {
                    return temporal2.query(query);
                }
                return query.queryFrom(this);
            }
        };
    }

    /* access modifiers changed from: package-private */
    public TemporalAccessor getTemporal() {
        return this.temporal;
    }

    /* access modifiers changed from: package-private */
    public Locale getLocale() {
        return this.locale;
    }

    /* access modifiers changed from: package-private */
    public DecimalStyle getSymbols() {
        return this.symbols;
    }

    /* access modifiers changed from: package-private */
    public void startOptional() {
        this.optional++;
    }

    /* access modifiers changed from: package-private */
    public void endOptional() {
        this.optional--;
    }

    /* access modifiers changed from: package-private */
    public <R> R getValue(TemporalQuery<R> query) {
        R result = this.temporal.query(query);
        if (result != null || this.optional != 0) {
            return result;
        }
        throw new DateTimeException("Unable to extract value: " + this.temporal.getClass());
    }

    /* access modifiers changed from: package-private */
    public Long getValue(TemporalField field) {
        try {
            return Long.valueOf(this.temporal.getLong(field));
        } catch (DateTimeException ex) {
            if (this.optional > 0) {
                return null;
            }
            throw ex;
        }
    }

    public String toString() {
        return this.temporal.toString();
    }

    /* access modifiers changed from: package-private */
    public void setDateTime(TemporalAccessor temporal2) {
        Jdk8Methods.requireNonNull(temporal2, "temporal");
        this.temporal = temporal2;
    }

    /* access modifiers changed from: package-private */
    public void setLocale(Locale locale2) {
        Jdk8Methods.requireNonNull(locale2, "locale");
        this.locale = locale2;
    }
}
