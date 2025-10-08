package androidx.core.view;

import android.view.Menu;
import android.view.MenuInflater;
import android.view.MenuItem;
import com.android.tools.r8.annotations.SynthesizedClassV2;

public interface MenuProvider {
    void onCreateMenu(Menu menu, MenuInflater menuInflater);

    void onMenuClosed(Menu menu);

    boolean onMenuItemSelected(MenuItem menuItem);

    void onPrepareMenu(Menu menu);

    @SynthesizedClassV2(kind = 7, versionHash = "5e5398f0546d1d7afd62641edb14d82894f11ddc41bce363a0c8d0dac82c9c5a")
    /* renamed from: androidx.core.view.MenuProvider$-CC  reason: invalid class name */
    public final /* synthetic */ class CC {
        public static void $default$onPrepareMenu(MenuProvider _this, Menu menu) {
        }

        public static void $default$onMenuClosed(MenuProvider _this, Menu menu) {
        }
    }
}
