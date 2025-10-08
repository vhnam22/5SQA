package com.exampanle.ky_thuat.manualclient.activity;

import android.content.Context;
import android.os.Build;
import android.os.Bundle;
import android.view.View;
import android.widget.ArrayAdapter;
import android.widget.Button;
import android.widget.CheckBox;
import android.widget.EditText;
import android.widget.LinearLayout;
import android.widget.Spinner;
import android.widget.Toast;
import androidx.appcompat.app.AppCompatActivity;
import androidx.recyclerview.widget.LinearLayoutManager;
import androidx.recyclerview.widget.RecyclerView;
import com.exampanle.ky_thuat.manualclient.R;
import com.exampanle.ky_thuat.manualclient.adapter.MachineTypeAdapter;
import com.exampanle.ky_thuat.manualclient.adapter.ResultConfigAdapter;
import com.exampanle.ky_thuat.manualclient.adapter.progressDialog;
import com.exampanle.ky_thuat.manualclient.apiclient.JSON;
import com.exampanle.ky_thuat.manualclient.apiclient.api.apiGetMacTypeAsync;
import com.exampanle.ky_thuat.manualclient.apiclient.api.apiGetMachineAsync;
import com.exampanle.ky_thuat.manualclient.apiclient.model.ConfigDto;
import com.exampanle.ky_thuat.manualclient.apiclient.model.MachineViewModel;
import com.exampanle.ky_thuat.manualclient.apiclient.model.MetadataValueViewModel;
import com.exampanle.ky_thuat.manualclient.apiclient.model.QueryArgs;
import com.exampanle.ky_thuat.manualclient.ultil.Common;
import com.exampanle.ky_thuat.manualclient.ultil.Constant;
import com.google.gson.reflect.TypeToken;
import java.util.ArrayList;
import java.util.Arrays;
import java.util.List;
import java.util.UUID;
import java.util.concurrent.ExecutionException;

public class SettingActivity extends AppCompatActivity {
    public static ArrayList<String> listMachineStr;
    private Button btnAdvance;
    private Button btnChart;
    private Button btnClear;
    private Button btnLogin;
    private Button btnMachineType;
    private Button btnResultConfig;
    private Button btnTablet;
    private Button btnUrlAPI;
    /* access modifiers changed from: private */
    public CheckBox cbChart;
    /* access modifiers changed from: private */
    public Spinner cbbChart;
    /* access modifiers changed from: private */
    public Spinner cbbTablet;
    /* access modifiers changed from: private */
    public LinearLayout linelayoutAdvance;
    /* access modifiers changed from: private */
    public LinearLayout linelayoutDatabase;
    /* access modifiers changed from: private */
    public LinearLayout linelayoutLogin;
    /* access modifiers changed from: private */
    public ArrayList<UUID> listIdTablet;
    private List<MetadataValueViewModel> listMachineType;
    /* access modifiers changed from: private */
    public List<ConfigDto> listResultConfig;
    private ArrayList<ConfigDto> listResultStr;
    /* access modifiers changed from: private */
    public Common mCommon;
    /* access modifiers changed from: private */
    public Context mContext;
    private MachineTypeAdapter machineTypeAdapter;
    /* access modifiers changed from: private */
    public progressDialog progressDialog;
    private RecyclerView rcvMachineType;
    private RecyclerView rcvResultConfig;
    private ResultConfigAdapter resultConfigAdapter;
    /* access modifiers changed from: private */
    public EditText txtPassword;
    /* access modifiers changed from: private */
    public EditText txtUrlAPI;

    /* access modifiers changed from: protected */
    public void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView((int) R.layout.activity_setting);
        addControls();
        load_ListMachineType();
        load_ListResultConfig();
        processControls();
    }

    /* access modifiers changed from: protected */
    public void onResume() {
        super.onResume();
        ActivityManager.setCurrentActivity(this);
    }

    private void processControls() {
        this.btnLogin.setOnClickListener(new View.OnClickListener() {
            public void onClick(View v) {
                if (SettingActivity.this.txtPassword.getText().toString().equals("1234567AA")) {
                    SettingActivity.this.txtUrlAPI.setText((String) SettingActivity.this.mCommon.getData(Constant.URLAPI, 1));
                    SettingActivity.this.linelayoutLogin.setVisibility(8);
                    SettingActivity.this.linelayoutDatabase.setVisibility(0);
                    SettingActivity.this.linelayoutAdvance.setVisibility(8);
                    return;
                }
                Toast.makeText(SettingActivity.this.mContext, "Password incorrect", 0).show();
            }
        });
        this.btnAdvance.setOnClickListener(new View.OnClickListener() {
            public void onClick(View v) {
                SettingActivity.this.progressDialog.showLoadingDialog();
                SettingActivity.this.linelayoutLogin.setVisibility(8);
                SettingActivity.this.linelayoutDatabase.setVisibility(8);
                SettingActivity.this.linelayoutAdvance.setVisibility(0);
                SettingActivity.this.load_cbbTablet();
                SettingActivity.this.load_rcvMachineType();
                SettingActivity.this.load_rcvResultConfig();
                SettingActivity.this.load_ChartConfig();
                SettingActivity.this.progressDialog.dismissProgressDialog();
            }
        });
        this.btnUrlAPI.setOnClickListener(new View.OnClickListener() {
            public void onClick(View v) {
                SettingActivity.this.mCommon.setData(Constant.URLAPI, SettingActivity.this.txtUrlAPI.getText().toString(), 1);
                Toast.makeText(SettingActivity.this.mContext, "Set url api successful", 0).show();
            }
        });
        this.btnTablet.setOnClickListener(new View.OnClickListener() {
            public void onClick(View v) {
                if (SettingActivity.this.listIdTablet != null) {
                    SettingActivity.this.mCommon.setData(Constant.TABLETID, ((UUID) SettingActivity.this.listIdTablet.get(SettingActivity.this.cbbTablet.getSelectedItemPosition())).toString(), 1);
                    SettingActivity.this.mCommon.setData(Constant.TABLETMARK, SettingActivity.this.cbbTablet.getSelectedItem().toString(), 1);
                    Toast.makeText(SettingActivity.this.mContext, "Set tablet name successful", 0).show();
                    return;
                }
                Toast.makeText(SettingActivity.this.mContext, "Hasn't machine tablet", 0).show();
            }
        });
        this.btnMachineType.setOnClickListener(new View.OnClickListener() {
            public void onClick(View v) {
                String json = "";
                if (SettingActivity.listMachineStr != null) {
                    json = new JSON().serialize(SettingActivity.listMachineStr);
                }
                SettingActivity.this.mCommon.setData(Constant.MACHINETYPELIST, json, 1);
                Toast.makeText(SettingActivity.this.mContext, "Set machine types for table successful", 0).show();
            }
        });
        this.btnResultConfig.setOnClickListener(new View.OnClickListener() {
            public void onClick(View v) {
                String json = "";
                if (SettingActivity.this.listResultConfig != null) {
                    json = new JSON().serialize(SettingActivity.this.listResultConfig);
                }
                SettingActivity.this.mCommon.setData(Constant.RESULTCONFIG, json, 1);
                Toast.makeText(SettingActivity.this.mContext, "Set config for result successful", 0).show();
            }
        });
        this.btnClear.setOnClickListener(new View.OnClickListener() {
            public void onClick(View v) {
                SettingActivity.this.mCommon.clearAllData();
                SettingActivity.this.finish();
            }
        });
        this.btnChart.setOnClickListener(new View.OnClickListener() {
            public void onClick(View v) {
                int limit = Integer.parseInt(SettingActivity.this.cbbChart.getSelectedItem().toString());
                boolean isshow = SettingActivity.this.cbChart.isChecked();
                SettingActivity.this.mCommon.setData(Constant.LIMITITEMCHART, Integer.valueOf(limit), 3);
                SettingActivity.this.mCommon.setData(Constant.SHOWCHART, Boolean.valueOf(isshow), 2);
                Toast.makeText(SettingActivity.this.mContext, "Set chart config successful", 0).show();
            }
        });
    }

    /* access modifiers changed from: private */
    public void load_cbbTablet() {
        QueryArgs body = new QueryArgs();
        body.setPredicate("machinetype.name=@0");
        body.setPredicateParameters(Arrays.asList(new Object[]{"Tablet"}));
        body.setOrder("Created");
        body.setLimit(Integer.MAX_VALUE);
        body.setPage(1);
        try {
            List<MachineViewModel> result = (List) new apiGetMachineAsync(this.mContext, body).execute(new String[]{MainActivity.BASE_URL}).get();
            if (result != null) {
                this.listIdTablet = new ArrayList<>();
                ArrayList<String> listTablet = new ArrayList<>();
                for (MachineViewModel machine : result) {
                    listTablet.add(machine.getName());
                    this.listIdTablet.add(machine.getId());
                }
                ArrayAdapter<String> adapter = new ArrayAdapter<>(this.mContext, 17367048, listTablet);
                adapter.setDropDownViewResource(17367055);
                this.cbbTablet.setAdapter(adapter);
                this.cbbTablet.setSelection(listTablet.indexOf(this.mCommon.getData(Constant.TABLETMARK, 1)));
            }
        } catch (ExecutionException e) {
            e.printStackTrace();
        } catch (InterruptedException e2) {
            e2.printStackTrace();
        }
    }

    /* access modifiers changed from: private */
    public void load_rcvMachineType() {
        QueryArgs body = new QueryArgs();
        body.setPredicate("name!=@0 && type.code=@1");
        body.setPredicateParameters(Arrays.asList(new Object[]{"Tablet", "MACHINETYPE"}));
        body.setOrder("Created");
        body.setLimit(Integer.MAX_VALUE);
        body.setPage(1);
        try {
            List<MetadataValueViewModel> result = (List) new apiGetMacTypeAsync(this.mContext, body).execute(new String[]{MainActivity.BASE_URL}).get();
            if (result != null) {
                this.listMachineType = result;
                MachineTypeAdapter machineTypeAdapter2 = new MachineTypeAdapter(this.listMachineType);
                this.machineTypeAdapter = machineTypeAdapter2;
                this.rcvMachineType.setAdapter(machineTypeAdapter2);
            }
        } catch (ExecutionException e) {
            e.printStackTrace();
        } catch (InterruptedException e2) {
            e2.printStackTrace();
        }
    }

    /* access modifiers changed from: private */
    public void load_rcvResultConfig() {
        ConfigDto check;
        for (String item : Arrays.asList(new String[]{"No", "Dimension", "Sample", "Important", "Nominal", "Tolerance", Constant.LSL, Constant.USL, "Unit", "MachineType", "Cavity", "Result", "Judge"})) {
            ConfigDto config = new ConfigDto(item, (String) null, false);
            if (Build.VERSION.SDK_INT >= 24 && (check = (ConfigDto) this.listResultStr.stream().filter(new SettingActivity$$ExternalSyntheticLambda0(item)).findFirst().orElse((Object) null)) != null) {
                config.setDisplayName(check.getDisplayName());
                config.setShow(check.isShow());
            }
            this.listResultConfig.add(config);
        }
        ResultConfigAdapter resultConfigAdapter2 = new ResultConfigAdapter(this.listResultConfig);
        this.resultConfigAdapter = resultConfigAdapter2;
        this.rcvResultConfig.setAdapter(resultConfigAdapter2);
    }

    /* access modifiers changed from: private */
    public void load_ChartConfig() {
        int item = ((Integer) this.mCommon.getData(Constant.LIMITITEMCHART, 3)).intValue();
        Spinner spinner = this.cbbChart;
        spinner.setSelection(((ArrayAdapter) spinner.getAdapter()).getPosition(String.valueOf(item)));
        this.cbChart.setChecked(((Boolean) this.mCommon.getData(Constant.SHOWCHART, 2)).booleanValue());
    }

    private void load_ListMachineType() {
        String list = (String) this.mCommon.getData(Constant.MACHINETYPELIST, 1);
        if (list != "") {
            listMachineStr = (ArrayList) new JSON().deserialize(list, new TypeToken<ArrayList<String>>() {
            }.getType());
        }
    }

    private void load_ListResultConfig() {
        String list = (String) this.mCommon.getData(Constant.RESULTCONFIG, 1);
        if (list != "") {
            this.listResultStr = (ArrayList) new JSON().deserialize(list, new TypeToken<ArrayList<ConfigDto>>() {
            }.getType());
        }
    }

    private void addControls() {
        this.txtPassword = (EditText) findViewById(R.id.txtPassword);
        this.txtUrlAPI = (EditText) findViewById(R.id.txtUrlAPI);
        this.btnLogin = (Button) findViewById(R.id.btnLogin);
        this.btnUrlAPI = (Button) findViewById(R.id.btnUrlAPI);
        this.btnMachineType = (Button) findViewById(R.id.btnMachineType);
        this.btnAdvance = (Button) findViewById(R.id.btnAdvance);
        this.btnResultConfig = (Button) findViewById(R.id.btnResultConfig);
        this.btnTablet = (Button) findViewById(R.id.btnTablet);
        this.btnClear = (Button) findViewById(R.id.btnClear);
        this.linelayoutLogin = (LinearLayout) findViewById(R.id.linelayoutLogin);
        this.linelayoutDatabase = (LinearLayout) findViewById(R.id.linelayoutDatabase);
        this.linelayoutAdvance = (LinearLayout) findViewById(R.id.linelayoutAdvance);
        this.cbbTablet = (Spinner) findViewById(R.id.cbbTablet);
        this.rcvMachineType = (RecyclerView) findViewById(R.id.rcvMachineType);
        this.rcvResultConfig = (RecyclerView) findViewById(R.id.rcvResultConfig);
        this.cbbChart = (Spinner) findViewById(R.id.cbbChart);
        this.cbChart = (CheckBox) findViewById(R.id.cbChart);
        this.btnChart = (Button) findViewById(R.id.btnChart);
        this.linelayoutLogin.setVisibility(0);
        this.linelayoutDatabase.setVisibility(8);
        this.linelayoutAdvance.setVisibility(8);
        this.mContext = this;
        this.progressDialog = new progressDialog(this.mContext);
        this.mCommon = new Common(this.mContext);
        listMachineStr = new ArrayList<>();
        this.listResultStr = new ArrayList<>();
        this.listResultConfig = new ArrayList();
        this.rcvMachineType.setLayoutManager(new LinearLayoutManager(this));
        this.rcvResultConfig.setLayoutManager(new LinearLayoutManager(this));
    }
}
