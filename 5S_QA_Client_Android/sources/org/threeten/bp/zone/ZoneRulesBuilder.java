package org.threeten.bp.zone;

import java.util.ArrayList;
import java.util.Collections;
import java.util.HashMap;
import java.util.List;
import java.util.Map;
import org.threeten.bp.DayOfWeek;
import org.threeten.bp.LocalDate;
import org.threeten.bp.LocalDateTime;
import org.threeten.bp.LocalTime;
import org.threeten.bp.Month;
import org.threeten.bp.Year;
import org.threeten.bp.ZoneOffset;
import org.threeten.bp.chrono.ChronoLocalDate;
import org.threeten.bp.chrono.IsoChronology;
import org.threeten.bp.jdk8.Jdk8Methods;
import org.threeten.bp.temporal.ChronoField;
import org.threeten.bp.temporal.TemporalAdjusters;
import org.threeten.bp.zone.ZoneOffsetTransitionRule;

class ZoneRulesBuilder {
    private Map<Object, Object> deduplicateMap;
    private List<TZWindow> windowList = new ArrayList();

    public ZoneRulesBuilder addWindow(ZoneOffset standardOffset, LocalDateTime until, ZoneOffsetTransitionRule.TimeDefinition untilDefinition) {
        Jdk8Methods.requireNonNull(standardOffset, "standardOffset");
        Jdk8Methods.requireNonNull(until, "until");
        Jdk8Methods.requireNonNull(untilDefinition, "untilDefinition");
        TZWindow window = new TZWindow(standardOffset, until, untilDefinition);
        if (this.windowList.size() > 0) {
            List<TZWindow> list = this.windowList;
            window.validateWindowOrder(list.get(list.size() - 1));
        }
        this.windowList.add(window);
        return this;
    }

    public ZoneRulesBuilder addWindowForever(ZoneOffset standardOffset) {
        return addWindow(standardOffset, LocalDateTime.MAX, ZoneOffsetTransitionRule.TimeDefinition.WALL);
    }

    public ZoneRulesBuilder setFixedSavingsToWindow(int fixedSavingAmountSecs) {
        if (!this.windowList.isEmpty()) {
            List<TZWindow> list = this.windowList;
            list.get(list.size() - 1).setFixedSavings(fixedSavingAmountSecs);
            return this;
        }
        throw new IllegalStateException("Must add a window before setting the fixed savings");
    }

    public ZoneRulesBuilder addRuleToWindow(LocalDateTime transitionDateTime, ZoneOffsetTransitionRule.TimeDefinition timeDefinition, int savingAmountSecs) {
        Jdk8Methods.requireNonNull(transitionDateTime, "transitionDateTime");
        return addRuleToWindow(transitionDateTime.getYear(), transitionDateTime.getYear(), transitionDateTime.getMonth(), transitionDateTime.getDayOfMonth(), (DayOfWeek) null, transitionDateTime.toLocalTime(), false, timeDefinition, savingAmountSecs);
    }

    public ZoneRulesBuilder addRuleToWindow(int year, Month month, int dayOfMonthIndicator, LocalTime time, boolean timeEndOfDay, ZoneOffsetTransitionRule.TimeDefinition timeDefinition, int savingAmountSecs) {
        return addRuleToWindow(year, year, month, dayOfMonthIndicator, (DayOfWeek) null, time, timeEndOfDay, timeDefinition, savingAmountSecs);
    }

    public ZoneRulesBuilder addRuleToWindow(int startYear, int endYear, Month month, int dayOfMonthIndicator, DayOfWeek dayOfWeek, LocalTime time, boolean timeEndOfDay, ZoneOffsetTransitionRule.TimeDefinition timeDefinition, int savingAmountSecs) {
        int i = dayOfMonthIndicator;
        LocalTime localTime = time;
        Jdk8Methods.requireNonNull(month, "month");
        Jdk8Methods.requireNonNull(localTime, "time");
        Jdk8Methods.requireNonNull(timeDefinition, "timeDefinition");
        ChronoField.YEAR.checkValidValue((long) startYear);
        ChronoField.YEAR.checkValidValue((long) endYear);
        if (i < -28 || i > 31 || i == 0) {
            throw new IllegalArgumentException("Day of month indicator must be between -28 and 31 inclusive excluding zero");
        } else if (timeEndOfDay && !localTime.equals(LocalTime.MIDNIGHT)) {
            throw new IllegalArgumentException("Time must be midnight when end of day flag is true");
        } else if (!this.windowList.isEmpty()) {
            List<TZWindow> list = this.windowList;
            list.get(list.size() - 1).addRule(startYear, endYear, month, dayOfMonthIndicator, dayOfWeek, time, timeEndOfDay, timeDefinition, savingAmountSecs);
            return this;
        } else {
            throw new IllegalStateException("Must add a window before adding a rule");
        }
    }

    public ZoneRules toRules(String zoneId) {
        return toRules(zoneId, new HashMap());
    }

    /* JADX DEBUG: Multi-variable search result rejected for TypeSearchVarInfo{r1v13, resolved type: java.lang.Object} */
    /* JADX DEBUG: Multi-variable search result rejected for TypeSearchVarInfo{r7v5, resolved type: org.threeten.bp.ZoneOffset} */
    /* JADX DEBUG: Multi-variable search result rejected for TypeSearchVarInfo{r1v16, resolved type: java.lang.Object} */
    /* JADX DEBUG: Multi-variable search result rejected for TypeSearchVarInfo{r8v11, resolved type: org.threeten.bp.LocalDateTime} */
    /* access modifiers changed from: package-private */
    /* JADX WARNING: Multi-variable type inference failed */
    /* Code decompiled incorrectly, please refer to instructions dump. */
    public org.threeten.bp.zone.ZoneRules toRules(java.lang.String r26, java.util.Map<java.lang.Object, java.lang.Object> r27) {
        /*
            r25 = this;
            r0 = r25
            java.lang.String r1 = "zoneId"
            r2 = r26
            org.threeten.bp.jdk8.Jdk8Methods.requireNonNull(r2, r1)
            r1 = r27
            r0.deduplicateMap = r1
            java.util.List<org.threeten.bp.zone.ZoneRulesBuilder$TZWindow> r3 = r0.windowList
            boolean r3 = r3.isEmpty()
            if (r3 != 0) goto L_0x01e7
            java.util.ArrayList r3 = new java.util.ArrayList
            r4 = 4
            r3.<init>(r4)
            java.util.ArrayList r4 = new java.util.ArrayList
            r5 = 256(0x100, float:3.59E-43)
            r4.<init>(r5)
            java.util.ArrayList r5 = new java.util.ArrayList
            r6 = 2
            r5.<init>(r6)
            r11 = r5
            java.util.List<org.threeten.bp.zone.ZoneRulesBuilder$TZWindow> r5 = r0.windowList
            r6 = 0
            java.lang.Object r5 = r5.get(r6)
            r12 = r5
            org.threeten.bp.zone.ZoneRulesBuilder$TZWindow r12 = (org.threeten.bp.zone.ZoneRulesBuilder.TZWindow) r12
            org.threeten.bp.ZoneOffset r5 = r12.standardOffset
            r7 = 0
            java.lang.Integer r8 = r12.fixedSavingAmountSecs
            if (r8 == 0) goto L_0x0047
            java.lang.Integer r8 = r12.fixedSavingAmountSecs
            int r7 = r8.intValue()
        L_0x0047:
            int r8 = r5.getTotalSeconds()
            int r8 = r8 + r7
            org.threeten.bp.ZoneOffset r8 = org.threeten.bp.ZoneOffset.ofTotalSeconds(r8)
            java.lang.Object r8 = r0.deduplicate(r8)
            r13 = r8
            org.threeten.bp.ZoneOffset r13 = (org.threeten.bp.ZoneOffset) r13
            r8 = -999999999(0xffffffffc4653601, float:-916.8438)
            r9 = 1
            org.threeten.bp.LocalDateTime r8 = org.threeten.bp.LocalDateTime.of((int) r8, (int) r9, (int) r9, (int) r6, (int) r6)
            java.lang.Object r8 = r0.deduplicate(r8)
            org.threeten.bp.LocalDateTime r8 = (org.threeten.bp.LocalDateTime) r8
            r10 = r13
            java.util.List<org.threeten.bp.zone.ZoneRulesBuilder$TZWindow> r14 = r0.windowList
            java.util.Iterator r14 = r14.iterator()
            r15 = r5
            r24 = r10
            r10 = r7
            r7 = r24
        L_0x0072:
            boolean r5 = r14.hasNext()
            if (r5 == 0) goto L_0x01d2
            java.lang.Object r5 = r14.next()
            org.threeten.bp.zone.ZoneRulesBuilder$TZWindow r5 = (org.threeten.bp.zone.ZoneRulesBuilder.TZWindow) r5
            int r9 = r8.getYear()
            r5.tidy(r9)
            java.lang.Integer r9 = r5.fixedSavingAmountSecs
            if (r9 != 0) goto L_0x00c0
            java.lang.Integer r9 = java.lang.Integer.valueOf(r6)
            java.util.List r17 = r5.ruleList
            java.util.Iterator r17 = r17.iterator()
        L_0x0097:
            boolean r18 = r17.hasNext()
            if (r18 == 0) goto L_0x00c0
            java.lang.Object r18 = r17.next()
            r6 = r18
            org.threeten.bp.zone.ZoneRulesBuilder$TZRule r6 = (org.threeten.bp.zone.ZoneRulesBuilder.TZRule) r6
            org.threeten.bp.zone.ZoneOffsetTransition r18 = r6.toTransition(r15, r10)
            long r19 = r18.toEpochSecond()
            long r21 = r8.toEpochSecond(r7)
            int r23 = (r19 > r21 ? 1 : (r19 == r21 ? 0 : -1))
            if (r23 <= 0) goto L_0x00b6
            goto L_0x00c0
        L_0x00b6:
            int r19 = r6.savingAmountSecs
            java.lang.Integer r9 = java.lang.Integer.valueOf(r19)
            r6 = 0
            goto L_0x0097
        L_0x00c0:
            org.threeten.bp.ZoneOffset r6 = r5.standardOffset
            boolean r6 = r15.equals(r6)
            if (r6 != 0) goto L_0x00f1
            org.threeten.bp.zone.ZoneOffsetTransition r6 = new org.threeten.bp.zone.ZoneOffsetTransition
            long r1 = r8.toEpochSecond(r7)
            r17 = r10
            r10 = 0
            org.threeten.bp.LocalDateTime r1 = org.threeten.bp.LocalDateTime.ofEpochSecond(r1, r10, r15)
            org.threeten.bp.ZoneOffset r2 = r5.standardOffset
            r6.<init>((org.threeten.bp.LocalDateTime) r1, (org.threeten.bp.ZoneOffset) r15, (org.threeten.bp.ZoneOffset) r2)
            java.lang.Object r1 = r0.deduplicate(r6)
            r3.add(r1)
            org.threeten.bp.ZoneOffset r1 = r5.standardOffset
            java.lang.Object r1 = r0.deduplicate(r1)
            org.threeten.bp.ZoneOffset r1 = (org.threeten.bp.ZoneOffset) r1
            r15 = r1
            goto L_0x00f3
        L_0x00f1:
            r17 = r10
        L_0x00f3:
            int r1 = r15.getTotalSeconds()
            int r2 = r9.intValue()
            int r1 = r1 + r2
            org.threeten.bp.ZoneOffset r1 = org.threeten.bp.ZoneOffset.ofTotalSeconds(r1)
            java.lang.Object r1 = r0.deduplicate(r1)
            org.threeten.bp.ZoneOffset r1 = (org.threeten.bp.ZoneOffset) r1
            boolean r2 = r7.equals(r1)
            if (r2 != 0) goto L_0x011a
            org.threeten.bp.zone.ZoneOffsetTransition r2 = new org.threeten.bp.zone.ZoneOffsetTransition
            r2.<init>((org.threeten.bp.LocalDateTime) r8, (org.threeten.bp.ZoneOffset) r7, (org.threeten.bp.ZoneOffset) r1)
            java.lang.Object r2 = r0.deduplicate(r2)
            org.threeten.bp.zone.ZoneOffsetTransition r2 = (org.threeten.bp.zone.ZoneOffsetTransition) r2
            r4.add(r2)
        L_0x011a:
            int r2 = r9.intValue()
            java.util.List r6 = r5.ruleList
            java.util.Iterator r6 = r6.iterator()
        L_0x0126:
            boolean r10 = r6.hasNext()
            if (r10 == 0) goto L_0x0182
            java.lang.Object r10 = r6.next()
            org.threeten.bp.zone.ZoneRulesBuilder$TZRule r10 = (org.threeten.bp.zone.ZoneRulesBuilder.TZRule) r10
            r18 = r1
            org.threeten.bp.zone.ZoneOffsetTransition r1 = r10.toTransition(r15, r2)
            java.lang.Object r1 = r0.deduplicate(r1)
            org.threeten.bp.zone.ZoneOffsetTransition r1 = (org.threeten.bp.zone.ZoneOffsetTransition) r1
            long r19 = r1.toEpochSecond()
            long r21 = r8.toEpochSecond(r7)
            int r17 = (r19 > r21 ? 1 : (r19 == r21 ? 0 : -1))
            if (r17 >= 0) goto L_0x014d
            r17 = 1
            goto L_0x014f
        L_0x014d:
            r17 = 0
        L_0x014f:
            if (r17 != 0) goto L_0x0177
            long r19 = r1.toEpochSecond()
            long r21 = r5.createDateTimeEpochSecond(r2)
            int r17 = (r19 > r21 ? 1 : (r19 == r21 ? 0 : -1))
            if (r17 >= 0) goto L_0x0177
            r17 = r2
            org.threeten.bp.ZoneOffset r2 = r1.getOffsetBefore()
            r19 = r6
            org.threeten.bp.ZoneOffset r6 = r1.getOffsetAfter()
            boolean r2 = r2.equals(r6)
            if (r2 != 0) goto L_0x017b
            r4.add(r1)
            int r2 = r10.savingAmountSecs
            goto L_0x017d
        L_0x0177:
            r17 = r2
            r19 = r6
        L_0x017b:
            r2 = r17
        L_0x017d:
            r1 = r18
            r6 = r19
            goto L_0x0126
        L_0x0182:
            r18 = r1
            r17 = r2
            r19 = r6
            java.util.List r1 = r5.lastRuleList
            java.util.Iterator r1 = r1.iterator()
            r10 = r17
        L_0x0192:
            boolean r2 = r1.hasNext()
            if (r2 == 0) goto L_0x01b0
            java.lang.Object r2 = r1.next()
            org.threeten.bp.zone.ZoneRulesBuilder$TZRule r2 = (org.threeten.bp.zone.ZoneRulesBuilder.TZRule) r2
            org.threeten.bp.zone.ZoneOffsetTransitionRule r6 = r2.toTransitionRule(r15, r10)
            java.lang.Object r6 = r0.deduplicate(r6)
            org.threeten.bp.zone.ZoneOffsetTransitionRule r6 = (org.threeten.bp.zone.ZoneOffsetTransitionRule) r6
            r11.add(r6)
            int r10 = r2.savingAmountSecs
            goto L_0x0192
        L_0x01b0:
            org.threeten.bp.ZoneOffset r1 = r5.createWallOffset(r10)
            java.lang.Object r1 = r0.deduplicate(r1)
            r7 = r1
            org.threeten.bp.ZoneOffset r7 = (org.threeten.bp.ZoneOffset) r7
            long r1 = r5.createDateTimeEpochSecond(r10)
            r6 = 0
            org.threeten.bp.LocalDateTime r1 = org.threeten.bp.LocalDateTime.ofEpochSecond(r1, r6, r7)
            java.lang.Object r1 = r0.deduplicate(r1)
            r8 = r1
            org.threeten.bp.LocalDateTime r8 = (org.threeten.bp.LocalDateTime) r8
            r2 = r26
            r1 = r27
            r9 = 1
            goto L_0x0072
        L_0x01d2:
            r17 = r10
            org.threeten.bp.zone.StandardZoneRules r1 = new org.threeten.bp.zone.StandardZoneRules
            org.threeten.bp.ZoneOffset r6 = r12.standardOffset
            r5 = r1
            r2 = r7
            r7 = r13
            r14 = r8
            r8 = r3
            r9 = r4
            r16 = r17
            r10 = r11
            r5.<init>((org.threeten.bp.ZoneOffset) r6, (org.threeten.bp.ZoneOffset) r7, (java.util.List<org.threeten.bp.zone.ZoneOffsetTransition>) r8, (java.util.List<org.threeten.bp.zone.ZoneOffsetTransition>) r9, (java.util.List<org.threeten.bp.zone.ZoneOffsetTransitionRule>) r10)
            return r1
        L_0x01e7:
            java.lang.IllegalStateException r1 = new java.lang.IllegalStateException
            java.lang.String r2 = "No windows have been added to the builder"
            r1.<init>(r2)
            throw r1
        */
        throw new UnsupportedOperationException("Method not decompiled: org.threeten.bp.zone.ZoneRulesBuilder.toRules(java.lang.String, java.util.Map):org.threeten.bp.zone.ZoneRules");
    }

    /* access modifiers changed from: package-private */
    public <T> T deduplicate(T object) {
        if (!this.deduplicateMap.containsKey(object)) {
            this.deduplicateMap.put(object, object);
        }
        return this.deduplicateMap.get(object);
    }

    class TZWindow {
        /* access modifiers changed from: private */
        public Integer fixedSavingAmountSecs;
        /* access modifiers changed from: private */
        public List<TZRule> lastRuleList = new ArrayList();
        private int maxLastRuleStartYear = Year.MIN_VALUE;
        /* access modifiers changed from: private */
        public List<TZRule> ruleList = new ArrayList();
        /* access modifiers changed from: private */
        public final ZoneOffset standardOffset;
        private final ZoneOffsetTransitionRule.TimeDefinition timeDefinition;
        private final LocalDateTime windowEnd;

        TZWindow(ZoneOffset standardOffset2, LocalDateTime windowEnd2, ZoneOffsetTransitionRule.TimeDefinition timeDefinition2) {
            this.windowEnd = windowEnd2;
            this.timeDefinition = timeDefinition2;
            this.standardOffset = standardOffset2;
        }

        /* access modifiers changed from: package-private */
        public void setFixedSavings(int fixedSavingAmount) {
            if (this.ruleList.size() > 0 || this.lastRuleList.size() > 0) {
                throw new IllegalStateException("Window has DST rules, so cannot have fixed savings");
            }
            this.fixedSavingAmountSecs = Integer.valueOf(fixedSavingAmount);
        }

        /* access modifiers changed from: package-private */
        public void addRule(int startYear, int endYear, Month month, int dayOfMonthIndicator, DayOfWeek dayOfWeek, LocalTime time, boolean timeEndOfDay, ZoneOffsetTransitionRule.TimeDefinition timeDefinition2, int savingAmountSecs) {
            int endYear2;
            if (this.fixedSavingAmountSecs != null) {
                int i = startYear;
                int i2 = endYear;
                throw new IllegalStateException("Window has a fixed DST saving, so cannot have DST rules");
            } else if (this.ruleList.size() < 2000) {
                boolean lastRule = false;
                int i3 = endYear;
                if (i3 == 999999999) {
                    lastRule = true;
                    endYear2 = startYear;
                } else {
                    endYear2 = i3;
                }
                for (int year = startYear; year <= endYear2; year++) {
                    TZRule rule = new TZRule(year, month, dayOfMonthIndicator, dayOfWeek, time, timeEndOfDay, timeDefinition2, savingAmountSecs);
                    if (lastRule) {
                        this.lastRuleList.add(rule);
                        this.maxLastRuleStartYear = Math.max(startYear, this.maxLastRuleStartYear);
                    } else {
                        int i4 = startYear;
                        this.ruleList.add(rule);
                    }
                }
                int i5 = startYear;
            } else {
                int i6 = startYear;
                int i7 = endYear;
                throw new IllegalStateException("Window has reached the maximum number of allowed rules");
            }
        }

        /* access modifiers changed from: package-private */
        public void validateWindowOrder(TZWindow previous) {
            if (this.windowEnd.isBefore(previous.windowEnd)) {
                throw new IllegalStateException("Windows must be added in date-time order: " + this.windowEnd + " < " + previous.windowEnd);
            }
        }

        /* access modifiers changed from: package-private */
        public void tidy(int windowStartYear) {
            if (this.lastRuleList.size() != 1) {
                if (this.windowEnd.equals(LocalDateTime.MAX)) {
                    this.maxLastRuleStartYear = Math.max(this.maxLastRuleStartYear, windowStartYear) + 1;
                    for (TZRule lastRule : this.lastRuleList) {
                        addRule(lastRule.year, this.maxLastRuleStartYear, lastRule.month, lastRule.dayOfMonthIndicator, lastRule.dayOfWeek, lastRule.time, lastRule.timeEndOfDay, lastRule.timeDefinition, lastRule.savingAmountSecs);
                        int unused = lastRule.year = this.maxLastRuleStartYear + 1;
                    }
                    int i = this.maxLastRuleStartYear;
                    if (i == 999999999) {
                        this.lastRuleList.clear();
                    } else {
                        this.maxLastRuleStartYear = i + 1;
                    }
                } else {
                    int endYear = this.windowEnd.getYear();
                    for (TZRule lastRule2 : this.lastRuleList) {
                        addRule(lastRule2.year, endYear + 1, lastRule2.month, lastRule2.dayOfMonthIndicator, lastRule2.dayOfWeek, lastRule2.time, lastRule2.timeEndOfDay, lastRule2.timeDefinition, lastRule2.savingAmountSecs);
                    }
                    this.lastRuleList.clear();
                    this.maxLastRuleStartYear = Year.MAX_VALUE;
                }
                Collections.sort(this.ruleList);
                Collections.sort(this.lastRuleList);
                if (this.ruleList.size() == 0 && this.fixedSavingAmountSecs == null) {
                    this.fixedSavingAmountSecs = 0;
                    return;
                }
                return;
            }
            throw new IllegalStateException("Cannot have only one rule defined as being forever");
        }

        /* access modifiers changed from: package-private */
        public boolean isSingleWindowStandardOffset() {
            return this.windowEnd.equals(LocalDateTime.MAX) && this.timeDefinition == ZoneOffsetTransitionRule.TimeDefinition.WALL && this.fixedSavingAmountSecs == null && this.lastRuleList.isEmpty() && this.ruleList.isEmpty();
        }

        /* access modifiers changed from: package-private */
        public ZoneOffset createWallOffset(int savingsSecs) {
            return ZoneOffset.ofTotalSeconds(this.standardOffset.getTotalSeconds() + savingsSecs);
        }

        /* access modifiers changed from: package-private */
        public long createDateTimeEpochSecond(int savingsSecs) {
            ZoneOffset wallOffset = createWallOffset(savingsSecs);
            return this.timeDefinition.createDateTime(this.windowEnd, this.standardOffset, wallOffset).toEpochSecond(wallOffset);
        }
    }

    class TZRule implements Comparable<TZRule> {
        /* access modifiers changed from: private */
        public int dayOfMonthIndicator;
        /* access modifiers changed from: private */
        public DayOfWeek dayOfWeek;
        /* access modifiers changed from: private */
        public Month month;
        /* access modifiers changed from: private */
        public int savingAmountSecs;
        /* access modifiers changed from: private */
        public LocalTime time;
        /* access modifiers changed from: private */
        public ZoneOffsetTransitionRule.TimeDefinition timeDefinition;
        /* access modifiers changed from: private */
        public boolean timeEndOfDay;
        /* access modifiers changed from: private */
        public int year;

        TZRule(int year2, Month month2, int dayOfMonthIndicator2, DayOfWeek dayOfWeek2, LocalTime time2, boolean timeEndOfDay2, ZoneOffsetTransitionRule.TimeDefinition timeDefinition2, int savingAfterSecs) {
            this.year = year2;
            this.month = month2;
            this.dayOfMonthIndicator = dayOfMonthIndicator2;
            this.dayOfWeek = dayOfWeek2;
            this.time = time2;
            this.timeEndOfDay = timeEndOfDay2;
            this.timeDefinition = timeDefinition2;
            this.savingAmountSecs = savingAfterSecs;
        }

        /* access modifiers changed from: package-private */
        public ZoneOffsetTransition toTransition(ZoneOffset standardOffset, int savingsBeforeSecs) {
            LocalDate date = toLocalDate();
            ZoneOffset wallOffset = (ZoneOffset) ZoneRulesBuilder.this.deduplicate(ZoneOffset.ofTotalSeconds(standardOffset.getTotalSeconds() + savingsBeforeSecs));
            return new ZoneOffsetTransition((LocalDateTime) ZoneRulesBuilder.this.deduplicate(this.timeDefinition.createDateTime((LocalDateTime) ZoneRulesBuilder.this.deduplicate(LocalDateTime.of((LocalDate) ZoneRulesBuilder.this.deduplicate(date), this.time)), standardOffset, wallOffset)), wallOffset, (ZoneOffset) ZoneRulesBuilder.this.deduplicate(ZoneOffset.ofTotalSeconds(standardOffset.getTotalSeconds() + this.savingAmountSecs)));
        }

        /* access modifiers changed from: package-private */
        public ZoneOffsetTransitionRule toTransitionRule(ZoneOffset standardOffset, int savingsBeforeSecs) {
            int i;
            if (this.dayOfMonthIndicator < 0 && this.month != Month.FEBRUARY) {
                this.dayOfMonthIndicator = this.month.maxLength() - 6;
            }
            if (this.timeEndOfDay && (i = this.dayOfMonthIndicator) > 0) {
                if (!(i == 28 && this.month == Month.FEBRUARY)) {
                    LocalDate date = LocalDate.of(2004, this.month, this.dayOfMonthIndicator).plusDays(1);
                    this.month = date.getMonth();
                    this.dayOfMonthIndicator = date.getDayOfMonth();
                    DayOfWeek dayOfWeek2 = this.dayOfWeek;
                    if (dayOfWeek2 != null) {
                        this.dayOfWeek = dayOfWeek2.plus(1);
                    }
                    this.timeEndOfDay = false;
                }
            }
            ZoneOffsetTransition trans = toTransition(standardOffset, savingsBeforeSecs);
            return new ZoneOffsetTransitionRule(this.month, this.dayOfMonthIndicator, this.dayOfWeek, this.time, this.timeEndOfDay, this.timeDefinition, standardOffset, trans.getOffsetBefore(), trans.getOffsetAfter());
        }

        public int compareTo(TZRule other) {
            int cmp = this.year - other.year;
            int cmp2 = cmp == 0 ? this.month.compareTo(other.month) : cmp;
            if (cmp2 == 0) {
                cmp2 = toLocalDate().compareTo((ChronoLocalDate) other.toLocalDate());
            }
            return cmp2 == 0 ? this.time.compareTo(other.time) : cmp2;
        }

        private LocalDate toLocalDate() {
            LocalDate date;
            int i = this.dayOfMonthIndicator;
            if (i < 0) {
                date = LocalDate.of(this.year, this.month, this.month.length(IsoChronology.INSTANCE.isLeapYear((long) this.year)) + 1 + this.dayOfMonthIndicator);
                DayOfWeek dayOfWeek2 = this.dayOfWeek;
                if (dayOfWeek2 != null) {
                    date = date.with(TemporalAdjusters.previousOrSame(dayOfWeek2));
                }
            } else {
                date = LocalDate.of(this.year, this.month, i);
                DayOfWeek dayOfWeek3 = this.dayOfWeek;
                if (dayOfWeek3 != null) {
                    date = date.with(TemporalAdjusters.nextOrSame(dayOfWeek3));
                }
            }
            if (this.timeEndOfDay) {
                return date.plusDays(1);
            }
            return date;
        }
    }
}
