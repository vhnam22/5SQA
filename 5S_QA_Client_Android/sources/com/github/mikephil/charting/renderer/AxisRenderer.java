package com.github.mikephil.charting.renderer;

import android.graphics.Canvas;
import android.graphics.Paint;
import androidx.core.view.ViewCompat;
import com.github.mikephil.charting.components.AxisBase;
import com.github.mikephil.charting.utils.MPPointD;
import com.github.mikephil.charting.utils.Transformer;
import com.github.mikephil.charting.utils.Utils;
import com.github.mikephil.charting.utils.ViewPortHandler;

public abstract class AxisRenderer extends Renderer {
    protected AxisBase mAxis;
    protected Paint mAxisLabelPaint;
    protected Paint mAxisLinePaint;
    protected Paint mGridPaint;
    protected Paint mLimitLinePaint;
    protected Transformer mTrans;

    public abstract void renderAxisLabels(Canvas canvas);

    public abstract void renderAxisLine(Canvas canvas);

    public abstract void renderGridLines(Canvas canvas);

    public abstract void renderLimitLines(Canvas canvas);

    public AxisRenderer(ViewPortHandler viewPortHandler, Transformer trans, AxisBase axis) {
        super(viewPortHandler);
        this.mTrans = trans;
        this.mAxis = axis;
        if (this.mViewPortHandler != null) {
            this.mAxisLabelPaint = new Paint(1);
            Paint paint = new Paint();
            this.mGridPaint = paint;
            paint.setColor(-7829368);
            this.mGridPaint.setStrokeWidth(1.0f);
            this.mGridPaint.setStyle(Paint.Style.STROKE);
            this.mGridPaint.setAlpha(90);
            Paint paint2 = new Paint();
            this.mAxisLinePaint = paint2;
            paint2.setColor(ViewCompat.MEASURED_STATE_MASK);
            this.mAxisLinePaint.setStrokeWidth(1.0f);
            this.mAxisLinePaint.setStyle(Paint.Style.STROKE);
            Paint paint3 = new Paint(1);
            this.mLimitLinePaint = paint3;
            paint3.setStyle(Paint.Style.STROKE);
        }
    }

    public Paint getPaintAxisLabels() {
        return this.mAxisLabelPaint;
    }

    public Paint getPaintGrid() {
        return this.mGridPaint;
    }

    public Paint getPaintAxisLine() {
        return this.mAxisLinePaint;
    }

    public Transformer getTransformer() {
        return this.mTrans;
    }

    public void computeAxis(float min, float max, boolean inverted) {
        if (this.mViewPortHandler != null && this.mViewPortHandler.contentWidth() > 10.0f && !this.mViewPortHandler.isFullyZoomedOutY()) {
            MPPointD p1 = this.mTrans.getValuesByTouchPoint(this.mViewPortHandler.contentLeft(), this.mViewPortHandler.contentTop());
            MPPointD p2 = this.mTrans.getValuesByTouchPoint(this.mViewPortHandler.contentLeft(), this.mViewPortHandler.contentBottom());
            if (!inverted) {
                min = (float) p2.y;
                max = (float) p1.y;
            } else {
                min = (float) p1.y;
                max = (float) p2.y;
            }
            MPPointD.recycleInstance(p1);
            MPPointD.recycleInstance(p2);
        }
        computeAxisValues(min, max);
    }

    /* access modifiers changed from: protected */
    public void computeAxisValues(float min, float max) {
        int n;
        int n2;
        double last;
        float yMin = min;
        float yMax = max;
        int labelCount = this.mAxis.getLabelCount();
        double range = (double) Math.abs(yMax - yMin);
        if (labelCount == 0 || range <= Utils.DOUBLE_EPSILON) {
            float f = yMax;
            int i = labelCount;
            double d = range;
        } else if (Double.isInfinite(range)) {
            float f2 = yMin;
            float f3 = yMax;
            int i2 = labelCount;
            double d2 = range;
        } else {
            double interval = (double) Utils.roundToNextSignificant(range / ((double) labelCount));
            if (this.mAxis.isGranularityEnabled()) {
                interval = interval < ((double) this.mAxis.getGranularity()) ? (double) this.mAxis.getGranularity() : interval;
            }
            double intervalMagnitude = (double) Utils.roundToNextSignificant(Math.pow(10.0d, (double) ((int) Math.log10(interval))));
            int intervalSigDigit = (int) (interval / intervalMagnitude);
            if (intervalSigDigit > 5) {
                interval = Math.floor(10.0d * intervalMagnitude);
            }
            int n3 = this.mAxis.isCenterAxisLabelsEnabled();
            if (this.mAxis.isForceLabelsEnabled()) {
                interval = (double) (((float) range) / ((float) (labelCount - 1)));
                this.mAxis.mEntryCount = labelCount;
                if (this.mAxis.mEntries.length < labelCount) {
                    this.mAxis.mEntries = new float[labelCount];
                }
                float v = min;
                int i3 = 0;
                while (i3 < labelCount) {
                    this.mAxis.mEntries[i3] = v;
                    v = (float) (((double) v) + interval);
                    i3++;
                    range = range;
                }
                n = labelCount;
                float f4 = yMin;
                float f5 = yMax;
                int i4 = labelCount;
                int i5 = intervalSigDigit;
            } else {
                double first = interval == Utils.DOUBLE_EPSILON ? Utils.DOUBLE_EPSILON : Math.ceil(((double) yMin) / interval) * interval;
                if (this.mAxis.isCenterAxisLabelsEnabled()) {
                    first -= interval;
                }
                if (interval == Utils.DOUBLE_EPSILON) {
                    int i6 = intervalSigDigit;
                    n2 = n3;
                    last = Utils.DOUBLE_EPSILON;
                } else {
                    int i7 = intervalSigDigit;
                    n2 = n3;
                    last = Utils.nextUp(Math.floor(((double) yMax) / interval) * interval);
                }
                if (interval != Utils.DOUBLE_EPSILON) {
                    for (double f6 = first; f6 <= last; f6 += interval) {
                        n2++;
                    }
                }
                float f7 = yMin;
                this.mAxis.mEntryCount = n2;
                if (this.mAxis.mEntries.length < n2) {
                    float f8 = yMax;
                    this.mAxis.mEntries = new float[n2];
                }
                double f9 = first;
                int i8 = labelCount;
                int i9 = 0;
                while (i9 < n2) {
                    if (f9 == Utils.DOUBLE_EPSILON) {
                        f9 = Utils.DOUBLE_EPSILON;
                    }
                    this.mAxis.mEntries[i9] = (float) f9;
                    f9 += interval;
                    i9++;
                    first = first;
                }
                n = n2;
            }
            if (interval < 1.0d) {
                this.mAxis.mDecimals = (int) Math.ceil(-Math.log10(interval));
            } else {
                this.mAxis.mDecimals = 0;
            }
            if (this.mAxis.isCenterAxisLabelsEnabled()) {
                if (this.mAxis.mCenteredEntries.length < n) {
                    this.mAxis.mCenteredEntries = new float[n];
                }
                float offset = ((float) interval) / 2.0f;
                for (int i10 = 0; i10 < n; i10++) {
                    this.mAxis.mCenteredEntries[i10] = this.mAxis.mEntries[i10] + offset;
                }
                return;
            }
            return;
        }
        this.mAxis.mEntries = new float[0];
        this.mAxis.mCenteredEntries = new float[0];
        this.mAxis.mEntryCount = 0;
    }
}
