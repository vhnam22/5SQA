package com.exampanle.ky_thuat.manualclient.adapter;

import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.CheckBox;
import android.widget.TextView;
import androidx.recyclerview.widget.RecyclerView;
import com.exampanle.ky_thuat.manualclient.R;
import com.exampanle.ky_thuat.manualclient.activity.SettingActivity;
import com.exampanle.ky_thuat.manualclient.apiclient.model.MetadataValueViewModel;
import java.util.List;

public class MachineTypeAdapter extends RecyclerView.Adapter<MachineTypeViewHolder> {
    private List<MetadataValueViewModel> mMachineTypes;

    public MachineTypeAdapter(List<MetadataValueViewModel> mMachineTypes2) {
        this.mMachineTypes = mMachineTypes2;
    }

    public MachineTypeViewHolder onCreateViewHolder(ViewGroup parent, int viewType) {
        return new MachineTypeViewHolder(LayoutInflater.from(parent.getContext()).inflate(R.layout.machinetype_row, parent, false));
    }

    public void onBindViewHolder(final MachineTypeViewHolder holder, int position) {
        final MetadataValueViewModel machine = this.mMachineTypes.get(position);
        if (machine != null) {
            holder.lblNo.setText(String.valueOf(position + 1));
            holder.lblName.setText(machine.getName());
            if (SettingActivity.listMachineStr != null) {
                holder.cbCheck.setChecked(SettingActivity.listMachineStr.contains(machine.getName()));
            }
            holder.itemView.setOnClickListener(new View.OnClickListener() {
                public void onClick(View view) {
                    holder.cbCheck.setChecked(!holder.cbCheck.isChecked());
                    if (holder.cbCheck.isChecked()) {
                        SettingActivity.listMachineStr.add(machine.getName());
                    } else {
                        SettingActivity.listMachineStr.remove(machine.getName());
                    }
                }
            });
        }
    }

    public int getItemCount() {
        List<MetadataValueViewModel> list = this.mMachineTypes;
        if (list != null) {
            return list.size();
        }
        return 0;
    }

    public class MachineTypeViewHolder extends RecyclerView.ViewHolder {
        /* access modifiers changed from: private */
        public CheckBox cbCheck;
        /* access modifiers changed from: private */
        public TextView lblName;
        /* access modifiers changed from: private */
        public TextView lblNo;

        public MachineTypeViewHolder(View itemView) {
            super(itemView);
            this.lblNo = (TextView) itemView.findViewById(R.id.lblNo);
            this.lblName = (TextView) itemView.findViewById(R.id.lblName);
            this.cbCheck = (CheckBox) itemView.findViewById(R.id.cbCheck);
        }
    }
}
