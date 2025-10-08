package com.journeyapps.barcodescanner.camera;

import android.content.Context;
import android.hardware.Camera;
import android.os.Build;
import android.util.Log;
import android.view.SurfaceHolder;
import com.google.zxing.client.android.AmbientLightManager;
import com.google.zxing.client.android.camera.CameraConfigurationUtils;
import com.google.zxing.client.android.camera.open.OpenCameraInterface;
import com.journeyapps.barcodescanner.Size;
import com.journeyapps.barcodescanner.SourceData;
import java.io.IOException;
import java.util.ArrayList;
import java.util.List;
import kotlinx.coroutines.DebugKt;

public final class CameraManager {
    /* access modifiers changed from: private */
    public static final String TAG = CameraManager.class.getSimpleName();
    private AmbientLightManager ambientLightManager;
    private AutoFocusManager autoFocusManager;
    private Camera camera;
    private Camera.CameraInfo cameraInfo;
    private final CameraPreviewCallback cameraPreviewCallback;
    private Context context;
    private String defaultParameters;
    private DisplayConfiguration displayConfiguration;
    private Size previewSize;
    private boolean previewing;
    private Size requestedPreviewSize;
    private int rotationDegrees = -1;
    private CameraSettings settings = new CameraSettings();

    private final class CameraPreviewCallback implements Camera.PreviewCallback {
        private PreviewCallback callback;
        private Size resolution;

        public CameraPreviewCallback() {
        }

        public void setResolution(Size resolution2) {
            this.resolution = resolution2;
        }

        public void setCallback(PreviewCallback callback2) {
            this.callback = callback2;
        }

        public void onPreviewFrame(byte[] data, Camera camera) {
            Size cameraResolution = this.resolution;
            PreviewCallback callback2 = this.callback;
            if (cameraResolution == null || callback2 == null) {
                Log.d(CameraManager.TAG, "Got preview callback, but no handler or resolution available");
                if (callback2 != null) {
                    callback2.onPreviewError(new Exception("No resolution available"));
                }
            } else if (data != null) {
                try {
                    byte[] bArr = data;
                    callback2.onPreview(new SourceData(bArr, cameraResolution.width, cameraResolution.height, camera.getParameters().getPreviewFormat(), CameraManager.this.getCameraRotation()));
                } catch (RuntimeException e) {
                    Log.e(CameraManager.TAG, "Camera preview failed", e);
                    callback2.onPreviewError(e);
                }
            } else {
                throw new NullPointerException("No preview data received");
            }
        }
    }

    public CameraManager(Context context2) {
        this.context = context2;
        this.cameraPreviewCallback = new CameraPreviewCallback();
    }

    public void open() {
        Camera open = OpenCameraInterface.open(this.settings.getRequestedCameraId());
        this.camera = open;
        if (open != null) {
            int cameraId = OpenCameraInterface.getCameraId(this.settings.getRequestedCameraId());
            Camera.CameraInfo cameraInfo2 = new Camera.CameraInfo();
            this.cameraInfo = cameraInfo2;
            Camera.getCameraInfo(cameraId, cameraInfo2);
            return;
        }
        throw new RuntimeException("Failed to open camera");
    }

    public void configure() {
        if (this.camera != null) {
            setParameters();
            return;
        }
        throw new RuntimeException("Camera not open");
    }

    public void setPreviewDisplay(SurfaceHolder holder) throws IOException {
        setPreviewDisplay(new CameraSurface(holder));
    }

    public void setPreviewDisplay(CameraSurface surface) throws IOException {
        surface.setPreview(this.camera);
    }

    public void startPreview() {
        Camera theCamera = this.camera;
        if (theCamera != null && !this.previewing) {
            theCamera.startPreview();
            this.previewing = true;
            this.autoFocusManager = new AutoFocusManager(this.camera, this.settings);
            AmbientLightManager ambientLightManager2 = new AmbientLightManager(this.context, this, this.settings);
            this.ambientLightManager = ambientLightManager2;
            ambientLightManager2.start();
        }
    }

    public void stopPreview() {
        AutoFocusManager autoFocusManager2 = this.autoFocusManager;
        if (autoFocusManager2 != null) {
            autoFocusManager2.stop();
            this.autoFocusManager = null;
        }
        AmbientLightManager ambientLightManager2 = this.ambientLightManager;
        if (ambientLightManager2 != null) {
            ambientLightManager2.stop();
            this.ambientLightManager = null;
        }
        Camera camera2 = this.camera;
        if (camera2 != null && this.previewing) {
            camera2.stopPreview();
            this.cameraPreviewCallback.setCallback((PreviewCallback) null);
            this.previewing = false;
        }
    }

    public void close() {
        Camera camera2 = this.camera;
        if (camera2 != null) {
            camera2.release();
            this.camera = null;
        }
    }

    public boolean isCameraRotated() {
        int i = this.rotationDegrees;
        if (i != -1) {
            return i % 180 != 0;
        }
        throw new IllegalStateException("Rotation not calculated yet. Call configure() first.");
    }

    public int getCameraRotation() {
        return this.rotationDegrees;
    }

    private Camera.Parameters getDefaultCameraParameters() {
        Camera.Parameters parameters = this.camera.getParameters();
        String str = this.defaultParameters;
        if (str == null) {
            this.defaultParameters = parameters.flatten();
        } else {
            parameters.unflatten(str);
        }
        return parameters;
    }

    private void setDesiredParameters(boolean safeMode) {
        Camera.Parameters parameters = getDefaultCameraParameters();
        if (parameters == null) {
            Log.w(TAG, "Device error: no camera parameters are available. Proceeding without configuration.");
            return;
        }
        String str = TAG;
        Log.i(str, "Initial camera parameters: " + parameters.flatten());
        if (safeMode) {
            Log.w(str, "In camera config safe mode -- most settings will not be honored");
        }
        CameraConfigurationUtils.setFocus(parameters, this.settings.getFocusMode(), safeMode);
        if (!safeMode) {
            CameraConfigurationUtils.setTorch(parameters, false);
            if (this.settings.isScanInverted()) {
                CameraConfigurationUtils.setInvertColor(parameters);
            }
            if (this.settings.isBarcodeSceneModeEnabled()) {
                CameraConfigurationUtils.setBarcodeSceneMode(parameters);
            }
            if (this.settings.isMeteringEnabled() && Build.VERSION.SDK_INT >= 15) {
                CameraConfigurationUtils.setVideoStabilization(parameters);
                CameraConfigurationUtils.setFocusArea(parameters);
                CameraConfigurationUtils.setMetering(parameters);
            }
        }
        List<Size> previewSizes = getPreviewSizes(parameters);
        if (previewSizes.size() == 0) {
            this.requestedPreviewSize = null;
        } else {
            Size bestPreviewSize = this.displayConfiguration.getBestPreviewSize(previewSizes, isCameraRotated());
            this.requestedPreviewSize = bestPreviewSize;
            parameters.setPreviewSize(bestPreviewSize.width, this.requestedPreviewSize.height);
        }
        if (Build.DEVICE.equals("glass-1")) {
            CameraConfigurationUtils.setBestPreviewFPS(parameters);
        }
        Log.i(str, "Final camera parameters: " + parameters.flatten());
        this.camera.setParameters(parameters);
    }

    private static List<Size> getPreviewSizes(Camera.Parameters parameters) {
        List<Camera.Size> rawSupportedSizes = parameters.getSupportedPreviewSizes();
        List<Size> previewSizes = new ArrayList<>();
        if (rawSupportedSizes == null) {
            Camera.Size defaultSize = parameters.getPreviewSize();
            if (defaultSize != null) {
                previewSizes.add(new Size(defaultSize.width, defaultSize.height));
            }
            return previewSizes;
        }
        for (Camera.Size size : rawSupportedSizes) {
            previewSizes.add(new Size(size.width, size.height));
        }
        return previewSizes;
    }

    private int calculateDisplayRotation() {
        int result;
        int degrees = 0;
        switch (this.displayConfiguration.getRotation()) {
            case 0:
                degrees = 0;
                break;
            case 1:
                degrees = 90;
                break;
            case 2:
                degrees = 180;
                break;
            case 3:
                degrees = 270;
                break;
        }
        if (this.cameraInfo.facing == 1) {
            result = (360 - ((this.cameraInfo.orientation + degrees) % 360)) % 360;
        } else {
            result = ((this.cameraInfo.orientation - degrees) + 360) % 360;
        }
        Log.i(TAG, "Camera Display Orientation: " + result);
        return result;
    }

    private void setCameraDisplayOrientation(int rotation) {
        this.camera.setDisplayOrientation(rotation);
    }

    private void setParameters() {
        try {
            int calculateDisplayRotation = calculateDisplayRotation();
            this.rotationDegrees = calculateDisplayRotation;
            setCameraDisplayOrientation(calculateDisplayRotation);
        } catch (Exception e) {
            Log.w(TAG, "Failed to set rotation.");
        }
        try {
            setDesiredParameters(false);
        } catch (Exception e2) {
            try {
                setDesiredParameters(true);
            } catch (Exception e3) {
                Log.w(TAG, "Camera rejected even safe-mode parameters! No configuration");
            }
        }
        Camera.Size realPreviewSize = this.camera.getParameters().getPreviewSize();
        if (realPreviewSize == null) {
            this.previewSize = this.requestedPreviewSize;
        } else {
            this.previewSize = new Size(realPreviewSize.width, realPreviewSize.height);
        }
        this.cameraPreviewCallback.setResolution(this.previewSize);
    }

    public boolean isOpen() {
        return this.camera != null;
    }

    public Size getNaturalPreviewSize() {
        return this.previewSize;
    }

    public Size getPreviewSize() {
        if (this.previewSize == null) {
            return null;
        }
        if (isCameraRotated()) {
            return this.previewSize.rotate();
        }
        return this.previewSize;
    }

    public void requestPreviewFrame(PreviewCallback callback) {
        Camera theCamera = this.camera;
        if (theCamera != null && this.previewing) {
            this.cameraPreviewCallback.setCallback(callback);
            theCamera.setOneShotPreviewCallback(this.cameraPreviewCallback);
        }
    }

    public CameraSettings getCameraSettings() {
        return this.settings;
    }

    public void setCameraSettings(CameraSettings settings2) {
        this.settings = settings2;
    }

    public DisplayConfiguration getDisplayConfiguration() {
        return this.displayConfiguration;
    }

    public void setDisplayConfiguration(DisplayConfiguration displayConfiguration2) {
        this.displayConfiguration = displayConfiguration2;
    }

    public void setTorch(boolean on) {
        if (this.camera != null) {
            try {
                if (on != isTorchOn()) {
                    AutoFocusManager autoFocusManager2 = this.autoFocusManager;
                    if (autoFocusManager2 != null) {
                        autoFocusManager2.stop();
                    }
                    Camera.Parameters parameters = this.camera.getParameters();
                    CameraConfigurationUtils.setTorch(parameters, on);
                    if (this.settings.isExposureEnabled()) {
                        CameraConfigurationUtils.setBestExposure(parameters, on);
                    }
                    this.camera.setParameters(parameters);
                    AutoFocusManager autoFocusManager3 = this.autoFocusManager;
                    if (autoFocusManager3 != null) {
                        autoFocusManager3.start();
                    }
                }
            } catch (RuntimeException e) {
                Log.e(TAG, "Failed to set torch", e);
            }
        }
    }

    public void changeCameraParameters(CameraParametersCallback callback) {
        Camera camera2 = this.camera;
        if (camera2 != null) {
            try {
                camera2.setParameters(callback.changeCameraParameters(camera2.getParameters()));
            } catch (RuntimeException e) {
                Log.e(TAG, "Failed to change camera parameters", e);
            }
        }
    }

    public boolean isTorchOn() {
        String flashMode;
        Camera.Parameters parameters = this.camera.getParameters();
        if (parameters == null || (flashMode = parameters.getFlashMode()) == null) {
            return false;
        }
        if (DebugKt.DEBUG_PROPERTY_VALUE_ON.equals(flashMode) || "torch".equals(flashMode)) {
            return true;
        }
        return false;
    }

    public Camera getCamera() {
        return this.camera;
    }
}
