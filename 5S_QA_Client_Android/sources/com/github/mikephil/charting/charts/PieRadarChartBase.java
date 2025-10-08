package com.github.mikephil.charting.charts;

import android.animation.ObjectAnimator;
import android.animation.ValueAnimator;
import android.content.Context;
import android.graphics.RectF;
import android.util.AttributeSet;
import android.view.MotionEvent;
import com.github.mikephil.charting.animation.Easing;
import com.github.mikephil.charting.components.Legend;
import com.github.mikephil.charting.data.ChartData;
import com.github.mikephil.charting.data.Entry;
import com.github.mikephil.charting.interfaces.datasets.IDataSet;
import com.github.mikephil.charting.listener.PieRadarChartTouchListener;
import com.github.mikephil.charting.utils.MPPointF;
import com.github.mikephil.charting.utils.Utils;

public abstract class PieRadarChartBase<T extends ChartData<? extends IDataSet<? extends Entry>>> extends Chart<T> {
    protected float mMinOffset = 0.0f;
    private float mRawRotationAngle = 270.0f;
    protected boolean mRotateEnabled = true;
    private float mRotationAngle = 270.0f;

    public abstract int getIndexForAngle(float f);

    public abstract float getRadius();

    /* access modifiers changed from: protected */
    public abstract float getRequiredBaseOffset();

    /* access modifiers changed from: protected */
    public abstract float getRequiredLegendOffset();

    public PieRadarChartBase(Context context) {
        super(context);
    }

    public PieRadarChartBase(Context context, AttributeSet attrs) {
        super(context, attrs);
    }

    public PieRadarChartBase(Context context, AttributeSet attrs, int defStyle) {
        super(context, attrs, defStyle);
    }

    /* access modifiers changed from: protected */
    public void init() {
        super.init();
        this.mChartTouchListener = new PieRadarChartTouchListener(this);
    }

    /* access modifiers changed from: protected */
    public void calcMinMax() {
    }

    public int getMaxVisibleCount() {
        return this.mData.getEntryCount();
    }

    public boolean onTouchEvent(MotionEvent event) {
        if (!this.mTouchEnabled || this.mChartTouchListener == null) {
            return super.onTouchEvent(event);
        }
        return this.mChartTouchListener.onTouch(this, event);
    }

    public void computeScroll() {
        if (this.mChartTouchListener instanceof PieRadarChartTouchListener) {
            ((PieRadarChartTouchListener) this.mChartTouchListener).computeScroll();
        }
    }

    public void notifyDataSetChanged() {
        if (this.mData != null) {
            calcMinMax();
            if (this.mLegend != null) {
                this.mLegendRenderer.computeLegend(this.mData);
            }
            calculateOffsets();
        }
    }

    /* JADX WARNING: Can't fix incorrect switch cases order */
    /* JADX WARNING: Code restructure failed: missing block: B:50:0x01c4, code lost:
        r1 = r16;
        r2 = r17;
        r3 = r18;
     */
    /* JADX WARNING: Code restructure failed: missing block: B:51:0x01ca, code lost:
        r1 = r1 + getRequiredBaseOffset();
        r2 = r2 + getRequiredBaseOffset();
        r4 = r4 + getRequiredBaseOffset();
        r3 = r3 + getRequiredBaseOffset();
     */
    /* Code decompiled incorrectly, please refer to instructions dump. */
    public void calculateOffsets() {
        /*
            r19 = this;
            r0 = r19
            r1 = 0
            r2 = 0
            r3 = 0
            r4 = 0
            com.github.mikephil.charting.components.Legend r5 = r0.mLegend
            if (r5 == 0) goto L_0x01df
            com.github.mikephil.charting.components.Legend r5 = r0.mLegend
            boolean r5 = r5.isEnabled()
            if (r5 == 0) goto L_0x01df
            com.github.mikephil.charting.components.Legend r5 = r0.mLegend
            boolean r5 = r5.isDrawInsideEnabled()
            if (r5 != 0) goto L_0x01df
            com.github.mikephil.charting.components.Legend r5 = r0.mLegend
            float r5 = r5.mNeededWidth
            com.github.mikephil.charting.utils.ViewPortHandler r6 = r0.mViewPortHandler
            float r6 = r6.getChartWidth()
            com.github.mikephil.charting.components.Legend r7 = r0.mLegend
            float r7 = r7.getMaxSizePercent()
            float r6 = r6 * r7
            float r5 = java.lang.Math.min(r5, r6)
            int[] r6 = com.github.mikephil.charting.charts.PieRadarChartBase.AnonymousClass2.$SwitchMap$com$github$mikephil$charting$components$Legend$LegendOrientation
            com.github.mikephil.charting.components.Legend r7 = r0.mLegend
            com.github.mikephil.charting.components.Legend$LegendOrientation r7 = r7.getOrientation()
            int r7 = r7.ordinal()
            r6 = r6[r7]
            switch(r6) {
                case 1: goto L_0x00a1;
                case 2: goto L_0x0049;
                default: goto L_0x0041;
            }
        L_0x0041:
            r16 = r1
            r17 = r2
            r18 = r3
            goto L_0x01c4
        L_0x0049:
            r6 = 0
            com.github.mikephil.charting.components.Legend r7 = r0.mLegend
            com.github.mikephil.charting.components.Legend$LegendVerticalAlignment r7 = r7.getVerticalAlignment()
            com.github.mikephil.charting.components.Legend$LegendVerticalAlignment r8 = com.github.mikephil.charting.components.Legend.LegendVerticalAlignment.TOP
            if (r7 == r8) goto L_0x0067
            com.github.mikephil.charting.components.Legend r7 = r0.mLegend
            com.github.mikephil.charting.components.Legend$LegendVerticalAlignment r7 = r7.getVerticalAlignment()
            com.github.mikephil.charting.components.Legend$LegendVerticalAlignment r8 = com.github.mikephil.charting.components.Legend.LegendVerticalAlignment.BOTTOM
            if (r7 != r8) goto L_0x005f
            goto L_0x0067
        L_0x005f:
            r16 = r1
            r17 = r2
            r18 = r3
            goto L_0x01c4
        L_0x0067:
            float r7 = r19.getRequiredLegendOffset()
            com.github.mikephil.charting.components.Legend r8 = r0.mLegend
            float r8 = r8.mNeededHeight
            float r8 = r8 + r7
            com.github.mikephil.charting.utils.ViewPortHandler r9 = r0.mViewPortHandler
            float r9 = r9.getChartHeight()
            com.github.mikephil.charting.components.Legend r10 = r0.mLegend
            float r10 = r10.getMaxSizePercent()
            float r9 = r9 * r10
            float r6 = java.lang.Math.min(r8, r9)
            int[] r8 = com.github.mikephil.charting.charts.PieRadarChartBase.AnonymousClass2.$SwitchMap$com$github$mikephil$charting$components$Legend$LegendVerticalAlignment
            com.github.mikephil.charting.components.Legend r9 = r0.mLegend
            com.github.mikephil.charting.components.Legend$LegendVerticalAlignment r9 = r9.getVerticalAlignment()
            int r9 = r9.ordinal()
            r8 = r8[r9]
            switch(r8) {
                case 1: goto L_0x009e;
                case 2: goto L_0x009b;
                default: goto L_0x0093;
            }
        L_0x0093:
            r16 = r1
            r17 = r2
            r18 = r3
            goto L_0x01c4
        L_0x009b:
            r3 = r6
            goto L_0x01ca
        L_0x009e:
            r4 = r6
            goto L_0x01ca
        L_0x00a1:
            r6 = 0
            com.github.mikephil.charting.components.Legend r7 = r0.mLegend
            com.github.mikephil.charting.components.Legend$LegendHorizontalAlignment r7 = r7.getHorizontalAlignment()
            com.github.mikephil.charting.components.Legend$LegendHorizontalAlignment r8 = com.github.mikephil.charting.components.Legend.LegendHorizontalAlignment.LEFT
            if (r7 == r8) goto L_0x00bf
            com.github.mikephil.charting.components.Legend r7 = r0.mLegend
            com.github.mikephil.charting.components.Legend$LegendHorizontalAlignment r7 = r7.getHorizontalAlignment()
            com.github.mikephil.charting.components.Legend$LegendHorizontalAlignment r8 = com.github.mikephil.charting.components.Legend.LegendHorizontalAlignment.RIGHT
            if (r7 != r8) goto L_0x00b7
            goto L_0x00bf
        L_0x00b7:
            r16 = r1
            r17 = r2
            r18 = r3
            goto L_0x0152
        L_0x00bf:
            com.github.mikephil.charting.components.Legend r7 = r0.mLegend
            com.github.mikephil.charting.components.Legend$LegendVerticalAlignment r7 = r7.getVerticalAlignment()
            com.github.mikephil.charting.components.Legend$LegendVerticalAlignment r8 = com.github.mikephil.charting.components.Legend.LegendVerticalAlignment.CENTER
            if (r7 != r8) goto L_0x00d9
            r7 = 1095761920(0x41500000, float:13.0)
            float r7 = com.github.mikephil.charting.utils.Utils.convertDpToPixel(r7)
            float r6 = r5 + r7
            r16 = r1
            r17 = r2
            r18 = r3
            goto L_0x0152
        L_0x00d9:
            r7 = 1090519040(0x41000000, float:8.0)
            float r7 = com.github.mikephil.charting.utils.Utils.convertDpToPixel(r7)
            float r8 = r5 + r7
            com.github.mikephil.charting.components.Legend r9 = r0.mLegend
            float r9 = r9.mNeededHeight
            com.github.mikephil.charting.components.Legend r10 = r0.mLegend
            float r10 = r10.mTextHeightMax
            float r9 = r9 + r10
            com.github.mikephil.charting.utils.MPPointF r10 = r19.getCenter()
            com.github.mikephil.charting.components.Legend r11 = r0.mLegend
            com.github.mikephil.charting.components.Legend$LegendHorizontalAlignment r11 = r11.getHorizontalAlignment()
            com.github.mikephil.charting.components.Legend$LegendHorizontalAlignment r12 = com.github.mikephil.charting.components.Legend.LegendHorizontalAlignment.RIGHT
            r13 = 1097859072(0x41700000, float:15.0)
            if (r11 != r12) goto L_0x0102
            int r11 = r19.getWidth()
            float r11 = (float) r11
            float r11 = r11 - r8
            float r11 = r11 + r13
            goto L_0x0104
        L_0x0102:
            float r11 = r8 - r13
        L_0x0104:
            float r13 = r13 + r9
            float r12 = r0.distanceToCenter(r11, r13)
            float r14 = r19.getRadius()
            float r15 = r0.getAngleForPoint(r11, r13)
            com.github.mikephil.charting.utils.MPPointF r14 = r0.getPosition(r10, r14, r15)
            float r15 = r14.x
            r16 = r1
            float r1 = r14.y
            float r1 = r0.distanceToCenter(r15, r1)
            r15 = 1084227584(0x40a00000, float:5.0)
            float r15 = com.github.mikephil.charting.utils.Utils.convertDpToPixel(r15)
            r17 = r2
            float r2 = r10.y
            int r2 = (r13 > r2 ? 1 : (r13 == r2 ? 0 : -1))
            if (r2 < 0) goto L_0x0141
            int r2 = r19.getHeight()
            float r2 = (float) r2
            float r2 = r2 - r8
            r18 = r3
            int r3 = r19.getWidth()
            float r3 = (float) r3
            int r2 = (r2 > r3 ? 1 : (r2 == r3 ? 0 : -1))
            if (r2 <= 0) goto L_0x0143
            r2 = r8
            r6 = r2
            goto L_0x014c
        L_0x0141:
            r18 = r3
        L_0x0143:
            int r2 = (r12 > r1 ? 1 : (r12 == r1 ? 0 : -1))
            if (r2 >= 0) goto L_0x014c
            float r2 = r1 - r12
            float r3 = r15 + r2
            r6 = r3
        L_0x014c:
            com.github.mikephil.charting.utils.MPPointF.recycleInstance(r10)
            com.github.mikephil.charting.utils.MPPointF.recycleInstance(r14)
        L_0x0152:
            int[] r1 = com.github.mikephil.charting.charts.PieRadarChartBase.AnonymousClass2.$SwitchMap$com$github$mikephil$charting$components$Legend$LegendHorizontalAlignment
            com.github.mikephil.charting.components.Legend r2 = r0.mLegend
            com.github.mikephil.charting.components.Legend$LegendHorizontalAlignment r2 = r2.getHorizontalAlignment()
            int r2 = r2.ordinal()
            r1 = r1[r2]
            switch(r1) {
                case 1: goto L_0x01b7;
                case 2: goto L_0x01b0;
                case 3: goto L_0x0164;
                default: goto L_0x0163;
            }
        L_0x0163:
            goto L_0x01bd
        L_0x0164:
            int[] r1 = com.github.mikephil.charting.charts.PieRadarChartBase.AnonymousClass2.$SwitchMap$com$github$mikephil$charting$components$Legend$LegendVerticalAlignment
            com.github.mikephil.charting.components.Legend r2 = r0.mLegend
            com.github.mikephil.charting.components.Legend$LegendVerticalAlignment r2 = r2.getVerticalAlignment()
            int r2 = r2.ordinal()
            r1 = r1[r2]
            switch(r1) {
                case 1: goto L_0x0192;
                case 2: goto L_0x0176;
                default: goto L_0x0175;
            }
        L_0x0175:
            goto L_0x01bd
        L_0x0176:
            com.github.mikephil.charting.components.Legend r1 = r0.mLegend
            float r1 = r1.mNeededHeight
            com.github.mikephil.charting.utils.ViewPortHandler r2 = r0.mViewPortHandler
            float r2 = r2.getChartHeight()
            com.github.mikephil.charting.components.Legend r3 = r0.mLegend
            float r3 = r3.getMaxSizePercent()
            float r2 = r2 * r3
            float r1 = java.lang.Math.min(r1, r2)
            r3 = r1
            r1 = r16
            r2 = r17
            goto L_0x01c3
        L_0x0192:
            com.github.mikephil.charting.components.Legend r1 = r0.mLegend
            float r1 = r1.mNeededHeight
            com.github.mikephil.charting.utils.ViewPortHandler r2 = r0.mViewPortHandler
            float r2 = r2.getChartHeight()
            com.github.mikephil.charting.components.Legend r3 = r0.mLegend
            float r3 = r3.getMaxSizePercent()
            float r2 = r2 * r3
            float r1 = java.lang.Math.min(r1, r2)
            r4 = r1
            r1 = r16
            r2 = r17
            r3 = r18
            goto L_0x01c3
        L_0x01b0:
            r1 = r6
            r2 = r1
            r1 = r16
            r3 = r18
            goto L_0x01c3
        L_0x01b7:
            r1 = r6
            r2 = r17
            r3 = r18
            goto L_0x01c3
        L_0x01bd:
            r1 = r16
            r2 = r17
            r3 = r18
        L_0x01c3:
            goto L_0x01ca
        L_0x01c4:
            r1 = r16
            r2 = r17
            r3 = r18
        L_0x01ca:
            float r6 = r19.getRequiredBaseOffset()
            float r1 = r1 + r6
            float r6 = r19.getRequiredBaseOffset()
            float r2 = r2 + r6
            float r6 = r19.getRequiredBaseOffset()
            float r4 = r4 + r6
            float r6 = r19.getRequiredBaseOffset()
            float r3 = r3 + r6
            goto L_0x01eb
        L_0x01df:
            r16 = r1
            r17 = r2
            r18 = r3
            r1 = r16
            r2 = r17
            r3 = r18
        L_0x01eb:
            float r5 = r0.mMinOffset
            float r5 = com.github.mikephil.charting.utils.Utils.convertDpToPixel(r5)
            boolean r6 = r0 instanceof com.github.mikephil.charting.charts.RadarChart
            if (r6 == 0) goto L_0x020c
            com.github.mikephil.charting.components.XAxis r6 = r19.getXAxis()
            boolean r7 = r6.isEnabled()
            if (r7 == 0) goto L_0x020c
            boolean r7 = r6.isDrawLabelsEnabled()
            if (r7 == 0) goto L_0x020c
            int r7 = r6.mLabelRotatedWidth
            float r7 = (float) r7
            float r5 = java.lang.Math.max(r5, r7)
        L_0x020c:
            float r6 = r19.getExtraTopOffset()
            float r4 = r4 + r6
            float r6 = r19.getExtraRightOffset()
            float r2 = r2 + r6
            float r6 = r19.getExtraBottomOffset()
            float r3 = r3 + r6
            float r6 = r19.getExtraLeftOffset()
            float r1 = r1 + r6
            float r6 = java.lang.Math.max(r5, r1)
            float r7 = java.lang.Math.max(r5, r4)
            float r8 = java.lang.Math.max(r5, r2)
            float r9 = r19.getRequiredBaseOffset()
            float r9 = java.lang.Math.max(r9, r3)
            float r9 = java.lang.Math.max(r5, r9)
            com.github.mikephil.charting.utils.ViewPortHandler r10 = r0.mViewPortHandler
            r10.restrainViewPort(r6, r7, r8, r9)
            boolean r10 = r0.mLogEnabled
            if (r10 == 0) goto L_0x0277
            java.lang.StringBuilder r10 = new java.lang.StringBuilder
            r10.<init>()
            java.lang.String r11 = "offsetLeft: "
            java.lang.StringBuilder r10 = r10.append(r11)
            java.lang.StringBuilder r10 = r10.append(r6)
            java.lang.String r11 = ", offsetTop: "
            java.lang.StringBuilder r10 = r10.append(r11)
            java.lang.StringBuilder r10 = r10.append(r7)
            java.lang.String r11 = ", offsetRight: "
            java.lang.StringBuilder r10 = r10.append(r11)
            java.lang.StringBuilder r10 = r10.append(r8)
            java.lang.String r11 = ", offsetBottom: "
            java.lang.StringBuilder r10 = r10.append(r11)
            java.lang.StringBuilder r10 = r10.append(r9)
            java.lang.String r10 = r10.toString()
            java.lang.String r11 = "MPAndroidChart"
            android.util.Log.i(r11, r10)
        L_0x0277:
            return
        */
        throw new UnsupportedOperationException("Method not decompiled: com.github.mikephil.charting.charts.PieRadarChartBase.calculateOffsets():void");
    }

    /* renamed from: com.github.mikephil.charting.charts.PieRadarChartBase$2  reason: invalid class name */
    static /* synthetic */ class AnonymousClass2 {
        static final /* synthetic */ int[] $SwitchMap$com$github$mikephil$charting$components$Legend$LegendHorizontalAlignment;
        static final /* synthetic */ int[] $SwitchMap$com$github$mikephil$charting$components$Legend$LegendOrientation;
        static final /* synthetic */ int[] $SwitchMap$com$github$mikephil$charting$components$Legend$LegendVerticalAlignment;

        static {
            int[] iArr = new int[Legend.LegendOrientation.values().length];
            $SwitchMap$com$github$mikephil$charting$components$Legend$LegendOrientation = iArr;
            try {
                iArr[Legend.LegendOrientation.VERTICAL.ordinal()] = 1;
            } catch (NoSuchFieldError e) {
            }
            try {
                $SwitchMap$com$github$mikephil$charting$components$Legend$LegendOrientation[Legend.LegendOrientation.HORIZONTAL.ordinal()] = 2;
            } catch (NoSuchFieldError e2) {
            }
            int[] iArr2 = new int[Legend.LegendHorizontalAlignment.values().length];
            $SwitchMap$com$github$mikephil$charting$components$Legend$LegendHorizontalAlignment = iArr2;
            try {
                iArr2[Legend.LegendHorizontalAlignment.LEFT.ordinal()] = 1;
            } catch (NoSuchFieldError e3) {
            }
            try {
                $SwitchMap$com$github$mikephil$charting$components$Legend$LegendHorizontalAlignment[Legend.LegendHorizontalAlignment.RIGHT.ordinal()] = 2;
            } catch (NoSuchFieldError e4) {
            }
            try {
                $SwitchMap$com$github$mikephil$charting$components$Legend$LegendHorizontalAlignment[Legend.LegendHorizontalAlignment.CENTER.ordinal()] = 3;
            } catch (NoSuchFieldError e5) {
            }
            int[] iArr3 = new int[Legend.LegendVerticalAlignment.values().length];
            $SwitchMap$com$github$mikephil$charting$components$Legend$LegendVerticalAlignment = iArr3;
            try {
                iArr3[Legend.LegendVerticalAlignment.TOP.ordinal()] = 1;
            } catch (NoSuchFieldError e6) {
            }
            try {
                $SwitchMap$com$github$mikephil$charting$components$Legend$LegendVerticalAlignment[Legend.LegendVerticalAlignment.BOTTOM.ordinal()] = 2;
            } catch (NoSuchFieldError e7) {
            }
        }
    }

    public float getAngleForPoint(float x, float y) {
        MPPointF c = getCenterOffsets();
        double tx = (double) (x - c.x);
        double ty = (double) (y - c.y);
        float angle = (float) Math.toDegrees(Math.acos(ty / Math.sqrt((tx * tx) + (ty * ty))));
        if (x > c.x) {
            angle = 360.0f - angle;
        }
        float angle2 = angle + 90.0f;
        if (angle2 > 360.0f) {
            angle2 -= 360.0f;
        }
        MPPointF.recycleInstance(c);
        return angle2;
    }

    public MPPointF getPosition(MPPointF center, float dist, float angle) {
        MPPointF p = MPPointF.getInstance(0.0f, 0.0f);
        getPosition(center, dist, angle, p);
        return p;
    }

    public void getPosition(MPPointF center, float dist, float angle, MPPointF outputPoint) {
        outputPoint.x = (float) (((double) center.x) + (((double) dist) * Math.cos(Math.toRadians((double) angle))));
        outputPoint.y = (float) (((double) center.y) + (((double) dist) * Math.sin(Math.toRadians((double) angle))));
    }

    public float distanceToCenter(float x, float y) {
        float xDist;
        float yDist;
        MPPointF c = getCenterOffsets();
        if (x > c.x) {
            xDist = x - c.x;
        } else {
            xDist = c.x - x;
        }
        if (y > c.y) {
            yDist = y - c.y;
        } else {
            yDist = c.y - y;
        }
        float dist = (float) Math.sqrt(Math.pow((double) xDist, 2.0d) + Math.pow((double) yDist, 2.0d));
        MPPointF.recycleInstance(c);
        return dist;
    }

    public void setRotationAngle(float angle) {
        this.mRawRotationAngle = angle;
        this.mRotationAngle = Utils.getNormalizedAngle(angle);
    }

    public float getRawRotationAngle() {
        return this.mRawRotationAngle;
    }

    public float getRotationAngle() {
        return this.mRotationAngle;
    }

    public void setRotationEnabled(boolean enabled) {
        this.mRotateEnabled = enabled;
    }

    public boolean isRotationEnabled() {
        return this.mRotateEnabled;
    }

    public float getMinOffset() {
        return this.mMinOffset;
    }

    public void setMinOffset(float minOffset) {
        this.mMinOffset = minOffset;
    }

    public float getDiameter() {
        RectF content = this.mViewPortHandler.getContentRect();
        content.left += getExtraLeftOffset();
        content.top += getExtraTopOffset();
        content.right -= getExtraRightOffset();
        content.bottom -= getExtraBottomOffset();
        return Math.min(content.width(), content.height());
    }

    public float getYChartMax() {
        return 0.0f;
    }

    public float getYChartMin() {
        return 0.0f;
    }

    public void spin(int durationmillis, float fromangle, float toangle, Easing.EasingFunction easing) {
        setRotationAngle(fromangle);
        ObjectAnimator spinAnimator = ObjectAnimator.ofFloat(this, "rotationAngle", new float[]{fromangle, toangle});
        spinAnimator.setDuration((long) durationmillis);
        spinAnimator.setInterpolator(easing);
        spinAnimator.addUpdateListener(new ValueAnimator.AnimatorUpdateListener() {
            public void onAnimationUpdate(ValueAnimator animation) {
                PieRadarChartBase.this.postInvalidate();
            }
        });
        spinAnimator.start();
    }
}
