package org.threeten.bp.format;

import java.math.BigDecimal;
import java.math.BigInteger;
import java.math.RoundingMode;
import java.text.DateFormat;
import java.text.SimpleDateFormat;
import java.util.AbstractMap;
import java.util.ArrayList;
import java.util.Collections;
import java.util.Comparator;
import java.util.HashMap;
import java.util.Iterator;
import java.util.LinkedHashMap;
import java.util.List;
import java.util.Locale;
import java.util.Map;
import java.util.MissingResourceException;
import java.util.ResourceBundle;
import java.util.Set;
import java.util.TimeZone;
import java.util.TreeMap;
import kotlin.time.DurationKt;
import org.threeten.bp.DateTimeException;
import org.threeten.bp.Instant;
import org.threeten.bp.LocalDate;
import org.threeten.bp.LocalDateTime;
import org.threeten.bp.ZoneId;
import org.threeten.bp.ZoneOffset;
import org.threeten.bp.chrono.ChronoLocalDate;
import org.threeten.bp.chrono.Chronology;
import org.threeten.bp.format.SimpleDateTimeTextProvider;
import org.threeten.bp.jdk8.Jdk8Methods;
import org.threeten.bp.temporal.ChronoField;
import org.threeten.bp.temporal.IsoFields;
import org.threeten.bp.temporal.TemporalAccessor;
import org.threeten.bp.temporal.TemporalField;
import org.threeten.bp.temporal.TemporalQueries;
import org.threeten.bp.temporal.TemporalQuery;
import org.threeten.bp.temporal.ValueRange;
import org.threeten.bp.temporal.WeekFields;
import org.threeten.bp.zone.ZoneRulesProvider;

public final class DateTimeFormatterBuilder {
    private static final Map<Character, TemporalField> FIELD_MAP;
    static final Comparator<String> LENGTH_SORT = new Comparator<String>() {
        public int compare(String str1, String str2) {
            return str1.length() == str2.length() ? str1.compareTo(str2) : str1.length() - str2.length();
        }
    };
    private static final TemporalQuery<ZoneId> QUERY_REGION_ONLY = new TemporalQuery<ZoneId>() {
        public ZoneId queryFrom(TemporalAccessor temporal) {
            ZoneId zone = (ZoneId) temporal.query(TemporalQueries.zoneId());
            if (zone == null || (zone instanceof ZoneOffset)) {
                return null;
            }
            return zone;
        }
    };
    private DateTimeFormatterBuilder active;
    private final boolean optional;
    private char padNextChar;
    private int padNextWidth;
    private final DateTimeFormatterBuilder parent;
    private final List<DateTimePrinterParser> printerParsers;
    private int valueParserIndex;

    interface DateTimePrinterParser {
        int parse(DateTimeParseContext dateTimeParseContext, CharSequence charSequence, int i);

        boolean print(DateTimePrintContext dateTimePrintContext, StringBuilder sb);
    }

    static {
        HashMap hashMap = new HashMap();
        FIELD_MAP = hashMap;
        hashMap.put('G', ChronoField.ERA);
        hashMap.put('y', ChronoField.YEAR_OF_ERA);
        hashMap.put('u', ChronoField.YEAR);
        hashMap.put('Q', IsoFields.QUARTER_OF_YEAR);
        hashMap.put('q', IsoFields.QUARTER_OF_YEAR);
        hashMap.put('M', ChronoField.MONTH_OF_YEAR);
        hashMap.put('L', ChronoField.MONTH_OF_YEAR);
        hashMap.put('D', ChronoField.DAY_OF_YEAR);
        hashMap.put('d', ChronoField.DAY_OF_MONTH);
        hashMap.put('F', ChronoField.ALIGNED_DAY_OF_WEEK_IN_MONTH);
        hashMap.put('E', ChronoField.DAY_OF_WEEK);
        hashMap.put('c', ChronoField.DAY_OF_WEEK);
        hashMap.put('e', ChronoField.DAY_OF_WEEK);
        hashMap.put('a', ChronoField.AMPM_OF_DAY);
        hashMap.put('H', ChronoField.HOUR_OF_DAY);
        hashMap.put('k', ChronoField.CLOCK_HOUR_OF_DAY);
        hashMap.put('K', ChronoField.HOUR_OF_AMPM);
        hashMap.put('h', ChronoField.CLOCK_HOUR_OF_AMPM);
        hashMap.put('m', ChronoField.MINUTE_OF_HOUR);
        hashMap.put('s', ChronoField.SECOND_OF_MINUTE);
        hashMap.put('S', ChronoField.NANO_OF_SECOND);
        hashMap.put('A', ChronoField.MILLI_OF_DAY);
        hashMap.put('n', ChronoField.NANO_OF_SECOND);
        hashMap.put('N', ChronoField.NANO_OF_DAY);
    }

    public static String getLocalizedDateTimePattern(FormatStyle dateStyle, FormatStyle timeStyle, Chronology chrono, Locale locale) {
        DateFormat dateFormat;
        Jdk8Methods.requireNonNull(locale, "locale");
        Jdk8Methods.requireNonNull(chrono, "chrono");
        if (dateStyle == null && timeStyle == null) {
            throw new IllegalArgumentException("Either dateStyle or timeStyle must be non-null");
        }
        if (dateStyle == null) {
            dateFormat = DateFormat.getTimeInstance(timeStyle.ordinal(), locale);
        } else if (timeStyle != null) {
            dateFormat = DateFormat.getDateTimeInstance(dateStyle.ordinal(), timeStyle.ordinal(), locale);
        } else {
            dateFormat = DateFormat.getDateInstance(dateStyle.ordinal(), locale);
        }
        if (dateFormat instanceof SimpleDateFormat) {
            return ((SimpleDateFormat) dateFormat).toPattern();
        }
        throw new IllegalArgumentException("Unable to determine pattern");
    }

    public DateTimeFormatterBuilder() {
        this.active = this;
        this.printerParsers = new ArrayList();
        this.valueParserIndex = -1;
        this.parent = null;
        this.optional = false;
    }

    private DateTimeFormatterBuilder(DateTimeFormatterBuilder parent2, boolean optional2) {
        this.active = this;
        this.printerParsers = new ArrayList();
        this.valueParserIndex = -1;
        this.parent = parent2;
        this.optional = optional2;
    }

    public DateTimeFormatterBuilder parseCaseSensitive() {
        appendInternal(SettingsParser.SENSITIVE);
        return this;
    }

    public DateTimeFormatterBuilder parseCaseInsensitive() {
        appendInternal(SettingsParser.INSENSITIVE);
        return this;
    }

    public DateTimeFormatterBuilder parseStrict() {
        appendInternal(SettingsParser.STRICT);
        return this;
    }

    public DateTimeFormatterBuilder parseLenient() {
        appendInternal(SettingsParser.LENIENT);
        return this;
    }

    public DateTimeFormatterBuilder parseDefaulting(TemporalField field, long value) {
        Jdk8Methods.requireNonNull(field, "field");
        appendInternal(new DefaultingParser(field, value));
        return this;
    }

    public DateTimeFormatterBuilder appendValue(TemporalField field) {
        Jdk8Methods.requireNonNull(field, "field");
        appendValue(new NumberPrinterParser(field, 1, 19, SignStyle.NORMAL));
        return this;
    }

    public DateTimeFormatterBuilder appendValue(TemporalField field, int width) {
        Jdk8Methods.requireNonNull(field, "field");
        if (width < 1 || width > 19) {
            throw new IllegalArgumentException("The width must be from 1 to 19 inclusive but was " + width);
        }
        appendValue(new NumberPrinterParser(field, width, width, SignStyle.NOT_NEGATIVE));
        return this;
    }

    public DateTimeFormatterBuilder appendValue(TemporalField field, int minWidth, int maxWidth, SignStyle signStyle) {
        if (minWidth == maxWidth && signStyle == SignStyle.NOT_NEGATIVE) {
            return appendValue(field, maxWidth);
        }
        Jdk8Methods.requireNonNull(field, "field");
        Jdk8Methods.requireNonNull(signStyle, "signStyle");
        if (minWidth < 1 || minWidth > 19) {
            throw new IllegalArgumentException("The minimum width must be from 1 to 19 inclusive but was " + minWidth);
        } else if (maxWidth < 1 || maxWidth > 19) {
            throw new IllegalArgumentException("The maximum width must be from 1 to 19 inclusive but was " + maxWidth);
        } else if (maxWidth >= minWidth) {
            appendValue(new NumberPrinterParser(field, minWidth, maxWidth, signStyle));
            return this;
        } else {
            throw new IllegalArgumentException("The maximum width must exceed or equal the minimum width but " + maxWidth + " < " + minWidth);
        }
    }

    public DateTimeFormatterBuilder appendValueReduced(TemporalField field, int width, int maxWidth, int baseValue) {
        Jdk8Methods.requireNonNull(field, "field");
        appendValue((NumberPrinterParser) new ReducedPrinterParser(field, width, maxWidth, baseValue, (ChronoLocalDate) null));
        return this;
    }

    public DateTimeFormatterBuilder appendValueReduced(TemporalField field, int width, int maxWidth, ChronoLocalDate baseDate) {
        Jdk8Methods.requireNonNull(field, "field");
        Jdk8Methods.requireNonNull(baseDate, "baseDate");
        appendValue((NumberPrinterParser) new ReducedPrinterParser(field, width, maxWidth, 0, baseDate));
        return this;
    }

    private DateTimeFormatterBuilder appendValue(NumberPrinterParser pp) {
        NumberPrinterParser basePP;
        DateTimeFormatterBuilder dateTimeFormatterBuilder = this.active;
        int i = dateTimeFormatterBuilder.valueParserIndex;
        if (i < 0 || !(dateTimeFormatterBuilder.printerParsers.get(i) instanceof NumberPrinterParser)) {
            this.active.valueParserIndex = appendInternal(pp);
        } else {
            DateTimeFormatterBuilder dateTimeFormatterBuilder2 = this.active;
            int activeValueParser = dateTimeFormatterBuilder2.valueParserIndex;
            NumberPrinterParser basePP2 = (NumberPrinterParser) dateTimeFormatterBuilder2.printerParsers.get(activeValueParser);
            if (pp.minWidth == pp.maxWidth && pp.signStyle == SignStyle.NOT_NEGATIVE) {
                basePP = basePP2.withSubsequentWidth(pp.maxWidth);
                appendInternal(pp.withFixedWidth());
                this.active.valueParserIndex = activeValueParser;
            } else {
                basePP = basePP2.withFixedWidth();
                this.active.valueParserIndex = appendInternal(pp);
            }
            this.active.printerParsers.set(activeValueParser, basePP);
        }
        return this;
    }

    public DateTimeFormatterBuilder appendFraction(TemporalField field, int minWidth, int maxWidth, boolean decimalPoint) {
        appendInternal(new FractionPrinterParser(field, minWidth, maxWidth, decimalPoint));
        return this;
    }

    public DateTimeFormatterBuilder appendText(TemporalField field) {
        return appendText(field, TextStyle.FULL);
    }

    public DateTimeFormatterBuilder appendText(TemporalField field, TextStyle textStyle) {
        Jdk8Methods.requireNonNull(field, "field");
        Jdk8Methods.requireNonNull(textStyle, "textStyle");
        appendInternal(new TextPrinterParser(field, textStyle, DateTimeTextProvider.getInstance()));
        return this;
    }

    public DateTimeFormatterBuilder appendText(TemporalField field, Map<Long, String> textLookup) {
        Jdk8Methods.requireNonNull(field, "field");
        Jdk8Methods.requireNonNull(textLookup, "textLookup");
        final SimpleDateTimeTextProvider.LocaleStore store = new SimpleDateTimeTextProvider.LocaleStore(Collections.singletonMap(TextStyle.FULL, new LinkedHashMap<>(textLookup)));
        appendInternal(new TextPrinterParser(field, TextStyle.FULL, new DateTimeTextProvider() {
            public String getText(TemporalField field, long value, TextStyle style, Locale locale) {
                return store.getText(value, style);
            }

            public Iterator<Map.Entry<String, Long>> getTextIterator(TemporalField field, TextStyle style, Locale locale) {
                return store.getTextIterator(style);
            }
        }));
        return this;
    }

    public DateTimeFormatterBuilder appendInstant() {
        appendInternal(new InstantPrinterParser(-2));
        return this;
    }

    public DateTimeFormatterBuilder appendInstant(int fractionalDigits) {
        if (fractionalDigits < -1 || fractionalDigits > 9) {
            throw new IllegalArgumentException("Invalid fractional digits: " + fractionalDigits);
        }
        appendInternal(new InstantPrinterParser(fractionalDigits));
        return this;
    }

    public DateTimeFormatterBuilder appendOffsetId() {
        appendInternal(OffsetIdPrinterParser.INSTANCE_ID);
        return this;
    }

    public DateTimeFormatterBuilder appendOffset(String pattern, String noOffsetText) {
        appendInternal(new OffsetIdPrinterParser(noOffsetText, pattern));
        return this;
    }

    public DateTimeFormatterBuilder appendLocalizedOffset(TextStyle style) {
        Jdk8Methods.requireNonNull(style, "style");
        if (style == TextStyle.FULL || style == TextStyle.SHORT) {
            appendInternal(new LocalizedOffsetPrinterParser(style));
            return this;
        }
        throw new IllegalArgumentException("Style must be either full or short");
    }

    public DateTimeFormatterBuilder appendZoneId() {
        appendInternal(new ZoneIdPrinterParser(TemporalQueries.zoneId(), "ZoneId()"));
        return this;
    }

    public DateTimeFormatterBuilder appendZoneRegionId() {
        appendInternal(new ZoneIdPrinterParser(QUERY_REGION_ONLY, "ZoneRegionId()"));
        return this;
    }

    public DateTimeFormatterBuilder appendZoneOrOffsetId() {
        appendInternal(new ZoneIdPrinterParser(TemporalQueries.zone(), "ZoneOrOffsetId()"));
        return this;
    }

    public DateTimeFormatterBuilder appendZoneText(TextStyle textStyle) {
        appendInternal(new ZoneTextPrinterParser(textStyle));
        return this;
    }

    public DateTimeFormatterBuilder appendZoneText(TextStyle textStyle, Set<ZoneId> preferredZones) {
        Jdk8Methods.requireNonNull(preferredZones, "preferredZones");
        appendInternal(new ZoneTextPrinterParser(textStyle));
        return this;
    }

    public DateTimeFormatterBuilder appendChronologyId() {
        appendInternal(new ChronoPrinterParser((TextStyle) null));
        return this;
    }

    public DateTimeFormatterBuilder appendChronologyText(TextStyle textStyle) {
        Jdk8Methods.requireNonNull(textStyle, "textStyle");
        appendInternal(new ChronoPrinterParser(textStyle));
        return this;
    }

    public DateTimeFormatterBuilder appendLocalized(FormatStyle dateStyle, FormatStyle timeStyle) {
        if (dateStyle == null && timeStyle == null) {
            throw new IllegalArgumentException("Either the date or time style must be non-null");
        }
        appendInternal(new LocalizedPrinterParser(dateStyle, timeStyle));
        return this;
    }

    public DateTimeFormatterBuilder appendLiteral(char literal) {
        appendInternal(new CharLiteralPrinterParser(literal));
        return this;
    }

    public DateTimeFormatterBuilder appendLiteral(String literal) {
        Jdk8Methods.requireNonNull(literal, "literal");
        if (literal.length() > 0) {
            if (literal.length() == 1) {
                appendInternal(new CharLiteralPrinterParser(literal.charAt(0)));
            } else {
                appendInternal(new StringLiteralPrinterParser(literal));
            }
        }
        return this;
    }

    public DateTimeFormatterBuilder append(DateTimeFormatter formatter) {
        Jdk8Methods.requireNonNull(formatter, "formatter");
        appendInternal(formatter.toPrinterParser(false));
        return this;
    }

    public DateTimeFormatterBuilder appendOptional(DateTimeFormatter formatter) {
        Jdk8Methods.requireNonNull(formatter, "formatter");
        appendInternal(formatter.toPrinterParser(true));
        return this;
    }

    public DateTimeFormatterBuilder appendPattern(String pattern) {
        Jdk8Methods.requireNonNull(pattern, "pattern");
        parsePattern(pattern);
        return this;
    }

    private void parsePattern(String pattern) {
        int start = 0;
        while (start < pattern.length()) {
            char cur = pattern.charAt(start);
            if ((cur >= 'A' && cur <= 'Z') || (cur >= 'a' && cur <= 'z')) {
                int pos = start + 1;
                while (pos < pattern.length() && pattern.charAt(pos) == cur) {
                    pos++;
                }
                int count = pos - start;
                if (cur == 'p') {
                    int pad = 0;
                    if (pos < pattern.length() && (((cur = pattern.charAt(pos)) >= 'A' && cur <= 'Z') || (cur >= 'a' && cur <= 'z'))) {
                        pad = count;
                        int pos2 = pos + 1;
                        int start2 = pos;
                        while (pos2 < pattern.length() && pattern.charAt(pos2) == cur) {
                            pos2++;
                        }
                        pos = pos2;
                        count = pos2 - start2;
                    }
                    if (pad != 0) {
                        padNext(pad);
                    } else {
                        throw new IllegalArgumentException("Pad letter 'p' must be followed by valid pad pattern: " + pattern);
                    }
                }
                TemporalField field = FIELD_MAP.get(Character.valueOf(cur));
                if (field != null) {
                    parseField(cur, count, field);
                } else if (cur == 'z') {
                    if (count > 4) {
                        throw new IllegalArgumentException("Too many pattern letters: " + cur);
                    } else if (count == 4) {
                        appendZoneText(TextStyle.FULL);
                    } else {
                        appendZoneText(TextStyle.SHORT);
                    }
                } else if (cur != 'V') {
                    String str = "+0000";
                    if (cur == 'Z') {
                        if (count < 4) {
                            appendOffset("+HHMM", str);
                        } else if (count == 4) {
                            appendLocalizedOffset(TextStyle.FULL);
                        } else if (count == 5) {
                            appendOffset("+HH:MM:ss", "Z");
                        } else {
                            throw new IllegalArgumentException("Too many pattern letters: " + cur);
                        }
                    } else if (cur != 'O') {
                        int i = 0;
                        if (cur == 'X') {
                            if (count <= 5) {
                                String[] strArr = OffsetIdPrinterParser.PATTERNS;
                                if (count != 1) {
                                    i = 1;
                                }
                                appendOffset(strArr[i + count], "Z");
                            } else {
                                throw new IllegalArgumentException("Too many pattern letters: " + cur);
                            }
                        } else if (cur == 'x') {
                            if (count <= 5) {
                                if (count == 1) {
                                    str = "+00";
                                } else if (count % 2 != 0) {
                                    str = "+00:00";
                                }
                                String zero = str;
                                String[] strArr2 = OffsetIdPrinterParser.PATTERNS;
                                if (count != 1) {
                                    i = 1;
                                }
                                appendOffset(strArr2[i + count], zero);
                            } else {
                                throw new IllegalArgumentException("Too many pattern letters: " + cur);
                            }
                        } else if (cur == 'W') {
                            if (count <= 1) {
                                appendInternal(new WeekFieldsPrinterParser('W', count));
                            } else {
                                throw new IllegalArgumentException("Too many pattern letters: " + cur);
                            }
                        } else if (cur == 'w') {
                            if (count <= 2) {
                                appendInternal(new WeekFieldsPrinterParser('w', count));
                            } else {
                                throw new IllegalArgumentException("Too many pattern letters: " + cur);
                            }
                        } else if (cur == 'Y') {
                            appendInternal(new WeekFieldsPrinterParser('Y', count));
                        } else {
                            throw new IllegalArgumentException("Unknown pattern letter: " + cur);
                        }
                    } else if (count == 1) {
                        appendLocalizedOffset(TextStyle.SHORT);
                    } else if (count == 4) {
                        appendLocalizedOffset(TextStyle.FULL);
                    } else {
                        throw new IllegalArgumentException("Pattern letter count must be 1 or 4: " + cur);
                    }
                } else if (count == 2) {
                    appendZoneId();
                } else {
                    throw new IllegalArgumentException("Pattern letter count must be 2: " + cur);
                }
                start = pos - 1;
            } else if (cur == '\'') {
                int pos3 = start + 1;
                while (pos3 < pattern.length()) {
                    if (pattern.charAt(pos3) == '\'') {
                        if (pos3 + 1 >= pattern.length() || pattern.charAt(pos3 + 1) != '\'') {
                            break;
                        }
                        pos3++;
                    }
                    pos3++;
                }
                if (pos3 < pattern.length()) {
                    String str2 = pattern.substring(start + 1, pos3);
                    if (str2.length() == 0) {
                        appendLiteral('\'');
                    } else {
                        appendLiteral(str2.replace("''", "'"));
                    }
                    start = pos3;
                } else {
                    throw new IllegalArgumentException("Pattern ends with an incomplete string literal: " + pattern);
                }
            } else if (cur == '[') {
                optionalStart();
            } else if (cur == ']') {
                if (this.active.parent != null) {
                    optionalEnd();
                } else {
                    throw new IllegalArgumentException("Pattern invalid as it contains ] without previous [");
                }
            } else if (cur == '{' || cur == '}' || cur == '#') {
                throw new IllegalArgumentException("Pattern includes reserved character: '" + cur + "'");
            } else {
                appendLiteral(cur);
            }
            start++;
        }
    }

    private void parseField(char cur, int count, TemporalField field) {
        switch (cur) {
            case 'D':
                if (count == 1) {
                    appendValue(field);
                    return;
                } else if (count <= 3) {
                    appendValue(field, count);
                    return;
                } else {
                    throw new IllegalArgumentException("Too many pattern letters: " + cur);
                }
            case 'E':
            case 'G':
                switch (count) {
                    case 1:
                    case 2:
                    case 3:
                        appendText(field, TextStyle.SHORT);
                        return;
                    case 4:
                        appendText(field, TextStyle.FULL);
                        return;
                    case 5:
                        appendText(field, TextStyle.NARROW);
                        return;
                    default:
                        throw new IllegalArgumentException("Too many pattern letters: " + cur);
                }
            case 'F':
                if (count == 1) {
                    appendValue(field);
                    return;
                }
                throw new IllegalArgumentException("Too many pattern letters: " + cur);
            case 'H':
            case 'K':
            case 'd':
            case 'h':
            case 'k':
            case 'm':
            case 's':
                if (count == 1) {
                    appendValue(field);
                    return;
                } else if (count == 2) {
                    appendValue(field, count);
                    return;
                } else {
                    throw new IllegalArgumentException("Too many pattern letters: " + cur);
                }
            case 'L':
            case 'q':
                switch (count) {
                    case 1:
                        appendValue(field);
                        return;
                    case 2:
                        appendValue(field, 2);
                        return;
                    case 3:
                        appendText(field, TextStyle.SHORT_STANDALONE);
                        return;
                    case 4:
                        appendText(field, TextStyle.FULL_STANDALONE);
                        return;
                    case 5:
                        appendText(field, TextStyle.NARROW_STANDALONE);
                        return;
                    default:
                        throw new IllegalArgumentException("Too many pattern letters: " + cur);
                }
            case 'M':
            case 'Q':
                switch (count) {
                    case 1:
                        appendValue(field);
                        return;
                    case 2:
                        appendValue(field, 2);
                        return;
                    case 3:
                        appendText(field, TextStyle.SHORT);
                        return;
                    case 4:
                        appendText(field, TextStyle.FULL);
                        return;
                    case 5:
                        appendText(field, TextStyle.NARROW);
                        return;
                    default:
                        throw new IllegalArgumentException("Too many pattern letters: " + cur);
                }
            case 'S':
                appendFraction(ChronoField.NANO_OF_SECOND, count, count, false);
                return;
            case 'a':
                if (count == 1) {
                    appendText(field, TextStyle.SHORT);
                    return;
                }
                throw new IllegalArgumentException("Too many pattern letters: " + cur);
            case 'c':
                switch (count) {
                    case 1:
                        appendInternal(new WeekFieldsPrinterParser('c', count));
                        return;
                    case 2:
                        throw new IllegalArgumentException("Invalid number of pattern letters: " + cur);
                    case 3:
                        appendText(field, TextStyle.SHORT_STANDALONE);
                        return;
                    case 4:
                        appendText(field, TextStyle.FULL_STANDALONE);
                        return;
                    case 5:
                        appendText(field, TextStyle.NARROW_STANDALONE);
                        return;
                    default:
                        throw new IllegalArgumentException("Too many pattern letters: " + cur);
                }
            case 'e':
                switch (count) {
                    case 1:
                    case 2:
                        appendInternal(new WeekFieldsPrinterParser('e', count));
                        return;
                    case 3:
                        appendText(field, TextStyle.SHORT);
                        return;
                    case 4:
                        appendText(field, TextStyle.FULL);
                        return;
                    case 5:
                        appendText(field, TextStyle.NARROW);
                        return;
                    default:
                        throw new IllegalArgumentException("Too many pattern letters: " + cur);
                }
            case 'u':
            case 'y':
                if (count == 2) {
                    appendValueReduced(field, 2, 2, (ChronoLocalDate) ReducedPrinterParser.BASE_DATE);
                    return;
                } else if (count < 4) {
                    appendValue(field, count, 19, SignStyle.NORMAL);
                    return;
                } else {
                    appendValue(field, count, 19, SignStyle.EXCEEDS_PAD);
                    return;
                }
            default:
                if (count == 1) {
                    appendValue(field);
                    return;
                } else {
                    appendValue(field, count);
                    return;
                }
        }
    }

    public DateTimeFormatterBuilder padNext(int padWidth) {
        return padNext(padWidth, ' ');
    }

    public DateTimeFormatterBuilder padNext(int padWidth, char padChar) {
        if (padWidth >= 1) {
            DateTimeFormatterBuilder dateTimeFormatterBuilder = this.active;
            dateTimeFormatterBuilder.padNextWidth = padWidth;
            dateTimeFormatterBuilder.padNextChar = padChar;
            dateTimeFormatterBuilder.valueParserIndex = -1;
            return this;
        }
        throw new IllegalArgumentException("The pad width must be at least one but was " + padWidth);
    }

    public DateTimeFormatterBuilder optionalStart() {
        DateTimeFormatterBuilder dateTimeFormatterBuilder = this.active;
        dateTimeFormatterBuilder.valueParserIndex = -1;
        this.active = new DateTimeFormatterBuilder(dateTimeFormatterBuilder, true);
        return this;
    }

    public DateTimeFormatterBuilder optionalEnd() {
        DateTimeFormatterBuilder dateTimeFormatterBuilder = this.active;
        if (dateTimeFormatterBuilder.parent != null) {
            if (dateTimeFormatterBuilder.printerParsers.size() > 0) {
                DateTimeFormatterBuilder dateTimeFormatterBuilder2 = this.active;
                CompositePrinterParser cpp = new CompositePrinterParser(dateTimeFormatterBuilder2.printerParsers, dateTimeFormatterBuilder2.optional);
                this.active = this.active.parent;
                appendInternal(cpp);
            } else {
                this.active = this.active.parent;
            }
            return this;
        }
        throw new IllegalStateException("Cannot call optionalEnd() as there was no previous call to optionalStart()");
    }

    private int appendInternal(DateTimePrinterParser pp) {
        Jdk8Methods.requireNonNull(pp, "pp");
        DateTimeFormatterBuilder dateTimeFormatterBuilder = this.active;
        int i = dateTimeFormatterBuilder.padNextWidth;
        if (i > 0) {
            if (pp != null) {
                pp = new PadPrinterParserDecorator(pp, i, dateTimeFormatterBuilder.padNextChar);
            }
            DateTimeFormatterBuilder dateTimeFormatterBuilder2 = this.active;
            dateTimeFormatterBuilder2.padNextWidth = 0;
            dateTimeFormatterBuilder2.padNextChar = 0;
        }
        this.active.printerParsers.add(pp);
        DateTimeFormatterBuilder dateTimeFormatterBuilder3 = this.active;
        dateTimeFormatterBuilder3.valueParserIndex = -1;
        return dateTimeFormatterBuilder3.printerParsers.size() - 1;
    }

    public DateTimeFormatter toFormatter() {
        return toFormatter(Locale.getDefault());
    }

    public DateTimeFormatter toFormatter(Locale locale) {
        Jdk8Methods.requireNonNull(locale, "locale");
        while (this.active.parent != null) {
            optionalEnd();
        }
        return new DateTimeFormatter(new CompositePrinterParser(this.printerParsers, false), locale, DecimalStyle.STANDARD, ResolverStyle.SMART, (Set<TemporalField>) null, (Chronology) null, (ZoneId) null);
    }

    /* access modifiers changed from: package-private */
    public DateTimeFormatter toFormatter(ResolverStyle style) {
        return toFormatter().withResolverStyle(style);
    }

    static final class CompositePrinterParser implements DateTimePrinterParser {
        private final boolean optional;
        private final DateTimePrinterParser[] printerParsers;

        CompositePrinterParser(List<DateTimePrinterParser> printerParsers2, boolean optional2) {
            this((DateTimePrinterParser[]) printerParsers2.toArray(new DateTimePrinterParser[printerParsers2.size()]), optional2);
        }

        CompositePrinterParser(DateTimePrinterParser[] printerParsers2, boolean optional2) {
            this.printerParsers = printerParsers2;
            this.optional = optional2;
        }

        public CompositePrinterParser withOptional(boolean optional2) {
            if (optional2 == this.optional) {
                return this;
            }
            return new CompositePrinterParser(this.printerParsers, optional2);
        }

        public boolean print(DateTimePrintContext context, StringBuilder buf) {
            int length = buf.length();
            if (this.optional) {
                context.startOptional();
            }
            try {
                for (DateTimePrinterParser pp : this.printerParsers) {
                    if (!pp.print(context, buf)) {
                        buf.setLength(length);
                        return true;
                    }
                }
                if (this.optional) {
                    context.endOptional();
                }
                return true;
            } finally {
                if (this.optional) {
                    context.endOptional();
                }
            }
        }

        public int parse(DateTimeParseContext context, CharSequence text, int position) {
            if (this.optional) {
                context.startOptional();
                int pos = position;
                for (DateTimePrinterParser pp : this.printerParsers) {
                    pos = pp.parse(context, text, pos);
                    if (pos < 0) {
                        context.endOptional(false);
                        return position;
                    }
                }
                context.endOptional(true);
                return pos;
            }
            for (DateTimePrinterParser pp2 : this.printerParsers) {
                position = pp2.parse(context, text, position);
                if (position < 0) {
                    break;
                }
            }
            return position;
        }

        public String toString() {
            StringBuilder buf = new StringBuilder();
            if (this.printerParsers != null) {
                buf.append(this.optional ? "[" : "(");
                for (DateTimePrinterParser pp : this.printerParsers) {
                    buf.append(pp);
                }
                buf.append(this.optional ? "]" : ")");
            }
            return buf.toString();
        }
    }

    static final class PadPrinterParserDecorator implements DateTimePrinterParser {
        private final char padChar;
        private final int padWidth;
        private final DateTimePrinterParser printerParser;

        PadPrinterParserDecorator(DateTimePrinterParser printerParser2, int padWidth2, char padChar2) {
            this.printerParser = printerParser2;
            this.padWidth = padWidth2;
            this.padChar = padChar2;
        }

        public boolean print(DateTimePrintContext context, StringBuilder buf) {
            int preLen = buf.length();
            if (!this.printerParser.print(context, buf)) {
                return false;
            }
            int len = buf.length() - preLen;
            if (len <= this.padWidth) {
                for (int i = 0; i < this.padWidth - len; i++) {
                    buf.insert(preLen, this.padChar);
                }
                return true;
            }
            throw new DateTimeException("Cannot print as output of " + len + " characters exceeds pad width of " + this.padWidth);
        }

        public int parse(DateTimeParseContext context, CharSequence text, int position) {
            boolean strict = context.isStrict();
            boolean caseSensitive = context.isCaseSensitive();
            if (position > text.length()) {
                throw new IndexOutOfBoundsException();
            } else if (position == text.length()) {
                return ~position;
            } else {
                int endPos = this.padWidth + position;
                if (endPos > text.length()) {
                    if (strict) {
                        return ~position;
                    }
                    endPos = text.length();
                }
                int pos = position;
                while (pos < endPos) {
                    char charAt = text.charAt(pos);
                    char c = this.padChar;
                    if (!caseSensitive) {
                        if (!context.charEquals(charAt, c)) {
                            break;
                        }
                    } else if (charAt != c) {
                        break;
                    }
                    pos++;
                }
                int resultPos = this.printerParser.parse(context, text.subSequence(0, endPos), pos);
                if (resultPos == endPos || !strict) {
                    return resultPos;
                }
                return ~(position + pos);
            }
        }

        public String toString() {
            return "Pad(" + this.printerParser + "," + this.padWidth + (this.padChar == ' ' ? ")" : ",'" + this.padChar + "')");
        }
    }

    enum SettingsParser implements DateTimePrinterParser {
        SENSITIVE,
        INSENSITIVE,
        STRICT,
        LENIENT;

        public boolean print(DateTimePrintContext context, StringBuilder buf) {
            return true;
        }

        public int parse(DateTimeParseContext context, CharSequence text, int position) {
            switch (ordinal()) {
                case 0:
                    context.setCaseSensitive(true);
                    break;
                case 1:
                    context.setCaseSensitive(false);
                    break;
                case 2:
                    context.setStrict(true);
                    break;
                case 3:
                    context.setStrict(false);
                    break;
            }
            return position;
        }

        public String toString() {
            switch (ordinal()) {
                case 0:
                    return "ParseCaseSensitive(true)";
                case 1:
                    return "ParseCaseSensitive(false)";
                case 2:
                    return "ParseStrict(true)";
                case 3:
                    return "ParseStrict(false)";
                default:
                    throw new IllegalStateException("Unreachable");
            }
        }
    }

    static class DefaultingParser implements DateTimePrinterParser {
        private final TemporalField field;
        private final long value;

        DefaultingParser(TemporalField field2, long value2) {
            this.field = field2;
            this.value = value2;
        }

        public boolean print(DateTimePrintContext context, StringBuilder buf) {
            return true;
        }

        public int parse(DateTimeParseContext context, CharSequence text, int position) {
            if (context.getParsed(this.field) == null) {
                context.setParsedField(this.field, this.value, position, position);
            }
            return position;
        }
    }

    static final class CharLiteralPrinterParser implements DateTimePrinterParser {
        private final char literal;

        CharLiteralPrinterParser(char literal2) {
            this.literal = literal2;
        }

        public boolean print(DateTimePrintContext context, StringBuilder buf) {
            buf.append(this.literal);
            return true;
        }

        public int parse(DateTimeParseContext context, CharSequence text, int position) {
            if (position == text.length()) {
                return ~position;
            }
            if (!context.charEquals(this.literal, text.charAt(position))) {
                return ~position;
            }
            return position + 1;
        }

        public String toString() {
            if (this.literal == '\'') {
                return "''";
            }
            return "'" + this.literal + "'";
        }
    }

    static final class StringLiteralPrinterParser implements DateTimePrinterParser {
        private final String literal;

        StringLiteralPrinterParser(String literal2) {
            this.literal = literal2;
        }

        public boolean print(DateTimePrintContext context, StringBuilder buf) {
            buf.append(this.literal);
            return true;
        }

        public int parse(DateTimeParseContext context, CharSequence text, int position) {
            if (position > text.length() || position < 0) {
                throw new IndexOutOfBoundsException();
            }
            String str = this.literal;
            if (!context.subSequenceEquals(text, position, str, 0, str.length())) {
                return ~position;
            }
            return this.literal.length() + position;
        }

        public String toString() {
            return "'" + this.literal.replace("'", "''") + "'";
        }
    }

    static class NumberPrinterParser implements DateTimePrinterParser {
        static final int[] EXCEED_POINTS = {0, 10, 100, 1000, 10000, 100000, DurationKt.NANOS_IN_MILLIS, 10000000, 100000000, 1000000000};
        final TemporalField field;
        final int maxWidth;
        final int minWidth;
        final SignStyle signStyle;
        final int subsequentWidth;

        NumberPrinterParser(TemporalField field2, int minWidth2, int maxWidth2, SignStyle signStyle2) {
            this.field = field2;
            this.minWidth = minWidth2;
            this.maxWidth = maxWidth2;
            this.signStyle = signStyle2;
            this.subsequentWidth = 0;
        }

        private NumberPrinterParser(TemporalField field2, int minWidth2, int maxWidth2, SignStyle signStyle2, int subsequentWidth2) {
            this.field = field2;
            this.minWidth = minWidth2;
            this.maxWidth = maxWidth2;
            this.signStyle = signStyle2;
            this.subsequentWidth = subsequentWidth2;
        }

        /* access modifiers changed from: package-private */
        public NumberPrinterParser withFixedWidth() {
            if (this.subsequentWidth == -1) {
                return this;
            }
            return new NumberPrinterParser(this.field, this.minWidth, this.maxWidth, this.signStyle, -1);
        }

        /* access modifiers changed from: package-private */
        public NumberPrinterParser withSubsequentWidth(int subsequentWidth2) {
            return new NumberPrinterParser(this.field, this.minWidth, this.maxWidth, this.signStyle, this.subsequentWidth + subsequentWidth2);
        }

        public boolean print(DateTimePrintContext context, StringBuilder buf) {
            Long valueLong = context.getValue(this.field);
            if (valueLong == null) {
                return false;
            }
            long value = getValue(context, valueLong.longValue());
            DecimalStyle symbols = context.getSymbols();
            String str = value == Long.MIN_VALUE ? "9223372036854775808" : Long.toString(Math.abs(value));
            if (str.length() <= this.maxWidth) {
                String str2 = symbols.convertNumberToI18N(str);
                if (value >= 0) {
                    switch (AnonymousClass4.$SwitchMap$org$threeten$bp$format$SignStyle[this.signStyle.ordinal()]) {
                        case 1:
                            int i = this.minWidth;
                            if (i < 19 && value >= ((long) EXCEED_POINTS[i])) {
                                buf.append(symbols.getPositiveSign());
                                break;
                            }
                        case 2:
                            buf.append(symbols.getPositiveSign());
                            break;
                    }
                } else {
                    switch (AnonymousClass4.$SwitchMap$org$threeten$bp$format$SignStyle[this.signStyle.ordinal()]) {
                        case 1:
                        case 2:
                        case 3:
                            buf.append(symbols.getNegativeSign());
                            break;
                        case 4:
                            throw new DateTimeException("Field " + this.field + " cannot be printed as the value " + value + " cannot be negative according to the SignStyle");
                    }
                }
                for (int i2 = 0; i2 < this.minWidth - str2.length(); i2++) {
                    buf.append(symbols.getZeroDigit());
                }
                buf.append(str2);
                return true;
            }
            throw new DateTimeException("Field " + this.field + " cannot be printed as the value " + value + " exceeds the maximum print width of " + this.maxWidth);
        }

        /* access modifiers changed from: package-private */
        public long getValue(DateTimePrintContext context, long value) {
            return value;
        }

        /* access modifiers changed from: package-private */
        public boolean isFixedWidth(DateTimeParseContext context) {
            int i = this.subsequentWidth;
            return i == -1 || (i > 0 && this.minWidth == this.maxWidth && this.signStyle == SignStyle.NOT_NEGATIVE);
        }

        public int parse(DateTimeParseContext context, CharSequence text, int position) {
            boolean positive;
            boolean negative;
            int position2;
            int pos;
            long total;
            BigInteger totalBig;
            long total2;
            int length;
            char sign;
            char sign2;
            int i = position;
            int length2 = text.length();
            if (i == length2) {
                return ~i;
            }
            char sign3 = text.charAt(position);
            int i2 = 1;
            if (sign3 == context.getSymbols().getPositiveSign()) {
                if (!this.signStyle.parse(true, context.isStrict(), this.minWidth == this.maxWidth)) {
                    return ~i;
                }
                position2 = i + 1;
                negative = false;
                positive = true;
            } else if (sign3 == context.getSymbols().getNegativeSign()) {
                if (!this.signStyle.parse(false, context.isStrict(), this.minWidth == this.maxWidth)) {
                    return ~i;
                }
                position2 = i + 1;
                negative = true;
                positive = false;
            } else if (this.signStyle == SignStyle.ALWAYS && context.isStrict()) {
                return ~i;
            } else {
                position2 = i;
                negative = false;
                positive = false;
            }
            if (context.isStrict() || isFixedWidth(context)) {
                i2 = this.minWidth;
            }
            int effMinWidth = i2;
            int minEndPos = position2 + effMinWidth;
            if (minEndPos > length2) {
                return ~position2;
            }
            long total3 = 0;
            BigInteger totalBig2 = null;
            int pos2 = position2;
            int pass = 0;
            int effMaxWidth = ((context.isStrict() || isFixedWidth(context)) ? this.maxWidth : 9) + Math.max(this.subsequentWidth, 0);
            while (true) {
                if (pass >= 2) {
                    char c = sign3;
                    pos = pos2;
                    break;
                }
                int digit = Math.min(pos2 + effMaxWidth, length2);
                while (true) {
                    if (pos2 >= digit) {
                        int maxEndPos = digit;
                        total2 = total3;
                        length = length2;
                        sign = sign3;
                        break;
                    }
                    int pos3 = pos2 + 1;
                    length = length2;
                    char ch = text.charAt(pos2);
                    int maxEndPos2 = digit;
                    int digit2 = context.getSymbols().convertToDigit(ch);
                    if (digit2 < 0) {
                        int pos4 = pos3 - 1;
                        if (pos4 < minEndPos) {
                            char c2 = ch;
                            return ~position2;
                        }
                        total2 = total3;
                        sign = sign3;
                        pos2 = pos4;
                    } else {
                        if (pos3 - position2 > 18) {
                            if (totalBig2 == null) {
                                totalBig2 = BigInteger.valueOf(total3);
                            }
                            sign2 = sign3;
                            totalBig2 = totalBig2.multiply(BigInteger.TEN).add(BigInteger.valueOf((long) digit2));
                        } else {
                            sign2 = sign3;
                            long j = total3;
                            total3 = ((long) digit2) + (10 * total3);
                        }
                        digit = maxEndPos2;
                        pos2 = pos3;
                        length2 = length;
                        sign3 = sign2;
                    }
                }
                int maxEndPos3 = this.subsequentWidth;
                if (maxEndPos3 <= 0 || pass != 0) {
                    pos = pos2;
                    total3 = total2;
                } else {
                    effMaxWidth = Math.max(effMinWidth, (pos2 - position2) - maxEndPos3);
                    pos2 = position2;
                    totalBig2 = null;
                    pass++;
                    total3 = 0;
                    length2 = length;
                    sign3 = sign;
                }
            }
            if (!negative) {
                if (this.signStyle == SignStyle.EXCEEDS_PAD && context.isStrict()) {
                    int parseLen = pos - position2;
                    if (positive) {
                        if (parseLen <= this.minWidth) {
                            return ~(position2 - 1);
                        }
                    } else if (parseLen > this.minWidth) {
                        return ~position2;
                    }
                }
                total = total3;
                totalBig = totalBig2;
            } else if (totalBig2 != null) {
                if (totalBig2.equals(BigInteger.ZERO) && context.isStrict()) {
                    return ~(position2 - 1);
                }
                total = total3;
                totalBig = totalBig2.negate();
            } else if (total3 == 0 && context.isStrict()) {
                return ~(position2 - 1);
            } else {
                total = -total3;
                totalBig = totalBig2;
            }
            if (totalBig == null) {
                return setValue(context, total, position2, pos);
            }
            if (totalBig.bitLength() > 63) {
                totalBig = totalBig.divide(BigInteger.TEN);
                pos--;
            }
            return setValue(context, totalBig.longValue(), position2, pos);
        }

        /* access modifiers changed from: package-private */
        public int setValue(DateTimeParseContext context, long value, int errorPos, int successPos) {
            return context.setParsedField(this.field, value, errorPos, successPos);
        }

        public String toString() {
            if (this.minWidth == 1 && this.maxWidth == 19 && this.signStyle == SignStyle.NORMAL) {
                return "Value(" + this.field + ")";
            }
            if (this.minWidth == this.maxWidth && this.signStyle == SignStyle.NOT_NEGATIVE) {
                return "Value(" + this.field + "," + this.minWidth + ")";
            }
            return "Value(" + this.field + "," + this.minWidth + "," + this.maxWidth + "," + this.signStyle + ")";
        }
    }

    /* renamed from: org.threeten.bp.format.DateTimeFormatterBuilder$4  reason: invalid class name */
    static /* synthetic */ class AnonymousClass4 {
        static final /* synthetic */ int[] $SwitchMap$org$threeten$bp$format$SignStyle;

        static {
            int[] iArr = new int[SignStyle.values().length];
            $SwitchMap$org$threeten$bp$format$SignStyle = iArr;
            try {
                iArr[SignStyle.EXCEEDS_PAD.ordinal()] = 1;
            } catch (NoSuchFieldError e) {
            }
            try {
                $SwitchMap$org$threeten$bp$format$SignStyle[SignStyle.ALWAYS.ordinal()] = 2;
            } catch (NoSuchFieldError e2) {
            }
            try {
                $SwitchMap$org$threeten$bp$format$SignStyle[SignStyle.NORMAL.ordinal()] = 3;
            } catch (NoSuchFieldError e3) {
            }
            try {
                $SwitchMap$org$threeten$bp$format$SignStyle[SignStyle.NOT_NEGATIVE.ordinal()] = 4;
            } catch (NoSuchFieldError e4) {
            }
        }
    }

    static final class ReducedPrinterParser extends NumberPrinterParser {
        static final LocalDate BASE_DATE = LocalDate.of(2000, 1, 1);
        private final ChronoLocalDate baseDate;
        private final int baseValue;

        ReducedPrinterParser(TemporalField field, int width, int maxWidth, int baseValue2, ChronoLocalDate baseDate2) {
            super(field, width, maxWidth, SignStyle.NOT_NEGATIVE);
            if (width < 1 || width > 10) {
                throw new IllegalArgumentException("The width must be from 1 to 10 inclusive but was " + width);
            } else if (maxWidth < 1 || maxWidth > 10) {
                throw new IllegalArgumentException("The maxWidth must be from 1 to 10 inclusive but was " + maxWidth);
            } else if (maxWidth >= width) {
                if (baseDate2 == null) {
                    if (!field.range().isValidValue((long) baseValue2)) {
                        throw new IllegalArgumentException("The base value must be within the range of the field");
                    } else if (((long) baseValue2) + ((long) EXCEED_POINTS[width]) > 2147483647L) {
                        throw new DateTimeException("Unable to add printer-parser as the range exceeds the capacity of an int");
                    }
                }
                this.baseValue = baseValue2;
                this.baseDate = baseDate2;
            } else {
                throw new IllegalArgumentException("The maxWidth must be greater than the width");
            }
        }

        private ReducedPrinterParser(TemporalField field, int minWidth, int maxWidth, int baseValue2, ChronoLocalDate baseDate2, int subsequentWidth) {
            super(field, minWidth, maxWidth, SignStyle.NOT_NEGATIVE, subsequentWidth);
            this.baseValue = baseValue2;
            this.baseDate = baseDate2;
        }

        /* access modifiers changed from: package-private */
        public long getValue(DateTimePrintContext context, long value) {
            long absValue = Math.abs(value);
            int baseValue2 = this.baseValue;
            if (this.baseDate != null) {
                baseValue2 = Chronology.from(context.getTemporal()).date(this.baseDate).get(this.field);
            }
            if (value < ((long) baseValue2) || value >= ((long) (EXCEED_POINTS[this.minWidth] + baseValue2))) {
                return absValue % ((long) EXCEED_POINTS[this.maxWidth]);
            }
            return absValue % ((long) EXCEED_POINTS[this.minWidth]);
        }

        /* access modifiers changed from: package-private */
        public int setValue(DateTimeParseContext context, long value, int errorPos, int successPos) {
            long range;
            long value2;
            int baseValue2 = this.baseValue;
            if (this.baseDate != null) {
                int baseValue3 = context.getEffectiveChronology().date(this.baseDate).get(this.field);
                context.addChronologyChangedParser(this, value, errorPos, successPos);
                baseValue2 = baseValue3;
            }
            if (successPos - errorPos != this.minWidth || value < 0) {
                range = value;
            } else {
                long range2 = (long) EXCEED_POINTS[this.minWidth];
                long basePart = ((long) baseValue2) - (((long) baseValue2) % range2);
                if (baseValue2 > 0) {
                    value2 = basePart + value;
                } else {
                    value2 = basePart - value;
                }
                if (value2 < ((long) baseValue2)) {
                    range = value2 + range2;
                } else {
                    range = value2;
                }
            }
            return context.setParsedField(this.field, range, errorPos, successPos);
        }

        /* access modifiers changed from: package-private */
        public NumberPrinterParser withFixedWidth() {
            if (this.subsequentWidth == -1) {
                return this;
            }
            return new ReducedPrinterParser(this.field, this.minWidth, this.maxWidth, this.baseValue, this.baseDate, -1);
        }

        /* access modifiers changed from: package-private */
        public ReducedPrinterParser withSubsequentWidth(int subsequentWidth) {
            return new ReducedPrinterParser(this.field, this.minWidth, this.maxWidth, this.baseValue, this.baseDate, this.subsequentWidth + subsequentWidth);
        }

        /* access modifiers changed from: package-private */
        public boolean isFixedWidth(DateTimeParseContext context) {
            if (!context.isStrict()) {
                return false;
            }
            return super.isFixedWidth(context);
        }

        public String toString() {
            StringBuilder append = new StringBuilder().append("ReducedValue(").append(this.field).append(",").append(this.minWidth).append(",").append(this.maxWidth).append(",");
            Object obj = this.baseDate;
            if (obj == null) {
                obj = Integer.valueOf(this.baseValue);
            }
            return append.append(obj).append(")").toString();
        }
    }

    static final class FractionPrinterParser implements DateTimePrinterParser {
        private final boolean decimalPoint;
        private final TemporalField field;
        private final int maxWidth;
        private final int minWidth;

        FractionPrinterParser(TemporalField field2, int minWidth2, int maxWidth2, boolean decimalPoint2) {
            Jdk8Methods.requireNonNull(field2, "field");
            if (!field2.range().isFixed()) {
                throw new IllegalArgumentException("Field must have a fixed set of values: " + field2);
            } else if (minWidth2 < 0 || minWidth2 > 9) {
                throw new IllegalArgumentException("Minimum width must be from 0 to 9 inclusive but was " + minWidth2);
            } else if (maxWidth2 < 1 || maxWidth2 > 9) {
                throw new IllegalArgumentException("Maximum width must be from 1 to 9 inclusive but was " + maxWidth2);
            } else if (maxWidth2 >= minWidth2) {
                this.field = field2;
                this.minWidth = minWidth2;
                this.maxWidth = maxWidth2;
                this.decimalPoint = decimalPoint2;
            } else {
                throw new IllegalArgumentException("Maximum width must exceed or equal the minimum width but " + maxWidth2 + " < " + minWidth2);
            }
        }

        public boolean print(DateTimePrintContext context, StringBuilder buf) {
            Long value = context.getValue(this.field);
            if (value == null) {
                return false;
            }
            DecimalStyle symbols = context.getSymbols();
            BigDecimal fraction = convertToFraction(value.longValue());
            if (fraction.scale() != 0) {
                String str = symbols.convertNumberToI18N(fraction.setScale(Math.min(Math.max(fraction.scale(), this.minWidth), this.maxWidth), RoundingMode.FLOOR).toPlainString().substring(2));
                if (this.decimalPoint) {
                    buf.append(symbols.getDecimalSeparator());
                }
                buf.append(str);
                return true;
            } else if (this.minWidth <= 0) {
                return true;
            } else {
                if (this.decimalPoint) {
                    buf.append(symbols.getDecimalSeparator());
                }
                for (int i = 0; i < this.minWidth; i++) {
                    buf.append(symbols.getZeroDigit());
                }
                return true;
            }
        }

        public int parse(DateTimeParseContext context, CharSequence text, int position) {
            int pos;
            int position2 = position;
            int effectiveMin = context.isStrict() ? this.minWidth : 0;
            int effectiveMax = context.isStrict() ? this.maxWidth : 9;
            int length = text.length();
            if (position2 == length) {
                return effectiveMin > 0 ? ~position2 : position2;
            }
            if (this.decimalPoint) {
                if (text.charAt(position) != context.getSymbols().getDecimalSeparator()) {
                    return effectiveMin > 0 ? ~position2 : position2;
                }
                position2++;
            }
            int minEndPos = position2 + effectiveMin;
            if (minEndPos > length) {
                return ~position2;
            }
            int maxEndPos = Math.min(position2 + effectiveMax, length);
            int pos2 = position2;
            int total = 0;
            while (true) {
                if (pos2 >= maxEndPos) {
                    CharSequence charSequence = text;
                    pos = pos2;
                    break;
                }
                int pos3 = pos2 + 1;
                int digit = context.getSymbols().convertToDigit(text.charAt(pos2));
                if (digit >= 0) {
                    total = (total * 10) + digit;
                    pos2 = pos3;
                } else if (pos3 < minEndPos) {
                    return ~position2;
                } else {
                    pos = pos3 - 1;
                }
            }
            BigDecimal fraction = new BigDecimal(total).movePointLeft(pos - position2);
            BigDecimal bigDecimal = fraction;
            return context.setParsedField(this.field, convertFromFraction(fraction), position2, pos);
        }

        private BigDecimal convertToFraction(long value) {
            ValueRange range = this.field.range();
            range.checkValidValue(value, this.field);
            BigDecimal minBD = BigDecimal.valueOf(range.getMinimum());
            BigDecimal fraction = BigDecimal.valueOf(value).subtract(minBD).divide(BigDecimal.valueOf(range.getMaximum()).subtract(minBD).add(BigDecimal.ONE), 9, RoundingMode.FLOOR);
            return fraction.compareTo(BigDecimal.ZERO) == 0 ? BigDecimal.ZERO : fraction.stripTrailingZeros();
        }

        private long convertFromFraction(BigDecimal fraction) {
            ValueRange range = this.field.range();
            BigDecimal minBD = BigDecimal.valueOf(range.getMinimum());
            return fraction.multiply(BigDecimal.valueOf(range.getMaximum()).subtract(minBD).add(BigDecimal.ONE)).setScale(0, RoundingMode.FLOOR).add(minBD).longValueExact();
        }

        public String toString() {
            return "Fraction(" + this.field + "," + this.minWidth + "," + this.maxWidth + (this.decimalPoint ? ",DecimalPoint" : "") + ")";
        }
    }

    static final class TextPrinterParser implements DateTimePrinterParser {
        private final TemporalField field;
        private volatile NumberPrinterParser numberPrinterParser;
        private final DateTimeTextProvider provider;
        private final TextStyle textStyle;

        TextPrinterParser(TemporalField field2, TextStyle textStyle2, DateTimeTextProvider provider2) {
            this.field = field2;
            this.textStyle = textStyle2;
            this.provider = provider2;
        }

        public boolean print(DateTimePrintContext context, StringBuilder buf) {
            Long value = context.getValue(this.field);
            if (value == null) {
                return false;
            }
            String text = this.provider.getText(this.field, value.longValue(), this.textStyle, context.getLocale());
            if (text == null) {
                return numberPrinterParser().print(context, buf);
            }
            buf.append(text);
            return true;
        }

        public int parse(DateTimeParseContext context, CharSequence parseText, int position) {
            int length = parseText.length();
            if (position < 0 || position > length) {
                throw new IndexOutOfBoundsException();
            }
            Iterator<Map.Entry<String, Long>> it = this.provider.getTextIterator(this.field, context.isStrict() ? this.textStyle : null, context.getLocale());
            if (it != null) {
                while (it.hasNext()) {
                    Map.Entry<String, Long> entry = it.next();
                    String itText = entry.getKey();
                    if (context.subSequenceEquals(itText, 0, parseText, position, itText.length())) {
                        return context.setParsedField(this.field, entry.getValue().longValue(), position, position + itText.length());
                    }
                }
                if (context.isStrict()) {
                    return ~position;
                }
            }
            return numberPrinterParser().parse(context, parseText, position);
        }

        private NumberPrinterParser numberPrinterParser() {
            if (this.numberPrinterParser == null) {
                this.numberPrinterParser = new NumberPrinterParser(this.field, 1, 19, SignStyle.NORMAL);
            }
            return this.numberPrinterParser;
        }

        public String toString() {
            if (this.textStyle == TextStyle.FULL) {
                return "Text(" + this.field + ")";
            }
            return "Text(" + this.field + "," + this.textStyle + ")";
        }
    }

    static final class InstantPrinterParser implements DateTimePrinterParser {
        private static final long SECONDS_0000_TO_1970 = 62167219200L;
        private static final long SECONDS_PER_10000_YEARS = 315569520000L;
        private final int fractionalDigits;

        InstantPrinterParser(int fractionalDigits2) {
            this.fractionalDigits = fractionalDigits2;
        }

        public boolean print(DateTimePrintContext context, StringBuilder buf) {
            StringBuilder sb = buf;
            Long inSecs = context.getValue((TemporalField) ChronoField.INSTANT_SECONDS);
            Long inNanos = 0L;
            if (context.getTemporal().isSupported(ChronoField.NANO_OF_SECOND)) {
                inNanos = Long.valueOf(context.getTemporal().getLong(ChronoField.NANO_OF_SECOND));
            }
            if (inSecs == null) {
                return false;
            }
            long inSec = inSecs.longValue();
            int inNano = ChronoField.NANO_OF_SECOND.checkValidIntValue(inNanos.longValue());
            if (inSec >= -62167219200L) {
                long zeroSecs = (inSec - SECONDS_PER_10000_YEARS) + SECONDS_0000_TO_1970;
                long hi = Jdk8Methods.floorDiv(zeroSecs, (long) SECONDS_PER_10000_YEARS) + 1;
                long lo = Jdk8Methods.floorMod(zeroSecs, (long) SECONDS_PER_10000_YEARS);
                Long l = inSecs;
                long j = lo;
                LocalDateTime ldt = LocalDateTime.ofEpochSecond(lo - SECONDS_0000_TO_1970, 0, ZoneOffset.UTC);
                if (hi > 0) {
                    sb.append('+').append(hi);
                }
                sb.append(ldt);
                if (ldt.getSecond() == 0) {
                    sb.append(":00");
                }
            } else {
                long zeroSecs2 = inSec + SECONDS_0000_TO_1970;
                long hi2 = zeroSecs2 / SECONDS_PER_10000_YEARS;
                long lo2 = zeroSecs2 % SECONDS_PER_10000_YEARS;
                long j2 = zeroSecs2;
                LocalDateTime ldt2 = LocalDateTime.ofEpochSecond(lo2 - SECONDS_0000_TO_1970, 0, ZoneOffset.UTC);
                int pos = buf.length();
                sb.append(ldt2);
                if (ldt2.getSecond() == 0) {
                    sb.append(":00");
                }
                if (hi2 < 0) {
                    if (ldt2.getYear() == -10000) {
                        sb.replace(pos, pos + 2, Long.toString(hi2 - 1));
                    } else if (lo2 == 0) {
                        sb.insert(pos, hi2);
                    } else {
                        sb.insert(pos + 1, Math.abs(hi2));
                    }
                }
            }
            int i = this.fractionalDigits;
            if (i == -2) {
                if (inNano != 0) {
                    sb.append('.');
                    if (inNano % DurationKt.NANOS_IN_MILLIS == 0) {
                        sb.append(Integer.toString((inNano / DurationKt.NANOS_IN_MILLIS) + 1000).substring(1));
                    } else if (inNano % 1000 == 0) {
                        sb.append(Integer.toString((inNano / 1000) + DurationKt.NANOS_IN_MILLIS).substring(1));
                    } else {
                        sb.append(Integer.toString(1000000000 + inNano).substring(1));
                    }
                }
            } else if (i > 0 || (i == -1 && inNano > 0)) {
                sb.append('.');
                int div = 100000000;
                int i2 = 0;
                while (true) {
                    int i3 = this.fractionalDigits;
                    if ((i3 != -1 || inNano <= 0) && i2 >= i3) {
                        break;
                    }
                    int digit = inNano / div;
                    sb.append((char) (digit + 48));
                    inNano -= digit * div;
                    div /= 10;
                    i2++;
                }
            }
            sb.append('Z');
            return true;
        }

        public int parse(DateTimeParseContext context, CharSequence text, int position) {
            int sec;
            int hour;
            int sec2;
            LocalDateTime ldt;
            int nano;
            int days;
            int i = position;
            DateTimeParseContext newContext = context.copy();
            int maxDigits = this.fractionalDigits;
            int nano2 = 0;
            int minDigits = maxDigits < 0 ? 0 : maxDigits;
            if (maxDigits < 0) {
                maxDigits = 9;
            }
            int pos = new DateTimeFormatterBuilder().append(DateTimeFormatter.ISO_LOCAL_DATE).appendLiteral('T').appendValue(ChronoField.HOUR_OF_DAY, 2).appendLiteral(':').appendValue(ChronoField.MINUTE_OF_HOUR, 2).appendLiteral(':').appendValue(ChronoField.SECOND_OF_MINUTE, 2).appendFraction(ChronoField.NANO_OF_SECOND, minDigits, maxDigits, true).appendLiteral('Z').toFormatter().toPrinterParser(false).parse(newContext, text, i);
            if (pos < 0) {
                return pos;
            }
            long yearParsed = newContext.getParsed(ChronoField.YEAR).longValue();
            int month = newContext.getParsed(ChronoField.MONTH_OF_YEAR).intValue();
            int day = newContext.getParsed(ChronoField.DAY_OF_MONTH).intValue();
            int hour2 = newContext.getParsed(ChronoField.HOUR_OF_DAY).intValue();
            int min = newContext.getParsed(ChronoField.MINUTE_OF_HOUR).intValue();
            Long secVal = newContext.getParsed(ChronoField.SECOND_OF_MINUTE);
            Long nanoVal = newContext.getParsed(ChronoField.NANO_OF_SECOND);
            int sec3 = secVal != null ? secVal.intValue() : 0;
            if (nanoVal != null) {
                nano2 = nanoVal.intValue();
            }
            int year = ((int) yearParsed) % 10000;
            DateTimeParseContext dateTimeParseContext = newContext;
            if (hour2 == 24 && min == 0 && sec3 == 0 && nano2 == 0) {
                hour = 0;
                sec = sec3;
                sec2 = 1;
            } else if (hour2 == 23 && min == 59 && sec3 == 60) {
                context.setParsedLeapSecond();
                hour = hour2;
                sec = 59;
                sec2 = 0;
            } else {
                hour = hour2;
                sec = sec3;
                sec2 = 0;
            }
            try {
                int i2 = min;
                int i3 = year;
                try {
                    ldt = LocalDateTime.of(year, month, day, hour, min, sec, 0).plusDays((long) sec2);
                    LocalDateTime localDateTime = ldt;
                    nano = nano2;
                    try {
                        long j = yearParsed;
                        days = sec2;
                    } catch (RuntimeException e) {
                        long j2 = yearParsed;
                        int i4 = hour;
                        int hour3 = nano;
                        int nano3 = sec2;
                        return ~i;
                    }
                } catch (RuntimeException e2) {
                    int i5 = sec2;
                    long j3 = yearParsed;
                    int i6 = hour;
                    int hour4 = nano2;
                    return ~i;
                }
                try {
                    int nano4 = nano;
                    DateTimeParseContext dateTimeParseContext2 = context;
                    int i7 = days;
                    int i8 = hour;
                    int nano5 = nano4;
                    int nano6 = position;
                    return dateTimeParseContext2.setParsedField(ChronoField.NANO_OF_SECOND, (long) nano5, nano6, dateTimeParseContext2.setParsedField(ChronoField.INSTANT_SECONDS, ldt.toEpochSecond(ZoneOffset.UTC) + Jdk8Methods.safeMultiply(yearParsed / 10000, (long) SECONDS_PER_10000_YEARS), nano6, pos));
                } catch (RuntimeException e3) {
                    int i9 = hour;
                    int hour5 = nano;
                    int nano7 = days;
                    return ~i;
                }
            } catch (RuntimeException e4) {
                int i10 = sec2;
                long j4 = yearParsed;
                int i11 = min;
                int i12 = year;
                int i13 = hour;
                int hour6 = nano2;
                return ~i;
            }
        }

        public String toString() {
            return "Instant()";
        }
    }

    static final class OffsetIdPrinterParser implements DateTimePrinterParser {
        static final OffsetIdPrinterParser INSTANCE_ID = new OffsetIdPrinterParser("Z", "+HH:MM:ss");
        static final String[] PATTERNS = {"+HH", "+HHmm", "+HH:mm", "+HHMM", "+HH:MM", "+HHMMss", "+HH:MM:ss", "+HHMMSS", "+HH:MM:SS"};
        private final String noOffsetText;
        private final int type;

        OffsetIdPrinterParser(String noOffsetText2, String pattern) {
            Jdk8Methods.requireNonNull(noOffsetText2, "noOffsetText");
            Jdk8Methods.requireNonNull(pattern, "pattern");
            this.noOffsetText = noOffsetText2;
            this.type = checkPattern(pattern);
        }

        private int checkPattern(String pattern) {
            int i = 0;
            while (true) {
                String[] strArr = PATTERNS;
                if (i >= strArr.length) {
                    throw new IllegalArgumentException("Invalid zone offset pattern: " + pattern);
                } else if (strArr[i].equals(pattern)) {
                    return i;
                } else {
                    i++;
                }
            }
        }

        public boolean print(DateTimePrintContext context, StringBuilder buf) {
            Long offsetSecs = context.getValue((TemporalField) ChronoField.OFFSET_SECONDS);
            if (offsetSecs == null) {
                return false;
            }
            int totalSecs = Jdk8Methods.safeToInt(offsetSecs.longValue());
            if (totalSecs == 0) {
                buf.append(this.noOffsetText);
            } else {
                int absHours = Math.abs((totalSecs / 3600) % 100);
                int absMinutes = Math.abs((totalSecs / 60) % 60);
                int absSeconds = Math.abs(totalSecs % 60);
                int bufPos = buf.length();
                int output = absHours;
                buf.append(totalSecs < 0 ? "-" : "+").append((char) ((absHours / 10) + 48)).append((char) ((absHours % 10) + 48));
                int i = this.type;
                if (i >= 3 || (i >= 1 && absMinutes > 0)) {
                    String str = ":";
                    buf.append(i % 2 == 0 ? str : "").append((char) ((absMinutes / 10) + 48)).append((char) ((absMinutes % 10) + 48));
                    output += absMinutes;
                    int i2 = this.type;
                    if (i2 >= 7 || (i2 >= 5 && absSeconds > 0)) {
                        if (i2 % 2 != 0) {
                            str = "";
                        }
                        buf.append(str).append((char) ((absSeconds / 10) + 48)).append((char) ((absSeconds % 10) + 48));
                        output += absSeconds;
                    }
                }
                if (output == 0) {
                    buf.setLength(bufPos);
                    buf.append(this.noOffsetText);
                }
            }
            return true;
        }

        /* JADX WARNING: Removed duplicated region for block: B:32:0x0082  */
        /* Code decompiled incorrectly, please refer to instructions dump. */
        public int parse(org.threeten.bp.format.DateTimeParseContext r19, java.lang.CharSequence r20, int r21) {
            /*
                r18 = this;
                r0 = r18
                r7 = r20
                r8 = r21
                int r9 = r20.length()
                java.lang.String r1 = r0.noOffsetText
                int r10 = r1.length()
                if (r10 != 0) goto L_0x0023
                if (r8 != r9) goto L_0x0046
                org.threeten.bp.temporal.ChronoField r2 = org.threeten.bp.temporal.ChronoField.OFFSET_SECONDS
                r3 = 0
                r1 = r19
                r5 = r21
                r6 = r21
                int r1 = r1.setParsedField(r2, r3, r5, r6)
                return r1
            L_0x0023:
                if (r8 != r9) goto L_0x0027
                int r1 = ~r8
                return r1
            L_0x0027:
                java.lang.String r4 = r0.noOffsetText
                r5 = 0
                r1 = r19
                r2 = r20
                r3 = r21
                r6 = r10
                boolean r1 = r1.subSequenceEquals(r2, r3, r4, r5, r6)
                if (r1 == 0) goto L_0x0046
                org.threeten.bp.temporal.ChronoField r2 = org.threeten.bp.temporal.ChronoField.OFFSET_SECONDS
                r3 = 0
                int r6 = r8 + r10
                r1 = r19
                r5 = r21
                int r1 = r1.setParsedField(r2, r3, r5, r6)
                return r1
            L_0x0046:
                char r11 = r20.charAt(r21)
                r1 = 43
                r2 = 45
                if (r11 == r1) goto L_0x0052
                if (r11 != r2) goto L_0x00a7
            L_0x0052:
                r1 = 1
                if (r11 != r2) goto L_0x0057
                r2 = -1
                goto L_0x0058
            L_0x0057:
                r2 = 1
            L_0x0058:
                r12 = r2
                r2 = 4
                int[] r13 = new int[r2]
                int r2 = r8 + 1
                r3 = 0
                r13[r3] = r2
                boolean r2 = r0.parseNumber(r13, r1, r7, r1)
                r4 = 2
                r5 = 3
                if (r2 != 0) goto L_0x007f
                int r2 = r0.type
                if (r2 < r5) goto L_0x006f
                r2 = 1
                goto L_0x0070
            L_0x006f:
                r2 = 0
            L_0x0070:
                boolean r2 = r0.parseNumber(r13, r4, r7, r2)
                if (r2 != 0) goto L_0x007f
                boolean r2 = r0.parseNumber(r13, r5, r7, r3)
                if (r2 == 0) goto L_0x007d
                goto L_0x007f
            L_0x007d:
                r2 = 0
                goto L_0x0080
            L_0x007f:
                r2 = 1
            L_0x0080:
                if (r2 != 0) goto L_0x00a7
                long r14 = (long) r12
                r1 = r13[r1]
                long r1 = (long) r1
                r16 = 3600(0xe10, double:1.7786E-320)
                long r1 = r1 * r16
                r4 = r13[r4]
                long r3 = (long) r4
                r16 = 60
                long r3 = r3 * r16
                long r1 = r1 + r3
                r3 = r13[r5]
                long r3 = (long) r3
                long r1 = r1 + r3
                long r14 = r14 * r1
                org.threeten.bp.temporal.ChronoField r2 = org.threeten.bp.temporal.ChronoField.OFFSET_SECONDS
                r1 = 0
                r6 = r13[r1]
                r1 = r19
                r3 = r14
                r5 = r21
                int r1 = r1.setParsedField(r2, r3, r5, r6)
                return r1
            L_0x00a7:
                if (r10 != 0) goto L_0x00b8
                org.threeten.bp.temporal.ChronoField r2 = org.threeten.bp.temporal.ChronoField.OFFSET_SECONDS
                r3 = 0
                int r6 = r8 + r10
                r1 = r19
                r5 = r21
                int r1 = r1.setParsedField(r2, r3, r5, r6)
                return r1
            L_0x00b8:
                int r1 = ~r8
                return r1
            */
            throw new UnsupportedOperationException("Method not decompiled: org.threeten.bp.format.DateTimeFormatterBuilder.OffsetIdPrinterParser.parse(org.threeten.bp.format.DateTimeParseContext, java.lang.CharSequence, int):int");
        }

        private boolean parseNumber(int[] array, int arrayIndex, CharSequence parseText, boolean required) {
            int value;
            int i = this.type;
            if ((i + 3) / 2 < arrayIndex) {
                return false;
            }
            int pos = array[0];
            if (i % 2 == 0 && arrayIndex > 1) {
                if (pos + 1 > parseText.length() || parseText.charAt(pos) != ':') {
                    return required;
                }
                pos++;
            }
            if (pos + 2 > parseText.length()) {
                return required;
            }
            int pos2 = pos + 1;
            char ch1 = parseText.charAt(pos);
            int pos3 = pos2 + 1;
            int pos4 = parseText.charAt(pos2);
            if (ch1 < '0' || ch1 > '9' || pos4 < 48 || pos4 > 57 || (value = ((ch1 - '0') * 10) + (pos4 - 48)) < 0 || value > 59) {
                return required;
            }
            array[arrayIndex] = value;
            array[0] = pos3;
            return false;
        }

        public String toString() {
            return "Offset(" + PATTERNS[this.type] + ",'" + this.noOffsetText.replace("'", "''") + "')";
        }
    }

    static final class LocalizedOffsetPrinterParser implements DateTimePrinterParser {
        private final TextStyle style;

        public LocalizedOffsetPrinterParser(TextStyle style2) {
            this.style = style2;
        }

        public boolean print(DateTimePrintContext context, StringBuilder buf) {
            Long offsetSecs = context.getValue((TemporalField) ChronoField.OFFSET_SECONDS);
            if (offsetSecs == null) {
                return false;
            }
            buf.append("GMT");
            if (this.style == TextStyle.FULL) {
                return new OffsetIdPrinterParser("", "+HH:MM:ss").print(context, buf);
            }
            int totalSecs = Jdk8Methods.safeToInt(offsetSecs.longValue());
            if (totalSecs == 0) {
                return true;
            }
            int absHours = Math.abs((totalSecs / 3600) % 100);
            int absMinutes = Math.abs((totalSecs / 60) % 60);
            int absSeconds = Math.abs(totalSecs % 60);
            buf.append(totalSecs < 0 ? "-" : "+").append(absHours);
            if (absMinutes <= 0 && absSeconds <= 0) {
                return true;
            }
            buf.append(":").append((char) ((absMinutes / 10) + 48)).append((char) ((absMinutes % 10) + 48));
            if (absSeconds <= 0) {
                return true;
            }
            buf.append(":").append((char) ((absSeconds / 10) + 48)).append((char) ((absSeconds % 10) + 48));
            return true;
        }

        public int parse(DateTimeParseContext context, CharSequence text, int position) {
            int hour;
            int min;
            CharSequence charSequence = text;
            int i = position;
            if (!context.subSequenceEquals(text, position, "GMT", 0, 3)) {
                return ~i;
            }
            int position2 = i + 3;
            if (this.style == TextStyle.FULL) {
                return new OffsetIdPrinterParser("", "+HH:MM:ss").parse(context, charSequence, position2);
            }
            DateTimeParseContext dateTimeParseContext = context;
            int end = text.length();
            if (position2 == end) {
                return context.setParsedField(ChronoField.OFFSET_SECONDS, 0, position2, position2);
            }
            char sign = charSequence.charAt(position2);
            if (sign == '+' || sign == '-') {
                int negative = sign == '-' ? -1 : 1;
                if (position2 == end) {
                    return ~position2;
                }
                int position3 = position2 + 1;
                char ch = charSequence.charAt(position3);
                if (ch < '0' || ch > '9') {
                    return ~position3;
                }
                int position4 = position3 + 1;
                int hour2 = ch - '0';
                if (position4 != end) {
                    char ch2 = charSequence.charAt(position4);
                    if (ch2 < '0' || ch2 > '9') {
                        char c = ch2;
                        hour = hour2;
                    } else {
                        int hour3 = (hour2 * 10) + (ch2 - '0');
                        if (hour3 > 23) {
                            return ~position4;
                        }
                        position4++;
                        char c2 = ch2;
                        hour = hour3;
                    }
                } else {
                    char c3 = ch;
                    hour = hour2;
                }
                if (position4 == end || charSequence.charAt(position4) != ':') {
                    return context.setParsedField(ChronoField.OFFSET_SECONDS, (long) (negative * 3600 * hour), position4, position4);
                }
                int position5 = position4 + 1;
                if (position5 > end - 2) {
                    return ~position5;
                }
                char ch3 = charSequence.charAt(position5);
                if (ch3 < '0' || ch3 > '9') {
                    return ~position5;
                }
                int position6 = position5 + 1;
                int min2 = ch3 - '0';
                char ch4 = charSequence.charAt(position6);
                if (ch4 < '0' || ch4 > '9') {
                    return ~position6;
                }
                int position7 = position6 + 1;
                int min3 = (ch4 - '0') + (min2 * 10);
                if (min3 > 59) {
                    return ~position7;
                }
                if (position7 == end) {
                    min = min3;
                } else if (charSequence.charAt(position7) != ':') {
                    min = min3;
                } else {
                    int position8 = position7 + 1;
                    if (position8 > end - 2) {
                        return ~position8;
                    }
                    char ch5 = charSequence.charAt(position8);
                    if (ch5 < '0') {
                    } else if (ch5 > '9') {
                        int i2 = min3;
                    } else {
                        int position9 = position8 + 1;
                        int sec = ch5 - '0';
                        char ch6 = charSequence.charAt(position9);
                        if (ch6 < '0') {
                        } else if (ch6 > '9') {
                            int i3 = min3;
                        } else {
                            int position10 = position9 + 1;
                            int sec2 = (sec * 10) + (ch6 - '0');
                            if (sec2 > 59) {
                                return ~position10;
                            }
                            int offset = negative * ((hour * 3600) + (min3 * 60) + sec2);
                            int i4 = offset;
                            int i5 = min3;
                            return context.setParsedField(ChronoField.OFFSET_SECONDS, (long) offset, position10, position10);
                        }
                        return ~position9;
                    }
                    return ~position8;
                }
                return context.setParsedField(ChronoField.OFFSET_SECONDS, (long) (((hour * 3600) + (min * 60)) * negative), position7, position7);
            }
            return context.setParsedField(ChronoField.OFFSET_SECONDS, 0, position2, position2);
        }
    }

    static final class ZoneTextPrinterParser implements DateTimePrinterParser {
        private static final Comparator<String> LENGTH_COMPARATOR = new Comparator<String>() {
            public int compare(String str1, String str2) {
                int cmp = str2.length() - str1.length();
                if (cmp == 0) {
                    return str1.compareTo(str2);
                }
                return cmp;
            }
        };
        private final TextStyle textStyle;

        ZoneTextPrinterParser(TextStyle textStyle2) {
            this.textStyle = (TextStyle) Jdk8Methods.requireNonNull(textStyle2, "textStyle");
        }

        public boolean print(DateTimePrintContext context, StringBuilder buf) {
            ZoneId zone = (ZoneId) context.getValue(TemporalQueries.zoneId());
            int tzstyle = 0;
            if (zone == null) {
                return false;
            }
            if (zone.normalized() instanceof ZoneOffset) {
                buf.append(zone.getId());
                return true;
            }
            TemporalAccessor temporal = context.getTemporal();
            boolean daylight = false;
            if (temporal.isSupported(ChronoField.INSTANT_SECONDS)) {
                daylight = zone.getRules().isDaylightSavings(Instant.ofEpochSecond(temporal.getLong(ChronoField.INSTANT_SECONDS)));
            }
            TimeZone tz = TimeZone.getTimeZone(zone.getId());
            if (this.textStyle.asNormal() == TextStyle.FULL) {
                tzstyle = 1;
            }
            buf.append(tz.getDisplayName(daylight, tzstyle, context.getLocale()));
            return true;
        }

        public int parse(DateTimeParseContext context, CharSequence text, int position) {
            Map<String, String> ids = new TreeMap<>(LENGTH_COMPARATOR);
            for (String id : ZoneId.getAvailableZoneIds()) {
                ids.put(id, id);
                TimeZone tz = TimeZone.getTimeZone(id);
                int tzstyle = this.textStyle.asNormal() == TextStyle.FULL ? 1 : 0;
                ids.put(tz.getDisplayName(false, tzstyle, context.getLocale()), id);
                ids.put(tz.getDisplayName(true, tzstyle, context.getLocale()), id);
            }
            for (Map.Entry<String, String> entry : ids.entrySet()) {
                String name = entry.getKey();
                if (context.subSequenceEquals(text, position, name, 0, name.length())) {
                    context.setParsed(ZoneId.of(entry.getValue()));
                    return name.length() + position;
                }
            }
            return ~position;
        }

        public String toString() {
            return "ZoneText(" + this.textStyle + ")";
        }
    }

    static final class ZoneIdPrinterParser implements DateTimePrinterParser {
        private static volatile Map.Entry<Integer, SubstringTree> cachedSubstringTree;
        private final String description;
        private final TemporalQuery<ZoneId> query;

        ZoneIdPrinterParser(TemporalQuery<ZoneId> query2, String description2) {
            this.query = query2;
            this.description = description2;
        }

        public boolean print(DateTimePrintContext context, StringBuilder buf) {
            ZoneId zone = (ZoneId) context.getValue(this.query);
            if (zone == null) {
                return false;
            }
            buf.append(zone.getId());
            return true;
        }

        public int parse(DateTimeParseContext context, CharSequence text, int position) {
            int length = text.length();
            if (position > length) {
                throw new IndexOutOfBoundsException();
            } else if (position == length) {
                return ~position;
            } else {
                char nextChar = text.charAt(position);
                if (nextChar == '+' || nextChar == '-') {
                    DateTimeParseContext newContext = context.copy();
                    int endPos = OffsetIdPrinterParser.INSTANCE_ID.parse(newContext, text, position);
                    if (endPos < 0) {
                        return endPos;
                    }
                    context.setParsed(ZoneOffset.ofTotalSeconds((int) newContext.getParsed(ChronoField.OFFSET_SECONDS).longValue()));
                    return endPos;
                }
                if (length >= position + 2) {
                    char nextNextChar = text.charAt(position + 1);
                    if (!context.charEquals(nextChar, 'U') || !context.charEquals(nextNextChar, 'T')) {
                        if (context.charEquals(nextChar, 'G') && length >= position + 3 && context.charEquals(nextNextChar, 'M') && context.charEquals(text.charAt(position + 2), 'T')) {
                            return parsePrefixedOffset(context, text, position, position + 3);
                        }
                    } else if (length < position + 3 || !context.charEquals(text.charAt(position + 2), 'C')) {
                        return parsePrefixedOffset(context, text, position, position + 2);
                    } else {
                        return parsePrefixedOffset(context, text, position, position + 3);
                    }
                }
                Set<String> regionIds = ZoneRulesProvider.getAvailableZoneIds();
                int regionIdsSize = regionIds.size();
                Map.Entry<Integer, SubstringTree> cached = cachedSubstringTree;
                if (cached == null || cached.getKey().intValue() != regionIdsSize) {
                    synchronized (this) {
                        cached = cachedSubstringTree;
                        if (cached == null || cached.getKey().intValue() != regionIdsSize) {
                            AbstractMap.SimpleImmutableEntry simpleImmutableEntry = new AbstractMap.SimpleImmutableEntry(Integer.valueOf(regionIdsSize), prepareParser(regionIds));
                            cached = simpleImmutableEntry;
                            cachedSubstringTree = simpleImmutableEntry;
                        }
                    }
                }
                SubstringTree tree = cached.getValue();
                String parsedZoneId = null;
                String lastZoneId = null;
                while (tree != null) {
                    int nodeLength = tree.length;
                    if (position + nodeLength > length) {
                        break;
                    }
                    lastZoneId = parsedZoneId;
                    parsedZoneId = text.subSequence(position, position + nodeLength).toString();
                    tree = tree.get(parsedZoneId, context.isCaseSensitive());
                }
                ZoneId zone = convertToZone(regionIds, parsedZoneId, context.isCaseSensitive());
                if (zone == null) {
                    zone = convertToZone(regionIds, lastZoneId, context.isCaseSensitive());
                    if (zone != null) {
                        parsedZoneId = lastZoneId;
                    } else if (!context.charEquals(nextChar, 'Z')) {
                        return ~position;
                    } else {
                        context.setParsed((ZoneId) ZoneOffset.UTC);
                        return position + 1;
                    }
                }
                context.setParsed(zone);
                return parsedZoneId.length() + position;
            }
        }

        private ZoneId convertToZone(Set<String> regionIds, String parsedZoneId, boolean caseSensitive) {
            if (parsedZoneId == null) {
                return null;
            }
            if (!caseSensitive) {
                for (String regionId : regionIds) {
                    if (regionId.equalsIgnoreCase(parsedZoneId)) {
                        return ZoneId.of(regionId);
                    }
                }
                return null;
            } else if (regionIds.contains(parsedZoneId)) {
                return ZoneId.of(parsedZoneId);
            } else {
                return null;
            }
        }

        private int parsePrefixedOffset(DateTimeParseContext context, CharSequence text, int prefixPos, int position) {
            String prefix = text.subSequence(prefixPos, position).toString().toUpperCase();
            DateTimeParseContext newContext = context.copy();
            if (position >= text.length() || !context.charEquals(text.charAt(position), 'Z')) {
                int endPos = OffsetIdPrinterParser.INSTANCE_ID.parse(newContext, text, position);
                if (endPos < 0) {
                    context.setParsed(ZoneId.ofOffset(prefix, ZoneOffset.UTC));
                    return position;
                }
                context.setParsed(ZoneId.ofOffset(prefix, ZoneOffset.ofTotalSeconds((int) newContext.getParsed(ChronoField.OFFSET_SECONDS).longValue())));
                return endPos;
            }
            context.setParsed(ZoneId.ofOffset(prefix, ZoneOffset.UTC));
            return position;
        }

        private static final class SubstringTree {
            final int length;
            private final Map<CharSequence, SubstringTree> substringMap;
            private final Map<String, SubstringTree> substringMapCI;

            private SubstringTree(int length2) {
                this.substringMap = new HashMap();
                this.substringMapCI = new HashMap();
                this.length = length2;
            }

            /* access modifiers changed from: private */
            public SubstringTree get(CharSequence substring2, boolean caseSensitive) {
                if (caseSensitive) {
                    return this.substringMap.get(substring2);
                }
                return this.substringMapCI.get(substring2.toString().toLowerCase(Locale.ENGLISH));
            }

            /* access modifiers changed from: private */
            public void add(String newSubstring) {
                int idLen = newSubstring.length();
                int i = this.length;
                if (idLen == i) {
                    this.substringMap.put(newSubstring, (Object) null);
                    this.substringMapCI.put(newSubstring.toLowerCase(Locale.ENGLISH), (Object) null);
                } else if (idLen > i) {
                    String substring = newSubstring.substring(0, i);
                    SubstringTree parserTree = this.substringMap.get(substring);
                    if (parserTree == null) {
                        parserTree = new SubstringTree(idLen);
                        this.substringMap.put(substring, parserTree);
                        this.substringMapCI.put(substring.toLowerCase(Locale.ENGLISH), parserTree);
                    }
                    parserTree.add(newSubstring);
                }
            }
        }

        private static SubstringTree prepareParser(Set<String> availableIDs) {
            List<String> ids = new ArrayList<>(availableIDs);
            Collections.sort(ids, DateTimeFormatterBuilder.LENGTH_SORT);
            SubstringTree tree = new SubstringTree(ids.get(0).length());
            for (String id : ids) {
                tree.add(id);
            }
            return tree;
        }

        public String toString() {
            return this.description;
        }
    }

    static final class ChronoPrinterParser implements DateTimePrinterParser {
        private final TextStyle textStyle;

        ChronoPrinterParser(TextStyle textStyle2) {
            this.textStyle = textStyle2;
        }

        public boolean print(DateTimePrintContext context, StringBuilder buf) {
            Chronology chrono = (Chronology) context.getValue(TemporalQueries.chronology());
            if (chrono == null) {
                return false;
            }
            if (this.textStyle == null) {
                buf.append(chrono.getId());
                return true;
            }
            try {
                buf.append(ResourceBundle.getBundle("org.threeten.bp.format.ChronologyText", context.getLocale(), DateTimeFormatterBuilder.class.getClassLoader()).getString(chrono.getId()));
                return true;
            } catch (MissingResourceException e) {
                buf.append(chrono.getId());
                return true;
            }
        }

        public int parse(DateTimeParseContext context, CharSequence text, int position) {
            int i = position;
            if (i < 0 || i > text.length()) {
                DateTimeParseContext dateTimeParseContext = context;
                throw new IndexOutOfBoundsException();
            }
            Chronology bestMatch = null;
            int matchLen = -1;
            for (Chronology chrono : Chronology.getAvailableChronologies()) {
                String id = chrono.getId();
                int idLen = id.length();
                if (idLen > matchLen && context.subSequenceEquals(text, position, id, 0, idLen)) {
                    bestMatch = chrono;
                    matchLen = idLen;
                }
            }
            if (bestMatch == null) {
                return ~i;
            }
            DateTimeParseContext dateTimeParseContext2 = context;
            context.setParsed(bestMatch);
            return i + matchLen;
        }
    }

    static final class LocalizedPrinterParser implements DateTimePrinterParser {
        private final FormatStyle dateStyle;
        private final FormatStyle timeStyle;

        LocalizedPrinterParser(FormatStyle dateStyle2, FormatStyle timeStyle2) {
            this.dateStyle = dateStyle2;
            this.timeStyle = timeStyle2;
        }

        public boolean print(DateTimePrintContext context, StringBuilder buf) {
            return formatter(context.getLocale(), Chronology.from(context.getTemporal())).toPrinterParser(false).print(context, buf);
        }

        public int parse(DateTimeParseContext context, CharSequence text, int position) {
            return formatter(context.getLocale(), context.getEffectiveChronology()).toPrinterParser(false).parse(context, text, position);
        }

        private DateTimeFormatter formatter(Locale locale, Chronology chrono) {
            return DateTimeFormatStyleProvider.getInstance().getFormatter(this.dateStyle, this.timeStyle, chrono, locale);
        }

        public String toString() {
            StringBuilder append = new StringBuilder().append("Localized(");
            Object obj = this.dateStyle;
            Object obj2 = "";
            if (obj == null) {
                obj = obj2;
            }
            StringBuilder append2 = append.append(obj).append(",");
            Object obj3 = this.timeStyle;
            if (obj3 != null) {
                obj2 = obj3;
            }
            return append2.append(obj2).append(")").toString();
        }
    }

    static final class WeekFieldsPrinterParser implements DateTimePrinterParser {
        private final int count;
        private final char letter;

        public WeekFieldsPrinterParser(char letter2, int count2) {
            this.letter = letter2;
            this.count = count2;
        }

        public boolean print(DateTimePrintContext context, StringBuilder buf) {
            return evaluate(WeekFields.of(context.getLocale())).print(context, buf);
        }

        public int parse(DateTimeParseContext context, CharSequence text, int position) {
            return evaluate(WeekFields.of(context.getLocale())).parse(context, text, position);
        }

        private DateTimePrinterParser evaluate(WeekFields weekFields) {
            switch (this.letter) {
                case 'W':
                    return new NumberPrinterParser(weekFields.weekOfMonth(), 1, 2, SignStyle.NOT_NEGATIVE);
                case 'Y':
                    if (this.count == 2) {
                        return new ReducedPrinterParser(weekFields.weekBasedYear(), 2, 2, 0, ReducedPrinterParser.BASE_DATE);
                    }
                    TemporalField weekBasedYear = weekFields.weekBasedYear();
                    int i = this.count;
                    return new NumberPrinterParser(weekBasedYear, i, 19, i < 4 ? SignStyle.NORMAL : SignStyle.EXCEEDS_PAD, -1);
                case 'c':
                    return new NumberPrinterParser(weekFields.dayOfWeek(), this.count, 2, SignStyle.NOT_NEGATIVE);
                case 'e':
                    return new NumberPrinterParser(weekFields.dayOfWeek(), this.count, 2, SignStyle.NOT_NEGATIVE);
                case 'w':
                    return new NumberPrinterParser(weekFields.weekOfWeekBasedYear(), this.count, 2, SignStyle.NOT_NEGATIVE);
                default:
                    return null;
            }
        }

        public String toString() {
            StringBuilder sb = new StringBuilder(30);
            sb.append("Localized(");
            char c = this.letter;
            if (c == 'Y') {
                int i = this.count;
                if (i == 1) {
                    sb.append("WeekBasedYear");
                } else if (i == 2) {
                    sb.append("ReducedValue(WeekBasedYear,2,2,2000-01-01)");
                } else {
                    sb.append("WeekBasedYear,").append(this.count).append(",").append(19).append(",").append(this.count < 4 ? SignStyle.NORMAL : SignStyle.EXCEEDS_PAD);
                }
            } else {
                if (c == 'c' || c == 'e') {
                    sb.append("DayOfWeek");
                } else if (c == 'w') {
                    sb.append("WeekOfWeekBasedYear");
                } else if (c == 'W') {
                    sb.append("WeekOfMonth");
                }
                sb.append(",");
                sb.append(this.count);
            }
            sb.append(")");
            return sb.toString();
        }
    }
}
