package org.threeten.bp;

import java.io.DataInput;
import java.io.DataOutput;
import java.io.IOException;
import java.io.InvalidObjectException;
import java.io.ObjectStreamException;
import java.io.Serializable;
import java.util.regex.Pattern;
import org.threeten.bp.jdk8.Jdk8Methods;
import org.threeten.bp.zone.ZoneRules;
import org.threeten.bp.zone.ZoneRulesException;
import org.threeten.bp.zone.ZoneRulesProvider;

final class ZoneRegion extends ZoneId implements Serializable {
    private static final Pattern PATTERN = Pattern.compile("[A-Za-z][A-Za-z0-9~/._+-]+");
    private static final long serialVersionUID = 8386373296231747096L;
    private final String id;
    private final transient ZoneRules rules;

    private static ZoneRegion ofLenient(String zoneId) {
        if (zoneId.equals("Z") || zoneId.startsWith("+") || zoneId.startsWith("-")) {
            throw new DateTimeException("Invalid ID for region-based ZoneId, invalid format: " + zoneId);
        } else if (zoneId.equals("UTC") || zoneId.equals("GMT") || zoneId.equals("UT")) {
            return new ZoneRegion(zoneId, ZoneOffset.UTC.getRules());
        } else {
            if (zoneId.startsWith("UTC+") || zoneId.startsWith("GMT+") || zoneId.startsWith("UTC-") || zoneId.startsWith("GMT-")) {
                ZoneOffset offset = ZoneOffset.of(zoneId.substring(3));
                if (offset.getTotalSeconds() == 0) {
                    return new ZoneRegion(zoneId.substring(0, 3), offset.getRules());
                }
                return new ZoneRegion(zoneId.substring(0, 3) + offset.getId(), offset.getRules());
            } else if (!zoneId.startsWith("UT+") && !zoneId.startsWith("UT-")) {
                return ofId(zoneId, false);
            } else {
                ZoneOffset offset2 = ZoneOffset.of(zoneId.substring(2));
                if (offset2.getTotalSeconds() == 0) {
                    return new ZoneRegion("UT", offset2.getRules());
                }
                return new ZoneRegion("UT" + offset2.getId(), offset2.getRules());
            }
        }
    }

    static ZoneRegion ofId(String zoneId, boolean checkAvailable) {
        Jdk8Methods.requireNonNull(zoneId, "zoneId");
        if (zoneId.length() < 2 || !PATTERN.matcher(zoneId).matches()) {
            throw new DateTimeException("Invalid ID for region-based ZoneId, invalid format: " + zoneId);
        }
        ZoneRules rules2 = null;
        try {
            rules2 = ZoneRulesProvider.getRules(zoneId, true);
        } catch (ZoneRulesException ex) {
            if (zoneId.equals("GMT0")) {
                rules2 = ZoneOffset.UTC.getRules();
            } else if (checkAvailable) {
                throw ex;
            }
        }
        return new ZoneRegion(zoneId, rules2);
    }

    ZoneRegion(String id2, ZoneRules rules2) {
        this.id = id2;
        this.rules = rules2;
    }

    public String getId() {
        return this.id;
    }

    public ZoneRules getRules() {
        ZoneRules zoneRules = this.rules;
        return zoneRules != null ? zoneRules : ZoneRulesProvider.getRules(this.id, false);
    }

    private Object writeReplace() {
        return new Ser((byte) 7, this);
    }

    private Object readResolve() throws ObjectStreamException {
        throw new InvalidObjectException("Deserialization via serialization delegate");
    }

    /* access modifiers changed from: package-private */
    public void write(DataOutput out) throws IOException {
        out.writeByte(7);
        writeExternal(out);
    }

    /* access modifiers changed from: package-private */
    public void writeExternal(DataOutput out) throws IOException {
        out.writeUTF(this.id);
    }

    static ZoneId readExternal(DataInput in) throws IOException {
        return ofLenient(in.readUTF());
    }
}
