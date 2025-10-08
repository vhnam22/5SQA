package com.exampanle.ky_thuat.manualclient.apiclient.api;

import android.content.Context;
import android.os.AsyncTask;
import android.widget.Toast;
import com.exampanle.ky_thuat.manualclient.activity.MainActivity;
import com.exampanle.ky_thuat.manualclient.apiclient.JSON;
import com.exampanle.ky_thuat.manualclient.apiclient.model.ResponseDto;
import java.io.IOException;
import java.util.List;
import java.util.UUID;
import okhttp3.MediaType;
import okhttp3.OkHttpClient;
import okhttp3.Request;
import okhttp3.RequestBody;
import okhttp3.Response;

public class apiGetResultSampleCompleteAsync extends AsyncTask<String, String, ResponseDto> {
    private Context context;
    private JSON json = new JSON();
    private List<String> obj;
    private OkHttpClient okHttpClient = new OkHttpClient.Builder().build();
    String result;
    private String url;

    public apiGetResultSampleCompleteAsync(Context context2, UUID id, List<String> obj2) {
        this.obj = obj2;
        this.context = context2;
        this.url = "/api/RequestResult/Gets/" + id;
    }

    /* access modifiers changed from: protected */
    public ResponseDto doInBackground(String... strings) {
        try {
            Response response = this.okHttpClient.newCall(new Request.Builder().url(strings[0] + this.url).addHeader("Accept", "text/plain").addHeader("Authorization", "Bearer " + MainActivity.TOKEN).post(RequestBody.create(MediaType.parse("application/json"), this.json.serialize(this.obj))).build()).execute();
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
            return responseDto;
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
