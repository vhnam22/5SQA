package org.threeten.bp.zone;

import java.io.DataInput;
import java.io.DataOutput;
import java.io.IOException;
import java.io.Serializable;
import java.util.ArrayList;
import java.util.Arrays;
import java.util.Collections;
import java.util.List;
import java.util.concurrent.ConcurrentHashMap;
import java.util.concurrent.ConcurrentMap;
import org.threeten.bp.Duration;
import org.threeten.bp.Instant;
import org.threeten.bp.LocalDate;
import org.threeten.bp.LocalDateTime;
import org.threeten.bp.ZoneOffset;
import org.threeten.bp.jdk8.Jdk8Methods;
import org.threeten.bp.zone.ZoneRules;

final class StandardZoneRules extends ZoneRules implements Serializable {
    private static final int LAST_CACHED_YEAR = 2100;
    private static final long serialVersionUID = 3044319355680032515L;
    private final ZoneOffsetTransitionRule[] lastRules;
    private final ConcurrentMap<Integer, ZoneOffsetTransition[]> lastRulesCache = new ConcurrentHashMap();
    private final long[] savingsInstantTransitions;
    private final LocalDateTime[] savingsLocalTransitions;
    private final ZoneOffset[] standardOffsets;
    private final long[] standardTransitions;
    private final ZoneOffset[] wallOffsets;

    StandardZoneRules(ZoneOffset baseStandardOffset, ZoneOffset baseWallOffset, List<ZoneOffsetTransition> standardOffsetTransitionList, List<ZoneOffsetTransition> transitionList, List<ZoneOffsetTransitionRule> lastRules2) {
        this.standardTransitions = new long[standardOffsetTransitionList.size()];
        ZoneOffset[] zoneOffsetArr = new ZoneOffset[(standardOffsetTransitionList.size() + 1)];
        this.standardOffsets = zoneOffsetArr;
        zoneOffsetArr[0] = baseStandardOffset;
        for (int i = 0; i < standardOffsetTransitionList.size(); i++) {
            this.standardTransitions[i] = standardOffsetTransitionList.get(i).toEpochSecond();
            this.standardOffsets[i + 1] = standardOffsetTransitionList.get(i).getOffsetAfter();
        }
        List<LocalDateTime> localTransitionList = new ArrayList<>();
        List<ZoneOffset> localTransitionOffsetList = new ArrayList<>();
        localTransitionOffsetList.add(baseWallOffset);
        for (ZoneOffsetTransition trans : transitionList) {
            if (trans.isGap()) {
                localTransitionList.add(trans.getDateTimeBefore());
                localTransitionList.add(trans.getDateTimeAfter());
            } else {
                localTransitionList.add(trans.getDateTimeAfter());
                localTransitionList.add(trans.getDateTimeBefore());
            }
            localTransitionOffsetList.add(trans.getOffsetAfter());
        }
        this.savingsLocalTransitions = (LocalDateTime[]) localTransitionList.toArray(new LocalDateTime[localTransitionList.size()]);
        this.wallOffsets = (ZoneOffset[]) localTransitionOffsetList.toArray(new ZoneOffset[localTransitionOffsetList.size()]);
        this.savingsInstantTransitions = new long[transitionList.size()];
        for (int i2 = 0; i2 < transitionList.size(); i2++) {
            this.savingsInstantTransitions[i2] = transitionList.get(i2).getInstant().getEpochSecond();
        }
        if (lastRules2.size() <= 15) {
            this.lastRules = (ZoneOffsetTransitionRule[]) lastRules2.toArray(new ZoneOffsetTransitionRule[lastRules2.size()]);
            return;
        }
        throw new IllegalArgumentException("Too many transition rules");
    }

    private StandardZoneRules(long[] standardTransitions2, ZoneOffset[] standardOffsets2, long[] savingsInstantTransitions2, ZoneOffset[] wallOffsets2, ZoneOffsetTransitionRule[] lastRules2) {
        this.standardTransitions = standardTransitions2;
        this.standardOffsets = standardOffsets2;
        this.savingsInstantTransitions = savingsInstantTransitions2;
        this.wallOffsets = wallOffsets2;
        this.lastRules = lastRules2;
        List<LocalDateTime> localTransitionList = new ArrayList<>();
        for (int i = 0; i < savingsInstantTransitions2.length; i++) {
            ZoneOffsetTransition trans = new ZoneOffsetTransition(savingsInstantTransitions2[i], wallOffsets2[i], wallOffsets2[i + 1]);
            if (trans.isGap()) {
                localTransitionList.add(trans.getDateTimeBefore());
                localTransitionList.add(trans.getDateTimeAfter());
            } else {
                localTransitionList.add(trans.getDateTimeAfter());
                localTransitionList.add(trans.getDateTimeBefore());
            }
        }
        this.savingsLocalTransitions = (LocalDateTime[]) localTransitionList.toArray(new LocalDateTime[localTransitionList.size()]);
    }

    private Object writeReplace() {
        return new Ser((byte) 1, this);
    }

    /* access modifiers changed from: package-private */
    public void writeExternal(DataOutput out) throws IOException {
        out.writeInt(this.standardTransitions.length);
        for (long trans : this.standardTransitions) {
            Ser.writeEpochSec(trans, out);
        }
        for (ZoneOffset offset : this.standardOffsets) {
            Ser.writeOffset(offset, out);
        }
        out.writeInt(this.savingsInstantTransitions.length);
        for (long trans2 : this.savingsInstantTransitions) {
            Ser.writeEpochSec(trans2, out);
        }
        for (ZoneOffset offset2 : this.wallOffsets) {
            Ser.writeOffset(offset2, out);
        }
        out.writeByte(this.lastRules.length);
        for (ZoneOffsetTransitionRule rule : this.lastRules) {
            rule.writeExternal(out);
        }
    }

    static StandardZoneRules readExternal(DataInput in) throws IOException, ClassNotFoundException {
        int stdSize = in.readInt();
        long[] stdTrans = new long[stdSize];
        for (int i = 0; i < stdSize; i++) {
            stdTrans[i] = Ser.readEpochSec(in);
        }
        ZoneOffset[] stdOffsets = new ZoneOffset[(stdSize + 1)];
        for (int i2 = 0; i2 < stdOffsets.length; i2++) {
            stdOffsets[i2] = Ser.readOffset(in);
        }
        int savSize = in.readInt();
        long[] savTrans = new long[savSize];
        for (int i3 = 0; i3 < savSize; i3++) {
            savTrans[i3] = Ser.readEpochSec(in);
        }
        ZoneOffset[] savOffsets = new ZoneOffset[(savSize + 1)];
        for (int i4 = 0; i4 < savOffsets.length; i4++) {
            savOffsets[i4] = Ser.readOffset(in);
        }
        int ruleSize = in.readByte();
        ZoneOffsetTransitionRule[] rules = new ZoneOffsetTransitionRule[ruleSize];
        for (int i5 = 0; i5 < ruleSize; i5++) {
            rules[i5] = ZoneOffsetTransitionRule.readExternal(in);
        }
        return new StandardZoneRules(stdTrans, stdOffsets, savTrans, savOffsets, rules);
    }

    public boolean isFixedOffset() {
        return this.savingsInstantTransitions.length == 0;
    }

    public ZoneOffset getOffset(Instant instant) {
        long epochSec = instant.getEpochSecond();
        if (this.lastRules.length > 0) {
            long[] jArr = this.savingsInstantTransitions;
            if (epochSec > jArr[jArr.length - 1]) {
                ZoneOffset[] zoneOffsetArr = this.wallOffsets;
                ZoneOffsetTransition[] transArray = findTransitionArray(findYear(epochSec, zoneOffsetArr[zoneOffsetArr.length - 1]));
                ZoneOffsetTransition trans = null;
                for (int i = 0; i < transArray.length; i++) {
                    trans = transArray[i];
                    if (epochSec < trans.toEpochSecond()) {
                        return trans.getOffsetBefore();
                    }
                }
                return trans.getOffsetAfter();
            }
        }
        int index = Arrays.binarySearch(this.savingsInstantTransitions, epochSec);
        if (index < 0) {
            index = (-index) - 2;
        }
        return this.wallOffsets[index + 1];
    }

    public ZoneOffset getOffset(LocalDateTime localDateTime) {
        Object info = getOffsetInfo(localDateTime);
        if (info instanceof ZoneOffsetTransition) {
            return ((ZoneOffsetTransition) info).getOffsetBefore();
        }
        return (ZoneOffset) info;
    }

    public List<ZoneOffset> getValidOffsets(LocalDateTime localDateTime) {
        Object info = getOffsetInfo(localDateTime);
        if (info instanceof ZoneOffsetTransition) {
            return ((ZoneOffsetTransition) info).getValidOffsets();
        }
        return Collections.singletonList((ZoneOffset) info);
    }

    public ZoneOffsetTransition getTransition(LocalDateTime localDateTime) {
        Object info = getOffsetInfo(localDateTime);
        if (info instanceof ZoneOffsetTransition) {
            return (ZoneOffsetTransition) info;
        }
        return null;
    }

    private Object getOffsetInfo(LocalDateTime dt) {
        if (this.lastRules.length > 0) {
            LocalDateTime[] localDateTimeArr = this.savingsLocalTransitions;
            if (dt.isAfter(localDateTimeArr[localDateTimeArr.length - 1])) {
                Object info = null;
                for (ZoneOffsetTransition trans : findTransitionArray(dt.getYear())) {
                    info = findOffsetInfo(dt, trans);
                    if ((info instanceof ZoneOffsetTransition) || info.equals(trans.getOffsetBefore())) {
                        return info;
                    }
                }
                return info;
            }
        }
        int index = Arrays.binarySearch(this.savingsLocalTransitions, dt);
        if (index == -1) {
            return this.wallOffsets[0];
        }
        if (index < 0) {
            index = (-index) - 2;
        } else {
            LocalDateTime[] localDateTimeArr2 = this.savingsLocalTransitions;
            if (index < localDateTimeArr2.length - 1 && localDateTimeArr2[index].equals(localDateTimeArr2[index + 1])) {
                index++;
            }
        }
        if ((index & 1) != 0) {
            return this.wallOffsets[(index / 2) + 1];
        }
        LocalDateTime[] localDateTimeArr3 = this.savingsLocalTransitions;
        LocalDateTime dtBefore = localDateTimeArr3[index];
        LocalDateTime dtAfter = localDateTimeArr3[index + 1];
        ZoneOffset[] zoneOffsetArr = this.wallOffsets;
        ZoneOffset offsetBefore = zoneOffsetArr[index / 2];
        ZoneOffset offsetAfter = zoneOffsetArr[(index / 2) + 1];
        if (offsetAfter.getTotalSeconds() > offsetBefore.getTotalSeconds()) {
            return new ZoneOffsetTransition(dtBefore, offsetBefore, offsetAfter);
        }
        return new ZoneOffsetTransition(dtAfter, offsetBefore, offsetAfter);
    }

    private Object findOffsetInfo(LocalDateTime dt, ZoneOffsetTransition trans) {
        LocalDateTime localTransition = trans.getDateTimeBefore();
        if (trans.isGap()) {
            if (dt.isBefore(localTransition)) {
                return trans.getOffsetBefore();
            }
            if (dt.isBefore(trans.getDateTimeAfter())) {
                return trans;
            }
            return trans.getOffsetAfter();
        } else if (!dt.isBefore(localTransition)) {
            return trans.getOffsetAfter();
        } else {
            if (dt.isBefore(trans.getDateTimeAfter())) {
                return trans.getOffsetBefore();
            }
            return trans;
        }
    }

    public boolean isValidOffset(LocalDateTime localDateTime, ZoneOffset offset) {
        return getValidOffsets(localDateTime).contains(offset);
    }

    private ZoneOffsetTransition[] findTransitionArray(int year) {
        Integer yearObj = Integer.valueOf(year);
        ZoneOffsetTransition[] transArray = (ZoneOffsetTransition[]) this.lastRulesCache.get(yearObj);
        if (transArray != null) {
            return transArray;
        }
        ZoneOffsetTransitionRule[] ruleArray = this.lastRules;
        ZoneOffsetTransition[] transArray2 = new ZoneOffsetTransition[ruleArray.length];
        for (int i = 0; i < ruleArray.length; i++) {
            transArray2[i] = ruleArray[i].createTransition(year);
        }
        if (year < LAST_CACHED_YEAR) {
            this.lastRulesCache.putIfAbsent(yearObj, transArray2);
        }
        return transArray2;
    }

    public ZoneOffset getStandardOffset(Instant instant) {
        int index = Arrays.binarySearch(this.standardTransitions, instant.getEpochSecond());
        if (index < 0) {
            index = (-index) - 2;
        }
        return this.standardOffsets[index + 1];
    }

    public Duration getDaylightSavings(Instant instant) {
        return Duration.ofSeconds((long) (getOffset(instant).getTotalSeconds() - getStandardOffset(instant).getTotalSeconds()));
    }

    public boolean isDaylightSavings(Instant instant) {
        return !getStandardOffset(instant).equals(getOffset(instant));
    }

    public ZoneOffsetTransition nextTransition(Instant instant) {
        int index;
        if (this.savingsInstantTransitions.length == 0) {
            return null;
        }
        long epochSec = instant.getEpochSecond();
        long[] jArr = this.savingsInstantTransitions;
        if (epochSec < jArr[jArr.length - 1]) {
            int index2 = Arrays.binarySearch(jArr, epochSec);
            if (index2 < 0) {
                index = (-index2) - 1;
            } else {
                index = index2 + 1;
            }
            long j = this.savingsInstantTransitions[index];
            ZoneOffset[] zoneOffsetArr = this.wallOffsets;
            return new ZoneOffsetTransition(j, zoneOffsetArr[index], zoneOffsetArr[index + 1]);
        } else if (this.lastRules.length == 0) {
            return null;
        } else {
            ZoneOffset[] zoneOffsetArr2 = this.wallOffsets;
            int year = findYear(epochSec, zoneOffsetArr2[zoneOffsetArr2.length - 1]);
            for (ZoneOffsetTransition trans : findTransitionArray(year)) {
                if (epochSec < trans.toEpochSecond()) {
                    return trans;
                }
            }
            if (year < 999999999) {
                return findTransitionArray(year + 1)[0];
            }
            return null;
        }
    }

    public ZoneOffsetTransition previousTransition(Instant instant) {
        if (this.savingsInstantTransitions.length == 0) {
            return null;
        }
        long epochSec = instant.getEpochSecond();
        if (instant.getNano() > 0 && epochSec < Long.MAX_VALUE) {
            epochSec++;
        }
        long[] jArr = this.savingsInstantTransitions;
        long lastHistoric = jArr[jArr.length - 1];
        if (this.lastRules.length > 0 && epochSec > lastHistoric) {
            ZoneOffset[] zoneOffsetArr = this.wallOffsets;
            ZoneOffset lastHistoricOffset = zoneOffsetArr[zoneOffsetArr.length - 1];
            int year = findYear(epochSec, lastHistoricOffset);
            ZoneOffsetTransition[] transArray = findTransitionArray(year);
            for (int i = transArray.length - 1; i >= 0; i--) {
                if (epochSec > transArray[i].toEpochSecond()) {
                    return transArray[i];
                }
            }
            int year2 = year - 1;
            if (year2 > findYear(lastHistoric, lastHistoricOffset)) {
                ZoneOffsetTransition[] transArray2 = findTransitionArray(year2);
                return transArray2[transArray2.length - 1];
            }
        }
        int index = Arrays.binarySearch(this.savingsInstantTransitions, epochSec);
        if (index < 0) {
            index = (-index) - 1;
        }
        if (index <= 0) {
            return null;
        }
        long j = this.savingsInstantTransitions[index - 1];
        ZoneOffset[] zoneOffsetArr2 = this.wallOffsets;
        return new ZoneOffsetTransition(j, zoneOffsetArr2[index - 1], zoneOffsetArr2[index]);
    }

    private int findYear(long epochSecond, ZoneOffset offset) {
        return LocalDate.ofEpochDay(Jdk8Methods.floorDiv(((long) offset.getTotalSeconds()) + epochSecond, 86400)).getYear();
    }

    public List<ZoneOffsetTransition> getTransitions() {
        List<ZoneOffsetTransition> list = new ArrayList<>();
        int i = 0;
        while (true) {
            long[] jArr = this.savingsInstantTransitions;
            if (i >= jArr.length) {
                return Collections.unmodifiableList(list);
            }
            long j = jArr[i];
            ZoneOffset[] zoneOffsetArr = this.wallOffsets;
            list.add(new ZoneOffsetTransition(j, zoneOffsetArr[i], zoneOffsetArr[i + 1]));
            i++;
        }
    }

    public List<ZoneOffsetTransitionRule> getTransitionRules() {
        return Collections.unmodifiableList(Arrays.asList(this.lastRules));
    }

    public boolean equals(Object obj) {
        if (this == obj) {
            return true;
        }
        if (obj instanceof StandardZoneRules) {
            StandardZoneRules other = (StandardZoneRules) obj;
            if (!Arrays.equals(this.standardTransitions, other.standardTransitions) || !Arrays.equals(this.standardOffsets, other.standardOffsets) || !Arrays.equals(this.savingsInstantTransitions, other.savingsInstantTransitions) || !Arrays.equals(this.wallOffsets, other.wallOffsets) || !Arrays.equals(this.lastRules, other.lastRules)) {
                return false;
            }
            return true;
        } else if (!(obj instanceof ZoneRules.Fixed) || !isFixedOffset() || !getOffset(Instant.EPOCH).equals(((ZoneRules.Fixed) obj).getOffset(Instant.EPOCH))) {
            return false;
        } else {
            return true;
        }
    }

    public int hashCode() {
        return (((Arrays.hashCode(this.standardTransitions) ^ Arrays.hashCode(this.standardOffsets)) ^ Arrays.hashCode(this.savingsInstantTransitions)) ^ Arrays.hashCode(this.wallOffsets)) ^ Arrays.hashCode(this.lastRules);
    }

    public String toString() {
        StringBuilder append = new StringBuilder().append("StandardZoneRules[currentStandardOffset=");
        ZoneOffset[] zoneOffsetArr = this.standardOffsets;
        return append.append(zoneOffsetArr[zoneOffsetArr.length - 1]).append("]").toString();
    }
}
