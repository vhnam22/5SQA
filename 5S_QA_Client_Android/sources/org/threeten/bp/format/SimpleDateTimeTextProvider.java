package org.threeten.bp.format;

import java.text.DateFormatSymbols;
import java.util.AbstractMap;
import java.util.ArrayList;
import java.util.Collections;
import java.util.Comparator;
import java.util.HashMap;
import java.util.Iterator;
import java.util.List;
import java.util.Locale;
import java.util.Map;
import java.util.concurrent.ConcurrentHashMap;
import java.util.concurrent.ConcurrentMap;
import org.threeten.bp.temporal.ChronoField;
import org.threeten.bp.temporal.IsoFields;
import org.threeten.bp.temporal.TemporalField;

final class SimpleDateTimeTextProvider extends DateTimeTextProvider {
    /* access modifiers changed from: private */
    public static final Comparator<Map.Entry<String, Long>> COMPARATOR = new Comparator<Map.Entry<String, Long>>() {
        public int compare(Map.Entry<String, Long> obj1, Map.Entry<String, Long> obj2) {
            return obj2.getKey().length() - obj1.getKey().length();
        }
    };
    private final ConcurrentMap<Map.Entry<TemporalField, Locale>, Object> cache = new ConcurrentHashMap(16, 0.75f, 2);

    SimpleDateTimeTextProvider() {
    }

    public String getText(TemporalField field, long value, TextStyle style, Locale locale) {
        Object store = findStore(field, locale);
        if (store instanceof LocaleStore) {
            return ((LocaleStore) store).getText(value, style);
        }
        return null;
    }

    public Iterator<Map.Entry<String, Long>> getTextIterator(TemporalField field, TextStyle style, Locale locale) {
        Object store = findStore(field, locale);
        if (store instanceof LocaleStore) {
            return ((LocaleStore) store).getTextIterator(style);
        }
        return null;
    }

    private Object findStore(TemporalField field, Locale locale) {
        Map.Entry<TemporalField, Locale> key = createEntry(field, locale);
        Object store = this.cache.get(key);
        if (store != null) {
            return store;
        }
        this.cache.putIfAbsent(key, createStore(field, locale));
        return this.cache.get(key);
    }

    private Object createStore(TemporalField field, Locale locale) {
        if (field == ChronoField.MONTH_OF_YEAR) {
            DateFormatSymbols oldSymbols = DateFormatSymbols.getInstance(locale);
            Map<TextStyle, Map<Long, String>> styleMap = new HashMap<>();
            String[] array = oldSymbols.getMonths();
            Map<Long, String> map = new HashMap<>();
            map.put(1L, array[0]);
            map.put(2L, array[1]);
            map.put(3L, array[2]);
            map.put(4L, array[3]);
            map.put(5L, array[4]);
            map.put(6L, array[5]);
            map.put(7L, array[6]);
            map.put(8L, array[7]);
            map.put(9L, array[8]);
            map.put(10L, array[9]);
            map.put(11L, array[10]);
            map.put(12L, array[11]);
            styleMap.put(TextStyle.FULL, map);
            Map<Long, String> map2 = new HashMap<>();
            map2.put(1L, array[0].substring(0, 1));
            map2.put(2L, array[1].substring(0, 1));
            map2.put(3L, array[2].substring(0, 1));
            map2.put(4L, array[3].substring(0, 1));
            map2.put(5L, array[4].substring(0, 1));
            map2.put(6L, array[5].substring(0, 1));
            map2.put(7L, array[6].substring(0, 1));
            map2.put(8L, array[7].substring(0, 1));
            map2.put(9L, array[8].substring(0, 1));
            map2.put(10L, array[9].substring(0, 1));
            map2.put(11L, array[10].substring(0, 1));
            map2.put(12L, array[11].substring(0, 1));
            Map<TextStyle, Map<Long, String>> styleMap2 = styleMap;
            styleMap2.put(TextStyle.NARROW, map2);
            String[] array2 = oldSymbols.getShortMonths();
            Map<Long, String> map3 = new HashMap<>();
            map3.put(1L, array2[0]);
            map3.put(2L, array2[1]);
            map3.put(3L, array2[2]);
            map3.put(4L, array2[3]);
            map3.put(5L, array2[4]);
            map3.put(6L, array2[5]);
            map3.put(7, array2[6]);
            map3.put(8L, array2[7]);
            map3.put(9L, array2[8]);
            map3.put(10L, array2[9]);
            map3.put(11L, array2[10]);
            map3.put(12L, array2[11]);
            styleMap2.put(TextStyle.SHORT, map3);
            return createLocaleStore(styleMap2);
        }
        TemporalField temporalField = field;
        if (temporalField == ChronoField.DAY_OF_WEEK) {
            DateFormatSymbols oldSymbols2 = DateFormatSymbols.getInstance(locale);
            Map<TextStyle, Map<Long, String>> styleMap3 = new HashMap<>();
            String[] array3 = oldSymbols2.getWeekdays();
            Map<Long, String> map4 = new HashMap<>();
            map4.put(1L, array3[2]);
            map4.put(2L, array3[3]);
            map4.put(3L, array3[4]);
            map4.put(4L, array3[5]);
            map4.put(5L, array3[6]);
            map4.put(6L, array3[7]);
            map4.put(7L, array3[1]);
            styleMap3.put(TextStyle.FULL, map4);
            Map<Long, String> map5 = new HashMap<>();
            map5.put(1L, array3[2].substring(0, 1));
            map5.put(2L, array3[3].substring(0, 1));
            map5.put(3L, array3[4].substring(0, 1));
            map5.put(4L, array3[5].substring(0, 1));
            map5.put(5L, array3[6].substring(0, 1));
            map5.put(6L, array3[7].substring(0, 1));
            map5.put(7L, array3[1].substring(0, 1));
            styleMap3.put(TextStyle.NARROW, map5);
            String[] array4 = oldSymbols2.getShortWeekdays();
            Map<Long, String> map6 = new HashMap<>();
            map6.put(1L, array4[2]);
            map6.put(2L, array4[3]);
            map6.put(3L, array4[4]);
            map6.put(4L, array4[5]);
            map6.put(5L, array4[6]);
            map6.put(6L, array4[7]);
            map6.put(7L, array4[1]);
            styleMap3.put(TextStyle.SHORT, map6);
            return createLocaleStore(styleMap3);
        } else if (temporalField == ChronoField.AMPM_OF_DAY) {
            DateFormatSymbols oldSymbols3 = DateFormatSymbols.getInstance(locale);
            Map<TextStyle, Map<Long, String>> styleMap4 = new HashMap<>();
            String[] array5 = oldSymbols3.getAmPmStrings();
            Map<Long, String> map7 = new HashMap<>();
            map7.put(0L, array5[0]);
            map7.put(1L, array5[1]);
            styleMap4.put(TextStyle.FULL, map7);
            styleMap4.put(TextStyle.SHORT, map7);
            return createLocaleStore(styleMap4);
        } else if (temporalField == ChronoField.ERA) {
            DateFormatSymbols oldSymbols4 = DateFormatSymbols.getInstance(locale);
            Map<TextStyle, Map<Long, String>> styleMap5 = new HashMap<>();
            String[] array6 = oldSymbols4.getEras();
            Map<Long, String> map8 = new HashMap<>();
            map8.put(0L, array6[0]);
            map8.put(1L, array6[1]);
            styleMap5.put(TextStyle.SHORT, map8);
            if (locale.getLanguage().equals(Locale.ENGLISH.getLanguage())) {
                Map<Long, String> map9 = new HashMap<>();
                map9.put(0L, "Before Christ");
                map9.put(1L, "Anno Domini");
                styleMap5.put(TextStyle.FULL, map9);
            } else {
                styleMap5.put(TextStyle.FULL, map8);
            }
            Map<Long, String> map10 = new HashMap<>();
            map10.put(0L, array6[0].substring(0, 1));
            map10.put(1L, array6[1].substring(0, 1));
            styleMap5.put(TextStyle.NARROW, map10);
            return createLocaleStore(styleMap5);
        } else if (temporalField != IsoFields.QUARTER_OF_YEAR) {
            return "";
        } else {
            Map<TextStyle, Map<Long, String>> styleMap6 = new HashMap<>();
            Map<Long, String> map11 = new HashMap<>();
            map11.put(1L, "Q1");
            map11.put(2L, "Q2");
            map11.put(3L, "Q3");
            map11.put(4L, "Q4");
            styleMap6.put(TextStyle.SHORT, map11);
            Map<Long, String> map12 = new HashMap<>();
            map12.put(1L, "1st quarter");
            map12.put(2L, "2nd quarter");
            map12.put(3L, "3rd quarter");
            map12.put(4L, "4th quarter");
            styleMap6.put(TextStyle.FULL, map12);
            return createLocaleStore(styleMap6);
        }
    }

    /* access modifiers changed from: private */
    public static <A, B> Map.Entry<A, B> createEntry(A text, B field) {
        return new AbstractMap.SimpleImmutableEntry(text, field);
    }

    private static LocaleStore createLocaleStore(Map<TextStyle, Map<Long, String>> valueTextMap) {
        valueTextMap.put(TextStyle.FULL_STANDALONE, valueTextMap.get(TextStyle.FULL));
        valueTextMap.put(TextStyle.SHORT_STANDALONE, valueTextMap.get(TextStyle.SHORT));
        if (valueTextMap.containsKey(TextStyle.NARROW) && !valueTextMap.containsKey(TextStyle.NARROW_STANDALONE)) {
            valueTextMap.put(TextStyle.NARROW_STANDALONE, valueTextMap.get(TextStyle.NARROW));
        }
        return new LocaleStore(valueTextMap);
    }

    static final class LocaleStore {
        private final Map<TextStyle, List<Map.Entry<String, Long>>> parsable;
        private final Map<TextStyle, Map<Long, String>> valueTextMap;

        LocaleStore(Map<TextStyle, Map<Long, String>> valueTextMap2) {
            this.valueTextMap = valueTextMap2;
            Map<TextStyle, List<Map.Entry<String, Long>>> map = new HashMap<>();
            List<Map.Entry<String, Long>> allList = new ArrayList<>();
            for (TextStyle style : valueTextMap2.keySet()) {
                Map<String, Map.Entry<String, Long>> reverse = new HashMap<>();
                for (Map.Entry<Long, String> entry : valueTextMap2.get(style).entrySet()) {
                    reverse.put(entry.getValue(), SimpleDateTimeTextProvider.createEntry(entry.getValue(), entry.getKey()));
                }
                List<Map.Entry<String, Long>> list = new ArrayList<>(reverse.values());
                Collections.sort(list, SimpleDateTimeTextProvider.COMPARATOR);
                map.put(style, list);
                allList.addAll(list);
                map.put((Object) null, allList);
            }
            Collections.sort(allList, SimpleDateTimeTextProvider.COMPARATOR);
            this.parsable = map;
        }

        /* access modifiers changed from: package-private */
        public String getText(long value, TextStyle style) {
            Map<Long, String> map = this.valueTextMap.get(style);
            if (map != null) {
                return map.get(Long.valueOf(value));
            }
            return null;
        }

        /* access modifiers changed from: package-private */
        public Iterator<Map.Entry<String, Long>> getTextIterator(TextStyle style) {
            List<Map.Entry<String, Long>> list = this.parsable.get(style);
            if (list != null) {
                return list.iterator();
            }
            return null;
        }
    }
}
