package com.exampanle.ky_thuat.manualclient.adapter;

import android.app.Activity;
import android.content.Context;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.Button;
import android.widget.LinearLayout;
import android.widget.TextView;
import androidx.core.internal.view.SupportMenu;
import androidx.core.view.GravityCompat;
import androidx.drawerlayout.widget.DrawerLayout;
import androidx.recyclerview.widget.LinearLayoutManager;
import androidx.recyclerview.widget.RecyclerView;
import com.exampanle.ky_thuat.manualclient.R;
import com.exampanle.ky_thuat.manualclient.activity.ResultViewActivity;
import com.exampanle.ky_thuat.manualclient.apiclient.JSON;
import com.exampanle.ky_thuat.manualclient.apiclient.model.ConfigDto;
import com.exampanle.ky_thuat.manualclient.apiclient.model.HistoryViewModel;
import com.exampanle.ky_thuat.manualclient.apiclient.model.ResultViewModel;
import com.exampanle.ky_thuat.manualclient.ultil.Constant;
import com.google.gson.reflect.TypeToken;
import java.util.ArrayList;
import java.util.Iterator;
import java.util.List;

public class ResultViewAdapter extends RecyclerView.Adapter<ResultViewHolder> {
    /* access modifiers changed from: private */
    public Context mContext;
    private List<ResultViewModel> mResults;

    public ResultViewAdapter(List<ResultViewModel> mResults2, Context mContext2) {
        this.mResults = mResults2;
        this.mContext = mContext2;
    }

    public ResultViewHolder onCreateViewHolder(ViewGroup parent, int viewType) {
        return new ResultViewHolder(LayoutInflater.from(parent.getContext()).inflate(R.layout.result_row, parent, false));
    }

    public void onBindViewHolder(final ResultViewHolder holder, int position) {
        final ResultViewModel result = this.mResults.get(position);
        if (result != null) {
            holder.lblMeasurement.setText(result.getName());
            holder.lblSample.setText(String.valueOf(result.getSample()));
            holder.lblImportant.setText(result.getImportantName());
            holder.lblMachineType.setText(result.getMachineTypeName());
            holder.lblCavity.setText(String.valueOf(result.getCavity()));
            holder.lblNominal.setText(result.getValue());
            String str = null;
            holder.lblUpper.setText(result.getUpper() == null ? null : String.valueOf(result.getUpper()));
            holder.lblLower.setText(result.getLower() == null ? null : String.valueOf(result.getLower()));
            holder.lblLSL.setText(result.getLSL() == null ? null : String.valueOf(result.getLSL()));
            TextView access$900 = holder.lblUSL;
            if (result.getUSL() != null) {
                str = String.valueOf(result.getUSL());
            }
            access$900.setText(str);
            holder.lblUnit.setText(result.getUnit());
            holder.lblResult.setText(result.getResult());
            holder.lblJudge.setText(result.getJudge());
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
                holder.lblNo.setText(String.valueOf(position + 1));
            } else {
                holder.lblNo.setText("*" + String.valueOf(position + 1));
            }
            if (result.getHistory() == null || result.getHistory().equals("")) {
                holder.btnHistory.setVisibility(4);
            } else {
                holder.btnHistory.setVisibility(0);
            }
            holder.btnHistory.setOnClickListener(new View.OnClickListener() {
                public void onClick(View v) {
                    holder.layoutHistory.setVisibility(0);
                    holder.drawerLayoutResult.openDrawer((int) GravityCompat.END);
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
        }
    }

    public int getItemCount() {
        List<ResultViewModel> list = this.mResults;
        if (list != null) {
            return list.size();
        }
        return 0;
    }

    public class ResultViewHolder extends RecyclerView.ViewHolder {
        /* access modifiers changed from: private */
        public Button btnHistory;
        /* access modifiers changed from: private */
        public DrawerLayout drawerLayoutResult;
        /* access modifiers changed from: private */
        public LinearLayout layoutHistory;
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
        private LinearLayout panelResult;
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
            this.lblCavity = (TextView) itemView.findViewById(R.id.lblCavity);
            this.lblNominal = (TextView) itemView.findViewById(R.id.lblNominal);
            this.lblUpper = (TextView) itemView.findViewById(R.id.lblUpper);
            this.lblLower = (TextView) itemView.findViewById(R.id.lblLower);
            this.lblLSL = (TextView) itemView.findViewById(R.id.lblLSL);
            this.lblUSL = (TextView) itemView.findViewById(R.id.lblUSL);
            this.lblUnit = (TextView) itemView.findViewById(R.id.lblUnit);
            this.lblResult = (TextView) itemView.findViewById(R.id.lblResult);
            this.lblJudge = (TextView) itemView.findViewById(R.id.lblJudge);
            this.btnHistory = (Button) itemView.findViewById(R.id.btnHistory);
            this.panelTolerance = (LinearLayout) itemView.findViewById(R.id.panelTolerance);
            this.panelResult = (LinearLayout) itemView.findViewById(R.id.panelResult);
            DrawerLayout drawerLayout = (DrawerLayout) ((Activity) ResultViewAdapter.this.mContext).findViewById(R.id.draverLayoutResult);
            this.drawerLayoutResult = drawerLayout;
            this.layoutHistory = (LinearLayout) drawerLayout.findViewById(R.id.layoutHistory);
            LinearLayoutManager linearLayoutManager2 = new LinearLayoutManager(ResultViewAdapter.this.mContext);
            this.linearLayoutManager = linearLayoutManager2;
            this.rcvHistory.setLayoutManager(linearLayoutManager2);
            Iterator<ConfigDto> it = ResultViewActivity.listResultConfig.iterator();
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
            this.layoutHistory.setVisibility(8);
            itemView.setOnClickListener(new View.OnClickListener(ResultViewAdapter.this) {
                public void onClick(View view) {
                    ResultViewHolder.this.layoutHistory.setVisibility(8);
                }
            });
        }
    }
}
