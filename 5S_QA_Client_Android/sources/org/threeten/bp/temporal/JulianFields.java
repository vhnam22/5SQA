package org.threeten.bp.temporal;

import java.util.Locale;
import java.util.Map;
import org.threeten.bp.DateTimeException;
import org.threeten.bp.chrono.Chronology;
import org.threeten.bp.format.ResolverStyle;
import org.threeten.bp.jdk8.Jdk8Methods;

public final class JulianFields {
    public static final TemporalField JULIAN_DAY = Field.JULIAN_DAY;
    public static final TemporalField MODIFIED_JULIAN_DAY = Field.MODIFIED_JULIAN_DAY;
    public static final TemporalField RATA_DIE = Field.RATA_DIE;

    private enum Field implements TemporalField {
        JULIAN_DAY("JulianDay", ChronoUnit.DAYS, ChronoUnit.FOREVER, 2440588),
        MODIFIED_JULIAN_DAY("ModifiedJulianDay", ChronoUnit.DAYS, ChronoUnit.FOREVER, 40587),
        RATA_DIE("RataDie", ChronoUnit.DAYS, ChronoUnit.FOREVER, 719163);
        
        private final TemporalUnit baseUnit;
        private final String name;
        private final long offset;
        private final ValueRange range;
        private final TemporalUnit rangeUnit;

        private Field(String name2, TemporalUnit baseUnit2, TemporalUnit rangeUnit2, long offset2) {
            this.name = name2;
            this.baseUnit = baseUnit2;
            this.rangeUnit = rangeUnit2;
            this.range = ValueRange.of(-365243219162L + offset2, 365241780471L + offset2);
            this.offset = offset2;
        }

        public TemporalUnit getBaseUnit() {
            return this.baseUnit;
        }

        public TemporalUnit getRangeUnit() {
            return this.rangeUnit;
        }

        public ValueRange range() {
            return this.range;
        }

        public boolean isDateBased() {
            return true;
        }

        public boolean isTimeBased() {
            return false;
        }

        public boolean isSupportedBy(TemporalAccessor temporal) {
            return temporal.isSupported(ChronoField.EPOCH_DAY);
        }

        public ValueRange rangeRefinedBy(TemporalAccessor temporal) {
            if (isSupportedBy(temporal)) {
                return range();
            }
            throw new UnsupportedTemporalTypeException("Unsupported field: " + this);
        }

        public long getFrom(TemporalAccessor temporal) {
            return temporal.getLong(ChronoField.EPOCH_DAY) + this.offset;
        }

        public <R extends Temporal> R adjustInto(R dateTime, long newValue) {
            if (range().isValidValue(newValue)) {
                return dateTime.with(ChronoField.EPOCH_DAY, Jdk8Methods.safeSubtract(newValue, this.offset));
            }
            throw new DateTimeException("Invalid value: " + this.name + " " + newValue);
        }

        public String getDisplayName(Locale locale) {
            Jdk8Methods.requireNonNull(locale, "locale");
            return toString();
        }

        public TemporalAccessor resolve(Map<TemporalField, Long> fieldValues, TemporalAccessor partialTemporal, ResolverStyle resolverStyle) {
            return Chronology.from(partialTemporal).dateEpochDay(Jdk8Methods.safeSubtract(fieldValues.remove(this).longValue(), this.offset));
        }

        public String toString() {
            return this.name;
        }
    }
}
