package org.threeten.bp;

import java.io.DataInput;
import java.io.DataOutput;
import java.io.Externalizable;
import java.io.IOException;
import java.io.InvalidClassException;
import java.io.ObjectInput;
import java.io.ObjectOutput;
import java.io.StreamCorruptedException;

final class Ser implements Externalizable {
    static final byte DURATION_TYPE = 1;
    static final byte INSTANT_TYPE = 2;
    static final byte LOCAL_DATE_TIME_TYPE = 4;
    static final byte LOCAL_DATE_TYPE = 3;
    static final byte LOCAL_TIME_TYPE = 5;
    static final byte MONTH_DAY_TYPE = 64;
    static final byte OFFSET_DATE_TIME_TYPE = 69;
    static final byte OFFSET_TIME_TYPE = 66;
    static final byte YEAR_MONTH_TYPE = 68;
    static final byte YEAR_TYPE = 67;
    static final byte ZONED_DATE_TIME_TYPE = 6;
    static final byte ZONE_OFFSET_TYPE = 8;
    static final byte ZONE_REGION_TYPE = 7;
    private static final long serialVersionUID = -7683839454370182990L;
    private Object object;
    private byte type;

    public Ser() {
    }

    Ser(byte type2, Object object2) {
        this.type = type2;
        this.object = object2;
    }

    public void writeExternal(ObjectOutput out) throws IOException {
        writeInternal(this.type, this.object, out);
    }

    static void writeInternal(byte type2, Object object2, DataOutput out) throws IOException {
        out.writeByte(type2);
        switch (type2) {
            case 1:
                ((Duration) object2).writeExternal(out);
                return;
            case 2:
                ((Instant) object2).writeExternal(out);
                return;
            case 3:
                ((LocalDate) object2).writeExternal(out);
                return;
            case 4:
                ((LocalDateTime) object2).writeExternal(out);
                return;
            case 5:
                ((LocalTime) object2).writeExternal(out);
                return;
            case 6:
                ((ZonedDateTime) object2).writeExternal(out);
                return;
            case 7:
                ((ZoneRegion) object2).writeExternal(out);
                return;
            case 8:
                ((ZoneOffset) object2).writeExternal(out);
                return;
            case 64:
                ((MonthDay) object2).writeExternal(out);
                return;
            case 66:
                ((OffsetTime) object2).writeExternal(out);
                return;
            case 67:
                ((Year) object2).writeExternal(out);
                return;
            case 68:
                ((YearMonth) object2).writeExternal(out);
                return;
            case 69:
                ((OffsetDateTime) object2).writeExternal(out);
                return;
            default:
                throw new InvalidClassException("Unknown serialized type");
        }
    }

    public void readExternal(ObjectInput in) throws IOException {
        byte readByte = in.readByte();
        this.type = readByte;
        this.object = readInternal(readByte, in);
    }

    static Object read(DataInput in) throws IOException {
        return readInternal(in.readByte(), in);
    }

    private static Object readInternal(byte type2, DataInput in) throws IOException {
        switch (type2) {
            case 1:
                return Duration.readExternal(in);
            case 2:
                return Instant.readExternal(in);
            case 3:
                return LocalDate.readExternal(in);
            case 4:
                return LocalDateTime.readExternal(in);
            case 5:
                return LocalTime.readExternal(in);
            case 6:
                return ZonedDateTime.readExternal(in);
            case 7:
                return ZoneRegion.readExternal(in);
            case 8:
                return ZoneOffset.readExternal(in);
            case 64:
                return MonthDay.readExternal(in);
            case 66:
                return OffsetTime.readExternal(in);
            case 67:
                return Year.readExternal(in);
            case 68:
                return YearMonth.readExternal(in);
            case 69:
                return OffsetDateTime.readExternal(in);
            default:
                throw new StreamCorruptedException("Unknown serialized type");
        }
    }

    private Object readResolve() {
        return this.object;
    }
}
