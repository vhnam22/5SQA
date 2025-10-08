package com.exampanle.ky_thuat.manualclient.adapter;

import android.app.Activity;
import android.content.Intent;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.Button;
import android.widget.TextView;
import androidx.recyclerview.widget.RecyclerView;
import com.exampanle.ky_thuat.manualclient.R;
import com.exampanle.ky_thuat.manualclient.activity.CommentActivity;
import com.exampanle.ky_thuat.manualclient.activity.MainActivity;
import com.exampanle.ky_thuat.manualclient.activity.RequestActivity;
import com.exampanle.ky_thuat.manualclient.activity.ResultActivity;
import com.exampanle.ky_thuat.manualclient.activity.ResultViewActivity;
import com.exampanle.ky_thuat.manualclient.apiclient.model.RequestViewModel;
import com.exampanle.ky_thuat.manualclient.ultil.Constant;
import com.exampanle.ky_thuat.manualclient.ultil.ProcessUI;
import java.util.List;

public class RequestAdapter extends RecyclerView.Adapter<RequestViewHolder> {
    private List<RequestViewModel> mRequests;

    public RequestAdapter(List<RequestViewModel> mRequests2) {
        this.mRequests = mRequests2;
    }

    public RequestViewHolder onCreateViewHolder(ViewGroup parent, int viewType) {
        return new RequestViewHolder(LayoutInflater.from(parent.getContext()).inflate(R.layout.request_row, parent, false));
    }

    public void onBindViewHolder(RequestViewHolder holder, int position) {
        RequestViewModel request = this.mRequests.get(position);
        if (request != null) {
            holder.lblNo.setText(String.valueOf(position + 1));
            holder.lblName.setText(request.getName() + " * Lot: " + request.getLot() + " * Sample: " + String.valueOf(request.getSample()) + " * Type: " + request.getType() + " * Status: " + request.getStatus());
            holder.request = request;
            if (!request.getStatus().contains(Constant.Activated) && !request.getStatus().contains(Constant.Rejected)) {
                holder.btnResult.setVisibility(8);
            }
            if (request.getStatus().equals(Constant.Approved)) {
                holder.btnComment.setVisibility(4);
            } else {
                holder.btnComment.setVisibility(0);
            }
        }
    }

    public int getItemCount() {
        List<RequestViewModel> list = this.mRequests;
        if (list != null) {
            return list.size();
        }
        return 0;
    }

    public class RequestViewHolder extends RecyclerView.ViewHolder {
        Button btnComment;
        Button btnDetail;
        Button btnResult;
        TextView lblName;
        TextView lblNo;
        RequestViewModel request;

        public RequestViewHolder(View itemView) {
            super(itemView);
            this.lblNo = (TextView) itemView.findViewById(R.id.lblNo);
            this.lblName = (TextView) itemView.findViewById(R.id.lblName);
            this.btnDetail = (Button) itemView.findViewById(R.id.btnDetail);
            this.btnResult = (Button) itemView.findViewById(R.id.btnResult);
            this.btnComment = (Button) itemView.findViewById(R.id.btnComment);
            this.btnResult.setOnClickListener(new View.OnClickListener(RequestAdapter.this) {
                public void onClick(View view) {
                    RequestActivity.progressDialog.showLoadingDialog();
                    Intent intent = new Intent(view.getContext(), ResultViewActivity.class);
                    intent.putExtra("REQUEST", RequestViewHolder.this.request);
                    ((Activity) view.getContext()).startActivityForResult(intent, 2);
                }
            });
            this.btnComment.setOnClickListener(new View.OnClickListener(RequestAdapter.this) {
                public void onClick(View view) {
                    if (!RequestViewHolder.this.request.getStatus().equals(Constant.Approved)) {
                        RequestActivity.progressDialog.showLoadingDialog();
                        Intent intent = new Intent(view.getContext(), CommentActivity.class);
                        intent.putExtra("REQUEST", RequestViewHolder.this.request);
                        ((Activity) view.getContext()).startActivityForResult(intent, 2);
                    }
                }
            });
            this.btnDetail.setOnClickListener(new View.OnClickListener(RequestAdapter.this) {
                public void onClick(View view) {
                    ProcessUI.openDialogRequestDetail(view.getContext(), RequestViewHolder.this.request);
                }
            });
            itemView.setOnClickListener(new View.OnClickListener(RequestAdapter.this) {
                public void onClick(View view) {
                    if (RequestViewHolder.this.request.getStatus().equals(Constant.Activated) || RequestViewHolder.this.request.getStatus().contains(Constant.Rejected)) {
                        RequestActivity.progressDialog.showLoadingDialog();
                        Intent intent = new Intent(view.getContext(), ResultActivity.class);
                        intent.putExtra("REQUEST", RequestViewHolder.this.request);
                        ((Activity) view.getContext()).startActivityForResult(intent, 2);
                        MainActivity.CURRENT_ACTIVITY = 3;
                        return;
                    }
                    RequestActivity.progressDialog.showLoadingDialog();
                    Intent intent2 = new Intent(view.getContext(), ResultViewActivity.class);
                    intent2.putExtra("REQUEST", RequestViewHolder.this.request);
                    ((Activity) view.getContext()).startActivityForResult(intent2, 2);
                }
            });
        }
    }
}
