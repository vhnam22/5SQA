package androidx.lifecycle;

import androidx.lifecycle.ViewModelProvider;
import androidx.lifecycle.viewmodel.CreationExtras;
import com.android.tools.r8.annotations.SynthesizedClassV2;

public interface HasDefaultViewModelProviderFactory {
    CreationExtras getDefaultViewModelCreationExtras();

    ViewModelProvider.Factory getDefaultViewModelProviderFactory();

    @SynthesizedClassV2(kind = 7, versionHash = "5e5398f0546d1d7afd62641edb14d82894f11ddc41bce363a0c8d0dac82c9c5a")
    /* renamed from: androidx.lifecycle.HasDefaultViewModelProviderFactory$-CC  reason: invalid class name */
    public final /* synthetic */ class CC {
        public static CreationExtras $default$getDefaultViewModelCreationExtras(HasDefaultViewModelProviderFactory _this) {
            return CreationExtras.Empty.INSTANCE;
        }
    }
}
