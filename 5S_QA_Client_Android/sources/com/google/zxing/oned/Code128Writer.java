package com.google.zxing.oned;

import com.google.zxing.BarcodeFormat;
import com.google.zxing.EncodeHintType;
import com.google.zxing.WriterException;
import com.google.zxing.common.BitMatrix;
import java.util.ArrayList;
import java.util.Map;

public final class Code128Writer extends OneDimensionalCodeWriter {
    private static final int CODE_CODE_A = 101;
    private static final int CODE_CODE_B = 100;
    private static final int CODE_CODE_C = 99;
    private static final int CODE_FNC_1 = 102;
    private static final int CODE_FNC_2 = 97;
    private static final int CODE_FNC_3 = 96;
    private static final int CODE_FNC_4_A = 101;
    private static final int CODE_FNC_4_B = 100;
    private static final int CODE_START_A = 103;
    private static final int CODE_START_B = 104;
    private static final int CODE_START_C = 105;
    private static final int CODE_STOP = 106;
    private static final char ESCAPE_FNC_1 = 'ñ';
    private static final char ESCAPE_FNC_2 = 'ò';
    private static final char ESCAPE_FNC_3 = 'ó';
    private static final char ESCAPE_FNC_4 = 'ô';

    private enum CType {
        UNCODABLE,
        ONE_DIGIT,
        TWO_DIGITS,
        FNC_1
    }

    public BitMatrix encode(String contents, BarcodeFormat format, int width, int height, Map<EncodeHintType, ?> hints) throws WriterException {
        if (format == BarcodeFormat.CODE_128) {
            return super.encode(contents, format, width, height, hints);
        }
        throw new IllegalArgumentException("Can only encode CODE_128, but got " + format);
    }

    public boolean[] encode(String str) {
        int i;
        int length = str.length();
        if (length <= 0 || length > 80) {
            throw new IllegalArgumentException("Contents length should be between 1 and 80 characters, but got " + length);
        }
        int i2 = 0;
        for (int i3 = 0; i3 < length; i3++) {
            char charAt = str.charAt(i3);
            switch (charAt) {
                case 241:
                case 242:
                case 243:
                case 244:
                    break;
                default:
                    if (charAt <= 127) {
                        break;
                    } else {
                        throw new IllegalArgumentException("Bad character in input: " + charAt);
                    }
            }
        }
        ArrayList<int[]> arrayList = new ArrayList<>();
        int i4 = 0;
        int i5 = 0;
        int i6 = 0;
        int i7 = 1;
        while (true) {
            int i8 = 103;
            if (i4 < length) {
                int chooseCode = chooseCode(str, i4, i6);
                if (chooseCode == i6) {
                    i = 101;
                    switch (str.charAt(i4)) {
                        case 241:
                            i = 102;
                            break;
                        case 242:
                            i = 97;
                            break;
                        case 243:
                            i = 96;
                            break;
                        case 244:
                            if (i6 != 101) {
                                i = 100;
                                break;
                            }
                            break;
                        default:
                            switch (i6) {
                                case 100:
                                    i = str.charAt(i4) - ' ';
                                    break;
                                case 101:
                                    i = str.charAt(i4) - ' ';
                                    if (i < 0) {
                                        i += 96;
                                        break;
                                    }
                                    break;
                                default:
                                    i = Integer.parseInt(str.substring(i4, i4 + 2));
                                    i4++;
                                    break;
                            }
                    }
                    i4++;
                } else {
                    if (i6 == 0) {
                        switch (chooseCode) {
                            case 100:
                                i8 = 104;
                                break;
                            case 101:
                                break;
                            default:
                                i8 = 105;
                                break;
                        }
                    } else {
                        i8 = chooseCode;
                    }
                    i6 = chooseCode;
                    i = i8;
                }
                arrayList.add(Code128Reader.CODE_PATTERNS[i]);
                i5 += i * i7;
                if (i4 != 0) {
                    i7++;
                }
            } else {
                arrayList.add(Code128Reader.CODE_PATTERNS[i5 % 103]);
                arrayList.add(Code128Reader.CODE_PATTERNS[106]);
                int i9 = 0;
                for (int[] iArr : arrayList) {
                    for (int i10 : (int[]) r11.next()) {
                        i9 += i10;
                    }
                }
                boolean[] zArr = new boolean[i9];
                for (int[] appendPattern : arrayList) {
                    i2 += appendPattern(zArr, i2, appendPattern, true);
                }
                return zArr;
            }
        }
    }

    private static CType findCType(CharSequence value, int start) {
        int last = value.length();
        if (start >= last) {
            return CType.UNCODABLE;
        }
        char charAt = value.charAt(start);
        char c = charAt;
        if (charAt == 241) {
            return CType.FNC_1;
        }
        if (c < '0' || c > '9') {
            return CType.UNCODABLE;
        }
        if (start + 1 >= last) {
            return CType.ONE_DIGIT;
        }
        char charAt2 = value.charAt(start + 1);
        char c2 = charAt2;
        if (charAt2 < '0' || c2 > '9') {
            return CType.ONE_DIGIT;
        }
        return CType.TWO_DIGITS;
    }

    private static int chooseCode(CharSequence value, int start, int oldCode) {
        CType lookahead;
        CType findCType = findCType(value, start);
        CType lookahead2 = findCType;
        if (findCType == CType.ONE_DIGIT) {
            return 100;
        }
        if (lookahead2 == CType.UNCODABLE) {
            if (start < value.length()) {
                char charAt = value.charAt(start);
                char c = charAt;
                if (charAt < ' ' || (oldCode == 101 && c < '`')) {
                    return 101;
                }
            }
            return 100;
        } else if (oldCode == 99) {
            return 99;
        } else {
            if (oldCode != 100) {
                if (lookahead2 == CType.FNC_1) {
                    lookahead2 = findCType(value, start + 1);
                }
                if (lookahead2 == CType.TWO_DIGITS) {
                    return 99;
                }
                return 100;
            } else if (lookahead2 == CType.FNC_1) {
                return 100;
            } else {
                CType findCType2 = findCType(value, start + 2);
                CType lookahead3 = findCType2;
                if (findCType2 == CType.UNCODABLE || lookahead3 == CType.ONE_DIGIT) {
                    return 100;
                }
                if (lookahead3 != CType.FNC_1) {
                    int index = start + 4;
                    while (true) {
                        CType findCType3 = findCType(value, index);
                        lookahead = findCType3;
                        if (findCType3 != CType.TWO_DIGITS) {
                            break;
                        }
                        index += 2;
                    }
                    if (lookahead == CType.ONE_DIGIT) {
                        return 100;
                    }
                    return 99;
                } else if (findCType(value, start + 3) == CType.TWO_DIGITS) {
                    return 99;
                } else {
                    return 100;
                }
            }
        }
    }
}
