package com.github.mikephil.charting.renderer;

import android.graphics.Canvas;
import android.graphics.DashPathEffect;
import android.graphics.Paint;
import android.graphics.Path;
import com.github.mikephil.charting.components.Legend;
import com.github.mikephil.charting.components.LegendEntry;
import com.github.mikephil.charting.utils.Utils;
import com.github.mikephil.charting.utils.ViewPortHandler;
import java.util.ArrayList;
import java.util.List;

public class LegendRenderer extends Renderer {
    protected List<LegendEntry> computedEntries = new ArrayList(16);
    protected Paint.FontMetrics legendFontMetrics = new Paint.FontMetrics();
    protected Legend mLegend;
    protected Paint mLegendFormPaint;
    protected Paint mLegendLabelPaint;
    private Path mLineFormPath = new Path();

    public LegendRenderer(ViewPortHandler viewPortHandler, Legend legend) {
        super(viewPortHandler);
        this.mLegend = legend;
        Paint paint = new Paint(1);
        this.mLegendLabelPaint = paint;
        paint.setTextSize(Utils.convertDpToPixel(9.0f));
        this.mLegendLabelPaint.setTextAlign(Paint.Align.LEFT);
        Paint paint2 = new Paint(1);
        this.mLegendFormPaint = paint2;
        paint2.setStyle(Paint.Style.FILL);
    }

    public Paint getLabelPaint() {
        return this.mLegendLabelPaint;
    }

    public Paint getFormPaint() {
        return this.mLegendFormPaint;
    }

    /* JADX DEBUG: Multi-variable search result rejected for TypeSearchVarInfo{r1v0, resolved type: com.github.mikephil.charting.data.ChartData<?>} */
    /* JADX DEBUG: Multi-variable search result rejected for TypeSearchVarInfo{r1v3, resolved type: com.github.mikephil.charting.data.ChartData<?>} */
    /* JADX DEBUG: Multi-variable search result rejected for TypeSearchVarInfo{r1v8, resolved type: com.github.mikephil.charting.data.ChartData<?>} */
    /* JADX DEBUG: Multi-variable search result rejected for TypeSearchVarInfo{r6v9, resolved type: com.github.mikephil.charting.data.ChartData<?>} */
    /* JADX WARNING: type inference failed for: r3v6, types: [com.github.mikephil.charting.interfaces.datasets.IDataSet] */
    /* JADX WARNING: type inference failed for: r7v1, types: [com.github.mikephil.charting.interfaces.datasets.IDataSet] */
    /* JADX WARNING: Multi-variable type inference failed */
    /* Code decompiled incorrectly, please refer to instructions dump. */
    public void computeLegend(com.github.mikephil.charting.data.ChartData<?> r20) {
        /*
            r19 = this;
            r0 = r19
            r1 = r20
            com.github.mikephil.charting.components.Legend r2 = r0.mLegend
            boolean r2 = r2.isLegendCustom()
            if (r2 != 0) goto L_0x01e0
            java.util.List<com.github.mikephil.charting.components.LegendEntry> r2 = r0.computedEntries
            r2.clear()
            r2 = 0
        L_0x0012:
            int r3 = r20.getDataSetCount()
            if (r2 >= r3) goto L_0x01c4
            com.github.mikephil.charting.interfaces.datasets.IDataSet r3 = r1.getDataSetByIndex(r2)
            java.util.List r4 = r3.getColors()
            int r5 = r3.getEntryCount()
            boolean r6 = r3 instanceof com.github.mikephil.charting.interfaces.datasets.IBarDataSet
            if (r6 == 0) goto L_0x009f
            r6 = r3
            com.github.mikephil.charting.interfaces.datasets.IBarDataSet r6 = (com.github.mikephil.charting.interfaces.datasets.IBarDataSet) r6
            boolean r6 = r6.isStacked()
            if (r6 == 0) goto L_0x009f
            r6 = r3
            com.github.mikephil.charting.interfaces.datasets.IBarDataSet r6 = (com.github.mikephil.charting.interfaces.datasets.IBarDataSet) r6
            java.lang.String[] r7 = r6.getStackLabels()
            r8 = 0
        L_0x0039:
            int r9 = r4.size()
            if (r8 >= r9) goto L_0x007b
            int r9 = r6.getStackSize()
            if (r8 >= r9) goto L_0x007b
            java.util.List<com.github.mikephil.charting.components.LegendEntry> r9 = r0.computedEntries
            com.github.mikephil.charting.components.LegendEntry r15 = new com.github.mikephil.charting.components.LegendEntry
            int r10 = r7.length
            int r10 = r8 % r10
            r11 = r7[r10]
            com.github.mikephil.charting.components.Legend$LegendForm r12 = r3.getForm()
            float r13 = r3.getFormSize()
            float r14 = r3.getFormLineWidth()
            android.graphics.DashPathEffect r16 = r3.getFormLineDashEffect()
            java.lang.Object r10 = r4.get(r8)
            java.lang.Integer r10 = (java.lang.Integer) r10
            int r17 = r10.intValue()
            r10 = r15
            r18 = r7
            r7 = r15
            r15 = r16
            r16 = r17
            r10.<init>(r11, r12, r13, r14, r15, r16)
            r9.add(r7)
            int r8 = r8 + 1
            r7 = r18
            goto L_0x0039
        L_0x007b:
            r18 = r7
            java.lang.String r7 = r6.getLabel()
            if (r7 == 0) goto L_0x009c
            java.util.List<com.github.mikephil.charting.components.LegendEntry> r7 = r0.computedEntries
            com.github.mikephil.charting.components.LegendEntry r15 = new com.github.mikephil.charting.components.LegendEntry
            java.lang.String r9 = r3.getLabel()
            com.github.mikephil.charting.components.Legend$LegendForm r10 = com.github.mikephil.charting.components.Legend.LegendForm.NONE
            r11 = 2143289344(0x7fc00000, float:NaN)
            r12 = 2143289344(0x7fc00000, float:NaN)
            r13 = 0
            r14 = 1122867(0x112233, float:1.573472E-39)
            r8 = r15
            r8.<init>(r9, r10, r11, r12, r13, r14)
            r7.add(r15)
        L_0x009c:
            r6 = r1
            goto L_0x01bf
        L_0x009f:
            boolean r6 = r3 instanceof com.github.mikephil.charting.interfaces.datasets.IPieDataSet
            if (r6 == 0) goto L_0x0109
            r6 = r3
            com.github.mikephil.charting.interfaces.datasets.IPieDataSet r6 = (com.github.mikephil.charting.interfaces.datasets.IPieDataSet) r6
            r7 = 0
        L_0x00a7:
            int r8 = r4.size()
            if (r7 >= r8) goto L_0x00e6
            if (r7 >= r5) goto L_0x00e6
            java.util.List<com.github.mikephil.charting.components.LegendEntry> r8 = r0.computedEntries
            com.github.mikephil.charting.components.LegendEntry r15 = new com.github.mikephil.charting.components.LegendEntry
            com.github.mikephil.charting.data.Entry r9 = r6.getEntryForIndex(r7)
            com.github.mikephil.charting.data.PieEntry r9 = (com.github.mikephil.charting.data.PieEntry) r9
            java.lang.String r10 = r9.getLabel()
            com.github.mikephil.charting.components.Legend$LegendForm r11 = r3.getForm()
            float r12 = r3.getFormSize()
            float r13 = r3.getFormLineWidth()
            android.graphics.DashPathEffect r14 = r3.getFormLineDashEffect()
            java.lang.Object r9 = r4.get(r7)
            java.lang.Integer r9 = (java.lang.Integer) r9
            int r16 = r9.intValue()
            r9 = r15
            r1 = r15
            r15 = r16
            r9.<init>(r10, r11, r12, r13, r14, r15)
            r8.add(r1)
            int r7 = r7 + 1
            r1 = r20
            goto L_0x00a7
        L_0x00e6:
            java.lang.String r1 = r6.getLabel()
            if (r1 == 0) goto L_0x0105
            java.util.List<com.github.mikephil.charting.components.LegendEntry> r1 = r0.computedEntries
            com.github.mikephil.charting.components.LegendEntry r14 = new com.github.mikephil.charting.components.LegendEntry
            java.lang.String r8 = r3.getLabel()
            com.github.mikephil.charting.components.Legend$LegendForm r9 = com.github.mikephil.charting.components.Legend.LegendForm.NONE
            r10 = 2143289344(0x7fc00000, float:NaN)
            r11 = 2143289344(0x7fc00000, float:NaN)
            r12 = 0
            r13 = 1122867(0x112233, float:1.573472E-39)
            r7 = r14
            r7.<init>(r8, r9, r10, r11, r12, r13)
            r1.add(r14)
        L_0x0105:
            r6 = r20
            goto L_0x01bf
        L_0x0109:
            boolean r1 = r3 instanceof com.github.mikephil.charting.interfaces.datasets.ICandleDataSet
            if (r1 == 0) goto L_0x0167
            r1 = r3
            com.github.mikephil.charting.interfaces.datasets.ICandleDataSet r1 = (com.github.mikephil.charting.interfaces.datasets.ICandleDataSet) r1
            int r1 = r1.getDecreasingColor()
            r6 = 1122867(0x112233, float:1.573472E-39)
            if (r1 == r6) goto L_0x0167
            r1 = r3
            com.github.mikephil.charting.interfaces.datasets.ICandleDataSet r1 = (com.github.mikephil.charting.interfaces.datasets.ICandleDataSet) r1
            int r1 = r1.getDecreasingColor()
            r6 = r3
            com.github.mikephil.charting.interfaces.datasets.ICandleDataSet r6 = (com.github.mikephil.charting.interfaces.datasets.ICandleDataSet) r6
            int r14 = r6.getIncreasingColor()
            java.util.List<com.github.mikephil.charting.components.LegendEntry> r13 = r0.computedEntries
            com.github.mikephil.charting.components.LegendEntry r15 = new com.github.mikephil.charting.components.LegendEntry
            r7 = 0
            com.github.mikephil.charting.components.Legend$LegendForm r8 = r3.getForm()
            float r9 = r3.getFormSize()
            float r10 = r3.getFormLineWidth()
            android.graphics.DashPathEffect r11 = r3.getFormLineDashEffect()
            r6 = r15
            r12 = r1
            r6.<init>(r7, r8, r9, r10, r11, r12)
            r13.add(r15)
            java.util.List<com.github.mikephil.charting.components.LegendEntry> r6 = r0.computedEntries
            com.github.mikephil.charting.components.LegendEntry r15 = new com.github.mikephil.charting.components.LegendEntry
            java.lang.String r8 = r3.getLabel()
            com.github.mikephil.charting.components.Legend$LegendForm r9 = r3.getForm()
            float r10 = r3.getFormSize()
            float r11 = r3.getFormLineWidth()
            android.graphics.DashPathEffect r12 = r3.getFormLineDashEffect()
            r7 = r15
            r13 = r14
            r7.<init>(r8, r9, r10, r11, r12, r13)
            r6.add(r15)
            r6 = r20
            goto L_0x01bf
        L_0x0167:
            r1 = 0
        L_0x0168:
            int r6 = r4.size()
            if (r1 >= r6) goto L_0x01bb
            if (r1 >= r5) goto L_0x01bb
            int r6 = r4.size()
            int r6 = r6 + -1
            if (r1 >= r6) goto L_0x0181
            int r6 = r5 + -1
            if (r1 >= r6) goto L_0x0181
            r6 = 0
            r7 = r6
            r6 = r20
            goto L_0x018b
        L_0x0181:
            r6 = r20
            com.github.mikephil.charting.interfaces.datasets.IDataSet r7 = r6.getDataSetByIndex(r2)
            java.lang.String r7 = r7.getLabel()
        L_0x018b:
            java.util.List<com.github.mikephil.charting.components.LegendEntry> r15 = r0.computedEntries
            com.github.mikephil.charting.components.LegendEntry r14 = new com.github.mikephil.charting.components.LegendEntry
            com.github.mikephil.charting.components.Legend$LegendForm r10 = r3.getForm()
            float r11 = r3.getFormSize()
            float r12 = r3.getFormLineWidth()
            android.graphics.DashPathEffect r13 = r3.getFormLineDashEffect()
            java.lang.Object r8 = r4.get(r1)
            java.lang.Integer r8 = (java.lang.Integer) r8
            int r16 = r8.intValue()
            r8 = r14
            r9 = r7
            r17 = r3
            r3 = r14
            r14 = r16
            r8.<init>(r9, r10, r11, r12, r13, r14)
            r15.add(r3)
            int r1 = r1 + 1
            r3 = r17
            goto L_0x0168
        L_0x01bb:
            r6 = r20
            r17 = r3
        L_0x01bf:
            int r2 = r2 + 1
            r1 = r6
            goto L_0x0012
        L_0x01c4:
            r6 = r1
            com.github.mikephil.charting.components.Legend r1 = r0.mLegend
            com.github.mikephil.charting.components.LegendEntry[] r1 = r1.getExtraEntries()
            if (r1 == 0) goto L_0x01d8
            java.util.List<com.github.mikephil.charting.components.LegendEntry> r1 = r0.computedEntries
            com.github.mikephil.charting.components.Legend r2 = r0.mLegend
            com.github.mikephil.charting.components.LegendEntry[] r2 = r2.getExtraEntries()
            java.util.Collections.addAll(r1, r2)
        L_0x01d8:
            com.github.mikephil.charting.components.Legend r1 = r0.mLegend
            java.util.List<com.github.mikephil.charting.components.LegendEntry> r2 = r0.computedEntries
            r1.setEntries(r2)
            goto L_0x01e1
        L_0x01e0:
            r6 = r1
        L_0x01e1:
            com.github.mikephil.charting.components.Legend r1 = r0.mLegend
            android.graphics.Typeface r1 = r1.getTypeface()
            if (r1 == 0) goto L_0x01ee
            android.graphics.Paint r2 = r0.mLegendLabelPaint
            r2.setTypeface(r1)
        L_0x01ee:
            android.graphics.Paint r2 = r0.mLegendLabelPaint
            com.github.mikephil.charting.components.Legend r3 = r0.mLegend
            float r3 = r3.getTextSize()
            r2.setTextSize(r3)
            android.graphics.Paint r2 = r0.mLegendLabelPaint
            com.github.mikephil.charting.components.Legend r3 = r0.mLegend
            int r3 = r3.getTextColor()
            r2.setColor(r3)
            com.github.mikephil.charting.components.Legend r2 = r0.mLegend
            android.graphics.Paint r3 = r0.mLegendLabelPaint
            com.github.mikephil.charting.utils.ViewPortHandler r4 = r0.mViewPortHandler
            r2.calculateDimensions(r3, r4)
            return
        */
        throw new UnsupportedOperationException("Method not decompiled: com.github.mikephil.charting.renderer.LegendRenderer.computeLegend(com.github.mikephil.charting.data.ChartData):void");
    }

    /* JADX WARNING: Removed duplicated region for block: B:139:0x03b5  */
    /* JADX WARNING: Removed duplicated region for block: B:140:0x03b7  */
    /* JADX WARNING: Removed duplicated region for block: B:143:0x03bc  */
    /* JADX WARNING: Removed duplicated region for block: B:151:0x03f1  */
    /* JADX WARNING: Removed duplicated region for block: B:153:0x03fe  */
    /* JADX WARNING: Removed duplicated region for block: B:169:0x0438  */
    /* Code decompiled incorrectly, please refer to instructions dump. */
    public void renderLegend(android.graphics.Canvas r41) {
        /*
            r40 = this;
            r6 = r40
            r7 = r41
            com.github.mikephil.charting.components.Legend r0 = r6.mLegend
            boolean r0 = r0.isEnabled()
            if (r0 != 0) goto L_0x000d
            return
        L_0x000d:
            com.github.mikephil.charting.components.Legend r0 = r6.mLegend
            android.graphics.Typeface r8 = r0.getTypeface()
            if (r8 == 0) goto L_0x001a
            android.graphics.Paint r0 = r6.mLegendLabelPaint
            r0.setTypeface(r8)
        L_0x001a:
            android.graphics.Paint r0 = r6.mLegendLabelPaint
            com.github.mikephil.charting.components.Legend r1 = r6.mLegend
            float r1 = r1.getTextSize()
            r0.setTextSize(r1)
            android.graphics.Paint r0 = r6.mLegendLabelPaint
            com.github.mikephil.charting.components.Legend r1 = r6.mLegend
            int r1 = r1.getTextColor()
            r0.setColor(r1)
            android.graphics.Paint r0 = r6.mLegendLabelPaint
            android.graphics.Paint$FontMetrics r1 = r6.legendFontMetrics
            float r9 = com.github.mikephil.charting.utils.Utils.getLineHeight(r0, r1)
            android.graphics.Paint r0 = r6.mLegendLabelPaint
            android.graphics.Paint$FontMetrics r1 = r6.legendFontMetrics
            float r0 = com.github.mikephil.charting.utils.Utils.getLineSpacing(r0, r1)
            com.github.mikephil.charting.components.Legend r1 = r6.mLegend
            float r1 = r1.getYEntrySpace()
            float r1 = com.github.mikephil.charting.utils.Utils.convertDpToPixel(r1)
            float r10 = r0 + r1
            android.graphics.Paint r0 = r6.mLegendLabelPaint
            java.lang.String r1 = "ABC"
            int r0 = com.github.mikephil.charting.utils.Utils.calcTextHeight(r0, r1)
            float r0 = (float) r0
            r11 = 1073741824(0x40000000, float:2.0)
            float r0 = r0 / r11
            float r12 = r9 - r0
            com.github.mikephil.charting.components.Legend r0 = r6.mLegend
            com.github.mikephil.charting.components.LegendEntry[] r13 = r0.getEntries()
            com.github.mikephil.charting.components.Legend r0 = r6.mLegend
            float r0 = r0.getFormToTextSpace()
            float r14 = com.github.mikephil.charting.utils.Utils.convertDpToPixel(r0)
            com.github.mikephil.charting.components.Legend r0 = r6.mLegend
            float r0 = r0.getXEntrySpace()
            float r15 = com.github.mikephil.charting.utils.Utils.convertDpToPixel(r0)
            com.github.mikephil.charting.components.Legend r0 = r6.mLegend
            com.github.mikephil.charting.components.Legend$LegendOrientation r5 = r0.getOrientation()
            com.github.mikephil.charting.components.Legend r0 = r6.mLegend
            com.github.mikephil.charting.components.Legend$LegendHorizontalAlignment r4 = r0.getHorizontalAlignment()
            com.github.mikephil.charting.components.Legend r0 = r6.mLegend
            com.github.mikephil.charting.components.Legend$LegendVerticalAlignment r16 = r0.getVerticalAlignment()
            com.github.mikephil.charting.components.Legend r0 = r6.mLegend
            com.github.mikephil.charting.components.Legend$LegendDirection r3 = r0.getDirection()
            com.github.mikephil.charting.components.Legend r0 = r6.mLegend
            float r0 = r0.getFormSize()
            float r17 = com.github.mikephil.charting.utils.Utils.convertDpToPixel(r0)
            com.github.mikephil.charting.components.Legend r0 = r6.mLegend
            float r0 = r0.getStackSpace()
            float r2 = com.github.mikephil.charting.utils.Utils.convertDpToPixel(r0)
            com.github.mikephil.charting.components.Legend r0 = r6.mLegend
            float r18 = r0.getYOffset()
            com.github.mikephil.charting.components.Legend r0 = r6.mLegend
            float r1 = r0.getXOffset()
            r0 = 0
            int[] r19 = com.github.mikephil.charting.renderer.LegendRenderer.AnonymousClass1.$SwitchMap$com$github$mikephil$charting$components$Legend$LegendHorizontalAlignment
            int r20 = r4.ordinal()
            r19 = r19[r20]
            switch(r19) {
                case 1: goto L_0x015a;
                case 2: goto L_0x012e;
                case 3: goto L_0x00c4;
                default: goto L_0x00b8;
            }
        L_0x00b8:
            r20 = r0
            r24 = r8
            r21 = r10
            r25 = r14
            r26 = r15
            goto L_0x017f
        L_0x00c4:
            com.github.mikephil.charting.components.Legend$LegendOrientation r11 = com.github.mikephil.charting.components.Legend.LegendOrientation.VERTICAL
            if (r5 != r11) goto L_0x00d3
            com.github.mikephil.charting.utils.ViewPortHandler r11 = r6.mViewPortHandler
            float r11 = r11.getChartWidth()
            r19 = 1073741824(0x40000000, float:2.0)
            float r11 = r11 / r19
            goto L_0x00e6
        L_0x00d3:
            r19 = 1073741824(0x40000000, float:2.0)
            com.github.mikephil.charting.utils.ViewPortHandler r11 = r6.mViewPortHandler
            float r11 = r11.contentLeft()
            r20 = r0
            com.github.mikephil.charting.utils.ViewPortHandler r0 = r6.mViewPortHandler
            float r0 = r0.contentWidth()
            float r0 = r0 / r19
            float r11 = r11 + r0
        L_0x00e6:
            com.github.mikephil.charting.components.Legend$LegendDirection r0 = com.github.mikephil.charting.components.Legend.LegendDirection.LEFT_TO_RIGHT
            if (r3 != r0) goto L_0x00ec
            r0 = r1
            goto L_0x00ed
        L_0x00ec:
            float r0 = -r1
        L_0x00ed:
            float r0 = r0 + r11
            com.github.mikephil.charting.components.Legend$LegendOrientation r11 = com.github.mikephil.charting.components.Legend.LegendOrientation.VERTICAL
            if (r5 != r11) goto L_0x0123
            r21 = r10
            double r10 = (double) r0
            r20 = r0
            com.github.mikephil.charting.components.Legend$LegendDirection r0 = com.github.mikephil.charting.components.Legend.LegendDirection.LEFT_TO_RIGHT
            r22 = 4611686018427387904(0x4000000000000000, double:2.0)
            if (r3 != r0) goto L_0x010e
            com.github.mikephil.charting.components.Legend r0 = r6.mLegend
            float r0 = r0.mNeededWidth
            float r0 = -r0
            r24 = r8
            double r7 = (double) r0
            double r7 = r7 / r22
            r25 = r14
            r26 = r15
            double r14 = (double) r1
            double r7 = r7 + r14
            goto L_0x011d
        L_0x010e:
            r24 = r8
            r25 = r14
            r26 = r15
            com.github.mikephil.charting.components.Legend r0 = r6.mLegend
            float r0 = r0.mNeededWidth
            double r7 = (double) r0
            double r7 = r7 / r22
            double r14 = (double) r1
            double r7 = r7 - r14
        L_0x011d:
            double r10 = r10 + r7
            float r0 = (float) r10
            r20 = r0
            goto L_0x017f
        L_0x0123:
            r20 = r0
            r24 = r8
            r21 = r10
            r25 = r14
            r26 = r15
            goto L_0x017f
        L_0x012e:
            r20 = r0
            r24 = r8
            r21 = r10
            r25 = r14
            r26 = r15
            com.github.mikephil.charting.components.Legend$LegendOrientation r0 = com.github.mikephil.charting.components.Legend.LegendOrientation.VERTICAL
            if (r5 != r0) goto L_0x0144
            com.github.mikephil.charting.utils.ViewPortHandler r0 = r6.mViewPortHandler
            float r0 = r0.getChartWidth()
            float r0 = r0 - r1
            goto L_0x014b
        L_0x0144:
            com.github.mikephil.charting.utils.ViewPortHandler r0 = r6.mViewPortHandler
            float r0 = r0.contentRight()
            float r0 = r0 - r1
        L_0x014b:
            com.github.mikephil.charting.components.Legend$LegendDirection r7 = com.github.mikephil.charting.components.Legend.LegendDirection.LEFT_TO_RIGHT
            if (r3 != r7) goto L_0x0157
            com.github.mikephil.charting.components.Legend r7 = r6.mLegend
            float r7 = r7.mNeededWidth
            float r0 = r0 - r7
            r20 = r0
            goto L_0x017f
        L_0x0157:
            r20 = r0
            goto L_0x017f
        L_0x015a:
            r20 = r0
            r24 = r8
            r21 = r10
            r25 = r14
            r26 = r15
            com.github.mikephil.charting.components.Legend$LegendOrientation r0 = com.github.mikephil.charting.components.Legend.LegendOrientation.VERTICAL
            if (r5 != r0) goto L_0x016a
            r0 = r1
            goto L_0x0171
        L_0x016a:
            com.github.mikephil.charting.utils.ViewPortHandler r0 = r6.mViewPortHandler
            float r0 = r0.contentLeft()
            float r0 = r0 + r1
        L_0x0171:
            com.github.mikephil.charting.components.Legend$LegendDirection r7 = com.github.mikephil.charting.components.Legend.LegendDirection.RIGHT_TO_LEFT
            if (r3 != r7) goto L_0x017d
            com.github.mikephil.charting.components.Legend r7 = r6.mLegend
            float r7 = r7.mNeededWidth
            float r0 = r0 + r7
            r20 = r0
            goto L_0x017f
        L_0x017d:
            r20 = r0
        L_0x017f:
            int[] r0 = com.github.mikephil.charting.renderer.LegendRenderer.AnonymousClass1.$SwitchMap$com$github$mikephil$charting$components$Legend$LegendOrientation
            int r7 = r5.ordinal()
            r0 = r0[r7]
            switch(r0) {
                case 1: goto L_0x02d9;
                case 2: goto L_0x019d;
                default: goto L_0x018a;
            }
        L_0x018a:
            r7 = r41
            r27 = r1
            r38 = r4
            r29 = r5
            r31 = r12
            r11 = r13
            r10 = r25
            r12 = r2
            r13 = r3
            r3 = r26
            goto L_0x0463
        L_0x019d:
            r0 = 0
            r10 = 0
            r11 = 0
            int[] r14 = com.github.mikephil.charting.renderer.LegendRenderer.AnonymousClass1.$SwitchMap$com$github$mikephil$charting$components$Legend$LegendVerticalAlignment
            int r15 = r16.ordinal()
            r14 = r14[r15]
            switch(r14) {
                case 1: goto L_0x01df;
                case 2: goto L_0x01c4;
                case 3: goto L_0x01ac;
                default: goto L_0x01ab;
            }
        L_0x01ab:
            goto L_0x01ef
        L_0x01ac:
            com.github.mikephil.charting.utils.ViewPortHandler r14 = r6.mViewPortHandler
            float r14 = r14.getChartHeight()
            r15 = 1073741824(0x40000000, float:2.0)
            float r14 = r14 / r15
            com.github.mikephil.charting.components.Legend r7 = r6.mLegend
            float r7 = r7.mNeededHeight
            float r7 = r7 / r15
            float r14 = r14 - r7
            com.github.mikephil.charting.components.Legend r7 = r6.mLegend
            float r7 = r7.getYOffset()
            float r11 = r14 + r7
            goto L_0x01ef
        L_0x01c4:
            com.github.mikephil.charting.components.Legend$LegendHorizontalAlignment r7 = com.github.mikephil.charting.components.Legend.LegendHorizontalAlignment.CENTER
            if (r4 != r7) goto L_0x01cf
            com.github.mikephil.charting.utils.ViewPortHandler r7 = r6.mViewPortHandler
            float r7 = r7.getChartHeight()
            goto L_0x01d5
        L_0x01cf:
            com.github.mikephil.charting.utils.ViewPortHandler r7 = r6.mViewPortHandler
            float r7 = r7.contentBottom()
        L_0x01d5:
            com.github.mikephil.charting.components.Legend r11 = r6.mLegend
            float r11 = r11.mNeededHeight
            float r11 = r11 + r18
            float r11 = r7 - r11
            goto L_0x01ef
        L_0x01df:
            com.github.mikephil.charting.components.Legend$LegendHorizontalAlignment r7 = com.github.mikephil.charting.components.Legend.LegendHorizontalAlignment.CENTER
            if (r4 != r7) goto L_0x01e5
            r7 = 0
            goto L_0x01eb
        L_0x01e5:
            com.github.mikephil.charting.utils.ViewPortHandler r7 = r6.mViewPortHandler
            float r7 = r7.contentTop()
        L_0x01eb:
            float r11 = r7 + r18
        L_0x01ef:
            r7 = 0
            r14 = r11
            r11 = r10
            r10 = r7
            r7 = r0
        L_0x01f4:
            int r0 = r13.length
            if (r10 >= r0) goto L_0x02bf
            r15 = r13[r10]
            com.github.mikephil.charting.components.Legend$LegendForm r0 = r15.form
            com.github.mikephil.charting.components.Legend$LegendForm r8 = com.github.mikephil.charting.components.Legend.LegendForm.NONE
            if (r0 == r8) goto L_0x0201
            r0 = 1
            goto L_0x0202
        L_0x0201:
            r0 = 0
        L_0x0202:
            r8 = r0
            float r0 = r15.formSize
            boolean r0 = java.lang.Float.isNaN(r0)
            if (r0 == 0) goto L_0x020e
            r0 = r17
            goto L_0x0214
        L_0x020e:
            float r0 = r15.formSize
            float r0 = com.github.mikephil.charting.utils.Utils.convertDpToPixel(r0)
        L_0x0214:
            r19 = r0
            r0 = r20
            if (r8 == 0) goto L_0x0251
            r27 = r1
            com.github.mikephil.charting.components.Legend$LegendDirection r1 = com.github.mikephil.charting.components.Legend.LegendDirection.LEFT_TO_RIGHT
            if (r3 != r1) goto L_0x0224
            float r0 = r0 + r7
            r28 = r0
            goto L_0x0229
        L_0x0224:
            float r1 = r19 - r7
            float r0 = r0 - r1
            r28 = r0
        L_0x0229:
            float r29 = r14 + r12
            com.github.mikephil.charting.components.Legend r1 = r6.mLegend
            r0 = r40
            r30 = r1
            r1 = r41
            r31 = r12
            r12 = r2
            r2 = r28
            r32 = r13
            r13 = r3
            r3 = r29
            r33 = r4
            r4 = r15
            r29 = r5
            r5 = r30
            r0.drawForm(r1, r2, r3, r4, r5)
            com.github.mikephil.charting.components.Legend$LegendDirection r0 = com.github.mikephil.charting.components.Legend.LegendDirection.LEFT_TO_RIGHT
            if (r13 != r0) goto L_0x024e
            float r0 = r28 + r19
            goto L_0x025d
        L_0x024e:
            r0 = r28
            goto L_0x025d
        L_0x0251:
            r27 = r1
            r33 = r4
            r29 = r5
            r31 = r12
            r32 = r13
            r12 = r2
            r13 = r3
        L_0x025d:
            java.lang.String r1 = r15.label
            if (r1 == 0) goto L_0x02a4
            if (r8 == 0) goto L_0x0272
            if (r11 != 0) goto L_0x0272
            com.github.mikephil.charting.components.Legend$LegendDirection r1 = com.github.mikephil.charting.components.Legend.LegendDirection.LEFT_TO_RIGHT
            if (r13 != r1) goto L_0x026d
            r1 = r25
            r5 = r1
            goto L_0x0270
        L_0x026d:
            r5 = r25
            float r1 = -r5
        L_0x0270:
            float r0 = r0 + r1
            goto L_0x0278
        L_0x0272:
            r5 = r25
            if (r11 == 0) goto L_0x0278
            r0 = r20
        L_0x0278:
            com.github.mikephil.charting.components.Legend$LegendDirection r1 = com.github.mikephil.charting.components.Legend.LegendDirection.RIGHT_TO_LEFT
            if (r13 != r1) goto L_0x0286
            android.graphics.Paint r1 = r6.mLegendLabelPaint
            java.lang.String r2 = r15.label
            int r1 = com.github.mikephil.charting.utils.Utils.calcTextWidth(r1, r2)
            float r1 = (float) r1
            float r0 = r0 - r1
        L_0x0286:
            if (r11 != 0) goto L_0x0292
            float r1 = r14 + r9
            java.lang.String r2 = r15.label
            r4 = r41
            r6.drawLabel(r4, r0, r1, r2)
            goto L_0x029e
        L_0x0292:
            r4 = r41
            float r1 = r9 + r21
            float r14 = r14 + r1
            float r1 = r14 + r9
            java.lang.String r2 = r15.label
            r6.drawLabel(r4, r0, r1, r2)
        L_0x029e:
            float r1 = r9 + r21
            float r14 = r14 + r1
            r1 = 0
            r7 = r1
            goto L_0x02ad
        L_0x02a4:
            r4 = r41
            r5 = r25
            float r2 = r19 + r12
            float r7 = r7 + r2
            r1 = 1
            r11 = r1
        L_0x02ad:
            int r10 = r10 + 1
            r25 = r5
            r2 = r12
            r3 = r13
            r1 = r27
            r5 = r29
            r12 = r31
            r13 = r32
            r4 = r33
            goto L_0x01f4
        L_0x02bf:
            r27 = r1
            r33 = r4
            r29 = r5
            r31 = r12
            r32 = r13
            r5 = r25
            r4 = r41
            r12 = r2
            r13 = r3
            r7 = r4
            r10 = r5
            r3 = r26
            r11 = r32
            r38 = r33
            goto L_0x0463
        L_0x02d9:
            r27 = r1
            r33 = r4
            r29 = r5
            r31 = r12
            r32 = r13
            r5 = r25
            r4 = r41
            r12 = r2
            r13 = r3
            com.github.mikephil.charting.components.Legend r0 = r6.mLegend
            java.util.List r7 = r0.getCalculatedLineSizes()
            com.github.mikephil.charting.components.Legend r0 = r6.mLegend
            java.util.List r8 = r0.getCalculatedLabelSizes()
            com.github.mikephil.charting.components.Legend r0 = r6.mLegend
            java.util.List r10 = r0.getCalculatedLabelBreakPoints()
            r0 = r20
            r1 = 0
            int[] r2 = com.github.mikephil.charting.renderer.LegendRenderer.AnonymousClass1.$SwitchMap$com$github$mikephil$charting$components$Legend$LegendVerticalAlignment
            int r3 = r16.ordinal()
            r2 = r2[r3]
            switch(r2) {
                case 1: goto L_0x032a;
                case 2: goto L_0x031b;
                case 3: goto L_0x030a;
                default: goto L_0x0309;
            }
        L_0x0309:
            goto L_0x032d
        L_0x030a:
            com.github.mikephil.charting.utils.ViewPortHandler r2 = r6.mViewPortHandler
            float r2 = r2.getChartHeight()
            com.github.mikephil.charting.components.Legend r3 = r6.mLegend
            float r3 = r3.mNeededHeight
            float r2 = r2 - r3
            r3 = 1073741824(0x40000000, float:2.0)
            float r2 = r2 / r3
            float r1 = r2 + r18
            goto L_0x032d
        L_0x031b:
            com.github.mikephil.charting.utils.ViewPortHandler r2 = r6.mViewPortHandler
            float r2 = r2.getChartHeight()
            float r2 = r2 - r18
            com.github.mikephil.charting.components.Legend r3 = r6.mLegend
            float r3 = r3.mNeededHeight
            float r1 = r2 - r3
            goto L_0x032d
        L_0x032a:
            r1 = r18
        L_0x032d:
            r2 = 0
            r3 = 0
            r11 = r32
            int r14 = r11.length
            r15 = r3
        L_0x0333:
            if (r15 >= r14) goto L_0x0456
            r3 = r11[r15]
            r25 = r0
            com.github.mikephil.charting.components.Legend$LegendForm r0 = r3.form
            com.github.mikephil.charting.components.Legend$LegendForm r4 = com.github.mikephil.charting.components.Legend.LegendForm.NONE
            if (r0 == r4) goto L_0x0341
            r0 = 1
            goto L_0x0342
        L_0x0341:
            r0 = 0
        L_0x0342:
            r28 = r0
            float r0 = r3.formSize
            boolean r0 = java.lang.Float.isNaN(r0)
            if (r0 == 0) goto L_0x034f
            r0 = r17
            goto L_0x0355
        L_0x034f:
            float r0 = r3.formSize
            float r0 = com.github.mikephil.charting.utils.Utils.convertDpToPixel(r0)
        L_0x0355:
            r30 = r0
            int r0 = r10.size()
            if (r15 >= r0) goto L_0x0371
            java.lang.Object r0 = r10.get(r15)
            java.lang.Boolean r0 = (java.lang.Boolean) r0
            boolean r0 = r0.booleanValue()
            if (r0 == 0) goto L_0x0371
            r0 = r20
            float r4 = r9 + r21
            float r1 = r1 + r4
            r25 = r1
            goto L_0x0375
        L_0x0371:
            r0 = r25
            r25 = r1
        L_0x0375:
            int r1 = (r0 > r20 ? 1 : (r0 == r20 ? 0 : -1))
            if (r1 != 0) goto L_0x03ab
            com.github.mikephil.charting.components.Legend$LegendHorizontalAlignment r1 = com.github.mikephil.charting.components.Legend.LegendHorizontalAlignment.CENTER
            r4 = r33
            if (r4 != r1) goto L_0x03a8
            int r1 = r7.size()
            if (r2 >= r1) goto L_0x03a5
            com.github.mikephil.charting.components.Legend$LegendDirection r1 = com.github.mikephil.charting.components.Legend.LegendDirection.RIGHT_TO_LEFT
            if (r13 != r1) goto L_0x0392
            java.lang.Object r1 = r7.get(r2)
            com.github.mikephil.charting.utils.FSize r1 = (com.github.mikephil.charting.utils.FSize) r1
            float r1 = r1.width
            goto L_0x039b
        L_0x0392:
            java.lang.Object r1 = r7.get(r2)
            com.github.mikephil.charting.utils.FSize r1 = (com.github.mikephil.charting.utils.FSize) r1
            float r1 = r1.width
            float r1 = -r1
        L_0x039b:
            r19 = 1073741824(0x40000000, float:2.0)
            float r1 = r1 / r19
            float r0 = r0 + r1
            int r2 = r2 + 1
            r32 = r2
            goto L_0x03b1
        L_0x03a5:
            r19 = 1073741824(0x40000000, float:2.0)
            goto L_0x03af
        L_0x03a8:
            r19 = 1073741824(0x40000000, float:2.0)
            goto L_0x03af
        L_0x03ab:
            r4 = r33
            r19 = 1073741824(0x40000000, float:2.0)
        L_0x03af:
            r32 = r2
        L_0x03b1:
            java.lang.String r1 = r3.label
            if (r1 != 0) goto L_0x03b7
            r1 = 1
            goto L_0x03b8
        L_0x03b7:
            r1 = 0
        L_0x03b8:
            r33 = r1
            if (r28 == 0) goto L_0x03f1
            com.github.mikephil.charting.components.Legend$LegendDirection r1 = com.github.mikephil.charting.components.Legend.LegendDirection.RIGHT_TO_LEFT
            if (r13 != r1) goto L_0x03c5
            float r0 = r0 - r30
            r34 = r0
            goto L_0x03c7
        L_0x03c5:
            r34 = r0
        L_0x03c7:
            float r35 = r25 + r31
            com.github.mikephil.charting.components.Legend r2 = r6.mLegend
            r0 = r40
            r1 = r41
            r36 = r2
            r2 = r34
            r37 = r3
            r3 = r35
            r38 = r4
            r35 = r7
            r7 = r41
            r4 = r37
            r39 = r10
            r10 = r5
            r5 = r36
            r0.drawForm(r1, r2, r3, r4, r5)
            com.github.mikephil.charting.components.Legend$LegendDirection r0 = com.github.mikephil.charting.components.Legend.LegendDirection.LEFT_TO_RIGHT
            if (r13 != r0) goto L_0x03ee
            float r0 = r34 + r30
            goto L_0x03fc
        L_0x03ee:
            r0 = r34
            goto L_0x03fc
        L_0x03f1:
            r37 = r3
            r38 = r4
            r35 = r7
            r39 = r10
            r7 = r41
            r10 = r5
        L_0x03fc:
            if (r33 != 0) goto L_0x0438
            if (r28 == 0) goto L_0x0408
            com.github.mikephil.charting.components.Legend$LegendDirection r1 = com.github.mikephil.charting.components.Legend.LegendDirection.RIGHT_TO_LEFT
            if (r13 != r1) goto L_0x0406
            float r5 = -r10
            goto L_0x0407
        L_0x0406:
            r5 = r10
        L_0x0407:
            float r0 = r0 + r5
        L_0x0408:
            com.github.mikephil.charting.components.Legend$LegendDirection r1 = com.github.mikephil.charting.components.Legend.LegendDirection.RIGHT_TO_LEFT
            if (r13 != r1) goto L_0x0415
            java.lang.Object r1 = r8.get(r15)
            com.github.mikephil.charting.utils.FSize r1 = (com.github.mikephil.charting.utils.FSize) r1
            float r1 = r1.width
            float r0 = r0 - r1
        L_0x0415:
            float r1 = r25 + r9
            r2 = r37
            java.lang.String r3 = r2.label
            r6.drawLabel(r7, r0, r1, r3)
            com.github.mikephil.charting.components.Legend$LegendDirection r1 = com.github.mikephil.charting.components.Legend.LegendDirection.LEFT_TO_RIGHT
            if (r13 != r1) goto L_0x042b
            java.lang.Object r1 = r8.get(r15)
            com.github.mikephil.charting.utils.FSize r1 = (com.github.mikephil.charting.utils.FSize) r1
            float r1 = r1.width
            float r0 = r0 + r1
        L_0x042b:
            com.github.mikephil.charting.components.Legend$LegendDirection r1 = com.github.mikephil.charting.components.Legend.LegendDirection.RIGHT_TO_LEFT
            if (r13 != r1) goto L_0x0433
            r3 = r26
            float r1 = -r3
            goto L_0x0436
        L_0x0433:
            r3 = r26
            r1 = r3
        L_0x0436:
            float r0 = r0 + r1
            goto L_0x0444
        L_0x0438:
            r3 = r26
            r2 = r37
            com.github.mikephil.charting.components.Legend$LegendDirection r1 = com.github.mikephil.charting.components.Legend.LegendDirection.RIGHT_TO_LEFT
            if (r13 != r1) goto L_0x0442
            float r1 = -r12
            goto L_0x0443
        L_0x0442:
            r1 = r12
        L_0x0443:
            float r0 = r0 + r1
        L_0x0444:
            int r15 = r15 + 1
            r26 = r3
            r4 = r7
            r5 = r10
            r1 = r25
            r2 = r32
            r7 = r35
            r33 = r38
            r10 = r39
            goto L_0x0333
        L_0x0456:
            r25 = r0
            r35 = r7
            r39 = r10
            r3 = r26
            r38 = r33
            r7 = r4
            r10 = r5
        L_0x0463:
            return
        */
        throw new UnsupportedOperationException("Method not decompiled: com.github.mikephil.charting.renderer.LegendRenderer.renderLegend(android.graphics.Canvas):void");
    }

    /* access modifiers changed from: protected */
    public void drawForm(Canvas c, float x, float y, LegendEntry entry, Legend legend) {
        Legend.LegendForm form;
        Canvas canvas = c;
        float f = x;
        float f2 = y;
        LegendEntry legendEntry = entry;
        if (legendEntry.formColor != 1122868 && legendEntry.formColor != 1122867 && legendEntry.formColor != 0) {
            int restoreCount = c.save();
            Legend.LegendForm form2 = legendEntry.form;
            if (form2 == Legend.LegendForm.DEFAULT) {
                form = legend.getForm();
            } else {
                form = form2;
            }
            this.mLegendFormPaint.setColor(legendEntry.formColor);
            float formSize = Utils.convertDpToPixel(Float.isNaN(legendEntry.formSize) ? legend.getFormSize() : legendEntry.formSize);
            float half = formSize / 2.0f;
            switch (AnonymousClass1.$SwitchMap$com$github$mikephil$charting$components$Legend$LegendForm[form.ordinal()]) {
                case 3:
                case 4:
                    this.mLegendFormPaint.setStyle(Paint.Style.FILL);
                    canvas.drawCircle(f + half, f2, half, this.mLegendFormPaint);
                    break;
                case 5:
                    this.mLegendFormPaint.setStyle(Paint.Style.FILL);
                    c.drawRect(x, f2 - half, f + formSize, f2 + half, this.mLegendFormPaint);
                    break;
                case 6:
                    float formLineWidth = Utils.convertDpToPixel(Float.isNaN(legendEntry.formLineWidth) ? legend.getFormLineWidth() : legendEntry.formLineWidth);
                    DashPathEffect formLineDashEffect = legendEntry.formLineDashEffect == null ? legend.getFormLineDashEffect() : legendEntry.formLineDashEffect;
                    this.mLegendFormPaint.setStyle(Paint.Style.STROKE);
                    this.mLegendFormPaint.setStrokeWidth(formLineWidth);
                    this.mLegendFormPaint.setPathEffect(formLineDashEffect);
                    this.mLineFormPath.reset();
                    this.mLineFormPath.moveTo(f, f2);
                    this.mLineFormPath.lineTo(f + formSize, f2);
                    canvas.drawPath(this.mLineFormPath, this.mLegendFormPaint);
                    break;
            }
            canvas.restoreToCount(restoreCount);
        }
    }

    /* renamed from: com.github.mikephil.charting.renderer.LegendRenderer$1  reason: invalid class name */
    static /* synthetic */ class AnonymousClass1 {
        static final /* synthetic */ int[] $SwitchMap$com$github$mikephil$charting$components$Legend$LegendForm;
        static final /* synthetic */ int[] $SwitchMap$com$github$mikephil$charting$components$Legend$LegendHorizontalAlignment;
        static final /* synthetic */ int[] $SwitchMap$com$github$mikephil$charting$components$Legend$LegendOrientation;
        static final /* synthetic */ int[] $SwitchMap$com$github$mikephil$charting$components$Legend$LegendVerticalAlignment;

        static {
            int[] iArr = new int[Legend.LegendForm.values().length];
            $SwitchMap$com$github$mikephil$charting$components$Legend$LegendForm = iArr;
            try {
                iArr[Legend.LegendForm.NONE.ordinal()] = 1;
            } catch (NoSuchFieldError e) {
            }
            try {
                $SwitchMap$com$github$mikephil$charting$components$Legend$LegendForm[Legend.LegendForm.EMPTY.ordinal()] = 2;
            } catch (NoSuchFieldError e2) {
            }
            try {
                $SwitchMap$com$github$mikephil$charting$components$Legend$LegendForm[Legend.LegendForm.DEFAULT.ordinal()] = 3;
            } catch (NoSuchFieldError e3) {
            }
            try {
                $SwitchMap$com$github$mikephil$charting$components$Legend$LegendForm[Legend.LegendForm.CIRCLE.ordinal()] = 4;
            } catch (NoSuchFieldError e4) {
            }
            try {
                $SwitchMap$com$github$mikephil$charting$components$Legend$LegendForm[Legend.LegendForm.SQUARE.ordinal()] = 5;
            } catch (NoSuchFieldError e5) {
            }
            try {
                $SwitchMap$com$github$mikephil$charting$components$Legend$LegendForm[Legend.LegendForm.LINE.ordinal()] = 6;
            } catch (NoSuchFieldError e6) {
            }
            int[] iArr2 = new int[Legend.LegendOrientation.values().length];
            $SwitchMap$com$github$mikephil$charting$components$Legend$LegendOrientation = iArr2;
            try {
                iArr2[Legend.LegendOrientation.HORIZONTAL.ordinal()] = 1;
            } catch (NoSuchFieldError e7) {
            }
            try {
                $SwitchMap$com$github$mikephil$charting$components$Legend$LegendOrientation[Legend.LegendOrientation.VERTICAL.ordinal()] = 2;
            } catch (NoSuchFieldError e8) {
            }
            int[] iArr3 = new int[Legend.LegendVerticalAlignment.values().length];
            $SwitchMap$com$github$mikephil$charting$components$Legend$LegendVerticalAlignment = iArr3;
            try {
                iArr3[Legend.LegendVerticalAlignment.TOP.ordinal()] = 1;
            } catch (NoSuchFieldError e9) {
            }
            try {
                $SwitchMap$com$github$mikephil$charting$components$Legend$LegendVerticalAlignment[Legend.LegendVerticalAlignment.BOTTOM.ordinal()] = 2;
            } catch (NoSuchFieldError e10) {
            }
            try {
                $SwitchMap$com$github$mikephil$charting$components$Legend$LegendVerticalAlignment[Legend.LegendVerticalAlignment.CENTER.ordinal()] = 3;
            } catch (NoSuchFieldError e11) {
            }
            int[] iArr4 = new int[Legend.LegendHorizontalAlignment.values().length];
            $SwitchMap$com$github$mikephil$charting$components$Legend$LegendHorizontalAlignment = iArr4;
            try {
                iArr4[Legend.LegendHorizontalAlignment.LEFT.ordinal()] = 1;
            } catch (NoSuchFieldError e12) {
            }
            try {
                $SwitchMap$com$github$mikephil$charting$components$Legend$LegendHorizontalAlignment[Legend.LegendHorizontalAlignment.RIGHT.ordinal()] = 2;
            } catch (NoSuchFieldError e13) {
            }
            try {
                $SwitchMap$com$github$mikephil$charting$components$Legend$LegendHorizontalAlignment[Legend.LegendHorizontalAlignment.CENTER.ordinal()] = 3;
            } catch (NoSuchFieldError e14) {
            }
        }
    }

    /* access modifiers changed from: protected */
    public void drawLabel(Canvas c, float x, float y, String label) {
        c.drawText(label, x, y, this.mLegendLabelPaint);
    }
}
