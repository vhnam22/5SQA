package com.journeyapps.barcodescanner.camera;

import android.content.Context;
import android.os.Handler;
import android.util.Log;
import android.view.SurfaceHolder;
import com.google.zxing.client.android.R;
import com.journeyapps.barcodescanner.Size;
import com.journeyapps.barcodescanner.Util;

public class CameraInstance {
    /* access modifiers changed from: private */
    public static final String TAG = CameraInstance.class.getSimpleName();
    /* access modifiers changed from: private */
    public boolean cameraClosed = true;
    /* access modifiers changed from: private */
    public CameraManager cameraManager;
    private CameraSettings cameraSettings = new CameraSettings();
    /* access modifiers changed from: private */
    public CameraThread cameraThread;
    private Runnable closer = new Runnable() {
        public void run() {
            try {
                Log.d(CameraInstance.TAG, "Closing camera");
                CameraInstance.this.cameraManager.stopPreview();
                CameraInstance.this.cameraManager.close();
            } catch (Exception e) {
                Log.e(CameraInstance.TAG, "Failed to close camera", e);
            }
            boolean unused = CameraInstance.this.cameraClosed = true;
            CameraInstance.this.readyHandler.sendEmptyMessage(R.id.zxing_camera_closed);
            CameraInstance.this.cameraThread.decrementInstances();
        }
    };
    private Runnable configure = new Runnable() {
        public void run() {
            try {
                Log.d(CameraInstance.TAG, "Configuring camera");
                CameraInstance.this.cameraManager.configure();
                if (CameraInstance.this.readyHandler != null) {
                    CameraInstance.this.readyHandler.obtainMessage(R.id.zxing_prewiew_size_ready, CameraInstance.this.getPreviewSize()).sendToTarget();
                }
            } catch (Exception e) {
                CameraInstance.this.notifyError(e);
                Log.e(CameraInstance.TAG, "Failed to configure camera", e);
            }
        }
    };
    private DisplayConfiguration displayConfiguration;
    private Handler mainHandler;
    /* access modifiers changed from: private */
    public boolean open = false;
    private Runnable opener = new Runnable() {
        public void run() {
            try {
                Log.d(CameraInstance.TAG, "Opening camera");
                CameraInstance.this.cameraManager.open();
            } catch (Exception e) {
                CameraInstance.this.notifyError(e);
                Log.e(CameraInstance.TAG, "Failed to open camera", e);
            }
        }
    };
    private Runnable previewStarter = new Runnable() {
        public void run() {
            try {
                Log.d(CameraInstance.TAG, "Starting preview");
                CameraInstance.this.cameraManager.setPreviewDisplay(CameraInstance.this.surface);
                CameraInstance.this.cameraManager.startPreview();
            } catch (Exception e) {
                CameraInstance.this.notifyError(e);
                Log.e(CameraInstance.TAG, "Failed to start preview", e);
            }
        }
    };
    /* access modifiers changed from: private */
    public Handler readyHandler;
    /* access modifiers changed from: private */
    public CameraSurface surface;

    public CameraInstance(Context context) {
        Util.validateMainThread();
        this.cameraThread = CameraThread.getInstance();
        CameraManager cameraManager2 = new CameraManager(context);
        this.cameraManager = cameraManager2;
        cameraManager2.setCameraSettings(this.cameraSettings);
        this.mainHandler = new Handler();
    }

    public CameraInstance(CameraManager cameraManager2) {
        Util.validateMainThread();
        this.cameraManager = cameraManager2;
    }

    public void setDisplayConfiguration(DisplayConfiguration configuration) {
        this.displayConfiguration = configuration;
        this.cameraManager.setDisplayConfiguration(configuration);
    }

    public DisplayConfiguration getDisplayConfiguration() {
        return this.displayConfiguration;
    }

    public void setReadyHandler(Handler readyHandler2) {
        this.readyHandler = readyHandler2;
    }

    public void setSurfaceHolder(SurfaceHolder surfaceHolder) {
        setSurface(new CameraSurface(surfaceHolder));
    }

    public void setSurface(CameraSurface surface2) {
        this.surface = surface2;
    }

    public CameraSettings getCameraSettings() {
        return this.cameraSettings;
    }

    public void setCameraSettings(CameraSettings cameraSettings2) {
        if (!this.open) {
            this.cameraSettings = cameraSettings2;
            this.cameraManager.setCameraSettings(cameraSettings2);
        }
    }

    /* access modifiers changed from: private */
    public Size getPreviewSize() {
        return this.cameraManager.getPreviewSize();
    }

    public int getCameraRotation() {
        return this.cameraManager.getCameraRotation();
    }

    public void open() {
        Util.validateMainThread();
        this.open = true;
        this.cameraClosed = false;
        this.cameraThread.incrementAndEnqueue(this.opener);
    }

    public void configureCamera() {
        Util.validateMainThread();
        validateOpen();
        this.cameraThread.enqueue(this.configure);
    }

    public void startPreview() {
        Util.validateMainThread();
        validateOpen();
        this.cameraThread.enqueue(this.previewStarter);
    }

    public void setTorch(final boolean on) {
        Util.validateMainThread();
        if (this.open) {
            this.cameraThread.enqueue(new Runnable() {
                public void run() {
                    CameraInstance.this.cameraManager.setTorch(on);
                }
            });
        }
    }

    public void changeCameraParameters(final CameraParametersCallback callback) {
        Util.validateMainThread();
        if (this.open) {
            this.cameraThread.enqueue(new Runnable() {
                public void run() {
                    CameraInstance.this.cameraManager.changeCameraParameters(callback);
                }
            });
        }
    }

    public void close() {
        Util.validateMainThread();
        if (this.open) {
            this.cameraThread.enqueue(this.closer);
        } else {
            this.cameraClosed = true;
        }
        this.open = false;
    }

    public boolean isOpen() {
        return this.open;
    }

    public boolean isCameraClosed() {
        return this.cameraClosed;
    }

    public void requestPreview(final PreviewCallback callback) {
        this.mainHandler.post(new Runnable() {
            public void run() {
                if (!CameraInstance.this.open) {
                    Log.d(CameraInstance.TAG, "Camera is closed, not requesting preview");
                } else {
                    CameraInstance.this.cameraThread.enqueue(new Runnable() {
                        public void run() {
                            CameraInstance.this.cameraManager.requestPreviewFrame(callback);
                        }
                    });
                }
            }
        });
    }

    private void validateOpen() {
        if (!this.open) {
            throw new IllegalStateException("CameraInstance is not open");
        }
    }

    /* access modifiers changed from: private */
    public void notifyError(Exception error) {
        Handler handler = this.readyHandler;
        if (handler != null) {
            handler.obtainMessage(R.id.zxing_camera_error, error).sendToTarget();
        }
    }

    /* access modifiers changed from: protected */
    public CameraManager getCameraManager() {
        return this.cameraManager;
    }

    /* access modifiers changed from: protected */
    public CameraThread getCameraThread() {
        return this.cameraThread;
    }

    /* access modifiers changed from: protected */
    public CameraSurface getSurface() {
        return this.surface;
    }
}
