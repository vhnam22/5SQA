package org.threeten.bp.format;

import java.util.ArrayList;
import java.util.HashMap;
import java.util.List;
import java.util.Locale;
import java.util.Map;
import org.threeten.bp.Period;
import org.threeten.bp.ZoneId;
import org.threeten.bp.chrono.Chronology;
import org.threeten.bp.chrono.IsoChronology;
import org.threeten.bp.format.DateTimeFormatterBuilder;
import org.threeten.bp.jdk8.DefaultInterfaceTemporalAccessor;
import org.threeten.bp.jdk8.Jdk8Methods;
import org.threeten.bp.temporal.TemporalField;
import org.threeten.bp.temporal.TemporalQueries;
import org.threeten.bp.temporal.TemporalQuery;
import org.threeten.bp.temporal.UnsupportedTemporalTypeException;

final class DateTimeParseContext {
    private boolean caseSensitive = true;
    private Locale locale;
    private Chronology overrideChronology;
    /* access modifiers changed from: private */
    public ZoneId overrideZone;
    private final ArrayList<Parsed> parsed;
    private boolean strict = true;
    private DecimalStyle symbols;

    DateTimeParseContext(DateTimeFormatter formatter) {
        ArrayList<Parsed> arrayList = new ArrayList<>();
        this.parsed = arrayList;
        this.locale = formatter.getLocale();
        this.symbols = formatter.getDecimalStyle();
        this.overrideChronology = formatter.getChronology();
        this.overrideZone = formatter.getZone();
        arrayList.add(new Parsed());
    }

    DateTimeParseContext(Locale locale2, DecimalStyle symbols2, Chronology chronology) {
        ArrayList<Parsed> arrayList = new ArrayList<>();
        this.parsed = arrayList;
        this.locale = locale2;
        this.symbols = symbols2;
        this.overrideChronology = chronology;
        this.overrideZone = null;
        arrayList.add(new Parsed());
    }

    DateTimeParseContext(DateTimeParseContext other) {
        ArrayList<Parsed> arrayList = new ArrayList<>();
        this.parsed = arrayList;
        this.locale = other.locale;
        this.symbols = other.symbols;
        this.overrideChronology = other.overrideChronology;
        this.overrideZone = other.overrideZone;
        this.caseSensitive = other.caseSensitive;
        this.strict = other.strict;
        arrayList.add(new Parsed());
    }

    /* access modifiers changed from: package-private */
    public DateTimeParseContext copy() {
        return new DateTimeParseContext(this);
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
    public Chronology getEffectiveChronology() {
        Chronology chrono = currentParsed().chrono;
        if (chrono != null) {
            return chrono;
        }
        Chronology chrono2 = this.overrideChronology;
        if (chrono2 == null) {
            return IsoChronology.INSTANCE;
        }
        return chrono2;
    }

    /* access modifiers changed from: package-private */
    public boolean isCaseSensitive() {
        return this.caseSensitive;
    }

    /* access modifiers changed from: package-private */
    public void setCaseSensitive(boolean caseSensitive2) {
        this.caseSensitive = caseSensitive2;
    }

    /* access modifiers changed from: package-private */
    public boolean subSequenceEquals(CharSequence cs1, int offset1, CharSequence cs2, int offset2, int length) {
        if (offset1 + length > cs1.length() || offset2 + length > cs2.length()) {
            return false;
        }
        if (isCaseSensitive()) {
            for (int i = 0; i < length; i++) {
                if (cs1.charAt(offset1 + i) != cs2.charAt(offset2 + i)) {
                    return false;
                }
            }
            return true;
        }
        for (int i2 = 0; i2 < length; i2++) {
            char ch1 = cs1.charAt(offset1 + i2);
            char ch2 = cs2.charAt(offset2 + i2);
            if (ch1 != ch2 && Character.toUpperCase(ch1) != Character.toUpperCase(ch2) && Character.toLowerCase(ch1) != Character.toLowerCase(ch2)) {
                return false;
            }
        }
        return true;
    }

    /* access modifiers changed from: package-private */
    public boolean charEquals(char ch1, char ch2) {
        if (isCaseSensitive()) {
            return ch1 == ch2;
        }
        return charEqualsIgnoreCase(ch1, ch2);
    }

    static boolean charEqualsIgnoreCase(char c1, char c2) {
        return c1 == c2 || Character.toUpperCase(c1) == Character.toUpperCase(c2) || Character.toLowerCase(c1) == Character.toLowerCase(c2);
    }

    /* access modifiers changed from: package-private */
    public boolean isStrict() {
        return this.strict;
    }

    /* access modifiers changed from: package-private */
    public void setStrict(boolean strict2) {
        this.strict = strict2;
    }

    /* access modifiers changed from: package-private */
    public void startOptional() {
        this.parsed.add(currentParsed().copy());
    }

    /* access modifiers changed from: package-private */
    public void endOptional(boolean successful) {
        if (successful) {
            ArrayList<Parsed> arrayList = this.parsed;
            arrayList.remove(arrayList.size() - 2);
            return;
        }
        ArrayList<Parsed> arrayList2 = this.parsed;
        arrayList2.remove(arrayList2.size() - 1);
    }

    private Parsed currentParsed() {
        ArrayList<Parsed> arrayList = this.parsed;
        return arrayList.get(arrayList.size() - 1);
    }

    /* access modifiers changed from: package-private */
    public Long getParsed(TemporalField field) {
        return currentParsed().fieldValues.get(field);
    }

    /* access modifiers changed from: package-private */
    public int setParsedField(TemporalField field, long value, int errorPos, int successPos) {
        Jdk8Methods.requireNonNull(field, "field");
        Long old = currentParsed().fieldValues.put(field, Long.valueOf(value));
        return (old == null || old.longValue() == value) ? successPos : ~errorPos;
    }

    /* access modifiers changed from: package-private */
    public void setParsed(Chronology chrono) {
        Jdk8Methods.requireNonNull(chrono, "chrono");
        Parsed currentParsed = currentParsed();
        currentParsed.chrono = chrono;
        if (currentParsed.callbacks != null) {
            List<Object[]> callbacks = new ArrayList<>(currentParsed.callbacks);
            currentParsed.callbacks.clear();
            for (Object[] objects : callbacks) {
                ((DateTimeFormatterBuilder.ReducedPrinterParser) objects[0]).setValue(this, ((Long) objects[1]).longValue(), ((Integer) objects[2]).intValue(), ((Integer) objects[3]).intValue());
            }
        }
    }

    /* access modifiers changed from: package-private */
    public void addChronologyChangedParser(DateTimeFormatterBuilder.ReducedPrinterParser reducedPrinterParser, long value, int errorPos, int successPos) {
        Parsed currentParsed = currentParsed();
        if (currentParsed.callbacks == null) {
            currentParsed.callbacks = new ArrayList(2);
        }
        currentParsed.callbacks.add(new Object[]{reducedPrinterParser, Long.valueOf(value), Integer.valueOf(errorPos), Integer.valueOf(successPos)});
    }

    /* access modifiers changed from: package-private */
    public void setParsed(ZoneId zone) {
        Jdk8Methods.requireNonNull(zone, "zone");
        currentParsed().zone = zone;
    }

    /* access modifiers changed from: package-private */
    public void setParsedLeapSecond() {
        currentParsed().leapSecond = true;
    }

    /* access modifiers changed from: package-private */
    public Parsed toParsed() {
        return currentParsed();
    }

    public String toString() {
        return currentParsed().toString();
    }

    final class Parsed extends DefaultInterfaceTemporalAccessor {
        List<Object[]> callbacks;
        Chronology chrono;
        Period excessDays;
        final Map<TemporalField, Long> fieldValues;
        boolean leapSecond;
        ZoneId zone;

        private Parsed() {
            this.chrono = null;
            this.zone = null;
            this.fieldValues = new HashMap();
            this.excessDays = Period.ZERO;
        }

        /* access modifiers changed from: protected */
        public Parsed copy() {
            Parsed cloned = new Parsed();
            cloned.chrono = this.chrono;
            cloned.zone = this.zone;
            cloned.fieldValues.putAll(this.fieldValues);
            cloned.leapSecond = this.leapSecond;
            return cloned;
        }

        public String toString() {
            return this.fieldValues.toString() + "," + this.chrono + "," + this.zone;
        }

        public boolean isSupported(TemporalField field) {
            return this.fieldValues.containsKey(field);
        }

        public int get(TemporalField field) {
            if (this.fieldValues.containsKey(field)) {
                return Jdk8Methods.safeToInt(this.fieldValues.get(field).longValue());
            }
            throw new UnsupportedTemporalTypeException("Unsupported field: " + field);
        }

        public long getLong(TemporalField field) {
            if (this.fieldValues.containsKey(field)) {
                return this.fieldValues.get(field).longValue();
            }
            throw new UnsupportedTemporalTypeException("Unsupported field: " + field);
        }

        public <R> R query(TemporalQuery<R> query) {
            if (query == TemporalQueries.chronology()) {
                return this.chrono;
            }
            if (query == TemporalQueries.zoneId() || query == TemporalQueries.zone()) {
                return this.zone;
            }
            return super.query(query);
        }

        /* access modifiers changed from: package-private */
        public DateTimeBuilder toBuilder() {
            DateTimeBuilder builder = new DateTimeBuilder();
            builder.fieldValues.putAll(this.fieldValues);
            builder.chrono = DateTimeParseContext.this.getEffectiveChronology();
            ZoneId zoneId = this.zone;
            if (zoneId != null) {
                builder.zone = zoneId;
            } else {
                builder.zone = DateTimeParseContext.this.overrideZone;
            }
            builder.leapSecond = this.leapSecond;
            builder.excessDays = this.excessDays;
            return builder;
        }
    }

    /* access modifiers changed from: package-private */
    public void setLocale(Locale locale2) {
        Jdk8Methods.requireNonNull(locale2, "locale");
        this.locale = locale2;
    }
}
