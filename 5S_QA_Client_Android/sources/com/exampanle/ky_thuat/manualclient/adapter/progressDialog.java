package com.exampanle.ky_thuat.manualclient.adapter;

import android.app.Activity;
import android.app.ProgressDialog;
import android.content.Context;
import android.content.res.Resources;
import com.exampanle.ky_thuat.manualclient.R;

public class progressDialog {
    /* access modifiers changed from: private */
    public Context mContext;
    /* access modifiers changed from: private */
    public ProgressDialog progressDialog;

    public progressDialog(Context context) {
        this.mContext = context;
    }

    public void showLoadingDialog() {
        ((Activity) this.mContext).runOnUiThread(new Runnable() {
            public void run() {
                Resources re = progressDialog.this.mContext.getResources();
                progressDialog progressdialog = progressDialog.this;
                ProgressDialog unused = progressdialog.progressDialog = ProgressDialog.show(progressdialog.mContext, re.getText(R.string.dialog_loading_title), re.getText(R.string.dialog_msg), true, false);
            }
        });
    }

    public void dismissProgressDialog() {
        ((Activity) this.mContext).runOnUiThread(new Runnable() {
            public void run() {
                if (progressDialog.this.progressDialog != null && progressDialog.this.progressDialog.isShowing()) {
                    progressDialog.this.progressDialog.dismiss();
                }
            }
        });
    }

    public void showConnectDialog() {
        ((Activity) this.mContext).runOnUiThread(new Runnable() {
            public void run() {
                Resources re = progressDialog.this.mContext.getResources();
                progressDialog progressdialog = progressDialog.this;
                ProgressDialog unused = progressdialog.progressDialog = ProgressDialog.show(progressdialog.mContext, re.getText(R.string.dialog_connect_title), re.getText(R.string.dialog_msg), true, false);
            }
        });
    }
}
