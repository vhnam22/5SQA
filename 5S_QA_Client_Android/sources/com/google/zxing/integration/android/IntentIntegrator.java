package com.google.zxing.integration.android;

import android.app.Activity;
import android.app.Fragment;
import android.content.Intent;
import android.os.Bundle;
import com.google.zxing.client.android.Intents;
import com.journeyapps.barcodescanner.CaptureActivity;
import java.util.Arrays;
import java.util.Collection;
import java.util.Collections;
import java.util.HashMap;
import java.util.List;
import java.util.Map;

public class IntentIntegrator {
    public static final Collection<String> ALL_CODE_TYPES = null;
    public static final String CODE_128 = "CODE_128";
    public static final String CODE_39 = "CODE_39";
    public static final String CODE_93 = "CODE_93";
    public static final String DATA_MATRIX = "DATA_MATRIX";
    public static final String EAN_13 = "EAN_13";
    public static final String EAN_8 = "EAN_8";
    public static final String ITF = "ITF";
    public static final Collection<String> ONE_D_CODE_TYPES = list(UPC_A, UPC_E, EAN_8, EAN_13, RSS_14, CODE_39, CODE_93, CODE_128, ITF, RSS_14, RSS_EXPANDED);
    public static final String PDF_417 = "PDF_417";
    public static final Collection<String> PRODUCT_CODE_TYPES = list(UPC_A, UPC_E, EAN_8, EAN_13, RSS_14);
    public static final String QR_CODE = "QR_CODE";
    public static final int REQUEST_CODE = 49374;
    public static final String RSS_14 = "RSS_14";
    public static final String RSS_EXPANDED = "RSS_EXPANDED";
    private static final String TAG = IntentIntegrator.class.getSimpleName();
    public static final String UPC_A = "UPC_A";
    public static final String UPC_E = "UPC_E";
    private final Activity activity;
    private Class<?> captureActivity;
    private Collection<String> desiredBarcodeFormats;
    private Fragment fragment;
    private final Map<String, Object> moreExtras = new HashMap(3);
    private int requestCode = REQUEST_CODE;
    private androidx.fragment.app.Fragment supportFragment;

    /* access modifiers changed from: protected */
    public Class<?> getDefaultCaptureActivity() {
        return CaptureActivity.class;
    }

    public IntentIntegrator(Activity activity2) {
        this.activity = activity2;
    }

    public Class<?> getCaptureActivity() {
        if (this.captureActivity == null) {
            this.captureActivity = getDefaultCaptureActivity();
        }
        return this.captureActivity;
    }

    public IntentIntegrator setCaptureActivity(Class<?> captureActivity2) {
        this.captureActivity = captureActivity2;
        return this;
    }

    public IntentIntegrator setRequestCode(int requestCode2) {
        if (requestCode2 <= 0 || requestCode2 > 65535) {
            throw new IllegalArgumentException("requestCode out of range");
        }
        this.requestCode = requestCode2;
        return this;
    }

    public static IntentIntegrator forSupportFragment(androidx.fragment.app.Fragment fragment2) {
        IntentIntegrator integrator = new IntentIntegrator(fragment2.getActivity());
        integrator.supportFragment = fragment2;
        return integrator;
    }

    public static IntentIntegrator forFragment(Fragment fragment2) {
        IntentIntegrator integrator = new IntentIntegrator(fragment2.getActivity());
        integrator.fragment = fragment2;
        return integrator;
    }

    public Map<String, ?> getMoreExtras() {
        return this.moreExtras;
    }

    public final IntentIntegrator addExtra(String key, Object value) {
        this.moreExtras.put(key, value);
        return this;
    }

    public final IntentIntegrator setPrompt(String prompt) {
        if (prompt != null) {
            addExtra(Intents.Scan.PROMPT_MESSAGE, prompt);
        }
        return this;
    }

    public IntentIntegrator setOrientationLocked(boolean locked) {
        addExtra(Intents.Scan.ORIENTATION_LOCKED, Boolean.valueOf(locked));
        return this;
    }

    public IntentIntegrator setCameraId(int cameraId) {
        if (cameraId >= 0) {
            addExtra(Intents.Scan.CAMERA_ID, Integer.valueOf(cameraId));
        }
        return this;
    }

    public IntentIntegrator setBeepEnabled(boolean enabled) {
        addExtra(Intents.Scan.BEEP_ENABLED, Boolean.valueOf(enabled));
        return this;
    }

    public IntentIntegrator setBarcodeImageEnabled(boolean enabled) {
        addExtra(Intents.Scan.BARCODE_IMAGE_ENABLED, Boolean.valueOf(enabled));
        return this;
    }

    public IntentIntegrator setDesiredBarcodeFormats(Collection<String> desiredBarcodeFormats2) {
        this.desiredBarcodeFormats = desiredBarcodeFormats2;
        return this;
    }

    public IntentIntegrator setDesiredBarcodeFormats(String... desiredBarcodeFormats2) {
        this.desiredBarcodeFormats = Arrays.asList(desiredBarcodeFormats2);
        return this;
    }

    public final void initiateScan() {
        startActivityForResult(createScanIntent(), this.requestCode);
    }

    public IntentIntegrator setTimeout(long timeout) {
        addExtra(Intents.Scan.TIMEOUT, Long.valueOf(timeout));
        return this;
    }

    public Intent createScanIntent() {
        Intent intentScan = new Intent(this.activity, getCaptureActivity());
        intentScan.setAction(Intents.Scan.ACTION);
        if (this.desiredBarcodeFormats != null) {
            StringBuilder joinedByComma = new StringBuilder();
            for (String format : this.desiredBarcodeFormats) {
                if (joinedByComma.length() > 0) {
                    joinedByComma.append(',');
                }
                joinedByComma.append(format);
            }
            intentScan.putExtra(Intents.Scan.FORMATS, joinedByComma.toString());
        }
        intentScan.addFlags(67108864);
        intentScan.addFlags(524288);
        attachMoreExtras(intentScan);
        return intentScan;
    }

    public final void initiateScan(Collection<String> desiredBarcodeFormats2) {
        setDesiredBarcodeFormats(desiredBarcodeFormats2);
        initiateScan();
    }

    /* access modifiers changed from: protected */
    public void startActivityForResult(Intent intent, int code) {
        Fragment fragment2 = this.fragment;
        if (fragment2 != null) {
            fragment2.startActivityForResult(intent, code);
            return;
        }
        androidx.fragment.app.Fragment fragment3 = this.supportFragment;
        if (fragment3 != null) {
            fragment3.startActivityForResult(intent, code);
        } else {
            this.activity.startActivityForResult(intent, code);
        }
    }

    /* access modifiers changed from: protected */
    public void startActivity(Intent intent) {
        Fragment fragment2 = this.fragment;
        if (fragment2 != null) {
            fragment2.startActivity(intent);
            return;
        }
        androidx.fragment.app.Fragment fragment3 = this.supportFragment;
        if (fragment3 != null) {
            fragment3.startActivity(intent);
        } else {
            this.activity.startActivity(intent);
        }
    }

    public static IntentResult parseActivityResult(int requestCode2, int resultCode, Intent intent) {
        if (requestCode2 == 49374) {
            return parseActivityResult(resultCode, intent);
        }
        return null;
    }

    public static IntentResult parseActivityResult(int resultCode, Intent intent) {
        if (resultCode != -1) {
            return new IntentResult();
        }
        String contents = intent.getStringExtra(Intents.Scan.RESULT);
        String formatName = intent.getStringExtra(Intents.Scan.RESULT_FORMAT);
        byte[] rawBytes = intent.getByteArrayExtra(Intents.Scan.RESULT_BYTES);
        int intentOrientation = intent.getIntExtra(Intents.Scan.RESULT_ORIENTATION, Integer.MIN_VALUE);
        return new IntentResult(contents, formatName, rawBytes, intentOrientation == Integer.MIN_VALUE ? null : Integer.valueOf(intentOrientation), intent.getStringExtra(Intents.Scan.RESULT_ERROR_CORRECTION_LEVEL), intent.getStringExtra(Intents.Scan.RESULT_BARCODE_IMAGE_PATH));
    }

    private static List<String> list(String... values) {
        return Collections.unmodifiableList(Arrays.asList(values));
    }

    private void attachMoreExtras(Intent intent) {
        for (Map.Entry<String, Object> entry : this.moreExtras.entrySet()) {
            String key = entry.getKey();
            Object value = entry.getValue();
            if (value instanceof Integer) {
                intent.putExtra(key, (Integer) value);
            } else if (value instanceof Long) {
                intent.putExtra(key, (Long) value);
            } else if (value instanceof Boolean) {
                intent.putExtra(key, (Boolean) value);
            } else if (value instanceof Double) {
                intent.putExtra(key, (Double) value);
            } else if (value instanceof Float) {
                intent.putExtra(key, (Float) value);
            } else if (value instanceof Bundle) {
                intent.putExtra(key, (Bundle) value);
            } else {
                intent.putExtra(key, value.toString());
            }
        }
    }
}
