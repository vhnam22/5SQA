package org.threeten.bp.zone;

import java.io.BufferedReader;
import java.io.ByteArrayOutputStream;
import java.io.DataOutputStream;
import java.io.File;
import java.io.FileOutputStream;
import java.io.FileReader;
import java.io.IOException;
import java.io.OutputStream;
import java.text.ParsePosition;
import java.util.ArrayList;
import java.util.Arrays;
import java.util.HashMap;
import java.util.HashSet;
import java.util.Iterator;
import java.util.List;
import java.util.Map;
import java.util.Set;
import java.util.SortedMap;
import java.util.StringTokenizer;
import java.util.TreeMap;
import java.util.TreeSet;
import java.util.jar.JarOutputStream;
import java.util.zip.ZipEntry;
import org.threeten.bp.DayOfWeek;
import org.threeten.bp.LocalDate;
import org.threeten.bp.LocalDateTime;
import org.threeten.bp.LocalTime;
import org.threeten.bp.Month;
import org.threeten.bp.Year;
import org.threeten.bp.ZoneOffset;
import org.threeten.bp.chrono.ChronoLocalDate;
import org.threeten.bp.format.DateTimeFormatter;
import org.threeten.bp.format.DateTimeFormatterBuilder;
import org.threeten.bp.temporal.ChronoField;
import org.threeten.bp.temporal.TemporalAccessor;
import org.threeten.bp.temporal.TemporalAdjusters;
import org.threeten.bp.temporal.TemporalField;
import org.threeten.bp.zone.ZoneOffsetTransitionRule;

final class TzdbZoneRulesCompiler {
    private static final DateTimeFormatter TIME_PARSER = new DateTimeFormatterBuilder().appendValue((TemporalField) ChronoField.HOUR_OF_DAY).optionalStart().appendLiteral(':').appendValue(ChronoField.MINUTE_OF_HOUR, 2).optionalStart().appendLiteral(':').appendValue(ChronoField.SECOND_OF_MINUTE, 2).toFormatter();
    private final SortedMap<String, ZoneRules> builtZones = new TreeMap();
    private Map<Object, Object> deduplicateMap = new HashMap();
    private final SortedMap<LocalDate, Byte> leapSeconds = new TreeMap();
    private final File leapSecondsFile;
    private final Map<String, String> links = new HashMap();
    private final Map<String, List<TZDBRule>> rules = new HashMap();
    private final List<File> sourceFiles;
    private final boolean verbose;
    private final String version;
    private final Map<String, List<TZDBZone>> zones = new HashMap();

    public static void main(String[] args) {
        String[] strArr = args;
        if (strArr.length < 2) {
            outputHelp();
            return;
        }
        String version2 = null;
        File baseSrcDir = null;
        File dstDir = null;
        boolean unpacked = false;
        boolean verbose2 = false;
        int i = 0;
        while (i < strArr.length) {
            String arg = strArr[i];
            if (!arg.startsWith("-")) {
                break;
            }
            if ("-srcdir".equals(arg)) {
                if (baseSrcDir == null && (i = i + 1) < strArr.length) {
                    baseSrcDir = new File(strArr[i]);
                }
                outputHelp();
                return;
            } else if ("-dstdir".equals(arg)) {
                if (dstDir == null && (i = i + 1) < strArr.length) {
                    dstDir = new File(strArr[i]);
                }
                outputHelp();
                return;
            } else if ("-version".equals(arg)) {
                if (version2 == null && (i = i + 1) < strArr.length) {
                    version2 = strArr[i];
                }
                outputHelp();
                return;
            } else if (!"-unpacked".equals(arg)) {
                if ("-verbose".equals(arg)) {
                    if (!verbose2) {
                        verbose2 = true;
                    }
                } else if (!"-help".equals(arg)) {
                    System.out.println("Unrecognised option: " + arg);
                }
                outputHelp();
                return;
            } else if (!unpacked) {
                unpacked = true;
            } else {
                outputHelp();
                return;
            }
            i++;
        }
        if (baseSrcDir == null) {
            System.out.println("Source directory must be specified using -srcdir: " + baseSrcDir);
        } else if (!baseSrcDir.isDirectory()) {
            System.out.println("Source does not exist or is not a directory: " + baseSrcDir);
        } else {
            File dstDir2 = dstDir != null ? dstDir : baseSrcDir;
            List<String> srcFileNames = Arrays.asList(Arrays.copyOfRange(strArr, i, strArr.length));
            if (srcFileNames.isEmpty()) {
                System.out.println("Source filenames not specified, using default set");
                System.out.println("(africa antarctica asia australasia backward etcetera europe northamerica southamerica)");
                srcFileNames = Arrays.asList(new String[]{"africa", "antarctica", "asia", "australasia", "backward", "etcetera", "europe", "northamerica", "southamerica"});
            }
            List<File> srcDirs = new ArrayList<>();
            if (version2 != null) {
                File srcDir = new File(baseSrcDir, version2);
                if (!srcDir.isDirectory()) {
                    System.out.println("Version does not represent a valid source directory : " + srcDir);
                    return;
                }
                srcDirs.add(srcDir);
            } else {
                for (File dir : baseSrcDir.listFiles()) {
                    if (dir.isDirectory() && dir.getName().matches("[12][0-9][0-9][0-9][A-Za-z0-9._-]+")) {
                        srcDirs.add(dir);
                    }
                }
            }
            if (srcDirs.isEmpty()) {
                System.out.println("Source directory contains no valid source folders: " + baseSrcDir);
            } else if (!dstDir2.exists() && !dstDir2.mkdirs()) {
                System.out.println("Destination directory could not be created: " + dstDir2);
            } else if (!dstDir2.isDirectory()) {
                System.out.println("Destination is not a directory: " + dstDir2);
            } else {
                process(srcDirs, srcFileNames, dstDir2, unpacked, verbose2);
            }
        }
    }

    private static void outputHelp() {
        System.out.println("Usage: TzdbZoneRulesCompiler <options> <tzdb source filenames>");
        System.out.println("where options include:");
        System.out.println("   -srcdir <directory>   Where to find source directories (required)");
        System.out.println("   -dstdir <directory>   Where to output generated files (default srcdir)");
        System.out.println("   -version <version>    Specify the version, such as 2009a (optional)");
        System.out.println("   -unpacked             Generate dat files without jar files");
        System.out.println("   -help                 Print this usage message");
        System.out.println("   -verbose              Output verbose information during compilation");
        System.out.println(" There must be one directory for each version in srcdir");
        System.out.println(" Each directory must have the name of the version, such as 2009a");
        System.out.println(" Each directory must contain the unpacked tzdb files, such as asia or europe");
        System.out.println(" Directories must match the regex [12][0-9][0-9][0-9][A-Za-z0-9._-]+");
        System.out.println(" There will be one jar file for each version and one combined jar in dstdir");
        System.out.println(" If the version is specified, only that version is processed");
    }

    private static void process(List<File> srcDirs, List<String> srcFileNames, File dstDir, boolean unpacked, boolean verbose2) {
        Iterator i$;
        File leapSecondsFile2;
        Map<Object, Object> deduplicateMap2;
        File file = dstDir;
        boolean z = verbose2;
        Map<Object, Object> deduplicateMap3 = new HashMap<>();
        Map<String, SortedMap<String, ZoneRules>> allBuiltZones = new TreeMap<>();
        Set<String> allRegionIds = new TreeSet<>();
        Set<ZoneRules> allRules = new HashSet<>();
        Iterator i$2 = srcDirs.iterator();
        SortedMap<LocalDate, Byte> bestLeapSeconds = null;
        while (i$2.hasNext()) {
            File srcDir = i$2.next();
            List<File> srcFiles = new ArrayList<>();
            for (String srcFileName : srcFileNames) {
                File file2 = new File(srcDir, srcFileName);
                if (file2.exists()) {
                    srcFiles.add(file2);
                }
            }
            if (!srcFiles.isEmpty()) {
                File leapSecondsFile3 = new File(srcDir, "leapseconds");
                if (!leapSecondsFile3.exists()) {
                    System.out.println("Version " + srcDir.getName() + " does not include leap seconds information.");
                    leapSecondsFile2 = null;
                } else {
                    leapSecondsFile2 = leapSecondsFile3;
                }
                String loopVersion = srcDir.getName();
                TzdbZoneRulesCompiler compiler = new TzdbZoneRulesCompiler(loopVersion, srcFiles, leapSecondsFile2, z);
                compiler.setDeduplicateMap(deduplicateMap3);
                try {
                    compiler.compile();
                    SortedMap<String, ZoneRules> builtZones2 = compiler.getZones();
                    SortedMap<LocalDate, Byte> parsedLeapSeconds = compiler.getLeapSeconds();
                    if (!unpacked) {
                        deduplicateMap2 = deduplicateMap3;
                        try {
                            i$ = i$2;
                            try {
                                File dstFile = new File(file, "threeten-TZDB-" + loopVersion + ".jar");
                                if (z) {
                                    File file3 = srcDir;
                                    try {
                                        System.out.println("Outputting file: " + dstFile);
                                    } catch (Exception e) {
                                        ex = e;
                                        System.out.println("Failed: " + ex.toString());
                                        ex.printStackTrace();
                                        System.exit(1);
                                        deduplicateMap3 = deduplicateMap2;
                                        i$2 = i$;
                                    }
                                }
                                outputFile(dstFile, loopVersion, builtZones2, parsedLeapSeconds);
                            } catch (Exception e2) {
                                ex = e2;
                                File file4 = srcDir;
                                System.out.println("Failed: " + ex.toString());
                                ex.printStackTrace();
                                System.exit(1);
                                deduplicateMap3 = deduplicateMap2;
                                i$2 = i$;
                            }
                        } catch (Exception e3) {
                            ex = e3;
                            i$ = i$2;
                            File file5 = srcDir;
                            System.out.println("Failed: " + ex.toString());
                            ex.printStackTrace();
                            System.exit(1);
                            deduplicateMap3 = deduplicateMap2;
                            i$2 = i$;
                        }
                    } else {
                        deduplicateMap2 = deduplicateMap3;
                        i$ = i$2;
                        File file6 = srcDir;
                    }
                    allBuiltZones.put(loopVersion, builtZones2);
                    allRegionIds.addAll(builtZones2.keySet());
                    allRules.addAll(builtZones2.values());
                    if (compiler.getMostRecentLeapSecond() != null && (bestLeapSeconds == null || compiler.getMostRecentLeapSecond().compareTo((ChronoLocalDate) bestLeapSeconds.lastKey()) > 0)) {
                        bestLeapSeconds = parsedLeapSeconds;
                    }
                } catch (Exception e4) {
                    ex = e4;
                    deduplicateMap2 = deduplicateMap3;
                    i$ = i$2;
                    File file7 = srcDir;
                    System.out.println("Failed: " + ex.toString());
                    ex.printStackTrace();
                    System.exit(1);
                    deduplicateMap3 = deduplicateMap2;
                    i$2 = i$;
                }
                deduplicateMap3 = deduplicateMap2;
                i$2 = i$;
            }
        }
        Iterator it = i$2;
        if (unpacked) {
            if (z) {
                System.out.println("Outputting combined files: " + file);
            }
            outputFilesDat(file, allBuiltZones, allRegionIds, allRules, bestLeapSeconds);
            return;
        }
        File dstFile2 = new File(file, "threeten-TZDB-all.jar");
        if (z) {
            System.out.println("Outputting combined file: " + dstFile2);
        }
        outputFile(dstFile2, allBuiltZones, allRegionIds, allRules, bestLeapSeconds);
    }

    private static void outputFilesDat(File dstDir, Map<String, SortedMap<String, ZoneRules>> allBuiltZones, Set<String> allRegionIds, Set<ZoneRules> allRules, SortedMap<LocalDate, Byte> sortedMap) {
        File tzdbFile = new File(dstDir, "TZDB.dat");
        tzdbFile.delete();
        FileOutputStream fos = null;
        try {
            fos = new FileOutputStream(tzdbFile);
            outputTzdbDat(fos, allBuiltZones, allRegionIds, allRules);
            fos.close();
        } catch (Exception ex) {
            System.out.println("Failed: " + ex.toString());
            ex.printStackTrace();
            System.exit(1);
        } catch (Throwable th) {
            if (fos != null) {
                fos.close();
            }
            throw th;
        }
    }

    private static void outputFile(File dstFile, String version2, SortedMap<String, ZoneRules> builtZones2, SortedMap<LocalDate, Byte> leapSeconds2) {
        Map<String, SortedMap<String, ZoneRules>> loopAllBuiltZones = new TreeMap<>();
        loopAllBuiltZones.put(version2, builtZones2);
        outputFile(dstFile, loopAllBuiltZones, new TreeSet<>(builtZones2.keySet()), new HashSet<>(builtZones2.values()), leapSeconds2);
    }

    private static void outputFile(File dstFile, Map<String, SortedMap<String, ZoneRules>> allBuiltZones, Set<String> allRegionIds, Set<ZoneRules> allRules, SortedMap<LocalDate, Byte> sortedMap) {
        JarOutputStream jos = null;
        try {
            jos = new JarOutputStream(new FileOutputStream(dstFile));
            outputTzdbEntry(jos, allBuiltZones, allRegionIds, allRules);
            try {
                jos.close();
            } catch (IOException e) {
            }
        } catch (Exception ex) {
            System.out.println("Failed: " + ex.toString());
            ex.printStackTrace();
            System.exit(1);
            if (jos != null) {
                jos.close();
            }
        } catch (Throwable th) {
            if (jos != null) {
                try {
                    jos.close();
                } catch (IOException e2) {
                }
            }
            throw th;
        }
    }

    private static void outputTzdbEntry(JarOutputStream jos, Map<String, SortedMap<String, ZoneRules>> allBuiltZones, Set<String> allRegionIds, Set<ZoneRules> allRules) {
        try {
            jos.putNextEntry(new ZipEntry("org/threeten/bp/TZDB.dat"));
            outputTzdbDat(jos, allBuiltZones, allRegionIds, allRules);
            jos.closeEntry();
        } catch (Exception ex) {
            System.out.println("Failed: " + ex.toString());
            ex.printStackTrace();
            System.exit(1);
        }
    }

    private static void outputTzdbDat(OutputStream jos, Map<String, SortedMap<String, ZoneRules>> allBuiltZones, Set<String> allRegionIds, Set<ZoneRules> allRules) throws IOException {
        DataOutputStream out = new DataOutputStream(jos);
        out.writeByte(1);
        out.writeUTF("TZDB");
        String[] versionArray = (String[]) allBuiltZones.keySet().toArray(new String[allBuiltZones.size()]);
        out.writeShort(versionArray.length);
        for (String version2 : versionArray) {
            out.writeUTF(version2);
        }
        String[] regionArray = (String[]) allRegionIds.toArray(new String[allRegionIds.size()]);
        out.writeShort(regionArray.length);
        for (String regionId : regionArray) {
            out.writeUTF(regionId);
        }
        List<ZoneRules> rulesList = new ArrayList<>(allRules);
        out.writeShort(rulesList.size());
        ByteArrayOutputStream baos = new ByteArrayOutputStream(1024);
        for (ZoneRules rules2 : rulesList) {
            baos.reset();
            DataOutputStream dataos = new DataOutputStream(baos);
            Ser.write(rules2, dataos);
            dataos.close();
            byte[] bytes = baos.toByteArray();
            out.writeShort(bytes.length);
            out.write(bytes);
        }
        for (String version3 : allBuiltZones.keySet()) {
            out.writeShort(allBuiltZones.get(version3).size());
            for (Map.Entry<String, ZoneRules> entry : allBuiltZones.get(version3).entrySet()) {
                int regionIndex = Arrays.binarySearch(regionArray, entry.getKey());
                int rulesIndex = rulesList.indexOf(entry.getValue());
                out.writeShort(regionIndex);
                out.writeShort(rulesIndex);
            }
        }
        out.flush();
    }

    public TzdbZoneRulesCompiler(String version2, List<File> sourceFiles2, File leapSecondsFile2, boolean verbose2) {
        this.version = version2;
        this.sourceFiles = sourceFiles2;
        this.leapSecondsFile = leapSecondsFile2;
        this.verbose = verbose2;
    }

    public void compile() throws Exception {
        printVerbose("Compiling TZDB version " + this.version);
        parseFiles();
        parseLeapSecondsFile();
        buildZoneRules();
        printVerbose("Compiled TZDB version " + this.version);
    }

    public SortedMap<String, ZoneRules> getZones() {
        return this.builtZones;
    }

    public SortedMap<LocalDate, Byte> getLeapSeconds() {
        return this.leapSeconds;
    }

    private LocalDate getMostRecentLeapSecond() {
        if (this.leapSeconds.isEmpty()) {
            return null;
        }
        return this.leapSeconds.lastKey();
    }

    /* access modifiers changed from: package-private */
    public void setDeduplicateMap(Map<Object, Object> deduplicateMap2) {
        this.deduplicateMap = deduplicateMap2;
    }

    private void parseFiles() throws Exception {
        for (File file : this.sourceFiles) {
            printVerbose("Parsing file: " + file);
            parseFile(file);
        }
    }

    private void parseLeapSecondsFile() throws Exception {
        printVerbose("Parsing leap second file: " + this.leapSecondsFile);
        int lineNumber = 1;
        BufferedReader in = null;
        try {
            BufferedReader in2 = new BufferedReader(new FileReader(this.leapSecondsFile));
            while (true) {
                String readLine = in2.readLine();
                String line = readLine;
                if (readLine != null) {
                    int index = line.indexOf(35);
                    if (index >= 0) {
                        line = line.substring(0, index);
                    }
                    if (line.trim().length() != 0) {
                        LeapSecondRule secondRule = parseLeapSecondRule(line);
                        this.leapSeconds.put(secondRule.leapDate, Byte.valueOf(secondRule.secondAdjustment));
                    }
                    lineNumber++;
                } else {
                    try {
                        in2.close();
                        return;
                    } catch (Exception e) {
                        return;
                    }
                }
            }
        } catch (Exception ex) {
            throw new Exception("Failed while processing file '" + this.leapSecondsFile + "' on line " + 1 + " '" + null + "'", ex);
        } catch (Throwable th) {
            if (in != null) {
                try {
                    in.close();
                } catch (Exception e2) {
                }
            }
            throw th;
        }
    }

    private LeapSecondRule parseLeapSecondRule(String line) {
        byte adjustmentByte;
        StringTokenizer st = new StringTokenizer(line, " \t");
        if (!st.nextToken().equals("Leap")) {
            throw new IllegalArgumentException("Unknown line");
        } else if (st.countTokens() >= 6) {
            LocalDate leapDate = LocalDate.of(Integer.parseInt(st.nextToken()), parseMonth(st.nextToken()), Integer.parseInt(st.nextToken()));
            String timeOfLeapSecond = st.nextToken();
            String adjustment = st.nextToken();
            if (adjustment.equals("+")) {
                if ("23:59:60".equals(timeOfLeapSecond)) {
                    adjustmentByte = 1;
                } else {
                    throw new IllegalArgumentException("Leap seconds can only be inserted at 23:59:60 - Date:" + leapDate);
                }
            } else if (!adjustment.equals("-")) {
                throw new IllegalArgumentException("Invalid adjustment '" + adjustment + "' in leap second rule for " + leapDate);
            } else if ("23:59:59".equals(timeOfLeapSecond)) {
                adjustmentByte = -1;
            } else {
                throw new IllegalArgumentException("Leap seconds can only be removed at 23:59:59 - Date:" + leapDate);
            }
            String rollingOrStationary = st.nextToken();
            if ("S".equalsIgnoreCase(rollingOrStationary)) {
                return new LeapSecondRule(leapDate, adjustmentByte);
            }
            throw new IllegalArgumentException("Only stationary ('S') leap seconds are supported, not '" + rollingOrStationary + "'");
        } else {
            printVerbose("Invalid leap second line in file: " + this.leapSecondsFile + ", line: " + line);
            throw new IllegalArgumentException("Invalid leap second line");
        }
    }

    private void parseFile(File file) throws Exception {
        int lineNumber = 1;
        String line = null;
        BufferedReader in = null;
        try {
            in = new BufferedReader(new FileReader(file));
            List<TZDBZone> openZone = null;
            while (true) {
                String readLine = in.readLine();
                line = readLine;
                if (readLine != null) {
                    int index = line.indexOf(35);
                    if (index >= 0) {
                        line = line.substring(0, index);
                    }
                    if (line.trim().length() != 0) {
                        StringTokenizer st = new StringTokenizer(line, " \t");
                        if (openZone == null || !Character.isWhitespace(line.charAt(0)) || !st.hasMoreTokens()) {
                            if (st.hasMoreTokens()) {
                                String first = st.nextToken();
                                if (!first.equals("Zone")) {
                                    openZone = null;
                                    if (first.equals("Rule")) {
                                        if (st.countTokens() >= 9) {
                                            parseRuleLine(st);
                                        } else {
                                            printVerbose("Invalid Rule line in file: " + file + ", line: " + line);
                                            throw new IllegalArgumentException("Invalid Rule line");
                                        }
                                    } else if (!first.equals("Link")) {
                                        throw new IllegalArgumentException("Unknown line");
                                    } else if (st.countTokens() >= 2) {
                                        String realId = st.nextToken();
                                        this.links.put(st.nextToken(), realId);
                                    } else {
                                        printVerbose("Invalid Link line in file: " + file + ", line: " + line);
                                        throw new IllegalArgumentException("Invalid Link line");
                                    }
                                } else if (st.countTokens() >= 3) {
                                    openZone = new ArrayList<>();
                                    this.zones.put(st.nextToken(), openZone);
                                    if (parseZoneLine(st, openZone)) {
                                        openZone = null;
                                    }
                                } else {
                                    printVerbose("Invalid Zone line in file: " + file + ", line: " + line);
                                    throw new IllegalArgumentException("Invalid Zone line");
                                }
                            } else {
                                continue;
                            }
                        } else if (parseZoneLine(st, openZone)) {
                            openZone = null;
                        }
                    }
                    lineNumber++;
                } else {
                    in.close();
                    return;
                }
            }
        } catch (Exception ex) {
            throw new Exception("Failed while processing file '" + file + "' on line " + lineNumber + " '" + line + "'", ex);
        } catch (Throwable th) {
            if (in != null) {
                in.close();
            }
            throw th;
        }
    }

    private void parseRuleLine(StringTokenizer st) {
        TZDBRule rule = new TZDBRule();
        String name = st.nextToken();
        if (!this.rules.containsKey(name)) {
            this.rules.put(name, new ArrayList());
        }
        this.rules.get(name).add(rule);
        rule.startYear = parseYear(st.nextToken(), 0);
        rule.endYear = parseYear(st.nextToken(), rule.startYear);
        if (rule.startYear <= rule.endYear) {
            parseOptional(st.nextToken());
            parseMonthDayTime(st, rule);
            rule.savingsAmount = parsePeriod(st.nextToken());
            rule.text = parseOptional(st.nextToken());
            return;
        }
        throw new IllegalArgumentException("Year order invalid: " + rule.startYear + " > " + rule.endYear);
    }

    private boolean parseZoneLine(StringTokenizer st, List<TZDBZone> zoneList) {
        TZDBZone zone = new TZDBZone();
        zoneList.add(zone);
        zone.standardOffset = parseOffset(st.nextToken());
        String savingsRule = parseOptional(st.nextToken());
        if (savingsRule == null) {
            zone.fixedSavingsSecs = 0;
            zone.savingsRule = null;
        } else {
            try {
                zone.fixedSavingsSecs = Integer.valueOf(parsePeriod(savingsRule));
                zone.savingsRule = null;
            } catch (Exception e) {
                zone.fixedSavingsSecs = null;
                zone.savingsRule = savingsRule;
            }
        }
        zone.text = st.nextToken();
        if (!st.hasMoreTokens()) {
            return true;
        }
        zone.year = Year.of(Integer.parseInt(st.nextToken()));
        if (st.hasMoreTokens()) {
            parseMonthDayTime(st, zone);
        }
        return false;
    }

    private void parseMonthDayTime(StringTokenizer st, TZDBMonthDayTime mdt) {
        mdt.month = parseMonth(st.nextToken());
        if (st.hasMoreTokens()) {
            String dayRule = st.nextToken();
            if (dayRule.startsWith("last")) {
                mdt.dayOfMonth = -1;
                mdt.dayOfWeek = parseDayOfWeek(dayRule.substring(4));
                mdt.adjustForwards = false;
            } else {
                int index = dayRule.indexOf(">=");
                if (index > 0) {
                    mdt.dayOfWeek = parseDayOfWeek(dayRule.substring(0, index));
                    dayRule = dayRule.substring(index + 2);
                } else {
                    int index2 = dayRule.indexOf("<=");
                    if (index2 > 0) {
                        mdt.dayOfWeek = parseDayOfWeek(dayRule.substring(0, index2));
                        mdt.adjustForwards = false;
                        dayRule = dayRule.substring(index2 + 2);
                    }
                }
                mdt.dayOfMonth = Integer.parseInt(dayRule);
            }
            if (st.hasMoreTokens() != 0) {
                String timeStr = st.nextToken();
                int secsOfDay = parseSecs(timeStr);
                if (secsOfDay == 86400) {
                    mdt.endOfDay = true;
                    secsOfDay = 0;
                }
                mdt.time = (LocalTime) deduplicate(LocalTime.ofSecondOfDay((long) secsOfDay));
                mdt.timeDefinition = parseTimeDefinition(timeStr.charAt(timeStr.length() - 1));
            }
        }
    }

    private int parseYear(String str, int defaultYear) {
        String str2 = str.toLowerCase();
        if (matches(str2, "minimum")) {
            return Year.MIN_VALUE;
        }
        if (matches(str2, "maximum")) {
            return Year.MAX_VALUE;
        }
        if (str2.equals("only")) {
            return defaultYear;
        }
        return Integer.parseInt(str2);
    }

    private Month parseMonth(String str) {
        String str2 = str.toLowerCase();
        for (Month moy : Month.values()) {
            if (matches(str2, moy.name().toLowerCase())) {
                return moy;
            }
        }
        throw new IllegalArgumentException("Unknown month: " + str2);
    }

    private DayOfWeek parseDayOfWeek(String str) {
        String str2 = str.toLowerCase();
        for (DayOfWeek dow : DayOfWeek.values()) {
            if (matches(str2, dow.name().toLowerCase())) {
                return dow;
            }
        }
        throw new IllegalArgumentException("Unknown day-of-week: " + str2);
    }

    private boolean matches(String str, String search) {
        return str.startsWith(search.substring(0, 3)) && search.startsWith(str) && str.length() <= search.length();
    }

    private String parseOptional(String str) {
        if (str.equals("-")) {
            return null;
        }
        return str;
    }

    private int parseSecs(String str) {
        String str2 = str;
        if (str2.equals("-")) {
            return 0;
        }
        int pos = 0;
        if (str2.startsWith("-")) {
            pos = 1;
        }
        ParsePosition pp = new ParsePosition(pos);
        TemporalAccessor parsed = TIME_PARSER.parseUnresolved(str2, pp);
        if (parsed == null || pp.getErrorIndex() >= 0) {
            throw new IllegalArgumentException(str2);
        }
        long hour = parsed.getLong(ChronoField.HOUR_OF_DAY);
        Long sec = null;
        Long min = parsed.isSupported(ChronoField.MINUTE_OF_HOUR) ? Long.valueOf(parsed.getLong(ChronoField.MINUTE_OF_HOUR)) : null;
        if (parsed.isSupported(ChronoField.SECOND_OF_MINUTE)) {
            sec = Long.valueOf(parsed.getLong(ChronoField.SECOND_OF_MINUTE));
        }
        long j = hour * 60 * 60;
        long j2 = 0;
        long longValue = min != null ? min.longValue() : 0;
        Long.signum(longValue);
        long j3 = j + (longValue * 60);
        if (sec != null) {
            j2 = sec.longValue();
        }
        int secs = (int) (j3 + j2);
        if (pos == 1) {
            return -secs;
        }
        return secs;
    }

    private ZoneOffset parseOffset(String str) {
        return ZoneOffset.ofTotalSeconds(parseSecs(str));
    }

    private int parsePeriod(String str) {
        return parseSecs(str);
    }

    private ZoneOffsetTransitionRule.TimeDefinition parseTimeDefinition(char c) {
        switch (c) {
            case 'G':
            case 'U':
            case 'Z':
            case 'g':
            case 'u':
            case 'z':
                return ZoneOffsetTransitionRule.TimeDefinition.UTC;
            case 'S':
            case 's':
                return ZoneOffsetTransitionRule.TimeDefinition.STANDARD;
            default:
                return ZoneOffsetTransitionRule.TimeDefinition.WALL;
        }
    }

    /* JADX DEBUG: Multi-variable search result rejected for TypeSearchVarInfo{r4v7, resolved type: java.lang.Object} */
    /* JADX DEBUG: Multi-variable search result rejected for TypeSearchVarInfo{r3v10, resolved type: org.threeten.bp.zone.ZoneRules} */
    /* JADX WARNING: Multi-variable type inference failed */
    /* Code decompiled incorrectly, please refer to instructions dump. */
    private void buildZoneRules() throws java.lang.Exception {
        /*
            r7 = this;
            java.util.Map<java.lang.String, java.util.List<org.threeten.bp.zone.TzdbZoneRulesCompiler$TZDBZone>> r0 = r7.zones
            java.util.Set r0 = r0.keySet()
            java.util.Iterator r0 = r0.iterator()
        L_0x000a:
            boolean r1 = r0.hasNext()
            if (r1 == 0) goto L_0x0067
            java.lang.Object r1 = r0.next()
            java.lang.String r1 = (java.lang.String) r1
            java.lang.StringBuilder r2 = new java.lang.StringBuilder
            r2.<init>()
            java.lang.String r3 = "Building zone "
            java.lang.StringBuilder r2 = r2.append(r3)
            java.lang.StringBuilder r2 = r2.append(r1)
            java.lang.String r2 = r2.toString()
            r7.printVerbose(r2)
            java.lang.Object r2 = r7.deduplicate(r1)
            r1 = r2
            java.lang.String r1 = (java.lang.String) r1
            java.util.Map<java.lang.String, java.util.List<org.threeten.bp.zone.TzdbZoneRulesCompiler$TZDBZone>> r2 = r7.zones
            java.lang.Object r2 = r2.get(r1)
            java.util.List r2 = (java.util.List) r2
            org.threeten.bp.zone.ZoneRulesBuilder r3 = new org.threeten.bp.zone.ZoneRulesBuilder
            r3.<init>()
            java.util.Iterator r4 = r2.iterator()
        L_0x0044:
            boolean r5 = r4.hasNext()
            if (r5 == 0) goto L_0x0057
            java.lang.Object r5 = r4.next()
            org.threeten.bp.zone.TzdbZoneRulesCompiler$TZDBZone r5 = (org.threeten.bp.zone.TzdbZoneRulesCompiler.TZDBZone) r5
            java.util.Map<java.lang.String, java.util.List<org.threeten.bp.zone.TzdbZoneRulesCompiler$TZDBRule>> r6 = r7.rules
            org.threeten.bp.zone.ZoneRulesBuilder r3 = r5.addToBuilder(r3, r6)
            goto L_0x0044
        L_0x0057:
            java.util.Map<java.lang.Object, java.lang.Object> r4 = r7.deduplicateMap
            org.threeten.bp.zone.ZoneRules r4 = r3.toRules(r1, r4)
            java.util.SortedMap<java.lang.String, org.threeten.bp.zone.ZoneRules> r5 = r7.builtZones
            java.lang.Object r6 = r7.deduplicate(r4)
            r5.put(r1, r6)
            goto L_0x000a
        L_0x0067:
            java.util.Map<java.lang.String, java.lang.String> r0 = r7.links
            java.util.Set r0 = r0.keySet()
            java.util.Iterator r0 = r0.iterator()
        L_0x0071:
            boolean r1 = r0.hasNext()
            if (r1 == 0) goto L_0x0125
            java.lang.Object r1 = r0.next()
            java.lang.String r1 = (java.lang.String) r1
            java.lang.Object r2 = r7.deduplicate(r1)
            r1 = r2
            java.lang.String r1 = (java.lang.String) r1
            java.util.Map<java.lang.String, java.lang.String> r2 = r7.links
            java.lang.Object r2 = r2.get(r1)
            java.lang.String r2 = (java.lang.String) r2
            java.lang.StringBuilder r3 = new java.lang.StringBuilder
            r3.<init>()
            java.lang.String r4 = "Linking alias "
            java.lang.StringBuilder r3 = r3.append(r4)
            java.lang.StringBuilder r3 = r3.append(r1)
            java.lang.String r4 = " to "
            java.lang.StringBuilder r3 = r3.append(r4)
            java.lang.StringBuilder r3 = r3.append(r2)
            java.lang.String r3 = r3.toString()
            r7.printVerbose(r3)
            java.util.SortedMap<java.lang.String, org.threeten.bp.zone.ZoneRules> r3 = r7.builtZones
            java.lang.Object r3 = r3.get(r2)
            org.threeten.bp.zone.ZoneRules r3 = (org.threeten.bp.zone.ZoneRules) r3
            if (r3 != 0) goto L_0x011e
            java.util.Map<java.lang.String, java.lang.String> r5 = r7.links
            java.lang.Object r5 = r5.get(r2)
            r2 = r5
            java.lang.String r2 = (java.lang.String) r2
            java.lang.StringBuilder r5 = new java.lang.StringBuilder
            r5.<init>()
            java.lang.String r6 = "Relinking alias "
            java.lang.StringBuilder r5 = r5.append(r6)
            java.lang.StringBuilder r5 = r5.append(r1)
            java.lang.StringBuilder r4 = r5.append(r4)
            java.lang.StringBuilder r4 = r4.append(r2)
            java.lang.String r4 = r4.toString()
            r7.printVerbose(r4)
            java.util.SortedMap<java.lang.String, org.threeten.bp.zone.ZoneRules> r4 = r7.builtZones
            java.lang.Object r4 = r4.get(r2)
            r3 = r4
            org.threeten.bp.zone.ZoneRules r3 = (org.threeten.bp.zone.ZoneRules) r3
            if (r3 == 0) goto L_0x00e9
            goto L_0x011e
        L_0x00e9:
            java.lang.IllegalArgumentException r4 = new java.lang.IllegalArgumentException
            java.lang.StringBuilder r5 = new java.lang.StringBuilder
            r5.<init>()
            java.lang.String r6 = "Alias '"
            java.lang.StringBuilder r5 = r5.append(r6)
            java.lang.StringBuilder r5 = r5.append(r1)
            java.lang.String r6 = "' links to invalid zone '"
            java.lang.StringBuilder r5 = r5.append(r6)
            java.lang.StringBuilder r5 = r5.append(r2)
            java.lang.String r6 = "' for '"
            java.lang.StringBuilder r5 = r5.append(r6)
            java.lang.String r6 = r7.version
            java.lang.StringBuilder r5 = r5.append(r6)
            java.lang.String r6 = "'"
            java.lang.StringBuilder r5 = r5.append(r6)
            java.lang.String r5 = r5.toString()
            r4.<init>(r5)
            throw r4
        L_0x011e:
            java.util.SortedMap<java.lang.String, org.threeten.bp.zone.ZoneRules> r4 = r7.builtZones
            r4.put(r1, r3)
            goto L_0x0071
        L_0x0125:
            java.util.SortedMap<java.lang.String, org.threeten.bp.zone.ZoneRules> r0 = r7.builtZones
            java.lang.String r1 = "UTC"
            r0.remove(r1)
            java.util.SortedMap<java.lang.String, org.threeten.bp.zone.ZoneRules> r0 = r7.builtZones
            java.lang.String r1 = "GMT"
            r0.remove(r1)
            java.util.SortedMap<java.lang.String, org.threeten.bp.zone.ZoneRules> r0 = r7.builtZones
            java.lang.String r1 = "GMT0"
            r0.remove(r1)
            java.util.SortedMap<java.lang.String, org.threeten.bp.zone.ZoneRules> r0 = r7.builtZones
            java.lang.String r1 = "GMT+0"
            r0.remove(r1)
            java.util.SortedMap<java.lang.String, org.threeten.bp.zone.ZoneRules> r0 = r7.builtZones
            java.lang.String r1 = "GMT-0"
            r0.remove(r1)
            return
        */
        throw new UnsupportedOperationException("Method not decompiled: org.threeten.bp.zone.TzdbZoneRulesCompiler.buildZoneRules():void");
    }

    /* access modifiers changed from: package-private */
    public <T> T deduplicate(T object) {
        if (!this.deduplicateMap.containsKey(object)) {
            this.deduplicateMap.put(object, object);
        }
        return this.deduplicateMap.get(object);
    }

    private void printVerbose(String message) {
        if (this.verbose) {
            System.out.println(message);
        }
    }

    abstract class TZDBMonthDayTime {
        boolean adjustForwards = true;
        int dayOfMonth = 1;
        DayOfWeek dayOfWeek;
        boolean endOfDay;
        Month month = Month.JANUARY;
        LocalTime time = LocalTime.MIDNIGHT;
        ZoneOffsetTransitionRule.TimeDefinition timeDefinition = ZoneOffsetTransitionRule.TimeDefinition.WALL;

        TZDBMonthDayTime() {
        }

        /* access modifiers changed from: package-private */
        public void adjustToFowards(int year) {
            int i;
            if (!this.adjustForwards && (i = this.dayOfMonth) > 0) {
                LocalDate adjustedDate = LocalDate.of(year, this.month, i).minusDays(6);
                this.dayOfMonth = adjustedDate.getDayOfMonth();
                this.month = adjustedDate.getMonth();
                this.adjustForwards = true;
            }
        }
    }

    final class TZDBRule extends TZDBMonthDayTime {
        int endYear;
        int savingsAmount;
        int startYear;
        String text;

        TZDBRule() {
            super();
        }

        /* access modifiers changed from: package-private */
        public void addToBuilder(ZoneRulesBuilder bld) {
            adjustToFowards(2004);
            bld.addRuleToWindow(this.startYear, this.endYear, this.month, this.dayOfMonth, this.dayOfWeek, this.time, this.endOfDay, this.timeDefinition, this.savingsAmount);
        }
    }

    final class TZDBZone extends TZDBMonthDayTime {
        Integer fixedSavingsSecs;
        String savingsRule;
        ZoneOffset standardOffset;
        String text;
        Year year;

        TZDBZone() {
            super();
        }

        /* access modifiers changed from: package-private */
        public ZoneRulesBuilder addToBuilder(ZoneRulesBuilder bld, Map<String, List<TZDBRule>> rules) {
            Year year2 = this.year;
            if (year2 != null) {
                bld.addWindow(this.standardOffset, toDateTime(year2.getValue()), this.timeDefinition);
            } else {
                bld.addWindowForever(this.standardOffset);
            }
            Integer num = this.fixedSavingsSecs;
            if (num != null) {
                bld.setFixedSavingsToWindow(num.intValue());
            } else {
                List<TZDBRule> tzdbRules = rules.get(this.savingsRule);
                if (tzdbRules != null) {
                    for (TZDBRule tzdbRule : tzdbRules) {
                        tzdbRule.addToBuilder(bld);
                    }
                } else {
                    throw new IllegalArgumentException("Rule not found: " + this.savingsRule);
                }
            }
            return bld;
        }

        private LocalDateTime toDateTime(int year2) {
            LocalDate date;
            adjustToFowards(year2);
            if (this.dayOfMonth == -1) {
                this.dayOfMonth = this.month.length(Year.isLeap((long) year2));
                date = LocalDate.of(year2, this.month, this.dayOfMonth);
                if (this.dayOfWeek != null) {
                    date = date.with(TemporalAdjusters.previousOrSame(this.dayOfWeek));
                }
            } else {
                date = LocalDate.of(year2, this.month, this.dayOfMonth);
                if (this.dayOfWeek != null) {
                    date = date.with(TemporalAdjusters.nextOrSame(this.dayOfWeek));
                }
            }
            LocalDateTime ldt = LocalDateTime.of((LocalDate) TzdbZoneRulesCompiler.this.deduplicate(date), this.time);
            if (this.endOfDay) {
                return ldt.plusDays(1);
            }
            return ldt;
        }
    }

    static final class LeapSecondRule {
        final LocalDate leapDate;
        byte secondAdjustment;

        public LeapSecondRule(LocalDate leapDate2, byte secondAdjustment2) {
            this.leapDate = leapDate2;
            this.secondAdjustment = secondAdjustment2;
        }
    }
}
