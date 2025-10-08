package org.threeten.bp.format;

import java.util.HashMap;
import java.util.Iterator;
import java.util.Map;
import java.util.Set;
import org.threeten.bp.DateTimeException;
import org.threeten.bp.Instant;
import org.threeten.bp.LocalDate;
import org.threeten.bp.LocalTime;
import org.threeten.bp.Period;
import org.threeten.bp.ZoneId;
import org.threeten.bp.ZoneOffset;
import org.threeten.bp.chrono.ChronoLocalDate;
import org.threeten.bp.chrono.ChronoLocalDateTime;
import org.threeten.bp.chrono.ChronoZonedDateTime;
import org.threeten.bp.chrono.Chronology;
import org.threeten.bp.chrono.IsoChronology;
import org.threeten.bp.jdk8.DefaultInterfaceTemporalAccessor;
import org.threeten.bp.jdk8.Jdk8Methods;
import org.threeten.bp.temporal.ChronoField;
import org.threeten.bp.temporal.TemporalAccessor;
import org.threeten.bp.temporal.TemporalAmount;
import org.threeten.bp.temporal.TemporalField;
import org.threeten.bp.temporal.TemporalQueries;
import org.threeten.bp.temporal.TemporalQuery;

final class DateTimeBuilder extends DefaultInterfaceTemporalAccessor implements TemporalAccessor, Cloneable {
    Chronology chrono;
    ChronoLocalDate date;
    Period excessDays;
    final Map<TemporalField, Long> fieldValues = new HashMap();
    boolean leapSecond;
    LocalTime time;
    ZoneId zone;

    public DateTimeBuilder() {
    }

    public DateTimeBuilder(TemporalField field, long value) {
        addFieldValue(field, value);
    }

    private Long getFieldValue0(TemporalField field) {
        return this.fieldValues.get(field);
    }

    /* access modifiers changed from: package-private */
    public DateTimeBuilder addFieldValue(TemporalField field, long value) {
        Jdk8Methods.requireNonNull(field, "field");
        Long old = getFieldValue0(field);
        if (old == null || old.longValue() == value) {
            return putFieldValue0(field, value);
        }
        throw new DateTimeException("Conflict found: " + field + " " + old + " differs from " + field + " " + value + ": " + this);
    }

    private DateTimeBuilder putFieldValue0(TemporalField field, long value) {
        this.fieldValues.put(field, Long.valueOf(value));
        return this;
    }

    /* access modifiers changed from: package-private */
    public void addObject(ChronoLocalDate date2) {
        this.date = date2;
    }

    /* access modifiers changed from: package-private */
    public void addObject(LocalTime time2) {
        this.time = time2;
    }

    public DateTimeBuilder resolve(ResolverStyle resolverStyle, Set<TemporalField> resolverFields) {
        ChronoLocalDate chronoLocalDate;
        if (resolverFields != null) {
            this.fieldValues.keySet().retainAll(resolverFields);
        }
        mergeInstantFields();
        mergeDate(resolverStyle);
        mergeTime(resolverStyle);
        if (resolveFields(resolverStyle)) {
            mergeInstantFields();
            mergeDate(resolverStyle);
            mergeTime(resolverStyle);
        }
        resolveTimeInferZeroes(resolverStyle);
        crossCheck();
        Period period = this.excessDays;
        if (!(period == null || period.isZero() || (chronoLocalDate = this.date) == null || this.time == null)) {
            this.date = chronoLocalDate.plus((TemporalAmount) this.excessDays);
            this.excessDays = Period.ZERO;
        }
        resolveFractional();
        resolveInstant();
        return this;
    }

    private boolean resolveFields(ResolverStyle resolverStyle) {
        int changes = 0;
        loop0:
        while (changes < 100) {
            for (Map.Entry<TemporalField, Long> entry : this.fieldValues.entrySet()) {
                TemporalField targetField = entry.getKey();
                TemporalAccessor resolvedObject = targetField.resolve(this.fieldValues, this, resolverStyle);
                if (resolvedObject != null) {
                    if (resolvedObject instanceof ChronoZonedDateTime) {
                        ChronoZonedDateTime chronoZonedDateTime = (ChronoZonedDateTime) resolvedObject;
                        ZoneId zoneId = this.zone;
                        if (zoneId == null) {
                            this.zone = chronoZonedDateTime.getZone();
                        } else if (!zoneId.equals(chronoZonedDateTime.getZone())) {
                            throw new DateTimeException("ChronoZonedDateTime must use the effective parsed zone: " + this.zone);
                        }
                        resolvedObject = chronoZonedDateTime.toLocalDateTime();
                    }
                    if (resolvedObject instanceof ChronoLocalDate) {
                        resolveMakeChanges(targetField, (ChronoLocalDate) resolvedObject);
                        changes++;
                    } else if (resolvedObject instanceof LocalTime) {
                        resolveMakeChanges(targetField, (LocalTime) resolvedObject);
                        changes++;
                    } else if (resolvedObject instanceof ChronoLocalDateTime) {
                        ChronoLocalDateTime chronoLocalDateTime = (ChronoLocalDateTime) resolvedObject;
                        resolveMakeChanges(targetField, chronoLocalDateTime.toLocalDate());
                        resolveMakeChanges(targetField, chronoLocalDateTime.toLocalTime());
                        changes++;
                    } else {
                        throw new DateTimeException("Unknown type: " + resolvedObject.getClass().getName());
                    }
                } else if (!this.fieldValues.containsKey(targetField)) {
                    changes++;
                }
            }
        }
        if (changes != 100) {
            return changes > 0;
        }
        throw new DateTimeException("Badly written field");
    }

    private void resolveMakeChanges(TemporalField targetField, ChronoLocalDate date2) {
        if (this.chrono.equals(date2.getChronology())) {
            long epochDay = date2.toEpochDay();
            Long old = this.fieldValues.put(ChronoField.EPOCH_DAY, Long.valueOf(epochDay));
            if (old != null && old.longValue() != epochDay) {
                throw new DateTimeException("Conflict found: " + LocalDate.ofEpochDay(old.longValue()) + " differs from " + LocalDate.ofEpochDay(epochDay) + " while resolving  " + targetField);
            }
            return;
        }
        throw new DateTimeException("ChronoLocalDate must use the effective parsed chronology: " + this.chrono);
    }

    private void resolveMakeChanges(TemporalField targetField, LocalTime time2) {
        long nanOfDay = time2.toNanoOfDay();
        Long old = this.fieldValues.put(ChronoField.NANO_OF_DAY, Long.valueOf(nanOfDay));
        if (old != null && old.longValue() != nanOfDay) {
            throw new DateTimeException("Conflict found: " + LocalTime.ofNanoOfDay(old.longValue()) + " differs from " + time2 + " while resolving  " + targetField);
        }
    }

    private void mergeDate(ResolverStyle resolverStyle) {
        if (this.chrono instanceof IsoChronology) {
            checkDate(IsoChronology.INSTANCE.resolveDate(this.fieldValues, resolverStyle));
        } else if (this.fieldValues.containsKey(ChronoField.EPOCH_DAY)) {
            checkDate(LocalDate.ofEpochDay(this.fieldValues.remove(ChronoField.EPOCH_DAY).longValue()));
        }
    }

    private void checkDate(LocalDate date2) {
        if (date2 != null) {
            addObject((ChronoLocalDate) date2);
            for (TemporalField field : this.fieldValues.keySet()) {
                if ((field instanceof ChronoField) && field.isDateBased()) {
                    try {
                        long val1 = date2.getLong(field);
                        Long val2 = this.fieldValues.get(field);
                        if (val1 != val2.longValue()) {
                            throw new DateTimeException("Conflict found: Field " + field + " " + val1 + " differs from " + field + " " + val2 + " derived from " + date2);
                        }
                    } catch (DateTimeException e) {
                    }
                }
            }
        }
    }

    private void mergeTime(ResolverStyle resolverStyle) {
        long j = 0;
        if (this.fieldValues.containsKey(ChronoField.CLOCK_HOUR_OF_DAY)) {
            long ch = this.fieldValues.remove(ChronoField.CLOCK_HOUR_OF_DAY).longValue();
            if (!(resolverStyle == ResolverStyle.LENIENT || (resolverStyle == ResolverStyle.SMART && ch == 0))) {
                ChronoField.CLOCK_HOUR_OF_DAY.checkValidValue(ch);
            }
            addFieldValue(ChronoField.HOUR_OF_DAY, ch == 24 ? 0 : ch);
        }
        if (this.fieldValues.containsKey(ChronoField.CLOCK_HOUR_OF_AMPM)) {
            long ch2 = this.fieldValues.remove(ChronoField.CLOCK_HOUR_OF_AMPM).longValue();
            if (!(resolverStyle == ResolverStyle.LENIENT || (resolverStyle == ResolverStyle.SMART && ch2 == 0))) {
                ChronoField.CLOCK_HOUR_OF_AMPM.checkValidValue(ch2);
            }
            ChronoField chronoField = ChronoField.HOUR_OF_AMPM;
            if (ch2 != 12) {
                j = ch2;
            }
            addFieldValue(chronoField, j);
        }
        if (resolverStyle != ResolverStyle.LENIENT) {
            if (this.fieldValues.containsKey(ChronoField.AMPM_OF_DAY)) {
                ChronoField.AMPM_OF_DAY.checkValidValue(this.fieldValues.get(ChronoField.AMPM_OF_DAY).longValue());
            }
            if (this.fieldValues.containsKey(ChronoField.HOUR_OF_AMPM)) {
                ChronoField.HOUR_OF_AMPM.checkValidValue(this.fieldValues.get(ChronoField.HOUR_OF_AMPM).longValue());
            }
        }
        if (this.fieldValues.containsKey(ChronoField.AMPM_OF_DAY) && this.fieldValues.containsKey(ChronoField.HOUR_OF_AMPM)) {
            addFieldValue(ChronoField.HOUR_OF_DAY, (12 * this.fieldValues.remove(ChronoField.AMPM_OF_DAY).longValue()) + this.fieldValues.remove(ChronoField.HOUR_OF_AMPM).longValue());
        }
        if (this.fieldValues.containsKey(ChronoField.NANO_OF_DAY)) {
            long nod = this.fieldValues.remove(ChronoField.NANO_OF_DAY).longValue();
            if (resolverStyle != ResolverStyle.LENIENT) {
                ChronoField.NANO_OF_DAY.checkValidValue(nod);
            }
            addFieldValue(ChronoField.SECOND_OF_DAY, nod / 1000000000);
            addFieldValue(ChronoField.NANO_OF_SECOND, nod % 1000000000);
        }
        if (this.fieldValues.containsKey(ChronoField.MICRO_OF_DAY)) {
            long cod = this.fieldValues.remove(ChronoField.MICRO_OF_DAY).longValue();
            if (resolverStyle != ResolverStyle.LENIENT) {
                ChronoField.MICRO_OF_DAY.checkValidValue(cod);
            }
            addFieldValue(ChronoField.SECOND_OF_DAY, cod / 1000000);
            addFieldValue(ChronoField.MICRO_OF_SECOND, cod % 1000000);
        }
        if (this.fieldValues.containsKey(ChronoField.MILLI_OF_DAY)) {
            long lod = this.fieldValues.remove(ChronoField.MILLI_OF_DAY).longValue();
            if (resolverStyle != ResolverStyle.LENIENT) {
                ChronoField.MILLI_OF_DAY.checkValidValue(lod);
            }
            addFieldValue(ChronoField.SECOND_OF_DAY, lod / 1000);
            addFieldValue(ChronoField.MILLI_OF_SECOND, lod % 1000);
        }
        if (this.fieldValues.containsKey(ChronoField.SECOND_OF_DAY)) {
            long sod = this.fieldValues.remove(ChronoField.SECOND_OF_DAY).longValue();
            if (resolverStyle != ResolverStyle.LENIENT) {
                ChronoField.SECOND_OF_DAY.checkValidValue(sod);
            }
            addFieldValue(ChronoField.HOUR_OF_DAY, sod / 3600);
            addFieldValue(ChronoField.MINUTE_OF_HOUR, (sod / 60) % 60);
            addFieldValue(ChronoField.SECOND_OF_MINUTE, sod % 60);
        }
        if (this.fieldValues.containsKey(ChronoField.MINUTE_OF_DAY)) {
            long mod = this.fieldValues.remove(ChronoField.MINUTE_OF_DAY).longValue();
            if (resolverStyle != ResolverStyle.LENIENT) {
                ChronoField.MINUTE_OF_DAY.checkValidValue(mod);
            }
            addFieldValue(ChronoField.HOUR_OF_DAY, mod / 60);
            addFieldValue(ChronoField.MINUTE_OF_HOUR, mod % 60);
        }
        if (resolverStyle != ResolverStyle.LENIENT) {
            if (this.fieldValues.containsKey(ChronoField.MILLI_OF_SECOND)) {
                ChronoField.MILLI_OF_SECOND.checkValidValue(this.fieldValues.get(ChronoField.MILLI_OF_SECOND).longValue());
            }
            if (this.fieldValues.containsKey(ChronoField.MICRO_OF_SECOND)) {
                ChronoField.MICRO_OF_SECOND.checkValidValue(this.fieldValues.get(ChronoField.MICRO_OF_SECOND).longValue());
            }
        }
        if (this.fieldValues.containsKey(ChronoField.MILLI_OF_SECOND) && this.fieldValues.containsKey(ChronoField.MICRO_OF_SECOND)) {
            addFieldValue(ChronoField.MICRO_OF_SECOND, (this.fieldValues.remove(ChronoField.MILLI_OF_SECOND).longValue() * 1000) + (this.fieldValues.get(ChronoField.MICRO_OF_SECOND).longValue() % 1000));
        }
        if (this.fieldValues.containsKey(ChronoField.MICRO_OF_SECOND) && this.fieldValues.containsKey(ChronoField.NANO_OF_SECOND)) {
            addFieldValue(ChronoField.MICRO_OF_SECOND, this.fieldValues.get(ChronoField.NANO_OF_SECOND).longValue() / 1000);
            this.fieldValues.remove(ChronoField.MICRO_OF_SECOND);
        }
        if (this.fieldValues.containsKey(ChronoField.MILLI_OF_SECOND) && this.fieldValues.containsKey(ChronoField.NANO_OF_SECOND)) {
            addFieldValue(ChronoField.MILLI_OF_SECOND, this.fieldValues.get(ChronoField.NANO_OF_SECOND).longValue() / 1000000);
            this.fieldValues.remove(ChronoField.MILLI_OF_SECOND);
        }
        if (this.fieldValues.containsKey(ChronoField.MICRO_OF_SECOND)) {
            addFieldValue(ChronoField.NANO_OF_SECOND, 1000 * this.fieldValues.remove(ChronoField.MICRO_OF_SECOND).longValue());
        } else if (this.fieldValues.containsKey(ChronoField.MILLI_OF_SECOND)) {
            addFieldValue(ChronoField.NANO_OF_SECOND, 1000000 * this.fieldValues.remove(ChronoField.MILLI_OF_SECOND).longValue());
        }
    }

    private void resolveTimeInferZeroes(ResolverStyle resolverStyle) {
        Long hod = this.fieldValues.get(ChronoField.HOUR_OF_DAY);
        Long moh = this.fieldValues.get(ChronoField.MINUTE_OF_HOUR);
        Long som = this.fieldValues.get(ChronoField.SECOND_OF_MINUTE);
        Long nos = this.fieldValues.get(ChronoField.NANO_OF_SECOND);
        if (hod != null) {
            if (moh == null && (som != null || nos != null)) {
                return;
            }
            if (moh == null || som != null || nos == null) {
                if (resolverStyle != ResolverStyle.LENIENT) {
                    if (hod != null) {
                        if (resolverStyle == ResolverStyle.SMART && hod.longValue() == 24 && ((moh == null || moh.longValue() == 0) && ((som == null || som.longValue() == 0) && (nos == null || nos.longValue() == 0)))) {
                            hod = 0L;
                            this.excessDays = Period.ofDays(1);
                        }
                        int hodVal = ChronoField.HOUR_OF_DAY.checkValidIntValue(hod.longValue());
                        if (moh != null) {
                            int mohVal = ChronoField.MINUTE_OF_HOUR.checkValidIntValue(moh.longValue());
                            if (som != null) {
                                int somVal = ChronoField.SECOND_OF_MINUTE.checkValidIntValue(som.longValue());
                                if (nos != null) {
                                    addObject(LocalTime.of(hodVal, mohVal, somVal, ChronoField.NANO_OF_SECOND.checkValidIntValue(nos.longValue())));
                                } else {
                                    addObject(LocalTime.of(hodVal, mohVal, somVal));
                                }
                            } else if (nos == null) {
                                addObject(LocalTime.of(hodVal, mohVal));
                            }
                        } else if (som == null && nos == null) {
                            addObject(LocalTime.of(hodVal, 0));
                        }
                    }
                } else if (hod != null) {
                    long hodVal2 = hod.longValue();
                    if (moh == null) {
                        int excessDays2 = Jdk8Methods.safeToInt(Jdk8Methods.floorDiv(hodVal2, 24));
                        addObject(LocalTime.of((int) ((long) Jdk8Methods.floorMod(hodVal2, 24)), 0));
                        this.excessDays = Period.ofDays(excessDays2);
                    } else if (som != null) {
                        if (nos == null) {
                            nos = 0L;
                        }
                        long totalNanos = Jdk8Methods.safeAdd(Jdk8Methods.safeAdd(Jdk8Methods.safeAdd(Jdk8Methods.safeMultiply(hodVal2, 3600000000000L), Jdk8Methods.safeMultiply(moh.longValue(), 60000000000L)), Jdk8Methods.safeMultiply(som.longValue(), 1000000000)), nos.longValue());
                        int excessDays3 = (int) Jdk8Methods.floorDiv(totalNanos, 86400000000000L);
                        addObject(LocalTime.ofNanoOfDay(Jdk8Methods.floorMod(totalNanos, 86400000000000L)));
                        this.excessDays = Period.ofDays(excessDays3);
                    } else {
                        long totalSecs = Jdk8Methods.safeAdd(Jdk8Methods.safeMultiply(hodVal2, 3600), Jdk8Methods.safeMultiply(moh.longValue(), 60));
                        int excessDays4 = (int) Jdk8Methods.floorDiv(totalSecs, 86400);
                        addObject(LocalTime.ofSecondOfDay(Jdk8Methods.floorMod(totalSecs, 86400)));
                        this.excessDays = Period.ofDays(excessDays4);
                    }
                }
                this.fieldValues.remove(ChronoField.HOUR_OF_DAY);
                this.fieldValues.remove(ChronoField.MINUTE_OF_HOUR);
                this.fieldValues.remove(ChronoField.SECOND_OF_MINUTE);
                this.fieldValues.remove(ChronoField.NANO_OF_SECOND);
            }
        }
    }

    private void mergeInstantFields() {
        if (this.fieldValues.containsKey(ChronoField.INSTANT_SECONDS)) {
            ZoneId zoneId = this.zone;
            if (zoneId != null) {
                mergeInstantFields0(zoneId);
                return;
            }
            Long offsetSecs = this.fieldValues.get(ChronoField.OFFSET_SECONDS);
            if (offsetSecs != null) {
                mergeInstantFields0(ZoneOffset.ofTotalSeconds(offsetSecs.intValue()));
            }
        }
    }

    private void mergeInstantFields0(ZoneId selectedZone) {
        ChronoZonedDateTime zonedDateTime = this.chrono.zonedDateTime(Instant.ofEpochSecond(this.fieldValues.remove(ChronoField.INSTANT_SECONDS).longValue()), selectedZone);
        if (this.date == null) {
            addObject(zonedDateTime.toLocalDate());
        } else {
            resolveMakeChanges((TemporalField) ChronoField.INSTANT_SECONDS, zonedDateTime.toLocalDate());
        }
        addFieldValue(ChronoField.SECOND_OF_DAY, (long) zonedDateTime.toLocalTime().toSecondOfDay());
    }

    private void crossCheck() {
        LocalTime localTime;
        if (this.fieldValues.size() > 0) {
            ChronoLocalDate chronoLocalDate = this.date;
            if (chronoLocalDate != null && (localTime = this.time) != null) {
                crossCheck(chronoLocalDate.atTime(localTime));
            } else if (chronoLocalDate != null) {
                crossCheck(chronoLocalDate);
            } else {
                LocalTime localTime2 = this.time;
                if (localTime2 != null) {
                    crossCheck(localTime2);
                }
            }
        }
    }

    private void crossCheck(TemporalAccessor temporal) {
        Iterator<Map.Entry<TemporalField, Long>> it = this.fieldValues.entrySet().iterator();
        while (it.hasNext()) {
            Map.Entry<TemporalField, Long> entry = it.next();
            TemporalField field = entry.getKey();
            long value = entry.getValue().longValue();
            if (temporal.isSupported(field)) {
                try {
                    long temporalValue = temporal.getLong(field);
                    if (temporalValue == value) {
                        it.remove();
                    } else {
                        throw new DateTimeException("Cross check failed: " + field + " " + temporalValue + " vs " + field + " " + value);
                    }
                } catch (RuntimeException e) {
                }
            }
        }
    }

    private void resolveFractional() {
        if (this.time != null) {
            return;
        }
        if (!this.fieldValues.containsKey(ChronoField.INSTANT_SECONDS) && !this.fieldValues.containsKey(ChronoField.SECOND_OF_DAY) && !this.fieldValues.containsKey(ChronoField.SECOND_OF_MINUTE)) {
            return;
        }
        if (this.fieldValues.containsKey(ChronoField.NANO_OF_SECOND)) {
            long nos = this.fieldValues.get(ChronoField.NANO_OF_SECOND).longValue();
            this.fieldValues.put(ChronoField.MICRO_OF_SECOND, Long.valueOf(nos / 1000));
            this.fieldValues.put(ChronoField.MILLI_OF_SECOND, Long.valueOf(nos / 1000000));
            return;
        }
        this.fieldValues.put(ChronoField.NANO_OF_SECOND, 0L);
        this.fieldValues.put(ChronoField.MICRO_OF_SECOND, 0L);
        this.fieldValues.put(ChronoField.MILLI_OF_SECOND, 0L);
    }

    private void resolveInstant() {
        LocalTime localTime;
        ChronoLocalDate chronoLocalDate = this.date;
        if (chronoLocalDate != null && (localTime = this.time) != null) {
            if (this.zone != null) {
                this.fieldValues.put(ChronoField.INSTANT_SECONDS, Long.valueOf(chronoLocalDate.atTime(localTime).atZone(this.zone).getLong(ChronoField.INSTANT_SECONDS)));
                return;
            }
            Long offsetSecs = this.fieldValues.get(ChronoField.OFFSET_SECONDS);
            if (offsetSecs != null) {
                this.fieldValues.put(ChronoField.INSTANT_SECONDS, Long.valueOf(this.date.atTime(this.time).atZone(ZoneOffset.ofTotalSeconds(offsetSecs.intValue())).getLong(ChronoField.INSTANT_SECONDS)));
            }
        }
    }

    public <R> R build(TemporalQuery<R> type) {
        return type.queryFrom(this);
    }

    public boolean isSupported(TemporalField field) {
        ChronoLocalDate chronoLocalDate;
        LocalTime localTime;
        if (field == null) {
            return false;
        }
        if (this.fieldValues.containsKey(field) || (((chronoLocalDate = this.date) != null && chronoLocalDate.isSupported(field)) || ((localTime = this.time) != null && localTime.isSupported(field)))) {
            return true;
        }
        return false;
    }

    public long getLong(TemporalField field) {
        Jdk8Methods.requireNonNull(field, "field");
        Long value = getFieldValue0(field);
        if (value != null) {
            return value.longValue();
        }
        ChronoLocalDate chronoLocalDate = this.date;
        if (chronoLocalDate != null && chronoLocalDate.isSupported(field)) {
            return this.date.getLong(field);
        }
        LocalTime localTime = this.time;
        if (localTime != null && localTime.isSupported(field)) {
            return this.time.getLong(field);
        }
        throw new DateTimeException("Field not found: " + field);
    }

    public <R> R query(TemporalQuery<R> query) {
        if (query == TemporalQueries.zoneId()) {
            return this.zone;
        }
        if (query == TemporalQueries.chronology()) {
            return this.chrono;
        }
        if (query == TemporalQueries.localDate()) {
            ChronoLocalDate chronoLocalDate = this.date;
            if (chronoLocalDate != null) {
                return LocalDate.from(chronoLocalDate);
            }
            return null;
        } else if (query == TemporalQueries.localTime()) {
            return this.time;
        } else {
            if (query == TemporalQueries.zone() || query == TemporalQueries.offset()) {
                return query.queryFrom(this);
            }
            if (query == TemporalQueries.precision()) {
                return null;
            }
            return query.queryFrom(this);
        }
    }

    public String toString() {
        StringBuilder buf = new StringBuilder(128);
        buf.append("DateTimeBuilder[");
        if (this.fieldValues.size() > 0) {
            buf.append("fields=").append(this.fieldValues);
        }
        buf.append(", ").append(this.chrono);
        buf.append(", ").append(this.zone);
        buf.append(", ").append(this.date);
        buf.append(", ").append(this.time);
        buf.append(']');
        return buf.toString();
    }
}
