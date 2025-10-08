package androidx.lifecycle;

import com.android.tools.r8.annotations.SynthesizedClassV2;

public interface DefaultLifecycleObserver extends FullLifecycleObserver {
    void onCreate(LifecycleOwner lifecycleOwner);

    void onDestroy(LifecycleOwner lifecycleOwner);

    void onPause(LifecycleOwner lifecycleOwner);

    void onResume(LifecycleOwner lifecycleOwner);

    void onStart(LifecycleOwner lifecycleOwner);

    void onStop(LifecycleOwner lifecycleOwner);

    @SynthesizedClassV2(kind = 7, versionHash = "5e5398f0546d1d7afd62641edb14d82894f11ddc41bce363a0c8d0dac82c9c5a")
    /* renamed from: androidx.lifecycle.DefaultLifecycleObserver$-CC  reason: invalid class name */
    public final /* synthetic */ class CC {
        public static void $default$onCreate(DefaultLifecycleObserver _this, LifecycleOwner owner) {
        }

        public static void $default$onStart(DefaultLifecycleObserver _this, LifecycleOwner owner) {
        }

        public static void $default$onResume(DefaultLifecycleObserver _this, LifecycleOwner owner) {
        }

        public static void $default$onPause(DefaultLifecycleObserver _this, LifecycleOwner owner) {
        }

        public static void $default$onStop(DefaultLifecycleObserver _this, LifecycleOwner owner) {
        }

        public static void $default$onDestroy(DefaultLifecycleObserver _this, LifecycleOwner owner) {
        }
    }
}
