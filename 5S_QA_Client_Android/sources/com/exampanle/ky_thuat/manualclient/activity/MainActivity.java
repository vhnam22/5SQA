package com.exampanle.ky_thuat.manualclient.activity;

import android.app.Activity;
import android.content.Context;
import android.content.Intent;
import android.content.IntentFilter;
import android.os.Build;
import android.os.Bundle;
import android.provider.Settings;
import android.telephony.TelephonyManager;
import android.view.View;
import android.widget.Button;
import android.widget.CheckBox;
import android.widget.EditText;
import android.widget.TextView;
import android.widget.Toast;
import androidx.appcompat.app.AppCompatActivity;
import androidx.core.app.ActivityCompat;
import com.exampanle.ky_thuat.manualclient.R;
import com.exampanle.ky_thuat.manualclient.adapter.NetworkChangeReceiver;
import com.exampanle.ky_thuat.manualclient.adapter.WebSocketClient;
import com.exampanle.ky_thuat.manualclient.adapter.progressDialog;
import com.exampanle.ky_thuat.manualclient.apiclient.api.apiLoginAsync;
import com.exampanle.ky_thuat.manualclient.apiclient.model.AuthUserViewModel;
import com.exampanle.ky_thuat.manualclient.apiclient.model.LoginDto;
import com.exampanle.ky_thuat.manualclient.ultil.Common;
import com.exampanle.ky_thuat.manualclient.ultil.Constant;
import com.google.zxing.integration.android.IntentIntegrator;
import com.google.zxing.integration.android.IntentResult;
import java.util.UUID;
import java.util.concurrent.ExecutionException;

public class MainActivity extends AppCompatActivity {
    static final /* synthetic */ boolean $assertionsDisabled = false;
    public static String BASE_URL;
    public static int CURRENT_ACTIVITY;
    public static String FULL_NAME;
    public static String IMEI;
    public static UUID TABLET_ID;
    public static String TOKEN;
    public static progressDialog progressDialog;
    public static WebSocketClient webSocketClient;
    private Button btnLogin;
    private Button btnQRScan;
    private Button btnSetting;
    /* access modifiers changed from: private */
    public CheckBox cbRemember;
    private TextView lblIME;
    /* access modifiers changed from: private */
    public TextView lblTabletName;
    /* access modifiers changed from: private */
    public Context mContext;
    private NetworkChangeReceiver networkChangeReceiver;
    /* access modifiers changed from: private */
    public EditText txtPassword;
    /* access modifiers changed from: private */
    public EditText txtUsername;

    /* access modifiers changed from: protected */
    public void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView((int) R.layout.activity_main);
        addControl();
        loadData();
        initSocket();
        processControl();
    }

    /* access modifiers changed from: protected */
    public void onResume() {
        super.onResume();
        ActivityManager.setCurrentActivity(this);
    }

    /* access modifiers changed from: protected */
    public void onActivityResult(int requestCode, int resultCode, Intent data) {
        super.onActivityResult(requestCode, resultCode, data);
        CURRENT_ACTIVITY = 1;
        IntentResult result = IntentIntegrator.parseActivityResult(requestCode, resultCode, data);
        if (result != null && result.getContents() == null) {
            Toast.makeText(this, "Cancel", 1).show();
        }
    }

    /* access modifiers changed from: protected */
    public void onDestroy() {
        webSocketClient.Disconnect();
        unregisterReceiver(this.networkChangeReceiver);
        super.onDestroy();
    }

    private void loadData() {
        try {
            Common common = new Common(this.mContext);
            String iMEIDeviceId = getIMEIDeviceId(this.mContext);
            IMEI = iMEIDeviceId;
            this.lblIME.setText(iMEIDeviceId);
            BASE_URL = (String) common.getData(Constant.URLAPI, 1);
            this.lblTabletName.setText((String) common.getData(Constant.TABLETMARK, "...", 1));
            TABLET_ID = UUID.fromString((String) common.getData(Constant.TABLETID, 1));
            if (((Boolean) common.getData(Constant.REMEMBER, 2)).booleanValue()) {
                this.txtUsername.setText((String) common.getData(Constant.USERNAME, 1));
                this.txtPassword.setText((String) common.getData(Constant.PASSWORD, 1));
                this.cbRemember.setChecked(true);
                this.txtPassword.requestFocus();
                return;
            }
            this.cbRemember.setChecked(false);
        } catch (Exception ex) {
            Toast.makeText(this.mContext, ex.toString(), 0).show();
        }
    }

    private String getIMEIDeviceId(Context context) {
        if (Build.VERSION.SDK_INT >= 29) {
            return Settings.Secure.getString(context.getContentResolver(), "android_id");
        }
        ActivityCompat.requestPermissions(this, new String[]{"android.permission.READ_PHONE_STATE"}, 0);
        TelephonyManager mTelephony = (TelephonyManager) context.getSystemService("phone");
        if (Build.VERSION.SDK_INT >= 23 && context.checkSelfPermission("android.permission.READ_PHONE_STATE") != 0) {
            return "";
        }
        if (mTelephony == null) {
            throw new AssertionError();
        } else if (mTelephony.getDeviceId() == null) {
            return Settings.Secure.getString(context.getContentResolver(), "android_id");
        } else {
            if (Build.VERSION.SDK_INT >= 26) {
                return mTelephony.getImei();
            }
            return mTelephony.getDeviceId();
        }
    }

    private void processControl() {
        this.btnLogin.setOnClickListener(new View.OnClickListener() {
            public void onClick(View v) {
                if (MainActivity.BASE_URL.equals("")) {
                    Toast.makeText(MainActivity.this.mContext, "Don't set host and port api", 0).show();
                } else if (MainActivity.this.lblTabletName.getText().equals("...")) {
                    Toast.makeText(MainActivity.this.mContext, "Don't set tablet name", 0).show();
                } else {
                    MainActivity.progressDialog.showLoadingDialog();
                    LoginDto body = new LoginDto();
                    body.setUsername(MainActivity.this.txtUsername.getText().toString());
                    body.setPassword(MainActivity.this.txtPassword.getText().toString());
                    try {
                        AuthUserViewModel result = (AuthUserViewModel) new apiLoginAsync(MainActivity.this.mContext, body).execute(new String[]{MainActivity.BASE_URL}).get();
                        if (result != null) {
                            Common common = new Common(MainActivity.this.mContext);
                            if (MainActivity.this.cbRemember.isChecked()) {
                                common.setData(Constant.USERNAME, body.getUsername(), 1);
                                common.setData(Constant.PASSWORD, body.getPassword(), 1);
                                common.setData(Constant.REMEMBER, true, 2);
                            } else {
                                common.setData(Constant.USERNAME, "", 1);
                                common.setData(Constant.PASSWORD, "", 1);
                                common.setData(Constant.REMEMBER, false, 2);
                            }
                            MainActivity.TOKEN = result.getRefreshToken();
                            MainActivity.FULL_NAME = result.getFullName();
                            ((Activity) MainActivity.this.mContext).startActivityForResult(new Intent(MainActivity.this.mContext, RequestActivity.class), 1);
                            MainActivity.CURRENT_ACTIVITY = 2;
                            return;
                        }
                        MainActivity.progressDialog.dismissProgressDialog();
                    } catch (ExecutionException e) {
                        e.printStackTrace();
                    } catch (InterruptedException e2) {
                        e2.printStackTrace();
                    }
                }
            }
        });
        this.btnSetting.setOnClickListener(new View.OnClickListener() {
            public void onClick(View v) {
                ((Activity) MainActivity.this.mContext).startActivityForResult(new Intent(MainActivity.this, SettingActivity.class), 3);
                MainActivity.CURRENT_ACTIVITY = 0;
            }
        });
        this.btnQRScan.setOnClickListener(new View.OnClickListener() {
            public void onClick(View v) {
                IntentIntegrator integrator = new IntentIntegrator((Activity) MainActivity.this.mContext);
                integrator.setPrompt("Scan the QR Code in the frame");
                integrator.setCameraId(0);
                integrator.initiateScan();
                MainActivity.CURRENT_ACTIVITY = 0;
            }
        });
    }

    private void addControl() {
        this.btnLogin = (Button) findViewById(R.id.btnLogin);
        this.btnSetting = (Button) findViewById(R.id.btnSetting);
        this.btnQRScan = (Button) findViewById(R.id.btnQRScan);
        this.txtUsername = (EditText) findViewById(R.id.txtUsername);
        this.txtPassword = (EditText) findViewById(R.id.txtPassword);
        this.cbRemember = (CheckBox) findViewById(R.id.cbRemember);
        this.lblTabletName = (TextView) findViewById(R.id.lblTabletName);
        this.lblIME = (TextView) findViewById(R.id.lblIME);
        this.mContext = this;
        progressDialog = new progressDialog(this.mContext);
        CURRENT_ACTIVITY = 1;
    }

    private void initSocket() {
        this.networkChangeReceiver = new NetworkChangeReceiver();
        IntentFilter intentFilter = new IntentFilter();
        intentFilter.addAction(NetworkChangeReceiver.ACTION_FIRST_ACTION);
        registerReceiver(this.networkChangeReceiver, intentFilter);
        webSocketClient = new WebSocketClient(this.mContext);
    }

    public void processRFID(String value) {
    }

    public void staffRFID(Long idStaff) {
    }
}
