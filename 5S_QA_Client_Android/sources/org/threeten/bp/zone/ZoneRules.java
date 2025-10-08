package org.threeten.bp.zone;

import java.io.Serializable;
import java.util.Collections;
import java.util.List;
import org.threeten.bp.Duration;
import org.threeten.bp.Instant;
import org.threeten.bp.LocalDateTime;
import org.threeten.bp.ZoneOffset;
import org.threeten.bp.jdk8.Jdk8Methods;

public abstract class ZoneRules {
    public abstract boolean equals(Object obj);

    public abstract Duration getDaylightSavings(Instant instant);

    public abstract ZoneOffset getOffset(Instant instant);

    public abstract ZoneOffset getOffset(LocalDateTime localDateTime);

    public abstract ZoneOffset getStandardOffset(Instant instant);

    public abstract ZoneOffsetTransition getTransition(LocalDateTime localDateTime);

    public abstract List<ZoneOffsetTransitionRule> getTransitionRules();

    public abstract List<ZoneOffsetTransition> getTransitions();

    public abstract List<ZoneOffset> getValidOffsets(LocalDateTime localDateTime);

    public abstract int hashCode();

    public abstract boolean isDaylightSavings(Instant instant);

    public abstract boolean isFixedOffset();

    public abstract boolean isValidOffset(LocalDateTime localDateTime, ZoneOffset zoneOffset);

    public abstract ZoneOffsetTransition nextTransition(Instant instant);

    public abstract ZoneOffsetTransition previousTransition(Instant instant);

    public static ZoneRules of(ZoneOffset baseStandardOffset, ZoneOffset baseWallOffset, List<ZoneOffsetTransition> standardOffsetTransitionList, List<ZoneOffsetTransition> transitionList, List<ZoneOffsetTransitionRule> lastRules) {
        Jdk8Methods.requireNonNull(baseStandardOffset, "baseStandardOffset");
        Jdk8Methods.requireNonNull(baseWallOffset, "baseWallOffset");
        Jdk8Methods.requireNonNull(standardOffsetTransitionList, "standardOffsetTransitionList");
        Jdk8Methods.requireNonNull(transitionList, "transitionList");
        Jdk8Methods.requireNonNull(lastRules, "lastRules");
        return new StandardZoneRules(baseStandardOffset, baseWallOffset, standardOffsetTransitionList, transitionList, lastRules);
    }

    public static ZoneRules of(ZoneOffset offset) {
        Jdk8Methods.requireNonNull(offset, "offset");
        return new Fixed(offset);
    }

    ZoneRules() {
    }

    static final class Fixed extends ZoneRules implements Serializable {
        private static final long serialVersionUID = -8733721350312276297L;
        private final ZoneOffset offset;

        Fixed(ZoneOffset offset2) {
            this.offset = offset2;
        }

        public boolean isFixedOffset() {
            return true;
        }

        public ZoneOffset getOffset(Instant instant) {
            return this.offset;
        }

        public ZoneOffset getOffset(LocalDateTime localDateTime) {
            return this.offset;
        }

        public List<ZoneOffset> getValidOffsets(LocalDateTime localDateTime) {
            return Collections.singletonList(this.offset);
        }

        public ZoneOffsetTransition getTransition(LocalDateTime localDateTime) {
            return null;
        }

        public boolean isValidOffset(LocalDateTime dateTime, ZoneOffset offset2) {
            return this.offset.equals(offset2);
        }

        public ZoneOffset getStandardOffset(Instant instant) {
            return this.offset;
        }

        public Duration getDaylightSavings(Instant instant) {
            return Duration.ZERO;
        }

        public boolean isDaylightSavings(Instant instant) {
            return false;
        }

        public ZoneOffsetTransition nextTransition(Instant instant) {
            return null;
        }

        public ZoneOffsetTransition previousTransition(Instant instant) {
            return null;
        }

        public List<ZoneOffsetTransition> getTransitions() {
            return Collections.emptyList();
        }

        public List<ZoneOffsetTransitionRule> getTransitionRules() {
            return Collections.emptyList();
        }

        public boolean equals(Object obj) {
            if (this == obj) {
                return true;
            }
            if (obj instanceof Fixed) {
                return this.offset.equals(((Fixed) obj).offset);
            }
            if (!(obj instanceof StandardZoneRules)) {
                return false;
            }
            StandardZoneRules szr = (StandardZoneRules) obj;
            if (!szr.isFixedOffset() || !this.offset.equals(szr.getOffset(Instant.EPOCH))) {
                return false;
            }
            return true;
        }

        public int hashCode() {
            return ((((this.offset.hashCode() + 31) ^ 1) ^ 1) ^ (this.offset.hashCode() + 31)) ^ 1;
        }

        public String toString() {
            return "FixedRules:" + this.offset;
        }
    }
}
