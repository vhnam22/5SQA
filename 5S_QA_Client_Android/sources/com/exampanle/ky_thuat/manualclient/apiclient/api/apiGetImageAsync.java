package com.exampanle.ky_thuat.manualclient.apiclient.api;

import android.os.AsyncTask;
import java.io.IOException;
import okhttp3.OkHttpClient;
import okhttp3.Request;

public class apiGetImageAsync extends AsyncTask<String, Void, byte[]> {
    OkHttpClient okHttpClient = new OkHttpClient.Builder().build();

    /* access modifiers changed from: protected */
    public byte[] doInBackground(String... strings) {
        Request.Builder builder = new Request.Builder();
        builder.url(strings[0]);
        try {
            return this.okHttpClient.newCall(builder.build()).execute().body().bytes();
        } catch (IOException e) {
            e.printStackTrace();
            return null;
        }
    }
}
