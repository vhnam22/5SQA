package com.exampanle.ky_thuat.manualclient.adapter;

import android.content.BroadcastReceiver;
import android.content.Context;
import android.content.Intent;
import android.util.Log;
import com.exampanle.ky_thuat.manualclient.activity.MainActivity;

public class NetworkChangeReceiver extends BroadcastReceiver {
    public static final String ACTION_FIRST_ACTION = "android.net.conn.CONNECTIVITY_CHANGE";

    public void onReceive(Context context, Intent intent) {
        int status = NetworkUtil.getConnectivityStatusString(context);
        Log.e("Network status", "Start netword status");
        if (ACTION_FIRST_ACTION.equals(intent.getAction()) && status != 0) {
            MainActivity.webSocketClient.Connect(MainActivity.BASE_URL);
        }
    }
}
