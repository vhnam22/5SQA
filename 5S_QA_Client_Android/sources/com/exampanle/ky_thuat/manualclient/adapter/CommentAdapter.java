package com.exampanle.ky_thuat.manualclient.adapter;

import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.TextView;
import androidx.recyclerview.widget.RecyclerView;
import com.exampanle.ky_thuat.manualclient.R;
import com.exampanle.ky_thuat.manualclient.activity.CommentActivity;
import com.exampanle.ky_thuat.manualclient.apiclient.model.CommentViewModel;
import java.util.List;

public class CommentAdapter extends RecyclerView.Adapter<CommentViewHolder> {
    private List<CommentViewModel> mComments;

    public CommentAdapter(List<CommentViewModel> mComments2) {
        this.mComments = mComments2;
    }

    public CommentViewHolder onCreateViewHolder(ViewGroup parent, int viewType) {
        return new CommentViewHolder(LayoutInflater.from(parent.getContext()).inflate(R.layout.comment_row, parent, false));
    }

    public void onBindViewHolder(CommentViewHolder holder, int position) {
        CommentViewModel comment = this.mComments.get(position);
        if (comment != null) {
            final int currentPos = position + 1;
            holder.lblNo.setText(String.valueOf(((CommentActivity.CURRENT_PAGE - 1) * CommentActivity.ITEMS_PER_PAGE) + currentPos));
            holder.lblContent.setText(comment.getContent());
            holder.itemView.setOnClickListener(new View.OnClickListener() {
                public void onClick(View view) {
                    CommentActivity.numAuto = currentPos;
                    CommentAdapter.this.notifyDataSetChanged();
                }
            });
            if (currentPos == CommentActivity.numAuto) {
                holder.lblNo.setBackgroundResource(R.drawable.text_select);
            } else {
                holder.lblNo.setBackgroundResource(R.drawable.text_white);
            }
        }
    }

    public int getItemCount() {
        List<CommentViewModel> list = this.mComments;
        if (list != null) {
            return list.size();
        }
        return 0;
    }

    public CommentViewModel getItem(int position) {
        List<CommentViewModel> list = this.mComments;
        if (list != null) {
            return list.get(position);
        }
        return null;
    }

    public class CommentViewHolder extends RecyclerView.ViewHolder {
        /* access modifiers changed from: private */
        public TextView lblContent;
        /* access modifiers changed from: private */
        public TextView lblNo;

        public CommentViewHolder(View itemView) {
            super(itemView);
            this.lblNo = (TextView) itemView.findViewById(R.id.lblNo);
            this.lblContent = (TextView) itemView.findViewById(R.id.lblContent);
        }
    }
}
