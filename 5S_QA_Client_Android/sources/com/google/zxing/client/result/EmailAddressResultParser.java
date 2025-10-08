package com.google.zxing.client.result;

import androidx.core.net.MailTo;
import com.google.zxing.Result;
import java.util.Map;
import java.util.regex.Pattern;

public final class EmailAddressResultParser extends ResultParser {
    private static final Pattern COMMA = Pattern.compile(",");

    public EmailAddressParsedResult parse(Result result) {
        String[] strArr;
        String str;
        String str2;
        String[] strArr2;
        String[] strArr3;
        String[] strArr4;
        String[] strArr5;
        String str3;
        String massagedText = getMassagedText(result);
        String[] strArr6 = null;
        if (massagedText.startsWith(MailTo.MAILTO_SCHEME) || massagedText.startsWith("MAILTO:")) {
            String substring = massagedText.substring(7);
            int indexOf = substring.indexOf(63);
            if (indexOf >= 0) {
                substring = substring.substring(0, indexOf);
            }
            try {
                String urlDecode = urlDecode(substring);
                if (!urlDecode.isEmpty()) {
                    strArr = COMMA.split(urlDecode);
                } else {
                    strArr = null;
                }
                Map<String, String> parseNameValuePairs = parseNameValuePairs(massagedText);
                if (parseNameValuePairs != null) {
                    if (strArr == null && (str3 = parseNameValuePairs.get("to")) != null) {
                        strArr = COMMA.split(str3);
                    }
                    String str4 = parseNameValuePairs.get("cc");
                    if (str4 != null) {
                        strArr5 = COMMA.split(str4);
                    } else {
                        strArr5 = null;
                    }
                    String str5 = parseNameValuePairs.get("bcc");
                    if (str5 != null) {
                        strArr6 = COMMA.split(str5);
                    }
                    str = parseNameValuePairs.get("body");
                    strArr4 = strArr;
                    strArr2 = strArr6;
                    strArr3 = strArr5;
                    str2 = parseNameValuePairs.get("subject");
                } else {
                    strArr4 = strArr;
                    strArr3 = null;
                    strArr2 = null;
                    str2 = null;
                    str = null;
                }
                return new EmailAddressParsedResult(strArr4, strArr3, strArr2, str2, str);
            } catch (IllegalArgumentException e) {
                return null;
            }
        } else if (!EmailDoCoMoResultParser.isBasicallyValidEmailAddress(massagedText)) {
            return null;
        } else {
            return new EmailAddressParsedResult(massagedText);
        }
    }
}
