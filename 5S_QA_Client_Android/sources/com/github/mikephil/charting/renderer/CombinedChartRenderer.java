package com.github.mikephil.charting.renderer;

import android.graphics.Canvas;
import android.util.Log;
import com.github.mikephil.charting.animation.ChartAnimator;
import com.github.mikephil.charting.charts.Chart;
import com.github.mikephil.charting.charts.CombinedChart;
import com.github.mikephil.charting.data.ChartData;
import com.github.mikephil.charting.data.CombinedData;
import com.github.mikephil.charting.highlight.Highlight;
import com.github.mikephil.charting.utils.ViewPortHandler;
import java.lang.ref.WeakReference;
import java.util.ArrayList;
import java.util.List;

public class CombinedChartRenderer extends DataRenderer {
    protected WeakReference<Chart> mChart;
    protected List<Highlight> mHighlightBuffer = new ArrayList();
    protected List<DataRenderer> mRenderers = new ArrayList(5);

    public CombinedChartRenderer(CombinedChart chart, ChartAnimator animator, ViewPortHandler viewPortHandler) {
        super(animator, viewPortHandler);
        this.mChart = new WeakReference<>(chart);
        createRenderers();
    }

    public void createRenderers() {
        this.mRenderers.clear();
        CombinedChart chart = (CombinedChart) this.mChart.get();
        if (chart != null) {
            for (CombinedChart.DrawOrder order : chart.getDrawOrder()) {
                switch (AnonymousClass1.$SwitchMap$com$github$mikephil$charting$charts$CombinedChart$DrawOrder[order.ordinal()]) {
                    case 1:
                        if (chart.getBarData() == null) {
                            break;
                        } else {
                            this.mRenderers.add(new BarChartRenderer(chart, this.mAnimator, this.mViewPortHandler));
                            break;
                        }
                    case 2:
                        if (chart.getBubbleData() == null) {
                            break;
                        } else {
                            this.mRenderers.add(new BubbleChartRenderer(chart, this.mAnimator, this.mViewPortHandler));
                            break;
                        }
                    case 3:
                        if (chart.getLineData() == null) {
                            break;
                        } else {
                            this.mRenderers.add(new LineChartRenderer(chart, this.mAnimator, this.mViewPortHandler));
                            break;
                        }
                    case 4:
                        if (chart.getCandleData() == null) {
                            break;
                        } else {
                            this.mRenderers.add(new CandleStickChartRenderer(chart, this.mAnimator, this.mViewPortHandler));
                            break;
                        }
                    case 5:
                        if (chart.getScatterData() == null) {
                            break;
                        } else {
                            this.mRenderers.add(new ScatterChartRenderer(chart, this.mAnimator, this.mViewPortHandler));
                            break;
                        }
                }
            }
        }
    }

    /* renamed from: com.github.mikephil.charting.renderer.CombinedChartRenderer$1  reason: invalid class name */
    static /* synthetic */ class AnonymousClass1 {
        static final /* synthetic */ int[] $SwitchMap$com$github$mikephil$charting$charts$CombinedChart$DrawOrder;

        static {
            int[] iArr = new int[CombinedChart.DrawOrder.values().length];
            $SwitchMap$com$github$mikephil$charting$charts$CombinedChart$DrawOrder = iArr;
            try {
                iArr[CombinedChart.DrawOrder.BAR.ordinal()] = 1;
            } catch (NoSuchFieldError e) {
            }
            try {
                $SwitchMap$com$github$mikephil$charting$charts$CombinedChart$DrawOrder[CombinedChart.DrawOrder.BUBBLE.ordinal()] = 2;
            } catch (NoSuchFieldError e2) {
            }
            try {
                $SwitchMap$com$github$mikephil$charting$charts$CombinedChart$DrawOrder[CombinedChart.DrawOrder.LINE.ordinal()] = 3;
            } catch (NoSuchFieldError e3) {
            }
            try {
                $SwitchMap$com$github$mikephil$charting$charts$CombinedChart$DrawOrder[CombinedChart.DrawOrder.CANDLE.ordinal()] = 4;
            } catch (NoSuchFieldError e4) {
            }
            try {
                $SwitchMap$com$github$mikephil$charting$charts$CombinedChart$DrawOrder[CombinedChart.DrawOrder.SCATTER.ordinal()] = 5;
            } catch (NoSuchFieldError e5) {
            }
        }
    }

    public void initBuffers() {
        for (DataRenderer renderer : this.mRenderers) {
            renderer.initBuffers();
        }
    }

    public void drawData(Canvas c) {
        for (DataRenderer renderer : this.mRenderers) {
            renderer.drawData(c);
        }
    }

    public void drawValue(Canvas c, String valueText, float x, float y, int color) {
        Log.e(Chart.LOG_TAG, "Erroneous call to drawValue() in CombinedChartRenderer!");
    }

    public void drawValues(Canvas c) {
        for (DataRenderer renderer : this.mRenderers) {
            renderer.drawValues(c);
        }
    }

    public void drawExtras(Canvas c) {
        for (DataRenderer renderer : this.mRenderers) {
            renderer.drawExtras(c);
        }
    }

    public void drawHighlighted(Canvas c, Highlight[] indices) {
        int dataIndex;
        Chart chart = (Chart) this.mChart.get();
        if (chart != null) {
            for (DataRenderer renderer : this.mRenderers) {
                ChartData data = null;
                if (renderer instanceof BarChartRenderer) {
                    data = ((BarChartRenderer) renderer).mChart.getBarData();
                } else if (renderer instanceof LineChartRenderer) {
                    data = ((LineChartRenderer) renderer).mChart.getLineData();
                } else if (renderer instanceof CandleStickChartRenderer) {
                    data = ((CandleStickChartRenderer) renderer).mChart.getCandleData();
                } else if (renderer instanceof ScatterChartRenderer) {
                    data = ((ScatterChartRenderer) renderer).mChart.getScatterData();
                } else if (renderer instanceof BubbleChartRenderer) {
                    data = ((BubbleChartRenderer) renderer).mChart.getBubbleData();
                }
                if (data == null) {
                    dataIndex = -1;
                } else {
                    dataIndex = ((CombinedData) chart.getData()).getAllData().indexOf(data);
                }
                this.mHighlightBuffer.clear();
                for (Highlight h : indices) {
                    if (h.getDataIndex() == dataIndex || h.getDataIndex() == -1) {
                        this.mHighlightBuffer.add(h);
                    }
                }
                List<Highlight> list = this.mHighlightBuffer;
                renderer.drawHighlighted(c, (Highlight[]) list.toArray(new Highlight[list.size()]));
            }
        }
    }

    public DataRenderer getSubRenderer(int index) {
        if (index >= this.mRenderers.size() || index < 0) {
            return null;
        }
        return this.mRenderers.get(index);
    }

    public List<DataRenderer> getSubRenderers() {
        return this.mRenderers;
    }

    public void setSubRenderers(List<DataRenderer> renderers) {
        this.mRenderers = renderers;
    }
}
