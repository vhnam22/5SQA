package com.exampanle.ky_thuat.manualclient.apiclient.api;

import android.content.Context;
import android.os.AsyncTask;
import android.widget.Toast;
import com.exampanle.ky_thuat.manualclient.activity.MainActivity;
import com.exampanle.ky_thuat.manualclient.apiclient.JSON;
import com.exampanle.ky_thuat.manualclient.apiclient.model.RequestViewModel;
import com.exampanle.ky_thuat.manualclient.apiclient.model.ResponseDto;
import java.io.IOException;
import okhttp3.MediaType;
import okhttp3.OkHttpClient;
import okhttp3.Request;
import okhttp3.RequestBody;
import okhttp3.Response;

public class apiSetActiveRequestAsync extends AsyncTask<String, String, RequestViewModel> {
    private Context context;
    private JSON json;
    private Object obj;
    private OkHttpClient okHttpClient = new OkHttpClient.Builder().build();
    String result;
    private String url;

    public apiSetActiveRequestAsync(Context context2, Object obj2) {
        this.obj = obj2;
        this.json = new JSON();
        this.context = context2;
        this.url = "/api/Request/Active";
    }

    /* access modifiers changed from: protected */
    public RequestViewModel doInBackground(String... strings) {
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
        if (responseDto.getData() != null) {
            return (RequestViewModel) this.json.deserialize(this.json.serialize(responseDto.getData()), RequestViewModel.class);
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
