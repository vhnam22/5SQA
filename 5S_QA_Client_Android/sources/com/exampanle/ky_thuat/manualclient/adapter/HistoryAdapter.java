package com.exampanle.ky_thuat.manualclient.adapter;

import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.TextView;
import androidx.core.internal.view.SupportMenu;
import androidx.recyclerview.widget.RecyclerView;
import com.exampanle.ky_thuat.manualclient.R;
import com.exampanle.ky_thuat.manualclient.apiclient.model.HistoryViewModel;
import com.exampanle.ky_thuat.manualclient.apiclient.model.ResultViewModel;
import com.exampanle.ky_thuat.manualclient.ultil.Constant;
import java.util.List;

public class HistoryAdapter extends RecyclerView.Adapter<HistoryViewHolder> {
    private List<HistoryViewModel> mHistories;
    private ResultViewModel mResult;

    public HistoryAdapter(List<HistoryViewModel> mHistories2, ResultViewModel mResult2) {
        this.mHistories = mHistories2;
        this.mResult = mResult2;
    }

    public HistoryViewHolder onCreateViewHolder(ViewGroup parent, int viewType) {
        return new HistoryViewHolder(LayoutInflater.from(parent.getContext()).inflate(R.layout.history_result, parent, false));
    }

    public void onBindViewHolder(HistoryViewHolder holder, int position) {
        HistoryViewModel history = this.mHistories.get(position);
        if (history != null) {
            holder.lblMachineType.setText(history.getMachineName());
            holder.lblResult.setText(history.getValue());
            holder.lblStaff.setText(history.getCreatedBy());
            holder.lblTime.setText(history.getCreated());
            if (history.getValue() == null || history.getValue().equals("") || history.getValue().equals(Constant.OK) || history.getValue().equals(Constant.GO)) {
                holder.lblJudge.setText(Constant.OK);
                holder.lblJudge.setTextColor(-16776961);
                holder.lblResult.setTextColor(-16776961);
            } else if (history.getValue().equals(Constant.NG) || history.getValue().equals(Constant.STOP)) {
                holder.lblJudge.setText(Constant.NG);
                holder.lblJudge.setTextColor(SupportMenu.CATEGORY_MASK);
                holder.lblResult.setTextColor(SupportMenu.CATEGORY_MASK);
            } else {
                Double deviation = Double.valueOf(new Double(history.getValue()).doubleValue() - new Double(this.mResult.getValue()).doubleValue());
                if (deviation.doubleValue() < this.mResult.getLower().doubleValue()) {
                    holder.lblJudge.setText("NG-");
                    holder.lblJudge.setTextColor(SupportMenu.CATEGORY_MASK);
                    holder.lblResult.setTextColor(SupportMenu.CATEGORY_MASK);
                } else if (deviation.doubleValue() > this.mResult.getUpper().doubleValue()) {
                    holder.lblJudge.setText("NG+");
                    holder.lblJudge.setTextColor(SupportMenu.CATEGORY_MASK);
                    holder.lblResult.setTextColor(SupportMenu.CATEGORY_MASK);
                } else {
                    holder.lblJudge.setText(Constant.OK);
                    holder.lblJudge.setTextColor(-16776961);
                    holder.lblResult.setTextColor(-16776961);
                }
            }
        }
    }

    public int getItemCount() {
        List<HistoryViewModel> list = this.mHistories;
        if (list != null) {
            return list.size();
        }
        return 0;
    }

    public class HistoryViewHolder extends RecyclerView.ViewHolder {
        /* access modifiers changed from: private */
        public TextView lblJudge;
        /* access modifiers changed from: private */
        public TextView lblMachineType;
        /* access modifiers changed from: private */
        public TextView lblResult;
        /* access modifiers changed from: private */
        public TextView lblStaff;
        /* access modifiers changed from: private */
        public TextView lblTime;

        public HistoryViewHolder(View itemView) {
            super(itemView);
            this.lblMachineType = (TextView) itemView.findViewById(R.id.lblMachineType);
            this.lblJudge = (TextView) itemView.findViewById(R.id.lblJudge);
            this.lblResult = (TextView) itemView.findViewById(R.id.lblResult);
            this.lblStaff = (TextView) itemView.findViewById(R.id.lblStaff);
            this.lblTime = (TextView) itemView.findViewById(R.id.lblTime);
        }
    }
}
