package org.threeten.bp;

import java.io.DataOutput;
import java.io.IOException;
import java.io.Serializable;
import java.util.Collections;
import java.util.HashMap;
import java.util.HashSet;
import java.util.Locale;
import java.util.Map;
import java.util.Set;
import java.util.TimeZone;
import org.threeten.bp.format.DateTimeFormatterBuilder;
import org.threeten.bp.format.TextStyle;
import org.threeten.bp.jdk8.DefaultInterfaceTemporalAccessor;
import org.threeten.bp.jdk8.Jdk8Methods;
import org.threeten.bp.temporal.TemporalAccessor;
import org.threeten.bp.temporal.TemporalField;
import org.threeten.bp.temporal.TemporalQueries;
import org.threeten.bp.temporal.TemporalQuery;
import org.threeten.bp.temporal.UnsupportedTemporalTypeException;
import org.threeten.bp.zone.ZoneRules;
import org.threeten.bp.zone.ZoneRulesException;
import org.threeten.bp.zone.ZoneRulesProvider;

public abstract class ZoneId implements Serializable {
    public static final TemporalQuery<ZoneId> FROM = new TemporalQuery<ZoneId>() {
        public ZoneId queryFrom(TemporalAccessor temporal) {
            return ZoneId.from(temporal);
        }
    };
    public static final Map<String, String> SHORT_IDS;
    private static final long serialVersionUID = 8352817235686L;

    public abstract String getId();

    public abstract ZoneRules getRules();

    /* access modifiers changed from: package-private */
    public abstract void write(DataOutput dataOutput) throws IOException;

    static {
        Map<String, String> base = new HashMap<>();
        base.put("ACT", "Australia/Darwin");
        base.put("AET", "Australia/Sydney");
        base.put("AGT", "America/Argentina/Buenos_Aires");
        base.put("ART", "Africa/Cairo");
        base.put("AST", "America/Anchorage");
        base.put("BET", "America/Sao_Paulo");
        base.put("BST", "Asia/Dhaka");
        base.put("CAT", "Africa/Harare");
        base.put("CNT", "America/St_Johns");
        base.put("CST", "America/Chicago");
        base.put("CTT", "Asia/Shanghai");
        base.put("EAT", "Africa/Addis_Ababa");
        base.put("ECT", "Europe/Paris");
        base.put("IET", "America/Indiana/Indianapolis");
        base.put("IST", "Asia/Kolkata");
        base.put("JST", "Asia/Tokyo");
        base.put("MIT", "Pacific/Apia");
        base.put("NET", "Asia/Yerevan");
        base.put("NST", "Pacific/Auckland");
        base.put("PLT", "Asia/Karachi");
        base.put("PNT", "America/Phoenix");
        base.put("PRT", "America/Puerto_Rico");
        base.put("PST", "America/Los_Angeles");
        base.put("SST", "Pacific/Guadalcanal");
        base.put("VST", "Asia/Ho_Chi_Minh");
        base.put("EST", "-05:00");
        base.put("MST", "-07:00");
        base.put("HST", "-10:00");
        SHORT_IDS = Collections.unmodifiableMap(base);
    }

    public static ZoneId systemDefault() {
        return of(TimeZone.getDefault().getID(), SHORT_IDS);
    }

    public static Set<String> getAvailableZoneIds() {
        return new HashSet(ZoneRulesProvider.getAvailableZoneIds());
    }

    public static ZoneId of(String zoneId, Map<String, String> aliasMap) {
        Jdk8Methods.requireNonNull(zoneId, "zoneId");
        Jdk8Methods.requireNonNull(aliasMap, "aliasMap");
        String id = aliasMap.get(zoneId);
        return of(id != null ? id : zoneId);
    }

    public static ZoneId of(String zoneId) {
        Jdk8Methods.requireNonNull(zoneId, "zoneId");
        if (zoneId.equals("Z")) {
            return ZoneOffset.UTC;
        }
        if (zoneId.length() == 1) {
            throw new DateTimeException("Invalid zone: " + zoneId);
        } else if (zoneId.startsWith("+") || zoneId.startsWith("-")) {
            return ZoneOffset.of(zoneId);
        } else {
            if (zoneId.equals("UTC") || zoneId.equals("GMT") || zoneId.equals("UT")) {
                return new ZoneRegion(zoneId, ZoneOffset.UTC.getRules());
            }
            if (zoneId.startsWith("UTC+") || zoneId.startsWith("GMT+") || zoneId.startsWith("UTC-") || zoneId.startsWith("GMT-")) {
                ZoneOffset offset = ZoneOffset.of(zoneId.substring(3));
                if (offset.getTotalSeconds() == 0) {
                    return new ZoneRegion(zoneId.substring(0, 3), offset.getRules());
                }
                return new ZoneRegion(zoneId.substring(0, 3) + offset.getId(), offset.getRules());
            } else if (!zoneId.startsWith("UT+") && !zoneId.startsWith("UT-")) {
                return ZoneRegion.ofId(zoneId, true);
            } else {
                ZoneOffset offset2 = ZoneOffset.of(zoneId.substring(2));
                if (offset2.getTotalSeconds() == 0) {
                    return new ZoneRegion("UT", offset2.getRules());
                }
                return new ZoneRegion("UT" + offset2.getId(), offset2.getRules());
            }
        }
    }

    public static ZoneId ofOffset(String prefix, ZoneOffset offset) {
        Jdk8Methods.requireNonNull(prefix, "prefix");
        Jdk8Methods.requireNonNull(offset, "offset");
        if (prefix.length() == 0) {
            return offset;
        }
        if (!prefix.equals("GMT") && !prefix.equals("UTC") && !prefix.equals("UT")) {
            throw new IllegalArgumentException("Invalid prefix, must be GMT, UTC or UT: " + prefix);
        } else if (offset.getTotalSeconds() == 0) {
            return new ZoneRegion(prefix, offset.getRules());
        } else {
            return new ZoneRegion(prefix + offset.getId(), offset.getRules());
        }
    }

    public static ZoneId from(TemporalAccessor temporal) {
        ZoneId obj = (ZoneId) temporal.query(TemporalQueries.zone());
        if (obj != null) {
            return obj;
        }
        throw new DateTimeException("Unable to obtain ZoneId from TemporalAccessor: " + temporal + ", type " + temporal.getClass().getName());
    }

    ZoneId() {
        if (getClass() != ZoneOffset.class && getClass() != ZoneRegion.class) {
            throw new AssertionError("Invalid subclass");
        }
    }

    public String getDisplayName(TextStyle style, Locale locale) {
        return new DateTimeFormatterBuilder().appendZoneText(style).toFormatter(locale).format(new DefaultInterfaceTemporalAccessor() {
            public boolean isSupported(TemporalField field) {
                return false;
            }

            public long getLong(TemporalField field) {
                throw new UnsupportedTemporalTypeException("Unsupported field: " + field);
            }

            public <R> R query(TemporalQuery<R> query) {
                if (query == TemporalQueries.zoneId()) {
                    return ZoneId.this;
                }
                return super.query(query);
            }
        });
    }

    public ZoneId normalized() {
        try {
            ZoneRules rules = getRules();
            if (rules.isFixedOffset()) {
                return rules.getOffset(Instant.EPOCH);
            }
        } catch (ZoneRulesException e) {
        }
        return this;
    }

    public boolean equals(Object obj) {
        if (this == obj) {
            return true;
        }
        if (obj instanceof ZoneId) {
            return getId().equals(((ZoneId) obj).getId());
        }
        return false;
    }

    public int hashCode() {
        return getId().hashCode();
    }

    public String toString() {
        return getId();
    }
}
