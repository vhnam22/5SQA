package com.exampanle.ky_thuat.manualclient.adapter;

import android.text.Editable;
import android.text.TextWatcher;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.CheckBox;
import android.widget.CompoundButton;
import android.widget.EditText;
import android.widget.TextView;
import androidx.recyclerview.widget.RecyclerView;
import com.exampanle.ky_thuat.manualclient.R;
import com.exampanle.ky_thuat.manualclient.apiclient.model.ConfigDto;
import java.util.List;

public class ResultConfigAdapter extends RecyclerView.Adapter<ResultConfigViewHolder> {
    private List<ConfigDto> mConfig;

    public ResultConfigAdapter(List<ConfigDto> mConfig2) {
        this.mConfig = mConfig2;
    }

    public ResultConfigViewHolder onCreateViewHolder(ViewGroup parent, int viewType) {
        return new ResultConfigViewHolder(LayoutInflater.from(parent.getContext()).inflate(R.layout.result_config, parent, false));
    }

    public void onBindViewHolder(ResultConfigViewHolder holder, int position) {
        final ConfigDto config = this.mConfig.get(position);
        if (config != null) {
            holder.cbIsShow.setChecked(config.isShow());
            holder.lblName.setText(config.getName());
            holder.txtDisplayName.setText(config.getDisplayName());
            holder.cbIsShow.setOnCheckedChangeListener(new CompoundButton.OnCheckedChangeListener() {
                public void onCheckedChanged(CompoundButton buttonView, boolean isChecked) {
                    config.setShow(isChecked);
                }
            });
            holder.txtDisplayName.addTextChangedListener(new TextWatcher() {
                public void afterTextChanged(Editable s) {
                    config.setDisplayName(s.toString());
                }

                public void beforeTextChanged(CharSequence s, int start, int count, int after) {
                }

                public void onTextChanged(CharSequence s, int start, int before, int count) {
                }
            });
        }
    }

    public int getItemCount() {
        List<ConfigDto> list = this.mConfig;
        if (list != null) {
            return list.size();
        }
        return 0;
    }

    public class ResultConfigViewHolder extends RecyclerView.ViewHolder {
        /* access modifiers changed from: private */
        public CheckBox cbIsShow;
        /* access modifiers changed from: private */
        public TextView lblName;
        /* access modifiers changed from: private */
        public EditText txtDisplayName;

        public ResultConfigViewHolder(View itemView) {
            super(itemView);
            this.cbIsShow = (CheckBox) itemView.findViewById(R.id.cbIsShow);
            this.lblName = (TextView) itemView.findViewById(R.id.lblName);
            this.txtDisplayName = (EditText) itemView.findViewById(R.id.txtDisplayName);
            itemView.setOnClickListener(new View.OnClickListener(ResultConfigAdapter.this) {
                public void onClick(View view) {
                    ResultConfigViewHolder.this.cbIsShow.setChecked(!ResultConfigViewHolder.this.cbIsShow.isChecked());
                }
            });
        }
    }
}
