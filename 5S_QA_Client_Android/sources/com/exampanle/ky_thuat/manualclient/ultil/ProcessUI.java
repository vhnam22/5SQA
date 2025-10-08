package com.exampanle.ky_thuat.manualclient.ultil;

import android.app.Activity;
import android.app.DatePickerDialog;
import android.app.Dialog;
import android.content.Context;
import android.content.Intent;
import android.graphics.BitmapFactory;
import android.util.DisplayMetrics;
import android.view.View;
import android.widget.AdapterView;
import android.widget.ArrayAdapter;
import android.widget.Button;
import android.widget.DatePicker;
import android.widget.EditText;
import android.widget.Spinner;
import android.widget.TextView;
import android.widget.Toast;
import androidx.recyclerview.widget.LinearLayoutManager;
import androidx.recyclerview.widget.RecyclerView;
import com.exampanle.ky_thuat.manualclient.R;
import com.exampanle.ky_thuat.manualclient.activity.MainActivity;
import com.exampanle.ky_thuat.manualclient.activity.RequestActivity;
import com.exampanle.ky_thuat.manualclient.activity.ResultActivity;
import com.exampanle.ky_thuat.manualclient.adapter.MachineAdapter;
import com.exampanle.ky_thuat.manualclient.apiclient.JSON;
import com.exampanle.ky_thuat.manualclient.apiclient.api.apiGetImageAsync;
import com.exampanle.ky_thuat.manualclient.apiclient.api.apiGetMachineForToolAsync;
import com.exampanle.ky_thuat.manualclient.apiclient.api.apiGetMetadataValueAsync;
import com.exampanle.ky_thuat.manualclient.apiclient.api.apiGetOrderAsync;
import com.exampanle.ky_thuat.manualclient.apiclient.api.apiGetProductAsync;
import com.exampanle.ky_thuat.manualclient.apiclient.api.apiGetProductGroupAsync;
import com.exampanle.ky_thuat.manualclient.apiclient.api.apiGetRequestAsync;
import com.exampanle.ky_thuat.manualclient.apiclient.api.apiSetRequestAsync;
import com.exampanle.ky_thuat.manualclient.apiclient.model.MachineForToolViewModel;
import com.exampanle.ky_thuat.manualclient.apiclient.model.MetadataValueViewModel;
import com.exampanle.ky_thuat.manualclient.apiclient.model.OrderViewModel;
import com.exampanle.ky_thuat.manualclient.apiclient.model.ProductGroupViewModel;
import com.exampanle.ky_thuat.manualclient.apiclient.model.ProductViewModel;
import com.exampanle.ky_thuat.manualclient.apiclient.model.QueryArgs;
import com.exampanle.ky_thuat.manualclient.apiclient.model.RequestViewModel;
import com.github.chrisbanes.photoview.PhotoView;
import com.google.gson.reflect.TypeToken;
import java.lang.reflect.Type;
import java.text.ParseException;
import java.text.SimpleDateFormat;
import java.util.ArrayList;
import java.util.Arrays;
import java.util.Calendar;
import java.util.Date;
import java.util.List;
import java.util.UUID;
import java.util.concurrent.ExecutionException;
import org.threeten.bp.OffsetDateTime;
import org.threeten.bp.format.DateTimeFormatter;

public class ProcessUI {
    public static void openDialogRequestDetail(Context context, RequestViewModel request) {
        Dialog dialog = new Dialog(context);
        dialog.getWindow().setBackgroundDrawableResource(17170445);
        dialog.requestWindowFeature(1);
        dialog.setContentView(R.layout.request_detail);
        DisplayMetrics metrics = context.getResources().getDisplayMetrics();
        int width = metrics.widthPixels;
        dialog.getWindow().setLayout((width * 6) / 7, -2);
        TextView lblQuantity = (TextView) dialog.findViewById(R.id.lblQuantity);
        TextView lblSample = (TextView) dialog.findViewById(R.id.lblSample);
        TextView lblType = (TextView) dialog.findViewById(R.id.lblType);
        DisplayMetrics displayMetrics = metrics;
        TextView lblStatus = (TextView) dialog.findViewById(R.id.lblStatus);
        int i = width;
        TextView lblCreated = (TextView) dialog.findViewById(R.id.lblCreated);
        TextView lblModified = (TextView) dialog.findViewById(R.id.lblModified);
        TextView lblCreatedBy = (TextView) dialog.findViewById(R.id.lblCreatedBy);
        TextView lblModifiedBy = (TextView) dialog.findViewById(R.id.lblModifiedBy);
        TextView lblIsActivated = (TextView) dialog.findViewById(R.id.lblIsActivated);
        Dialog dialog2 = dialog;
        PhotoView imgProductImage = (PhotoView) dialog.findViewById(R.id.imgProductImage);
        ((TextView) dialog.findViewById(R.id.lblId)).setText(String.valueOf(request.getId()));
        ((TextView) dialog.findViewById(R.id.lblCode)).setText(request.getCode());
        ((TextView) dialog.findViewById(R.id.lblName)).setText(request.getName());
        ((TextView) dialog.findViewById(R.id.lblStageName)).setText(request.getProductStage());
        ((TextView) dialog.findViewById(R.id.lblProductName)).setText(request.getProductName());
        ((TextView) dialog.findViewById(R.id.lblDate)).setText(String.valueOf(request.getDate()));
        ((TextView) dialog.findViewById(R.id.lblLine)).setText(request.getLine());
        ((TextView) dialog.findViewById(R.id.lblIntention)).setText(request.getIntention());
        ((TextView) dialog.findViewById(R.id.lblLot)).setText(request.getLot());
        lblQuantity.setText(String.valueOf(request.getQuantity() == null ? "" : request.getQuantity()));
        lblSample.setText(String.valueOf(request.getSample()));
        lblType.setText(request.getType());
        lblStatus.setText(request.getStatus());
        lblIsActivated.setText(String.valueOf(request.getIsActivated()));
        TextView textView = lblType;
        lblCreated.setText(String.valueOf(request.getCreated()));
        lblCreatedBy.setText(request.getCreatedBy());
        lblModified.setText(String.valueOf(request.getModified()));
        lblModifiedBy.setText(request.getModifiedBy());
        String uri = MainActivity.BASE_URL + "/ProductImage/" + request.getProductImageUrl();
        try {
            apiGetImageAsync apigetimageasync = new apiGetImageAsync();
            TextView textView2 = lblStatus;
            try {
                String[] strArr = new String[1];
                TextView textView3 = lblIsActivated;
                try {
                    strArr[0] = uri;
                    byte[] result = (byte[]) apigetimageasync.execute(strArr).get();
                    if (result != null && result.length > 0) {
                        imgProductImage.setImageBitmap(BitmapFactory.decodeByteArray(result, 0, result.length));
                        imgProductImage.setVisibility(0);
                    }
                } catch (ExecutionException e) {
                    e = e;
                    e.printStackTrace();
                    dialog2.show();
                } catch (InterruptedException e2) {
                    e = e2;
                    e.printStackTrace();
                    dialog2.show();
                }
            } catch (ExecutionException e3) {
                e = e3;
                TextView textView4 = lblIsActivated;
                e.printStackTrace();
                dialog2.show();
            } catch (InterruptedException e4) {
                e = e4;
                TextView textView5 = lblIsActivated;
                e.printStackTrace();
                dialog2.show();
            }
        } catch (ExecutionException e5) {
            e = e5;
            TextView textView6 = lblStatus;
            TextView textView7 = lblIsActivated;
            e.printStackTrace();
            dialog2.show();
        } catch (InterruptedException e6) {
            e = e6;
            TextView textView8 = lblStatus;
            TextView textView9 = lblIsActivated;
            e.printStackTrace();
            dialog2.show();
        }
        dialog2.show();
    }

    public static void openDialogMachine(Context context) {
        String str;
        Context context2 = context;
        Dialog dialog = new Dialog(context2);
        dialog.getWindow().setBackgroundDrawableResource(17170445);
        dialog.requestWindowFeature(1);
        dialog.setContentView(R.layout.tool_row);
        DisplayMetrics metrics = context.getResources().getDisplayMetrics();
        dialog.getWindow().setLayout((metrics.widthPixels * 6) / 7, -2);
        Button btnSortName = (Button) dialog.findViewById(R.id.btnSortName);
        Button btnSortType = (Button) dialog.findViewById(R.id.btnSortType);
        RecyclerView rcvMachine = (RecyclerView) dialog.findViewById(R.id.rcvMachine);
        rcvMachine.setLayoutManager(new LinearLayoutManager(context2));
        ArrayList<MachineForToolViewModel> listMachinetype = new ArrayList<>();
        final MachineAdapter machineAdapter = new MachineAdapter(listMachinetype);
        rcvMachine.setAdapter(machineAdapter);
        Common common = new Common(context2);
        String strMacType = (String) common.getData(Constant.MACHINETYPELIST, "", 1);
        Type type = new TypeToken<ArrayList<String>>() {
        }.getType();
        ArrayList<String> tokens = (ArrayList) new JSON().deserialize(strMacType, type);
        ArrayList arrayList = new ArrayList();
        DisplayMetrics displayMetrics = metrics;
        String predicate = "";
        int i = 0;
        while (true) {
            Type type2 = type;
            if (i >= tokens.size()) {
                break;
            }
            String strMacType2 = strMacType;
            if (i < tokens.size() - 1) {
                str = predicate + "machinetype.name=@" + i + " || ";
            } else {
                str = predicate + "machinetype.name=@" + i;
            }
            predicate = str;
            arrayList.add(tokens.get(i).trim());
            i++;
            type = type2;
            strMacType = strMacType2;
        }
        QueryArgs body = new QueryArgs();
        body.setPredicate(predicate);
        body.setPredicateParameters(arrayList);
        body.setOrder("machinetype.name");
        body.setLimit(Integer.MAX_VALUE);
        body.setPage(1);
        try {
            List<MachineForToolViewModel> result = (List) new apiGetMachineForToolAsync(context2, MainActivity.TABLET_ID, body).execute(new String[]{MainActivity.BASE_URL}).get();
            listMachinetype.clear();
            if (result != null) {
                listMachinetype.addAll(result);
            }
            machineAdapter.notifyDataSetChanged();
        } catch (ExecutionException e) {
            e.printStackTrace();
        } catch (InterruptedException e2) {
            e2.printStackTrace();
        }
        dialog.show();
        ArrayList arrayList2 = arrayList;
        ArrayList<String> arrayList3 = tokens;
        final Button button = btnSortName;
        final QueryArgs queryArgs = body;
        QueryArgs body2 = body;
        final Context context3 = context;
        Common common2 = common;
        final ArrayList<MachineForToolViewModel> arrayList4 = listMachinetype;
        MachineAdapter machineAdapter2 = machineAdapter;
        btnSortName.setOnClickListener(new View.OnClickListener() {
            public void onClick(View v) {
                if (button.getText().equals("D")) {
                    button.setBackgroundResource(R.drawable.button_sortup);
                    button.setText("U");
                    queryArgs.setOrder("name");
                } else {
                    button.setBackgroundResource(R.drawable.button_sortdown);
                    button.setText("D");
                    queryArgs.setOrder("name DESC");
                }
                try {
                    List<MachineForToolViewModel> result = (List) new apiGetMachineForToolAsync(context3, MainActivity.TABLET_ID, queryArgs).execute(new String[]{MainActivity.BASE_URL}).get();
                    arrayList4.clear();
                    if (result != null) {
                        arrayList4.addAll(result);
                    }
                    machineAdapter.notifyDataSetChanged();
                } catch (ExecutionException e) {
                    e.printStackTrace();
                } catch (InterruptedException e2) {
                    e2.printStackTrace();
                }
            }
        });
        final Button button2 = btnSortType;
        final QueryArgs queryArgs2 = body2;
        btnSortType.setOnClickListener(new View.OnClickListener() {
            public void onClick(View v) {
                if (button2.getText().equals("D")) {
                    button2.setBackgroundResource(R.drawable.button_sortup);
                    button2.setText("U");
                    queryArgs2.setOrder("machinetype.name");
                } else {
                    button2.setBackgroundResource(R.drawable.button_sortdown);
                    button2.setText("D");
                    queryArgs2.setOrder("machinetype.name DESC");
                }
                try {
                    List<MachineForToolViewModel> result = (List) new apiGetMachineForToolAsync(context3, MainActivity.TABLET_ID, queryArgs2).execute(new String[]{MainActivity.BASE_URL}).get();
                    arrayList4.clear();
                    if (result != null) {
                        arrayList4.addAll(result);
                    }
                    machineAdapter.notifyDataSetChanged();
                } catch (ExecutionException e) {
                    e.printStackTrace();
                } catch (InterruptedException e2) {
                    e2.printStackTrace();
                }
            }
        });
    }

    public static void openDialogRequestAdd(Context context) {
        final Context context2 = context;
        Dialog dialog = new Dialog(context2);
        dialog.getWindow().setBackgroundDrawableResource(17170445);
        dialog.requestWindowFeature(1);
        dialog.setContentView(R.layout.request_add);
        DisplayMetrics metrics = context.getResources().getDisplayMetrics();
        dialog.getWindow().setLayout((metrics.widthPixels * 6) / 7, -2);
        final EditText txtDate = (EditText) dialog.findViewById(R.id.txtDate);
        final Spinner cbbProduct = (Spinner) dialog.findViewById(R.id.cbbGroup);
        final Spinner cbbStage = (Spinner) dialog.findViewById(R.id.cbbProduct);
        Spinner cbbLine = (Spinner) dialog.findViewById(R.id.cbbLine);
        final EditText txtSample = (EditText) dialog.findViewById(R.id.txtSample);
        Button btnNextDate = (Button) dialog.findViewById(R.id.btnNextDate);
        Button btnPreDate = (Button) dialog.findViewById(R.id.btnPreDate);
        Button btnNowDate = (Button) dialog.findViewById(R.id.btnNowDate);
        Spinner cbbType = (Spinner) dialog.findViewById(R.id.cbbType);
        loadTxtDate(context2, txtDate);
        load_cbbProduct(context2, cbbProduct);
        load_cbbLine(context2, cbbLine);
        load_cbbType(context2, cbbType);
        txtSample.setText("10");
        cbbProduct.setOnItemSelectedListener(new AdapterView.OnItemSelectedListener() {
            public void onItemSelected(AdapterView<?> adapterView, View view, int position, long id) {
                ProcessUI.load_cbbStage(context2, cbbStage, ((ProductGroupViewModel) cbbProduct.getSelectedItem()).getId());
            }

            public void onNothingSelected(AdapterView<?> adapterView) {
            }
        });
        btnNextDate.setOnClickListener(new View.OnClickListener() {
            public void onClick(View view) {
                SimpleDateFormat dateFormat = new SimpleDateFormat("yyyy/MM/dd");
                try {
                    Date date = dateFormat.parse(txtDate.getText().toString());
                    Calendar calendar = Calendar.getInstance();
                    calendar.setTime(date);
                    calendar.add(5, 1);
                    txtDate.setText(dateFormat.format(calendar.getTime()));
                } catch (ParseException e) {
                    e.printStackTrace();
                }
            }
        });
        btnPreDate.setOnClickListener(new View.OnClickListener() {
            public void onClick(View view) {
                SimpleDateFormat dateFormat = new SimpleDateFormat("yyyy/MM/dd");
                try {
                    Date date = dateFormat.parse(txtDate.getText().toString());
                    Calendar calendar = Calendar.getInstance();
                    calendar.setTime(date);
                    calendar.add(5, -1);
                    txtDate.setText(dateFormat.format(calendar.getTime()));
                } catch (ParseException e) {
                    e.printStackTrace();
                }
            }
        });
        btnNowDate.setOnClickListener(new View.OnClickListener() {
            public void onClick(View view) {
                Calendar calendar = Calendar.getInstance();
                txtDate.setText(new SimpleDateFormat("yyyy/MM/dd").format(calendar.getTime()));
            }
        });
        AnonymousClass8 r12 = r0;
        final Spinner spinner = cbbProduct;
        Spinner cbbType2 = cbbType;
        final Context context3 = context;
        Button button = btnNowDate;
        final Spinner spinner2 = cbbStage;
        Button button2 = btnPreDate;
        final EditText editText = (EditText) dialog.findViewById(R.id.txtQuantity);
        Button button3 = btnNextDate;
        final EditText editText2 = (EditText) dialog.findViewById(R.id.txtLot);
        DisplayMetrics displayMetrics = metrics;
        Button btnConfirm = (Button) dialog.findViewById(R.id.btnConfirm);
        final Spinner spinner3 = cbbLine;
        EditText editText3 = txtSample;
        Spinner spinner4 = cbbLine;
        final Spinner cbbLine2 = cbbType2;
        Spinner spinner5 = cbbStage;
        final EditText editText4 = txtDate;
        Spinner spinner6 = cbbProduct;
        final EditText editText5 = (EditText) dialog.findViewById(R.id.txtIntention);
        EditText editText6 = txtDate;
        final Dialog dialog2 = dialog;
        AnonymousClass8 r0 = new View.OnClickListener() {
            public void onClick(View view) {
                if (spinner.getSelectedItem() == null) {
                    Toast.makeText(context3, "Please select product", 0).show();
                } else if (spinner2.getSelectedItem() == null) {
                    Toast.makeText(context3, "Please select stage", 0).show();
                } else if (editText.getText().toString().isEmpty()) {
                    Toast.makeText(context3, "Please enter quantity", 0).show();
                } else if (editText2.getText().toString().isEmpty()) {
                    Toast.makeText(context3, "Please enter lot", 0).show();
                } else if (spinner3.getSelectedItem() == null) {
                    Toast.makeText(context3, "Please select line", 0).show();
                } else if (txtSample.getText().toString().isEmpty()) {
                    Toast.makeText(context3, "Please enter sample", 0).show();
                } else if (cbbLine2.getSelectedItem() == null) {
                    Toast.makeText(context3, "Please select type", 0).show();
                } else {
                    RequestViewModel body = new RequestViewModel();
                    body.setCode(ProcessUI.setCode(context3));
                    body.setName(((ProductGroupViewModel) spinner.getSelectedItem()).getCode() + "_" + ((ProductViewModel) spinner2.getSelectedItem()).getName() + "_" + editText2.getText().toString() + "_" + ((MetadataValueViewModel) spinner3.getSelectedItem()).getName() + "_" + editText4.getText().toString().replace("/", "").substring(2));
                    body.setDate(ProcessUI.getDate(editText4));
                    body.setProductId(((ProductViewModel) spinner2.getSelectedItem()).getId());
                    body.setLot(editText2.getText().toString().trim());
                    body.setQuantity(Integer.valueOf(Integer.parseInt(editText.getText().toString())));
                    body.setLine(((MetadataValueViewModel) spinner3.getSelectedItem()).getName());
                    if (!editText5.getText().toString().trim().isEmpty()) {
                        body.setIntention(editText5.getText().toString().trim());
                    }
                    body.setSample(Integer.parseInt(txtSample.getText().toString()));
                    body.setType(((MetadataValueViewModel) cbbLine2.getSelectedItem()).getName());
                    body.setStatus(Constant.Activated);
                    try {
                        RequestViewModel result = (RequestViewModel) new apiSetRequestAsync(context3, body).execute(new String[]{MainActivity.BASE_URL}).get();
                        if (result != null) {
                            Toast.makeText(context3, "Request create successful", 0).show();
                            ((RequestActivity) context3).readRequest();
                            dialog2.dismiss();
                            RequestActivity.progressDialog.showLoadingDialog();
                            Intent intent = new Intent(context3, ResultActivity.class);
                            intent.putExtra("REQUEST", result);
                            ((Activity) context3).startActivityForResult(intent, 2);
                            MainActivity.CURRENT_ACTIVITY = 3;
                        }
                    } catch (ExecutionException e) {
                        e.printStackTrace();
                    } catch (InterruptedException e2) {
                        e2.printStackTrace();
                    }
                }
            }
        };
        btnConfirm.setOnClickListener(r12);
        dialog.show();
    }

    /* access modifiers changed from: private */
    public static OffsetDateTime getDate(EditText txtDate) {
        return OffsetDateTime.parse(txtDate.getText().toString().replace("/", "-") + "T00:00:00.000+07:00", DateTimeFormatter.ISO_OFFSET_DATE_TIME);
    }

    /* access modifiers changed from: private */
    public static String setCode(Context context) {
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

    private static void loadTxtDate(final Context context, final EditText txtDate) {
        txtDate.setText(new SimpleDateFormat("yyyy/MM/dd").format(Calendar.getInstance().getTime()));
        txtDate.setOnClickListener(new View.OnClickListener() {
            public void onClick(View view) {
                final SimpleDateFormat simpleDateFormat = new SimpleDateFormat("yyyy/MM/dd");
                final Calendar calendar = Calendar.getInstance();
                try {
                    calendar.setTime(simpleDateFormat.parse(txtDate.getText().toString()));
                } catch (ParseException e) {
                    e.printStackTrace();
                }
                int date = calendar.get(5);
                int month = calendar.get(2);
                new DatePickerDialog(context, new DatePickerDialog.OnDateSetListener() {
                    public void onDateSet(DatePicker datePicker, int i, int i1, int i2) {
                        calendar.set(i, i1, i2);
                        txtDate.setText(simpleDateFormat.format(calendar.getTime()));
                    }
                }, calendar.get(1), month, date).show();
            }
        });
    }

    private static void loadTxtProduceDate(final Context context, final EditText txtProduceDate) {
        txtProduceDate.setText(new SimpleDateFormat("yyyy/MM/dd").format(Calendar.getInstance().getTime()));
        txtProduceDate.setOnClickListener(new View.OnClickListener() {
            public void onClick(View view) {
                final SimpleDateFormat simpleDateFormat = new SimpleDateFormat("yyyy/MM/dd");
                final Calendar calendar = Calendar.getInstance();
                try {
                    calendar.setTime(simpleDateFormat.parse(txtProduceDate.getText().toString()));
                } catch (ParseException e) {
                    e.printStackTrace();
                }
                int date = calendar.get(5);
                int month = calendar.get(2);
                new DatePickerDialog(context, new DatePickerDialog.OnDateSetListener() {
                    public void onDateSet(DatePicker datePicker, int i, int i1, int i2) {
                        calendar.set(i, i1, i2);
                        txtProduceDate.setText(simpleDateFormat.format(calendar.getTime()));
                    }
                }, calendar.get(1), month, date).show();
            }
        });
    }

    private static void load_cbbProduct(Context context, Spinner cbb) {
        QueryArgs body = new QueryArgs();
        body.setPredicate("Products.Any() && IsActivated");
        body.setOrder("Created DESC");
        body.setLimit(Integer.MAX_VALUE);
        body.setPage(1);
        try {
            List<ProductGroupViewModel> result = (List) new apiGetProductGroupAsync(context, body).execute(new String[]{MainActivity.BASE_URL}).get();
            if (result != null) {
                ArrayAdapter<ProductGroupViewModel> adapter = new ArrayAdapter<>(context, 17367048, result);
                adapter.setDropDownViewResource(17367055);
                cbb.setAdapter(adapter);
            }
        } catch (ExecutionException e) {
            e.printStackTrace();
        } catch (InterruptedException e2) {
            e2.printStackTrace();
        }
    }

    /* access modifiers changed from: private */
    public static void load_cbbStage(Context context, Spinner cbb, UUID groupId) {
        QueryArgs body = new QueryArgs();
        body.setPredicate("Measurements.Any() && IsActivated && GroupId=@0");
        body.setPredicateParameters(Arrays.asList(new Object[]{groupId}));
        body.setOrder("Created DESC");
        body.setLimit(Integer.MAX_VALUE);
        body.setPage(1);
        try {
            List<ProductViewModel> result = (List) new apiGetProductAsync(context, body).execute(new String[]{MainActivity.BASE_URL}).get();
            if (result != null) {
                ArrayAdapter<ProductViewModel> adapter = new ArrayAdapter<>(context, 17367048, result);
                adapter.setDropDownViewResource(17367055);
                cbb.setAdapter(adapter);
            }
        } catch (ExecutionException e) {
            e.printStackTrace();
        } catch (InterruptedException e2) {
            e2.printStackTrace();
        }
    }

    private static void load_cbbOrder(Context context, Spinner cbb) {
        QueryArgs body = new QueryArgs();
        body.setPredicate("Quantity>OK || OK=null");
        body.setOrder("Created DESC");
        body.setLimit(Integer.MAX_VALUE);
        body.setPage(1);
        try {
            List<OrderViewModel> result = (List) new apiGetOrderAsync(context, body).execute(new String[]{MainActivity.BASE_URL}).get();
            if (result != null) {
                ArrayAdapter<OrderViewModel> adapter = new ArrayAdapter<>(context, 17367048, result);
                adapter.setDropDownViewResource(17367055);
                cbb.setAdapter(adapter);
            }
        } catch (ExecutionException e) {
            e.printStackTrace();
        } catch (InterruptedException e2) {
            e2.printStackTrace();
        }
    }

    private static void load_cbbLine(Context context, Spinner cbb) {
        QueryArgs body = new QueryArgs();
        body.setPredicate("TypeId=@0");
        body.setPredicateParameters(Arrays.asList(new Object[]{Constant.LineId}));
        body.setOrder("Created");
        body.setLimit(Integer.MAX_VALUE);
        body.setPage(1);
        try {
            List<MetadataValueViewModel> result = (List) new apiGetMetadataValueAsync(context, body).execute(new String[]{MainActivity.BASE_URL}).get();
            if (result != null) {
                ArrayAdapter<MetadataValueViewModel> adapter = new ArrayAdapter<>(context, 17367048, result);
                adapter.setDropDownViewResource(17367055);
                cbb.setAdapter(adapter);
            }
        } catch (ExecutionException e) {
            e.printStackTrace();
        } catch (InterruptedException e2) {
            e2.printStackTrace();
        }
    }

    private static void load_cbbType(Context context, Spinner cbb) {
        QueryArgs body = new QueryArgs();
        body.setPredicate("TypeId=@0");
        body.setPredicateParameters(Arrays.asList(new Object[]{Constant.TypeId}));
        body.setOrder("Created");
        body.setLimit(Integer.MAX_VALUE);
        body.setPage(1);
        try {
            List<MetadataValueViewModel> result = (List) new apiGetMetadataValueAsync(context, body).execute(new String[]{MainActivity.BASE_URL}).get();
            if (result != null) {
                ArrayAdapter<MetadataValueViewModel> adapter = new ArrayAdapter<>(context, 17367048, result);
                adapter.setDropDownViewResource(17367055);
                cbb.setAdapter(adapter);
            }
        } catch (ExecutionException e) {
            e.printStackTrace();
        } catch (InterruptedException e2) {
            e2.printStackTrace();
        }
    }

    private static void load_cbbMoldMachine(Context context, Spinner cbb) {
        QueryArgs body = new QueryArgs();
        body.setPredicate("TypeId=@0");
        body.setPredicateParameters(Arrays.asList(new Object[]{Constant.MoldMachineId}));
        body.setOrder("Created");
        body.setLimit(Integer.MAX_VALUE);
        body.setPage(1);
        try {
            List<MetadataValueViewModel> result = (List) new apiGetMetadataValueAsync(context, body).execute(new String[]{MainActivity.BASE_URL}).get();
            if (result != null) {
                ArrayAdapter<MetadataValueViewModel> adapter = new ArrayAdapter<>(context, 17367048, result);
                adapter.setDropDownViewResource(17367055);
                cbb.setAdapter(adapter);
            }
        } catch (ExecutionException e) {
            e.printStackTrace();
        } catch (InterruptedException e2) {
            e2.printStackTrace();
        }
    }
}
