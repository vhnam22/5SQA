package com.exampanle.ky_thuat.manualclient.activity;

import android.app.AlertDialog;
import android.content.Context;
import android.content.DialogInterface;
import android.os.Bundle;
import android.view.View;
import android.widget.AdapterView;
import android.widget.ArrayAdapter;
import android.widget.Button;
import android.widget.EditText;
import android.widget.LinearLayout;
import android.widget.Spinner;
import android.widget.TextView;
import android.widget.Toast;
import androidx.appcompat.app.AppCompatActivity;
import androidx.recyclerview.widget.LinearLayoutManager;
import androidx.recyclerview.widget.RecyclerView;
import com.exampanle.ky_thuat.manualclient.R;
import com.exampanle.ky_thuat.manualclient.adapter.ResultViewAdapter;
import com.exampanle.ky_thuat.manualclient.apiclient.JSON;
import com.exampanle.ky_thuat.manualclient.apiclient.api.apiGetRequestDetailAsync;
import com.exampanle.ky_thuat.manualclient.apiclient.api.apiGetResultAsync;
import com.exampanle.ky_thuat.manualclient.apiclient.api.apiSetActiveRequestAsync;
import com.exampanle.ky_thuat.manualclient.apiclient.model.ActiveRequestDto;
import com.exampanle.ky_thuat.manualclient.apiclient.model.ConfigDto;
import com.exampanle.ky_thuat.manualclient.apiclient.model.RequestDetailViewModel;
import com.exampanle.ky_thuat.manualclient.apiclient.model.RequestViewModel;
import com.exampanle.ky_thuat.manualclient.apiclient.model.ResultViewModel;
import com.exampanle.ky_thuat.manualclient.ultil.Common;
import com.exampanle.ky_thuat.manualclient.ultil.Constant;
import com.exampanle.ky_thuat.manualclient.ultil.ProcessUI;
import com.google.gson.reflect.TypeToken;
import java.util.ArrayList;
import java.util.Iterator;
import java.util.List;
import java.util.concurrent.ExecutionException;

public class ResultViewActivity extends AppCompatActivity {
    public static ArrayList<ConfigDto> listResultConfig;
    public int CURRENT_PAGE;
    public int ITEMS_PER_PAGE;
    /* access modifiers changed from: private */
    public int TOTAL_PAGES;
    private int TOTAL_ROWS;
    private Button btnBack;
    private Button btnConfirm;
    private Button btnNextPage;
    /* access modifiers changed from: private */
    public Button btnNextSample;
    private Button btnPrePage;
    /* access modifiers changed from: private */
    public Button btnPreSample;
    private Button btnRequest;
    private Button btnSearch;
    /* access modifiers changed from: private */
    public Button btnSort;
    private Spinner cbbItem;
    /* access modifiers changed from: private */
    public Spinner cbbSample;
    private TextView lblCavi;
    private TextView lblCavity;
    private TextView lblEdit;
    private TextView lblEmpty;
    private TextView lblImportant;
    private TextView lblJudge;
    private TextView lblLSL;
    private TextView lblMachineType;
    private TextView lblMeasurement;
    private TextView lblNG;
    private TextView lblNo;
    private TextView lblNominal;
    private TextView lblOK;
    private TextView lblPinDate;
    private TextView lblProduceNo;
    private TextView lblRequest;
    private TextView lblResult;
    private TextView lblSample;
    private TextView lblShotMold;
    private TextView lblStaff;
    private TextView lblTolerance;
    private TextView lblTotal;
    private TextView lblTotalPage;
    private TextView lblUSL;
    private TextView lblUnit;
    private TextView lblWarning;
    private ArrayList<ResultViewModel> listResult;
    private ArrayList<ResultViewModel> listResultTotal;
    /* access modifiers changed from: private */
    public Context mContext = null;
    /* access modifiers changed from: private */
    public RequestViewModel mRequest;
    /* access modifiers changed from: private */
    public boolean mSort;
    private LinearLayout panelMeasurement;
    private RecyclerView rcvResult;
    private ResultViewAdapter resultAdapter;
    private EditText txtSearch;

    /* access modifiers changed from: protected */
    public void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView((int) R.layout.activity_result_view);
        addControls();
        load_ListResultConfig();
        loadData();
        processControl();
    }

    /* access modifiers changed from: protected */
    public void onStart() {
        super.onStart();
        RequestActivity.progressDialog.dismissProgressDialog();
    }

    /* access modifiers changed from: protected */
    public void onResume() {
        super.onResume();
        ActivityManager.setCurrentActivity(this);
    }

    public void onBackPressed() {
        setResult(-1);
        super.onBackPressed();
    }

    private void load_ListResultConfig() {
        String list = (String) new Common(this.mContext).getData(Constant.RESULTCONFIG, 1);
        if (list != "") {
            listResultConfig = (ArrayList) new JSON().deserialize(list, new TypeToken<ArrayList<ConfigDto>>() {
            }.getType());
        }
        update_Title();
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
        throw new UnsupportedOperationException("Method not decompiled: com.exampanle.ky_thuat.manualclient.activity.ResultViewActivity.update_Title():void");
    }

    private void addControls() {
        this.rcvResult = (RecyclerView) findViewById(R.id.rcvResult);
        this.lblRequest = (TextView) findViewById(R.id.lblRequest);
        this.lblStaff = (TextView) findViewById(R.id.lblStaff);
        this.txtSearch = (EditText) findViewById(R.id.txtSearch);
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
        this.btnSort = (Button) findViewById(R.id.btnSort);
        this.btnBack = (Button) findViewById(R.id.btnBack);
        this.btnRequest = (Button) findViewById(R.id.btnRequest);
        this.btnConfirm = (Button) findViewById(R.id.btnConfirm);
        this.lblCavity = (TextView) findViewById(R.id.lblCavity);
        this.lblPinDate = (TextView) findViewById(R.id.lblPinDate);
        this.lblShotMold = (TextView) findViewById(R.id.lblShotMold);
        this.lblProduceNo = (TextView) findViewById(R.id.lblProduceNo);
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
        this.rcvResult.setLayoutManager(new LinearLayoutManager(this));
        ResultViewAdapter resultViewAdapter = new ResultViewAdapter(this.listResult, this.mContext);
        this.resultAdapter = resultViewAdapter;
        this.rcvResult.setAdapter(resultViewAdapter);
    }

    private void loadData() {
        Common common = new Common(this.mContext);
        this.mSort = false;
        RequestViewModel requestViewModel = (RequestViewModel) getIntent().getParcelableExtra("REQUEST");
        this.mRequest = requestViewModel;
        this.lblRequest.setText(requestViewModel.getName());
        this.lblStaff.setText(MainActivity.FULL_NAME + " is logging");
        this.CURRENT_PAGE = 1;
        this.ITEMS_PER_PAGE = ((Integer) common.getData(Constant.NUMPERPAGE_RESULTVIEW, 3)).intValue();
        Spinner spinner = this.cbbItem;
        spinner.setSelection(((ArrayAdapter) spinner.getAdapter()).getPosition(String.valueOf(this.ITEMS_PER_PAGE)));
        loadcbbSample(this.mRequest.getSample());
        if (!this.mRequest.getStatus().contains(Constant.Activated) && !this.mRequest.getStatus().contains(Constant.Rejected)) {
            this.btnConfirm.setVisibility(8);
        }
    }

    private void loadcbbSample(int sample) {
        ArrayList<String> listSample = new ArrayList<>();
        for (int i = 1; i <= sample; i++) {
            listSample.add(String.valueOf(i));
        }
        ArrayAdapter<String> adapter = new ArrayAdapter<>(this.mContext, 17367048, listSample);
        adapter.setDropDownViewResource(17367055);
        this.cbbSample.setAdapter(adapter);
        this.cbbSample.setSelection(0);
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
            int r2 = r4.ITEMS_PER_PAGE
            int r3 = r0 % r2
            if (r3 == 0) goto L_0x000c
            goto L_0x0010
        L_0x000c:
            int r0 = r0 / r2
            r4.TOTAL_PAGES = r0
            goto L_0x0016
        L_0x0010:
            int r2 = r4.ITEMS_PER_PAGE
            int r0 = r0 / r2
            int r0 = r0 + r1
            r4.TOTAL_PAGES = r0
        L_0x0016:
            int r0 = r4.CURRENT_PAGE
            int r2 = r4.TOTAL_PAGES
            if (r0 <= r2) goto L_0x001d
            r0 = r2
        L_0x001d:
            r4.CURRENT_PAGE = r0
            android.widget.TextView r0 = r4.lblTotalPage
            java.lang.StringBuilder r2 = new java.lang.StringBuilder
            r2.<init>()
            int r3 = r4.CURRENT_PAGE
            java.lang.StringBuilder r2 = r2.append(r3)
            java.lang.String r3 = "/"
            java.lang.StringBuilder r2 = r2.append(r3)
            int r3 = r4.TOTAL_PAGES
            java.lang.StringBuilder r2 = r2.append(r3)
            java.lang.String r2 = r2.toString()
            r0.setText(r2)
            int r0 = r4.CURRENT_PAGE
            r2 = 0
            if (r0 != r1) goto L_0x004a
            android.widget.Button r0 = r4.btnPrePage
            r0.setEnabled(r2)
            goto L_0x004f
        L_0x004a:
            android.widget.Button r0 = r4.btnPrePage
            r0.setEnabled(r1)
        L_0x004f:
            int r0 = r4.CURRENT_PAGE
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
        throw new UnsupportedOperationException("Method not decompiled: com.exampanle.ky_thuat.manualclient.activity.ResultViewActivity.calc_totalPage():void");
    }

    private void readRequestDetail() {
        try {
            List<RequestDetailViewModel> results = (List) new apiGetRequestDetailAsync(this.mContext, this.mRequest.getId(), Integer.parseInt(this.cbbSample.getSelectedItem().toString())).execute(new String[]{MainActivity.BASE_URL}).get();
            if (results == null || results.size() <= 0) {
                this.lblCavity.setText("");
                this.lblPinDate.setText("");
                this.lblShotMold.setText("");
                this.lblProduceNo.setText("");
                return;
            }
            this.lblCavity.setText(results.get(0).getCavity());
            this.lblPinDate.setText(results.get(0).getPinDate());
            this.lblShotMold.setText(results.get(0).getShotMold());
            this.lblProduceNo.setText(results.get(0).getProduceNo());
        } catch (ExecutionException e) {
            e.printStackTrace();
        } catch (InterruptedException e2) {
            e2.printStackTrace();
        }
    }

    /* access modifiers changed from: private */
    public void readResult() {
        try {
            this.listResultTotal.clear();
            List<ResultViewModel> results = (List) new apiGetResultAsync(this.mContext, this.mRequest.getId(), Integer.parseInt(this.cbbSample.getSelectedItem().toString())).execute(new String[]{MainActivity.BASE_URL}).get();
            if (results != null) {
                this.listResultTotal = (ArrayList) results;
            }
            displayResult();
        } catch (ExecutionException e) {
            e.printStackTrace();
        } catch (InterruptedException e2) {
            e2.printStackTrace();
        }
    }

    /* access modifiers changed from: private */
    public void displayResult() {
        int judgeOK = 0;
        int jubgeNG = 0;
        int Edit = 0;
        int Warning = 0;
        ArrayList<ResultViewModel> temp = new ArrayList<>();
        Iterator<ResultViewModel> it = this.listResultTotal.iterator();
        while (it.hasNext()) {
            ResultViewModel result = it.next();
            if (result.getName() != null && result.getName().contains(this.txtSearch.getText())) {
                temp.add(result);
                if (result.getJudge() != null) {
                    String judge = result.getJudge();
                    char c = 65535;
                    switch (judge.hashCode()) {
                        case 2489:
                            if (judge.equals(Constant.NG)) {
                                c = 3;
                                break;
                            }
                            break;
                        case 2524:
                            if (judge.equals(Constant.OK)) {
                                c = 0;
                                break;
                            }
                            break;
                        case 77202:
                            if (judge.equals("NG+")) {
                                c = 4;
                                break;
                            }
                            break;
                        case 77204:
                            if (judge.equals("NG-")) {
                                c = 5;
                                break;
                            }
                            break;
                        case 78287:
                            if (judge.equals("OK+")) {
                                c = 1;
                                break;
                            }
                            break;
                        case 78289:
                            if (judge.equals("OK-")) {
                                c = 2;
                                break;
                            }
                            break;
                    }
                    switch (c) {
                        case 0:
                            judgeOK++;
                            break;
                        case 1:
                            judgeOK++;
                            Warning++;
                            break;
                        case 2:
                            judgeOK++;
                            Warning++;
                            break;
                        case 3:
                            jubgeNG++;
                            break;
                        case 4:
                            jubgeNG++;
                            break;
                        case 5:
                            jubgeNG++;
                            break;
                    }
                }
                if (result.getHistory() != null && !result.getHistory().equals("") && !result.getHistory().equals("[]")) {
                    Edit++;
                }
            }
        }
        int size = temp.size();
        this.TOTAL_ROWS = size;
        this.lblTotal.setText(String.valueOf(size));
        this.lblOK.setText(String.valueOf(judgeOK));
        this.lblNG.setText(String.valueOf(jubgeNG));
        this.lblEmpty.setText(String.valueOf((this.TOTAL_ROWS - judgeOK) - jubgeNG));
        this.lblWarning.setText(String.valueOf(Warning));
        this.lblEdit.setText(String.valueOf(Edit));
        calc_totalPage();
        int i = this.CURRENT_PAGE;
        int i2 = this.ITEMS_PER_PAGE;
        int i3 = (i - 1) * i2;
        int i4 = this.TOTAL_ROWS;
        int skip = i3 < i4 ? (i - 1) * i2 : i4 - 1;
        if (i * i2 < i4) {
            i4 = i * i2;
        }
        int take = i4;
        this.listResult.clear();
        if (temp.size() > 0) {
            this.listResult.addAll(temp.subList(skip, take));
        }
        this.resultAdapter.notifyDataSetChanged();
    }

    private void processControl() {
        this.btnSearch.setOnClickListener(new View.OnClickListener() {
            public void onClick(View v) {
                ResultViewActivity.this.displayResult();
            }
        });
        this.btnPrePage.setOnClickListener(new View.OnClickListener() {
            public void onClick(View v) {
                if (ResultViewActivity.this.CURRENT_PAGE > 1) {
                    ResultViewActivity.this.CURRENT_PAGE--;
                    ResultViewActivity.this.displayResult();
                }
            }
        });
        this.btnNextPage.setOnClickListener(new View.OnClickListener() {
            public void onClick(View v) {
                if (ResultViewActivity.this.CURRENT_PAGE < ResultViewActivity.this.TOTAL_PAGES) {
                    ResultViewActivity.this.CURRENT_PAGE++;
                    ResultViewActivity.this.displayResult();
                }
            }
        });
        this.cbbItem.setOnItemSelectedListener(new AdapterView.OnItemSelectedListener() {
            public void onItemSelected(AdapterView<?> parent, View view, int position, long id) {
                ResultViewActivity.this.ITEMS_PER_PAGE = Integer.parseInt(parent.getSelectedItem().toString());
                new Common(ResultViewActivity.this.mContext).setData(Constant.NUMPERPAGE_RESULTVIEW, Integer.valueOf(ResultViewActivity.this.ITEMS_PER_PAGE), 3);
                ResultViewActivity.this.displayResult();
            }

            public void onNothingSelected(AdapterView<?> adapterView) {
            }
        });
        this.btnRequest.setOnClickListener(new View.OnClickListener() {
            public void onClick(View v) {
                ProcessUI.openDialogRequestDetail(ResultViewActivity.this.mContext, ResultViewActivity.this.mRequest);
            }
        });
        this.btnSort.setOnClickListener(new View.OnClickListener() {
            public void onClick(View v) {
                if (ResultViewActivity.this.btnSort.getText().equals("D")) {
                    ResultViewActivity.this.btnSort.setBackgroundResource(R.drawable.button_sortup);
                    ResultViewActivity.this.btnSort.setText("U");
                    boolean unused = ResultViewActivity.this.mSort = true;
                } else {
                    ResultViewActivity.this.btnSort.setBackgroundResource(R.drawable.button_sortdown);
                    ResultViewActivity.this.btnSort.setText("D");
                    boolean unused2 = ResultViewActivity.this.mSort = false;
                }
                ResultViewActivity.this.readResult();
            }
        });
        this.btnBack.setOnClickListener(new View.OnClickListener() {
            public void onClick(View v) {
                ResultViewActivity.this.onBackPressed();
            }
        });
        this.btnConfirm.setOnClickListener(new View.OnClickListener() {
            public void onClick(View view) {
                DialogInterface.OnClickListener dialogClickListener = new DialogInterface.OnClickListener() {
                    public void onClick(DialogInterface dialog, int which) {
                        switch (which) {
                            case -1:
                                ActiveRequestDto body = new ActiveRequestDto();
                                body.setId(ResultViewActivity.this.mRequest.getId());
                                body.setStatus(Constant.Completed);
                                try {
                                    if (((RequestViewModel) new apiSetActiveRequestAsync(ResultViewActivity.this.mContext, body).execute(new String[]{MainActivity.BASE_URL}).get()) != null) {
                                        Toast.makeText(ResultViewActivity.this.mContext, "Confirm request to complete", 0).show();
                                        MainActivity.CURRENT_ACTIVITY = 3;
                                        ResultViewActivity.this.finish();
                                        return;
                                    }
                                    return;
                                } catch (ExecutionException e) {
                                    e.printStackTrace();
                                    return;
                                } catch (InterruptedException e2) {
                                    e2.printStackTrace();
                                    return;
                                }
                            default:
                                return;
                        }
                    }
                };
                new AlertDialog.Builder(ResultViewActivity.this.mContext).setMessage("Are you sure finish this request?").setPositiveButton("Yes", dialogClickListener).setNegativeButton("No", dialogClickListener).show();
            }
        });
        this.cbbSample.setOnItemSelectedListener(new AdapterView.OnItemSelectedListener() {
            public void onItemSelected(AdapterView<?> adapterView, View view, int i, long l) {
                if (i == 0) {
                    ResultViewActivity.this.btnPreSample.setEnabled(false);
                } else {
                    ResultViewActivity.this.btnPreSample.setEnabled(true);
                }
                if (i == ResultViewActivity.this.mRequest.getSample() - 1) {
                    ResultViewActivity.this.btnNextSample.setEnabled(false);
                } else {
                    ResultViewActivity.this.btnNextSample.setEnabled(true);
                }
                ResultViewActivity.this.readResult();
            }

            public void onNothingSelected(AdapterView<?> adapterView) {
            }
        });
        this.btnPreSample.setOnClickListener(new View.OnClickListener() {
            public void onClick(View view) {
                int index = ResultViewActivity.this.cbbSample.getSelectedItemPosition();
                if (index > 0) {
                    index--;
                }
                ResultViewActivity.this.cbbSample.setSelection(index);
            }
        });
        this.btnNextSample.setOnClickListener(new View.OnClickListener() {
            public void onClick(View view) {
                int index = ResultViewActivity.this.cbbSample.getSelectedItemPosition();
                if (index < ResultViewActivity.this.mRequest.getSample() - 1) {
                    index++;
                }
                ResultViewActivity.this.cbbSample.setSelection(index);
            }
        });
    }
}
