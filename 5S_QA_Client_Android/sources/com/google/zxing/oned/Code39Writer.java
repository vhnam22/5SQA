package com.google.zxing.oned;

import com.google.zxing.BarcodeFormat;
import com.google.zxing.EncodeHintType;
import com.google.zxing.WriterException;
import com.google.zxing.common.BitMatrix;
import java.util.Map;
import kotlin.text.Typography;

public final class Code39Writer extends OneDimensionalCodeWriter {
    public BitMatrix encode(String contents, BarcodeFormat format, int width, int height, Map<EncodeHintType, ?> hints) throws WriterException {
        if (format == BarcodeFormat.CODE_39) {
            return super.encode(contents, format, width, height, hints);
        }
        throw new IllegalArgumentException("Can only encode CODE_39, but got " + format);
    }

    public boolean[] encode(String str) {
        int length = str.length();
        if (length <= 80) {
            int i = 0;
            while (true) {
                if (i >= length) {
                    break;
                } else if ("0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ-. $/+%".indexOf(str.charAt(i)) < 0) {
                    str = tryToConvertToExtendedMode(str);
                    length = str.length();
                    if (length > 80) {
                        throw new IllegalArgumentException("Requested contents should be less than 80 digits long, but got " + length + " (extended full ASCII mode)");
                    }
                } else {
                    i++;
                }
            }
            int[] iArr = new int[9];
            int i2 = length + 25;
            for (int i3 = 0; i3 < length; i3++) {
                toIntArray(Code39Reader.CHARACTER_ENCODINGS["0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ-. $/+%".indexOf(str.charAt(i3))], iArr);
                for (int i4 = 0; i4 < 9; i4++) {
                    i2 += iArr[i4];
                }
            }
            boolean[] zArr = new boolean[i2];
            toIntArray(148, iArr);
            int appendPattern = appendPattern(zArr, 0, iArr, true);
            int[] iArr2 = {1};
            int appendPattern2 = appendPattern + appendPattern(zArr, appendPattern, iArr2, false);
            for (int i5 = 0; i5 < length; i5++) {
                toIntArray(Code39Reader.CHARACTER_ENCODINGS["0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ-. $/+%".indexOf(str.charAt(i5))], iArr);
                int appendPattern3 = appendPattern2 + appendPattern(zArr, appendPattern2, iArr, true);
                appendPattern2 = appendPattern3 + appendPattern(zArr, appendPattern3, iArr2, false);
            }
            toIntArray(148, iArr);
            appendPattern(zArr, appendPattern2, iArr, true);
            return zArr;
        }
        throw new IllegalArgumentException("Requested contents should be less than 80 digits long, but got " + length);
    }

    private static void toIntArray(int a, int[] toReturn) {
        for (int i = 0; i < 9; i++) {
            int i2 = 1;
            if (((1 << (8 - i)) & a) != 0) {
                i2 = 2;
            }
            toReturn[i] = i2;
        }
    }

    private static String tryToConvertToExtendedMode(String contents) {
        int length = contents.length();
        StringBuilder extendedContent = new StringBuilder();
        for (int i = 0; i < length; i++) {
            char charAt = contents.charAt(i);
            char character = charAt;
            switch (charAt) {
                case 0:
                    extendedContent.append("%U");
                    break;
                case ' ':
                case '-':
                case '.':
                    extendedContent.append(character);
                    break;
                case '@':
                    extendedContent.append("%V");
                    break;
                case '`':
                    extendedContent.append("%W");
                    break;
                default:
                    if (character <= 0 || character >= 27) {
                        if (character <= 26 || character >= ' ') {
                            if ((character <= ' ' || character >= '-') && character != '/' && character != ':') {
                                if (character <= '/' || character >= ':') {
                                    if (character <= ':' || character >= '@') {
                                        if (character <= '@' || character >= '[') {
                                            if (character <= 'Z' || character >= '`') {
                                                if (character <= '`' || character >= '{') {
                                                    if (character > 'z' && character < 128) {
                                                        extendedContent.append('%');
                                                        extendedContent.append((char) ((character - '{') + 80));
                                                        break;
                                                    } else {
                                                        throw new IllegalArgumentException("Requested content contains a non-encodable character: '" + contents.charAt(i) + "'");
                                                    }
                                                } else {
                                                    extendedContent.append('+');
                                                    extendedContent.append((char) ((character - 'a') + 65));
                                                    break;
                                                }
                                            } else {
                                                extendedContent.append('%');
                                                extendedContent.append((char) ((character - '[') + 75));
                                                break;
                                            }
                                        } else {
                                            extendedContent.append((char) ((character - 'A') + 65));
                                            break;
                                        }
                                    } else {
                                        extendedContent.append('%');
                                        extendedContent.append((char) ((character - ';') + 70));
                                        break;
                                    }
                                } else {
                                    extendedContent.append((char) ((character - '0') + 48));
                                    break;
                                }
                            } else {
                                extendedContent.append('/');
                                extendedContent.append((char) ((character - '!') + 65));
                                break;
                            }
                        } else {
                            extendedContent.append('%');
                            extendedContent.append((char) ((character - 27) + 65));
                            break;
                        }
                    } else {
                        extendedContent.append(Typography.dollar);
                        extendedContent.append((char) ((character - 1) + 65));
                        break;
                    }
            }
        }
        return extendedContent.toString();
    }
}
