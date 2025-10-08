package com.journeyapps.barcodescanner.camera;

import android.hardware.Camera;
import android.os.Handler;
import android.os.Message;
import android.util.Log;
import java.util.ArrayList;
import java.util.Collection;
import kotlinx.coroutines.DebugKt;

public final class AutoFocusManager {
    private static final long AUTO_FOCUS_INTERVAL_MS = 2000;
    private static final Collection<String> FOCUS_MODES_CALLING_AF;
    private static final String TAG = AutoFocusManager.class.getSimpleName();
    /* access modifiers changed from: private */
    public int MESSAGE_FOCUS = 1;
    private final Camera.AutoFocusCallback autoFocusCallback;
    private final Camera camera;
    private final Handler.Callback focusHandlerCallback;
    /* access modifiers changed from: private */
    public boolean focusing;
    /* access modifiers changed from: private */
    public Handler handler;
    private boolean stopped;
    private final boolean useAutoFocus;

    static {
        ArrayList arrayList = new ArrayList(2);
        FOCUS_MODES_CALLING_AF = arrayList;
        arrayList.add(DebugKt.DEBUG_PROPERTY_VALUE_AUTO);
        arrayList.add("macro");
    }

    public AutoFocusManager(Camera camera2, CameraSettings settings) {
        boolean z = true;
        AnonymousClass1 r1 = new Handler.Callback() {
            public boolean handleMessage(Message msg) {
                if (msg.what != AutoFocusManager.this.MESSAGE_FOCUS) {
                    return false;
                }
                AutoFocusManager.this.focus();
                return true;
            }
        };
        this.focusHandlerCallback = r1;
        this.autoFocusCallback = new Camera.AutoFocusCallback() {
            public void onAutoFocus(boolean success, Camera theCamera) {
                AutoFocusManager.this.handler.post(new Runnable() {
                    public void run() {
                        boolean unused = AutoFocusManager.this.focusing = false;
                        AutoFocusManager.this.autoFocusAgainLater();
                    }
                });
            }
        };
        this.handler = new Handler(r1);
        this.camera = camera2;
        String currentFocusMode = camera2.getParameters().getFocusMode();
        z = (!settings.isAutoFocusEnabled() || !FOCUS_MODES_CALLING_AF.contains(currentFocusMode)) ? false : z;
        this.useAutoFocus = z;
        Log.i(TAG, "Current focus mode '" + currentFocusMode + "'; use auto focus? " + z);
        start();
    }

    /* access modifiers changed from: private */
    public synchronized void autoFocusAgainLater() {
        if (!this.stopped && !this.handler.hasMessages(this.MESSAGE_FOCUS)) {
            Handler handler2 = this.handler;
            handler2.sendMessageDelayed(handler2.obtainMessage(this.MESSAGE_FOCUS), AUTO_FOCUS_INTERVAL_MS);
        }
    }

    public void start() {
        this.stopped = false;
        focus();
    }

    /* access modifiers changed from: private */
    public void focus() {
        if (this.useAutoFocus && !this.stopped && !this.focusing) {
            try {
                this.camera.autoFocus(this.autoFocusCallback);
                this.focusing = true;
            } catch (RuntimeException re) {
                Log.w(TAG, "Unexpected exception while focusing", re);
                autoFocusAgainLater();
            }
        }
    }

    private void cancelOutstandingTask() {
        this.handler.removeMessages(this.MESSAGE_FOCUS);
    }

    public void stop() {
        this.stopped = true;
        this.focusing = false;
        cancelOutstandingTask();
        if (this.useAutoFocus) {
            try {
                this.camera.cancelAutoFocus();
            } catch (RuntimeException re) {
                Log.w(TAG, "Unexpected exception while cancelling focusing", re);
            }
        }
    }
}
