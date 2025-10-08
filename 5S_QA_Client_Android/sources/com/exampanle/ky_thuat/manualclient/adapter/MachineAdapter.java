package com.exampanle.ky_thuat.manualclient.adapter;

import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.CheckBox;
import android.widget.CompoundButton;
import android.widget.TextView;
import android.widget.Toast;
import androidx.core.view.ViewCompat;
import androidx.recyclerview.widget.RecyclerView;
import com.exampanle.ky_thuat.manualclient.R;
import com.exampanle.ky_thuat.manualclient.activity.MainActivity;
import com.exampanle.ky_thuat.manualclient.apiclient.api.apiSetToolAsync;
import com.exampanle.ky_thuat.manualclient.apiclient.model.MachineForToolViewModel;
import com.exampanle.ky_thuat.manualclient.apiclient.model.ToolViewModel;
import java.util.List;
import java.util.concurrent.ExecutionException;

public class MachineAdapter extends RecyclerView.Adapter<MachineViewHolder> {
    private List<MachineForToolViewModel> mMachines;

    public MachineAdapter(List<MachineForToolViewModel> mMachines2) {
        this.mMachines = mMachines2;
    }

    public MachineViewHolder onCreateViewHolder(ViewGroup parent, int viewType) {
        return new MachineViewHolder(LayoutInflater.from(parent.getContext()).inflate(R.layout.machine_row, parent, false));
    }

    public void onBindViewHolder(final MachineViewHolder holder, int position) {
        final MachineForToolViewModel machine = this.mMachines.get(position);
        if (machine != null) {
            holder.lblNo.setText(String.valueOf(position + 1));
            holder.lblName.setText(machine.getName());
            holder.lblType.setText(machine.getMachineTypeName());
            holder.cbType.setChecked(machine.getHasTool().booleanValue());
            if (machine.getHasTool().booleanValue()) {
                holder.lblNo.setTextColor(-16776961);
                holder.lblName.setTextColor(-16776961);
                holder.lblType.setTextColor(-16776961);
            } else {
                holder.lblNo.setTextColor(ViewCompat.MEASURED_STATE_MASK);
                holder.lblName.setTextColor(ViewCompat.MEASURED_STATE_MASK);
                holder.lblType.setTextColor(ViewCompat.MEASURED_STATE_MASK);
            }
            holder.cbType.setOnCheckedChangeListener(new CompoundButton.OnCheckedChangeListener() {
                public void onCheckedChanged(CompoundButton buttonView, boolean isChecked) {
                    ToolViewModel tool = new ToolViewModel();
                    if (isChecked) {
                        tool.setTabletId(MainActivity.TABLET_ID);
                    }
                    tool.setMachineId(machine.getId());
                    try {
                        boolean z = true;
                        if (((ToolViewModel) new apiSetToolAsync(buttonView.getContext(), tool).execute(new String[]{MainActivity.BASE_URL}).get()) == null) {
                            CheckBox access$300 = holder.cbType;
                            if (isChecked) {
                                z = false;
                            }
                            access$300.setChecked(z);
                            Toast.makeText(buttonView.getContext(), "Error", 0).show();
                        } else if (isChecked) {
                            holder.lblNo.setTextColor(-16776961);
                            holder.lblName.setTextColor(-16776961);
                            holder.lblType.setTextColor(-16776961);
                        } else {
                            holder.lblNo.setTextColor(ViewCompat.MEASURED_STATE_MASK);
                            holder.lblName.setTextColor(ViewCompat.MEASURED_STATE_MASK);
                            holder.lblType.setTextColor(ViewCompat.MEASURED_STATE_MASK);
                        }
                    } catch (ExecutionException e) {
                        e.printStackTrace();
                    } catch (InterruptedException e2) {
                        e2.printStackTrace();
                    }
                }
            });
        }
    }

    public int getItemCount() {
        List<MachineForToolViewModel> list = this.mMachines;
        if (list != null) {
            return list.size();
        }
        return 0;
    }

    public class MachineViewHolder extends RecyclerView.ViewHolder {
        /* access modifiers changed from: private */
        public CheckBox cbType;
        /* access modifiers changed from: private */
        public TextView lblName;
        /* access modifiers changed from: private */
        public TextView lblNo;
        /* access modifiers changed from: private */
        public TextView lblType;

        public MachineViewHolder(View itemView) {
            super(itemView);
            this.lblNo = (TextView) itemView.findViewById(R.id.lblNo);
            this.lblName = (TextView) itemView.findViewById(R.id.lblName);
            this.lblType = (TextView) itemView.findViewById(R.id.lblType);
            this.cbType = (CheckBox) itemView.findViewById(R.id.cbType);
            itemView.setOnClickListener(new View.OnClickListener(MachineAdapter.this) {
                public void onClick(View view) {
                    MachineViewHolder.this.cbType.setChecked(!MachineViewHolder.this.cbType.isChecked());
                }
            });
        }
    }
}
