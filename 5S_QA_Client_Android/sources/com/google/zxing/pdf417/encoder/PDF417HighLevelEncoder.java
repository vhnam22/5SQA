package com.google.zxing.pdf417.encoder;

import com.google.zxing.WriterException;
import com.google.zxing.common.CharacterSetECI;
import java.math.BigInteger;
import java.nio.charset.Charset;
import java.nio.charset.CharsetEncoder;
import java.nio.charset.StandardCharsets;
import java.util.Arrays;
import kotlin.UByte;

final class PDF417HighLevelEncoder {
    private static final int BYTE_COMPACTION = 1;
    private static final Charset DEFAULT_ENCODING = StandardCharsets.ISO_8859_1;
    private static final int ECI_CHARSET = 927;
    private static final int ECI_GENERAL_PURPOSE = 926;
    private static final int ECI_USER_DEFINED = 925;
    private static final int LATCH_TO_BYTE = 924;
    private static final int LATCH_TO_BYTE_PADDED = 901;
    private static final int LATCH_TO_NUMERIC = 902;
    private static final int LATCH_TO_TEXT = 900;
    private static final byte[] MIXED;
    private static final int NUMERIC_COMPACTION = 2;
    private static final byte[] PUNCTUATION = new byte[128];
    private static final int SHIFT_TO_BYTE = 913;
    private static final int SUBMODE_ALPHA = 0;
    private static final int SUBMODE_LOWER = 1;
    private static final int SUBMODE_MIXED = 2;
    private static final int SUBMODE_PUNCTUATION = 3;
    private static final int TEXT_COMPACTION = 0;
    private static final byte[] TEXT_MIXED_RAW = {48, 49, 50, 51, 52, 53, 54, 55, 56, 57, 38, 13, 9, 44, 58, 35, 45, 46, 36, 47, 43, 37, 42, 61, 94, 0, 32, 0, 0, 0};
    private static final byte[] TEXT_PUNCTUATION_RAW = {59, 60, 62, 64, 91, 92, 93, 95, 96, 126, 33, 13, 9, 44, 58, 10, 45, 46, 36, 47, 34, 124, 42, 40, 41, 63, 123, 125, 39, 0};

    static {
        byte[] bArr = new byte[128];
        MIXED = bArr;
        Arrays.fill(bArr, (byte) -1);
        int i = 0;
        int i2 = 0;
        while (true) {
            byte[] bArr2 = TEXT_MIXED_RAW;
            if (i2 >= bArr2.length) {
                break;
            }
            byte b = bArr2[i2];
            if (b > 0) {
                MIXED[b] = (byte) i2;
            }
            i2++;
        }
        Arrays.fill(PUNCTUATION, (byte) -1);
        while (true) {
            byte[] bArr3 = TEXT_PUNCTUATION_RAW;
            if (i < bArr3.length) {
                byte b2 = bArr3[i];
                if (b2 > 0) {
                    PUNCTUATION[b2] = (byte) i;
                }
                i++;
            } else {
                return;
            }
        }
    }

    private PDF417HighLevelEncoder() {
    }

    static String encodeHighLevel(String msg, Compaction compaction, Charset encoding) throws WriterException {
        String str = msg;
        Charset encoding2 = encoding;
        StringBuilder sb = new StringBuilder(msg.length());
        if (encoding2 == null) {
            encoding2 = DEFAULT_ENCODING;
        } else if (!DEFAULT_ENCODING.equals(encoding2)) {
            CharacterSetECI characterSetECIByName = CharacterSetECI.getCharacterSetECIByName(encoding.name());
            CharacterSetECI eci = characterSetECIByName;
            if (characterSetECIByName != null) {
                encodingECI(eci.getValue(), sb);
            }
        }
        int len = msg.length();
        int p = 0;
        int textSubMode = 0;
        switch (AnonymousClass1.$SwitchMap$com$google$zxing$pdf417$encoder$Compaction[compaction.ordinal()]) {
            case 1:
                encodeText(str, 0, len, sb, 0);
                break;
            case 2:
                byte[] msgBytes = str.getBytes(encoding2);
                encodeBinary(msgBytes, 0, msgBytes.length, 1, sb);
                break;
            case 3:
                sb.append(902);
                encodeNumeric(str, 0, len, sb);
                break;
            default:
                int encodingMode = 0;
                while (p < len) {
                    int determineConsecutiveDigitCount = determineConsecutiveDigitCount(str, p);
                    int n = determineConsecutiveDigitCount;
                    if (determineConsecutiveDigitCount >= 13) {
                        sb.append(902);
                        encodingMode = 2;
                        textSubMode = 0;
                        encodeNumeric(str, p, n, sb);
                        p += n;
                    } else {
                        int determineConsecutiveTextCount = determineConsecutiveTextCount(str, p);
                        int t = determineConsecutiveTextCount;
                        if (determineConsecutiveTextCount >= 5 || n == len) {
                            if (encodingMode != 0) {
                                sb.append(900);
                                encodingMode = 0;
                                textSubMode = 0;
                            }
                            textSubMode = encodeText(str, p, t, sb, textSubMode);
                            p += t;
                        } else {
                            int determineConsecutiveBinaryCount = determineConsecutiveBinaryCount(str, p, encoding2);
                            int b = determineConsecutiveBinaryCount;
                            if (determineConsecutiveBinaryCount == 0) {
                                b = 1;
                            }
                            byte[] bytes = str.substring(p, p + b).getBytes(encoding2);
                            byte[] bytes2 = bytes;
                            if (bytes.length == 1 && encodingMode == 0) {
                                encodeBinary(bytes2, 0, 1, 0, sb);
                            } else {
                                encodeBinary(bytes2, 0, bytes2.length, encodingMode, sb);
                                encodingMode = 1;
                                textSubMode = 0;
                            }
                            p += b;
                        }
                    }
                }
                break;
        }
        return sb.toString();
    }

    /* renamed from: com.google.zxing.pdf417.encoder.PDF417HighLevelEncoder$1  reason: invalid class name */
    static /* synthetic */ class AnonymousClass1 {
        static final /* synthetic */ int[] $SwitchMap$com$google$zxing$pdf417$encoder$Compaction;

        static {
            int[] iArr = new int[Compaction.values().length];
            $SwitchMap$com$google$zxing$pdf417$encoder$Compaction = iArr;
            try {
                iArr[Compaction.TEXT.ordinal()] = 1;
            } catch (NoSuchFieldError e) {
            }
            try {
                $SwitchMap$com$google$zxing$pdf417$encoder$Compaction[Compaction.BYTE.ordinal()] = 2;
            } catch (NoSuchFieldError e2) {
            }
            try {
                $SwitchMap$com$google$zxing$pdf417$encoder$Compaction[Compaction.NUMERIC.ordinal()] = 3;
            } catch (NoSuchFieldError e3) {
            }
        }
    }

    /* JADX WARNING: Can't fix incorrect switch cases order */
    /* Code decompiled incorrectly, please refer to instructions dump. */
    private static int encodeText(java.lang.CharSequence r16, int r17, int r18, java.lang.StringBuilder r19, int r20) {
        /*
            r0 = r16
            r1 = r18
            r2 = r19
            java.lang.StringBuilder r3 = new java.lang.StringBuilder
            r3.<init>(r1)
            r4 = 2
            r5 = 1
            r6 = 0
            r7 = r20
            r8 = 0
        L_0x0012:
            int r9 = r17 + r8
            char r10 = r0.charAt(r9)
            r11 = 26
            r12 = 32
            r13 = 28
            r14 = 27
            r15 = 29
            switch(r7) {
                case 0: goto L_0x00bb;
                case 1: goto L_0x007f;
                case 2: goto L_0x0035;
                default: goto L_0x0025;
            }
        L_0x0025:
            boolean r9 = isPunctuation(r10)
            if (r9 == 0) goto L_0x0128
            byte[] r9 = PUNCTUATION
            byte r9 = r9[r10]
            char r9 = (char) r9
            r3.append(r9)
            goto L_0x00f4
        L_0x0035:
            boolean r11 = isMixed(r10)
            if (r11 == 0) goto L_0x0045
            byte[] r9 = MIXED
            byte r9 = r9[r10]
            char r9 = (char) r9
            r3.append(r9)
            goto L_0x00f4
        L_0x0045:
            boolean r11 = isAlphaUpper(r10)
            if (r11 == 0) goto L_0x0051
            r3.append(r13)
            r7 = 0
            goto L_0x0012
        L_0x0051:
            boolean r11 = isAlphaLower(r10)
            if (r11 == 0) goto L_0x005d
            r3.append(r14)
            r7 = 1
            goto L_0x0012
        L_0x005d:
            int r9 = r9 + 1
            if (r9 >= r1) goto L_0x0072
            char r9 = r0.charAt(r9)
            boolean r9 = isPunctuation(r9)
            if (r9 == 0) goto L_0x0072
            r7 = 3
            r9 = 25
            r3.append(r9)
            goto L_0x0012
        L_0x0072:
            r3.append(r15)
            byte[] r9 = PUNCTUATION
            byte r9 = r9[r10]
            char r9 = (char) r9
            r3.append(r9)
            goto L_0x00f4
        L_0x007f:
            boolean r9 = isAlphaLower(r10)
            if (r9 == 0) goto L_0x0092
            if (r10 != r12) goto L_0x008b
            r3.append(r11)
            goto L_0x00f4
        L_0x008b:
            int r10 = r10 + -97
            char r9 = (char) r10
            r3.append(r9)
            goto L_0x00f4
        L_0x0092:
            boolean r9 = isAlphaUpper(r10)
            if (r9 == 0) goto L_0x00a2
            r3.append(r14)
            int r10 = r10 + -65
            char r9 = (char) r10
            r3.append(r9)
            goto L_0x00f4
        L_0x00a2:
            boolean r9 = isMixed(r10)
            if (r9 == 0) goto L_0x00af
            r3.append(r13)
            r7 = 2
            goto L_0x0012
        L_0x00af:
            r3.append(r15)
            byte[] r9 = PUNCTUATION
            byte r9 = r9[r10]
            char r9 = (char) r9
            r3.append(r9)
            goto L_0x00f4
        L_0x00bb:
            boolean r9 = isAlphaUpper(r10)
            if (r9 == 0) goto L_0x00ce
            if (r10 != r12) goto L_0x00c7
            r3.append(r11)
            goto L_0x00f4
        L_0x00c7:
            int r10 = r10 + -65
            char r9 = (char) r10
            r3.append(r9)
            goto L_0x00f4
        L_0x00ce:
            boolean r9 = isAlphaLower(r10)
            if (r9 == 0) goto L_0x00db
            r3.append(r14)
            r7 = 1
            goto L_0x0012
        L_0x00db:
            boolean r9 = isMixed(r10)
            if (r9 == 0) goto L_0x00e8
            r3.append(r13)
            r7 = 2
            goto L_0x0012
        L_0x00e8:
            r3.append(r15)
            byte[] r9 = PUNCTUATION
            byte r9 = r9[r10]
            char r9 = (char) r9
            r3.append(r9)
        L_0x00f4:
            int r8 = r8 + 1
            if (r8 < r1) goto L_0x0012
            int r0 = r3.length()
            r1 = 0
            r8 = 0
        L_0x00ff:
            if (r1 >= r0) goto L_0x011d
            int r9 = r1 % 2
            if (r9 == 0) goto L_0x0107
            r9 = 1
            goto L_0x0108
        L_0x0107:
            r9 = 0
        L_0x0108:
            if (r9 == 0) goto L_0x0116
            int r8 = r8 * 30
            char r9 = r3.charAt(r1)
            int r8 = r8 + r9
            char r8 = (char) r8
            r2.append(r8)
            goto L_0x011a
        L_0x0116:
            char r8 = r3.charAt(r1)
        L_0x011a:
            int r1 = r1 + 1
            goto L_0x00ff
        L_0x011d:
            int r0 = r0 % r4
            if (r0 == 0) goto L_0x0127
            int r8 = r8 * 30
            int r8 = r8 + r15
            char r0 = (char) r8
            r2.append(r0)
        L_0x0127:
            return r7
        L_0x0128:
            r3.append(r15)
            r7 = 0
            goto L_0x0012
        */
        throw new UnsupportedOperationException("Method not decompiled: com.google.zxing.pdf417.encoder.PDF417HighLevelEncoder.encodeText(java.lang.CharSequence, int, int, java.lang.StringBuilder, int):int");
    }

    private static void encodeBinary(byte[] bytes, int startpos, int count, int startmode, StringBuilder sb) {
        if (count == 1 && startmode == 0) {
            sb.append(913);
        } else if (count % 6 == 0) {
            sb.append(924);
        } else {
            sb.append(901);
        }
        int idx = startpos;
        if (count >= 6) {
            char[] chars = new char[5];
            while ((startpos + count) - idx >= 6) {
                long t = 0;
                for (int i = 0; i < 6; i++) {
                    t = (t << 8) + ((long) (bytes[idx + i] & UByte.MAX_VALUE));
                }
                for (int i2 = 0; i2 < 5; i2++) {
                    chars[i2] = (char) ((int) (t % 900));
                    t /= 900;
                }
                for (int i3 = 4; i3 >= 0; i3--) {
                    sb.append(chars[i3]);
                }
                idx += 6;
            }
        }
        for (int i4 = idx; i4 < startpos + count; i4++) {
            sb.append((char) (bytes[i4] & 255));
        }
    }

    private static void encodeNumeric(String msg, int startpos, int count, StringBuilder sb) {
        BigInteger divide;
        int idx = 0;
        StringBuilder tmp = new StringBuilder((count / 3) + 1);
        BigInteger num900 = BigInteger.valueOf(900);
        BigInteger num0 = BigInteger.valueOf(0);
        while (idx < count) {
            tmp.setLength(0);
            int len = Math.min(44, count - idx);
            BigInteger bigint = new BigInteger("1" + msg.substring(startpos + idx, startpos + idx + len));
            do {
                tmp.append((char) bigint.mod(num900).intValue());
                divide = bigint.divide(num900);
                bigint = divide;
            } while (!divide.equals(num0));
            for (int i = tmp.length() - 1; i >= 0; i--) {
                sb.append(tmp.charAt(i));
            }
            idx += len;
        }
    }

    private static boolean isDigit(char ch) {
        return ch >= '0' && ch <= '9';
    }

    private static boolean isAlphaUpper(char ch) {
        if (ch != ' ') {
            return ch >= 'A' && ch <= 'Z';
        }
        return true;
    }

    private static boolean isAlphaLower(char ch) {
        if (ch != ' ') {
            return ch >= 'a' && ch <= 'z';
        }
        return true;
    }

    private static boolean isMixed(char ch) {
        return MIXED[ch] != -1;
    }

    private static boolean isPunctuation(char ch) {
        return PUNCTUATION[ch] != -1;
    }

    private static boolean isText(char ch) {
        if (ch == 9 || ch == 10 || ch == 13) {
            return true;
        }
        return ch >= ' ' && ch <= '~';
    }

    private static int determineConsecutiveDigitCount(CharSequence msg, int startpos) {
        int count = 0;
        int len = msg.length();
        int idx = startpos;
        if (startpos < len) {
            char ch = msg.charAt(startpos);
            while (isDigit(ch) && idx < len) {
                count++;
                idx++;
                if (idx < len) {
                    ch = msg.charAt(idx);
                }
            }
        }
        return count;
    }

    private static int determineConsecutiveTextCount(CharSequence msg, int startpos) {
        int len = msg.length();
        int idx = startpos;
        while (idx < len) {
            char ch = msg.charAt(idx);
            int numericCount = 0;
            while (numericCount < 13 && isDigit(ch) && idx < len) {
                numericCount++;
                idx++;
                if (idx < len) {
                    ch = msg.charAt(idx);
                }
            }
            if (numericCount < 13) {
                if (numericCount <= 0) {
                    if (!isText(msg.charAt(idx))) {
                        break;
                    }
                    idx++;
                }
            } else {
                return (idx - startpos) - numericCount;
            }
        }
        return idx - startpos;
    }

    private static int determineConsecutiveBinaryCount(String msg, int startpos, Charset encoding) throws WriterException {
        CharsetEncoder encoder = encoding.newEncoder();
        int len = msg.length();
        int idx = startpos;
        while (idx < len) {
            char ch = msg.charAt(idx);
            int numericCount = 0;
            while (numericCount < 13 && isDigit(ch)) {
                numericCount++;
                int i = idx + numericCount;
                int i2 = i;
                if (i >= len) {
                    break;
                }
                ch = msg.charAt(i2);
            }
            if (numericCount >= 13) {
                return idx - startpos;
            }
            char ch2 = msg.charAt(idx);
            if (encoder.canEncode(ch2)) {
                idx++;
            } else {
                throw new WriterException("Non-encodable character detected: " + ch2 + " (Unicode: " + ch2 + ')');
            }
        }
        return idx - startpos;
    }

    private static void encodingECI(int eci, StringBuilder sb) throws WriterException {
        if (eci >= 0 && eci < LATCH_TO_TEXT) {
            sb.append(927);
            sb.append((char) eci);
        } else if (eci < 810900) {
            sb.append(926);
            sb.append((char) ((eci / LATCH_TO_TEXT) - 1));
            sb.append((char) (eci % LATCH_TO_TEXT));
        } else if (eci < 811800) {
            sb.append(925);
            sb.append((char) (810900 - eci));
        } else {
            throw new WriterException("ECI number not in valid range from 0..811799, but was " + eci);
        }
    }
}
