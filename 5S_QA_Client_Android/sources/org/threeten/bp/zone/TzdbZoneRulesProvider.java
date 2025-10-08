package org.threeten.bp.zone;

import java.io.ByteArrayInputStream;
import java.io.DataInputStream;
import java.io.IOException;
import java.io.InputStream;
import java.io.StreamCorruptedException;
import java.net.URL;
import java.util.Arrays;
import java.util.Enumeration;
import java.util.HashSet;
import java.util.List;
import java.util.NavigableMap;
import java.util.Set;
import java.util.TreeMap;
import java.util.concurrent.ConcurrentNavigableMap;
import java.util.concurrent.ConcurrentSkipListMap;
import java.util.concurrent.CopyOnWriteArraySet;
import java.util.concurrent.atomic.AtomicReferenceArray;
import org.threeten.bp.jdk8.Jdk8Methods;

public final class TzdbZoneRulesProvider extends ZoneRulesProvider {
    private Set<String> loadedUrls = new CopyOnWriteArraySet();
    private List<String> regionIds;
    private final ConcurrentNavigableMap<String, Version> versions = new ConcurrentSkipListMap();

    public TzdbZoneRulesProvider() {
        if (!load(ZoneRulesProvider.class.getClassLoader())) {
            throw new ZoneRulesException("No time-zone rules found for 'TZDB'");
        }
    }

    public TzdbZoneRulesProvider(URL url) {
        try {
            if (!load(url)) {
                throw new ZoneRulesException("No time-zone rules found: " + url);
            }
        } catch (Exception ex) {
            throw new ZoneRulesException("Unable to load TZDB time-zone rules: " + url, ex);
        }
    }

    public TzdbZoneRulesProvider(InputStream stream) {
        try {
            load(stream);
        } catch (Exception ex) {
            throw new ZoneRulesException("Unable to load TZDB time-zone rules", ex);
        }
    }

    /* access modifiers changed from: protected */
    public Set<String> provideZoneIds() {
        return new HashSet(this.regionIds);
    }

    /* access modifiers changed from: protected */
    public ZoneRules provideRules(String zoneId, boolean forCaching) {
        Jdk8Methods.requireNonNull(zoneId, "zoneId");
        ZoneRules rules = ((Version) this.versions.lastEntry().getValue()).getRules(zoneId);
        if (rules != null) {
            return rules;
        }
        throw new ZoneRulesException("Unknown time-zone ID: " + zoneId);
    }

    /* access modifiers changed from: protected */
    public NavigableMap<String, ZoneRules> provideVersions(String zoneId) {
        TreeMap<String, ZoneRules> map = new TreeMap<>();
        for (Version version : this.versions.values()) {
            ZoneRules rules = version.getRules(zoneId);
            if (rules != null) {
                map.put(version.versionId, rules);
            }
        }
        return map;
    }

    private boolean load(ClassLoader classLoader) {
        boolean updated = false;
        try {
            Enumeration<URL> en = classLoader.getResources("org/threeten/bp/TZDB.dat");
            while (en.hasMoreElements()) {
                updated |= load(en.nextElement());
            }
            return updated;
        } catch (Exception ex) {
            throw new ZoneRulesException("Unable to load TZDB time-zone rules: " + null, ex);
        }
    }

    private boolean load(URL url) throws ClassNotFoundException, IOException, ZoneRulesException {
        boolean updated = false;
        if (this.loadedUrls.add(url.toExternalForm())) {
            InputStream in = null;
            try {
                in = url.openStream();
                updated = false | load(in);
            } finally {
                if (in != null) {
                    in.close();
                }
            }
        }
        return updated;
    }

    private boolean load(InputStream in) throws IOException, StreamCorruptedException {
        boolean updated = false;
        for (Version loadedVersion : loadData(in)) {
            Version existing = (Version) this.versions.putIfAbsent(loadedVersion.versionId, loadedVersion);
            if (existing == null || existing.versionId.equals(loadedVersion.versionId)) {
                updated = true;
            } else {
                throw new ZoneRulesException("Data already loaded for TZDB time-zone rules version: " + loadedVersion.versionId);
            }
        }
        return updated;
    }

    /* JADX DEBUG: Multi-variable search result rejected for TypeSearchVarInfo{r9v0, resolved type: java.lang.Object[]} */
    /* JADX WARNING: Multi-variable type inference failed */
    /* Code decompiled incorrectly, please refer to instructions dump. */
    private java.lang.Iterable<org.threeten.bp.zone.TzdbZoneRulesProvider.Version> loadData(java.io.InputStream r18) throws java.io.IOException, java.io.StreamCorruptedException {
        /*
            r17 = this;
            java.io.DataInputStream r0 = new java.io.DataInputStream
            r1 = r18
            r0.<init>(r1)
            byte r2 = r0.readByte()
            java.lang.String r3 = "File format not recognised"
            r4 = 1
            if (r2 != r4) goto L_0x00a7
            java.lang.String r2 = r0.readUTF()
            java.lang.String r4 = "TZDB"
            boolean r4 = r4.equals(r2)
            if (r4 == 0) goto L_0x009f
            short r3 = r0.readShort()
            java.lang.String[] r4 = new java.lang.String[r3]
            r5 = 0
        L_0x0023:
            if (r5 >= r3) goto L_0x002e
            java.lang.String r6 = r0.readUTF()
            r4[r5] = r6
            int r5 = r5 + 1
            goto L_0x0023
        L_0x002e:
            short r5 = r0.readShort()
            java.lang.String[] r6 = new java.lang.String[r5]
            r7 = 0
        L_0x0035:
            if (r7 >= r5) goto L_0x0040
            java.lang.String r8 = r0.readUTF()
            r6[r7] = r8
            int r7 = r7 + 1
            goto L_0x0035
        L_0x0040:
            java.util.List r7 = java.util.Arrays.asList(r6)
            r8 = r17
            r8.regionIds = r7
            short r7 = r0.readShort()
            java.lang.Object[] r9 = new java.lang.Object[r7]
            r10 = 0
        L_0x004f:
            if (r10 >= r7) goto L_0x005f
            short r11 = r0.readShort()
            byte[] r11 = new byte[r11]
            r0.readFully(r11)
            r9[r10] = r11
            int r10 = r10 + 1
            goto L_0x004f
        L_0x005f:
            java.util.concurrent.atomic.AtomicReferenceArray r10 = new java.util.concurrent.atomic.AtomicReferenceArray
            r10.<init>(r9)
            java.util.HashSet r11 = new java.util.HashSet
            r11.<init>(r3)
            r12 = 0
        L_0x006a:
            if (r12 >= r3) goto L_0x009e
            short r13 = r0.readShort()
            java.lang.String[] r14 = new java.lang.String[r13]
            short[] r15 = new short[r13]
            r16 = 0
            r1 = r16
        L_0x0078:
            if (r1 >= r13) goto L_0x008b
            short r16 = r0.readShort()
            r16 = r6[r16]
            r14[r1] = r16
            short r16 = r0.readShort()
            r15[r1] = r16
            int r1 = r1 + 1
            goto L_0x0078
        L_0x008b:
            org.threeten.bp.zone.TzdbZoneRulesProvider$Version r1 = new org.threeten.bp.zone.TzdbZoneRulesProvider$Version
            r16 = r0
            r0 = r4[r12]
            r1.<init>(r0, r14, r15, r10)
            r11.add(r1)
            int r12 = r12 + 1
            r1 = r18
            r0 = r16
            goto L_0x006a
        L_0x009e:
            return r11
        L_0x009f:
            r16 = r0
            java.io.StreamCorruptedException r0 = new java.io.StreamCorruptedException
            r0.<init>(r3)
            throw r0
        L_0x00a7:
            r16 = r0
            java.io.StreamCorruptedException r0 = new java.io.StreamCorruptedException
            r0.<init>(r3)
            throw r0
        */
        throw new UnsupportedOperationException("Method not decompiled: org.threeten.bp.zone.TzdbZoneRulesProvider.loadData(java.io.InputStream):java.lang.Iterable");
    }

    public String toString() {
        return "TZDB";
    }

    static class Version {
        private final String[] regionArray;
        private final AtomicReferenceArray<Object> ruleData;
        private final short[] ruleIndices;
        /* access modifiers changed from: private */
        public final String versionId;

        Version(String versionId2, String[] regionIds, short[] ruleIndices2, AtomicReferenceArray<Object> ruleData2) {
            this.ruleData = ruleData2;
            this.versionId = versionId2;
            this.regionArray = regionIds;
            this.ruleIndices = ruleIndices2;
        }

        /* access modifiers changed from: package-private */
        public ZoneRules getRules(String regionId) {
            int regionIndex = Arrays.binarySearch(this.regionArray, regionId);
            if (regionIndex < 0) {
                return null;
            }
            try {
                return createRule(this.ruleIndices[regionIndex]);
            } catch (Exception ex) {
                throw new ZoneRulesException("Invalid binary time-zone data: TZDB:" + regionId + ", version: " + this.versionId, ex);
            }
        }

        /* access modifiers changed from: package-private */
        public ZoneRules createRule(short index) throws Exception {
            Object obj = this.ruleData.get(index);
            if (obj instanceof byte[]) {
                obj = Ser.read(new DataInputStream(new ByteArrayInputStream((byte[]) obj)));
                this.ruleData.set(index, obj);
            }
            return (ZoneRules) obj;
        }

        public String toString() {
            return this.versionId;
        }
    }
}
