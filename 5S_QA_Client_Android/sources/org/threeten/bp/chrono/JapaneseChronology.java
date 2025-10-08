package org.threeten.bp.chrono;

import java.io.Serializable;
import java.util.Arrays;
import java.util.Calendar;
import java.util.HashMap;
import java.util.List;
import java.util.Locale;
import java.util.Map;
import org.threeten.bp.Clock;
import org.threeten.bp.DateTimeException;
import org.threeten.bp.Instant;
import org.threeten.bp.LocalDate;
import org.threeten.bp.ZoneId;
import org.threeten.bp.format.ResolverStyle;
import org.threeten.bp.jdk8.Jdk8Methods;
import org.threeten.bp.temporal.ChronoField;
import org.threeten.bp.temporal.ChronoUnit;
import org.threeten.bp.temporal.TemporalAccessor;
import org.threeten.bp.temporal.TemporalField;
import org.threeten.bp.temporal.TemporalUnit;
import org.threeten.bp.temporal.ValueRange;

public final class JapaneseChronology extends Chronology implements Serializable {
    private static final Map<String, String[]> ERA_FULL_NAMES;
    private static final Map<String, String[]> ERA_NARROW_NAMES;
    private static final Map<String, String[]> ERA_SHORT_NAMES;
    private static final String FALLBACK_LANGUAGE = "en";
    public static final JapaneseChronology INSTANCE = new JapaneseChronology();
    static final Locale LOCALE = new Locale(TARGET_LANGUAGE, "JP", "JP");
    private static final String TARGET_LANGUAGE = "ja";
    private static final long serialVersionUID = 459996390165777884L;

    static {
        HashMap hashMap = new HashMap();
        ERA_NARROW_NAMES = hashMap;
        HashMap hashMap2 = new HashMap();
        ERA_SHORT_NAMES = hashMap2;
        HashMap hashMap3 = new HashMap();
        ERA_FULL_NAMES = hashMap3;
        hashMap.put(FALLBACK_LANGUAGE, new String[]{"Unknown", "K", "M", "T", "S", "H"});
        hashMap.put(TARGET_LANGUAGE, new String[]{"Unknown", "K", "M", "T", "S", "H"});
        hashMap2.put(FALLBACK_LANGUAGE, new String[]{"Unknown", "K", "M", "T", "S", "H"});
        hashMap2.put(TARGET_LANGUAGE, new String[]{"Unknown", "慶", "明", "大", "昭", "平"});
        hashMap3.put(FALLBACK_LANGUAGE, new String[]{"Unknown", "Keio", "Meiji", "Taisho", "Showa", "Heisei"});
        hashMap3.put(TARGET_LANGUAGE, new String[]{"Unknown", "慶応", "明治", "大正", "昭和", "平成"});
    }

    private JapaneseChronology() {
    }

    private Object readResolve() {
        return INSTANCE;
    }

    public String getId() {
        return "Japanese";
    }

    public String getCalendarType() {
        return "japanese";
    }

    public JapaneseDate date(Era era, int yearOfEra, int month, int dayOfMonth) {
        if (era instanceof JapaneseEra) {
            return JapaneseDate.of((JapaneseEra) era, yearOfEra, month, dayOfMonth);
        }
        throw new ClassCastException("Era must be JapaneseEra");
    }

    public JapaneseDate date(int prolepticYear, int month, int dayOfMonth) {
        return new JapaneseDate(LocalDate.of(prolepticYear, month, dayOfMonth));
    }

    public JapaneseDate dateYearDay(Era era, int yearOfEra, int dayOfYear) {
        if (era instanceof JapaneseEra) {
            return JapaneseDate.ofYearDay((JapaneseEra) era, yearOfEra, dayOfYear);
        }
        throw new ClassCastException("Era must be JapaneseEra");
    }

    public JapaneseDate dateYearDay(int prolepticYear, int dayOfYear) {
        LocalDate date = LocalDate.ofYearDay(prolepticYear, dayOfYear);
        return date(prolepticYear, date.getMonthValue(), date.getDayOfMonth());
    }

    public JapaneseDate dateEpochDay(long epochDay) {
        return new JapaneseDate(LocalDate.ofEpochDay(epochDay));
    }

    public JapaneseDate date(TemporalAccessor temporal) {
        if (temporal instanceof JapaneseDate) {
            return (JapaneseDate) temporal;
        }
        return new JapaneseDate(LocalDate.from(temporal));
    }

    public ChronoLocalDateTime<JapaneseDate> localDateTime(TemporalAccessor temporal) {
        return super.localDateTime(temporal);
    }

    public ChronoZonedDateTime<JapaneseDate> zonedDateTime(TemporalAccessor temporal) {
        return super.zonedDateTime(temporal);
    }

    public ChronoZonedDateTime<JapaneseDate> zonedDateTime(Instant instant, ZoneId zone) {
        return super.zonedDateTime(instant, zone);
    }

    public JapaneseDate dateNow() {
        return (JapaneseDate) super.dateNow();
    }

    public JapaneseDate dateNow(ZoneId zone) {
        return (JapaneseDate) super.dateNow(zone);
    }

    public JapaneseDate dateNow(Clock clock) {
        Jdk8Methods.requireNonNull(clock, "clock");
        return (JapaneseDate) super.dateNow(clock);
    }

    public boolean isLeapYear(long prolepticYear) {
        return IsoChronology.INSTANCE.isLeapYear(prolepticYear);
    }

    public int prolepticYear(Era era, int yearOfEra) {
        if (era instanceof JapaneseEra) {
            JapaneseEra jera = (JapaneseEra) era;
            int isoYear = (jera.startDate().getYear() + yearOfEra) - 1;
            ValueRange.of(1, (long) ((jera.endDate().getYear() - jera.startDate().getYear()) + 1)).checkValidValue((long) yearOfEra, ChronoField.YEAR_OF_ERA);
            return isoYear;
        }
        throw new ClassCastException("Era must be JapaneseEra");
    }

    public JapaneseEra eraOf(int eraValue) {
        return JapaneseEra.of(eraValue);
    }

    public List<Era> eras() {
        return Arrays.asList(JapaneseEra.values());
    }

    public ValueRange range(ChronoField field) {
        switch (AnonymousClass1.$SwitchMap$org$threeten$bp$temporal$ChronoField[field.ordinal()]) {
            case 1:
            case 2:
            case 3:
            case 4:
            case 5:
            case 6:
            case 7:
            case 8:
            case 9:
            case 10:
            case 11:
            case 12:
            case 13:
            case 14:
            case 15:
            case 16:
            case 17:
            case 18:
                return field.range();
            default:
                Calendar jcal = Calendar.getInstance(LOCALE);
                switch (field) {
                    case ERA:
                        JapaneseEra[] eras = JapaneseEra.values();
                        return ValueRange.of((long) eras[0].getValue(), (long) eras[eras.length - 1].getValue());
                    case YEAR:
                        JapaneseEra[] eras2 = JapaneseEra.values();
                        return ValueRange.of((long) JapaneseDate.MIN_DATE.getYear(), (long) eras2[eras2.length - 1].endDate().getYear());
                    case YEAR_OF_ERA:
                        JapaneseEra[] eras3 = JapaneseEra.values();
                        int maxJapanese = (eras3[eras3.length - 1].endDate().getYear() - eras3[eras3.length - 1].startDate().getYear()) + 1;
                        int min = Integer.MAX_VALUE;
                        for (int i = 0; i < eras3.length; i++) {
                            min = Math.min(min, (eras3[i].endDate().getYear() - eras3[i].startDate().getYear()) + 1);
                        }
                        return ValueRange.of(1, 6, (long) min, (long) maxJapanese);
                    case MONTH_OF_YEAR:
                        return ValueRange.of((long) (jcal.getMinimum(2) + 1), (long) (jcal.getGreatestMinimum(2) + 1), (long) (jcal.getLeastMaximum(2) + 1), (long) (jcal.getMaximum(2) + 1));
                    case DAY_OF_YEAR:
                        JapaneseEra[] eras4 = JapaneseEra.values();
                        int min2 = 366;
                        for (int i2 = 0; i2 < eras4.length; i2++) {
                            min2 = Math.min(min2, (eras4[i2].startDate().lengthOfYear() - eras4[i2].startDate().getDayOfYear()) + 1);
                        }
                        return ValueRange.of(1, (long) min2, 366);
                    default:
                        throw new UnsupportedOperationException("Unimplementable field: " + field);
                }
        }
    }

    /* JADX DEBUG: Multi-variable search result rejected for TypeSearchVarInfo{r10v49, resolved type: java.lang.Object} */
    /* JADX DEBUG: Multi-variable search result rejected for TypeSearchVarInfo{r6v3, resolved type: org.threeten.bp.chrono.JapaneseEra} */
    /* JADX WARNING: Multi-variable type inference failed */
    /* Code decompiled incorrectly, please refer to instructions dump. */
    public org.threeten.bp.chrono.JapaneseDate resolveDate(java.util.Map<org.threeten.bp.temporal.TemporalField, java.lang.Long> r18, org.threeten.bp.format.ResolverStyle r19) {
        /*
            r17 = this;
            r0 = r17
            r1 = r18
            r2 = r19
            org.threeten.bp.temporal.ChronoField r3 = org.threeten.bp.temporal.ChronoField.EPOCH_DAY
            boolean r3 = r1.containsKey(r3)
            if (r3 == 0) goto L_0x001f
            org.threeten.bp.temporal.ChronoField r3 = org.threeten.bp.temporal.ChronoField.EPOCH_DAY
            java.lang.Object r3 = r1.remove(r3)
            java.lang.Long r3 = (java.lang.Long) r3
            long r3 = r3.longValue()
            org.threeten.bp.chrono.JapaneseDate r3 = r0.dateEpochDay((long) r3)
            return r3
        L_0x001f:
            org.threeten.bp.temporal.ChronoField r3 = org.threeten.bp.temporal.ChronoField.PROLEPTIC_MONTH
            java.lang.Object r3 = r1.remove(r3)
            java.lang.Long r3 = (java.lang.Long) r3
            r4 = 1
            if (r3 == 0) goto L_0x0057
            org.threeten.bp.format.ResolverStyle r5 = org.threeten.bp.format.ResolverStyle.LENIENT
            if (r2 == r5) goto L_0x0037
            org.threeten.bp.temporal.ChronoField r5 = org.threeten.bp.temporal.ChronoField.PROLEPTIC_MONTH
            long r6 = r3.longValue()
            r5.checkValidValue(r6)
        L_0x0037:
            org.threeten.bp.temporal.ChronoField r5 = org.threeten.bp.temporal.ChronoField.MONTH_OF_YEAR
            long r6 = r3.longValue()
            r8 = 12
            int r6 = org.threeten.bp.jdk8.Jdk8Methods.floorMod((long) r6, (int) r8)
            int r6 = r6 + r4
            long r6 = (long) r6
            r0.updateResolveMap(r1, r5, r6)
            org.threeten.bp.temporal.ChronoField r5 = org.threeten.bp.temporal.ChronoField.YEAR
            long r6 = r3.longValue()
            r8 = 12
            long r6 = org.threeten.bp.jdk8.Jdk8Methods.floorDiv((long) r6, (long) r8)
            r0.updateResolveMap(r1, r5, r6)
        L_0x0057:
            org.threeten.bp.temporal.ChronoField r5 = org.threeten.bp.temporal.ChronoField.ERA
            java.lang.Object r5 = r1.get(r5)
            java.lang.Long r5 = (java.lang.Long) r5
            r6 = 0
            if (r5 == 0) goto L_0x0076
            org.threeten.bp.temporal.ChronoField r7 = org.threeten.bp.temporal.ChronoField.ERA
            org.threeten.bp.temporal.ValueRange r7 = r0.range(r7)
            long r8 = r5.longValue()
            org.threeten.bp.temporal.ChronoField r10 = org.threeten.bp.temporal.ChronoField.ERA
            int r7 = r7.checkValidIntValue(r8, r10)
            org.threeten.bp.chrono.JapaneseEra r6 = r0.eraOf((int) r7)
        L_0x0076:
            org.threeten.bp.temporal.ChronoField r7 = org.threeten.bp.temporal.ChronoField.YEAR_OF_ERA
            java.lang.Object r7 = r1.get(r7)
            java.lang.Long r7 = (java.lang.Long) r7
            if (r7 == 0) goto L_0x00e8
            org.threeten.bp.temporal.ChronoField r8 = org.threeten.bp.temporal.ChronoField.YEAR_OF_ERA
            org.threeten.bp.temporal.ValueRange r8 = r0.range(r8)
            long r9 = r7.longValue()
            org.threeten.bp.temporal.ChronoField r11 = org.threeten.bp.temporal.ChronoField.YEAR_OF_ERA
            int r8 = r8.checkValidIntValue(r9, r11)
            if (r6 != 0) goto L_0x00ae
            org.threeten.bp.format.ResolverStyle r9 = org.threeten.bp.format.ResolverStyle.STRICT
            if (r2 == r9) goto L_0x00ae
            org.threeten.bp.temporal.ChronoField r9 = org.threeten.bp.temporal.ChronoField.YEAR
            boolean r9 = r1.containsKey(r9)
            if (r9 != 0) goto L_0x00ae
            java.util.List r9 = r17.eras()
            int r10 = r9.size()
            int r10 = r10 - r4
            java.lang.Object r10 = r9.get(r10)
            r6 = r10
            org.threeten.bp.chrono.JapaneseEra r6 = (org.threeten.bp.chrono.JapaneseEra) r6
        L_0x00ae:
            if (r6 == 0) goto L_0x00cf
            org.threeten.bp.temporal.ChronoField r9 = org.threeten.bp.temporal.ChronoField.MONTH_OF_YEAR
            boolean r9 = r1.containsKey(r9)
            if (r9 == 0) goto L_0x00cf
            org.threeten.bp.temporal.ChronoField r9 = org.threeten.bp.temporal.ChronoField.DAY_OF_MONTH
            boolean r9 = r1.containsKey(r9)
            if (r9 == 0) goto L_0x00cf
            org.threeten.bp.temporal.ChronoField r4 = org.threeten.bp.temporal.ChronoField.ERA
            r1.remove(r4)
            org.threeten.bp.temporal.ChronoField r4 = org.threeten.bp.temporal.ChronoField.YEAR_OF_ERA
            r1.remove(r4)
            org.threeten.bp.chrono.JapaneseDate r4 = r0.resolveEYMD(r1, r2, r6, r8)
            return r4
        L_0x00cf:
            if (r6 == 0) goto L_0x00e8
            org.threeten.bp.temporal.ChronoField r9 = org.threeten.bp.temporal.ChronoField.DAY_OF_YEAR
            boolean r9 = r1.containsKey(r9)
            if (r9 == 0) goto L_0x00e8
            org.threeten.bp.temporal.ChronoField r4 = org.threeten.bp.temporal.ChronoField.ERA
            r1.remove(r4)
            org.threeten.bp.temporal.ChronoField r4 = org.threeten.bp.temporal.ChronoField.YEAR_OF_ERA
            r1.remove(r4)
            org.threeten.bp.chrono.JapaneseDate r4 = r0.resolveEYD(r1, r2, r6, r8)
            return r4
        L_0x00e8:
            org.threeten.bp.temporal.ChronoField r8 = org.threeten.bp.temporal.ChronoField.YEAR
            boolean r8 = r1.containsKey(r8)
            if (r8 == 0) goto L_0x04c4
            org.threeten.bp.temporal.ChronoField r8 = org.threeten.bp.temporal.ChronoField.MONTH_OF_YEAR
            boolean r8 = r1.containsKey(r8)
            java.lang.String r9 = "Strict mode rejected date parsed to a different month"
            r10 = 1
            if (r8 == 0) goto L_0x032e
            org.threeten.bp.temporal.ChronoField r8 = org.threeten.bp.temporal.ChronoField.DAY_OF_MONTH
            boolean r8 = r1.containsKey(r8)
            if (r8 == 0) goto L_0x0190
            org.threeten.bp.temporal.ChronoField r8 = org.threeten.bp.temporal.ChronoField.YEAR
            org.threeten.bp.temporal.ChronoField r9 = org.threeten.bp.temporal.ChronoField.YEAR
            java.lang.Object r9 = r1.remove(r9)
            java.lang.Long r9 = (java.lang.Long) r9
            long r12 = r9.longValue()
            int r8 = r8.checkValidIntValue(r12)
            org.threeten.bp.format.ResolverStyle r9 = org.threeten.bp.format.ResolverStyle.LENIENT
            if (r2 != r9) goto L_0x0147
            org.threeten.bp.temporal.ChronoField r9 = org.threeten.bp.temporal.ChronoField.MONTH_OF_YEAR
            java.lang.Object r9 = r1.remove(r9)
            java.lang.Long r9 = (java.lang.Long) r9
            long r12 = r9.longValue()
            long r12 = org.threeten.bp.jdk8.Jdk8Methods.safeSubtract((long) r12, (long) r10)
            org.threeten.bp.temporal.ChronoField r9 = org.threeten.bp.temporal.ChronoField.DAY_OF_MONTH
            java.lang.Object r9 = r1.remove(r9)
            java.lang.Long r9 = (java.lang.Long) r9
            long r14 = r9.longValue()
            long r9 = org.threeten.bp.jdk8.Jdk8Methods.safeSubtract((long) r14, (long) r10)
            org.threeten.bp.chrono.JapaneseDate r4 = r0.date((int) r8, (int) r4, (int) r4)
            org.threeten.bp.chrono.JapaneseDate r4 = r4.plusMonths((long) r12)
            org.threeten.bp.chrono.JapaneseDate r4 = r4.plusDays((long) r9)
            return r4
        L_0x0147:
            org.threeten.bp.temporal.ChronoField r9 = org.threeten.bp.temporal.ChronoField.MONTH_OF_YEAR
            org.threeten.bp.temporal.ValueRange r9 = r0.range(r9)
            org.threeten.bp.temporal.ChronoField r10 = org.threeten.bp.temporal.ChronoField.MONTH_OF_YEAR
            java.lang.Object r10 = r1.remove(r10)
            java.lang.Long r10 = (java.lang.Long) r10
            long r10 = r10.longValue()
            org.threeten.bp.temporal.ChronoField r12 = org.threeten.bp.temporal.ChronoField.MONTH_OF_YEAR
            int r9 = r9.checkValidIntValue(r10, r12)
            org.threeten.bp.temporal.ChronoField r10 = org.threeten.bp.temporal.ChronoField.DAY_OF_MONTH
            org.threeten.bp.temporal.ValueRange r10 = r0.range(r10)
            org.threeten.bp.temporal.ChronoField r11 = org.threeten.bp.temporal.ChronoField.DAY_OF_MONTH
            java.lang.Object r11 = r1.remove(r11)
            java.lang.Long r11 = (java.lang.Long) r11
            long r11 = r11.longValue()
            org.threeten.bp.temporal.ChronoField r13 = org.threeten.bp.temporal.ChronoField.DAY_OF_MONTH
            int r10 = r10.checkValidIntValue(r11, r13)
            org.threeten.bp.format.ResolverStyle r11 = org.threeten.bp.format.ResolverStyle.SMART
            if (r2 != r11) goto L_0x018b
            r11 = 28
            if (r10 <= r11) goto L_0x018b
            org.threeten.bp.chrono.JapaneseDate r4 = r0.date((int) r8, (int) r9, (int) r4)
            int r4 = r4.lengthOfMonth()
            int r10 = java.lang.Math.min(r10, r4)
        L_0x018b:
            org.threeten.bp.chrono.JapaneseDate r4 = r0.date((int) r8, (int) r9, (int) r10)
            return r4
        L_0x0190:
            org.threeten.bp.temporal.ChronoField r8 = org.threeten.bp.temporal.ChronoField.ALIGNED_WEEK_OF_MONTH
            boolean r8 = r1.containsKey(r8)
            if (r8 == 0) goto L_0x032b
            org.threeten.bp.temporal.ChronoField r8 = org.threeten.bp.temporal.ChronoField.ALIGNED_DAY_OF_WEEK_IN_MONTH
            boolean r8 = r1.containsKey(r8)
            if (r8 == 0) goto L_0x025f
            org.threeten.bp.temporal.ChronoField r8 = org.threeten.bp.temporal.ChronoField.YEAR
            org.threeten.bp.temporal.ChronoField r12 = org.threeten.bp.temporal.ChronoField.YEAR
            java.lang.Object r12 = r1.remove(r12)
            java.lang.Long r12 = (java.lang.Long) r12
            long r12 = r12.longValue()
            int r8 = r8.checkValidIntValue(r12)
            org.threeten.bp.format.ResolverStyle r12 = org.threeten.bp.format.ResolverStyle.LENIENT
            if (r2 != r12) goto L_0x0200
            org.threeten.bp.temporal.ChronoField r9 = org.threeten.bp.temporal.ChronoField.MONTH_OF_YEAR
            java.lang.Object r9 = r1.remove(r9)
            java.lang.Long r9 = (java.lang.Long) r9
            long r12 = r9.longValue()
            long r12 = org.threeten.bp.jdk8.Jdk8Methods.safeSubtract((long) r12, (long) r10)
            org.threeten.bp.temporal.ChronoField r9 = org.threeten.bp.temporal.ChronoField.ALIGNED_WEEK_OF_MONTH
            java.lang.Object r9 = r1.remove(r9)
            java.lang.Long r9 = (java.lang.Long) r9
            long r14 = r9.longValue()
            long r14 = org.threeten.bp.jdk8.Jdk8Methods.safeSubtract((long) r14, (long) r10)
            org.threeten.bp.temporal.ChronoField r9 = org.threeten.bp.temporal.ChronoField.ALIGNED_DAY_OF_WEEK_IN_MONTH
            java.lang.Object r9 = r1.remove(r9)
            java.lang.Long r9 = (java.lang.Long) r9
            r16 = r5
            long r4 = r9.longValue()
            long r4 = org.threeten.bp.jdk8.Jdk8Methods.safeSubtract((long) r4, (long) r10)
            r9 = 1
            org.threeten.bp.chrono.JapaneseDate r9 = r0.date((int) r8, (int) r9, (int) r9)
            org.threeten.bp.temporal.ChronoUnit r10 = org.threeten.bp.temporal.ChronoUnit.MONTHS
            org.threeten.bp.chrono.JapaneseDate r9 = r9.plus((long) r12, (org.threeten.bp.temporal.TemporalUnit) r10)
            org.threeten.bp.temporal.ChronoUnit r10 = org.threeten.bp.temporal.ChronoUnit.WEEKS
            org.threeten.bp.chrono.JapaneseDate r9 = r9.plus((long) r14, (org.threeten.bp.temporal.TemporalUnit) r10)
            org.threeten.bp.temporal.ChronoUnit r10 = org.threeten.bp.temporal.ChronoUnit.DAYS
            org.threeten.bp.chrono.JapaneseDate r9 = r9.plus((long) r4, (org.threeten.bp.temporal.TemporalUnit) r10)
            return r9
        L_0x0200:
            r16 = r5
            org.threeten.bp.temporal.ChronoField r4 = org.threeten.bp.temporal.ChronoField.MONTH_OF_YEAR
            org.threeten.bp.temporal.ChronoField r5 = org.threeten.bp.temporal.ChronoField.MONTH_OF_YEAR
            java.lang.Object r5 = r1.remove(r5)
            java.lang.Long r5 = (java.lang.Long) r5
            long r10 = r5.longValue()
            int r4 = r4.checkValidIntValue(r10)
            org.threeten.bp.temporal.ChronoField r5 = org.threeten.bp.temporal.ChronoField.ALIGNED_WEEK_OF_MONTH
            org.threeten.bp.temporal.ChronoField r10 = org.threeten.bp.temporal.ChronoField.ALIGNED_WEEK_OF_MONTH
            java.lang.Object r10 = r1.remove(r10)
            java.lang.Long r10 = (java.lang.Long) r10
            long r10 = r10.longValue()
            int r5 = r5.checkValidIntValue(r10)
            org.threeten.bp.temporal.ChronoField r10 = org.threeten.bp.temporal.ChronoField.ALIGNED_DAY_OF_WEEK_IN_MONTH
            org.threeten.bp.temporal.ChronoField r11 = org.threeten.bp.temporal.ChronoField.ALIGNED_DAY_OF_WEEK_IN_MONTH
            java.lang.Object r11 = r1.remove(r11)
            java.lang.Long r11 = (java.lang.Long) r11
            long r11 = r11.longValue()
            int r10 = r10.checkValidIntValue(r11)
            r11 = 1
            org.threeten.bp.chrono.JapaneseDate r11 = r0.date((int) r8, (int) r4, (int) r11)
            int r12 = r5 + -1
            int r12 = r12 * 7
            int r13 = r10 + -1
            int r12 = r12 + r13
            long r12 = (long) r12
            org.threeten.bp.temporal.ChronoUnit r14 = org.threeten.bp.temporal.ChronoUnit.DAYS
            org.threeten.bp.chrono.JapaneseDate r11 = r11.plus((long) r12, (org.threeten.bp.temporal.TemporalUnit) r14)
            org.threeten.bp.format.ResolverStyle r12 = org.threeten.bp.format.ResolverStyle.STRICT
            if (r2 != r12) goto L_0x025e
            org.threeten.bp.temporal.ChronoField r12 = org.threeten.bp.temporal.ChronoField.MONTH_OF_YEAR
            int r12 = r11.get(r12)
            if (r12 != r4) goto L_0x0258
            goto L_0x025e
        L_0x0258:
            org.threeten.bp.DateTimeException r12 = new org.threeten.bp.DateTimeException
            r12.<init>(r9)
            throw r12
        L_0x025e:
            return r11
        L_0x025f:
            r16 = r5
            org.threeten.bp.temporal.ChronoField r4 = org.threeten.bp.temporal.ChronoField.DAY_OF_WEEK
            boolean r4 = r1.containsKey(r4)
            if (r4 == 0) goto L_0x0330
            org.threeten.bp.temporal.ChronoField r4 = org.threeten.bp.temporal.ChronoField.YEAR
            org.threeten.bp.temporal.ChronoField r5 = org.threeten.bp.temporal.ChronoField.YEAR
            java.lang.Object r5 = r1.remove(r5)
            java.lang.Long r5 = (java.lang.Long) r5
            long r12 = r5.longValue()
            int r4 = r4.checkValidIntValue(r12)
            org.threeten.bp.format.ResolverStyle r5 = org.threeten.bp.format.ResolverStyle.LENIENT
            if (r2 != r5) goto L_0x02c7
            org.threeten.bp.temporal.ChronoField r5 = org.threeten.bp.temporal.ChronoField.MONTH_OF_YEAR
            java.lang.Object r5 = r1.remove(r5)
            java.lang.Long r5 = (java.lang.Long) r5
            long r8 = r5.longValue()
            long r8 = org.threeten.bp.jdk8.Jdk8Methods.safeSubtract((long) r8, (long) r10)
            org.threeten.bp.temporal.ChronoField r5 = org.threeten.bp.temporal.ChronoField.ALIGNED_WEEK_OF_MONTH
            java.lang.Object r5 = r1.remove(r5)
            java.lang.Long r5 = (java.lang.Long) r5
            long r12 = r5.longValue()
            long r12 = org.threeten.bp.jdk8.Jdk8Methods.safeSubtract((long) r12, (long) r10)
            org.threeten.bp.temporal.ChronoField r5 = org.threeten.bp.temporal.ChronoField.DAY_OF_WEEK
            java.lang.Object r5 = r1.remove(r5)
            java.lang.Long r5 = (java.lang.Long) r5
            long r14 = r5.longValue()
            long r10 = org.threeten.bp.jdk8.Jdk8Methods.safeSubtract((long) r14, (long) r10)
            r5 = 1
            org.threeten.bp.chrono.JapaneseDate r5 = r0.date((int) r4, (int) r5, (int) r5)
            org.threeten.bp.temporal.ChronoUnit r14 = org.threeten.bp.temporal.ChronoUnit.MONTHS
            org.threeten.bp.chrono.JapaneseDate r5 = r5.plus((long) r8, (org.threeten.bp.temporal.TemporalUnit) r14)
            org.threeten.bp.temporal.ChronoUnit r14 = org.threeten.bp.temporal.ChronoUnit.WEEKS
            org.threeten.bp.chrono.JapaneseDate r5 = r5.plus((long) r12, (org.threeten.bp.temporal.TemporalUnit) r14)
            org.threeten.bp.temporal.ChronoUnit r14 = org.threeten.bp.temporal.ChronoUnit.DAYS
            org.threeten.bp.chrono.JapaneseDate r5 = r5.plus((long) r10, (org.threeten.bp.temporal.TemporalUnit) r14)
            return r5
        L_0x02c7:
            org.threeten.bp.temporal.ChronoField r5 = org.threeten.bp.temporal.ChronoField.MONTH_OF_YEAR
            org.threeten.bp.temporal.ChronoField r8 = org.threeten.bp.temporal.ChronoField.MONTH_OF_YEAR
            java.lang.Object r8 = r1.remove(r8)
            java.lang.Long r8 = (java.lang.Long) r8
            long r10 = r8.longValue()
            int r5 = r5.checkValidIntValue(r10)
            org.threeten.bp.temporal.ChronoField r8 = org.threeten.bp.temporal.ChronoField.ALIGNED_WEEK_OF_MONTH
            org.threeten.bp.temporal.ChronoField r10 = org.threeten.bp.temporal.ChronoField.ALIGNED_WEEK_OF_MONTH
            java.lang.Object r10 = r1.remove(r10)
            java.lang.Long r10 = (java.lang.Long) r10
            long r10 = r10.longValue()
            int r8 = r8.checkValidIntValue(r10)
            org.threeten.bp.temporal.ChronoField r10 = org.threeten.bp.temporal.ChronoField.DAY_OF_WEEK
            org.threeten.bp.temporal.ChronoField r11 = org.threeten.bp.temporal.ChronoField.DAY_OF_WEEK
            java.lang.Object r11 = r1.remove(r11)
            java.lang.Long r11 = (java.lang.Long) r11
            long r11 = r11.longValue()
            int r10 = r10.checkValidIntValue(r11)
            r11 = 1
            org.threeten.bp.chrono.JapaneseDate r11 = r0.date((int) r4, (int) r5, (int) r11)
            int r12 = r8 + -1
            long r12 = (long) r12
            org.threeten.bp.temporal.ChronoUnit r14 = org.threeten.bp.temporal.ChronoUnit.WEEKS
            org.threeten.bp.chrono.JapaneseDate r11 = r11.plus((long) r12, (org.threeten.bp.temporal.TemporalUnit) r14)
            org.threeten.bp.DayOfWeek r12 = org.threeten.bp.DayOfWeek.of(r10)
            org.threeten.bp.temporal.TemporalAdjuster r12 = org.threeten.bp.temporal.TemporalAdjusters.nextOrSame(r12)
            org.threeten.bp.chrono.JapaneseDate r11 = r11.with((org.threeten.bp.temporal.TemporalAdjuster) r12)
            org.threeten.bp.format.ResolverStyle r12 = org.threeten.bp.format.ResolverStyle.STRICT
            if (r2 != r12) goto L_0x032a
            org.threeten.bp.temporal.ChronoField r12 = org.threeten.bp.temporal.ChronoField.MONTH_OF_YEAR
            int r12 = r11.get(r12)
            if (r12 != r5) goto L_0x0324
            goto L_0x032a
        L_0x0324:
            org.threeten.bp.DateTimeException r12 = new org.threeten.bp.DateTimeException
            r12.<init>(r9)
            throw r12
        L_0x032a:
            return r11
        L_0x032b:
            r16 = r5
            goto L_0x0330
        L_0x032e:
            r16 = r5
        L_0x0330:
            org.threeten.bp.temporal.ChronoField r4 = org.threeten.bp.temporal.ChronoField.DAY_OF_YEAR
            boolean r4 = r1.containsKey(r4)
            if (r4 == 0) goto L_0x037f
            org.threeten.bp.temporal.ChronoField r4 = org.threeten.bp.temporal.ChronoField.YEAR
            org.threeten.bp.temporal.ChronoField r5 = org.threeten.bp.temporal.ChronoField.YEAR
            java.lang.Object r5 = r1.remove(r5)
            java.lang.Long r5 = (java.lang.Long) r5
            long r8 = r5.longValue()
            int r4 = r4.checkValidIntValue(r8)
            org.threeten.bp.format.ResolverStyle r5 = org.threeten.bp.format.ResolverStyle.LENIENT
            if (r2 != r5) goto L_0x0368
            org.threeten.bp.temporal.ChronoField r5 = org.threeten.bp.temporal.ChronoField.DAY_OF_YEAR
            java.lang.Object r5 = r1.remove(r5)
            java.lang.Long r5 = (java.lang.Long) r5
            long r8 = r5.longValue()
            long r8 = org.threeten.bp.jdk8.Jdk8Methods.safeSubtract((long) r8, (long) r10)
            r5 = 1
            org.threeten.bp.chrono.JapaneseDate r5 = r0.dateYearDay((int) r4, (int) r5)
            org.threeten.bp.chrono.JapaneseDate r5 = r5.plusDays((long) r8)
            return r5
        L_0x0368:
            org.threeten.bp.temporal.ChronoField r5 = org.threeten.bp.temporal.ChronoField.DAY_OF_YEAR
            org.threeten.bp.temporal.ChronoField r8 = org.threeten.bp.temporal.ChronoField.DAY_OF_YEAR
            java.lang.Object r8 = r1.remove(r8)
            java.lang.Long r8 = (java.lang.Long) r8
            long r8 = r8.longValue()
            int r5 = r5.checkValidIntValue(r8)
            org.threeten.bp.chrono.JapaneseDate r8 = r0.dateYearDay((int) r4, (int) r5)
            return r8
        L_0x037f:
            org.threeten.bp.temporal.ChronoField r4 = org.threeten.bp.temporal.ChronoField.ALIGNED_WEEK_OF_YEAR
            boolean r4 = r1.containsKey(r4)
            if (r4 == 0) goto L_0x04c6
            org.threeten.bp.temporal.ChronoField r4 = org.threeten.bp.temporal.ChronoField.ALIGNED_DAY_OF_WEEK_IN_YEAR
            boolean r4 = r1.containsKey(r4)
            if (r4 == 0) goto L_0x0422
            org.threeten.bp.temporal.ChronoField r4 = org.threeten.bp.temporal.ChronoField.YEAR
            org.threeten.bp.temporal.ChronoField r5 = org.threeten.bp.temporal.ChronoField.YEAR
            java.lang.Object r5 = r1.remove(r5)
            java.lang.Long r5 = (java.lang.Long) r5
            long r8 = r5.longValue()
            int r4 = r4.checkValidIntValue(r8)
            org.threeten.bp.format.ResolverStyle r5 = org.threeten.bp.format.ResolverStyle.LENIENT
            if (r2 != r5) goto L_0x03d7
            org.threeten.bp.temporal.ChronoField r5 = org.threeten.bp.temporal.ChronoField.ALIGNED_WEEK_OF_YEAR
            java.lang.Object r5 = r1.remove(r5)
            java.lang.Long r5 = (java.lang.Long) r5
            long r8 = r5.longValue()
            long r8 = org.threeten.bp.jdk8.Jdk8Methods.safeSubtract((long) r8, (long) r10)
            org.threeten.bp.temporal.ChronoField r5 = org.threeten.bp.temporal.ChronoField.ALIGNED_DAY_OF_WEEK_IN_YEAR
            java.lang.Object r5 = r1.remove(r5)
            java.lang.Long r5 = (java.lang.Long) r5
            long r12 = r5.longValue()
            long r10 = org.threeten.bp.jdk8.Jdk8Methods.safeSubtract((long) r12, (long) r10)
            r5 = 1
            org.threeten.bp.chrono.JapaneseDate r5 = r0.date((int) r4, (int) r5, (int) r5)
            org.threeten.bp.temporal.ChronoUnit r12 = org.threeten.bp.temporal.ChronoUnit.WEEKS
            org.threeten.bp.chrono.JapaneseDate r5 = r5.plus((long) r8, (org.threeten.bp.temporal.TemporalUnit) r12)
            org.threeten.bp.temporal.ChronoUnit r12 = org.threeten.bp.temporal.ChronoUnit.DAYS
            org.threeten.bp.chrono.JapaneseDate r5 = r5.plus((long) r10, (org.threeten.bp.temporal.TemporalUnit) r12)
            return r5
        L_0x03d7:
            org.threeten.bp.temporal.ChronoField r5 = org.threeten.bp.temporal.ChronoField.ALIGNED_WEEK_OF_YEAR
            org.threeten.bp.temporal.ChronoField r8 = org.threeten.bp.temporal.ChronoField.ALIGNED_WEEK_OF_YEAR
            java.lang.Object r8 = r1.remove(r8)
            java.lang.Long r8 = (java.lang.Long) r8
            long r8 = r8.longValue()
            int r5 = r5.checkValidIntValue(r8)
            org.threeten.bp.temporal.ChronoField r8 = org.threeten.bp.temporal.ChronoField.ALIGNED_DAY_OF_WEEK_IN_YEAR
            org.threeten.bp.temporal.ChronoField r9 = org.threeten.bp.temporal.ChronoField.ALIGNED_DAY_OF_WEEK_IN_YEAR
            java.lang.Object r9 = r1.remove(r9)
            java.lang.Long r9 = (java.lang.Long) r9
            long r9 = r9.longValue()
            int r8 = r8.checkValidIntValue(r9)
            r9 = 1
            org.threeten.bp.chrono.JapaneseDate r9 = r0.date((int) r4, (int) r9, (int) r9)
            int r10 = r5 + -1
            int r10 = r10 * 7
            int r11 = r8 + -1
            int r10 = r10 + r11
            long r10 = (long) r10
            org.threeten.bp.chrono.JapaneseDate r9 = r9.plusDays((long) r10)
            org.threeten.bp.format.ResolverStyle r10 = org.threeten.bp.format.ResolverStyle.STRICT
            if (r2 != r10) goto L_0x0421
            org.threeten.bp.temporal.ChronoField r10 = org.threeten.bp.temporal.ChronoField.YEAR
            int r10 = r9.get(r10)
            if (r10 != r4) goto L_0x0419
            goto L_0x0421
        L_0x0419:
            org.threeten.bp.DateTimeException r10 = new org.threeten.bp.DateTimeException
            java.lang.String r11 = "Strict mode rejected date parsed to a different year"
            r10.<init>(r11)
            throw r10
        L_0x0421:
            return r9
        L_0x0422:
            org.threeten.bp.temporal.ChronoField r4 = org.threeten.bp.temporal.ChronoField.DAY_OF_WEEK
            boolean r4 = r1.containsKey(r4)
            if (r4 == 0) goto L_0x04c6
            org.threeten.bp.temporal.ChronoField r4 = org.threeten.bp.temporal.ChronoField.YEAR
            org.threeten.bp.temporal.ChronoField r5 = org.threeten.bp.temporal.ChronoField.YEAR
            java.lang.Object r5 = r1.remove(r5)
            java.lang.Long r5 = (java.lang.Long) r5
            long r12 = r5.longValue()
            int r4 = r4.checkValidIntValue(r12)
            org.threeten.bp.format.ResolverStyle r5 = org.threeten.bp.format.ResolverStyle.LENIENT
            if (r2 != r5) goto L_0x0472
            org.threeten.bp.temporal.ChronoField r5 = org.threeten.bp.temporal.ChronoField.ALIGNED_WEEK_OF_YEAR
            java.lang.Object r5 = r1.remove(r5)
            java.lang.Long r5 = (java.lang.Long) r5
            long r8 = r5.longValue()
            long r8 = org.threeten.bp.jdk8.Jdk8Methods.safeSubtract((long) r8, (long) r10)
            org.threeten.bp.temporal.ChronoField r5 = org.threeten.bp.temporal.ChronoField.DAY_OF_WEEK
            java.lang.Object r5 = r1.remove(r5)
            java.lang.Long r5 = (java.lang.Long) r5
            long r12 = r5.longValue()
            long r10 = org.threeten.bp.jdk8.Jdk8Methods.safeSubtract((long) r12, (long) r10)
            r5 = 1
            org.threeten.bp.chrono.JapaneseDate r5 = r0.date((int) r4, (int) r5, (int) r5)
            org.threeten.bp.temporal.ChronoUnit r12 = org.threeten.bp.temporal.ChronoUnit.WEEKS
            org.threeten.bp.chrono.JapaneseDate r5 = r5.plus((long) r8, (org.threeten.bp.temporal.TemporalUnit) r12)
            org.threeten.bp.temporal.ChronoUnit r12 = org.threeten.bp.temporal.ChronoUnit.DAYS
            org.threeten.bp.chrono.JapaneseDate r5 = r5.plus((long) r10, (org.threeten.bp.temporal.TemporalUnit) r12)
            return r5
        L_0x0472:
            org.threeten.bp.temporal.ChronoField r5 = org.threeten.bp.temporal.ChronoField.ALIGNED_WEEK_OF_YEAR
            org.threeten.bp.temporal.ChronoField r8 = org.threeten.bp.temporal.ChronoField.ALIGNED_WEEK_OF_YEAR
            java.lang.Object r8 = r1.remove(r8)
            java.lang.Long r8 = (java.lang.Long) r8
            long r10 = r8.longValue()
            int r5 = r5.checkValidIntValue(r10)
            org.threeten.bp.temporal.ChronoField r8 = org.threeten.bp.temporal.ChronoField.DAY_OF_WEEK
            org.threeten.bp.temporal.ChronoField r10 = org.threeten.bp.temporal.ChronoField.DAY_OF_WEEK
            java.lang.Object r10 = r1.remove(r10)
            java.lang.Long r10 = (java.lang.Long) r10
            long r10 = r10.longValue()
            int r8 = r8.checkValidIntValue(r10)
            r10 = 1
            org.threeten.bp.chrono.JapaneseDate r10 = r0.date((int) r4, (int) r10, (int) r10)
            int r11 = r5 + -1
            long r11 = (long) r11
            org.threeten.bp.temporal.ChronoUnit r13 = org.threeten.bp.temporal.ChronoUnit.WEEKS
            org.threeten.bp.chrono.JapaneseDate r10 = r10.plus((long) r11, (org.threeten.bp.temporal.TemporalUnit) r13)
            org.threeten.bp.DayOfWeek r11 = org.threeten.bp.DayOfWeek.of(r8)
            org.threeten.bp.temporal.TemporalAdjuster r11 = org.threeten.bp.temporal.TemporalAdjusters.nextOrSame(r11)
            org.threeten.bp.chrono.JapaneseDate r10 = r10.with((org.threeten.bp.temporal.TemporalAdjuster) r11)
            org.threeten.bp.format.ResolverStyle r11 = org.threeten.bp.format.ResolverStyle.STRICT
            if (r2 != r11) goto L_0x04c3
            org.threeten.bp.temporal.ChronoField r11 = org.threeten.bp.temporal.ChronoField.YEAR
            int r11 = r10.get(r11)
            if (r11 != r4) goto L_0x04bd
            goto L_0x04c3
        L_0x04bd:
            org.threeten.bp.DateTimeException r11 = new org.threeten.bp.DateTimeException
            r11.<init>(r9)
            throw r11
        L_0x04c3:
            return r10
        L_0x04c4:
            r16 = r5
        L_0x04c6:
            r4 = 0
            return r4
        */
        throw new UnsupportedOperationException("Method not decompiled: org.threeten.bp.chrono.JapaneseChronology.resolveDate(java.util.Map, org.threeten.bp.format.ResolverStyle):org.threeten.bp.chrono.JapaneseDate");
    }

    private JapaneseDate resolveEYMD(Map<TemporalField, Long> fieldValues, ResolverStyle resolverStyle, JapaneseEra era, int yoe) {
        if (resolverStyle == ResolverStyle.LENIENT) {
            long months = Jdk8Methods.safeSubtract(fieldValues.remove(ChronoField.MONTH_OF_YEAR).longValue(), 1);
            return date((era.startDate().getYear() + yoe) - 1, 1, 1).plus(months, (TemporalUnit) ChronoUnit.MONTHS).plus(Jdk8Methods.safeSubtract(fieldValues.remove(ChronoField.DAY_OF_MONTH).longValue(), 1), (TemporalUnit) ChronoUnit.DAYS);
        }
        int moy = range(ChronoField.MONTH_OF_YEAR).checkValidIntValue(fieldValues.remove(ChronoField.MONTH_OF_YEAR).longValue(), ChronoField.MONTH_OF_YEAR);
        int dom = range(ChronoField.DAY_OF_MONTH).checkValidIntValue(fieldValues.remove(ChronoField.DAY_OF_MONTH).longValue(), ChronoField.DAY_OF_MONTH);
        if (resolverStyle != ResolverStyle.SMART) {
            return date((Era) era, yoe, moy, dom);
        }
        if (yoe >= 1) {
            int y = (era.startDate().getYear() + yoe) - 1;
            if (dom > 28) {
                dom = Math.min(dom, date(y, moy, 1).lengthOfMonth());
            }
            JapaneseDate jd = date(y, moy, dom);
            if (jd.getEra() != era) {
                if (Math.abs(jd.getEra().getValue() - era.getValue()) > 1) {
                    throw new DateTimeException("Invalid Era/YearOfEra: " + era + " " + yoe);
                } else if (!(jd.get(ChronoField.YEAR_OF_ERA) == 1 || yoe == 1)) {
                    throw new DateTimeException("Invalid Era/YearOfEra: " + era + " " + yoe);
                }
            }
            return jd;
        }
        throw new DateTimeException("Invalid YearOfEra: " + yoe);
    }

    private JapaneseDate resolveEYD(Map<TemporalField, Long> fieldValues, ResolverStyle resolverStyle, JapaneseEra era, int yoe) {
        if (resolverStyle != ResolverStyle.LENIENT) {
            return dateYearDay((Era) era, yoe, range(ChronoField.DAY_OF_YEAR).checkValidIntValue(fieldValues.remove(ChronoField.DAY_OF_YEAR).longValue(), ChronoField.DAY_OF_YEAR));
        }
        return dateYearDay((era.startDate().getYear() + yoe) - 1, 1).plus(Jdk8Methods.safeSubtract(fieldValues.remove(ChronoField.DAY_OF_YEAR).longValue(), 1), (TemporalUnit) ChronoUnit.DAYS);
    }
}
