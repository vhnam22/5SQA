package com.exampanle.ky_thuat.manualclient.activity;

import android.app.Activity;
import android.app.DatePickerDialog;
import android.content.Context;
import android.content.Intent;
import android.os.Bundle;
import android.view.View;
import android.widget.AdapterView;
import android.widget.ArrayAdapter;
import android.widget.Button;
import android.widget.DatePicker;
import android.widget.EditText;
import android.widget.Spinner;
import android.widget.TextView;
import android.widget.Toast;
import androidx.appcompat.app.AppCompatActivity;
import androidx.recyclerview.widget.LinearLayoutManager;
import androidx.recyclerview.widget.RecyclerView;
import com.exampanle.ky_thuat.manualclient.R;
import com.exampanle.ky_thuat.manualclient.adapter.RequestAdapter;
import com.exampanle.ky_thuat.manualclient.adapter.progressDialog;
import com.exampanle.ky_thuat.manualclient.apiclient.api.apiGetProductAsync;
import com.exampanle.ky_thuat.manualclient.apiclient.api.apiGetRequestAsync;
import com.exampanle.ky_thuat.manualclient.apiclient.api.apiSetRequestAsync;
import com.exampanle.ky_thuat.manualclient.apiclient.model.ProductViewModel;
import com.exampanle.ky_thuat.manualclient.apiclient.model.QueryArgs;
import com.exampanle.ky_thuat.manualclient.apiclient.model.RequestViewModel;
import com.exampanle.ky_thuat.manualclient.ultil.Common;
import com.exampanle.ky_thuat.manualclient.ultil.Constant;
import com.exampanle.ky_thuat.manualclient.ultil.ProcessUI;
import com.google.zxing.integration.android.IntentIntegrator;
import com.google.zxing.integration.android.IntentResult;
import java.text.ParseException;
import java.text.SimpleDateFormat;
import java.util.ArrayList;
import java.util.Arrays;
import java.util.Calendar;
import java.util.Date;
import java.util.Iterator;
import java.util.List;
import java.util.UUID;
import java.util.concurrent.ExecutionException;
import org.threeten.bp.OffsetDateTime;
import org.threeten.bp.format.DateTimeFormatter;

public class RequestActivity extends AppCompatActivity {
    public static progressDialog progressDialog;
    public int CURRENT_PAGE;
    public int CURRENT_PAGE_VIEW;
    public int ITEMS_PER_PAGE;
    public int ITEMS_PER_PAGE_VIEW;
    /* access modifiers changed from: private */
    public int TOTAL_PAGES;
    /* access modifiers changed from: private */
    public int TOTAL_PAGES_VIEW;
    private int TOTAL_ROWS;
    private int TOTAL_ROWS_VIEW;
    private Button btnAdd;
    private Button btnBack;
    private Button btnNextDate;
    private Button btnNextPage;
    private Button btnNextPageView;
    private Button btnNowDate;
    private Button btnPreDate;
    private Button btnPrePage;
    private Button btnPrePageView;
    private Button btnQRScan;
    private Button btnRequestView;
    private Button btnSearch;
    private Button btnSetting;
    /* access modifiers changed from: private */
    public Button btnSort;
    /* access modifiers changed from: private */
    public Button btnSortView;
    private Spinner cbbItem;
    private Spinner cbbItemView;
    private TextView lblStatus;
    private TextView lblTotal;
    private TextView lblTotalPage;
    private TextView lblTotalPageView;
    private ArrayList<RequestViewModel> listRequest;
    private ArrayList<RequestViewModel> listRequestTotal;
    private ArrayList<RequestViewModel> listRequestTotalView;
    private ArrayList<RequestViewModel> listRequestView;
    /* access modifiers changed from: private */
    public Context mContext;
    /* access modifiers changed from: private */
    public boolean mSort;
    /* access modifiers changed from: private */
    public boolean mSortView;
    private RecyclerView rcvRequest;
    private RecyclerView rcvRequestView;
    private RequestAdapter requestAdapter;
    private RequestAdapter requestAdapterView;
    /* access modifiers changed from: private */
    public EditText txtDate;
    private EditText txtSearch;

    /* access modifiers changed from: protected */
    public void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView((int) R.layout.activity_request);
        addControls();
        loadData();
        processControl();
    }

    /* access modifiers changed from: protected */
    public void onResume() {
        super.onResume();
        ActivityManager.setCurrentActivity(this);
    }

    private void loadData() {
        this.mSort = false;
        this.mSortView = false;
        Calendar calendar = Calendar.getInstance();
        this.txtDate.setText(new SimpleDateFormat("yyyy/MM/dd").format(calendar.getTime()));
        this.lblStatus.setText(MainActivity.FULL_NAME + " is logging.");
        Common common = new Common(this.mContext);
        this.CURRENT_PAGE = 1;
        this.ITEMS_PER_PAGE = ((Integer) common.getData(Constant.NUMPERPAGE_REQUEST, 3)).intValue();
        Spinner spinner = this.cbbItem;
        spinner.setSelection(((ArrayAdapter) spinner.getAdapter()).getPosition(String.valueOf(this.ITEMS_PER_PAGE)));
        this.CURRENT_PAGE_VIEW = 1;
        this.ITEMS_PER_PAGE_VIEW = ((Integer) common.getData(Constant.NUMPERPAGE_REQUEST_VIEW, 3)).intValue();
        Spinner spinner2 = this.cbbItemView;
        spinner2.setSelection(((ArrayAdapter) spinner2.getAdapter()).getPosition(String.valueOf(this.ITEMS_PER_PAGE_VIEW)));
        readRequest();
        readRequestView();
    }

    /* access modifiers changed from: protected */
    public void onStart() {
        super.onStart();
        MainActivity.progressDialog.dismissProgressDialog();
    }

    /* access modifiers changed from: protected */
    public void onActivityResult(int requestCode, int resultCode, Intent data) {
        super.onActivityResult(requestCode, resultCode, data);
        if (requestCode == 2 && MainActivity.CURRENT_ACTIVITY != 4) {
            readRequest();
            readRequestView();
        }
        MainActivity.CURRENT_ACTIVITY = 2;
        IntentResult result = IntentIntegrator.parseActivityResult(requestCode, resultCode, data);
        if (result == null) {
            return;
        }
        if (result.getContents() == null) {
            Toast.makeText(this, "Cancel", 1).show();
            return;
        }
        String[] results = result.getContents().split("@");
        if (results.length < 4) {
            Toast.makeText(this, "QR Code incorrect.", 1).show();
            return;
        }
        String orderNo = results[0].trim();
        if (orderNo.isEmpty()) {
            Toast.makeText(this, "Order no. is empty", 1).show();
            return;
        }
        String productCode = results[1].trim();
        String quantity = results[3].trim();
        String name = productCode + "_" + orderNo + "_" + this.txtDate.getText().toString().replace("/", "").substring(2);
        RequestViewModel request = getRequest(name);
        if (request == null) {
            UUID productId = getProductId(productCode);
            if (productId == null) {
                Toast.makeText(this, "This product hasn't", 1).show();
                return;
            }
            RequestViewModel body = new RequestViewModel();
            body.setCode(setCode(this.mContext));
            body.setName(name);
            body.setDate(getDate(this.txtDate));
            body.setProductId(productId);
            body.setIntention(orderNo);
            if (!quantity.isEmpty()) {
                body.setQuantity(Integer.valueOf(Integer.parseInt(quantity)));
            }
            body.setSample(10);
            body.setStatus(Constant.Activated);
            try {
                RequestViewModel _result = (RequestViewModel) new apiSetRequestAsync(this.mContext, body).execute(new String[]{MainActivity.BASE_URL}).get();
                if (_result != null) {
                    Toast.makeText(this.mContext, "Request create successful", 0).show();
                    readRequest();
                    progressDialog.showLoadingDialog();
                    Intent intent = new Intent(this.mContext, ResultActivity.class);
                    intent.putExtra("REQUEST", _result);
                    ((Activity) this.mContext).startActivityForResult(intent, 2);
                    MainActivity.CURRENT_ACTIVITY = 3;
                }
            } catch (ExecutionException e) {
                e.printStackTrace();
            } catch (InterruptedException e2) {
                e2.printStackTrace();
            }
        } else if (request.getStatus().contains(Constant.Activated) || request.getStatus().contains(Constant.Rejected)) {
            progressDialog.showLoadingDialog();
            Intent intent2 = new Intent(this.mContext, ResultActivity.class);
            intent2.putExtra("REQUEST", request);
            ((Activity) this.mContext).startActivityForResult(intent2, 2);
            MainActivity.CURRENT_ACTIVITY = 3;
        } else {
            Toast.makeText(this, "This request has completed.", 1).show();
        }
    }

    private RequestViewModel getRequest(String name) {
        QueryArgs body = new QueryArgs();
        body.setPredicate("Name=@0");
        body.setPredicateParameters(Arrays.asList(new Object[]{name}));
        body.setOrder("Created");
        body.setLimit(1);
        body.setPage(1);
        try {
            List<RequestViewModel> requests = (List) new apiGetRequestAsync(this.mContext, body).execute(new String[]{MainActivity.BASE_URL}).get();
            if (requests == null || requests.size() <= 0) {
                return null;
            }
            return requests.get(0);
        } catch (ExecutionException e) {
            e.printStackTrace();
            return null;
        } catch (InterruptedException e2) {
            e2.printStackTrace();
            return null;
        }
    }

    private UUID getProductId(String code) {
        QueryArgs body = new QueryArgs();
        body.setPredicate("Measurements.Any() && IsActivated && Code=@0");
        body.setPredicateParameters(Arrays.asList(new Object[]{code}));
        body.setOrder("Created");
        body.setLimit(1);
        body.setPage(1);
        try {
            List<ProductViewModel> products = (List) new apiGetProductAsync(this.mContext, body).execute(new String[]{MainActivity.BASE_URL}).get();
            if (products == null || products.size() <= 0) {
                return null;
            }
            return products.get(0).getId();
        } catch (ExecutionException e) {
            e.printStackTrace();
            return null;
        } catch (InterruptedException e2) {
            e2.printStackTrace();
            return null;
        }
    }

    private OffsetDateTime getDate(EditText txtDate2) {
        return OffsetDateTime.parse(txtDate2.getText().toString().replace("/", "-") + "T00:00:00.000+07:00", DateTimeFormatter.ISO_OFFSET_DATE_TIME);
    }

    private String setCode(Context context) {
        String name;
        QueryArgs body = new QueryArgs();
        body.setOrder("Created DESC");
        body.setLimit(1);
        body.setPage(1);
        try {
            List<RequestViewModel> result = (List) new apiGetRequestAsync(context, body).execute(new String[]{MainActivity.BASE_URL}).get();
            if (result != null) {
                if (result.size() > 0) {
                    String[] listcode = result.get(0).getCode().split("-");
                    if (listcode.length > 1) {
                        name = Constant.RequestSign + String.valueOf(Integer.parseInt(listcode[1]) + 1);
                    } else {
                        name = Constant.RequestSign + "1";
                    }
                    return name;
                }
            }
            return Constant.RequestSign + "1";
        } catch (ExecutionException e) {
            e.printStackTrace();
            return Constant.RequestSign;
        } catch (InterruptedException e2) {
            e2.printStackTrace();
            return Constant.RequestSign;
        }
    }

    private void processControl() {
        this.btnSetting.setOnClickListener(new View.OnClickListener() {
            public void onClick(View v) {
                ProcessUI.openDialogMachine(RequestActivity.this.mContext);
            }
        });
        this.btnSearch.setOnClickListener(new View.OnClickListener() {
            public void onClick(View v) {
                RequestActivity.this.displayRequest();
            }
        });
        this.btnPrePage.setOnClickListener(new View.OnClickListener() {
            public void onClick(View v) {
                if (RequestActivity.this.CURRENT_PAGE > 1) {
                    RequestActivity.this.CURRENT_PAGE--;
                    RequestActivity.this.displayRequest();
                }
            }
        });
        this.btnNextPage.setOnClickListener(new View.OnClickListener() {
            public void onClick(View v) {
                if (RequestActivity.this.CURRENT_PAGE < RequestActivity.this.TOTAL_PAGES) {
                    RequestActivity.this.CURRENT_PAGE++;
                    RequestActivity.this.displayRequest();
                }
            }
        });
        this.cbbItem.setOnItemSelectedListener(new AdapterView.OnItemSelectedListener() {
            public void onItemSelected(AdapterView<?> parent, View view, int position, long id) {
                RequestActivity.this.ITEMS_PER_PAGE = Integer.parseInt(parent.getSelectedItem().toString());
                new Common(RequestActivity.this.mContext).setData(Constant.NUMPERPAGE_REQUEST, Integer.valueOf(RequestActivity.this.ITEMS_PER_PAGE), 3);
                RequestActivity.this.displayRequest();
            }

            public void onNothingSelected(AdapterView<?> adapterView) {
            }
        });
        this.btnSort.setOnClickListener(new View.OnClickListener() {
            public void onClick(View v) {
                if (RequestActivity.this.btnSort.getText().equals("D")) {
                    RequestActivity.this.btnSort.setBackgroundResource(R.drawable.button_sortup);
                    RequestActivity.this.btnSort.setText("U");
                    boolean unused = RequestActivity.this.mSort = false;
                } else {
                    RequestActivity.this.btnSort.setBackgroundResource(R.drawable.button_sortdown);
                    RequestActivity.this.btnSort.setText("D");
                    boolean unused2 = RequestActivity.this.mSort = true;
                }
                RequestActivity.this.readRequest();
            }
        });
        this.btnQRScan.setOnClickListener(new View.OnClickListener() {
            public void onClick(View v) {
                IntentIntegrator integrator = new IntentIntegrator((Activity) RequestActivity.this.mContext);
                integrator.setPrompt("Scan the QR Code in the frame");
                integrator.setCameraId(0);
                integrator.initiateScan();
                MainActivity.CURRENT_ACTIVITY = 0;
            }
        });
        this.btnBack.setOnClickListener(new View.OnClickListener() {
            public void onClick(View view) {
                RequestActivity.this.onBackPressed();
            }
        });
        this.txtDate.setOnClickListener(new View.OnClickListener() {
            public void onClick(View view) {
                RequestActivity.this.getDate();
            }
        });
        this.btnNextDate.setOnClickListener(new View.OnClickListener() {
            public void onClick(View view) {
                SimpleDateFormat dateFormat = new SimpleDateFormat("yyyy/MM/dd");
                try {
                    Date date = dateFormat.parse(RequestActivity.this.txtDate.getText().toString());
                    Calendar calendar = Calendar.getInstance();
                    calendar.setTime(date);
                    calendar.add(5, 1);
                    RequestActivity.this.txtDate.setText(dateFormat.format(calendar.getTime()));
                } catch (ParseException e) {
                    e.printStackTrace();
                }
                RequestActivity.this.readRequest();
            }
        });
        this.btnPreDate.setOnClickListener(new View.OnClickListener() {
            public void onClick(View view) {
                SimpleDateFormat dateFormat = new SimpleDateFormat("yyyy/MM/dd");
                try {
                    Date date = dateFormat.parse(RequestActivity.this.txtDate.getText().toString());
                    Calendar calendar = Calendar.getInstance();
                    calendar.setTime(date);
                    calendar.add(5, -1);
                    RequestActivity.this.txtDate.setText(dateFormat.format(calendar.getTime()));
                } catch (ParseException e) {
                    e.printStackTrace();
                }
                RequestActivity.this.readRequest();
            }
        });
        this.btnNowDate.setOnClickListener(new View.OnClickListener() {
            public void onClick(View view) {
                Calendar calendar = Calendar.getInstance();
                RequestActivity.this.txtDate.setText(new SimpleDateFormat("yyyy/MM/dd").format(calendar.getTime()));
                RequestActivity.this.readRequest();
            }
        });
        this.btnAdd.setOnClickListener(new View.OnClickListener() {
            public void onClick(View view) {
                ProcessUI.openDialogRequestAdd(view.getContext());
            }
        });
        this.btnPrePageView.setOnClickListener(new View.OnClickListener() {
            public void onClick(View v) {
                if (RequestActivity.this.CURRENT_PAGE_VIEW > 1) {
                    RequestActivity.this.CURRENT_PAGE_VIEW--;
                    RequestActivity.this.displayRequestView();
                }
            }
        });
        this.btnNextPageView.setOnClickListener(new View.OnClickListener() {
            public void onClick(View v) {
                if (RequestActivity.this.CURRENT_PAGE_VIEW < RequestActivity.this.TOTAL_PAGES_VIEW) {
                    RequestActivity.this.CURRENT_PAGE_VIEW++;
                    RequestActivity.this.displayRequestView();
                }
            }
        });
        this.cbbItemView.setOnItemSelectedListener(new AdapterView.OnItemSelectedListener() {
            public void onItemSelected(AdapterView<?> parent, View view, int position, long id) {
                RequestActivity.this.ITEMS_PER_PAGE_VIEW = Integer.parseInt(parent.getSelectedItem().toString());
                new Common(RequestActivity.this.mContext).setData(Constant.NUMPERPAGE_REQUEST_VIEW, Integer.valueOf(RequestActivity.this.ITEMS_PER_PAGE_VIEW), 3);
                RequestActivity.this.displayRequestView();
            }

            public void onNothingSelected(AdapterView<?> adapterView) {
            }
        });
        this.btnSortView.setOnClickListener(new View.OnClickListener() {
            public void onClick(View v) {
                if (RequestActivity.this.btnSortView.getText().equals("D")) {
                    RequestActivity.this.btnSortView.setBackgroundResource(R.drawable.button_sortup);
                    RequestActivity.this.btnSortView.setText("U");
                    boolean unused = RequestActivity.this.mSortView = false;
                } else {
                    RequestActivity.this.btnSortView.setBackgroundResource(R.drawable.button_sortdown);
                    RequestActivity.this.btnSortView.setText("D");
                    boolean unused2 = RequestActivity.this.mSortView = true;
                }
                RequestActivity.this.readRequestView();
            }
        });
        this.btnRequestView.setOnClickListener(new View.OnClickListener() {
            public void onClick(View v) {
                RequestActivity.this.readRequestView();
            }
        });
    }

    /* access modifiers changed from: private */
    public void getDate() {
        final SimpleDateFormat simpleDateFormat = new SimpleDateFormat("yyyy/MM/dd");
        final Calendar calendar = Calendar.getInstance();
        try {
            calendar.setTime(simpleDateFormat.parse(this.txtDate.getText().toString()));
        } catch (ParseException e) {
            e.printStackTrace();
        }
        int date = calendar.get(5);
        int month = calendar.get(2);
        new DatePickerDialog(this.mContext, new DatePickerDialog.OnDateSetListener() {
            public void onDateSet(DatePicker datePicker, int i, int i1, int i2) {
                calendar.set(i, i1, i2);
                RequestActivity.this.txtDate.setText(simpleDateFormat.format(calendar.getTime()));
                RequestActivity.this.readRequest();
            }
        }, calendar.get(1), month, date).show();
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
        throw new UnsupportedOperationException("Method not decompiled: com.exampanle.ky_thuat.manualclient.activity.RequestActivity.calc_totalPage():void");
    }

    public void readRequest() {
        SimpleDateFormat dateFormat = new SimpleDateFormat("yyyy/MM/dd");
        Calendar calendar = Calendar.getInstance();
        try {
            calendar.setTime(dateFormat.parse(this.txtDate.getText().toString()));
        } catch (ParseException e) {
            e.printStackTrace();
        }
        String date = new SimpleDateFormat("yyyy/MM/dd HH:mm:ss").format(calendar.getTime());
        QueryArgs body = new QueryArgs();
        body.setPredicate("Date=@0 && (Status=@1 || Status.contains(@2))");
        body.setPredicateParameters(Arrays.asList(new Object[]{date, Constant.Activated, Constant.Rejected}));
        if (this.mSort) {
            body.setOrder("Created");
        } else {
            body.setOrder("Created DESC");
        }
        body.setLimit(Integer.MAX_VALUE);
        body.setPage(1);
        try {
            List<RequestViewModel> result = (List) new apiGetRequestAsync(this.mContext, body).execute(new String[]{MainActivity.BASE_URL}).get();
            this.listRequestTotal.clear();
            if (result != null) {
                this.listRequestTotal = (ArrayList) result;
            }
            displayRequest();
        } catch (ExecutionException e2) {
            e2.printStackTrace();
        } catch (InterruptedException e3) {
            e3.printStackTrace();
        }
    }

    /* access modifiers changed from: private */
    public void displayRequest() {
        ArrayList<RequestViewModel> temp = new ArrayList<>();
        Iterator<RequestViewModel> it = this.listRequestTotal.iterator();
        while (it.hasNext()) {
            RequestViewModel request = it.next();
            if (request.getName() != null && request.getName().contains(this.txtSearch.getText())) {
                temp.add(request);
            }
        }
        int size = temp.size();
        this.TOTAL_ROWS = size;
        this.lblTotal.setText(String.valueOf(size));
        calc_totalPage();
        int i = this.CURRENT_PAGE;
        int i2 = this.ITEMS_PER_PAGE;
        int i3 = (i - 1) * i2;
        int i4 = this.TOTAL_ROWS;
        Integer skip = Integer.valueOf(i3 < i4 ? (i - 1) * i2 : i4 - 1);
        int i5 = this.CURRENT_PAGE;
        int i6 = this.ITEMS_PER_PAGE;
        int i7 = i5 * i6;
        int i8 = this.TOTAL_ROWS;
        if (i7 < i8) {
            i8 = i5 * i6;
        }
        Integer take = Integer.valueOf(i8);
        this.listRequest.clear();
        if (temp.size() > 0) {
            this.listRequest.addAll(temp.subList(skip.intValue(), take.intValue()));
        }
        this.requestAdapter.notifyDataSetChanged();
    }

    /* JADX WARNING: Removed duplicated region for block: B:11:0x0044  */
    /* JADX WARNING: Removed duplicated region for block: B:12:0x004a  */
    /* JADX WARNING: Removed duplicated region for block: B:15:0x0055  */
    /* JADX WARNING: Removed duplicated region for block: B:16:0x005b  */
    /* JADX WARNING: Removed duplicated region for block: B:8:0x001c  */
    /* Code decompiled incorrectly, please refer to instructions dump. */
    private void calc_totalPageView() {
        /*
            r4 = this;
            int r0 = r4.TOTAL_ROWS_VIEW
            r1 = 1
            if (r0 == 0) goto L_0x0010
            int r2 = r4.ITEMS_PER_PAGE_VIEW
            int r3 = r0 % r2
            if (r3 == 0) goto L_0x000c
            goto L_0x0010
        L_0x000c:
            int r0 = r0 / r2
            r4.TOTAL_PAGES_VIEW = r0
            goto L_0x0016
        L_0x0010:
            int r2 = r4.ITEMS_PER_PAGE_VIEW
            int r0 = r0 / r2
            int r0 = r0 + r1
            r4.TOTAL_PAGES_VIEW = r0
        L_0x0016:
            int r0 = r4.CURRENT_PAGE_VIEW
            int r2 = r4.TOTAL_PAGES_VIEW
            if (r0 <= r2) goto L_0x001d
            r0 = r2
        L_0x001d:
            r4.CURRENT_PAGE_VIEW = r0
            android.widget.TextView r0 = r4.lblTotalPageView
            java.lang.StringBuilder r2 = new java.lang.StringBuilder
            r2.<init>()
            int r3 = r4.CURRENT_PAGE_VIEW
            java.lang.StringBuilder r2 = r2.append(r3)
            java.lang.String r3 = "/"
            java.lang.StringBuilder r2 = r2.append(r3)
            int r3 = r4.TOTAL_PAGES_VIEW
            java.lang.StringBuilder r2 = r2.append(r3)
            java.lang.String r2 = r2.toString()
            r0.setText(r2)
            int r0 = r4.CURRENT_PAGE_VIEW
            r2 = 0
            if (r0 != r1) goto L_0x004a
            android.widget.Button r0 = r4.btnPrePageView
            r0.setEnabled(r2)
            goto L_0x004f
        L_0x004a:
            android.widget.Button r0 = r4.btnPrePageView
            r0.setEnabled(r1)
        L_0x004f:
            int r0 = r4.CURRENT_PAGE_VIEW
            int r3 = r4.TOTAL_PAGES_VIEW
            if (r0 != r3) goto L_0x005b
            android.widget.Button r0 = r4.btnNextPageView
            r0.setEnabled(r2)
            goto L_0x0060
        L_0x005b:
            android.widget.Button r0 = r4.btnNextPageView
            r0.setEnabled(r1)
        L_0x0060:
            return
        */
        throw new UnsupportedOperationException("Method not decompiled: com.exampanle.ky_thuat.manualclient.activity.RequestActivity.calc_totalPageView():void");
    }

    public void readRequestView() {
        String date = new SimpleDateFormat("yyyy/MM/dd").format(Calendar.getInstance().getTime());
        QueryArgs body = new QueryArgs();
        body.setPredicate("Date=@0 && !Status.contains(@1) && !Status.contains(@2)");
        body.setPredicateParameters(Arrays.asList(new Object[]{date, Constant.Activated, Constant.Rejected}));
        if (this.mSortView) {
            body.setOrder("Created");
        } else {
            body.setOrder("Created DESC");
        }
        body.setLimit(Integer.MAX_VALUE);
        body.setPage(1);
        try {
            List<RequestViewModel> result = (List) new apiGetRequestAsync(this.mContext, body).execute(new String[]{MainActivity.BASE_URL}).get();
            this.listRequestTotalView.clear();
            if (result != null) {
                this.listRequestTotalView = (ArrayList) result;
            }
            displayRequestView();
        } catch (ExecutionException e) {
            e.printStackTrace();
        } catch (InterruptedException e2) {
            e2.printStackTrace();
        }
    }

    /* access modifiers changed from: private */
    public void displayRequestView() {
        ArrayList<RequestViewModel> temp = new ArrayList<>();
        Iterator<RequestViewModel> it = this.listRequestTotalView.iterator();
        while (it.hasNext()) {
            RequestViewModel request = it.next();
            if (request.getName() != null && request.getName().contains(this.txtSearch.getText())) {
                temp.add(request);
            }
        }
        this.TOTAL_ROWS_VIEW = temp.size();
        calc_totalPageView();
        int i = this.CURRENT_PAGE_VIEW;
        int i2 = this.ITEMS_PER_PAGE_VIEW;
        int i3 = (i - 1) * i2;
        int i4 = this.TOTAL_ROWS_VIEW;
        Integer skip = Integer.valueOf(i3 < i4 ? (i - 1) * i2 : i4 - 1);
        int i5 = this.CURRENT_PAGE_VIEW;
        int i6 = this.ITEMS_PER_PAGE_VIEW;
        int i7 = i5 * i6;
        int i8 = this.TOTAL_ROWS_VIEW;
        if (i7 < i8) {
            i8 = i5 * i6;
        }
        Integer take = Integer.valueOf(i8);
        this.listRequestView.clear();
        if (temp.size() > 0) {
            this.listRequestView.addAll(temp.subList(skip.intValue(), take.intValue()));
        }
        this.requestAdapterView.notifyDataSetChanged();
    }

    private void addControls() {
        this.btnSetting = (Button) findViewById(R.id.btnSetting);
        this.btnSearch = (Button) findViewById(R.id.btnSearch);
        this.btnPrePage = (Button) findViewById(R.id.btnPrePage);
        this.btnNextPage = (Button) findViewById(R.id.btnNextPage);
        this.btnSort = (Button) findViewById(R.id.btnSort);
        this.btnAdd = (Button) findViewById(R.id.btnAdd);
        this.btnQRScan = (Button) findViewById(R.id.btnQRScan);
        this.btnBack = (Button) findViewById(R.id.btnBack);
        this.btnNowDate = (Button) findViewById(R.id.btnNowDate);
        this.btnPreDate = (Button) findViewById(R.id.btnPreDate);
        this.btnNextDate = (Button) findViewById(R.id.btnNextDate);
        this.txtSearch = (EditText) findViewById(R.id.txtSearch);
        this.txtDate = (EditText) findViewById(R.id.txtDate);
        this.lblStatus = (TextView) findViewById(R.id.lblStatus);
        this.lblTotal = (TextView) findViewById(R.id.lblTotal);
        this.lblTotalPage = (TextView) findViewById(R.id.lblTotalPage);
        this.rcvRequest = (RecyclerView) findViewById(R.id.rcvRequest);
        this.cbbItem = (Spinner) findViewById(R.id.cbbItem);
        this.btnRequestView = (Button) findViewById(R.id.btnRequestView);
        this.btnPrePageView = (Button) findViewById(R.id.btnPrePageView);
        this.btnNextPageView = (Button) findViewById(R.id.btnNextPageView);
        this.btnSortView = (Button) findViewById(R.id.btnSortView);
        this.lblTotalPageView = (TextView) findViewById(R.id.lblTotalPageView);
        this.rcvRequestView = (RecyclerView) findViewById(R.id.rcvRequestView);
        this.cbbItemView = (Spinner) findViewById(R.id.cbbItemView);
        this.mContext = this;
        progressDialog = new progressDialog(this);
        this.listRequestTotal = new ArrayList<>();
        this.listRequest = new ArrayList<>();
        this.requestAdapter = new RequestAdapter(this.listRequest);
        this.rcvRequest.setLayoutManager(new LinearLayoutManager(this));
        this.rcvRequest.setAdapter(this.requestAdapter);
        this.listRequestTotalView = new ArrayList<>();
        this.listRequestView = new ArrayList<>();
        this.requestAdapterView = new RequestAdapter(this.listRequestView);
        this.rcvRequestView.setLayoutManager(new LinearLayoutManager(this));
        this.rcvRequestView.setAdapter(this.requestAdapterView);
    }

    public void processRFID(String value) {
    }

    public void requestRFID(Long idRequest) {
    }

    public void requestClose() {
        ((Activity) this.mContext).onBackPressed();
    }
}
