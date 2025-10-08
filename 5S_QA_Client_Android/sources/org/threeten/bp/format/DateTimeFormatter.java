package org.threeten.bp.format;

import java.io.IOException;
import java.text.FieldPosition;
import java.text.Format;
import java.text.ParseException;
import java.text.ParsePosition;
import java.util.Arrays;
import java.util.Collections;
import java.util.HashMap;
import java.util.HashSet;
import java.util.Locale;
import java.util.Map;
import java.util.Set;
import org.threeten.bp.DateTimeException;
import org.threeten.bp.Period;
import org.threeten.bp.ZoneId;
import org.threeten.bp.chrono.Chronology;
import org.threeten.bp.chrono.IsoChronology;
import org.threeten.bp.format.DateTimeFormatterBuilder;
import org.threeten.bp.format.DateTimeParseContext;
import org.threeten.bp.jdk8.Jdk8Methods;
import org.threeten.bp.temporal.ChronoField;
import org.threeten.bp.temporal.IsoFields;
import org.threeten.bp.temporal.TemporalAccessor;
import org.threeten.bp.temporal.TemporalField;
import org.threeten.bp.temporal.TemporalQuery;

public final class DateTimeFormatter {
    public static final DateTimeFormatter BASIC_ISO_DATE = new DateTimeFormatterBuilder().parseCaseInsensitive().appendValue(ChronoField.YEAR, 4).appendValue(ChronoField.MONTH_OF_YEAR, 2).appendValue(ChronoField.DAY_OF_MONTH, 2).optionalStart().appendOffset("+HHMMss", "Z").toFormatter(ResolverStyle.STRICT).withChronology(IsoChronology.INSTANCE);
    public static final DateTimeFormatter ISO_DATE;
    public static final DateTimeFormatter ISO_DATE_TIME;
    public static final DateTimeFormatter ISO_INSTANT = new DateTimeFormatterBuilder().parseCaseInsensitive().appendInstant().toFormatter(ResolverStyle.STRICT);
    public static final DateTimeFormatter ISO_LOCAL_DATE;
    public static final DateTimeFormatter ISO_LOCAL_DATE_TIME;
    public static final DateTimeFormatter ISO_LOCAL_TIME;
    public static final DateTimeFormatter ISO_OFFSET_DATE;
    public static final DateTimeFormatter ISO_OFFSET_DATE_TIME;
    public static final DateTimeFormatter ISO_OFFSET_TIME;
    public static final DateTimeFormatter ISO_ORDINAL_DATE = new DateTimeFormatterBuilder().parseCaseInsensitive().appendValue(ChronoField.YEAR, 4, 10, SignStyle.EXCEEDS_PAD).appendLiteral('-').appendValue(ChronoField.DAY_OF_YEAR, 3).optionalStart().appendOffsetId().toFormatter(ResolverStyle.STRICT).withChronology(IsoChronology.INSTANCE);
    public static final DateTimeFormatter ISO_TIME;
    public static final DateTimeFormatter ISO_WEEK_DATE = new DateTimeFormatterBuilder().parseCaseInsensitive().appendValue(IsoFields.WEEK_BASED_YEAR, 4, 10, SignStyle.EXCEEDS_PAD).appendLiteral("-W").appendValue(IsoFields.WEEK_OF_WEEK_BASED_YEAR, 2).appendLiteral('-').appendValue(ChronoField.DAY_OF_WEEK, 1).optionalStart().appendOffsetId().toFormatter(ResolverStyle.STRICT).withChronology(IsoChronology.INSTANCE);
    public static final DateTimeFormatter ISO_ZONED_DATE_TIME;
    private static final TemporalQuery<Period> PARSED_EXCESS_DAYS = new TemporalQuery<Period>() {
        public Period queryFrom(TemporalAccessor temporal) {
            if (temporal instanceof DateTimeBuilder) {
                return ((DateTimeBuilder) temporal).excessDays;
            }
            return Period.ZERO;
        }
    };
    private static final TemporalQuery<Boolean> PARSED_LEAP_SECOND = new TemporalQuery<Boolean>() {
        public Boolean queryFrom(TemporalAccessor temporal) {
            if (temporal instanceof DateTimeBuilder) {
                return Boolean.valueOf(((DateTimeBuilder) temporal).leapSecond);
            }
            return Boolean.FALSE;
        }
    };
    public static final DateTimeFormatter RFC_1123_DATE_TIME;
    private final Chronology chrono;
    private final DecimalStyle decimalStyle;
    private final Locale locale;
    private final DateTimeFormatterBuilder.CompositePrinterParser printerParser;
    private final Set<TemporalField> resolverFields;
    private final ResolverStyle resolverStyle;
    private final ZoneId zone;

    static {
        DateTimeFormatter withChronology = new DateTimeFormatterBuilder().appendValue(ChronoField.YEAR, 4, 10, SignStyle.EXCEEDS_PAD).appendLiteral('-').appendValue(ChronoField.MONTH_OF_YEAR, 2).appendLiteral('-').appendValue(ChronoField.DAY_OF_MONTH, 2).toFormatter(ResolverStyle.STRICT).withChronology(IsoChronology.INSTANCE);
        ISO_LOCAL_DATE = withChronology;
        ISO_OFFSET_DATE = new DateTimeFormatterBuilder().parseCaseInsensitive().append(withChronology).appendOffsetId().toFormatter(ResolverStyle.STRICT).withChronology(IsoChronology.INSTANCE);
        ISO_DATE = new DateTimeFormatterBuilder().parseCaseInsensitive().append(withChronology).optionalStart().appendOffsetId().toFormatter(ResolverStyle.STRICT).withChronology(IsoChronology.INSTANCE);
        DateTimeFormatter formatter = new DateTimeFormatterBuilder().appendValue(ChronoField.HOUR_OF_DAY, 2).appendLiteral(':').appendValue(ChronoField.MINUTE_OF_HOUR, 2).optionalStart().appendLiteral(':').appendValue(ChronoField.SECOND_OF_MINUTE, 2).optionalStart().appendFraction(ChronoField.NANO_OF_SECOND, 0, 9, true).toFormatter(ResolverStyle.STRICT);
        ISO_LOCAL_TIME = formatter;
        ISO_OFFSET_TIME = new DateTimeFormatterBuilder().parseCaseInsensitive().append(formatter).appendOffsetId().toFormatter(ResolverStyle.STRICT);
        ISO_TIME = new DateTimeFormatterBuilder().parseCaseInsensitive().append(formatter).optionalStart().appendOffsetId().toFormatter(ResolverStyle.STRICT);
        DateTimeFormatter withChronology2 = new DateTimeFormatterBuilder().parseCaseInsensitive().append(withChronology).appendLiteral('T').append(formatter).toFormatter(ResolverStyle.STRICT).withChronology(IsoChronology.INSTANCE);
        ISO_LOCAL_DATE_TIME = withChronology2;
        DateTimeFormatter withChronology3 = new DateTimeFormatterBuilder().parseCaseInsensitive().append(withChronology2).appendOffsetId().toFormatter(ResolverStyle.STRICT).withChronology(IsoChronology.INSTANCE);
        ISO_OFFSET_DATE_TIME = withChronology3;
        ISO_ZONED_DATE_TIME = new DateTimeFormatterBuilder().append(withChronology3).optionalStart().appendLiteral('[').parseCaseSensitive().appendZoneRegionId().appendLiteral(']').toFormatter(ResolverStyle.STRICT).withChronology(IsoChronology.INSTANCE);
        ISO_DATE_TIME = new DateTimeFormatterBuilder().append(withChronology2).optionalStart().appendOffsetId().optionalStart().appendLiteral('[').parseCaseSensitive().appendZoneRegionId().appendLiteral(']').toFormatter(ResolverStyle.STRICT).withChronology(IsoChronology.INSTANCE);
        Map<Long, String> dow = new HashMap<>();
        dow.put(1L, "Mon");
        dow.put(2L, "Tue");
        dow.put(3L, "Wed");
        dow.put(4L, "Thu");
        dow.put(5L, "Fri");
        dow.put(6L, "Sat");
        dow.put(7L, "Sun");
        Map<Long, String> moy = new HashMap<>();
        moy.put(1L, "Jan");
        moy.put(2L, "Feb");
        moy.put(3L, "Mar");
        moy.put(4L, "Apr");
        moy.put(5L, "May");
        moy.put(6L, "Jun");
        moy.put(7L, "Jul");
        moy.put(8L, "Aug");
        moy.put(9L, "Sep");
        moy.put(10L, "Oct");
        moy.put(11L, "Nov");
        moy.put(12L, "Dec");
        RFC_1123_DATE_TIME = new DateTimeFormatterBuilder().parseCaseInsensitive().parseLenient().optionalStart().appendText((TemporalField) ChronoField.DAY_OF_WEEK, dow).appendLiteral(", ").optionalEnd().appendValue(ChronoField.DAY_OF_MONTH, 1, 2, SignStyle.NOT_NEGATIVE).appendLiteral(' ').appendText((TemporalField) ChronoField.MONTH_OF_YEAR, moy).appendLiteral(' ').appendValue(ChronoField.YEAR, 4).appendLiteral(' ').appendValue(ChronoField.HOUR_OF_DAY, 2).appendLiteral(':').appendValue(ChronoField.MINUTE_OF_HOUR, 2).optionalStart().appendLiteral(':').appendValue(ChronoField.SECOND_OF_MINUTE, 2).optionalEnd().appendLiteral(' ').appendOffset("+HHMM", "GMT").toFormatter(ResolverStyle.SMART).withChronology(IsoChronology.INSTANCE);
    }

    public static DateTimeFormatter ofPattern(String pattern) {
        return new DateTimeFormatterBuilder().appendPattern(pattern).toFormatter();
    }

    public static DateTimeFormatter ofPattern(String pattern, Locale locale2) {
        return new DateTimeFormatterBuilder().appendPattern(pattern).toFormatter(locale2);
    }

    public static DateTimeFormatter ofLocalizedDate(FormatStyle dateStyle) {
        Jdk8Methods.requireNonNull(dateStyle, "dateStyle");
        return new DateTimeFormatterBuilder().appendLocalized(dateStyle, (FormatStyle) null).toFormatter().withChronology(IsoChronology.INSTANCE);
    }

    public static DateTimeFormatter ofLocalizedTime(FormatStyle timeStyle) {
        Jdk8Methods.requireNonNull(timeStyle, "timeStyle");
        return new DateTimeFormatterBuilder().appendLocalized((FormatStyle) null, timeStyle).toFormatter().withChronology(IsoChronology.INSTANCE);
    }

    public static DateTimeFormatter ofLocalizedDateTime(FormatStyle dateTimeStyle) {
        Jdk8Methods.requireNonNull(dateTimeStyle, "dateTimeStyle");
        return new DateTimeFormatterBuilder().appendLocalized(dateTimeStyle, dateTimeStyle).toFormatter().withChronology(IsoChronology.INSTANCE);
    }

    public static DateTimeFormatter ofLocalizedDateTime(FormatStyle dateStyle, FormatStyle timeStyle) {
        Jdk8Methods.requireNonNull(dateStyle, "dateStyle");
        Jdk8Methods.requireNonNull(timeStyle, "timeStyle");
        return new DateTimeFormatterBuilder().appendLocalized(dateStyle, timeStyle).toFormatter().withChronology(IsoChronology.INSTANCE);
    }

    public static final TemporalQuery<Period> parsedExcessDays() {
        return PARSED_EXCESS_DAYS;
    }

    public static final TemporalQuery<Boolean> parsedLeapSecond() {
        return PARSED_LEAP_SECOND;
    }

    DateTimeFormatter(DateTimeFormatterBuilder.CompositePrinterParser printerParser2, Locale locale2, DecimalStyle decimalStyle2, ResolverStyle resolverStyle2, Set<TemporalField> resolverFields2, Chronology chrono2, ZoneId zone2) {
        this.printerParser = (DateTimeFormatterBuilder.CompositePrinterParser) Jdk8Methods.requireNonNull(printerParser2, "printerParser");
        this.locale = (Locale) Jdk8Methods.requireNonNull(locale2, "locale");
        this.decimalStyle = (DecimalStyle) Jdk8Methods.requireNonNull(decimalStyle2, "decimalStyle");
        this.resolverStyle = (ResolverStyle) Jdk8Methods.requireNonNull(resolverStyle2, "resolverStyle");
        this.resolverFields = resolverFields2;
        this.chrono = chrono2;
        this.zone = zone2;
    }

    public Locale getLocale() {
        return this.locale;
    }

    public DateTimeFormatter withLocale(Locale locale2) {
        if (this.locale.equals(locale2)) {
            return this;
        }
        return new DateTimeFormatter(this.printerParser, locale2, this.decimalStyle, this.resolverStyle, this.resolverFields, this.chrono, this.zone);
    }

    public DecimalStyle getDecimalStyle() {
        return this.decimalStyle;
    }

    public DateTimeFormatter withDecimalStyle(DecimalStyle decimalStyle2) {
        if (this.decimalStyle.equals(decimalStyle2)) {
            return this;
        }
        return new DateTimeFormatter(this.printerParser, this.locale, decimalStyle2, this.resolverStyle, this.resolverFields, this.chrono, this.zone);
    }

    public Chronology getChronology() {
        return this.chrono;
    }

    public DateTimeFormatter withChronology(Chronology chrono2) {
        if (Jdk8Methods.equals(this.chrono, chrono2)) {
            return this;
        }
        return new DateTimeFormatter(this.printerParser, this.locale, this.decimalStyle, this.resolverStyle, this.resolverFields, chrono2, this.zone);
    }

    public ZoneId getZone() {
        return this.zone;
    }

    public DateTimeFormatter withZone(ZoneId zone2) {
        if (Jdk8Methods.equals(this.zone, zone2)) {
            return this;
        }
        return new DateTimeFormatter(this.printerParser, this.locale, this.decimalStyle, this.resolverStyle, this.resolverFields, this.chrono, zone2);
    }

    public ResolverStyle getResolverStyle() {
        return this.resolverStyle;
    }

    public DateTimeFormatter withResolverStyle(ResolverStyle resolverStyle2) {
        Jdk8Methods.requireNonNull(resolverStyle2, "resolverStyle");
        if (Jdk8Methods.equals(this.resolverStyle, resolverStyle2)) {
            return this;
        }
        return new DateTimeFormatter(this.printerParser, this.locale, this.decimalStyle, resolverStyle2, this.resolverFields, this.chrono, this.zone);
    }

    public Set<TemporalField> getResolverFields() {
        return this.resolverFields;
    }

    public DateTimeFormatter withResolverFields(TemporalField... resolverFields2) {
        if (resolverFields2 == null) {
            return new DateTimeFormatter(this.printerParser, this.locale, this.decimalStyle, this.resolverStyle, (Set<TemporalField>) null, this.chrono, this.zone);
        }
        Set<TemporalField> fields = new HashSet<>(Arrays.asList(resolverFields2));
        if (Jdk8Methods.equals(this.resolverFields, fields)) {
            return this;
        }
        return new DateTimeFormatter(this.printerParser, this.locale, this.decimalStyle, this.resolverStyle, Collections.unmodifiableSet(fields), this.chrono, this.zone);
    }

    public DateTimeFormatter withResolverFields(Set<TemporalField> resolverFields2) {
        if (resolverFields2 == null) {
            return new DateTimeFormatter(this.printerParser, this.locale, this.decimalStyle, this.resolverStyle, (Set<TemporalField>) null, this.chrono, this.zone);
        }
        if (Jdk8Methods.equals(this.resolverFields, resolverFields2)) {
            return this;
        }
        return new DateTimeFormatter(this.printerParser, this.locale, this.decimalStyle, this.resolverStyle, Collections.unmodifiableSet(new HashSet(resolverFields2)), this.chrono, this.zone);
    }

    public String format(TemporalAccessor temporal) {
        StringBuilder buf = new StringBuilder(32);
        formatTo(temporal, buf);
        return buf.toString();
    }

    public void formatTo(TemporalAccessor temporal, Appendable appendable) {
        Jdk8Methods.requireNonNull(temporal, "temporal");
        Jdk8Methods.requireNonNull(appendable, "appendable");
        try {
            DateTimePrintContext context = new DateTimePrintContext(temporal, this);
            if (appendable instanceof StringBuilder) {
                this.printerParser.print(context, (StringBuilder) appendable);
                return;
            }
            StringBuilder buf = new StringBuilder(32);
            this.printerParser.print(context, buf);
            appendable.append(buf);
        } catch (IOException ex) {
            throw new DateTimeException(ex.getMessage(), ex);
        }
    }

    public TemporalAccessor parse(CharSequence text) {
        Jdk8Methods.requireNonNull(text, "text");
        try {
            return parseToBuilder(text, (ParsePosition) null).resolve(this.resolverStyle, this.resolverFields);
        } catch (DateTimeParseException ex) {
            throw ex;
        } catch (RuntimeException ex2) {
            throw createError(text, ex2);
        }
    }

    public TemporalAccessor parse(CharSequence text, ParsePosition position) {
        Jdk8Methods.requireNonNull(text, "text");
        Jdk8Methods.requireNonNull(position, "position");
        try {
            return parseToBuilder(text, position).resolve(this.resolverStyle, this.resolverFields);
        } catch (DateTimeParseException ex) {
            throw ex;
        } catch (IndexOutOfBoundsException ex2) {
            throw ex2;
        } catch (RuntimeException ex3) {
            throw createError(text, ex3);
        }
    }

    public <T> T parse(CharSequence text, TemporalQuery<T> type) {
        Jdk8Methods.requireNonNull(text, "text");
        Jdk8Methods.requireNonNull(type, "type");
        try {
            return parseToBuilder(text, (ParsePosition) null).resolve(this.resolverStyle, this.resolverFields).build(type);
        } catch (DateTimeParseException ex) {
            throw ex;
        } catch (RuntimeException ex2) {
            throw createError(text, ex2);
        }
    }

    /* JADX WARNING: Code restructure failed: missing block: B:15:0x004c, code lost:
        r0 = move-exception;
     */
    /* JADX WARNING: Code restructure failed: missing block: B:17:0x0051, code lost:
        throw createError(r7, r0);
     */
    /* JADX WARNING: Code restructure failed: missing block: B:18:0x0052, code lost:
        r0 = move-exception;
     */
    /* JADX WARNING: Code restructure failed: missing block: B:19:0x0053, code lost:
        throw r0;
     */
    /* JADX WARNING: Failed to process nested try/catch */
    /* JADX WARNING: Removed duplicated region for block: B:18:0x0052 A[ExcHandler: DateTimeParseException (r0v5 'ex' org.threeten.bp.format.DateTimeParseException A[CUSTOM_DECLARE]), Splitter:B:3:0x0011] */
    /* Code decompiled incorrectly, please refer to instructions dump. */
    public org.threeten.bp.temporal.TemporalAccessor parseBest(java.lang.CharSequence r7, org.threeten.bp.temporal.TemporalQuery<?>... r8) {
        /*
            r6 = this;
            java.lang.String r0 = "text"
            org.threeten.bp.jdk8.Jdk8Methods.requireNonNull(r7, r0)
            java.lang.String r0 = "types"
            org.threeten.bp.jdk8.Jdk8Methods.requireNonNull(r8, r0)
            int r0 = r8.length
            r1 = 2
            if (r0 < r1) goto L_0x0054
            r0 = 0
            org.threeten.bp.format.DateTimeBuilder r0 = r6.parseToBuilder(r7, r0)     // Catch:{ DateTimeParseException -> 0x0052, RuntimeException -> 0x004c }
            org.threeten.bp.format.ResolverStyle r1 = r6.resolverStyle     // Catch:{ DateTimeParseException -> 0x0052, RuntimeException -> 0x004c }
            java.util.Set<org.threeten.bp.temporal.TemporalField> r2 = r6.resolverFields     // Catch:{ DateTimeParseException -> 0x0052, RuntimeException -> 0x004c }
            org.threeten.bp.format.DateTimeBuilder r0 = r0.resolve(r1, r2)     // Catch:{ DateTimeParseException -> 0x0052, RuntimeException -> 0x004c }
            r1 = r8
            int r2 = r1.length     // Catch:{ DateTimeParseException -> 0x0052, RuntimeException -> 0x004c }
            r3 = 0
        L_0x0020:
            if (r3 >= r2) goto L_0x002f
            r4 = r1[r3]     // Catch:{ DateTimeParseException -> 0x0052, RuntimeException -> 0x004c }
            java.lang.Object r5 = r0.build(r4)     // Catch:{ RuntimeException -> 0x002b, DateTimeParseException -> 0x0052 }
            org.threeten.bp.temporal.TemporalAccessor r5 = (org.threeten.bp.temporal.TemporalAccessor) r5     // Catch:{ RuntimeException -> 0x002b, DateTimeParseException -> 0x0052 }
            return r5
        L_0x002b:
            r5 = move-exception
            int r3 = r3 + 1
            goto L_0x0020
        L_0x002f:
            org.threeten.bp.DateTimeException r1 = new org.threeten.bp.DateTimeException     // Catch:{ DateTimeParseException -> 0x0052, RuntimeException -> 0x004c }
            java.lang.StringBuilder r2 = new java.lang.StringBuilder     // Catch:{ DateTimeParseException -> 0x0052, RuntimeException -> 0x004c }
            r2.<init>()     // Catch:{ DateTimeParseException -> 0x0052, RuntimeException -> 0x004c }
            java.lang.String r3 = "Unable to convert parsed text to any specified type: "
            java.lang.StringBuilder r2 = r2.append(r3)     // Catch:{ DateTimeParseException -> 0x0052, RuntimeException -> 0x004c }
            java.lang.String r3 = java.util.Arrays.toString(r8)     // Catch:{ DateTimeParseException -> 0x0052, RuntimeException -> 0x004c }
            java.lang.StringBuilder r2 = r2.append(r3)     // Catch:{ DateTimeParseException -> 0x0052, RuntimeException -> 0x004c }
            java.lang.String r2 = r2.toString()     // Catch:{ DateTimeParseException -> 0x0052, RuntimeException -> 0x004c }
            r1.<init>(r2)     // Catch:{ DateTimeParseException -> 0x0052, RuntimeException -> 0x004c }
            throw r1     // Catch:{ DateTimeParseException -> 0x0052, RuntimeException -> 0x004c }
        L_0x004c:
            r0 = move-exception
            org.threeten.bp.format.DateTimeParseException r1 = r6.createError(r7, r0)
            throw r1
        L_0x0052:
            r0 = move-exception
            throw r0
        L_0x0054:
            java.lang.IllegalArgumentException r0 = new java.lang.IllegalArgumentException
            java.lang.String r1 = "At least two types must be specified"
            r0.<init>(r1)
            throw r0
        */
        throw new UnsupportedOperationException("Method not decompiled: org.threeten.bp.format.DateTimeFormatter.parseBest(java.lang.CharSequence, org.threeten.bp.temporal.TemporalQuery[]):org.threeten.bp.temporal.TemporalAccessor");
    }

    private DateTimeParseException createError(CharSequence text, RuntimeException ex) {
        String abbr;
        if (text.length() > 64) {
            abbr = text.subSequence(0, 64).toString() + "...";
        } else {
            abbr = text.toString();
        }
        return new DateTimeParseException("Text '" + abbr + "' could not be parsed: " + ex.getMessage(), text, 0, ex);
    }

    /* access modifiers changed from: private */
    public DateTimeBuilder parseToBuilder(CharSequence text, ParsePosition position) {
        String abbr;
        ParsePosition pos = position != null ? position : new ParsePosition(0);
        DateTimeParseContext.Parsed result = parseUnresolved0(text, pos);
        if (result != null && pos.getErrorIndex() < 0 && (position != null || pos.getIndex() >= text.length())) {
            return result.toBuilder();
        }
        if (text.length() > 64) {
            abbr = text.subSequence(0, 64).toString() + "...";
        } else {
            abbr = text.toString();
        }
        if (pos.getErrorIndex() >= 0) {
            throw new DateTimeParseException("Text '" + abbr + "' could not be parsed at index " + pos.getErrorIndex(), text, pos.getErrorIndex());
        }
        throw new DateTimeParseException("Text '" + abbr + "' could not be parsed, unparsed text found at index " + pos.getIndex(), text, pos.getIndex());
    }

    public TemporalAccessor parseUnresolved(CharSequence text, ParsePosition position) {
        return parseUnresolved0(text, position);
    }

    /* access modifiers changed from: private */
    public DateTimeParseContext.Parsed parseUnresolved0(CharSequence text, ParsePosition position) {
        Jdk8Methods.requireNonNull(text, "text");
        Jdk8Methods.requireNonNull(position, "position");
        DateTimeParseContext context = new DateTimeParseContext(this);
        int pos = this.printerParser.parse(context, text, position.getIndex());
        if (pos < 0) {
            position.setErrorIndex(~pos);
            return null;
        }
        position.setIndex(pos);
        return context.toParsed();
    }

    /* access modifiers changed from: package-private */
    public DateTimeFormatterBuilder.CompositePrinterParser toPrinterParser(boolean optional) {
        return this.printerParser.withOptional(optional);
    }

    public Format toFormat() {
        return new ClassicFormat(this, (TemporalQuery<?>) null);
    }

    public Format toFormat(TemporalQuery<?> query) {
        Jdk8Methods.requireNonNull(query, "query");
        return new ClassicFormat(this, query);
    }

    public String toString() {
        String pattern = this.printerParser.toString();
        return pattern.startsWith("[") ? pattern : pattern.substring(1, pattern.length() - 1);
    }

    static class ClassicFormat extends Format {
        private final DateTimeFormatter formatter;
        private final TemporalQuery<?> query;

        public ClassicFormat(DateTimeFormatter formatter2, TemporalQuery<?> query2) {
            this.formatter = formatter2;
            this.query = query2;
        }

        public StringBuffer format(Object obj, StringBuffer toAppendTo, FieldPosition pos) {
            Jdk8Methods.requireNonNull(obj, "obj");
            Jdk8Methods.requireNonNull(toAppendTo, "toAppendTo");
            Jdk8Methods.requireNonNull(pos, "pos");
            if (obj instanceof TemporalAccessor) {
                pos.setBeginIndex(0);
                pos.setEndIndex(0);
                try {
                    this.formatter.formatTo((TemporalAccessor) obj, toAppendTo);
                    return toAppendTo;
                } catch (RuntimeException ex) {
                    throw new IllegalArgumentException(ex.getMessage(), ex);
                }
            } else {
                throw new IllegalArgumentException("Format target must implement TemporalAccessor");
            }
        }

        public Object parseObject(String text) throws ParseException {
            Jdk8Methods.requireNonNull(text, "text");
            try {
                TemporalQuery<?> temporalQuery = this.query;
                if (temporalQuery == null) {
                    return this.formatter.parseToBuilder(text, (ParsePosition) null).resolve(this.formatter.getResolverStyle(), this.formatter.getResolverFields());
                }
                return this.formatter.parse((CharSequence) text, temporalQuery);
            } catch (DateTimeParseException ex) {
                throw new ParseException(ex.getMessage(), ex.getErrorIndex());
            } catch (RuntimeException ex2) {
                throw ((ParseException) new ParseException(ex2.getMessage(), 0).initCause(ex2));
            }
        }

        public Object parseObject(String text, ParsePosition pos) {
            Jdk8Methods.requireNonNull(text, "text");
            try {
                DateTimeParseContext.Parsed unresolved = this.formatter.parseUnresolved0(text, pos);
                if (unresolved == null) {
                    if (pos.getErrorIndex() < 0) {
                        pos.setErrorIndex(0);
                    }
                    return null;
                }
                try {
                    DateTimeBuilder builder = unresolved.toBuilder().resolve(this.formatter.getResolverStyle(), this.formatter.getResolverFields());
                    TemporalQuery<?> temporalQuery = this.query;
                    if (temporalQuery == null) {
                        return builder;
                    }
                    return builder.build(temporalQuery);
                } catch (RuntimeException e) {
                    pos.setErrorIndex(0);
                    return null;
                }
            } catch (IndexOutOfBoundsException e2) {
                if (pos.getErrorIndex() < 0) {
                    pos.setErrorIndex(0);
                }
                return null;
            }
        }
    }
}
