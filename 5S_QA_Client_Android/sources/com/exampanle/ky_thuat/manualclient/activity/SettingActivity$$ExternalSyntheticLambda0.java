package com.exampanle.ky_thuat.manualclient.activity;

import com.exampanle.ky_thuat.manualclient.apiclient.model.ConfigDto;
import java.util.function.Predicate;

/* compiled from: D8$$SyntheticClass */
public final /* synthetic */ class SettingActivity$$ExternalSyntheticLambda0 implements Predicate {
    public final /* synthetic */ String f$0;

    public /* synthetic */ SettingActivity$$ExternalSyntheticLambda0(String str) {
        this.f$0 = str;
    }

    public final boolean test(Object obj) {
        return ((ConfigDto) obj).getName().equals(this.f$0);
    }
}
