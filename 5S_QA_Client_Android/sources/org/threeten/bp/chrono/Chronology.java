package org.threeten.bp.chrono;

import java.io.DataInput;
import java.io.DataOutput;
import java.io.IOException;
import java.io.InvalidObjectException;
import java.io.ObjectStreamException;
import java.lang.reflect.InvocationTargetException;
import java.lang.reflect.Method;
import java.util.HashSet;
import java.util.Iterator;
import java.util.List;
import java.util.Locale;
import java.util.Map;
import java.util.ServiceLoader;
import java.util.Set;
import java.util.concurrent.ConcurrentHashMap;
import org.threeten.bp.Clock;
import org.threeten.bp.DateTimeException;
import org.threeten.bp.Instant;
import org.threeten.bp.LocalDate;
import org.threeten.bp.LocalTime;
import org.threeten.bp.ZoneId;
import org.threeten.bp.ZoneOffset;
import org.threeten.bp.format.DateTimeFormatterBuilder;
import org.threeten.bp.format.ResolverStyle;
import org.threeten.bp.format.TextStyle;
import org.threeten.bp.jdk8.DefaultInterfaceTemporalAccessor;
import org.threeten.bp.jdk8.Jdk8Methods;
import org.threeten.bp.temporal.ChronoField;
import org.threeten.bp.temporal.Temporal;
import org.threeten.bp.temporal.TemporalAccessor;
import org.threeten.bp.temporal.TemporalField;
import org.threeten.bp.temporal.TemporalQueries;
import org.threeten.bp.temporal.TemporalQuery;
import org.threeten.bp.temporal.UnsupportedTemporalTypeException;
import org.threeten.bp.temporal.ValueRange;

public abstract class Chronology implements Comparable<Chronology> {
    private static final ConcurrentHashMap<String, Chronology> CHRONOS_BY_ID = new ConcurrentHashMap<>();
    private static final ConcurrentHashMap<String, Chronology> CHRONOS_BY_TYPE = new ConcurrentHashMap<>();
    public static final TemporalQuery<Chronology> FROM = new TemporalQuery<Chronology>() {
        public Chronology queryFrom(TemporalAccessor temporal) {
            return Chronology.from(temporal);
        }
    };
    private static final Method LOCALE_METHOD;

    public abstract ChronoLocalDate date(int i, int i2, int i3);

    public abstract ChronoLocalDate date(TemporalAccessor temporalAccessor);

    public abstract ChronoLocalDate dateEpochDay(long j);

    public abstract ChronoLocalDate dateYearDay(int i, int i2);

    public abstract Era eraOf(int i);

    public abstract List<Era> eras();

    public abstract String getCalendarType();

    public abstract String getId();

    public abstract boolean isLeapYear(long j);

    public abstract int prolepticYear(Era era, int i);

    public abstract ValueRange range(ChronoField chronoField);

    public abstract ChronoLocalDate resolveDate(Map<TemporalField, Long> map, ResolverStyle resolverStyle);

    static {
        Method method = null;
        try {
            method = Locale.class.getMethod("getUnicodeLocaleType", new Class[]{String.class});
        } catch (Throwable th) {
        }
        LOCALE_METHOD = method;
    }

    public static Chronology from(TemporalAccessor temporal) {
        Jdk8Methods.requireNonNull(temporal, "temporal");
        Chronology obj = (Chronology) temporal.query(TemporalQueries.chronology());
        return obj != null ? obj : IsoChronology.INSTANCE;
    }

    public static Chronology ofLocale(Locale locale) {
        init();
        Jdk8Methods.requireNonNull(locale, "locale");
        String type = "iso";
        Method method = LOCALE_METHOD;
        if (method != null) {
            try {
                type = (String) method.invoke(locale, new Object[]{"ca"});
            } catch (IllegalAccessException | IllegalArgumentException | InvocationTargetException e) {
            }
        } else if (locale.equals(JapaneseChronology.LOCALE)) {
            type = "japanese";
        }
        if (type == null || "iso".equals(type) || "iso8601".equals(type)) {
            return IsoChronology.INSTANCE;
        }
        Chronology chrono = CHRONOS_BY_TYPE.get(type);
        if (chrono != null) {
            return chrono;
        }
        throw new DateTimeException("Unknown calendar system: " + type);
    }

    public static Chronology of(String id) {
        init();
        Chronology chrono = CHRONOS_BY_ID.get(id);
        if (chrono != null) {
            return chrono;
        }
        Chronology chrono2 = CHRONOS_BY_TYPE.get(id);
        if (chrono2 != null) {
            return chrono2;
        }
        throw new DateTimeException("Unknown chronology: " + id);
    }

    public static Set<Chronology> getAvailableChronologies() {
        init();
        return new HashSet(CHRONOS_BY_ID.values());
    }

    private static void init() {
        Class<Chronology> cls = Chronology.class;
        ConcurrentHashMap<String, Chronology> concurrentHashMap = CHRONOS_BY_ID;
        if (concurrentHashMap.isEmpty()) {
            register(IsoChronology.INSTANCE);
            register(ThaiBuddhistChronology.INSTANCE);
            register(MinguoChronology.INSTANCE);
            register(JapaneseChronology.INSTANCE);
            register(HijrahChronology.INSTANCE);
            concurrentHashMap.putIfAbsent("Hijrah", HijrahChronology.INSTANCE);
            CHRONOS_BY_TYPE.putIfAbsent("islamic", HijrahChronology.INSTANCE);
            Iterator i$ = ServiceLoader.load(cls, cls.getClassLoader()).iterator();
            while (i$.hasNext()) {
                Chronology chrono = i$.next();
                CHRONOS_BY_ID.putIfAbsent(chrono.getId(), chrono);
                String type = chrono.getCalendarType();
                if (type != null) {
                    CHRONOS_BY_TYPE.putIfAbsent(type, chrono);
                }
            }
        }
    }

    private static void register(Chronology chrono) {
        CHRONOS_BY_ID.putIfAbsent(chrono.getId(), chrono);
        String type = chrono.getCalendarType();
        if (type != null) {
            CHRONOS_BY_TYPE.putIfAbsent(type, chrono);
        }
    }

    protected Chronology() {
    }

    /* access modifiers changed from: package-private */
    public <D extends ChronoLocalDate> D ensureChronoLocalDate(Temporal temporal) {
        D other = (ChronoLocalDate) temporal;
        if (equals(other.getChronology())) {
            return other;
        }
        throw new ClassCastException("Chrono mismatch, expected: " + getId() + ", actual: " + other.getChronology().getId());
    }

    /* access modifiers changed from: package-private */
    public <D extends ChronoLocalDate> ChronoLocalDateTimeImpl<D> ensureChronoLocalDateTime(Temporal temporal) {
        ChronoLocalDateTimeImpl<D> other = (ChronoLocalDateTimeImpl) temporal;
        if (equals(other.toLocalDate().getChronology())) {
            return other;
        }
        throw new ClassCastException("Chrono mismatch, required: " + getId() + ", supplied: " + other.toLocalDate().getChronology().getId());
    }

    /* access modifiers changed from: package-private */
    public <D extends ChronoLocalDate> ChronoZonedDateTimeImpl<D> ensureChronoZonedDateTime(Temporal temporal) {
        ChronoZonedDateTimeImpl<D> other = (ChronoZonedDateTimeImpl) temporal;
        if (equals(other.toLocalDate().getChronology())) {
            return other;
        }
        throw new ClassCastException("Chrono mismatch, required: " + getId() + ", supplied: " + other.toLocalDate().getChronology().getId());
    }

    public ChronoLocalDate date(Era era, int yearOfEra, int month, int dayOfMonth) {
        return date(prolepticYear(era, yearOfEra), month, dayOfMonth);
    }

    public ChronoLocalDate dateYearDay(Era era, int yearOfEra, int dayOfYear) {
        return dateYearDay(prolepticYear(era, yearOfEra), dayOfYear);
    }

    public ChronoLocalDate dateNow() {
        return dateNow(Clock.systemDefaultZone());
    }

    public ChronoLocalDate dateNow(ZoneId zone) {
        return dateNow(Clock.system(zone));
    }

    public ChronoLocalDate dateNow(Clock clock) {
        Jdk8Methods.requireNonNull(clock, "clock");
        return date(LocalDate.now(clock));
    }

    public ChronoLocalDateTime<?> localDateTime(TemporalAccessor temporal) {
        try {
            return date(temporal).atTime(LocalTime.from(temporal));
        } catch (DateTimeException ex) {
            throw new DateTimeException("Unable to obtain ChronoLocalDateTime from TemporalAccessor: " + temporal.getClass(), ex);
        }
    }

    public ChronoZonedDateTime<?> zonedDateTime(TemporalAccessor temporal) {
        try {
            ZoneId zone = ZoneId.from(temporal);
            try {
                return zonedDateTime(Instant.from(temporal), zone);
            } catch (DateTimeException e) {
                return ChronoZonedDateTimeImpl.ofBest(ensureChronoLocalDateTime(localDateTime(temporal)), zone, (ZoneOffset) null);
            }
        } catch (DateTimeException ex) {
            throw new DateTimeException("Unable to obtain ChronoZonedDateTime from TemporalAccessor: " + temporal.getClass(), ex);
        }
    }

    public ChronoZonedDateTime<?> zonedDateTime(Instant instant, ZoneId zone) {
        return ChronoZonedDateTimeImpl.ofInstant(this, instant, zone);
    }

    public ChronoPeriod period(int years, int months, int days) {
        return new ChronoPeriodImpl(this, years, months, days);
    }

    public String getDisplayName(TextStyle style, Locale locale) {
        return new DateTimeFormatterBuilder().appendChronologyText(style).toFormatter(locale).format(new DefaultInterfaceTemporalAccessor() {
            public boolean isSupported(TemporalField field) {
                return false;
            }

            public long getLong(TemporalField field) {
                throw new UnsupportedTemporalTypeException("Unsupported field: " + field);
            }

            public <R> R query(TemporalQuery<R> query) {
                if (query == TemporalQueries.chronology()) {
                    return Chronology.this;
                }
                return super.query(query);
            }
        });
    }

    /* access modifiers changed from: package-private */
    public void updateResolveMap(Map<TemporalField, Long> fieldValues, ChronoField field, long value) {
        Long current = fieldValues.get(field);
        if (current == null || current.longValue() == value) {
            fieldValues.put(field, Long.valueOf(value));
            return;
        }
        throw new DateTimeException("Invalid state, field: " + field + " " + current + " conflicts with " + field + " " + value);
    }

    public int compareTo(Chronology other) {
        return getId().compareTo(other.getId());
    }

    public boolean equals(Object obj) {
        if (this == obj) {
            return true;
        }
        if (!(obj instanceof Chronology) || compareTo((Chronology) obj) != 0) {
            return false;
        }
        return true;
    }

    public int hashCode() {
        return getClass().hashCode() ^ getId().hashCode();
    }

    public String toString() {
        return getId();
    }

    private Object writeReplace() {
        return new Ser((byte) 11, this);
    }

    private Object readResolve() throws ObjectStreamException {
        throw new InvalidObjectException("Deserialization via serialization delegate");
    }

    /* access modifiers changed from: package-private */
    public void writeExternal(DataOutput out) throws IOException {
        out.writeUTF(getId());
    }

    static Chronology readExternal(DataInput in) throws IOException {
        return of(in.readUTF());
    }
}
