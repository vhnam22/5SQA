package org.threeten.bp.chrono;

import java.io.DataInput;
import java.io.DataOutput;
import java.io.IOException;
import java.io.InvalidObjectException;
import java.io.ObjectStreamException;
import java.io.Serializable;
import java.util.Arrays;
import java.util.concurrent.atomic.AtomicReference;
import org.threeten.bp.DateTimeException;
import org.threeten.bp.LocalDate;
import org.threeten.bp.jdk8.DefaultInterfaceEra;
import org.threeten.bp.jdk8.Jdk8Methods;
import org.threeten.bp.temporal.ChronoField;
import org.threeten.bp.temporal.TemporalField;
import org.threeten.bp.temporal.ValueRange;

public final class JapaneseEra extends DefaultInterfaceEra implements Serializable {
    private static final int ADDITIONAL_VALUE = 3;
    static final int ERA_OFFSET = 2;
    public static final JapaneseEra HEISEI;
    private static final AtomicReference<JapaneseEra[]> KNOWN_ERAS;
    public static final JapaneseEra MEIJI;
    public static final JapaneseEra SHOWA;
    public static final JapaneseEra TAISHO;
    private static final long serialVersionUID = 1466499369062886794L;
    private final int eraValue;
    private final transient String name;
    private final transient LocalDate since;

    static {
        JapaneseEra japaneseEra = new JapaneseEra(-1, LocalDate.of(1868, 9, 8), "Meiji");
        MEIJI = japaneseEra;
        JapaneseEra japaneseEra2 = new JapaneseEra(0, LocalDate.of(1912, 7, 30), "Taisho");
        TAISHO = japaneseEra2;
        JapaneseEra japaneseEra3 = new JapaneseEra(1, LocalDate.of(1926, 12, 25), "Showa");
        SHOWA = japaneseEra3;
        JapaneseEra japaneseEra4 = new JapaneseEra(2, LocalDate.of(1989, 1, 8), "Heisei");
        HEISEI = japaneseEra4;
        KNOWN_ERAS = new AtomicReference<>(new JapaneseEra[]{japaneseEra, japaneseEra2, japaneseEra3, japaneseEra4});
    }

    private JapaneseEra(int eraValue2, LocalDate since2, String name2) {
        this.eraValue = eraValue2;
        this.since = since2;
        this.name = name2;
    }

    private Object readResolve() throws ObjectStreamException {
        try {
            return of(this.eraValue);
        } catch (DateTimeException e) {
            InvalidObjectException ex = new InvalidObjectException("Invalid era");
            ex.initCause(e);
            throw ex;
        }
    }

    public static JapaneseEra registerEra(LocalDate since2, String name2) {
        AtomicReference<JapaneseEra[]> atomicReference = KNOWN_ERAS;
        JapaneseEra[] known = atomicReference.get();
        if (known.length <= 4) {
            Jdk8Methods.requireNonNull(since2, "since");
            Jdk8Methods.requireNonNull(name2, "name");
            if (since2.isAfter(HEISEI.since)) {
                JapaneseEra era = new JapaneseEra(3, since2, name2);
                JapaneseEra[] newArray = (JapaneseEra[]) Arrays.copyOf(known, 5);
                newArray[4] = era;
                if (JapaneseEra$$ExternalSyntheticBackportWithForwarding0.m(atomicReference, known, newArray)) {
                    return era;
                }
                throw new DateTimeException("Only one additional Japanese era can be added");
            }
            throw new DateTimeException("Invalid since date for additional Japanese era, must be after Heisei");
        }
        throw new DateTimeException("Only one additional Japanese era can be added");
    }

    public static JapaneseEra of(int japaneseEra) {
        JapaneseEra[] known = KNOWN_ERAS.get();
        if (japaneseEra >= MEIJI.eraValue && japaneseEra <= known[known.length - 1].eraValue) {
            return known[ordinal(japaneseEra)];
        }
        throw new DateTimeException("japaneseEra is invalid");
    }

    public static JapaneseEra valueOf(String japaneseEra) {
        Jdk8Methods.requireNonNull(japaneseEra, "japaneseEra");
        for (JapaneseEra era : KNOWN_ERAS.get()) {
            if (japaneseEra.equals(era.name)) {
                return era;
            }
        }
        throw new IllegalArgumentException("Era not found: " + japaneseEra);
    }

    public static JapaneseEra[] values() {
        JapaneseEra[] known = KNOWN_ERAS.get();
        return (JapaneseEra[]) Arrays.copyOf(known, known.length);
    }

    static JapaneseEra from(LocalDate date) {
        if (!date.isBefore(MEIJI.since)) {
            JapaneseEra[] known = KNOWN_ERAS.get();
            for (int i = known.length - 1; i >= 0; i--) {
                JapaneseEra era = known[i];
                if (date.compareTo((ChronoLocalDate) era.since) >= 0) {
                    return era;
                }
            }
            return null;
        }
        throw new DateTimeException("Date too early: " + date);
    }

    private static int ordinal(int eraValue2) {
        return eraValue2 + 1;
    }

    /* access modifiers changed from: package-private */
    public LocalDate startDate() {
        return this.since;
    }

    /* access modifiers changed from: package-private */
    public LocalDate endDate() {
        int ordinal = ordinal(this.eraValue);
        JapaneseEra[] eras = values();
        if (ordinal >= eras.length - 1) {
            return LocalDate.MAX;
        }
        return eras[ordinal + 1].startDate().minusDays(1);
    }

    public int getValue() {
        return this.eraValue;
    }

    public ValueRange range(TemporalField field) {
        if (field == ChronoField.ERA) {
            return JapaneseChronology.INSTANCE.range(ChronoField.ERA);
        }
        return super.range(field);
    }

    public String toString() {
        return this.name;
    }

    private Object writeReplace() {
        return new Ser((byte) 2, this);
    }

    /* access modifiers changed from: package-private */
    public void writeExternal(DataOutput out) throws IOException {
        out.writeByte(getValue());
    }

    static JapaneseEra readExternal(DataInput in) throws IOException {
        return of(in.readByte());
    }
}
