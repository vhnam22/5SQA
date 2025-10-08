package org.threeten.bp.zone;

import java.io.DataInput;
import java.io.DataOutput;
import java.io.IOException;
import java.io.Serializable;
import java.util.Arrays;
import java.util.Collections;
import java.util.List;
import org.threeten.bp.Duration;
import org.threeten.bp.Instant;
import org.threeten.bp.LocalDateTime;
import org.threeten.bp.ZoneOffset;
import org.threeten.bp.jdk8.Jdk8Methods;

public final class ZoneOffsetTransition implements Comparable<ZoneOffsetTransition>, Serializable {
    private static final long serialVersionUID = -6946044323557704546L;
    private final ZoneOffset offsetAfter;
    private final ZoneOffset offsetBefore;
    private final LocalDateTime transition;

    public static ZoneOffsetTransition of(LocalDateTime transition2, ZoneOffset offsetBefore2, ZoneOffset offsetAfter2) {
        Jdk8Methods.requireNonNull(transition2, "transition");
        Jdk8Methods.requireNonNull(offsetBefore2, "offsetBefore");
        Jdk8Methods.requireNonNull(offsetAfter2, "offsetAfter");
        if (offsetBefore2.equals(offsetAfter2)) {
            throw new IllegalArgumentException("Offsets must not be equal");
        } else if (transition2.getNano() == 0) {
            return new ZoneOffsetTransition(transition2, offsetBefore2, offsetAfter2);
        } else {
            throw new IllegalArgumentException("Nano-of-second must be zero");
        }
    }

    ZoneOffsetTransition(LocalDateTime transition2, ZoneOffset offsetBefore2, ZoneOffset offsetAfter2) {
        this.transition = transition2;
        this.offsetBefore = offsetBefore2;
        this.offsetAfter = offsetAfter2;
    }

    ZoneOffsetTransition(long epochSecond, ZoneOffset offsetBefore2, ZoneOffset offsetAfter2) {
        this.transition = LocalDateTime.ofEpochSecond(epochSecond, 0, offsetBefore2);
        this.offsetBefore = offsetBefore2;
        this.offsetAfter = offsetAfter2;
    }

    private Object writeReplace() {
        return new Ser((byte) 2, this);
    }

    /* access modifiers changed from: package-private */
    public void writeExternal(DataOutput out) throws IOException {
        Ser.writeEpochSec(toEpochSecond(), out);
        Ser.writeOffset(this.offsetBefore, out);
        Ser.writeOffset(this.offsetAfter, out);
    }

    static ZoneOffsetTransition readExternal(DataInput in) throws IOException {
        long epochSecond = Ser.readEpochSec(in);
        ZoneOffset before = Ser.readOffset(in);
        ZoneOffset after = Ser.readOffset(in);
        if (!before.equals(after)) {
            return new ZoneOffsetTransition(epochSecond, before, after);
        }
        throw new IllegalArgumentException("Offsets must not be equal");
    }

    public Instant getInstant() {
        return this.transition.toInstant(this.offsetBefore);
    }

    public long toEpochSecond() {
        return this.transition.toEpochSecond(this.offsetBefore);
    }

    public LocalDateTime getDateTimeBefore() {
        return this.transition;
    }

    public LocalDateTime getDateTimeAfter() {
        return this.transition.plusSeconds((long) getDurationSeconds());
    }

    public ZoneOffset getOffsetBefore() {
        return this.offsetBefore;
    }

    public ZoneOffset getOffsetAfter() {
        return this.offsetAfter;
    }

    public Duration getDuration() {
        return Duration.ofSeconds((long) getDurationSeconds());
    }

    private int getDurationSeconds() {
        return getOffsetAfter().getTotalSeconds() - getOffsetBefore().getTotalSeconds();
    }

    public boolean isGap() {
        return getOffsetAfter().getTotalSeconds() > getOffsetBefore().getTotalSeconds();
    }

    public boolean isOverlap() {
        return getOffsetAfter().getTotalSeconds() < getOffsetBefore().getTotalSeconds();
    }

    public boolean isValidOffset(ZoneOffset offset) {
        if (isGap()) {
            return false;
        }
        return getOffsetBefore().equals(offset) || getOffsetAfter().equals(offset);
    }

    /* access modifiers changed from: package-private */
    public List<ZoneOffset> getValidOffsets() {
        if (isGap()) {
            return Collections.emptyList();
        }
        return Arrays.asList(new ZoneOffset[]{getOffsetBefore(), getOffsetAfter()});
    }

    public int compareTo(ZoneOffsetTransition transition2) {
        return getInstant().compareTo(transition2.getInstant());
    }

    public boolean equals(Object other) {
        if (other == this) {
            return true;
        }
        if (!(other instanceof ZoneOffsetTransition)) {
            return false;
        }
        ZoneOffsetTransition d = (ZoneOffsetTransition) other;
        if (!this.transition.equals(d.transition) || !this.offsetBefore.equals(d.offsetBefore) || !this.offsetAfter.equals(d.offsetAfter)) {
            return false;
        }
        return true;
    }

    public int hashCode() {
        return (this.transition.hashCode() ^ this.offsetBefore.hashCode()) ^ Integer.rotateLeft(this.offsetAfter.hashCode(), 16);
    }

    public String toString() {
        StringBuilder buf = new StringBuilder();
        buf.append("Transition[").append(isGap() ? "Gap" : "Overlap").append(" at ").append(this.transition).append(this.offsetBefore).append(" to ").append(this.offsetAfter).append(']');
        return buf.toString();
    }
}
