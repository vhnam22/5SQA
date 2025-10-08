package com.exampanle.ky_thuat.manualclient.activity;

import android.app.Activity;
import java.lang.ref.WeakReference;

public class ActivityManager {
    private static WeakReference<Activity> currentActivityRef;

    public static void setCurrentActivity(Activity activity) {
        currentActivityRef = new WeakReference<>(activity);
    }

    public static Activity getCurrentActivity() {
        WeakReference<Activity> weakReference = currentActivityRef;
        if (weakReference != null) {
            return (Activity) weakReference.get();
        }
        return null;
    }
}
