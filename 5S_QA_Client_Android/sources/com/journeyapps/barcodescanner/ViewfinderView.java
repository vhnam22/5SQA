package com.journeyapps.barcodescanner;

import android.content.Context;
import android.content.res.Resources;
import android.content.res.TypedArray;
import android.graphics.Bitmap;
import android.graphics.Canvas;
import android.graphics.Paint;
import android.graphics.Rect;
import android.util.AttributeSet;
import android.view.View;
import com.google.zxing.ResultPoint;
import com.google.zxing.client.android.R;
import com.journeyapps.barcodescanner.CameraPreview;
import java.util.ArrayList;
import java.util.Iterator;
import java.util.List;

public class ViewfinderView extends View {
    protected static final long ANIMATION_DELAY = 80;
    protected static final int CURRENT_POINT_OPACITY = 160;
    protected static final int MAX_RESULT_POINTS = 20;
    protected static final int POINT_SIZE = 6;
    protected static final int[] SCANNER_ALPHA = {0, 64, 128, 192, 255, 192, 128, 64};
    protected static final String TAG = ViewfinderView.class.getSimpleName();
    protected CameraPreview cameraPreview;
    protected Rect framingRect;
    protected final int laserColor;
    protected List<ResultPoint> lastPossibleResultPoints;
    protected final int maskColor;
    protected final Paint paint = new Paint(1);
    protected List<ResultPoint> possibleResultPoints;
    protected Rect previewFramingRect;
    protected Bitmap resultBitmap;
    protected final int resultColor;
    protected final int resultPointColor;
    protected int scannerAlpha;

    public ViewfinderView(Context context, AttributeSet attrs) {
        super(context, attrs);
        Resources resources = getResources();
        TypedArray attributes = getContext().obtainStyledAttributes(attrs, R.styleable.zxing_finder);
        this.maskColor = attributes.getColor(R.styleable.zxing_finder_zxing_viewfinder_mask, resources.getColor(R.color.zxing_viewfinder_mask));
        this.resultColor = attributes.getColor(R.styleable.zxing_finder_zxing_result_view, resources.getColor(R.color.zxing_result_view));
        this.laserColor = attributes.getColor(R.styleable.zxing_finder_zxing_viewfinder_laser, resources.getColor(R.color.zxing_viewfinder_laser));
        this.resultPointColor = attributes.getColor(R.styleable.zxing_finder_zxing_possible_result_points, resources.getColor(R.color.zxing_possible_result_points));
        attributes.recycle();
        this.scannerAlpha = 0;
        this.possibleResultPoints = new ArrayList(20);
        this.lastPossibleResultPoints = new ArrayList(20);
    }

    public void setCameraPreview(CameraPreview view) {
        this.cameraPreview = view;
        view.addStateListener(new CameraPreview.StateListener() {
            public void previewSized() {
                ViewfinderView.this.refreshSizes();
                ViewfinderView.this.invalidate();
            }

            public void previewStarted() {
            }

            public void previewStopped() {
            }

            public void cameraError(Exception error) {
            }

            public void cameraClosed() {
            }
        });
    }

    /* access modifiers changed from: protected */
    public void refreshSizes() {
        CameraPreview cameraPreview2 = this.cameraPreview;
        if (cameraPreview2 != null) {
            Rect framingRect2 = cameraPreview2.getFramingRect();
            Rect previewFramingRect2 = this.cameraPreview.getPreviewFramingRect();
            if (framingRect2 != null && previewFramingRect2 != null) {
                this.framingRect = framingRect2;
                this.previewFramingRect = previewFramingRect2;
            }
        }
    }

    public void onDraw(Canvas canvas) {
        Canvas canvas2 = canvas;
        refreshSizes();
        if (this.framingRect != null && this.previewFramingRect != null) {
            Rect frame = this.framingRect;
            Rect previewFrame = this.previewFramingRect;
            int width = canvas.getWidth();
            int height = canvas.getHeight();
            this.paint.setColor(this.resultBitmap != null ? this.resultColor : this.maskColor);
            canvas.drawRect(0.0f, 0.0f, (float) width, (float) frame.top, this.paint);
            canvas.drawRect(0.0f, (float) frame.top, (float) frame.left, (float) (frame.bottom + 1), this.paint);
            canvas.drawRect((float) (frame.right + 1), (float) frame.top, (float) width, (float) (frame.bottom + 1), this.paint);
            canvas.drawRect(0.0f, (float) (frame.bottom + 1), (float) width, (float) height, this.paint);
            if (this.resultBitmap != null) {
                this.paint.setAlpha(CURRENT_POINT_OPACITY);
                canvas2.drawBitmap(this.resultBitmap, (Rect) null, frame, this.paint);
                return;
            }
            this.paint.setColor(this.laserColor);
            Paint paint2 = this.paint;
            int[] iArr = SCANNER_ALPHA;
            paint2.setAlpha(iArr[this.scannerAlpha]);
            this.scannerAlpha = (this.scannerAlpha + 1) % iArr.length;
            int middle = (frame.height() / 2) + frame.top;
            canvas.drawRect((float) (frame.left + 2), (float) (middle - 1), (float) (frame.right - 1), (float) (middle + 2), this.paint);
            float scaleX = ((float) frame.width()) / ((float) previewFrame.width());
            float scaleY = ((float) frame.height()) / ((float) previewFrame.height());
            int frameLeft = frame.left;
            int frameTop = frame.top;
            if (!this.lastPossibleResultPoints.isEmpty()) {
                this.paint.setAlpha(80);
                this.paint.setColor(this.resultPointColor);
                for (Iterator<ResultPoint> it = this.lastPossibleResultPoints.iterator(); it.hasNext(); it = it) {
                    ResultPoint point = it.next();
                    canvas2.drawCircle((float) (((int) (point.getX() * scaleX)) + frameLeft), (float) (((int) (point.getY() * scaleY)) + frameTop), 3.0f, this.paint);
                }
                this.lastPossibleResultPoints.clear();
            }
            if (!this.possibleResultPoints.isEmpty()) {
                this.paint.setAlpha(CURRENT_POINT_OPACITY);
                this.paint.setColor(this.resultPointColor);
                for (Iterator<ResultPoint> it2 = this.possibleResultPoints.iterator(); it2.hasNext(); it2 = it2) {
                    ResultPoint point2 = it2.next();
                    canvas2.drawCircle((float) (((int) (point2.getX() * scaleX)) + frameLeft), (float) (((int) (point2.getY() * scaleY)) + frameTop), 6.0f, this.paint);
                }
                List<ResultPoint> temp = this.possibleResultPoints;
                List<ResultPoint> list = this.lastPossibleResultPoints;
                this.possibleResultPoints = list;
                this.lastPossibleResultPoints = temp;
                list.clear();
            }
            int i = frameTop;
            int i2 = frameLeft;
            postInvalidateDelayed(ANIMATION_DELAY, frame.left - 6, frame.top - 6, frame.right + 6, frame.bottom + 6);
        }
    }

    public void drawViewfinder() {
        Bitmap resultBitmap2 = this.resultBitmap;
        this.resultBitmap = null;
        if (resultBitmap2 != null) {
            resultBitmap2.recycle();
        }
        invalidate();
    }

    public void drawResultBitmap(Bitmap result) {
        this.resultBitmap = result;
        invalidate();
    }

    public void addPossibleResultPoint(ResultPoint point) {
        if (this.possibleResultPoints.size() < 20) {
            this.possibleResultPoints.add(point);
        }
    }
}
