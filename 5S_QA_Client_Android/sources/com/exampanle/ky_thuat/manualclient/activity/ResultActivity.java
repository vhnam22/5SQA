package com.exampanle.ky_thuat.manualclient.activity;

import android.app.Activity;
import android.content.Context;
import android.content.Intent;
import android.graphics.BitmapFactory;
import android.media.ToneGenerator;
import android.os.Bundle;
import android.text.Editable;
import android.text.TextWatcher;
import android.view.KeyEvent;
import android.view.View;
import android.widget.AdapterView;
import android.widget.ArrayAdapter;
import android.widget.Button;
import android.widget.CompoundButton;
import android.widget.EditText;
import android.widget.LinearLayout;
import android.widget.RadioButton;
import android.widget.Spinner;
import android.widget.TextView;
import android.widget.Toast;
import androidx.appcompat.app.AppCompatActivity;
import androidx.coordinatorlayout.widget.CoordinatorLayout;
import androidx.core.view.GravityCompat;
import androidx.drawerlayout.widget.DrawerLayout;
import androidx.recyclerview.widget.LinearLayoutManager;
import androidx.recyclerview.widget.RecyclerView;
import com.exampanle.ky_thuat.manualclient.R;
import com.exampanle.ky_thuat.manualclient.adapter.ResultAdapter;
import com.exampanle.ky_thuat.manualclient.apiclient.JSON;
import com.exampanle.ky_thuat.manualclient.apiclient.api.apiGetImageAsync;
import com.exampanle.ky_thuat.manualclient.apiclient.api.apiGetMeasurementAsync;
import com.exampanle.ky_thuat.manualclient.apiclient.api.apiGetRequestDetailAsync;
import com.exampanle.ky_thuat.manualclient.apiclient.api.apiGetResultConditionAsync;
import com.exampanle.ky_thuat.manualclient.apiclient.api.apiGetResultForMeasAsync;
import com.exampanle.ky_thuat.manualclient.apiclient.api.apiGetResultSampleCompleteAsync;
import com.exampanle.ky_thuat.manualclient.apiclient.api.apiSetRequestDetailAsync;
import com.exampanle.ky_thuat.manualclient.apiclient.api.apiSetResultAsync;
import com.exampanle.ky_thuat.manualclient.apiclient.model.ConfigDto;
import com.exampanle.ky_thuat.manualclient.apiclient.model.ReceiveResult;
import com.exampanle.ky_thuat.manualclient.apiclient.model.RequestDetailViewModel;
import com.exampanle.ky_thuat.manualclient.apiclient.model.RequestViewModel;
import com.exampanle.ky_thuat.manualclient.apiclient.model.ResponseDto;
import com.exampanle.ky_thuat.manualclient.apiclient.model.ResultViewModel;
import com.exampanle.ky_thuat.manualclient.ultil.Common;
import com.exampanle.ky_thuat.manualclient.ultil.Constant;
import com.exampanle.ky_thuat.manualclient.ultil.ProcessUI;
import com.github.chrisbanes.photoview.PhotoView;
import com.google.gson.reflect.TypeToken;
import com.google.zxing.integration.android.IntentIntegrator;
import com.google.zxing.integration.android.IntentResult;
import java.util.ArrayList;
import java.util.List;
import java.util.UUID;
import java.util.concurrent.ExecutionException;

public class ResultActivity extends AppCompatActivity {
    public static int CURRENT_PAGE;
    public static int ITEMS_PER_PAGE;
    public static boolean firstAuto = true;
    public static ArrayList<ConfigDto> listResultConfig;
    public static boolean mIsShowChart;
    public static int mItemChart;
    public static int numAuto = 0;
    public static String tempResult = "";
    public static EditText txtSearch;
    /* access modifiers changed from: private */
    public int TOTAL_PAGES;
    private int TOTAL_ROWS;
    private Button btnBack;
    private Button btnClose;
    private Button btnComment;
    private Button btnKeyboard;
    private Button btnNextPage;
    /* access modifiers changed from: private */
    public Button btnNextSample;
    private Button btnPrePage;
    /* access modifiers changed from: private */
    public Button btnPreSample;
    private Button btnProductImage;
    private Button btnQRScan;
    private Button btnRequest;
    private Button btnResult;
    private Button btnSave;
    private Button btnSearch;
    /* access modifiers changed from: private */
    public Button btnSearchDelete;
    /* access modifiers changed from: private */
    public Button btnSort;
    private Button btnToolRegister;
    private Button btnZoom;
    private Spinner cbbItem;
    /* access modifiers changed from: private */
    public Spinner cbbSample;
    /* access modifiers changed from: private */
    public DrawerLayout drawerLayoutResult;
    /* access modifiers changed from: private */
    public boolean firstData = true;
    private PhotoView imgProductImage;
    private boolean isSaveResultFinish = true;
    /* access modifiers changed from: private */
    public LinearLayout layoutHistory;
    /* access modifiers changed from: private */
    public LinearLayout layoutKeyPad;
    private TextView lblCavi;
    private TextView lblEdit;
    private TextView lblEmpty;
    private TextView lblImportant;
    private TextView lblJudge;
    /* access modifiers changed from: private */
    public TextView lblKey;
    private TextView lblLSL;
    private TextView lblMachineType;
    private TextView lblMeasurement;
    private TextView lblNG;
    private TextView lblNo;
    private TextView lblNominal;
    private TextView lblOK;
    private TextView lblRequestMark;
    private TextView lblResult;
    private TextView lblSample;
    private TextView lblStaff;
    private TextView lblTolerance;
    private TextView lblTotal;
    private TextView lblTotalPage;
    private TextView lblUSL;
    private TextView lblUnit;
    private TextView lblWarning;
    private LinearLayoutManager linearLayoutManager;
    /* access modifiers changed from: private */
    public LinearLayout linearlayoutKeyNumber;
    /* access modifiers changed from: private */
    public LinearLayout linearlayoutKeyOkNg;
    private List<String> listMachineType;
    /* access modifiers changed from: private */
    public ArrayList<ResultViewModel> listResult;
    private ArrayList<ResultViewModel> listResultTotal;
    /* access modifiers changed from: private */
    public Context mContext = null;
    /* access modifiers changed from: private */
    public UUID mIdDetail;
    /* access modifiers changed from: private */
    public RequestViewModel mRequest;
    /* access modifiers changed from: private */
    public boolean mSort;
    private LinearLayout panelChart;
    private LinearLayout panelDetail;
    /* access modifiers changed from: private */
    public CoordinatorLayout panelImage;
    private LinearLayout panelMeasurement;
    private RadioButton rbMeas;
    private RadioButton rbSample;
    private RecyclerView rcvResult;
    private ResultAdapter resultAdapter;
    /* access modifiers changed from: private */
    public EditText txtCavity;
    /* access modifiers changed from: private */
    public EditText txtPinDate;
    /* access modifiers changed from: private */
    public EditText txtProduceNo;
    /* access modifiers changed from: private */
    public EditText txtShotMold;

    /* access modifiers changed from: protected */
    public void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView((int) R.layout.activity_result);
        addControls();
        load_ChartConfig();
        load_ListResultConfig();
        loadData();
        proccessControl();
    }

    /* access modifiers changed from: protected */
    public void onResume() {
        super.onResume();
        ActivityManager.setCurrentActivity(this);
    }

    /* access modifiers changed from: protected */
    public void onStart() {
        super.onStart();
        RequestActivity.progressDialog.dismissProgressDialog();
    }

    /* access modifiers changed from: protected */
    public void onActivityResult(int requestCode, int resultCode, Intent data) {
        super.onActivityResult(requestCode, resultCode, data);
        if (requestCode == 2 && MainActivity.CURRENT_ACTIVITY == 3) {
            finish();
        }
        MainActivity.CURRENT_ACTIVITY = 3;
        IntentResult result = IntentIntegrator.parseActivityResult(requestCode, resultCode, data);
        if (result != null && result.getContents() != null) {
            tempResult = result.getContents();
            saveResult();
        }
    }

    public boolean onKeyUp(int keyCode, KeyEvent event) {
        if (keyCode == 4 || !this.drawerLayoutResult.isDrawerOpen((int) GravityCompat.END) || this.layoutKeyPad.getVisibility() != 0) {
            return super.onKeyUp(keyCode, event);
        }
        this.drawerLayoutResult.clearFocus();
        if (this.linearlayoutKeyNumber.getVisibility() != 0) {
            switch (keyCode) {
                case 70:
                    tempResult = Constant.OK;
                    this.lblKey.setText(Constant.OK);
                    saveResult();
                    break;
                case 154:
                    tempResult = Constant.NG;
                    this.lblKey.setText(Constant.NG);
                    saveResult();
                    break;
            }
        } else {
            switch (keyCode) {
                case 67:
                    if (tempResult.equals(Constant.OK) || tempResult.equals(Constant.NG)) {
                        tempResult = "";
                    } else if (this.firstData) {
                        tempResult = "";
                    } else if (tempResult.length() <= 1) {
                        tempResult = "";
                    } else if (tempResult.length() != 2 || !tempResult.contains("-")) {
                        String str = tempResult;
                        tempResult = str.substring(0, str.length() - 1);
                    } else {
                        tempResult = "";
                    }
                    this.firstData = false;
                    this.lblKey.setText(tempResult);
                    return true;
                case 156:
                    if (tempResult.length() > 8) {
                        return true;
                    }
                    if (tempResult.contains("-")) {
                        tempResult = tempResult.replace("-", "");
                    } else {
                        tempResult = "-" + tempResult;
                    }
                    this.firstData = false;
                    this.lblKey.setText(tempResult);
                    return true;
                case 160:
                    saveResult();
                    return true;
                default:
                    if (keyCode == 144 || keyCode == 145 || keyCode == 146 || keyCode == 147 || keyCode == 148 || keyCode == 149 || keyCode == 150 || keyCode == 151 || keyCode == 152 || keyCode == 153 || keyCode == 158) {
                        if (tempResult.equals(Constant.OK) || tempResult.equals(Constant.NG) || tempResult.equals("") || tempResult.equals("0") || tempResult.equals("-0")) {
                            if (tempResult.equals("-0")) {
                                tempResult = "-";
                            } else {
                                tempResult = "0";
                            }
                        }
                        if (tempResult.length() <= 7) {
                            switch (keyCode) {
                                case 144:
                                    if (tempResult == "0") {
                                        tempResult = "0";
                                        break;
                                    } else {
                                        tempResult += "0";
                                        break;
                                    }
                                case 145:
                                    if (tempResult == "0") {
                                        tempResult = "1";
                                        break;
                                    } else {
                                        tempResult += "1";
                                        break;
                                    }
                                case 146:
                                    if (tempResult == "0") {
                                        tempResult = "2";
                                        break;
                                    } else {
                                        tempResult += "2";
                                        break;
                                    }
                                case 147:
                                    if (tempResult == "0") {
                                        tempResult = "3";
                                        break;
                                    } else {
                                        tempResult += "3";
                                        break;
                                    }
                                case 148:
                                    if (tempResult == "0") {
                                        tempResult = "4";
                                        break;
                                    } else {
                                        tempResult += "4";
                                        break;
                                    }
                                case 149:
                                    if (tempResult == "0") {
                                        tempResult = "5";
                                        break;
                                    } else {
                                        tempResult += "5";
                                        break;
                                    }
                                case 150:
                                    if (tempResult == "0") {
                                        tempResult = "6";
                                        break;
                                    } else {
                                        tempResult += "6";
                                        break;
                                    }
                                case 151:
                                    if (tempResult == "0") {
                                        tempResult = "7";
                                        break;
                                    } else {
                                        tempResult += "7";
                                        break;
                                    }
                                case 152:
                                    if (tempResult == "0") {
                                        tempResult = "8";
                                        break;
                                    } else {
                                        tempResult += "8";
                                        break;
                                    }
                                case 153:
                                    if (tempResult == "0") {
                                        tempResult = "9";
                                        break;
                                    } else {
                                        tempResult += "9";
                                        break;
                                    }
                                case 158:
                                    if (!tempResult.contains(".")) {
                                        tempResult += ".";
                                        break;
                                    } else {
                                        return true;
                                    }
                            }
                            this.firstData = false;
                            this.lblKey.setText(tempResult);
                            break;
                        } else {
                            return true;
                        }
                    } else {
                        return true;
                    }
            }
        }
        return true;
    }

    private void loadData() {
        Common common = new Common(this.mContext);
        String strMacType = (String) common.getData(Constant.MACHINETYPELIST, "", 1);
        if (((Boolean) common.getData(Constant.TYPEMEAS, true, 2)).booleanValue()) {
            this.rbMeas.setChecked(true);
        } else {
            this.rbSample.setChecked(true);
        }
        this.listMachineType = (List) new JSON().deserialize(strMacType, new TypeToken<ArrayList<String>>() {
        }.getType());
        this.mSort = false;
        RequestViewModel requestViewModel = (RequestViewModel) getIntent().getParcelableExtra("REQUEST");
        this.mRequest = requestViewModel;
        this.lblRequestMark.setText(requestViewModel.getName());
        this.lblStaff.setText(MainActivity.FULL_NAME + " is logging");
        if (this.mRequest.getProductImageUrl() == null) {
            this.btnProductImage.setVisibility(4);
        } else {
            try {
                byte[] result = (byte[]) new apiGetImageAsync().execute(new String[]{MainActivity.BASE_URL + "/ProductImage/" + this.mRequest.getProductImageUrl()}).get();
                if (result != null && result.length > 0) {
                    this.imgProductImage.setImageBitmap(BitmapFactory.decodeByteArray(result, 0, result.length));
                    this.imgProductImage.setVisibility(0);
                }
            } catch (ExecutionException e) {
                e.printStackTrace();
            } catch (InterruptedException e2) {
                e2.printStackTrace();
            }
        }
        CURRENT_PAGE = 1;
        ITEMS_PER_PAGE = ((Integer) common.getData(Constant.NUMPERPAGE_RESULT, 3)).intValue();
        Spinner spinner = this.cbbItem;
        spinner.setSelection(((ArrayAdapter) spinner.getAdapter()).getPosition(String.valueOf(ITEMS_PER_PAGE)));
    }

    private void loadcbbSample() {
        List<ResultViewModel> listresults = new ArrayList<>();
        if (this.rbMeas.isChecked()) {
            try {
                listresults = (List) new apiGetMeasurementAsync(this.mContext, this.mRequest.getProductId(), this.listMachineType).execute(new String[]{MainActivity.BASE_URL}).get();
            } catch (ExecutionException e) {
                e.printStackTrace();
            } catch (InterruptedException e2) {
                e2.printStackTrace();
            }
        } else {
            for (int i = 1; i <= this.mRequest.getSample(); i++) {
                ResultViewModel result = new ResultViewModel();
                result.setMeasurementCode(String.valueOf(i));
                listresults.add(result);
            }
        }
        ArrayAdapter<ResultViewModel> adapter = new ArrayAdapter<>(this.mContext, 17367048, listresults);
        adapter.setDropDownViewResource(17367055);
        this.cbbSample.setAdapter(adapter);
        if (this.rbMeas.isChecked()) {
            this.cbbSample.setSelection(setMeasComplete().intValue());
        } else {
            this.cbbSample.setSelection(setSampleComplete().intValue());
        }
    }

    private Integer setSampleComplete() {
        try {
            return Integer.valueOf(((ResponseDto) new apiGetResultSampleCompleteAsync(this.mContext, this.mRequest.getId(), this.listMachineType).execute(new String[]{MainActivity.BASE_URL}).get()).getCount().intValue() - 1);
        } catch (ExecutionException e) {
            e.printStackTrace();
            return 0;
        } catch (InterruptedException e2) {
            e2.printStackTrace();
            return 0;
        }
    }

    private Integer setMeasComplete() {
        try {
            JSON json = new JSON();
            ResultViewModel results = (ResultViewModel) json.deserialize(json.serialize(((ResponseDto) new apiGetResultSampleCompleteAsync(this.mContext, this.mRequest.getId(), this.listMachineType).execute(new String[]{MainActivity.BASE_URL}).get()).getData()), new TypeToken<ResultViewModel>() {
            }.getType());
            ArrayAdapter<ResultViewModel> temps = (ArrayAdapter) this.cbbSample.getAdapter();
            for (int i = 0; i < temps.getCount(); i++) {
                if (temps.getItem(i).getMeasurementId().equals(results.getMeasurementId())) {
                    return Integer.valueOf(i);
                }
            }
            return 0;
        } catch (ExecutionException e) {
            e.printStackTrace();
            return 0;
        } catch (InterruptedException e2) {
            e2.printStackTrace();
            return 0;
        }
    }

    public void onBackPressed() {
        setResult(-1);
        firstAuto = true;
        numAuto = 0;
        super.onBackPressed();
    }

    private void proccessControl() {
        loadcbbSample();
        processNavigationview();
    }

    /* access modifiers changed from: private */
    public void saveType(Boolean check) {
        new Common(this.mContext).setData(Constant.TYPEMEAS, check, 2);
        loadcbbSample();
    }

    private void processNavigationview() {
        final Button btnKey0 = (Button) findViewById(R.id.btnKey0);
        final Button btnKey1 = (Button) findViewById(R.id.btnKey1);
        final Button btnKey2 = (Button) findViewById(R.id.btnKey2);
        final Button btnKey3 = (Button) findViewById(R.id.btnKey3);
        final Button btnKey4 = (Button) findViewById(R.id.btnKey4);
        final Button btnKey5 = (Button) findViewById(R.id.btnKey5);
        final Button btnKey6 = (Button) findViewById(R.id.btnKey6);
        final Button btnKey7 = (Button) findViewById(R.id.btnKey7);
        final Button btnKey8 = (Button) findViewById(R.id.btnKey8);
        final Button btnKey9 = (Button) findViewById(R.id.btnKey9);
        Button btnKeyNG = (Button) findViewById(R.id.btnKeyNG);
        final Button btnKeyIN = (Button) findViewById(R.id.btnKeyIN);
        Button btnKeyOK = (Button) findViewById(R.id.btnKeyOK);
        final Button btnKeySTOP = (Button) findViewById(R.id.btnKeySTOP);
        Button btnKeyDelete = (Button) findViewById(R.id.btnKeyDelete);
        final Button btnKeyGO = (Button) findViewById(R.id.btnKeyGO);
        Button btnKeyAbs = (Button) findViewById(R.id.btnKeyAbs);
        Button btnKeyNOGO = (Button) findViewById(R.id.btnKeyNOGO);
        Button btnKeyDot = (Button) findViewById(R.id.btnKeyDot);
        btnKeyIN.setVisibility(8);
        btnKeySTOP.setVisibility(8);
        btnKeyGO.setVisibility(8);
        btnKeyNOGO.setVisibility(8);
        Button btnKeyEnter = (Button) findViewById(R.id.btnKeyEnter);
        this.lblKey = (TextView) findViewById(R.id.lblKey);
        Button btnKeyNOGO2 = btnKeyNOGO;
        this.rbMeas.setOnCheckedChangeListener(new CompoundButton.OnCheckedChangeListener() {
            public void onCheckedChanged(CompoundButton compoundButton, boolean b) {
                ResultActivity.this.saveType(Boolean.valueOf(b));
            }
        });
        btnKey0.setOnClickListener(new View.OnClickListener() {
            public void onClick(View v) {
                if (ResultActivity.tempResult.equals(Constant.OK) || ResultActivity.tempResult.equals(Constant.NG) || ResultActivity.tempResult.equals("") || ResultActivity.tempResult.equals("0") || ResultActivity.tempResult.equals("-0")) {
                    if (ResultActivity.tempResult.equals("-0")) {
                        ResultActivity.tempResult = "-";
                    } else {
                        ResultActivity.tempResult = "0";
                    }
                }
                if (ResultActivity.tempResult.length() <= 7) {
                    if (ResultActivity.tempResult != "0") {
                        ResultActivity.tempResult += "0";
                    } else {
                        ResultActivity.tempResult = "0";
                    }
                    boolean unused = ResultActivity.this.firstData = false;
                    ResultActivity.this.lblKey.setText(ResultActivity.tempResult);
                    ResultActivity.this.lblKey.setTextColor(btnKey0.getTextColors());
                }
            }
        });
        btnKey1.setOnClickListener(new View.OnClickListener() {
            public void onClick(View v) {
                if (ResultActivity.tempResult.equals(Constant.OK) || ResultActivity.tempResult.equals(Constant.NG) || ResultActivity.tempResult.equals("") || ResultActivity.tempResult.equals("0") || ResultActivity.tempResult.equals("-0")) {
                    if (ResultActivity.tempResult.equals("-0")) {
                        ResultActivity.tempResult = "-";
                    } else {
                        ResultActivity.tempResult = "0";
                    }
                }
                if (ResultActivity.tempResult.length() <= 7) {
                    if (ResultActivity.tempResult != "0") {
                        ResultActivity.tempResult += "1";
                    } else {
                        ResultActivity.tempResult = "1";
                    }
                    boolean unused = ResultActivity.this.firstData = false;
                    ResultActivity.this.lblKey.setText(ResultActivity.tempResult);
                    ResultActivity.this.lblKey.setTextColor(btnKey1.getTextColors());
                }
            }
        });
        btnKey2.setOnClickListener(new View.OnClickListener() {
            public void onClick(View v) {
                if (ResultActivity.tempResult.equals(Constant.OK) || ResultActivity.tempResult.equals(Constant.NG) || ResultActivity.tempResult.equals("") || ResultActivity.tempResult.equals("0") || ResultActivity.tempResult.equals("-0")) {
                    if (ResultActivity.tempResult.equals("-0")) {
                        ResultActivity.tempResult = "-";
                    } else {
                        ResultActivity.tempResult = "0";
                    }
                }
                if (ResultActivity.tempResult.length() <= 7) {
                    if (ResultActivity.tempResult != "0") {
                        ResultActivity.tempResult += "2";
                    } else {
                        ResultActivity.tempResult = "2";
                    }
                    boolean unused = ResultActivity.this.firstData = false;
                    ResultActivity.this.lblKey.setText(ResultActivity.tempResult);
                    ResultActivity.this.lblKey.setTextColor(btnKey2.getTextColors());
                }
            }
        });
        btnKey3.setOnClickListener(new View.OnClickListener() {
            public void onClick(View v) {
                if (ResultActivity.tempResult.equals(Constant.OK) || ResultActivity.tempResult.equals(Constant.NG) || ResultActivity.tempResult.equals("") || ResultActivity.tempResult.equals("0") || ResultActivity.tempResult.equals("-0")) {
                    if (ResultActivity.tempResult.equals("-0")) {
                        ResultActivity.tempResult = "-";
                    } else {
                        ResultActivity.tempResult = "0";
                    }
                }
                if (ResultActivity.tempResult.length() <= 7) {
                    if (ResultActivity.tempResult != "0") {
                        ResultActivity.tempResult += "3";
                    } else {
                        ResultActivity.tempResult = "3";
                    }
                    boolean unused = ResultActivity.this.firstData = false;
                    ResultActivity.this.lblKey.setText(ResultActivity.tempResult);
                    ResultActivity.this.lblKey.setTextColor(btnKey3.getTextColors());
                }
            }
        });
        btnKey4.setOnClickListener(new View.OnClickListener() {
            public void onClick(View v) {
                if (ResultActivity.tempResult.equals(Constant.OK) || ResultActivity.tempResult.equals(Constant.NG) || ResultActivity.tempResult.equals("") || ResultActivity.tempResult.equals("0") || ResultActivity.tempResult.equals("-0")) {
                    if (ResultActivity.tempResult.equals("-0")) {
                        ResultActivity.tempResult = "-";
                    } else {
                        ResultActivity.tempResult = "0";
                    }
                }
                if (ResultActivity.tempResult.length() <= 7) {
                    if (ResultActivity.tempResult != "0") {
                        ResultActivity.tempResult += "4";
                    } else {
                        ResultActivity.tempResult = "4";
                    }
                    boolean unused = ResultActivity.this.firstData = false;
                    ResultActivity.this.lblKey.setText(ResultActivity.tempResult);
                    ResultActivity.this.lblKey.setTextColor(btnKey4.getTextColors());
                }
            }
        });
        btnKey5.setOnClickListener(new View.OnClickListener() {
            public void onClick(View v) {
                if (ResultActivity.tempResult.equals(Constant.OK) || ResultActivity.tempResult.equals(Constant.NG) || ResultActivity.tempResult.equals("") || ResultActivity.tempResult.equals("0") || ResultActivity.tempResult.equals("-0")) {
                    if (ResultActivity.tempResult.equals("-0")) {
                        ResultActivity.tempResult = "-";
                    } else {
                        ResultActivity.tempResult = "0";
                    }
                }
                if (ResultActivity.tempResult.length() <= 7) {
                    if (ResultActivity.tempResult != "0") {
                        ResultActivity.tempResult += "5";
                    } else {
                        ResultActivity.tempResult = "5";
                    }
                    boolean unused = ResultActivity.this.firstData = false;
                    ResultActivity.this.lblKey.setText(ResultActivity.tempResult);
                    ResultActivity.this.lblKey.setTextColor(btnKey5.getTextColors());
                }
            }
        });
        btnKey6.setOnClickListener(new View.OnClickListener() {
            public void onClick(View v) {
                if (ResultActivity.tempResult.equals(Constant.OK) || ResultActivity.tempResult.equals(Constant.NG) || ResultActivity.tempResult.equals("") || ResultActivity.tempResult.equals("0") || ResultActivity.tempResult.equals("-0")) {
                    if (ResultActivity.tempResult.equals("-0")) {
                        ResultActivity.tempResult = "-";
                    } else {
                        ResultActivity.tempResult = "0";
                    }
                }
                if (ResultActivity.tempResult.length() <= 7) {
                    if (ResultActivity.tempResult != "0") {
                        ResultActivity.tempResult += "6";
                    } else {
                        ResultActivity.tempResult = "6";
                    }
                    boolean unused = ResultActivity.this.firstData = false;
                    ResultActivity.this.lblKey.setText(ResultActivity.tempResult);
                    ResultActivity.this.lblKey.setTextColor(btnKey6.getTextColors());
                }
            }
        });
        btnKey7.setOnClickListener(new View.OnClickListener() {
            public void onClick(View v) {
                if (ResultActivity.tempResult.equals(Constant.OK) || ResultActivity.tempResult.equals(Constant.NG) || ResultActivity.tempResult.equals("") || ResultActivity.tempResult.equals("0") || ResultActivity.tempResult.equals("-0")) {
                    if (ResultActivity.tempResult.equals("-0")) {
                        ResultActivity.tempResult = "-";
                    } else {
                        ResultActivity.tempResult = "0";
                    }
                }
                if (ResultActivity.tempResult.length() <= 7) {
                    if (ResultActivity.tempResult != "0") {
                        ResultActivity.tempResult += "7";
                    } else {
                        ResultActivity.tempResult = "7";
                    }
                    boolean unused = ResultActivity.this.firstData = false;
                    ResultActivity.this.lblKey.setText(ResultActivity.tempResult);
                    ResultActivity.this.lblKey.setTextColor(btnKey7.getTextColors());
                }
            }
        });
        btnKey8.setOnClickListener(new View.OnClickListener() {
            public void onClick(View v) {
                if (ResultActivity.tempResult.equals(Constant.OK) || ResultActivity.tempResult.equals(Constant.NG) || ResultActivity.tempResult.equals("") || ResultActivity.tempResult.equals("0") || ResultActivity.tempResult.equals("-0")) {
                    if (ResultActivity.tempResult.equals("-0")) {
                        ResultActivity.tempResult = "-";
                    } else {
                        ResultActivity.tempResult = "0";
                    }
                }
                if (ResultActivity.tempResult.length() <= 7) {
                    if (ResultActivity.tempResult != "0") {
                        ResultActivity.tempResult += "8";
                    } else {
                        ResultActivity.tempResult = "8";
                    }
                    boolean unused = ResultActivity.this.firstData = false;
                    ResultActivity.this.lblKey.setText(ResultActivity.tempResult);
                    ResultActivity.this.lblKey.setTextColor(btnKey8.getTextColors());
                }
            }
        });
        btnKey9.setOnClickListener(new View.OnClickListener() {
            public void onClick(View v) {
                if (ResultActivity.tempResult.equals(Constant.OK) || ResultActivity.tempResult.equals(Constant.NG) || ResultActivity.tempResult.equals("") || ResultActivity.tempResult.equals("0") || ResultActivity.tempResult.equals("-0")) {
                    if (ResultActivity.tempResult.equals("-0")) {
                        ResultActivity.tempResult = "-";
                    } else {
                        ResultActivity.tempResult = "0";
                    }
                }
                if (ResultActivity.tempResult.length() <= 7) {
                    if (ResultActivity.tempResult != "0") {
                        ResultActivity.tempResult += "9";
                    } else {
                        ResultActivity.tempResult = "9";
                    }
                    boolean unused = ResultActivity.this.firstData = false;
                    ResultActivity.this.lblKey.setText(ResultActivity.tempResult);
                    ResultActivity.this.lblKey.setTextColor(btnKey9.getTextColors());
                }
            }
        });
        final Button btnKeyDot2 = btnKeyDot;
        btnKeyDot2.setOnClickListener(new View.OnClickListener() {
            public void onClick(View v) {
                if (ResultActivity.tempResult.equals(Constant.OK) || ResultActivity.tempResult.equals(Constant.NG) || ResultActivity.tempResult.equals("") || ResultActivity.tempResult.equals("0") || ResultActivity.tempResult.equals("-")) {
                    if (ResultActivity.tempResult.equals("-")) {
                        ResultActivity.tempResult = "-0";
                    } else {
                        ResultActivity.tempResult = "0";
                    }
                }
                if (ResultActivity.tempResult.length() <= 7 && !ResultActivity.tempResult.contains(".")) {
                    ResultActivity.tempResult += ".";
                    boolean unused = ResultActivity.this.firstData = false;
                    ResultActivity.this.lblKey.setText(ResultActivity.tempResult);
                    ResultActivity.this.lblKey.setTextColor(btnKeyDot2.getTextColors());
                }
            }
        });
        Button button = btnKey0;
        final Button btnKeyAbs2 = btnKeyAbs;
        btnKeyAbs2.setOnClickListener(new View.OnClickListener() {
            public void onClick(View v) {
                if (ResultActivity.tempResult.length() <= 8) {
                    if (ResultActivity.tempResult.contains("-")) {
                        ResultActivity.tempResult = ResultActivity.tempResult.replace("-", "");
                    } else {
                        ResultActivity.tempResult = "-" + ResultActivity.tempResult;
                    }
                    boolean unused = ResultActivity.this.firstData = false;
                    ResultActivity.this.lblKey.setText(ResultActivity.tempResult);
                    ResultActivity.this.lblKey.setTextColor(btnKeyAbs2.getTextColors());
                }
            }
        });
        final Button btnKeyAbs3 = btnKeyDelete;
        btnKeyAbs3.setOnClickListener(new View.OnClickListener() {
            public void onClick(View v) {
                if (ResultActivity.tempResult.equals(Constant.OK) || ResultActivity.tempResult.equals(Constant.NG)) {
                    ResultActivity.tempResult = "";
                } else if (ResultActivity.this.firstData) {
                    ResultActivity.tempResult = "";
                } else if (ResultActivity.tempResult.length() <= 1) {
                    ResultActivity.tempResult = "";
                } else if (ResultActivity.tempResult.length() != 2 || !ResultActivity.tempResult.contains("-")) {
                    ResultActivity.tempResult = ResultActivity.tempResult.substring(0, ResultActivity.tempResult.length() - 1);
                } else {
                    ResultActivity.tempResult = "";
                }
                boolean unused = ResultActivity.this.firstData = false;
                ResultActivity.this.lblKey.setText(ResultActivity.tempResult);
                ResultActivity.this.lblKey.setTextColor(btnKeyAbs3.getTextColors());
            }
        });
        final Button btnKeyOK2 = btnKeyOK;
        btnKeyOK2.setOnClickListener(new View.OnClickListener() {
            public void onClick(View v) {
                ResultActivity.tempResult = Constant.OK;
                ResultActivity.this.lblKey.setText(ResultActivity.tempResult);
                ResultActivity.this.lblKey.setTextColor(btnKeyOK2.getTextColors());
                ResultActivity.this.saveResult();
            }
        });
        final Button btnKeyOK3 = btnKeyNG;
        btnKeyOK3.setOnClickListener(new View.OnClickListener() {
            public void onClick(View v) {
                ResultActivity.tempResult = Constant.NG;
                ResultActivity.this.lblKey.setText(ResultActivity.tempResult);
                ResultActivity.this.lblKey.setTextColor(btnKeyOK3.getTextColors());
                ResultActivity.this.saveResult();
            }
        });
        btnKeyGO.setOnClickListener(new View.OnClickListener() {
            public void onClick(View v) {
                ResultActivity.tempResult = Constant.GO;
                ResultActivity.this.lblKey.setText(ResultActivity.tempResult);
                ResultActivity.this.lblKey.setTextColor(btnKeyGO.getTextColors());
                ResultActivity.this.saveResult();
            }
        });
        btnKeySTOP.setOnClickListener(new View.OnClickListener() {
            public void onClick(View v) {
                ResultActivity.tempResult = Constant.STOP;
                ResultActivity.this.lblKey.setText(ResultActivity.tempResult);
                ResultActivity.this.lblKey.setTextColor(btnKeySTOP.getTextColors());
                ResultActivity.this.saveResult();
            }
        });
        btnKeyIN.setOnClickListener(new View.OnClickListener() {
            public void onClick(View v) {
                ResultActivity.tempResult = Constant.IN;
                ResultActivity.this.lblKey.setText(ResultActivity.tempResult);
                ResultActivity.this.lblKey.setTextColor(btnKeyIN.getTextColors());
                ResultActivity.this.saveResult();
            }
        });
        final Button btnKeyNOGO3 = btnKeyNOGO2;
        btnKeyNOGO3.setOnClickListener(new View.OnClickListener() {
            public void onClick(View v) {
                ResultActivity.tempResult = Constant.NOGO;
                ResultActivity.this.lblKey.setText(ResultActivity.tempResult);
                ResultActivity.this.lblKey.setTextColor(btnKeyNOGO3.getTextColors());
                ResultActivity.this.saveResult();
            }
        });
        btnKeyEnter.setOnClickListener(new View.OnClickListener() {
            public void onClick(View v) {
                ResultActivity.this.saveResult();
            }
        });
        this.btnSearch.setOnClickListener(new View.OnClickListener() {
            public void onClick(View v) {
                ResultActivity.firstAuto = true;
                ResultActivity.this.displayResult();
            }
        });
        this.btnPrePage.setOnClickListener(new View.OnClickListener() {
            public void onClick(View v) {
                if (ResultActivity.CURRENT_PAGE > 1) {
                    ResultActivity.CURRENT_PAGE--;
                    ResultActivity.firstAuto = true;
                    ResultActivity.this.displayResult();
                }
            }
        });
        this.btnNextPage.setOnClickListener(new View.OnClickListener() {
            public void onClick(View v) {
                if (ResultActivity.CURRENT_PAGE < ResultActivity.this.TOTAL_PAGES) {
                    ResultActivity.CURRENT_PAGE++;
                    ResultActivity.firstAuto = true;
                    ResultActivity.this.displayResult();
                }
            }
        });
        this.cbbItem.setOnItemSelectedListener(new AdapterView.OnItemSelectedListener() {
            public void onItemSelected(AdapterView<?> parent, View view, int position, long id) {
                ResultActivity.ITEMS_PER_PAGE = Integer.parseInt(parent.getSelectedItem().toString());
                new Common(ResultActivity.this.mContext).setData(Constant.NUMPERPAGE_RESULT, Integer.valueOf(ResultActivity.ITEMS_PER_PAGE), 3);
                ResultActivity.firstAuto = true;
                ResultActivity.this.displayResult();
            }

            public void onNothingSelected(AdapterView<?> adapterView) {
            }
        });
        this.btnToolRegister.setOnClickListener(new View.OnClickListener() {
            public void onClick(View v) {
                ProcessUI.openDialogMachine(ResultActivity.this.mContext);
            }
        });
        this.btnSort.setOnClickListener(new View.OnClickListener() {
            public void onClick(View v) {
                if (ResultActivity.this.btnSort.getText().equals("D")) {
                    ResultActivity.this.btnSort.setBackgroundResource(R.drawable.button_sortup);
                    ResultActivity.this.btnSort.setText("U");
                    boolean unused = ResultActivity.this.mSort = true;
                } else {
                    ResultActivity.this.btnSort.setBackgroundResource(R.drawable.button_sortdown);
                    ResultActivity.this.btnSort.setText("D");
                    boolean unused2 = ResultActivity.this.mSort = false;
                }
                ResultActivity.firstAuto = true;
                ResultActivity.this.readResult();
            }
        });
        this.btnBack.setOnClickListener(new View.OnClickListener() {
            public void onClick(View v) {
                ResultActivity.this.onBackPressed();
            }
        });
        this.btnRequest.setOnClickListener(new View.OnClickListener() {
            public void onClick(View v) {
                ProcessUI.openDialogRequestDetail(ResultActivity.this.mContext, ResultActivity.this.mRequest);
            }
        });
        this.btnProductImage.setOnClickListener(new View.OnClickListener() {
            public void onClick(View v) {
                ResultActivity.this.panelImage.setVisibility(0);
            }
        });
        this.cbbSample.setOnItemSelectedListener(new AdapterView.OnItemSelectedListener() {
            public void onItemSelected(AdapterView<?> adapterView, View view, int i, long l) {
                if (i == 0) {
                    ResultActivity.this.btnPreSample.setEnabled(false);
                } else {
                    ResultActivity.this.btnPreSample.setEnabled(true);
                }
                if (i == adapterView.getCount() - 1) {
                    ResultActivity.this.btnNextSample.setEnabled(false);
                } else {
                    ResultActivity.this.btnNextSample.setEnabled(true);
                }
                ResultActivity.firstAuto = true;
                ResultActivity.this.readResult();
            }

            public void onNothingSelected(AdapterView<?> adapterView) {
            }
        });
        this.btnPreSample.setOnClickListener(new View.OnClickListener() {
            public void onClick(View view) {
                int index = ResultActivity.this.cbbSample.getSelectedItemPosition();
                if (index > 0) {
                    index--;
                }
                ResultActivity.this.cbbSample.setSelection(index);
            }
        });
        this.btnNextSample.setOnClickListener(new View.OnClickListener() {
            public void onClick(View view) {
                int index = ResultActivity.this.cbbSample.getSelectedItemPosition();
                if (index < ResultActivity.this.cbbSample.getAdapter().getCount() - 1) {
                    index++;
                }
                ResultActivity.this.cbbSample.setSelection(index);
            }
        });
        this.btnKeyboard.setOnClickListener(new View.OnClickListener() {
            public void onClick(View view) {
                if (ResultActivity.numAuto != 0) {
                    ResultActivity.this.drawerLayoutResult.openDrawer((int) GravityCompat.END);
                    ResultActivity.this.layoutHistory.setVisibility(8);
                    ResultActivity.this.layoutKeyPad.setVisibility(0);
                    try {
                        Double.parseDouble(((ResultViewModel) ResultActivity.this.listResult.get(ResultActivity.numAuto - 1)).getValue());
                        ResultActivity.this.linearlayoutKeyNumber.setVisibility(0);
                        ResultActivity.this.linearlayoutKeyOkNg.setVisibility(8);
                    } catch (Exception e) {
                        e.printStackTrace();
                        ResultActivity.this.linearlayoutKeyNumber.setVisibility(8);
                        ResultActivity.this.linearlayoutKeyOkNg.setVisibility(0);
                    }
                }
            }
        });
        this.btnQRScan.setOnClickListener(new View.OnClickListener() {
            public void onClick(View view) {
                if (ResultActivity.numAuto != 0) {
                    IntentIntegrator integrator = new IntentIntegrator((Activity) ResultActivity.this.mContext);
                    integrator.setPrompt("Scan the QR Code in the frame");
                    integrator.setCameraId(0);
                    integrator.initiateScan();
                }
            }
        });
        this.btnComment.setOnClickListener(new View.OnClickListener() {
            public void onClick(View view) {
                RequestActivity.progressDialog.showLoadingDialog();
                Intent intent = new Intent(view.getContext(), CommentActivity.class);
                intent.putExtra("REQUEST", ResultActivity.this.mRequest);
                ((Activity) view.getContext()).startActivityForResult(intent, 2);
                MainActivity.CURRENT_ACTIVITY = 4;
            }
        });
        this.btnResult.setOnClickListener(new View.OnClickListener() {
            public void onClick(View view) {
                RequestActivity.progressDialog.showLoadingDialog();
                Intent intent = new Intent(view.getContext(), ResultViewActivity.class);
                intent.putExtra("REQUEST", ResultActivity.this.mRequest);
                ((Activity) view.getContext()).startActivityForResult(intent, 2);
                MainActivity.CURRENT_ACTIVITY = 4;
            }
        });
        this.btnSave.setOnClickListener(new View.OnClickListener() {
            public void onClick(View view) {
                RequestDetailViewModel body = new RequestDetailViewModel();
                body.setRequestId(ResultActivity.this.mRequest.getId());
                body.setSample(Integer.parseInt(ResultActivity.this.cbbSample.getSelectedItem().toString()));
                body.setId(ResultActivity.this.mIdDetail);
                body.setActivated(true);
                String cavity = ResultActivity.this.txtCavity.getText().toString().trim();
                String pindate = ResultActivity.this.txtPinDate.getText().toString().trim();
                String shotmold = ResultActivity.this.txtShotMold.getText().toString().trim();
                String produceno = ResultActivity.this.txtProduceNo.getText().toString().trim();
                if (!cavity.isEmpty()) {
                    body.setCavity(cavity);
                }
                if (!pindate.isEmpty()) {
                    body.setPinDate(pindate);
                }
                if (!shotmold.isEmpty()) {
                    body.setShotMold(shotmold);
                }
                if (!produceno.isEmpty()) {
                    body.setProduceNo(produceno);
                }
                try {
                    if (((RequestDetailViewModel) new apiSetRequestDetailAsync(ResultActivity.this.mContext, body).execute(new String[]{MainActivity.BASE_URL}).get()) != null) {
                        Toast.makeText(ResultActivity.this.mContext, "Request detail save successful", 0).show();
                    }
                } catch (ExecutionException e) {
                    e.printStackTrace();
                } catch (InterruptedException e2) {
                    e2.printStackTrace();
                }
            }
        });
        this.btnZoom.setOnClickListener(new View.OnClickListener() {
            public void onClick(View v) {
                String uri = MainActivity.BASE_URL + "/ProductImage/" + ResultActivity.this.mRequest.getProductImageUrl();
                if (ResultActivity.this.mRequest.getProductImageUrl() != null) {
                    Intent intent = new Intent(ResultActivity.this.mContext, ViewImageActivity.class);
                    intent.putExtra("IMAGE", uri);
                    ((Activity) ResultActivity.this.mContext).startActivityForResult(intent, 2);
                    MainActivity.CURRENT_ACTIVITY = 4;
                }
            }
        });
        this.btnClose.setOnClickListener(new View.OnClickListener() {
            public void onClick(View v) {
                ResultActivity.this.panelImage.setVisibility(8);
            }
        });
        this.btnSearchDelete.setOnClickListener(new View.OnClickListener() {
            public void onClick(View v) {
                ResultActivity.firstAuto = true;
                ResultActivity.txtSearch.setText("");
                ResultActivity.this.displayResult();
            }
        });
        txtSearch.addTextChangedListener(new TextWatcher() {
            public void afterTextChanged(Editable s) {
                if (s.toString().isEmpty()) {
                    ResultActivity.this.btnSearchDelete.setVisibility(8);
                } else {
                    ResultActivity.this.btnSearchDelete.setVisibility(0);
                }
            }

            public void beforeTextChanged(CharSequence s, int start, int count, int after) {
            }

            public void onTextChanged(CharSequence s, int start, int before, int count) {
            }
        });
    }

    /* access modifiers changed from: private */
    public void saveResult() {
        int currentPos = numAuto - 1;
        if (currentPos == -1) {
            Toast.makeText(this.mContext, "Please select a result row", 0).show();
            return;
        }
        ResultViewModel current = this.resultAdapter.getItem(currentPos);
        ResultViewModel body = new ResultViewModel();
        body.setId(current.getId());
        body.setRequestId(this.mRequest.getId());
        body.setMeasurementId(current.getMeasurementId());
        String str = tempResult;
        if (str == "") {
            str = null;
        }
        body.setResult(str);
        body.setMachineName("Manual Input");
        body.setStaffName(MainActivity.FULL_NAME);
        body.setIsActivated(true);
        body.setSample(current.getSample());
        body.setCavity(current.getCavity());
        try {
            if (current.getUnit() == null) {
                String str2 = tempResult;
                if (!(str2 == Constant.OK || str2 == Constant.NG)) {
                    Toast.makeText(this.mContext, "Result incorrect", 0).show();
                    return;
                }
            } else if (!isDouble(tempResult)) {
                Toast.makeText(this.mContext, "Result incorrect", 0).show();
                return;
            }
            ResultViewModel result = (ResultViewModel) new apiSetResultAsync(this.mContext, body).execute(new String[]{MainActivity.BASE_URL}).get();
            if (result != null) {
                nextnumAuto();
                if (result.getJudge() != null) {
                    if (result.getJudge().contains(Constant.OK)) {
                        BeepHelper(97);
                    } else {
                        BeepHelper(59);
                    }
                }
            }
        } catch (ExecutionException e) {
            e.printStackTrace();
        } catch (InterruptedException e2) {
            e2.printStackTrace();
        }
        ((DrawerLayout) findViewById(R.id.draverLayoutResult)).closeDrawer((int) GravityCompat.END);
    }

    private boolean isDouble(String string) {
        try {
            Double.parseDouble(string);
            return true;
        } catch (NumberFormatException e) {
            return false;
        }
    }

    private void readRequestDetail() {
        if (this.rbSample.isChecked()) {
            this.panelDetail.setVisibility(8);
            try {
                List<RequestDetailViewModel> results = (List) new apiGetRequestDetailAsync(this.mContext, this.mRequest.getId(), Integer.parseInt(this.cbbSample.getSelectedItem().toString())).execute(new String[]{MainActivity.BASE_URL}).get();
                if (results == null || results.size() <= 0) {
                    this.txtCavity.setText("");
                    this.txtPinDate.setText("");
                    this.txtShotMold.setText("");
                    this.txtProduceNo.setText("");
                    this.mIdDetail = null;
                    return;
                }
                this.txtCavity.setText(results.get(0).getCavity());
                this.txtPinDate.setText(results.get(0).getPinDate());
                this.txtShotMold.setText(results.get(0).getShotMold());
                this.txtProduceNo.setText(results.get(0).getProduceNo());
                this.mIdDetail = results.get(0).getId();
            } catch (ExecutionException e) {
                e.printStackTrace();
            } catch (InterruptedException e2) {
                e2.printStackTrace();
            }
        } else {
            this.panelDetail.setVisibility(8);
        }
    }

    /* JADX WARNING: Removed duplicated region for block: B:11:0x0044  */
    /* JADX WARNING: Removed duplicated region for block: B:12:0x004a  */
    /* JADX WARNING: Removed duplicated region for block: B:15:0x0055  */
    /* JADX WARNING: Removed duplicated region for block: B:16:0x005b  */
    /* JADX WARNING: Removed duplicated region for block: B:8:0x001c  */
    /* Code decompiled incorrectly, please refer to instructions dump. */
    private void calc_totalPage() {
        /*
            r4 = this;
            int r0 = r4.TOTAL_ROWS
            r1 = 1
            if (r0 == 0) goto L_0x0010
            int r2 = ITEMS_PER_PAGE
            int r3 = r0 % r2
            if (r3 == 0) goto L_0x000c
            goto L_0x0010
        L_0x000c:
            int r0 = r0 / r2
            r4.TOTAL_PAGES = r0
            goto L_0x0016
        L_0x0010:
            int r2 = ITEMS_PER_PAGE
            int r0 = r0 / r2
            int r0 = r0 + r1
            r4.TOTAL_PAGES = r0
        L_0x0016:
            int r0 = CURRENT_PAGE
            int r2 = r4.TOTAL_PAGES
            if (r0 <= r2) goto L_0x001d
            r0 = r2
        L_0x001d:
            CURRENT_PAGE = r0
            android.widget.TextView r0 = r4.lblTotalPage
            java.lang.StringBuilder r2 = new java.lang.StringBuilder
            r2.<init>()
            int r3 = CURRENT_PAGE
            java.lang.StringBuilder r2 = r2.append(r3)
            java.lang.String r3 = "/"
            java.lang.StringBuilder r2 = r2.append(r3)
            int r3 = r4.TOTAL_PAGES
            java.lang.StringBuilder r2 = r2.append(r3)
            java.lang.String r2 = r2.toString()
            r0.setText(r2)
            int r0 = CURRENT_PAGE
            r2 = 0
            if (r0 != r1) goto L_0x004a
            android.widget.Button r0 = r4.btnPrePage
            r0.setEnabled(r2)
            goto L_0x004f
        L_0x004a:
            android.widget.Button r0 = r4.btnPrePage
            r0.setEnabled(r1)
        L_0x004f:
            int r0 = CURRENT_PAGE
            int r3 = r4.TOTAL_PAGES
            if (r0 != r3) goto L_0x005b
            android.widget.Button r0 = r4.btnNextPage
            r0.setEnabled(r2)
            goto L_0x0060
        L_0x005b:
            android.widget.Button r0 = r4.btnNextPage
            r0.setEnabled(r1)
        L_0x0060:
            return
        */
        throw new UnsupportedOperationException("Method not decompiled: com.exampanle.ky_thuat.manualclient.activity.ResultActivity.calc_totalPage():void");
    }

    /* access modifiers changed from: private */
    public void readResult() {
        try {
            this.listResultTotal.clear();
            if (this.rbMeas.isChecked()) {
                List<ResultViewModel> results = (List) new apiGetResultForMeasAsync(this.mContext, this.mRequest.getId(), ((ResultViewModel) this.cbbSample.getAdapter().getItem(this.cbbSample.getSelectedItemPosition())).getMeasurementId()).execute(new String[]{MainActivity.BASE_URL}).get();
                if (results != null) {
                    this.listResultTotal = (ArrayList) results;
                }
            } else {
                List<ResultViewModel> results2 = (List) new apiGetResultConditionAsync(this.mContext, this.mRequest.getId(), Integer.parseInt(this.cbbSample.getSelectedItem().toString()), this.listMachineType).execute(new String[]{MainActivity.BASE_URL}).get();
                if (results2 != null) {
                    this.listResultTotal = (ArrayList) results2;
                }
            }
            displayResult();
        } catch (ExecutionException e) {
            e.printStackTrace();
        } catch (InterruptedException e2) {
            e2.printStackTrace();
        }
    }

    /* access modifiers changed from: private */
    /* JADX WARNING: Can't fix incorrect switch cases order */
    /* JADX WARNING: Code restructure failed: missing block: B:26:0x008d, code lost:
        if (r10.equals(com.exampanle.ky_thuat.manualclient.ultil.Constant.OK) != false) goto L_0x009b;
     */
    /* JADX WARNING: Removed duplicated region for block: B:60:0x0155  */
    /* JADX WARNING: Removed duplicated region for block: B:76:0x0178 A[SYNTHETIC] */
    /* Code decompiled incorrectly, please refer to instructions dump. */
    public void displayResult() {
        /*
            r14 = this;
            r0 = 0
            r1 = 0
            r2 = 0
            r3 = 0
            java.util.ArrayList r4 = new java.util.ArrayList
            r4.<init>()
            java.util.ArrayList<com.exampanle.ky_thuat.manualclient.apiclient.model.ResultViewModel> r5 = r14.listResultTotal
            java.util.Iterator r5 = r5.iterator()
        L_0x000f:
            boolean r6 = r5.hasNext()
            r7 = 0
            java.lang.String r8 = ""
            r9 = 1
            if (r6 == 0) goto L_0x00d4
            java.lang.Object r6 = r5.next()
            com.exampanle.ky_thuat.manualclient.apiclient.model.ResultViewModel r6 = (com.exampanle.ky_thuat.manualclient.apiclient.model.ResultViewModel) r6
            java.lang.String r10 = r6.getMachineTypeName()
            if (r10 == 0) goto L_0x00d2
            android.widget.EditText r10 = txtSearch
            android.text.Editable r10 = r10.getText()
            java.lang.String r10 = r10.toString()
            boolean r10 = r10.isEmpty()
            if (r10 != 0) goto L_0x0049
            java.lang.String r10 = r6.getMachineTypeName()
            android.widget.EditText r11 = txtSearch
            android.text.Editable r11 = r11.getText()
            java.lang.String r11 = r11.toString()
            boolean r10 = r10.equals(r11)
            if (r10 == 0) goto L_0x00d2
        L_0x0049:
            r4.add(r6)
            java.lang.String r10 = r6.getJudge()
            if (r10 == 0) goto L_0x00b4
            java.lang.String r10 = r6.getJudge()
            r11 = -1
            int r12 = r10.hashCode()
            switch(r12) {
                case 2489: goto L_0x0090;
                case 2524: goto L_0x0087;
                case 77202: goto L_0x007d;
                case 77204: goto L_0x0073;
                case 78287: goto L_0x0069;
                case 78289: goto L_0x005f;
                default: goto L_0x005e;
            }
        L_0x005e:
            goto L_0x009a
        L_0x005f:
            java.lang.String r7 = "OK-"
            boolean r7 = r10.equals(r7)
            if (r7 == 0) goto L_0x005e
            r7 = 2
            goto L_0x009b
        L_0x0069:
            java.lang.String r7 = "OK+"
            boolean r7 = r10.equals(r7)
            if (r7 == 0) goto L_0x005e
            r7 = 1
            goto L_0x009b
        L_0x0073:
            java.lang.String r7 = "NG-"
            boolean r7 = r10.equals(r7)
            if (r7 == 0) goto L_0x005e
            r7 = 5
            goto L_0x009b
        L_0x007d:
            java.lang.String r7 = "NG+"
            boolean r7 = r10.equals(r7)
            if (r7 == 0) goto L_0x005e
            r7 = 4
            goto L_0x009b
        L_0x0087:
            java.lang.String r9 = "OK"
            boolean r9 = r10.equals(r9)
            if (r9 == 0) goto L_0x005e
            goto L_0x009b
        L_0x0090:
            java.lang.String r7 = "NG"
            boolean r7 = r10.equals(r7)
            if (r7 == 0) goto L_0x005e
            r7 = 3
            goto L_0x009b
        L_0x009a:
            r7 = -1
        L_0x009b:
            switch(r7) {
                case 0: goto L_0x00b2;
                case 1: goto L_0x00ad;
                case 2: goto L_0x00a8;
                case 3: goto L_0x00a5;
                case 4: goto L_0x00a2;
                case 5: goto L_0x009f;
                default: goto L_0x009e;
            }
        L_0x009e:
            goto L_0x00b4
        L_0x009f:
            int r1 = r1 + 1
            goto L_0x00b4
        L_0x00a2:
            int r1 = r1 + 1
            goto L_0x00b4
        L_0x00a5:
            int r1 = r1 + 1
            goto L_0x00b4
        L_0x00a8:
            int r0 = r0 + 1
            int r3 = r3 + 1
            goto L_0x00b4
        L_0x00ad:
            int r0 = r0 + 1
            int r3 = r3 + 1
            goto L_0x00b4
        L_0x00b2:
            int r0 = r0 + 1
        L_0x00b4:
            java.lang.String r7 = r6.getHistory()
            if (r7 == 0) goto L_0x00d2
            java.lang.String r7 = r6.getHistory()
            boolean r7 = r7.equals(r8)
            if (r7 != 0) goto L_0x00d2
            java.lang.String r7 = r6.getHistory()
            java.lang.String r8 = "[]"
            boolean r7 = r7.equals(r8)
            if (r7 != 0) goto L_0x00d2
            int r2 = r2 + 1
        L_0x00d2:
            goto L_0x000f
        L_0x00d4:
            int r5 = r4.size()
            r14.TOTAL_ROWS = r5
            android.widget.TextView r6 = r14.lblTotal
            java.lang.String r5 = java.lang.String.valueOf(r5)
            r6.setText(r5)
            int r5 = r14.TOTAL_ROWS
            int r5 = r5 - r0
            int r5 = r5 - r1
            android.widget.TextView r6 = r14.lblOK
            java.lang.String r10 = java.lang.String.valueOf(r0)
            r6.setText(r10)
            android.widget.TextView r6 = r14.lblNG
            java.lang.String r10 = java.lang.String.valueOf(r1)
            r6.setText(r10)
            android.widget.TextView r6 = r14.lblEmpty
            java.lang.String r10 = java.lang.String.valueOf(r5)
            r6.setText(r10)
            android.widget.TextView r6 = r14.lblWarning
            java.lang.String r10 = java.lang.String.valueOf(r3)
            r6.setText(r10)
            android.widget.TextView r6 = r14.lblEdit
            java.lang.String r10 = java.lang.String.valueOf(r2)
            r6.setText(r10)
            r14.calc_totalPage()
            int r6 = CURRENT_PAGE
            int r10 = r6 + -1
            int r11 = ITEMS_PER_PAGE
            int r10 = r10 * r11
            int r12 = r14.TOTAL_ROWS
            if (r10 >= r12) goto L_0x0128
            int r10 = r6 + -1
            int r10 = r10 * r11
            goto L_0x012a
        L_0x0128:
            int r10 = r12 + -1
        L_0x012a:
            int r13 = r6 * r11
            if (r13 >= r12) goto L_0x0130
            int r12 = r6 * r11
        L_0x0130:
            r6 = r12
            java.util.ArrayList<com.exampanle.ky_thuat.manualclient.apiclient.model.ResultViewModel> r11 = r14.listResult
            r11.clear()
            int r11 = r4.size()
            if (r11 <= 0) goto L_0x0145
            java.util.ArrayList<com.exampanle.ky_thuat.manualclient.apiclient.model.ResultViewModel> r11 = r14.listResult
            java.util.List r12 = r4.subList(r10, r6)
            r11.addAll(r12)
        L_0x0145:
            boolean r11 = firstAuto
            if (r11 == 0) goto L_0x0178
            java.util.ArrayList<com.exampanle.ky_thuat.manualclient.apiclient.model.ResultViewModel> r11 = r14.listResult
            java.util.Iterator r11 = r11.iterator()
        L_0x014f:
            boolean r12 = r11.hasNext()
            if (r12 == 0) goto L_0x0178
            java.lang.Object r12 = r11.next()
            com.exampanle.ky_thuat.manualclient.apiclient.model.ResultViewModel r12 = (com.exampanle.ky_thuat.manualclient.apiclient.model.ResultViewModel) r12
            java.lang.String r13 = r12.getResult()
            if (r13 == 0) goto L_0x016d
            java.lang.String r13 = r12.getResult()
            boolean r13 = r13.equals(r8)
            if (r13 == 0) goto L_0x016c
            goto L_0x016d
        L_0x016c:
            goto L_0x014f
        L_0x016d:
            java.util.ArrayList<com.exampanle.ky_thuat.manualclient.apiclient.model.ResultViewModel> r8 = r14.listResult
            int r8 = r8.indexOf(r12)
            int r8 = r8 + r9
            numAuto = r8
            firstAuto = r7
        L_0x0178:
            com.exampanle.ky_thuat.manualclient.adapter.ResultAdapter r7 = r14.resultAdapter
            r7.notifyDataSetChanged()
            int r7 = numAuto
            if (r7 <= 0) goto L_0x0185
            int r7 = r7 - r9
            r14.scrollToItem(r7)
        L_0x0185:
            return
        */
        throw new UnsupportedOperationException("Method not decompiled: com.exampanle.ky_thuat.manualclient.activity.ResultActivity.displayResult():void");
    }

    private void scrollToItem(int pos) {
        LinearLayoutManager linearLayoutManager2 = this.linearLayoutManager;
        if (linearLayoutManager2 != null) {
            linearLayoutManager2.scrollToPosition(pos);
        }
    }

    private void load_ListResultConfig() {
        String list = (String) new Common(this.mContext).getData(Constant.RESULTCONFIG, 1);
        if (list != "") {
            listResultConfig = (ArrayList) new JSON().deserialize(list, new TypeToken<ArrayList<ConfigDto>>() {
            }.getType());
        }
        update_Title();
    }

    private void load_ChartConfig() {
        Common common = new Common(this.mContext);
        mItemChart = ((Integer) common.getData(Constant.LIMITITEMCHART, 3)).intValue();
        boolean booleanValue = ((Boolean) common.getData(Constant.SHOWCHART, 2)).booleanValue();
        mIsShowChart = booleanValue;
        if (!booleanValue) {
            this.panelChart.setVisibility(8);
        }
    }

    /* JADX WARNING: Can't fix incorrect switch cases order */
    /* Code decompiled incorrectly, please refer to instructions dump. */
    private void update_Title() {
        /*
            r20 = this;
            r0 = r20
            java.util.ArrayList<com.exampanle.ky_thuat.manualclient.apiclient.model.ConfigDto> r1 = listResultConfig
            java.util.Iterator r1 = r1.iterator()
        L_0x0008:
            boolean r2 = r1.hasNext()
            if (r2 == 0) goto L_0x0236
            java.lang.Object r2 = r1.next()
            com.exampanle.ky_thuat.manualclient.apiclient.model.ConfigDto r2 = (com.exampanle.ky_thuat.manualclient.apiclient.model.ConfigDto) r2
            boolean r3 = r2.isShow()
            java.lang.String r11 = "Cavity"
            java.lang.String r12 = "Important"
            java.lang.String r13 = "MachineType"
            java.lang.String r14 = "Dimension"
            java.lang.String r15 = "Judge"
            java.lang.String r4 = "Unit"
            java.lang.String r5 = "USL"
            java.lang.String r6 = "LSL"
            java.lang.String r7 = "No"
            java.lang.String r8 = "Nominal"
            java.lang.String r9 = "Tolerance"
            java.lang.String r10 = "Sample"
            r16 = r1
            java.lang.String r1 = "Result"
            r17 = -1
            r0 = 8
            if (r3 != 0) goto L_0x0129
            java.lang.String r3 = r2.getName()
            int r18 = r3.hashCode()
            switch(r18) {
                case -1850559427: goto L_0x00ad;
                case -1825807926: goto L_0x00a5;
                case -1257926675: goto L_0x009d;
                case -507420484: goto L_0x0095;
                case 2529: goto L_0x008d;
                case 75685: goto L_0x0085;
                case 84334: goto L_0x007d;
                case 2641316: goto L_0x0074;
                case 71925495: goto L_0x006b;
                case 908954950: goto L_0x0063;
                case 1449751553: goto L_0x005a;
                case 1795442690: goto L_0x0051;
                case 2011354614: goto L_0x0047;
                default: goto L_0x0045;
            }
        L_0x0045:
            goto L_0x00b6
        L_0x0047:
            boolean r1 = r3.equals(r11)
            if (r1 == 0) goto L_0x0045
            r4 = 10
            goto L_0x00b7
        L_0x0051:
            boolean r1 = r3.equals(r12)
            if (r1 == 0) goto L_0x0045
            r4 = 3
            goto L_0x00b7
        L_0x005a:
            boolean r1 = r3.equals(r13)
            if (r1 == 0) goto L_0x0045
            r4 = 9
            goto L_0x00b7
        L_0x0063:
            boolean r1 = r3.equals(r14)
            if (r1 == 0) goto L_0x0045
            r4 = 1
            goto L_0x00b7
        L_0x006b:
            boolean r1 = r3.equals(r15)
            if (r1 == 0) goto L_0x0045
            r4 = 12
            goto L_0x00b7
        L_0x0074:
            boolean r1 = r3.equals(r4)
            if (r1 == 0) goto L_0x0045
            r4 = 8
            goto L_0x00b7
        L_0x007d:
            boolean r1 = r3.equals(r5)
            if (r1 == 0) goto L_0x0045
            r4 = 7
            goto L_0x00b7
        L_0x0085:
            boolean r1 = r3.equals(r6)
            if (r1 == 0) goto L_0x0045
            r4 = 6
            goto L_0x00b7
        L_0x008d:
            boolean r1 = r3.equals(r7)
            if (r1 == 0) goto L_0x0045
            r4 = 0
            goto L_0x00b7
        L_0x0095:
            boolean r1 = r3.equals(r8)
            if (r1 == 0) goto L_0x0045
            r4 = 4
            goto L_0x00b7
        L_0x009d:
            boolean r1 = r3.equals(r9)
            if (r1 == 0) goto L_0x0045
            r4 = 5
            goto L_0x00b7
        L_0x00a5:
            boolean r1 = r3.equals(r10)
            if (r1 == 0) goto L_0x0045
            r4 = 2
            goto L_0x00b7
        L_0x00ad:
            boolean r1 = r3.equals(r1)
            if (r1 == 0) goto L_0x0045
            r4 = 11
            goto L_0x00b7
        L_0x00b6:
            r4 = -1
        L_0x00b7:
            switch(r4) {
                case 0: goto L_0x011f;
                case 1: goto L_0x0117;
                case 2: goto L_0x010f;
                case 3: goto L_0x0107;
                case 4: goto L_0x00ff;
                case 5: goto L_0x00f7;
                case 6: goto L_0x00ef;
                case 7: goto L_0x00e7;
                case 8: goto L_0x00df;
                case 9: goto L_0x00d7;
                case 10: goto L_0x00cf;
                case 11: goto L_0x00c7;
                case 12: goto L_0x00be;
                default: goto L_0x00ba;
            }
        L_0x00ba:
            r3 = r20
            goto L_0x0127
        L_0x00be:
            r3 = r20
            android.widget.TextView r1 = r3.lblJudge
            r1.setVisibility(r0)
            goto L_0x0127
        L_0x00c7:
            r3 = r20
            android.widget.TextView r1 = r3.lblResult
            r1.setVisibility(r0)
            goto L_0x0127
        L_0x00cf:
            r3 = r20
            android.widget.TextView r1 = r3.lblCavi
            r1.setVisibility(r0)
            goto L_0x0127
        L_0x00d7:
            r3 = r20
            android.widget.TextView r1 = r3.lblMachineType
            r1.setVisibility(r0)
            goto L_0x0127
        L_0x00df:
            r3 = r20
            android.widget.TextView r1 = r3.lblUnit
            r1.setVisibility(r0)
            goto L_0x0127
        L_0x00e7:
            r3 = r20
            android.widget.TextView r1 = r3.lblUSL
            r1.setVisibility(r0)
            goto L_0x0127
        L_0x00ef:
            r3 = r20
            android.widget.TextView r1 = r3.lblLSL
            r1.setVisibility(r0)
            goto L_0x0127
        L_0x00f7:
            r3 = r20
            android.widget.TextView r1 = r3.lblTolerance
            r1.setVisibility(r0)
            goto L_0x0127
        L_0x00ff:
            r3 = r20
            android.widget.TextView r1 = r3.lblNominal
            r1.setVisibility(r0)
            goto L_0x0127
        L_0x0107:
            r3 = r20
            android.widget.TextView r1 = r3.lblImportant
            r1.setVisibility(r0)
            goto L_0x0127
        L_0x010f:
            r3 = r20
            android.widget.TextView r1 = r3.lblSample
            r1.setVisibility(r0)
            goto L_0x0127
        L_0x0117:
            r3 = r20
            android.widget.LinearLayout r1 = r3.panelMeasurement
            r1.setVisibility(r0)
            goto L_0x0127
        L_0x011f:
            r3 = r20
            android.widget.TextView r1 = r3.lblNo
            r1.setVisibility(r0)
        L_0x0127:
            goto L_0x0231
        L_0x0129:
            r3 = r20
            java.lang.String r0 = r2.getName()
            int r19 = r0.hashCode()
            switch(r19) {
                case -1850559427: goto L_0x019e;
                case -1825807926: goto L_0x0196;
                case -1257926675: goto L_0x018e;
                case -507420484: goto L_0x0186;
                case 2529: goto L_0x017e;
                case 75685: goto L_0x0176;
                case 84334: goto L_0x016e;
                case 2641316: goto L_0x0165;
                case 71925495: goto L_0x015c;
                case 908954950: goto L_0x0154;
                case 1449751553: goto L_0x014b;
                case 1795442690: goto L_0x0142;
                case 2011354614: goto L_0x0138;
                default: goto L_0x0136;
            }
        L_0x0136:
            goto L_0x01a7
        L_0x0138:
            boolean r0 = r0.equals(r11)
            if (r0 == 0) goto L_0x0136
            r4 = 10
            goto L_0x01a8
        L_0x0142:
            boolean r0 = r0.equals(r12)
            if (r0 == 0) goto L_0x0136
            r4 = 3
            goto L_0x01a8
        L_0x014b:
            boolean r0 = r0.equals(r13)
            if (r0 == 0) goto L_0x0136
            r4 = 9
            goto L_0x01a8
        L_0x0154:
            boolean r0 = r0.equals(r14)
            if (r0 == 0) goto L_0x0136
            r4 = 1
            goto L_0x01a8
        L_0x015c:
            boolean r0 = r0.equals(r15)
            if (r0 == 0) goto L_0x0136
            r4 = 12
            goto L_0x01a8
        L_0x0165:
            boolean r0 = r0.equals(r4)
            if (r0 == 0) goto L_0x0136
            r4 = 8
            goto L_0x01a8
        L_0x016e:
            boolean r0 = r0.equals(r5)
            if (r0 == 0) goto L_0x0136
            r4 = 7
            goto L_0x01a8
        L_0x0176:
            boolean r0 = r0.equals(r6)
            if (r0 == 0) goto L_0x0136
            r4 = 6
            goto L_0x01a8
        L_0x017e:
            boolean r0 = r0.equals(r7)
            if (r0 == 0) goto L_0x0136
            r4 = 0
            goto L_0x01a8
        L_0x0186:
            boolean r0 = r0.equals(r8)
            if (r0 == 0) goto L_0x0136
            r4 = 4
            goto L_0x01a8
        L_0x018e:
            boolean r0 = r0.equals(r9)
            if (r0 == 0) goto L_0x0136
            r4 = 5
            goto L_0x01a8
        L_0x0196:
            boolean r0 = r0.equals(r10)
            if (r0 == 0) goto L_0x0136
            r4 = 2
            goto L_0x01a8
        L_0x019e:
            boolean r0 = r0.equals(r1)
            if (r0 == 0) goto L_0x0136
            r4 = 11
            goto L_0x01a8
        L_0x01a7:
            r4 = -1
        L_0x01a8:
            switch(r4) {
                case 0: goto L_0x0227;
                case 1: goto L_0x021d;
                case 2: goto L_0x0213;
                case 3: goto L_0x0209;
                case 4: goto L_0x01ff;
                case 5: goto L_0x01f5;
                case 6: goto L_0x01eb;
                case 7: goto L_0x01e1;
                case 8: goto L_0x01d7;
                case 9: goto L_0x01cd;
                case 10: goto L_0x01c3;
                case 11: goto L_0x01b8;
                case 12: goto L_0x01ad;
                default: goto L_0x01ab;
            }
        L_0x01ab:
            goto L_0x0231
        L_0x01ad:
            android.widget.TextView r0 = r3.lblJudge
            java.lang.String r1 = r2.getDisplayName()
            r0.setText(r1)
            goto L_0x0231
        L_0x01b8:
            android.widget.TextView r0 = r3.lblResult
            java.lang.String r1 = r2.getDisplayName()
            r0.setText(r1)
            goto L_0x0231
        L_0x01c3:
            android.widget.TextView r0 = r3.lblCavi
            java.lang.String r1 = r2.getDisplayName()
            r0.setText(r1)
            goto L_0x0231
        L_0x01cd:
            android.widget.TextView r0 = r3.lblMachineType
            java.lang.String r1 = r2.getDisplayName()
            r0.setText(r1)
            goto L_0x0231
        L_0x01d7:
            android.widget.TextView r0 = r3.lblUnit
            java.lang.String r1 = r2.getDisplayName()
            r0.setText(r1)
            goto L_0x0231
        L_0x01e1:
            android.widget.TextView r0 = r3.lblUSL
            java.lang.String r1 = r2.getDisplayName()
            r0.setText(r1)
            goto L_0x0231
        L_0x01eb:
            android.widget.TextView r0 = r3.lblLSL
            java.lang.String r1 = r2.getDisplayName()
            r0.setText(r1)
            goto L_0x0231
        L_0x01f5:
            android.widget.TextView r0 = r3.lblTolerance
            java.lang.String r1 = r2.getDisplayName()
            r0.setText(r1)
            goto L_0x0231
        L_0x01ff:
            android.widget.TextView r0 = r3.lblNominal
            java.lang.String r1 = r2.getDisplayName()
            r0.setText(r1)
            goto L_0x0231
        L_0x0209:
            android.widget.TextView r0 = r3.lblImportant
            java.lang.String r1 = r2.getDisplayName()
            r0.setText(r1)
            goto L_0x0231
        L_0x0213:
            android.widget.TextView r0 = r3.lblSample
            java.lang.String r1 = r2.getDisplayName()
            r0.setText(r1)
            goto L_0x0231
        L_0x021d:
            android.widget.TextView r0 = r3.lblMeasurement
            java.lang.String r1 = r2.getDisplayName()
            r0.setText(r1)
            goto L_0x0231
        L_0x0227:
            android.widget.TextView r0 = r3.lblNo
            java.lang.String r1 = r2.getDisplayName()
            r0.setText(r1)
        L_0x0231:
            r0 = r3
            r1 = r16
            goto L_0x0008
        L_0x0236:
            r3 = r0
            return
        */
        throw new UnsupportedOperationException("Method not decompiled: com.exampanle.ky_thuat.manualclient.activity.ResultActivity.update_Title():void");
    }

    private void addControls() {
        this.rcvResult = (RecyclerView) findViewById(R.id.rcvResult);
        this.lblRequestMark = (TextView) findViewById(R.id.lblRequestMark);
        this.lblStaff = (TextView) findViewById(R.id.lblStaff);
        txtSearch = (EditText) findViewById(R.id.txtSearch);
        this.cbbItem = (Spinner) findViewById(R.id.cbbItem);
        this.cbbSample = (Spinner) findViewById(R.id.cbbSample);
        this.lblTotalPage = (TextView) findViewById(R.id.lblTotalPage);
        this.lblTotal = (TextView) findViewById(R.id.lblTotal);
        this.btnPrePage = (Button) findViewById(R.id.btnPrePage);
        this.btnNextPage = (Button) findViewById(R.id.btnNextPage);
        this.btnPreSample = (Button) findViewById(R.id.btnPreSample);
        this.btnNextSample = (Button) findViewById(R.id.btnNextSample);
        this.lblOK = (TextView) findViewById(R.id.lblOK);
        this.lblNG = (TextView) findViewById(R.id.lblNG);
        this.lblEmpty = (TextView) findViewById(R.id.lblEmpty);
        this.lblEdit = (TextView) findViewById(R.id.lblEdit);
        this.lblWarning = (TextView) findViewById(R.id.lblWarning);
        this.btnSearch = (Button) findViewById(R.id.btnSearch);
        this.btnSearchDelete = (Button) findViewById(R.id.btnSearchDelete);
        this.btnToolRegister = (Button) findViewById(R.id.btnToolRegister);
        this.btnSort = (Button) findViewById(R.id.btnSort);
        this.btnBack = (Button) findViewById(R.id.btnBack);
        this.btnComment = (Button) findViewById(R.id.btnComment);
        this.btnResult = (Button) findViewById(R.id.btnResult);
        this.btnRequest = (Button) findViewById(R.id.btnRequest);
        this.btnProductImage = (Button) findViewById(R.id.btnProductImage);
        this.rbMeas = (RadioButton) findViewById(R.id.rbMeas);
        this.rbSample = (RadioButton) findViewById(R.id.rbResultSample);
        this.btnKeyboard = (Button) findViewById(R.id.btnKeyboard);
        this.btnQRScan = (Button) findViewById(R.id.btnQRScan);
        this.drawerLayoutResult = (DrawerLayout) findViewById(R.id.draverLayoutResult);
        this.layoutKeyPad = (LinearLayout) findViewById(R.id.layoutKeyPad);
        this.layoutHistory = (LinearLayout) findViewById(R.id.layoutHistory);
        this.linearlayoutKeyNumber = (LinearLayout) findViewById(R.id.linearlayoutKeyNumber);
        this.linearlayoutKeyOkNg = (LinearLayout) findViewById(R.id.linearlayoutKeyOkNg);
        this.panelChart = (LinearLayout) findViewById(R.id.panelChart);
        this.txtCavity = (EditText) findViewById(R.id.txtCavity);
        this.txtPinDate = (EditText) findViewById(R.id.txtPinDate);
        this.txtShotMold = (EditText) findViewById(R.id.txtShotMold);
        this.txtProduceNo = (EditText) findViewById(R.id.txtProduceNo);
        this.btnSave = (Button) findViewById(R.id.btnSave);
        this.panelDetail = (LinearLayout) findViewById(R.id.panelDetail);
        this.btnZoom = (Button) findViewById(R.id.btnZoom);
        this.panelImage = (CoordinatorLayout) findViewById(R.id.panelImage);
        this.btnClose = (Button) findViewById(R.id.btnClose);
        this.imgProductImage = (PhotoView) findViewById(R.id.imgProductImage);
        this.lblNo = (TextView) findViewById(R.id.lblNo);
        this.panelMeasurement = (LinearLayout) findViewById(R.id.panelMeasurement);
        this.lblMeasurement = (TextView) findViewById(R.id.lblMeasurement);
        this.lblSample = (TextView) findViewById(R.id.lblSample);
        this.lblImportant = (TextView) findViewById(R.id.lblImportant);
        this.lblNominal = (TextView) findViewById(R.id.lblNominal);
        this.lblTolerance = (TextView) findViewById(R.id.lblTolerance);
        this.lblLSL = (TextView) findViewById(R.id.lblLSL);
        this.lblUSL = (TextView) findViewById(R.id.lblUSL);
        this.lblUnit = (TextView) findViewById(R.id.lblUnit);
        this.lblMachineType = (TextView) findViewById(R.id.lblMachineType);
        this.lblCavi = (TextView) findViewById(R.id.lblCavi);
        this.lblResult = (TextView) findViewById(R.id.lblResult);
        this.lblJudge = (TextView) findViewById(R.id.lblJudge);
        this.mContext = this;
        listResultConfig = new ArrayList<>();
        this.listResultTotal = new ArrayList<>();
        this.listResult = new ArrayList<>();
        LinearLayoutManager linearLayoutManager2 = new LinearLayoutManager(this);
        this.linearLayoutManager = linearLayoutManager2;
        this.rcvResult.setLayoutManager(linearLayoutManager2);
        ResultAdapter resultAdapter2 = new ResultAdapter(this.listResult, this.mContext);
        this.resultAdapter = resultAdapter2;
        this.rcvResult.setAdapter(resultAdapter2);
    }

    public void processRFID(String value) {
        try {
            if (!this.isSaveResultFinish) {
                this.isSaveResultFinish = true;
                return;
            }
            this.isSaveResultFinish = false;
            int currentPos = numAuto - 1;
            if (currentPos == -1) {
                Toast.makeText(this.mContext, "Please select a result row", 0).show();
                this.isSaveResultFinish = true;
                return;
            }
            ReceiveResult result = (ReceiveResult) new JSON().deserialize(value, ReceiveResult.class);
            if (result.getTabletId().equals(MainActivity.TABLET_ID)) {
                ResultViewModel current = this.resultAdapter.getItem(currentPos);
                if (!result.getMachineTypeName().equals(current.getMachineTypeName())) {
                    Toast.makeText(this.mContext, "Mac. type: " + result.getMachineTypeName() + " for measurement incorrect", 0).show();
                    this.isSaveResultFinish = true;
                    return;
                } else if (!result.getUnit().equals(current.getUnit())) {
                    Toast.makeText(this.mContext, "Unit: " + result.getUnit() + " for measurement incorrect", 0).show();
                    this.isSaveResultFinish = true;
                    return;
                } else {
                    ResultViewModel body = new ResultViewModel();
                    body.setId(current.getId());
                    body.setRequestId(this.mRequest.getId());
                    body.setMeasurementId(current.getMeasurementId());
                    body.setResult(result.getResult());
                    body.setMachineName(result.getMachineName());
                    body.setStaffName(MainActivity.FULL_NAME);
                    body.setIsActivated(true);
                    body.setSample(current.getSample());
                    body.setCavity(current.getCavity());
                    try {
                        ResultViewModel resultRequest = (ResultViewModel) new apiSetResultAsync(this.mContext, body).execute(new String[]{MainActivity.BASE_URL}).get();
                        if (resultRequest != null) {
                            nextnumAuto();
                            if (resultRequest.getJudge() != null) {
                                if (resultRequest.getJudge().contains(Constant.OK)) {
                                    BeepHelper(97);
                                } else {
                                    BeepHelper(59);
                                }
                            }
                        }
                    } catch (ExecutionException e) {
                        e.printStackTrace();
                    } catch (InterruptedException e2) {
                        e2.printStackTrace();
                    }
                }
            } else {
                Toast.makeText(this.mContext, "Tool: " + result.getMachineName() + " unregister for tablet", 0).show();
            }
            this.isSaveResultFinish = true;
        } catch (Exception ex) {
            Toast.makeText(this.mContext, ex.getMessage(), 0).show();
        } catch (Throwable th) {
            this.isSaveResultFinish = true;
            throw th;
        }
    }

    private void resultRFID(String value) {
    }

    private void nextnumAuto() {
        firstAuto = false;
        readResult();
        int i = numAuto - 1;
        while (true) {
            if (i >= this.resultAdapter.getItemCount()) {
                break;
            }
            ResultViewModel result = this.resultAdapter.getItem(i);
            if (result.getResult() == null || result.getResult().equals("")) {
                numAuto = i + 1;
            } else {
                numAuto = 0;
                i++;
            }
        }
        numAuto = i + 1;
        if (numAuto == 0) {
            int i2 = CURRENT_PAGE;
            if (i2 < this.TOTAL_PAGES) {
                CURRENT_PAGE = i2 + 1;
            } else {
                numAuto = 0;
            }
            firstAuto = true;
            displayResult();
            return;
        }
        this.resultAdapter.notifyDataSetChanged();
        int i3 = numAuto;
        if (i3 > 0) {
            scrollToItem(i3 - 1);
        }
    }

    private void BeepHelper(int tone) {
        new ToneGenerator(3, 100).startTone(tone, 100);
    }
}
