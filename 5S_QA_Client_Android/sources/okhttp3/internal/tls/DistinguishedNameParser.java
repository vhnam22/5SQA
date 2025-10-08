package okhttp3.internal.tls;

import javax.security.auth.x500.X500Principal;

final class DistinguishedNameParser {
    private int beg;
    private char[] chars;
    private int cur;
    private final String dn;
    private int end;
    private final int length;
    private int pos;

    DistinguishedNameParser(X500Principal principal) {
        String name = principal.getName("RFC2253");
        this.dn = name;
        this.length = name.length();
    }

    /* JADX WARNING: Removed duplicated region for block: B:6:0x0015 A[RETURN] */
    /* JADX WARNING: Removed duplicated region for block: B:7:0x0017  */
    /* Code decompiled incorrectly, please refer to instructions dump. */
    private java.lang.String nextAT() {
        /*
            r6 = this;
        L_0x0000:
            int r0 = r6.pos
            int r1 = r6.length
            r2 = 32
            if (r0 >= r1) goto L_0x0013
            char[] r3 = r6.chars
            char r3 = r3[r0]
            if (r3 != r2) goto L_0x0013
            int r0 = r0 + 1
            r6.pos = r0
            goto L_0x0000
        L_0x0013:
            if (r0 != r1) goto L_0x0017
            r0 = 0
            return r0
        L_0x0017:
            r6.beg = r0
            int r0 = r0 + 1
            r6.pos = r0
        L_0x001d:
            int r0 = r6.pos
            int r1 = r6.length
            r3 = 61
            if (r0 >= r1) goto L_0x0032
            char[] r4 = r6.chars
            char r4 = r4[r0]
            if (r4 == r3) goto L_0x0032
            if (r4 == r2) goto L_0x0032
            int r0 = r0 + 1
            r6.pos = r0
            goto L_0x001d
        L_0x0032:
            java.lang.String r4 = "Unexpected end of DN: "
            if (r0 >= r1) goto L_0x00d5
            r6.end = r0
            char[] r1 = r6.chars
            char r0 = r1[r0]
            if (r0 != r2) goto L_0x0073
        L_0x003e:
            int r0 = r6.pos
            int r1 = r6.length
            if (r0 >= r1) goto L_0x0051
            char[] r5 = r6.chars
            char r5 = r5[r0]
            if (r5 == r3) goto L_0x0051
            if (r5 != r2) goto L_0x0051
            int r0 = r0 + 1
            r6.pos = r0
            goto L_0x003e
        L_0x0051:
            char[] r5 = r6.chars
            char r5 = r5[r0]
            if (r5 != r3) goto L_0x005a
            if (r0 == r1) goto L_0x005a
            goto L_0x0073
        L_0x005a:
            java.lang.IllegalStateException r0 = new java.lang.IllegalStateException
            java.lang.StringBuilder r1 = new java.lang.StringBuilder
            r1.<init>()
            java.lang.StringBuilder r1 = r1.append(r4)
            java.lang.String r2 = r6.dn
            java.lang.StringBuilder r1 = r1.append(r2)
            java.lang.String r1 = r1.toString()
            r0.<init>(r1)
            throw r0
        L_0x0073:
            int r0 = r6.pos
            int r0 = r0 + 1
            r6.pos = r0
        L_0x0079:
            int r0 = r6.pos
            int r1 = r6.length
            if (r0 >= r1) goto L_0x008a
            char[] r1 = r6.chars
            char r1 = r1[r0]
            if (r1 != r2) goto L_0x008a
            int r0 = r0 + 1
            r6.pos = r0
            goto L_0x0079
        L_0x008a:
            int r0 = r6.end
            int r1 = r6.beg
            int r2 = r0 - r1
            r3 = 4
            if (r2 <= r3) goto L_0x00ca
            char[] r2 = r6.chars
            int r4 = r1 + 3
            char r4 = r2[r4]
            r5 = 46
            if (r4 != r5) goto L_0x00ca
            char r4 = r2[r1]
            r5 = 79
            if (r4 == r5) goto L_0x00a7
            r5 = 111(0x6f, float:1.56E-43)
            if (r4 != r5) goto L_0x00ca
        L_0x00a7:
            int r4 = r1 + 1
            char r4 = r2[r4]
            r5 = 73
            if (r4 == r5) goto L_0x00b7
            int r4 = r1 + 1
            char r4 = r2[r4]
            r5 = 105(0x69, float:1.47E-43)
            if (r4 != r5) goto L_0x00ca
        L_0x00b7:
            int r4 = r1 + 2
            char r4 = r2[r4]
            r5 = 68
            if (r4 == r5) goto L_0x00c7
            int r4 = r1 + 2
            char r2 = r2[r4]
            r4 = 100
            if (r2 != r4) goto L_0x00ca
        L_0x00c7:
            int r1 = r1 + r3
            r6.beg = r1
        L_0x00ca:
            java.lang.String r1 = new java.lang.String
            char[] r2 = r6.chars
            int r3 = r6.beg
            int r0 = r0 - r3
            r1.<init>(r2, r3, r0)
            return r1
        L_0x00d5:
            java.lang.IllegalStateException r0 = new java.lang.IllegalStateException
            java.lang.StringBuilder r1 = new java.lang.StringBuilder
            r1.<init>()
            java.lang.StringBuilder r1 = r1.append(r4)
            java.lang.String r2 = r6.dn
            java.lang.StringBuilder r1 = r1.append(r2)
            java.lang.String r1 = r1.toString()
            r0.<init>(r1)
            throw r0
        */
        throw new UnsupportedOperationException("Method not decompiled: okhttp3.internal.tls.DistinguishedNameParser.nextAT():java.lang.String");
    }

    private String quotedAV() {
        int i = this.pos + 1;
        this.pos = i;
        this.beg = i;
        this.end = i;
        while (true) {
            int i2 = this.pos;
            if (i2 != this.length) {
                char[] cArr = this.chars;
                char c = cArr[i2];
                if (c == '\"') {
                    this.pos = i2 + 1;
                    while (true) {
                        int i3 = this.pos;
                        if (i3 >= this.length || this.chars[i3] != ' ') {
                            char[] cArr2 = this.chars;
                            int i4 = this.beg;
                        } else {
                            this.pos = i3 + 1;
                        }
                    }
                    char[] cArr22 = this.chars;
                    int i42 = this.beg;
                    return new String(cArr22, i42, this.end - i42);
                }
                if (c == '\\') {
                    cArr[this.end] = getEscaped();
                } else {
                    cArr[this.end] = c;
                }
                this.pos++;
                this.end++;
            } else {
                throw new IllegalStateException("Unexpected end of DN: " + this.dn);
            }
        }
    }

    /* JADX WARNING: Code restructure failed: missing block: B:5:0x0016, code lost:
        r1 = r5.chars;
     */
    /* Code decompiled incorrectly, please refer to instructions dump. */
    private java.lang.String hexAV() {
        /*
            r5 = this;
            int r0 = r5.pos
            int r1 = r0 + 4
            int r2 = r5.length
            java.lang.String r3 = "Unexpected end of DN: "
            if (r1 >= r2) goto L_0x009c
            r5.beg = r0
            int r0 = r0 + 1
            r5.pos = r0
        L_0x0010:
            int r0 = r5.pos
            int r1 = r5.length
            if (r0 == r1) goto L_0x0054
            char[] r1 = r5.chars
            char r2 = r1[r0]
            r4 = 43
            if (r2 == r4) goto L_0x0054
            r4 = 44
            if (r2 == r4) goto L_0x0054
            r4 = 59
            if (r2 != r4) goto L_0x0027
            goto L_0x0054
        L_0x0027:
            r4 = 32
            if (r2 != r4) goto L_0x0042
            r5.end = r0
            int r0 = r0 + 1
            r5.pos = r0
        L_0x0031:
            int r0 = r5.pos
            int r1 = r5.length
            if (r0 >= r1) goto L_0x0057
            char[] r1 = r5.chars
            char r1 = r1[r0]
            if (r1 != r4) goto L_0x0057
            int r0 = r0 + 1
            r5.pos = r0
            goto L_0x0031
        L_0x0042:
            r4 = 65
            if (r2 < r4) goto L_0x004f
            r4 = 70
            if (r2 > r4) goto L_0x004f
            int r2 = r2 + 32
            char r2 = (char) r2
            r1[r0] = r2
        L_0x004f:
            int r0 = r0 + 1
            r5.pos = r0
            goto L_0x0010
        L_0x0054:
            r5.end = r0
        L_0x0057:
            int r0 = r5.end
            int r1 = r5.beg
            int r0 = r0 - r1
            r2 = 5
            if (r0 < r2) goto L_0x0083
            r2 = r0 & 1
            if (r2 == 0) goto L_0x0083
            int r2 = r0 / 2
            byte[] r2 = new byte[r2]
            r3 = 0
            int r1 = r1 + 1
        L_0x006a:
            int r4 = r2.length
            if (r3 >= r4) goto L_0x0079
            int r4 = r5.getByte(r1)
            byte r4 = (byte) r4
            r2[r3] = r4
            int r1 = r1 + 2
            int r3 = r3 + 1
            goto L_0x006a
        L_0x0079:
            java.lang.String r1 = new java.lang.String
            char[] r3 = r5.chars
            int r4 = r5.beg
            r1.<init>(r3, r4, r0)
            return r1
        L_0x0083:
            java.lang.IllegalStateException r1 = new java.lang.IllegalStateException
            java.lang.StringBuilder r2 = new java.lang.StringBuilder
            r2.<init>()
            java.lang.StringBuilder r2 = r2.append(r3)
            java.lang.String r3 = r5.dn
            java.lang.StringBuilder r2 = r2.append(r3)
            java.lang.String r2 = r2.toString()
            r1.<init>(r2)
            throw r1
        L_0x009c:
            java.lang.IllegalStateException r0 = new java.lang.IllegalStateException
            java.lang.StringBuilder r1 = new java.lang.StringBuilder
            r1.<init>()
            java.lang.StringBuilder r1 = r1.append(r3)
            java.lang.String r2 = r5.dn
            java.lang.StringBuilder r1 = r1.append(r2)
            java.lang.String r1 = r1.toString()
            r0.<init>(r1)
            throw r0
        */
        throw new UnsupportedOperationException("Method not decompiled: okhttp3.internal.tls.DistinguishedNameParser.hexAV():java.lang.String");
    }

    private String escapedAV() {
        int i;
        int i2;
        char c;
        int i3 = this.pos;
        this.beg = i3;
        this.end = i3;
        while (true) {
            int i4 = this.pos;
            if (i4 < this.length) {
                char[] cArr = this.chars;
                char c2 = cArr[i4];
                switch (c2) {
                    case ' ':
                        int i5 = this.end;
                        this.cur = i5;
                        this.pos = i4 + 1;
                        this.end = i5 + 1;
                        cArr[i5] = ' ';
                        while (true) {
                            i = this.pos;
                            i2 = this.length;
                            if (i < i2) {
                                char[] cArr2 = this.chars;
                                if (cArr2[i] == ' ') {
                                    int i6 = this.end;
                                    this.end = i6 + 1;
                                    cArr2[i6] = ' ';
                                    this.pos = i + 1;
                                }
                            }
                        }
                        if (i != i2 && (c = this.chars[i]) != ',' && c != '+' && c != ';') {
                            break;
                        } else {
                            char[] cArr3 = this.chars;
                            int i7 = this.beg;
                            break;
                        }
                    case '+':
                    case ',':
                    case ';':
                        int i8 = this.beg;
                        return new String(cArr, i8, this.end - i8);
                    case '\\':
                        int i9 = this.end;
                        this.end = i9 + 1;
                        cArr[i9] = getEscaped();
                        this.pos++;
                        break;
                    default:
                        int i10 = this.end;
                        this.end = i10 + 1;
                        cArr[i10] = c2;
                        this.pos = i4 + 1;
                        break;
                }
            } else {
                char[] cArr4 = this.chars;
                int i11 = this.beg;
                return new String(cArr4, i11, this.end - i11);
            }
        }
        char[] cArr32 = this.chars;
        int i72 = this.beg;
        return new String(cArr32, i72, this.cur - i72);
    }

    private char getEscaped() {
        int i = this.pos + 1;
        this.pos = i;
        if (i != this.length) {
            char c = this.chars[i];
            switch (c) {
                case ' ':
                case '\"':
                case '#':
                case '%':
                case '*':
                case '+':
                case ',':
                case ';':
                case '<':
                case '=':
                case '>':
                case '\\':
                case '_':
                    return c;
                default:
                    return getUTF8();
            }
        } else {
            throw new IllegalStateException("Unexpected end of DN: " + this.dn);
        }
    }

    private char getUTF8() {
        int count;
        int res;
        int res2 = getByte(this.pos);
        this.pos++;
        if (res2 < 128) {
            return (char) res2;
        }
        if (res2 < 192 || res2 > 247) {
            return '?';
        }
        if (res2 <= 223) {
            count = 1;
            res = res2 & 31;
        } else if (res2 <= 239) {
            count = 2;
            res = res2 & 15;
        } else {
            count = 3;
            res = res2 & 7;
        }
        for (int i = 0; i < count; i++) {
            int i2 = this.pos + 1;
            this.pos = i2;
            if (i2 == this.length || this.chars[i2] != '\\') {
                return '?';
            }
            int i3 = i2 + 1;
            this.pos = i3;
            int b = getByte(i3);
            this.pos++;
            if ((b & 192) != 128) {
                return '?';
            }
            res = (res << 6) + (b & 63);
        }
        return (char) res;
    }

    private int getByte(int position) {
        int b1;
        int b2;
        if (position + 1 < this.length) {
            char[] cArr = this.chars;
            char b12 = cArr[position];
            if (b12 >= '0' && b12 <= '9') {
                b1 = b12 - '0';
            } else if (b12 >= 'a' && b12 <= 'f') {
                b1 = b12 - 'W';
            } else if (b12 < 'A' || b12 > 'F') {
                throw new IllegalStateException("Malformed DN: " + this.dn);
            } else {
                b1 = b12 - '7';
            }
            char b22 = cArr[position + 1];
            if (b22 >= '0' && b22 <= '9') {
                b2 = b22 - '0';
            } else if (b22 >= 'a' && b22 <= 'f') {
                b2 = b22 - 'W';
            } else if (b22 < 'A' || b22 > 'F') {
                throw new IllegalStateException("Malformed DN: " + this.dn);
            } else {
                b2 = b22 - '7';
            }
            return (b1 << 4) + b2;
        }
        throw new IllegalStateException("Malformed DN: " + this.dn);
    }

    public String findMostSpecific(String attributeType) {
        this.pos = 0;
        this.beg = 0;
        this.end = 0;
        this.cur = 0;
        this.chars = this.dn.toCharArray();
        String attType = nextAT();
        if (attType == null) {
            return null;
        }
        do {
            String attValue = "";
            int i = this.pos;
            if (i == this.length) {
                return null;
            }
            switch (this.chars[i]) {
                case '\"':
                    attValue = quotedAV();
                    break;
                case '#':
                    attValue = hexAV();
                    break;
                case '+':
                case ',':
                case ';':
                    break;
                default:
                    attValue = escapedAV();
                    break;
            }
            if (attributeType.equalsIgnoreCase(attType)) {
                return attValue;
            }
            int i2 = this.pos;
            if (i2 >= this.length) {
                return null;
            }
            char c = this.chars[i2];
            if (c == ',' || c == ';' || c == '+') {
                this.pos = i2 + 1;
                attType = nextAT();
            } else {
                throw new IllegalStateException("Malformed DN: " + this.dn);
            }
        } while (attType != null);
        throw new IllegalStateException("Malformed DN: " + this.dn);
    }
}
