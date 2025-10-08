package com.journeyapps.barcodescanner;

import android.graphics.Rect;
import android.os.Bundle;
import android.os.Handler;
import android.os.HandlerThread;
import android.os.Message;
import android.util.Log;
import com.google.zxing.LuminanceSource;
import com.google.zxing.Result;
import com.google.zxing.client.android.R;
import com.journeyapps.barcodescanner.camera.CameraInstance;
import com.journeyapps.barcodescanner.camera.PreviewCallback;

public class DecoderThread {
    private static final String TAG = DecoderThread.class.getSimpleName();
    /* access modifiers changed from: private */
    public final Object LOCK = new Object();
    private final Handler.Callback callback = new Handler.Callback() {
        public boolean handleMessage(Message message) {
            if (message.what == R.id.zxing_decode) {
                DecoderThread.this.decode((SourceData) message.obj);
                return true;
            } else if (message.what != R.id.zxing_preview_failed) {
                return true;
            } else {
                DecoderThread.this.requestNextPreview();
                return true;
            }
        }
    };
    private CameraInstance cameraInstance;
    private Rect cropRect;
    private Decoder decoder;
    /* access modifiers changed from: private */
    public Handler handler;
    private final PreviewCallback previewCallback = new PreviewCallback() {
        public void onPreview(SourceData sourceData) {
            synchronized (DecoderThread.this.LOCK) {
                if (DecoderThread.this.running) {
                    DecoderThread.this.handler.obtainMessage(R.id.zxing_decode, sourceData).sendToTarget();
                }
            }
        }

        public void onPreviewError(Exception e) {
            synchronized (DecoderThread.this.LOCK) {
                if (DecoderThread.this.running) {
                    DecoderThread.this.handler.obtainMessage(R.id.zxing_preview_failed).sendToTarget();
                }
            }
        }
    };
    private Handler resultHandler;
    /* access modifiers changed from: private */
    public boolean running = false;
    private HandlerThread thread;

    public DecoderThread(CameraInstance cameraInstance2, Decoder decoder2, Handler resultHandler2) {
        Util.validateMainThread();
        this.cameraInstance = cameraInstance2;
        this.decoder = decoder2;
        this.resultHandler = resultHandler2;
    }

    public Decoder getDecoder() {
        return this.decoder;
    }

    public void setDecoder(Decoder decoder2) {
        this.decoder = decoder2;
    }

    public Rect getCropRect() {
        return this.cropRect;
    }

    public void setCropRect(Rect cropRect2) {
        this.cropRect = cropRect2;
    }

    public void start() {
        Util.validateMainThread();
        HandlerThread handlerThread = new HandlerThread(TAG);
        this.thread = handlerThread;
        handlerThread.start();
        this.handler = new Handler(this.thread.getLooper(), this.callback);
        this.running = true;
        requestNextPreview();
    }

    public void stop() {
        Util.validateMainThread();
        synchronized (this.LOCK) {
            this.running = false;
            this.handler.removeCallbacksAndMessages((Object) null);
            this.thread.quit();
        }
    }

    /* access modifiers changed from: private */
    public void requestNextPreview() {
        this.cameraInstance.requestPreview(this.previewCallback);
    }

    /* access modifiers changed from: protected */
    public LuminanceSource createSource(SourceData sourceData) {
        if (this.cropRect == null) {
            return null;
        }
        return sourceData.createSource();
    }

    /* access modifiers changed from: private */
    public void decode(SourceData sourceData) {
        long start = System.currentTimeMillis();
        Result rawResult = null;
        sourceData.setCropRect(this.cropRect);
        LuminanceSource source = createSource(sourceData);
        if (source != null) {
            rawResult = this.decoder.decode(source);
        }
        if (rawResult != null) {
            Log.d(TAG, "Found barcode in " + (System.currentTimeMillis() - start) + " ms");
            if (this.resultHandler != null) {
                Message message = Message.obtain(this.resultHandler, R.id.zxing_decode_succeeded, new BarcodeResult(rawResult, sourceData));
                message.setData(new Bundle());
                message.sendToTarget();
            }
        } else {
            Handler handler2 = this.resultHandler;
            if (handler2 != null) {
                Message.obtain(handler2, R.id.zxing_decode_failed).sendToTarget();
            }
        }
        if (this.resultHandler != null) {
            Message.obtain(this.resultHandler, R.id.zxing_possible_result_points, this.decoder.getPossibleResultPoints()).sendToTarget();
        }
        requestNextPreview();
    }
}
