package org.threeten.bp.chrono;

import java.io.Externalizable;
import java.io.IOException;
import java.io.InvalidClassException;
import java.io.ObjectInput;
import java.io.ObjectOutput;
import java.io.StreamCorruptedException;

final class Ser implements Externalizable {
    static final byte CHRONO_LOCALDATETIME_TYPE = 12;
    static final byte CHRONO_TYPE = 11;
    static final byte CHRONO_ZONEDDATETIME_TYPE = 13;
    static final byte HIJRAH_DATE_TYPE = 3;
    static final byte HIJRAH_ERA_TYPE = 4;
    static final byte JAPANESE_DATE_TYPE = 1;
    static final byte JAPANESE_ERA_TYPE = 2;
    static final byte MINGUO_DATE_TYPE = 5;
    static final byte MINGUO_ERA_TYPE = 6;
    static final byte THAIBUDDHIST_DATE_TYPE = 7;
    static final byte THAIBUDDHIST_ERA_TYPE = 8;
    private static final long serialVersionUID = 7857518227608961174L;
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

    private static void writeInternal(byte type2, Object object2, ObjectOutput out) throws IOException {
        out.writeByte(type2);
        switch (type2) {
            case 1:
                ((JapaneseDate) object2).writeExternal(out);
                return;
            case 2:
                ((JapaneseEra) object2).writeExternal(out);
                return;
            case 3:
                ((HijrahDate) object2).writeExternal(out);
                return;
            case 4:
                ((HijrahEra) object2).writeExternal(out);
                return;
            case 5:
                ((MinguoDate) object2).writeExternal(out);
                return;
            case 6:
                ((MinguoEra) object2).writeExternal(out);
                return;
            case 7:
                ((ThaiBuddhistDate) object2).writeExternal(out);
                return;
            case 8:
                ((ThaiBuddhistEra) object2).writeExternal(out);
                return;
            case 11:
                ((Chronology) object2).writeExternal(out);
                return;
            case 12:
                ((ChronoLocalDateTimeImpl) object2).writeExternal(out);
                return;
            case 13:
                ((ChronoZonedDateTimeImpl) object2).writeExternal(out);
                return;
            default:
                throw new InvalidClassException("Unknown serialized type");
        }
    }

    public void readExternal(ObjectInput in) throws IOException, ClassNotFoundException {
        byte readByte = in.readByte();
        this.type = readByte;
        this.object = readInternal(readByte, in);
    }

    static Object read(ObjectInput in) throws IOException, ClassNotFoundException {
        return readInternal(in.readByte(), in);
    }

    private static Object readInternal(byte type2, ObjectInput in) throws IOException, ClassNotFoundException {
        switch (type2) {
            case 1:
                return JapaneseDate.readExternal(in);
            case 2:
                return JapaneseEra.readExternal(in);
            case 3:
                return HijrahDate.readExternal(in);
            case 4:
                return HijrahEra.readExternal(in);
            case 5:
                return MinguoDate.readExternal(in);
            case 6:
                return MinguoEra.readExternal(in);
            case 7:
                return ThaiBuddhistDate.readExternal(in);
            case 8:
                return ThaiBuddhistEra.readExternal(in);
            case 11:
                return Chronology.readExternal(in);
            case 12:
                return ChronoLocalDateTimeImpl.readExternal(in);
            case 13:
                return ChronoZonedDateTimeImpl.readExternal(in);
            default:
                throw new StreamCorruptedException("Unknown serialized type");
        }
    }

    private Object readResolve() {
        return this.object;
    }
}
