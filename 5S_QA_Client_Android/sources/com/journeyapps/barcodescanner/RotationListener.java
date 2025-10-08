package com.journeyapps.barcodescanner;

import android.content.Context;
import android.view.OrientationEventListener;
import android.view.WindowManager;

public class RotationListener {
    /* access modifiers changed from: private */
    public RotationCallback callback;
    /* access modifiers changed from: private */
    public int lastRotation;
    private OrientationEventListener orientationEventListener;
    /* access modifiers changed from: private */
    public WindowManager windowManager;

    public void listen(Context context, RotationCallback callback2) {
        stop();
        Context context2 = context.getApplicationContext();
        this.callback = callback2;
        this.windowManager = (WindowManager) context2.getSystemService("window");
        AnonymousClass1 r0 = new OrientationEventListener(context2, 3) {
            public void onOrientationChanged(int orientation) {
                int newRotation;
                WindowManager localWindowManager = RotationListener.this.windowManager;
                RotationCallback localCallback = RotationListener.this.callback;
                if (RotationListener.this.windowManager != null && localCallback != null && (newRotation = localWindowManager.getDefaultDisplay().getRotation()) != RotationListener.this.lastRotation) {
                    int unused = RotationListener.this.lastRotation = newRotation;
                    localCallback.onRotationChanged(newRotation);
                }
            }
        };
        this.orientationEventListener = r0;
        r0.enable();
        this.lastRotation = this.windowManager.getDefaultDisplay().getRotation();
    }

    public void stop() {
        OrientationEventListener orientationEventListener2 = this.orientationEventListener;
        if (orientationEventListener2 != null) {
            orientationEventListener2.disable();
        }
        this.orientationEventListener = null;
        this.windowManager = null;
        this.callback = null;
    }
}
