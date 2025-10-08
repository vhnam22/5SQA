package org.threeten.bp.chrono;

import java.io.Serializable;
import java.util.Arrays;
import java.util.List;
import java.util.Map;
import org.threeten.bp.Clock;
import org.threeten.bp.DateTimeException;
import org.threeten.bp.DayOfWeek;
import org.threeten.bp.Instant;
import org.threeten.bp.LocalDate;
import org.threeten.bp.LocalDateTime;
import org.threeten.bp.Month;
import org.threeten.bp.Year;
import org.threeten.bp.ZoneId;
import org.threeten.bp.ZonedDateTime;
import org.threeten.bp.format.ResolverStyle;
import org.threeten.bp.jdk8.Jdk8Methods;
import org.threeten.bp.temporal.ChronoField;
import org.threeten.bp.temporal.TemporalAccessor;
import org.threeten.bp.temporal.TemporalAdjusters;
import org.threeten.bp.temporal.TemporalField;
import org.threeten.bp.temporal.ValueRange;

public final class IsoChronology extends Chronology implements Serializable {
    public static final IsoChronology INSTANCE = new IsoChronology();
    private static final long serialVersionUID = -1440403870442975015L;

    private IsoChronology() {
    }

    private Object readResolve() {
        return INSTANCE;
    }

    public String getId() {
        return "ISO";
    }

    public String getCalendarType() {
        return "iso8601";
    }

    public LocalDate date(Era era, int yearOfEra, int month, int dayOfMonth) {
        return date(prolepticYear(era, yearOfEra), month, dayOfMonth);
    }

    public LocalDate date(int prolepticYear, int month, int dayOfMonth) {
        return LocalDate.of(prolepticYear, month, dayOfMonth);
    }

    public LocalDate dateYearDay(Era era, int yearOfEra, int dayOfYear) {
        return dateYearDay(prolepticYear(era, yearOfEra), dayOfYear);
    }

    public LocalDate dateYearDay(int prolepticYear, int dayOfYear) {
        return LocalDate.ofYearDay(prolepticYear, dayOfYear);
    }

    public LocalDate dateEpochDay(long epochDay) {
        return LocalDate.ofEpochDay(epochDay);
    }

    public LocalDate date(TemporalAccessor temporal) {
        return LocalDate.from(temporal);
    }

    public LocalDateTime localDateTime(TemporalAccessor temporal) {
        return LocalDateTime.from(temporal);
    }

    public ZonedDateTime zonedDateTime(TemporalAccessor temporal) {
        return ZonedDateTime.from(temporal);
    }

    public ZonedDateTime zonedDateTime(Instant instant, ZoneId zone) {
        return ZonedDateTime.ofInstant(instant, zone);
    }

    public LocalDate dateNow() {
        return dateNow(Clock.systemDefaultZone());
    }

    public LocalDate dateNow(ZoneId zone) {
        return dateNow(Clock.system(zone));
    }

    public LocalDate dateNow(Clock clock) {
        Jdk8Methods.requireNonNull(clock, "clock");
        return date((TemporalAccessor) LocalDate.now(clock));
    }

    public boolean isLeapYear(long prolepticYear) {
        return (3 & prolepticYear) == 0 && (prolepticYear % 100 != 0 || prolepticYear % 400 == 0);
    }

    public int prolepticYear(Era era, int yearOfEra) {
        if (era instanceof IsoEra) {
            return era == IsoEra.CE ? yearOfEra : 1 - yearOfEra;
        }
        throw new ClassCastException("Era must be IsoEra");
    }

    public IsoEra eraOf(int eraValue) {
        return IsoEra.of(eraValue);
    }

    public List<Era> eras() {
        return Arrays.asList(IsoEra.values());
    }

    public ValueRange range(ChronoField field) {
        return field.range();
    }

    public LocalDate resolveDate(Map<TemporalField, Long> fieldValues, ResolverStyle resolverStyle) {
        if (fieldValues.containsKey(ChronoField.EPOCH_DAY)) {
            return LocalDate.ofEpochDay(fieldValues.remove(ChronoField.EPOCH_DAY).longValue());
        }
        Long prolepticMonth = fieldValues.remove(ChronoField.PROLEPTIC_MONTH);
        if (prolepticMonth != null) {
            if (resolverStyle != ResolverStyle.LENIENT) {
                ChronoField.PROLEPTIC_MONTH.checkValidValue(prolepticMonth.longValue());
            }
            updateResolveMap(fieldValues, ChronoField.MONTH_OF_YEAR, (long) (Jdk8Methods.floorMod(prolepticMonth.longValue(), 12) + 1));
            updateResolveMap(fieldValues, ChronoField.YEAR, Jdk8Methods.floorDiv(prolepticMonth.longValue(), 12));
        }
        Long yoeLong = fieldValues.remove(ChronoField.YEAR_OF_ERA);
        if (yoeLong != null) {
            if (resolverStyle != ResolverStyle.LENIENT) {
                ChronoField.YEAR_OF_ERA.checkValidValue(yoeLong.longValue());
            }
            Long era = fieldValues.remove(ChronoField.ERA);
            if (era == null) {
                Long year = fieldValues.get(ChronoField.YEAR);
                if (resolverStyle != ResolverStyle.STRICT) {
                    updateResolveMap(fieldValues, ChronoField.YEAR, (year == null || year.longValue() > 0) ? yoeLong.longValue() : Jdk8Methods.safeSubtract(1, yoeLong.longValue()));
                } else if (year != null) {
                    ChronoField chronoField = ChronoField.YEAR;
                    int i = (year.longValue() > 0 ? 1 : (year.longValue() == 0 ? 0 : -1));
                    long longValue = yoeLong.longValue();
                    if (i <= 0) {
                        longValue = Jdk8Methods.safeSubtract(1, longValue);
                    }
                    updateResolveMap(fieldValues, chronoField, longValue);
                } else {
                    fieldValues.put(ChronoField.YEAR_OF_ERA, yoeLong);
                }
            } else if (era.longValue() == 1) {
                updateResolveMap(fieldValues, ChronoField.YEAR, yoeLong.longValue());
            } else if (era.longValue() == 0) {
                updateResolveMap(fieldValues, ChronoField.YEAR, Jdk8Methods.safeSubtract(1, yoeLong.longValue()));
            } else {
                throw new DateTimeException("Invalid value for era: " + era);
            }
        } else if (fieldValues.containsKey(ChronoField.ERA)) {
            ChronoField.ERA.checkValidValue(fieldValues.get(ChronoField.ERA).longValue());
        }
        if (!fieldValues.containsKey(ChronoField.YEAR)) {
            return null;
        }
        if (fieldValues.containsKey(ChronoField.MONTH_OF_YEAR)) {
            if (fieldValues.containsKey(ChronoField.DAY_OF_MONTH)) {
                int y = ChronoField.YEAR.checkValidIntValue(fieldValues.remove(ChronoField.YEAR).longValue());
                int moy = Jdk8Methods.safeToInt(fieldValues.remove(ChronoField.MONTH_OF_YEAR).longValue());
                int dom = Jdk8Methods.safeToInt(fieldValues.remove(ChronoField.DAY_OF_MONTH).longValue());
                if (resolverStyle == ResolverStyle.LENIENT) {
                    return LocalDate.of(y, 1, 1).plusMonths((long) Jdk8Methods.safeSubtract(moy, 1)).plusDays((long) Jdk8Methods.safeSubtract(dom, 1));
                } else if (resolverStyle != ResolverStyle.SMART) {
                    return LocalDate.of(y, moy, dom);
                } else {
                    ChronoField.DAY_OF_MONTH.checkValidValue((long) dom);
                    if (moy == 4 || moy == 6 || moy == 9 || moy == 11) {
                        dom = Math.min(dom, 30);
                    } else if (moy == 2) {
                        dom = Math.min(dom, Month.FEBRUARY.length(Year.isLeap((long) y)));
                    }
                    return LocalDate.of(y, moy, dom);
                }
            } else if (fieldValues.containsKey(ChronoField.ALIGNED_WEEK_OF_MONTH)) {
                if (fieldValues.containsKey(ChronoField.ALIGNED_DAY_OF_WEEK_IN_MONTH)) {
                    int y2 = ChronoField.YEAR.checkValidIntValue(fieldValues.remove(ChronoField.YEAR).longValue());
                    if (resolverStyle == ResolverStyle.LENIENT) {
                        long months = Jdk8Methods.safeSubtract(fieldValues.remove(ChronoField.MONTH_OF_YEAR).longValue(), 1);
                        return LocalDate.of(y2, 1, 1).plusMonths(months).plusWeeks(Jdk8Methods.safeSubtract(fieldValues.remove(ChronoField.ALIGNED_WEEK_OF_MONTH).longValue(), 1)).plusDays(Jdk8Methods.safeSubtract(fieldValues.remove(ChronoField.ALIGNED_DAY_OF_WEEK_IN_MONTH).longValue(), 1));
                    }
                    int moy2 = ChronoField.MONTH_OF_YEAR.checkValidIntValue(fieldValues.remove(ChronoField.MONTH_OF_YEAR).longValue());
                    LocalDate date = LocalDate.of(y2, moy2, 1).plusDays((long) (((ChronoField.ALIGNED_WEEK_OF_MONTH.checkValidIntValue(fieldValues.remove(ChronoField.ALIGNED_WEEK_OF_MONTH).longValue()) - 1) * 7) + (ChronoField.ALIGNED_DAY_OF_WEEK_IN_MONTH.checkValidIntValue(fieldValues.remove(ChronoField.ALIGNED_DAY_OF_WEEK_IN_MONTH).longValue()) - 1)));
                    if (resolverStyle != ResolverStyle.STRICT || date.get(ChronoField.MONTH_OF_YEAR) == moy2) {
                        return date;
                    }
                    throw new DateTimeException("Strict mode rejected date parsed to a different month");
                } else if (fieldValues.containsKey(ChronoField.DAY_OF_WEEK)) {
                    int y3 = ChronoField.YEAR.checkValidIntValue(fieldValues.remove(ChronoField.YEAR).longValue());
                    if (resolverStyle == ResolverStyle.LENIENT) {
                        long months2 = Jdk8Methods.safeSubtract(fieldValues.remove(ChronoField.MONTH_OF_YEAR).longValue(), 1);
                        return LocalDate.of(y3, 1, 1).plusMonths(months2).plusWeeks(Jdk8Methods.safeSubtract(fieldValues.remove(ChronoField.ALIGNED_WEEK_OF_MONTH).longValue(), 1)).plusDays(Jdk8Methods.safeSubtract(fieldValues.remove(ChronoField.DAY_OF_WEEK).longValue(), 1));
                    }
                    int moy3 = ChronoField.MONTH_OF_YEAR.checkValidIntValue(fieldValues.remove(ChronoField.MONTH_OF_YEAR).longValue());
                    LocalDate date2 = LocalDate.of(y3, moy3, 1).plusWeeks((long) (ChronoField.ALIGNED_WEEK_OF_MONTH.checkValidIntValue(fieldValues.remove(ChronoField.ALIGNED_WEEK_OF_MONTH).longValue()) - 1)).with(TemporalAdjusters.nextOrSame(DayOfWeek.of(ChronoField.DAY_OF_WEEK.checkValidIntValue(fieldValues.remove(ChronoField.DAY_OF_WEEK).longValue()))));
                    if (resolverStyle != ResolverStyle.STRICT || date2.get(ChronoField.MONTH_OF_YEAR) == moy3) {
                        return date2;
                    }
                    throw new DateTimeException("Strict mode rejected date parsed to a different month");
                }
            }
        }
        if (fieldValues.containsKey(ChronoField.DAY_OF_YEAR)) {
            int y4 = ChronoField.YEAR.checkValidIntValue(fieldValues.remove(ChronoField.YEAR).longValue());
            if (resolverStyle != ResolverStyle.LENIENT) {
                return LocalDate.ofYearDay(y4, ChronoField.DAY_OF_YEAR.checkValidIntValue(fieldValues.remove(ChronoField.DAY_OF_YEAR).longValue()));
            }
            return LocalDate.ofYearDay(y4, 1).plusDays(Jdk8Methods.safeSubtract(fieldValues.remove(ChronoField.DAY_OF_YEAR).longValue(), 1));
        } else if (!fieldValues.containsKey(ChronoField.ALIGNED_WEEK_OF_YEAR)) {
            return null;
        } else {
            if (fieldValues.containsKey(ChronoField.ALIGNED_DAY_OF_WEEK_IN_YEAR)) {
                int y5 = ChronoField.YEAR.checkValidIntValue(fieldValues.remove(ChronoField.YEAR).longValue());
                if (resolverStyle == ResolverStyle.LENIENT) {
                    return LocalDate.of(y5, 1, 1).plusWeeks(Jdk8Methods.safeSubtract(fieldValues.remove(ChronoField.ALIGNED_WEEK_OF_YEAR).longValue(), 1)).plusDays(Jdk8Methods.safeSubtract(fieldValues.remove(ChronoField.ALIGNED_DAY_OF_WEEK_IN_YEAR).longValue(), 1));
                }
                LocalDate date3 = LocalDate.of(y5, 1, 1).plusDays((long) (((ChronoField.ALIGNED_WEEK_OF_YEAR.checkValidIntValue(fieldValues.remove(ChronoField.ALIGNED_WEEK_OF_YEAR).longValue()) - 1) * 7) + (ChronoField.ALIGNED_DAY_OF_WEEK_IN_YEAR.checkValidIntValue(fieldValues.remove(ChronoField.ALIGNED_DAY_OF_WEEK_IN_YEAR).longValue()) - 1)));
                if (resolverStyle != ResolverStyle.STRICT || date3.get(ChronoField.YEAR) == y5) {
                    return date3;
                }
                throw new DateTimeException("Strict mode rejected date parsed to a different year");
            } else if (!fieldValues.containsKey(ChronoField.DAY_OF_WEEK)) {
                return null;
            } else {
                int y6 = ChronoField.YEAR.checkValidIntValue(fieldValues.remove(ChronoField.YEAR).longValue());
                if (resolverStyle == ResolverStyle.LENIENT) {
                    return LocalDate.of(y6, 1, 1).plusWeeks(Jdk8Methods.safeSubtract(fieldValues.remove(ChronoField.ALIGNED_WEEK_OF_YEAR).longValue(), 1)).plusDays(Jdk8Methods.safeSubtract(fieldValues.remove(ChronoField.DAY_OF_WEEK).longValue(), 1));
                }
                LocalDate date4 = LocalDate.of(y6, 1, 1).plusWeeks((long) (ChronoField.ALIGNED_WEEK_OF_YEAR.checkValidIntValue(fieldValues.remove(ChronoField.ALIGNED_WEEK_OF_YEAR).longValue()) - 1)).with(TemporalAdjusters.nextOrSame(DayOfWeek.of(ChronoField.DAY_OF_WEEK.checkValidIntValue(fieldValues.remove(ChronoField.DAY_OF_WEEK).longValue()))));
                if (resolverStyle != ResolverStyle.STRICT || date4.get(ChronoField.YEAR) == y6) {
                    return date4;
                }
                throw new DateTimeException("Strict mode rejected date parsed to a different month");
            }
        }
    }
}
