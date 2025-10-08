package com.exampanle.ky_thuat.manualclient.adapter;

import android.app.Activity;
import android.content.Context;
import android.graphics.DashPathEffect;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.Button;
import android.widget.LinearLayout;
import android.widget.TextView;
import androidx.core.internal.view.SupportMenu;
import androidx.core.view.GravityCompat;
import androidx.core.view.ViewCompat;
import androidx.drawerlayout.widget.DrawerLayout;
import androidx.recyclerview.widget.LinearLayoutManager;
import androidx.recyclerview.widget.RecyclerView;
import com.exampanle.ky_thuat.manualclient.R;
import com.exampanle.ky_thuat.manualclient.activity.MainActivity;
import com.exampanle.ky_thuat.manualclient.activity.ResultActivity;
import com.exampanle.ky_thuat.manualclient.apiclient.JSON;
import com.exampanle.ky_thuat.manualclient.apiclient.api.apiGetResultWithQueryAsync;
import com.exampanle.ky_thuat.manualclient.apiclient.model.ArrayChartDto;
import com.exampanle.ky_thuat.manualclient.apiclient.model.ConfigDto;
import com.exampanle.ky_thuat.manualclient.apiclient.model.HistoryViewModel;
import com.exampanle.ky_thuat.manualclient.apiclient.model.QueryArgs;
import com.exampanle.ky_thuat.manualclient.apiclient.model.ResultViewModel;
import com.exampanle.ky_thuat.manualclient.ultil.Constant;
import com.github.mikephil.charting.charts.LineChart;
import com.github.mikephil.charting.components.Description;
import com.github.mikephil.charting.components.Legend;
import com.github.mikephil.charting.components.LegendEntry;
import com.github.mikephil.charting.components.XAxis;
import com.github.mikephil.charting.components.YAxis;
import com.github.mikephil.charting.data.Entry;
import com.github.mikephil.charting.data.LineData;
import com.github.mikephil.charting.data.LineDataSet;
import com.github.mikephil.charting.interfaces.datasets.ILineDataSet;
import com.google.gson.reflect.TypeToken;
import java.text.DecimalFormat;
import java.util.ArrayList;
import java.util.Arrays;
import java.util.Iterator;
import java.util.List;
import java.util.concurrent.ExecutionException;

public class ResultAdapter extends RecyclerView.Adapter<ResultViewHolder> {
    private List<ArrayChartDto> mChartDto = new ArrayList();
    /* access modifiers changed from: private */
    public Context mContext;
    private List<ResultViewModel> mResultForCharts;
    private List<ResultViewModel> mResults;

    public ResultAdapter(List<ResultViewModel> mResults2, Context mContext2) {
        this.mResults = mResults2;
        this.mContext = mContext2;
    }

    public ResultViewHolder onCreateViewHolder(ViewGroup parent, int viewType) {
        return new ResultViewHolder(LayoutInflater.from(parent.getContext()).inflate(R.layout.result_row, parent, false));
    }

    public void onBindViewHolder(final ResultViewHolder holder, int position) {
        String str;
        final ResultViewModel result = this.mResults.get(position);
        if (result != null) {
            final int currentPos = position + 1;
            holder.lblNo.setText(String.valueOf(((ResultActivity.CURRENT_PAGE - 1) * ResultActivity.ITEMS_PER_PAGE) + currentPos));
            holder.lblMeasurement.setText(result.getName());
            holder.lblSample.setText(String.valueOf(result.getSample()));
            holder.lblImportant.setText(result.getImportantName());
            holder.lblMachineType.setText(result.getMachineTypeName());
            holder.lblCavity.setText(String.valueOf(result.getCavity()));
            holder.lblNominal.setText(result.getValue());
            holder.lblUpper.setText(result.getUpper() == null ? null : String.valueOf(result.getUpper()));
            holder.lblLower.setText(result.getLower() == null ? null : String.valueOf(result.getLower()));
            holder.lblLSL.setText(result.getLSL() == null ? null : String.valueOf(result.getLSL()));
            holder.lblUSL.setText(result.getUSL() == null ? null : String.valueOf(result.getUSL()));
            holder.lblUnit.setText(result.getUnit());
            holder.lblResult.setText(result.getResult());
            holder.lblJudge.setText(result.getJudge());
            if (currentPos == ResultActivity.numAuto) {
                holder.panelResultRow.setBackgroundResource(R.drawable.text_select);
                holder.layoutHistory.setVisibility(8);
                holder.layoutKeyPad.setVisibility(0);
                if (result.getResult() == null) {
                    str = "";
                } else {
                    str = result.getResult();
                }
                ResultActivity.tempResult = str;
                try {
                    Double.parseDouble(result.getValue());
                    holder.linearlayoutKeyNumber.setVisibility(0);
                    holder.linearlayoutKeyOkNg.setVisibility(8);
                    if (ResultActivity.mIsShowChart) {
                        cal_YAxis(result);
                        getResultForMeas(result, ResultActivity.mItemChart);
                        draw_ChartRx(holder.chartRx, result, ResultActivity.mItemChart);
                    }
                } catch (Exception e) {
                    e.printStackTrace();
                    holder.linearlayoutKeyNumber.setVisibility(8);
                    holder.linearlayoutKeyOkNg.setVisibility(0);
                    if (ResultActivity.mIsShowChart) {
                        holder.chartRx.setData(null);
                        holder.chartRx.invalidate();
                        holder.chartHistogram.setData(null);
                        holder.chartHistogram.invalidate();
                    }
                    if (ResultActivity.tempResult.isEmpty()) {
                        holder.drawerLayoutResult.openDrawer((int) GravityCompat.END);
                    }
                }
                holder.lblKey.setText(ResultActivity.tempResult);
                if (result.getJudge() != null) {
                    if (result.getJudge().contains(Constant.OK)) {
                        holder.lblKey.setTextColor(-16776961);
                    } else {
                        holder.lblKey.setTextColor(SupportMenu.CATEGORY_MASK);
                    }
                }
            } else {
                holder.panelResultRow.setBackgroundResource(R.drawable.bg_rowselect);
            }
            if (result.getJudge() != null) {
                if (result.getJudge().contains(Constant.OK)) {
                    holder.lblResult.setTextColor(-16776961);
                    holder.lblJudge.setTextColor(-16776961);
                } else {
                    holder.lblResult.setTextColor(SupportMenu.CATEGORY_MASK);
                    holder.lblJudge.setTextColor(SupportMenu.CATEGORY_MASK);
                }
            }
            if (result.getHistory() == null || result.getHistory().equals("") || result.getHistory().equals("[]")) {
                holder.lblMeasurement.setText(result.getName());
            } else {
                holder.lblMeasurement.setText("* " + result.getName());
            }
            if (result.getHistory() == null || result.getHistory().equals("")) {
                holder.btnHistory.setVisibility(4);
            } else {
                holder.btnHistory.setVisibility(0);
            }
            holder.itemView.setOnClickListener(new View.OnClickListener() {
                public void onClick(View view) {
                    ResultActivity.numAuto = currentPos;
                    ResultAdapter.this.notifyDataSetChanged();
                    ResultActivity.firstAuto = false;
                }
            });
            holder.btnHistory.setOnClickListener(new View.OnClickListener() {
                public void onClick(View v) {
                    holder.drawerLayoutResult.openDrawer((int) GravityCompat.END);
                    holder.layoutHistory.setVisibility(0);
                    holder.layoutKeyPad.setVisibility(8);
                    holder.lblHisMachineType.setText(result.getMachineName());
                    holder.lblHisStaff.setText(result.getStaffName());
                    holder.lblHisTime.setText(result.getModified().toString());
                    holder.lblHisResult.setText(result.getResult());
                    holder.lblHisJudge.setText(result.getJudge());
                    if (result.getJudge() != null) {
                        if (result.getJudge().contains(Constant.OK)) {
                            holder.lblHisResult.setTextColor(-16776961);
                            holder.lblHisJudge.setTextColor(-16776961);
                        } else {
                            holder.lblHisResult.setTextColor(SupportMenu.CATEGORY_MASK);
                            holder.lblHisJudge.setTextColor(SupportMenu.CATEGORY_MASK);
                        }
                    }
                    new ArrayList<>().clear();
                    holder.rcvHistory.setAdapter(new HistoryAdapter((ArrayList) new JSON().deserialize(result.getHistory(), new TypeToken<ArrayList<HistoryViewModel>>() {
                    }.getType()), result));
                }
            });
            holder.lblMachineType.setOnClickListener(new View.OnClickListener() {
                public void onClick(View view) {
                    ResultActivity.txtSearch.setText(result.getMachineTypeName());
                    ResultActivity.numAuto = currentPos;
                    ResultAdapter.this.notifyDataSetChanged();
                    ResultActivity.firstAuto = false;
                }
            });
        }
    }

    public int getItemCount() {
        List<ResultViewModel> list = this.mResults;
        if (list != null) {
            return list.size();
        }
        return 0;
    }

    public ResultViewModel getItem(int position) {
        List<ResultViewModel> list = this.mResults;
        if (list != null) {
            return list.get(position);
        }
        return null;
    }

    private void draw_ChartRx(LineChart chart, ResultViewModel result, int item) {
        LineChart lineChart = chart;
        int i = item;
        if (result.getUnit() == null) {
            ResultViewModel resultViewModel = result;
        } else if (result.getUnit().isEmpty()) {
            ResultViewModel resultViewModel2 = result;
        } else {
            ArrayList<ILineDataSet> dataSets = new ArrayList<>();
            getLineDataSetSame(dataSets, result, i);
            ArrayList<Entry> resultLine = new ArrayList<>();
            for (int i2 = 0; i2 < this.mResultForCharts.size(); i2++) {
                resultLine.add(new Entry((float) (i2 + 1), (float) Double.parseDouble(this.mResultForCharts.get(i2).getResult())));
            }
            LineDataSet lineDataSetResult = new LineDataSet(resultLine, result.getName());
            lineDataSetResult.setColor(ViewCompat.MEASURED_STATE_MASK);
            lineDataSetResult.setCircleColor(SupportMenu.CATEGORY_MASK);
            lineDataSetResult.setDrawValues(false);
            dataSets.add(lineDataSetResult);
            Legend legend = chart.getLegend();
            legend.setForm(Legend.LegendForm.LINE);
            legend.setFormSize(20.0f);
            legend.setFormLineWidth(1.0f);
            legend.setXEntrySpace(10.0f);
            DashPathEffect path = new DashPathEffect(new float[]{10.0f, 5.0f}, 0.0f);
            LegendEntry[] legendEntries = new LegendEntry[dataSets.size()];
            for (int i3 = 0; i3 < legendEntries.length; i3++) {
                ILineDataSet line = dataSets.get(i3);
                LegendEntry legendEntry = new LegendEntry();
                legendEntry.label = line.getLabel();
                legendEntry.formColor = line.getColor();
                if (line.isDashedLineEnabled()) {
                    legendEntry.formLineDashEffect = path;
                }
                legendEntries[i3] = legendEntry;
            }
            legend.setCustom(legendEntries);
            setChartInfo(lineChart, i, 2, 2);
            chart.getAxisRight().setEnabled(false);
            lineChart.setData(new LineData((List<ILineDataSet>) dataSets));
            chart.invalidate();
        }
    }

    private void draw_ChartHistogram(LineChart chart, ResultViewModel result, int item) {
        if (result.getUnit() != null && !result.getUnit().isEmpty()) {
            ArrayList<ILineDataSet> dataSets = new ArrayList<>();
            getLineDataSetSame(dataSets, result, item);
            int count = getResultChartHistogram(dataSets);
            chart.getLegend().setTextColor(-1);
            chart.getLegend().setFormSize(0.0f);
            setChartInfo(chart, count + 1, 1, 2);
            chart.getAxisLeft().setDrawLabels(false);
            chart.setData(new LineData((List<ILineDataSet>) dataSets));
            chart.invalidate();
        }
    }

    private void cal_YAxis(ResultViewModel result) {
        this.mChartDto = new ArrayList();
        double usl = result.getUSL().doubleValue();
        double lsl = result.getLSL().doubleValue();
        for (float i = -2.5f; i < 13.0f; i += 0.5f) {
            this.mChartDto.add(new ArrayChartDto(usl - (((usl - lsl) * ((double) i)) / 10.0d), 0));
        }
    }

    private void getLineDataSetSame(ArrayList<ILineDataSet> dataSets, ResultViewModel result, int item) {
        ArrayList<ILineDataSet> arrayList = dataSets;
        int i = item;
        ArrayList<Entry> nominalLine = new ArrayList<>();
        ArrayList<Entry> upperLine = new ArrayList<>();
        ArrayList<Entry> lowerLine = new ArrayList<>();
        ArrayList<Entry> max80Line = new ArrayList<>();
        ArrayList<Entry> min80Line = new ArrayList<>();
        double nominal = Double.parseDouble(result.getValue());
        double usl = result.getUSL().doubleValue();
        double lsl = result.getLSL().doubleValue();
        double max80 = (result.getUpper().doubleValue() * 0.8d) + nominal;
        ArrayList<Entry> max80Line2 = max80Line;
        ArrayList<Entry> min80Line2 = min80Line;
        nominalLine.add(new Entry(0.0f, (float) nominal));
        nominalLine.add(new Entry((float) i, (float) nominal));
        upperLine.add(new Entry(0.0f, (float) usl));
        upperLine.add(new Entry((float) i, (float) usl));
        lowerLine.add(new Entry(0.0f, (float) lsl));
        lowerLine.add(new Entry((float) i, (float) lsl));
        ArrayList<Entry> max80Line3 = max80Line2;
        max80Line3.add(new Entry(0.0f, (float) max80));
        max80Line3.add(new Entry((float) i, (float) max80));
        double d = nominal;
        double min80 = nominal + (result.getLower().doubleValue() * 0.8d);
        ArrayList<Entry> min80Line3 = min80Line2;
        min80Line3.add(new Entry(0.0f, (float) min80));
        min80Line3.add(new Entry((float) i, (float) min80));
        LineDataSet lineDataSetNominal = new LineDataSet(nominalLine, "Center value");
        lineDataSetNominal.setDrawCircles(false);
        lineDataSetNominal.setDrawValues(false);
        LineDataSet lineDataSetUSL = new LineDataSet(upperLine, Constant.USL);
        lineDataSetUSL.setColor(SupportMenu.CATEGORY_MASK);
        lineDataSetUSL.setDrawCircles(false);
        lineDataSetUSL.setDrawValues(false);
        ArrayList<Entry> arrayList2 = nominalLine;
        LineDataSet lineDataSetLSL = new LineDataSet(lowerLine, Constant.LSL);
        lineDataSetLSL.setColor(SupportMenu.CATEGORY_MASK);
        lineDataSetLSL.setDrawCircles(false);
        lineDataSetLSL.setDrawValues(false);
        ArrayList<Entry> arrayList3 = upperLine;
        LineDataSet lineDataSetMax80 = new LineDataSet(max80Line3, "80% Max");
        lineDataSetMax80.setColor(-16776961);
        lineDataSetMax80.setDrawCircles(false);
        lineDataSetMax80.setDrawValues(false);
        ArrayList<Entry> arrayList4 = lowerLine;
        ArrayList<Entry> arrayList5 = max80Line3;
        lineDataSetMax80.enableDashedLine(10.0f, 5.0f, 0.0f);
        LineDataSet lineDataSetMin80 = new LineDataSet(min80Line3, "80% Min");
        lineDataSetMin80.setColor(-16776961);
        lineDataSetMin80.setDrawCircles(false);
        lineDataSetMin80.setDrawValues(false);
        lineDataSetMin80.enableDashedLine(10.0f, 5.0f, 0.0f);
        ArrayList<ILineDataSet> arrayList6 = dataSets;
        arrayList6.add(lineDataSetUSL);
        arrayList6.add(lineDataSetMax80);
        arrayList6.add(lineDataSetNominal);
        arrayList6.add(lineDataSetMin80);
        arrayList6.add(lineDataSetLSL);
    }

    private void setChartInfo(LineChart chart, int item, int xdiv, int ydiv) {
        chart.setDescription((Description) null);
        chart.setDrawBorders(true);
        XAxis xAxis = chart.getXAxis();
        xAxis.setLabelCount(item / xdiv, false);
        xAxis.setAxisMinimum(0.0f);
        xAxis.setAxisMaximum((float) item);
        YAxis yAxisLeft = chart.getAxisLeft();
        yAxisLeft.setLabelCount(this.mChartDto.size() / ydiv, false);
        yAxisLeft.setAxisMaximum((float) this.mChartDto.get(0).getValue());
        List<ArrayChartDto> list = this.mChartDto;
        yAxisLeft.setAxisMinimum((float) list.get(list.size() - 1).getValue());
        chart.getAxisRight().setEnabled(false);
    }

    private int getResultChartHistogram(ArrayList<ILineDataSet> dataSets) {
        int countMax = 0;
        ArrayList<Entry> allEntries = new ArrayList<>();
        for (int i = 0; i < this.mChartDto.size() - 1; i++) {
            int count = getFrequency(this.mResultForCharts, this.mChartDto.get(i + 1).getValue(), this.mChartDto.get(i).getValue());
            this.mChartDto.get(i).setCount(count);
            if (countMax < count) {
                countMax = count;
            }
            allEntries.add(new Entry((float) count, (float) this.mChartDto.get(i).getValue()));
            if (count > 0) {
                ArrayList<Entry> entries = new ArrayList<>();
                entries.add(new Entry(0.0f, (float) this.mChartDto.get(i).getValue()));
                entries.add(new Entry((float) count, (float) this.mChartDto.get(i).getValue()));
                LineDataSet lineDataSet = new LineDataSet(entries, String.valueOf(i));
                lineDataSet.setColor(-16711936);
                lineDataSet.setLineWidth(7.0f);
                lineDataSet.setDrawCircles(false);
                lineDataSet.setDrawValues(false);
                dataSets.add(lineDataSet);
            }
        }
        LineDataSet alllineDataSet = new LineDataSet(allEntries, "all");
        alllineDataSet.setColor(ViewCompat.MEASURED_STATE_MASK);
        alllineDataSet.setDrawCircles(false);
        alllineDataSet.setDrawValues(false);
        alllineDataSet.setMode(LineDataSet.Mode.CUBIC_BEZIER);
        dataSets.add(alllineDataSet);
        return countMax;
    }

    private int getFrequency(List<ResultViewModel> results, double srcFrom, double srcTo) {
        int count = 0;
        new DecimalFormat("0.00");
        for (ResultViewModel result : results) {
            double temp = Double.parseDouble(result.getResult());
            boolean z = false;
            boolean z2 = srcFrom < temp;
            if (temp <= srcTo) {
                z = true;
            }
            if (z && z2) {
                count++;
            }
        }
        return count;
    }

    private void getResultForMeas(ResultViewModel result, int item) {
        QueryArgs body = new QueryArgs();
        body.setPredicate("MeasurementId=@0 && !String.IsNullOrEmpty(Result)");
        body.setPredicateParameters(Arrays.asList(new Object[]{result.getMeasurementId()}));
        body.setOrder("Request.Date, Sample");
        body.setPage(1);
        body.setLimit(Integer.valueOf(item));
        try {
            this.mResultForCharts = new ArrayList();
            List<ResultViewModel> viewModels = (List) new apiGetResultWithQueryAsync(this.mContext, body).execute(new String[]{MainActivity.BASE_URL}).get();
            if (viewModels != null) {
                this.mResultForCharts = viewModels;
            }
        } catch (ExecutionException e) {
            e.printStackTrace();
        } catch (InterruptedException e2) {
            e2.printStackTrace();
        }
    }

    public class ResultViewHolder extends RecyclerView.ViewHolder {
        /* access modifiers changed from: private */
        public Button btnHistory;
        /* access modifiers changed from: private */
        public LineChart chartHistogram = ((LineChart) this.drawerLayoutResult.findViewById(R.id.chartHistogram));
        /* access modifiers changed from: private */
        public LineChart chartRx = ((LineChart) this.drawerLayoutResult.findViewById(R.id.chartRX));
        /* access modifiers changed from: private */
        public DrawerLayout drawerLayoutResult;
        /* access modifiers changed from: private */
        public LinearLayout layoutHistory = ((LinearLayout) this.drawerLayoutResult.findViewById(R.id.layoutHistory));
        /* access modifiers changed from: private */
        public LinearLayout layoutKeyPad;
        /* access modifiers changed from: private */
        public TextView lblCavity;
        /* access modifiers changed from: private */
        public TextView lblHisJudge = ((TextView) this.layoutHistory.findViewById(R.id.lblJudge));
        /* access modifiers changed from: private */
        public TextView lblHisMachineType = ((TextView) this.layoutHistory.findViewById(R.id.lblMachineType));
        /* access modifiers changed from: private */
        public TextView lblHisResult = ((TextView) this.layoutHistory.findViewById(R.id.lblResult));
        /* access modifiers changed from: private */
        public TextView lblHisStaff = ((TextView) this.layoutHistory.findViewById(R.id.lblStaff));
        /* access modifiers changed from: private */
        public TextView lblHisTime = ((TextView) this.layoutHistory.findViewById(R.id.lblTime));
        /* access modifiers changed from: private */
        public TextView lblImportant;
        /* access modifiers changed from: private */
        public TextView lblJudge;
        /* access modifiers changed from: private */
        public TextView lblKey = ((TextView) this.layoutKeyPad.findViewById(R.id.lblKey));
        /* access modifiers changed from: private */
        public TextView lblLSL;
        /* access modifiers changed from: private */
        public TextView lblLower;
        /* access modifiers changed from: private */
        public TextView lblMachineType;
        /* access modifiers changed from: private */
        public TextView lblMeasurement;
        /* access modifiers changed from: private */
        public TextView lblNo;
        /* access modifiers changed from: private */
        public TextView lblNominal;
        /* access modifiers changed from: private */
        public TextView lblResult;
        /* access modifiers changed from: private */
        public TextView lblSample;
        /* access modifiers changed from: private */
        public TextView lblUSL;
        /* access modifiers changed from: private */
        public TextView lblUnit;
        /* access modifiers changed from: private */
        public TextView lblUpper;
        private LinearLayoutManager linearLayoutManager;
        /* access modifiers changed from: private */
        public LinearLayout linearlayoutKeyNumber = ((LinearLayout) this.drawerLayoutResult.findViewById(R.id.linearlayoutKeyNumber));
        /* access modifiers changed from: private */
        public LinearLayout linearlayoutKeyOkNg = ((LinearLayout) this.drawerLayoutResult.findViewById(R.id.linearlayoutKeyOkNg));
        private LinearLayout panelResult;
        /* access modifiers changed from: private */
        public LinearLayout panelResultRow;
        private LinearLayout panelTolerance;
        /* access modifiers changed from: private */
        public RecyclerView rcvHistory = ((RecyclerView) this.drawerLayoutResult.findViewById(R.id.rcvHistory));

        public ResultViewHolder(View itemView) {
            super(itemView);
            this.lblNo = (TextView) itemView.findViewById(R.id.lblNo);
            this.lblMeasurement = (TextView) itemView.findViewById(R.id.lblMeasurement);
            this.lblSample = (TextView) itemView.findViewById(R.id.lblSample);
            this.lblImportant = (TextView) itemView.findViewById(R.id.lblImportant);
            this.lblMachineType = (TextView) itemView.findViewById(R.id.lblMachineType);
            this.lblNominal = (TextView) itemView.findViewById(R.id.lblNominal);
            this.lblUpper = (TextView) itemView.findViewById(R.id.lblUpper);
            this.lblLower = (TextView) itemView.findViewById(R.id.lblLower);
            this.lblLSL = (TextView) itemView.findViewById(R.id.lblLSL);
            this.lblUSL = (TextView) itemView.findViewById(R.id.lblUSL);
            this.lblUnit = (TextView) itemView.findViewById(R.id.lblUnit);
            this.lblCavity = (TextView) itemView.findViewById(R.id.lblCavity);
            this.lblResult = (TextView) itemView.findViewById(R.id.lblResult);
            this.lblJudge = (TextView) itemView.findViewById(R.id.lblJudge);
            this.btnHistory = (Button) itemView.findViewById(R.id.btnHistory);
            this.panelTolerance = (LinearLayout) itemView.findViewById(R.id.panelTolerance);
            this.panelResult = (LinearLayout) itemView.findViewById(R.id.panelResult);
            this.panelResultRow = (LinearLayout) itemView.findViewById(R.id.panelResultRow);
            DrawerLayout drawerLayout = (DrawerLayout) ((Activity) ResultAdapter.this.mContext).findViewById(R.id.draverLayoutResult);
            this.drawerLayoutResult = drawerLayout;
            this.layoutKeyPad = (LinearLayout) drawerLayout.findViewById(R.id.layoutKeyPad);
            LinearLayoutManager linearLayoutManager2 = new LinearLayoutManager(ResultAdapter.this.mContext);
            this.linearLayoutManager = linearLayoutManager2;
            this.rcvHistory.setLayoutManager(linearLayoutManager2);
            Iterator<ConfigDto> it = ResultActivity.listResultConfig.iterator();
            while (it.hasNext()) {
                ConfigDto item = it.next();
                if (!item.isShow()) {
                    String name = item.getName();
                    char c = 65535;
                    switch (name.hashCode()) {
                        case -1850559427:
                            if (name.equals("Result")) {
                                c = 11;
                                break;
                            }
                            break;
                        case -1825807926:
                            if (name.equals("Sample")) {
                                c = 2;
                                break;
                            }
                            break;
                        case -1257926675:
                            if (name.equals("Tolerance")) {
                                c = 5;
                                break;
                            }
                            break;
                        case -507420484:
                            if (name.equals("Nominal")) {
                                c = 4;
                                break;
                            }
                            break;
                        case 2529:
                            if (name.equals("No")) {
                                c = 0;
                                break;
                            }
                            break;
                        case 75685:
                            if (name.equals(Constant.LSL)) {
                                c = 6;
                                break;
                            }
                            break;
                        case 84334:
                            if (name.equals(Constant.USL)) {
                                c = 7;
                                break;
                            }
                            break;
                        case 2641316:
                            if (name.equals("Unit")) {
                                c = 8;
                                break;
                            }
                            break;
                        case 71925495:
                            if (name.equals("Judge")) {
                                c = 12;
                                break;
                            }
                            break;
                        case 908954950:
                            if (name.equals("Dimension")) {
                                c = 1;
                                break;
                            }
                            break;
                        case 1449751553:
                            if (name.equals("MachineType")) {
                                c = 9;
                                break;
                            }
                            break;
                        case 1795442690:
                            if (name.equals("Important")) {
                                c = 3;
                                break;
                            }
                            break;
                        case 2011354614:
                            if (name.equals("Cavity")) {
                                c = 10;
                                break;
                            }
                            break;
                    }
                    switch (c) {
                        case 0:
                            this.lblNo.setVisibility(8);
                            break;
                        case 1:
                            this.lblMeasurement.setVisibility(8);
                            break;
                        case 2:
                            this.lblSample.setVisibility(8);
                            break;
                        case 3:
                            this.lblImportant.setVisibility(8);
                            break;
                        case 4:
                            this.lblNominal.setVisibility(8);
                            break;
                        case 5:
                            this.panelTolerance.setVisibility(8);
                            break;
                        case 6:
                            this.lblLSL.setVisibility(8);
                            break;
                        case 7:
                            this.lblUSL.setVisibility(8);
                            break;
                        case 8:
                            this.lblUnit.setVisibility(8);
                            break;
                        case 9:
                            this.lblMachineType.setVisibility(8);
                            break;
                        case 10:
                            this.lblCavity.setVisibility(8);
                            break;
                        case 11:
                            this.panelResult.setVisibility(8);
                            break;
                        case 12:
                            this.lblJudge.setVisibility(8);
                            break;
                    }
                }
            }
        }
    }
}
