package com.exampanle.ky_thuat.manualclient.apiclient.api;

import android.content.Context;
import android.os.AsyncTask;
import android.widget.Toast;
import com.exampanle.ky_thuat.manualclient.activity.MainActivity;
import com.exampanle.ky_thuat.manualclient.apiclient.JSON;
import com.exampanle.ky_thuat.manualclient.apiclient.model.RequestDetailViewModel;
import com.exampanle.ky_thuat.manualclient.apiclient.model.ResponseDto;
import com.google.gson.reflect.TypeToken;
import java.io.IOException;
import java.util.List;
import java.util.UUID;
import okhttp3.OkHttpClient;
import okhttp3.Request;
import okhttp3.Response;

public class apiGetRequestDetailAsync extends AsyncTask<String, String, List<RequestDetailViewModel>> {
    private Context context;
    private JSON json = new JSON();
    private OkHttpClient okHttpClient = new OkHttpClient.Builder().build();
    String result;
    private String url;

    public apiGetRequestDetailAsync(Context context2, UUID id, int sample) {
        this.context = context2;
        this.url = "/api/RequestDetail/Gets/" + id + "/" + sample;
    }

    /* access modifiers changed from: protected */
    public List<RequestDetailViewModel> doInBackground(String... strings) {
        try {
            Response response = this.okHttpClient.newCall(new Request.Builder().url(strings[0] + this.url).addHeader("Accept", "text/plain").addHeader("Authorization", "Bearer " + MainActivity.TOKEN).get().build()).execute();
            if (response.code() == 200) {
                this.result = response.body().string();
            } else {
                this.result = "ERROR: " + response.message();
            }
        } catch (IOException e) {
            e.printStackTrace();
            this.result = "ERROR: " + e.getMessage();
        }
        if (this.result.contains("ERROR:")) {
            publishProgress(new String[]{this.result});
            return null;
        }
        ResponseDto responseDto = (ResponseDto) this.json.deserialize(this.result, ResponseDto.class);
        if (responseDto.isSuccess().booleanValue()) {
            return (List) this.json.deserialize(this.json.serialize(responseDto.getData()), new TypeToken<List<RequestDetailViewModel>>() {
            }.getType());
        }
        publishProgress(new String[]{responseDto.getMessages().get(0).getMessage()});
        return null;
    }

    /* access modifiers changed from: protected */
    public void onProgressUpdate(String... values) {
        super.onProgressUpdate(values);
        Toast.makeText(this.context, values[0], 1).show();
    }
}
