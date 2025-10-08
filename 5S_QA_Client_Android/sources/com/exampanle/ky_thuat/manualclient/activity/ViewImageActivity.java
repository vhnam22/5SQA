package com.exampanle.ky_thuat.manualclient.activity;

import android.graphics.BitmapFactory;
import android.os.Bundle;
import androidx.appcompat.app.AppCompatActivity;
import com.exampanle.ky_thuat.manualclient.R;
import com.exampanle.ky_thuat.manualclient.apiclient.api.apiGetImageAsync;
import com.github.chrisbanes.photoview.PhotoView;
import java.util.concurrent.ExecutionException;

public class ViewImageActivity extends AppCompatActivity {
    /* access modifiers changed from: protected */
    public void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView((int) R.layout.activity_view_image);
        PhotoView image = (PhotoView) findViewById(R.id.imgProductImageZoom);
        String uri = getIntent().getStringExtra("IMAGE");
        try {
            byte[] result = (byte[]) new apiGetImageAsync().execute(new String[]{uri}).get();
            if (result != null && result.length > 0) {
                image.setImageBitmap(BitmapFactory.decodeByteArray(result, 0, result.length));
            }
        } catch (ExecutionException e) {
            e.printStackTrace();
        } catch (InterruptedException e2) {
            e2.printStackTrace();
        }
    }

    /* access modifiers changed from: protected */
    public void onResume() {
        super.onResume();
        ActivityManager.setCurrentActivity(this);
    }

    public void onBackPressed() {
        setResult(-1);
        super.onBackPressed();
    }
}
