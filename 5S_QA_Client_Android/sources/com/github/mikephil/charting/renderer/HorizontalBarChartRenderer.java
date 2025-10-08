package com.github.mikephil.charting.renderer;

import android.graphics.Canvas;
import android.graphics.Paint;
import android.graphics.RectF;
import android.graphics.drawable.Drawable;
import com.github.mikephil.charting.animation.ChartAnimator;
import com.github.mikephil.charting.buffer.BarBuffer;
import com.github.mikephil.charting.buffer.HorizontalBarBuffer;
import com.github.mikephil.charting.data.BarData;
import com.github.mikephil.charting.data.BarEntry;
import com.github.mikephil.charting.formatter.ValueFormatter;
import com.github.mikephil.charting.highlight.Highlight;
import com.github.mikephil.charting.interfaces.dataprovider.BarDataProvider;
import com.github.mikephil.charting.interfaces.dataprovider.ChartInterface;
import com.github.mikephil.charting.interfaces.datasets.IBarDataSet;
import com.github.mikephil.charting.utils.MPPointF;
import com.github.mikephil.charting.utils.Transformer;
import com.github.mikephil.charting.utils.Utils;
import com.github.mikephil.charting.utils.ViewPortHandler;
import java.util.List;

public class HorizontalBarChartRenderer extends BarChartRenderer {
    private RectF mBarShadowRectBuffer = new RectF();

    public HorizontalBarChartRenderer(BarDataProvider chart, ChartAnimator animator, ViewPortHandler viewPortHandler) {
        super(chart, animator, viewPortHandler);
        this.mValuePaint.setTextAlign(Paint.Align.LEFT);
    }

    public void initBuffers() {
        BarData barData = this.mChart.getBarData();
        this.mBarBuffers = new HorizontalBarBuffer[barData.getDataSetCount()];
        for (int i = 0; i < this.mBarBuffers.length; i++) {
            IBarDataSet set = (IBarDataSet) barData.getDataSetByIndex(i);
            this.mBarBuffers[i] = new HorizontalBarBuffer(set.getEntryCount() * 4 * (set.isStacked() ? set.getStackSize() : 1), barData.getDataSetCount(), set.isStacked());
        }
    }

    /* access modifiers changed from: protected */
    public void drawDataSet(Canvas c, IBarDataSet dataSet, int index) {
        BarData barData;
        IBarDataSet iBarDataSet = dataSet;
        int i = index;
        Transformer trans = this.mChart.getTransformer(dataSet.getAxisDependency());
        this.mBarBorderPaint.setColor(dataSet.getBarBorderColor());
        this.mBarBorderPaint.setStrokeWidth(Utils.convertDpToPixel(dataSet.getBarBorderWidth()));
        boolean drawBorder = dataSet.getBarBorderWidth() > 0.0f;
        float phaseX = this.mAnimator.getPhaseX();
        float phaseY = this.mAnimator.getPhaseY();
        if (this.mChart.isDrawBarShadowEnabled()) {
            this.mShadowPaint.setColor(dataSet.getBarShadowColor());
            BarData barData2 = this.mChart.getBarData();
            float barWidthHalf = barData2.getBarWidth() / 2.0f;
            int i2 = 0;
            int count = Math.min((int) Math.ceil((double) (((float) dataSet.getEntryCount()) * phaseX)), dataSet.getEntryCount());
            while (true) {
                if (i2 >= count) {
                    Canvas canvas = c;
                    break;
                }
                float x = ((BarEntry) iBarDataSet.getEntryForIndex(i2)).getX();
                this.mBarShadowRectBuffer.top = x - barWidthHalf;
                this.mBarShadowRectBuffer.bottom = x + barWidthHalf;
                trans.rectValueToPixel(this.mBarShadowRectBuffer);
                if (!this.mViewPortHandler.isInBoundsTop(this.mBarShadowRectBuffer.bottom)) {
                    barData = barData2;
                    Canvas canvas2 = c;
                } else if (!this.mViewPortHandler.isInBoundsBottom(this.mBarShadowRectBuffer.top)) {
                    Canvas canvas3 = c;
                    break;
                } else {
                    this.mBarShadowRectBuffer.left = this.mViewPortHandler.contentLeft();
                    this.mBarShadowRectBuffer.right = this.mViewPortHandler.contentRight();
                    barData = barData2;
                    c.drawRect(this.mBarShadowRectBuffer, this.mShadowPaint);
                }
                i2++;
                barData2 = barData;
            }
        } else {
            Canvas canvas4 = c;
        }
        BarBuffer buffer = this.mBarBuffers[i];
        buffer.setPhases(phaseX, phaseY);
        buffer.setDataSet(i);
        buffer.setInverted(this.mChart.isInverted(dataSet.getAxisDependency()));
        buffer.setBarWidth(this.mChart.getBarData().getBarWidth());
        buffer.feed(iBarDataSet);
        trans.pointValuesToPixel(buffer.buffer);
        boolean isSingleColor = dataSet.getColors().size() == 1;
        if (isSingleColor) {
            this.mRenderPaint.setColor(dataSet.getColor());
        }
        int j = 0;
        while (j < buffer.size() && this.mViewPortHandler.isInBoundsTop(buffer.buffer[j + 3])) {
            if (this.mViewPortHandler.isInBoundsBottom(buffer.buffer[j + 1])) {
                if (!isSingleColor) {
                    this.mRenderPaint.setColor(iBarDataSet.getColor(j / 4));
                }
                c.drawRect(buffer.buffer[j], buffer.buffer[j + 1], buffer.buffer[j + 2], buffer.buffer[j + 3], this.mRenderPaint);
                if (drawBorder) {
                    c.drawRect(buffer.buffer[j], buffer.buffer[j + 1], buffer.buffer[j + 2], buffer.buffer[j + 3], this.mBarBorderPaint);
                }
            }
            j += 4;
        }
    }

    public void drawValues(Canvas c) {
        float valueOffsetPlus;
        List<IBarDataSet> dataSets;
        int i;
        MPPointF iconsOffset;
        float posOffset;
        float negOffset;
        float posOffset2;
        float negOffset2;
        float valueOffsetPlus2;
        float halfTextHeight;
        int index;
        Transformer trans;
        float[] vals;
        float posOffset3;
        BarEntry entry;
        float negOffset3;
        float negOffset4;
        float posOffset4;
        int k;
        float[] transformed;
        float x;
        float y;
        float y2;
        String formattedValue;
        float negOffset5;
        float[] vals2;
        float negOffset6;
        float posOffset5;
        BarEntry entry2;
        float posOffset6;
        float negOffset7;
        List<IBarDataSet> dataSets2;
        int j;
        int i2;
        BarBuffer buffer;
        MPPointF iconsOffset2;
        BarEntry entry3;
        float negOffset8;
        MPPointF iconsOffset3;
        float negOffset9;
        float posOffset7;
        if (isDrawingValuesAllowed(this.mChart)) {
            List<IBarDataSet> dataSets3 = this.mChart.getBarData().getDataSets();
            float posOffset8 = Utils.convertDpToPixel(5.0f);
            float posOffset9 = 0.0f;
            float negOffset10 = 0.0f;
            boolean drawValueAboveBar = this.mChart.isDrawValueAboveBarEnabled();
            int i3 = 0;
            while (i3 < this.mChart.getBarData().getDataSetCount()) {
                IBarDataSet dataSet = dataSets3.get(i3);
                if (!shouldDrawValues(dataSet)) {
                    dataSets = dataSets3;
                    valueOffsetPlus = posOffset8;
                    i = i3;
                } else {
                    boolean isInverted = this.mChart.isInverted(dataSet.getAxisDependency());
                    applyValueTextStyle(dataSet);
                    float halfTextHeight2 = ((float) Utils.calcTextHeight(this.mValuePaint, "10")) / 2.0f;
                    ValueFormatter formatter = dataSet.getValueFormatter();
                    BarBuffer buffer2 = this.mBarBuffers[i3];
                    float phaseY = this.mAnimator.getPhaseY();
                    MPPointF iconsOffset4 = MPPointF.getInstance(dataSet.getIconsOffset());
                    iconsOffset4.x = Utils.convertDpToPixel(iconsOffset4.x);
                    iconsOffset4.y = Utils.convertDpToPixel(iconsOffset4.y);
                    if (!dataSet.isStacked()) {
                        int j2 = 0;
                        while (true) {
                            posOffset6 = posOffset9;
                            if (((float) j2) >= ((float) buffer2.buffer.length) * this.mAnimator.getPhaseX()) {
                                negOffset7 = negOffset10;
                                int i4 = j2;
                                dataSets = dataSets3;
                                i = i3;
                                iconsOffset = iconsOffset4;
                                BarBuffer barBuffer = buffer2;
                                break;
                            }
                            float y3 = (buffer2.buffer[j2 + 1] + buffer2.buffer[j2 + 3]) / 2.0f;
                            if (!this.mViewPortHandler.isInBoundsTop(buffer2.buffer[j2 + 1])) {
                                negOffset7 = negOffset10;
                                dataSets = dataSets3;
                                i = i3;
                                iconsOffset = iconsOffset4;
                                BarBuffer barBuffer2 = buffer2;
                                break;
                            }
                            if (this.mViewPortHandler.isInBoundsX(buffer2.buffer[j2]) && this.mViewPortHandler.isInBoundsBottom(buffer2.buffer[j2 + 1])) {
                                BarEntry entry4 = (BarEntry) dataSet.getEntryForIndex(j2 / 4);
                                float val = entry4.getY();
                                String formattedValue2 = formatter.getBarLabel(entry4);
                                float f = negOffset10;
                                float valueTextWidth = (float) Utils.calcTextWidth(this.mValuePaint, formattedValue2);
                                String formattedValue3 = formattedValue2;
                                float posOffset10 = drawValueAboveBar ? posOffset8 : -(valueTextWidth + posOffset8);
                                if (drawValueAboveBar) {
                                    entry3 = entry4;
                                    negOffset8 = -(valueTextWidth + posOffset8);
                                } else {
                                    entry3 = entry4;
                                    negOffset8 = posOffset8;
                                }
                                if (isInverted) {
                                    iconsOffset3 = iconsOffset4;
                                    posOffset7 = (-posOffset10) - valueTextWidth;
                                    negOffset9 = (-negOffset8) - valueTextWidth;
                                } else {
                                    iconsOffset3 = iconsOffset4;
                                    posOffset7 = posOffset10;
                                    negOffset9 = negOffset8;
                                }
                                if (dataSet.isDrawValuesEnabled()) {
                                    float f2 = valueTextWidth;
                                    j = j2;
                                    dataSets2 = dataSets3;
                                    iconsOffset2 = iconsOffset3;
                                    i2 = i3;
                                    buffer = buffer2;
                                    drawValue(c, formattedValue3, buffer2.buffer[j2 + 2] + (val >= 0.0f ? posOffset7 : negOffset9), y3 + halfTextHeight2, dataSet.getValueTextColor(j2 / 2));
                                } else {
                                    j = j2;
                                    dataSets2 = dataSets3;
                                    iconsOffset2 = iconsOffset3;
                                    i2 = i3;
                                    buffer = buffer2;
                                }
                                if (entry3.getIcon() != null && dataSet.isDrawIconsEnabled()) {
                                    Drawable icon = entry3.getIcon();
                                    Utils.drawImage(c, icon, (int) (buffer.buffer[j + 2] + (val >= 0.0f ? posOffset7 : negOffset9) + iconsOffset2.x), (int) (y3 + iconsOffset2.y), icon.getIntrinsicWidth(), icon.getIntrinsicHeight());
                                }
                                posOffset9 = posOffset7;
                                negOffset10 = negOffset9;
                            } else {
                                j = j2;
                                dataSets2 = dataSets3;
                                i2 = i3;
                                posOffset9 = posOffset6;
                                iconsOffset2 = iconsOffset4;
                                buffer = buffer2;
                            }
                            j2 = j + 4;
                            iconsOffset4 = iconsOffset2;
                            buffer2 = buffer;
                            i3 = i2;
                            dataSets3 = dataSets2;
                        }
                        valueOffsetPlus = posOffset8;
                        float f3 = halfTextHeight2;
                        posOffset2 = posOffset6;
                        negOffset2 = negOffset7;
                    } else {
                        dataSets = dataSets3;
                        i = i3;
                        iconsOffset = iconsOffset4;
                        BarBuffer buffer3 = buffer2;
                        Transformer trans2 = this.mChart.getTransformer(dataSet.getAxisDependency());
                        int bufferIndex = 0;
                        int index2 = 0;
                        while (true) {
                            if (((float) index2) >= ((float) dataSet.getEntryCount()) * this.mAnimator.getPhaseX()) {
                                posOffset = posOffset9;
                                negOffset = negOffset10;
                                int i5 = index2;
                                valueOffsetPlus = posOffset8;
                                Transformer transformer = trans2;
                                float f4 = halfTextHeight2;
                                break;
                            }
                            BarEntry entry5 = (BarEntry) dataSet.getEntryForIndex(index2);
                            int color = dataSet.getValueTextColor(index2);
                            float[] vals3 = entry5.getYVals();
                            if (vals3 != null) {
                                float posOffset11 = posOffset9;
                                float negOffset11 = negOffset10;
                                BarEntry entry6 = entry5;
                                index = index2;
                                halfTextHeight = halfTextHeight2;
                                vals = vals3;
                                float[] transformed2 = new float[(vals.length * 2)];
                                int k2 = 0;
                                int idx = 0;
                                float posY = 0.0f;
                                float negY = -entry6.getNegativeSum();
                                while (k2 < transformed2.length) {
                                    float value = vals[idx];
                                    if (value == 0.0f && (posY == 0.0f || negY == 0.0f)) {
                                        y2 = value;
                                    } else if (value >= 0.0f) {
                                        posY += value;
                                        y2 = posY;
                                    } else {
                                        y2 = negY;
                                        negY -= value;
                                    }
                                    transformed2[k2] = y2 * phaseY;
                                    k2 += 2;
                                    idx++;
                                }
                                trans2.pointValuesToPixel(transformed2);
                                int k3 = 0;
                                posOffset3 = posOffset11;
                                negOffset10 = negOffset11;
                                while (true) {
                                    if (k3 >= transformed2.length) {
                                        float negOffset12 = posOffset3;
                                        float f5 = negOffset10;
                                        int i6 = k3;
                                        float[] fArr = transformed2;
                                        valueOffsetPlus2 = posOffset8;
                                        BarEntry barEntry = entry6;
                                        trans = trans2;
                                        break;
                                    }
                                    float val2 = vals[k3 / 2];
                                    BarEntry entry7 = entry6;
                                    trans = trans2;
                                    String formattedValue4 = formatter.getBarStackedLabel(val2, entry7);
                                    float f6 = posOffset3;
                                    float valueTextWidth2 = (float) Utils.calcTextWidth(this.mValuePaint, formattedValue4);
                                    float f7 = negOffset10;
                                    float negOffset13 = drawValueAboveBar ? posOffset8 : -(valueTextWidth2 + posOffset8);
                                    if (drawValueAboveBar) {
                                        entry = entry7;
                                        negOffset3 = -(valueTextWidth2 + posOffset8);
                                    } else {
                                        entry = entry7;
                                        negOffset3 = posOffset8;
                                    }
                                    if (isInverted) {
                                        valueOffsetPlus2 = posOffset8;
                                        posOffset4 = (-negOffset13) - valueTextWidth2;
                                        negOffset4 = (-negOffset3) - valueTextWidth2;
                                    } else {
                                        valueOffsetPlus2 = posOffset8;
                                        posOffset4 = negOffset13;
                                        negOffset4 = negOffset3;
                                    }
                                    float x2 = (((val2 > 0.0f ? 1 : (val2 == 0.0f ? 0 : -1)) == 0 && (negY > 0.0f ? 1 : (negY == 0.0f ? 0 : -1)) == 0 && (posY > 0.0f ? 1 : (posY == 0.0f ? 0 : -1)) > 0) || (val2 > 0.0f ? 1 : (val2 == 0.0f ? 0 : -1)) < 0 ? negOffset4 : posOffset4) + transformed2[k3];
                                    float f8 = valueTextWidth2;
                                    float y4 = (buffer3.buffer[bufferIndex + 1] + buffer3.buffer[bufferIndex + 3]) / 2.0f;
                                    if (!this.mViewPortHandler.isInBoundsTop(y4)) {
                                        posOffset3 = posOffset4;
                                        negOffset10 = negOffset4;
                                        break;
                                    }
                                    if (!this.mViewPortHandler.isInBoundsX(x2)) {
                                        k = k3;
                                        transformed = transformed2;
                                    } else if (!this.mViewPortHandler.isInBoundsBottom(y4)) {
                                        k = k3;
                                        transformed = transformed2;
                                    } else {
                                        if (dataSet.isDrawValuesEnabled()) {
                                            y = y4;
                                            x = x2;
                                            float f9 = val2;
                                            k = k3;
                                            transformed = transformed2;
                                            drawValue(c, formattedValue4, x, y4 + halfTextHeight, color);
                                        } else {
                                            y = y4;
                                            x = x2;
                                            float f10 = val2;
                                            k = k3;
                                            transformed = transformed2;
                                        }
                                        if (entry.getIcon() != null && dataSet.isDrawIconsEnabled()) {
                                            Drawable icon2 = entry.getIcon();
                                            Utils.drawImage(c, icon2, (int) (x + iconsOffset.x), (int) (y + iconsOffset.y), icon2.getIntrinsicWidth(), icon2.getIntrinsicHeight());
                                        }
                                    }
                                    k3 = k + 2;
                                    posOffset3 = posOffset4;
                                    negOffset10 = negOffset4;
                                    trans2 = trans;
                                    entry6 = entry;
                                    posOffset8 = valueOffsetPlus2;
                                    transformed2 = transformed;
                                }
                            } else {
                                posOffset = posOffset9;
                                if (!this.mViewPortHandler.isInBoundsTop(buffer3.buffer[bufferIndex + 1])) {
                                    negOffset = negOffset10;
                                    valueOffsetPlus = posOffset8;
                                    float f11 = halfTextHeight2;
                                    break;
                                } else if (this.mViewPortHandler.isInBoundsX(buffer3.buffer[bufferIndex]) && this.mViewPortHandler.isInBoundsBottom(buffer3.buffer[bufferIndex + 1])) {
                                    String formattedValue5 = formatter.getBarLabel(entry5);
                                    float valueTextWidth3 = (float) Utils.calcTextWidth(this.mValuePaint, formattedValue5);
                                    float f12 = negOffset10;
                                    float negOffset14 = drawValueAboveBar ? posOffset8 : -(valueTextWidth3 + posOffset8);
                                    if (drawValueAboveBar) {
                                        formattedValue = formattedValue5;
                                        negOffset5 = -(valueTextWidth3 + posOffset8);
                                    } else {
                                        formattedValue = formattedValue5;
                                        negOffset5 = posOffset8;
                                    }
                                    if (isInverted) {
                                        vals2 = vals3;
                                        posOffset5 = (-negOffset14) - valueTextWidth3;
                                        negOffset6 = (-negOffset5) - valueTextWidth3;
                                    } else {
                                        vals2 = vals3;
                                        posOffset5 = negOffset14;
                                        negOffset6 = negOffset5;
                                    }
                                    if (dataSet.isDrawValuesEnabled()) {
                                        float f13 = buffer3.buffer[bufferIndex + 2];
                                        float f14 = entry5.getY() >= 0.0f ? posOffset5 : negOffset6;
                                        float f15 = buffer3.buffer[bufferIndex + 1] + halfTextHeight2;
                                        float f16 = valueTextWidth3;
                                        halfTextHeight = halfTextHeight2;
                                        vals = vals2;
                                        entry2 = entry5;
                                        float f17 = f15;
                                        index = index2;
                                        drawValue(c, formattedValue, f13 + f14, f17, color);
                                    } else {
                                        index = index2;
                                        halfTextHeight = halfTextHeight2;
                                        vals = vals2;
                                        entry2 = entry5;
                                    }
                                    if (entry2.getIcon() != null && dataSet.isDrawIconsEnabled()) {
                                        Drawable icon3 = entry2.getIcon();
                                        Utils.drawImage(c, icon3, (int) (buffer3.buffer[bufferIndex + 2] + (entry2.getY() >= 0.0f ? posOffset5 : negOffset6) + iconsOffset.x), (int) (buffer3.buffer[bufferIndex + 1] + iconsOffset.y), icon3.getIntrinsicWidth(), icon3.getIntrinsicHeight());
                                    }
                                    valueOffsetPlus2 = posOffset8;
                                    posOffset3 = posOffset5;
                                    negOffset10 = negOffset6;
                                    BarEntry barEntry2 = entry2;
                                    trans = trans2;
                                } else {
                                    posOffset3 = posOffset;
                                }
                            }
                            bufferIndex = vals == null ? bufferIndex + 4 : bufferIndex + (vals.length * 4);
                            index2 = index + 1;
                            trans2 = trans;
                            halfTextHeight2 = halfTextHeight;
                            posOffset8 = valueOffsetPlus2;
                        }
                        posOffset2 = posOffset;
                        negOffset2 = negOffset;
                    }
                    MPPointF.recycleInstance(iconsOffset);
                }
                i3 = i + 1;
                dataSets3 = dataSets;
                posOffset8 = valueOffsetPlus;
            }
            float f18 = posOffset8;
            int i7 = i3;
        }
    }

    public void drawValue(Canvas c, String valueText, float x, float y, int color) {
        this.mValuePaint.setColor(color);
        c.drawText(valueText, x, y, this.mValuePaint);
    }

    /* access modifiers changed from: protected */
    public void prepareBarHighlight(float x, float y1, float y2, float barWidthHalf, Transformer trans) {
        RectF rectF = this.mBarRect;
        rectF.set(y1, x - barWidthHalf, y2, x + barWidthHalf);
        trans.rectToPixelPhaseHorizontal(this.mBarRect, this.mAnimator.getPhaseY());
    }

    /* access modifiers changed from: protected */
    public void setHighlightDrawPos(Highlight high, RectF bar) {
        high.setDraw(bar.centerY(), bar.right);
    }

    /* access modifiers changed from: protected */
    public boolean isDrawingValuesAllowed(ChartInterface chart) {
        return ((float) chart.getData().getEntryCount()) < ((float) chart.getMaxVisibleCount()) * this.mViewPortHandler.getScaleY();
    }
}
