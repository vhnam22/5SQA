package com.journeyapps.barcodescanner.camera;

import android.graphics.Rect;
import com.journeyapps.barcodescanner.Size;
import java.util.List;

public class DisplayConfiguration {
    private static final String TAG = DisplayConfiguration.class.getSimpleName();
    private boolean center = false;
    private PreviewScalingStrategy previewScalingStrategy = new FitCenterStrategy();
    private int rotation;
    private Size viewfinderSize;

    public DisplayConfiguration(int rotation2) {
        this.rotation = rotation2;
    }

    public DisplayConfiguration(int rotation2, Size viewfinderSize2) {
        this.rotation = rotation2;
        this.viewfinderSize = viewfinderSize2;
    }

    public int getRotation() {
        return this.rotation;
    }

    public Size getViewfinderSize() {
        return this.viewfinderSize;
    }

    public PreviewScalingStrategy getPreviewScalingStrategy() {
        return this.previewScalingStrategy;
    }

    public void setPreviewScalingStrategy(PreviewScalingStrategy previewScalingStrategy2) {
        this.previewScalingStrategy = previewScalingStrategy2;
    }

    public Size getDesiredPreviewSize(boolean rotate) {
        Size size = this.viewfinderSize;
        if (size == null) {
            return null;
        }
        if (rotate) {
            return size.rotate();
        }
        return size;
    }

    public Size getBestPreviewSize(List<Size> sizes, boolean isRotated) {
        return this.previewScalingStrategy.getBestPreviewSize(sizes, getDesiredPreviewSize(isRotated));
    }

    public Rect scalePreview(Size previewSize) {
        return this.previewScalingStrategy.scalePreview(previewSize, this.viewfinderSize);
    }
}
