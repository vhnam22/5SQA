package com.exampanle.ky_thuat.manualclient.adapter;

import android.content.Context;
import android.net.ConnectivityManager;
import android.net.NetworkInfo;

public class NetworkUtil {
    public static final int NETWORK_STATUS_MOBILE = 2;
    public static final int NETWORK_STATUS_NOT_CONNECTED = 0;
    public static final int NETWORK_STATUS_WIFI = 1;
    public static final int TYPE_MOBILE = 2;
    public static final int TYPE_NOT_CONNECTED = 0;
    public static final int TYPE_WIFI = 1;

    public static int getConnectivityStatus(Context context) {
        NetworkInfo activeNetwork = ((ConnectivityManager) context.getSystemService("connectivity")).getActiveNetworkInfo();
        if (activeNetwork == null) {
            return 0;
        }
        if (activeNetwork.getType() == 1) {
            return 1;
        }
        if (activeNetwork.getType() == 0) {
            return 2;
        }
        return 0;
    }

    public static int getConnectivityStatusString(Context context) {
        int conn = getConnectivityStatus(context);
        if (conn == 1) {
            return 1;
        }
        if (conn == 2) {
            return 2;
        }
        return 0;
    }
}
