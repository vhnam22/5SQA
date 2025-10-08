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
import android.widget.Spinner;
import android.widget.TextView;
import android.widget.Toast;
import androidx.appcompat.app.AppCompatActivity;
import androidx.core.view.GravityCompat;
import androidx.drawerlayout.widget.DrawerLayout;
import androidx.recyclerview.widget.LinearLayoutManager;
import androidx.recyclerview.widget.RecyclerView;
import com.exampanle.ky_thuat.manualclient.R;
import com.exampanle.ky_thuat.manualclient.adapter.CommentAdapter;
import com.exampanle.ky_thuat.manualclient.apiclient.api.apiDeleteCommentAsync;
import com.exampanle.ky_thuat.manualclient.apiclient.api.apiGetCommentAsync;
import com.exampanle.ky_thuat.manualclient.apiclient.api.apiSaveCommentAsync;
import com.exampanle.ky_thuat.manualclient.apiclient.model.CommentViewModel;
import com.exampanle.ky_thuat.manualclient.apiclient.model.QueryArgs;
import com.exampanle.ky_thuat.manualclient.apiclient.model.RequestViewModel;
import com.exampanle.ky_thuat.manualclient.ultil.Common;
import com.exampanle.ky_thuat.manualclient.ultil.Constant;
import com.exampanle.ky_thuat.manualclient.ultil.ProcessUI;
import java.util.ArrayList;
import java.util.Iterator;
import java.util.List;
import java.util.UUID;
import java.util.concurrent.ExecutionException;

public class CommentActivity extends AppCompatActivity {
    public static int CURRENT_PAGE;
    public static int ITEMS_PER_PAGE;
    public static int numAuto = 0;
    /* access modifiers changed from: private */
    public int TOTAL_PAGES;
    private int TOTAL_ROWS;
    private Button btnBack;
    /* access modifiers changed from: private */
    public Button btnConfirm;
    private Button btnDelete;
    private Button btnEdit;
    private Button btnNew;
    private Button btnNextPage;
    private Button btnPrePage;
    private Button btnRequest;
    private Button btnSearch;
    /* access modifiers changed from: private */
    public Button btnSort;
    private Spinner cbbItem;
    public CommentAdapter commentAdapter;
    /* access modifiers changed from: private */
    public UUID idComment;
    private TextView lblRequestMark;
    private TextView lblStaff;
    /* access modifiers changed from: private */
    public TextView lblTitle;
    private TextView lblTotal;
    private TextView lblTotalPage;
    private LinearLayoutManager linearLayoutManager;
    private ArrayList<CommentViewModel> listComment;
    private ArrayList<CommentViewModel> listCommentTotal;
    /* access modifiers changed from: private */
    public Context mContext = null;
    /* access modifiers changed from: private */
    public RequestViewModel mRequest;
    /* access modifiers changed from: private */
    public boolean mSort;
    /* access modifiers changed from: private */
    public DrawerLayout navigationView;
    private RecyclerView rcvComment;
    /* access modifiers changed from: private */
    public EditText txtContent;
    private EditText txtSearch;

    /* access modifiers changed from: protected */
    public void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView((int) R.layout.activity_comment);
        addControls();
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

    private void addControls() {
        this.rcvComment = (RecyclerView) findViewById(R.id.rcvComment);
        this.lblRequestMark = (TextView) findViewById(R.id.lblRequestMark);
        this.lblStaff = (TextView) findViewById(R.id.lblStaff);
        this.lblTitle = (TextView) findViewById(R.id.lblTitle);
        this.cbbItem = (Spinner) findViewById(R.id.cbbItem);
        this.lblTotalPage = (TextView) findViewById(R.id.lblTotalPage);
        this.txtSearch = (EditText) findViewById(R.id.txtSearch);
        this.lblTotal = (TextView) findViewById(R.id.lblTotal);
        this.btnPrePage = (Button) findViewById(R.id.btnPrePage);
        this.btnNextPage = (Button) findViewById(R.id.btnNextPage);
        this.btnSearch = (Button) findViewById(R.id.btnSearch);
        this.btnSort = (Button) findViewById(R.id.btnSort);
        this.btnBack = (Button) findViewById(R.id.btnBack);
        this.btnRequest = (Button) findViewById(R.id.btnRequest);
        this.btnNew = (Button) findViewById(R.id.btnNew);
        this.btnEdit = (Button) findViewById(R.id.btnEdit);
        this.btnDelete = (Button) findViewById(R.id.btnDelete);
        this.navigationView = (DrawerLayout) findViewById(R.id.draverLayoutComment);
        this.txtContent = (EditText) findViewById(R.id.txtContent);
        this.btnConfirm = (Button) findViewById(R.id.btnConfirm);
        this.mContext = this;
        this.listCommentTotal = new ArrayList<>();
        this.listComment = new ArrayList<>();
        LinearLayoutManager linearLayoutManager2 = new LinearLayoutManager(this);
        this.linearLayoutManager = linearLayoutManager2;
        this.rcvComment.setLayoutManager(linearLayoutManager2);
        CommentAdapter commentAdapter2 = new CommentAdapter(this.listComment);
        this.commentAdapter = commentAdapter2;
        this.rcvComment.setAdapter(commentAdapter2);
    }

    private void loadData() {
        this.mSort = false;
        RequestViewModel requestViewModel = (RequestViewModel) getIntent().getParcelableExtra("REQUEST");
        this.mRequest = requestViewModel;
        this.lblRequestMark.setText(requestViewModel.getName());
        this.lblStaff.setText(MainActivity.FULL_NAME + " is logging");
        Common common = new Common(this.mContext);
        CURRENT_PAGE = 1;
        ITEMS_PER_PAGE = ((Integer) common.getData(Constant.NUMPERPAGE_COMMENT, 3)).intValue();
        Spinner spinner = this.cbbItem;
        spinner.setSelection(((ArrayAdapter) spinner.getAdapter()).getPosition(String.valueOf(ITEMS_PER_PAGE)));
        readComment();
    }

    private void processControl() {
        this.btnSearch.setOnClickListener(new View.OnClickListener() {
            public void onClick(View v) {
                CommentActivity.this.displayComment();
            }
        });
        this.btnPrePage.setOnClickListener(new View.OnClickListener() {
            public void onClick(View v) {
                if (CommentActivity.CURRENT_PAGE > 1) {
                    CommentActivity.CURRENT_PAGE--;
                    CommentActivity.this.displayComment();
                }
            }
        });
        this.btnNextPage.setOnClickListener(new View.OnClickListener() {
            public void onClick(View v) {
                if (CommentActivity.CURRENT_PAGE < CommentActivity.this.TOTAL_PAGES) {
                    CommentActivity.CURRENT_PAGE++;
                    CommentActivity.this.displayComment();
                }
            }
        });
        this.cbbItem.setOnItemSelectedListener(new AdapterView.OnItemSelectedListener() {
            public void onItemSelected(AdapterView<?> parent, View view, int position, long id) {
                CommentActivity.ITEMS_PER_PAGE = Integer.parseInt(parent.getSelectedItem().toString());
                new Common(CommentActivity.this.mContext).setData(Constant.NUMPERPAGE_COMMENT, Integer.valueOf(CommentActivity.ITEMS_PER_PAGE), 3);
                CommentActivity.this.displayComment();
            }

            public void onNothingSelected(AdapterView<?> adapterView) {
            }
        });
        this.btnSort.setOnClickListener(new View.OnClickListener() {
            public void onClick(View v) {
                if (CommentActivity.this.btnSort.getText().equals("D")) {
                    CommentActivity.this.btnSort.setBackgroundResource(R.drawable.button_sortup);
                    CommentActivity.this.btnSort.setText("U");
                    boolean unused = CommentActivity.this.mSort = true;
                } else {
                    CommentActivity.this.btnSort.setBackgroundResource(R.drawable.button_sortdown);
                    CommentActivity.this.btnSort.setText("D");
                    boolean unused2 = CommentActivity.this.mSort = false;
                }
                CommentActivity.this.readComment();
            }
        });
        this.btnBack.setOnClickListener(new View.OnClickListener() {
            public void onClick(View v) {
                CommentActivity.this.onBackPressed();
            }
        });
        this.btnNew.setOnClickListener(new View.OnClickListener() {
            public void onClick(View v) {
                CommentActivity.this.navigationView.openDrawer((int) GravityCompat.END);
                CommentActivity.this.lblTitle.setText("ADD");
                CommentActivity.this.btnConfirm.setVisibility(0);
                CommentActivity.this.txtContent.setEnabled(true);
            }
        });
        this.btnEdit.setOnClickListener(new View.OnClickListener() {
            public void onClick(View v) {
                if (CommentActivity.this.commentAdapter.getItemCount() == 0 || CommentActivity.numAuto == 0) {
                    Toast.makeText(CommentActivity.this.mContext, "Has't row comment", 0).show();
                    return;
                }
                CommentActivity.this.navigationView.openDrawer((int) GravityCompat.END);
                CommentActivity.this.lblTitle.setText("EDIT");
                CommentActivity.this.btnConfirm.setVisibility(0);
                CommentActivity.this.txtContent.setEnabled(true);
            }
        });
        this.btnDelete.setOnClickListener(new View.OnClickListener() {
            public void onClick(View v) {
                if (CommentActivity.this.commentAdapter.getItemCount() == 0 || CommentActivity.numAuto == 0) {
                    Toast.makeText(CommentActivity.this.mContext, "Has't row comment", 0).show();
                    return;
                }
                CommentViewModel commentViewModel = CommentActivity.this.commentAdapter.getItem(CommentActivity.numAuto - 1);
                UUID unused = CommentActivity.this.idComment = commentViewModel.getId();
                DialogInterface.OnClickListener dialogClickListener = new DialogInterface.OnClickListener() {
                    public void onClick(DialogInterface dialog, int which) {
                        switch (which) {
                            case -1:
                                try {
                                    if (((Boolean) new apiDeleteCommentAsync(CommentActivity.this.mContext, CommentActivity.this.idComment).execute(new String[]{MainActivity.BASE_URL}).get()).booleanValue()) {
                                        CommentActivity.this.readComment();
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
                new AlertDialog.Builder(CommentActivity.this.mContext).setMessage("Are you sure delete this comment:\r\n    Content: " + commentViewModel.getContent()).setPositiveButton("Yes", dialogClickListener).setNegativeButton("No", dialogClickListener).show();
            }
        });
        this.btnRequest.setOnClickListener(new View.OnClickListener() {
            public void onClick(View v) {
                ProcessUI.openDialogRequestDetail(CommentActivity.this.mContext, CommentActivity.this.mRequest);
            }
        });
        processNavigationView();
    }

    private void processNavigationView() {
        this.btnConfirm.setOnClickListener(new View.OnClickListener() {
            public void onClick(View v) {
                if (CommentActivity.this.txtContent.getText() == null || CommentActivity.this.txtContent.getText().toString().trim().equals("")) {
                    Toast.makeText(CommentActivity.this.mContext, "Please enter content", 0).show();
                    CommentActivity.this.txtContent.requestFocus();
                } else if (CommentActivity.this.lblTitle.getText().equals("ADD")) {
                    CommentViewModel body = new CommentViewModel();
                    body.setContent(CommentActivity.this.txtContent.getText().toString().trim());
                    body.setRequestId(CommentActivity.this.mRequest.getId());
                    try {
                        if (((Boolean) new apiSaveCommentAsync(CommentActivity.this.mContext, body).execute(new String[]{MainActivity.BASE_URL}).get()).booleanValue()) {
                            CommentActivity.this.readComment();
                            CommentActivity.this.navigationView.closeDrawer((int) GravityCompat.END);
                        }
                    } catch (ExecutionException e) {
                        e.printStackTrace();
                    } catch (InterruptedException e2) {
                        e2.printStackTrace();
                    }
                } else if (CommentActivity.this.lblTitle.getText().equals("EDIT")) {
                    DialogInterface.OnClickListener dialogClickListener = new DialogInterface.OnClickListener() {
                        public void onClick(DialogInterface dialog, int which) {
                            switch (which) {
                                case -1:
                                    CommentViewModel body = new CommentViewModel();
                                    body.setContent(CommentActivity.this.txtContent.getText().toString().trim());
                                    body.setRequestId(CommentActivity.this.mRequest.getId());
                                    body.setId(CommentActivity.this.idComment);
                                    try {
                                        if (((Boolean) new apiSaveCommentAsync(CommentActivity.this.mContext, body).execute(new String[]{MainActivity.BASE_URL}).get()).booleanValue()) {
                                            CommentActivity.this.readComment();
                                            CommentActivity.this.navigationView.closeDrawer((int) GravityCompat.END);
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
                    new AlertDialog.Builder(CommentActivity.this.mContext).setMessage("Are you sure edit information this comment").setPositiveButton("Yes", dialogClickListener).setNegativeButton("No", dialogClickListener).show();
                }
            }
        });
        this.navigationView.addDrawerListener(new DrawerLayout.DrawerListener() {
            public void onDrawerSlide(View view, float v) {
            }

            public void onDrawerOpened(View view) {
            }

            public void onDrawerClosed(View view) {
                CommentActivity.this.lblTitle.setText("VIEW");
                CommentActivity.this.btnConfirm.setVisibility(8);
                CommentActivity.this.txtContent.setEnabled(false);
            }

            public void onDrawerStateChanged(int i) {
                if (i == 2 && !CommentActivity.this.navigationView.isDrawerOpen((int) GravityCompat.START)) {
                    if (!(CommentActivity.this.commentAdapter.getItemCount() == 0 || CommentActivity.numAuto == 0)) {
                        CommentViewModel commentViewModel = CommentActivity.this.commentAdapter.getItem(CommentActivity.numAuto - 1);
                        UUID unused = CommentActivity.this.idComment = commentViewModel.getId();
                        CommentActivity.this.txtContent.setText(commentViewModel.getContent());
                    }
                    CommentActivity.this.txtContent.requestFocus();
                }
            }
        });
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
        throw new UnsupportedOperationException("Method not decompiled: com.exampanle.ky_thuat.manualclient.activity.CommentActivity.calc_totalPage():void");
    }

    /* access modifiers changed from: private */
    public void readComment() {
        List<Object> predicateParameters = new ArrayList<>();
        predicateParameters.add(this.mRequest.getId());
        QueryArgs body = new QueryArgs();
        body.setPredicate("RequestId=@0");
        body.setPredicateParameters(predicateParameters);
        if (!this.mSort) {
            body.setOrder("Created");
        } else {
            body.setOrder("Created DESC");
        }
        body.setLimit(Integer.MAX_VALUE);
        body.setPage(1);
        try {
            List<CommentViewModel> results = (List) new apiGetCommentAsync(this.mContext, body).execute(new String[]{MainActivity.BASE_URL}).get();
            this.listCommentTotal.clear();
            if (results != null) {
                this.listCommentTotal = (ArrayList) results;
            }
            displayComment();
        } catch (ExecutionException e) {
            e.printStackTrace();
        } catch (InterruptedException e2) {
            e2.printStackTrace();
        }
    }

    /* access modifiers changed from: private */
    public void displayComment() {
        ArrayList<CommentViewModel> temp = new ArrayList<>();
        Iterator<CommentViewModel> it = this.listCommentTotal.iterator();
        while (it.hasNext()) {
            CommentViewModel result = it.next();
            if (result.getContent() != null && result.getContent().contains(this.txtSearch.getText())) {
                temp.add(result);
            }
        }
        int size = temp.size();
        this.TOTAL_ROWS = size;
        this.lblTotal.setText(String.valueOf(size));
        calc_totalPage();
        int i = CURRENT_PAGE;
        int i2 = ITEMS_PER_PAGE;
        int i3 = (i - 1) * i2;
        int i4 = this.TOTAL_ROWS;
        int skip = i3 < i4 ? (i - 1) * i2 : i4 - 1;
        if (i * i2 < i4) {
            i4 = i * i2;
        }
        int take = i4;
        this.listComment.clear();
        if (temp.size() > 0) {
            this.listComment.addAll(temp.subList(skip, take));
        }
        if (numAuto > this.listComment.size()) {
            numAuto = this.listComment.size();
        }
        this.commentAdapter.notifyDataSetChanged();
        int i5 = numAuto;
        if (i5 > 0) {
            scrollToItem(i5 - 1);
        }
    }

    private void scrollToItem(int pos) {
        LinearLayoutManager linearLayoutManager2 = this.linearLayoutManager;
        if (linearLayoutManager2 != null) {
            linearLayoutManager2.scrollToPosition(pos);
        }
    }
}
