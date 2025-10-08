package com.exampanle.ky_thuat.manualclient.adapter;

import android.app.Activity;
import android.content.Context;
import android.os.Handler;
import android.os.Message;
import android.util.Log;
import android.widget.Toast;
import com.exampanle.ky_thuat.manualclient.activity.ActivityManager;
import com.exampanle.ky_thuat.manualclient.activity.MainActivity;
import com.exampanle.ky_thuat.manualclient.activity.RequestActivity;
import com.exampanle.ky_thuat.manualclient.activity.ResultActivity;
import okhttp3.OkHttpClient;
import okhttp3.Request;
import okhttp3.Response;
import okhttp3.WebSocket;
import okhttp3.WebSocketListener;
import okio.ByteString;

public class WebSocketClient {
    /* access modifiers changed from: private */
    public static String TAG = "WebSocketClient";
    private final int STATE_CLOSED = 4;
    private final int STATE_CONNECTED = 1;
    private final int STATE_FAILED = 2;
    private final int STATE_RECEIVED = 3;
    /* access modifiers changed from: private */
    public WebSocket connectionSocket;
    /* access modifiers changed from: private */
    public Handler handler = new Handler(new Handler.Callback() {
        public boolean handleMessage(Message msg) {
            switch (msg.what) {
                case 1:
                    Toast.makeText(WebSocketClient.this.mContext, "WebSocket connected: " + msg.obj, 0).show();
                    break;
                case 2:
                    Toast.makeText(WebSocketClient.this.mContext, "WebSocket error: " + msg.obj, 0).show();
                    break;
                case 3:
                    if (!msg.obj.toString().contains("GetId@")) {
                        if (!msg.obj.toString().contains("Unsuccessful")) {
                            if (!msg.obj.toString().contains("Successful")) {
                                Activity currentActivity = ActivityManager.getCurrentActivity();
                                if (!(currentActivity instanceof RequestActivity)) {
                                    if ((currentActivity instanceof ResultActivity) && msg.obj.toString().contains("MachineTypeName")) {
                                        ((ResultActivity) currentActivity).processRFID(msg.obj.toString());
                                        break;
                                    }
                                } else if (msg.obj.toString().contains("IsActivated")) {
                                    RequestActivity activity = (RequestActivity) currentActivity;
                                    activity.readRequest();
                                    activity.readRequestView();
                                    break;
                                }
                            } else {
                                Toast.makeText(WebSocketClient.this.mContext, "Tablet registration successful", 0).show();
                                break;
                            }
                        } else {
                            Toast.makeText(WebSocketClient.this.mContext, "Tablet registration failed", 0).show();
                            break;
                        }
                    } else {
                        WebSocketClient.this.connectionSocket.send("SetId@" + MainActivity.TABLET_ID);
                        break;
                    }
                    break;
                case 4:
                    Toast.makeText(WebSocketClient.this.mContext, "WebSocket closed: " + msg.obj, 0).show();
                    break;
            }
            WebSocketClient.this.progressDialog.dismissProgressDialog();
            return false;
        }
    });
    /* access modifiers changed from: private */
    public boolean isConnected = false;
    /* access modifiers changed from: private */
    public Context mContext;
    /* access modifiers changed from: private */
    public String mUrl;
    /* access modifiers changed from: private */
    public progressDialog progressDialog;

    public WebSocketClient(Context context) {
        this.mContext = context;
        this.progressDialog = new progressDialog(this.mContext);
    }

    public void Disconnect() {
        WebSocket webSocket = this.connectionSocket;
        if (webSocket != null) {
            webSocket.close(1000, "Closed");
        }
    }

    public void Reconnect() {
        if (!this.isConnected) {
            Log.d(TAG, "WebSocket reconnecting...");
            Disconnect();
            Connect(this.mUrl);
        }
    }

    public void Connect(String url) {
        this.progressDialog.showConnectDialog();
        this.mUrl = url.toLowerCase().replace("http", "ws");
        new Thread(new ConnectRunnable()).start();
    }

    public class ConnectRunnable implements Runnable {
        public ConnectRunnable() {
        }

        public void run() {
            try {
                Log.d(WebSocketClient.TAG, "WebSocket connecting...");
                OkHttpClient client = new OkHttpClient();
                Request request = new Request.Builder().url(WebSocketClient.this.mUrl + "/ws").build();
                WebSocketClient webSocketClient = WebSocketClient.this;
                WebSocket unused = WebSocketClient.this.connectionSocket = client.newWebSocket(request, new SocketListener(webSocketClient.mContext));
            } catch (Exception e) {
                Log.d(WebSocketClient.TAG, "WebSocket error: " + e.getMessage());
                Message message = Message.obtain();
                message.what = 2;
                message.obj = e.getMessage();
                WebSocketClient.this.handler.sendMessage(message);
            }
        }
    }

    public class SocketListener extends WebSocketListener {
        Context mContext;

        public SocketListener(Context context) {
            this.mContext = context;
        }

        public void onOpen(WebSocket webSocket, Response response) {
            super.onOpen(webSocket, response);
            Log.d(WebSocketClient.TAG, "WebSocket connected: " + response.message());
            boolean unused = WebSocketClient.this.isConnected = true;
            Message message = Message.obtain();
            message.what = 1;
            message.obj = response.message();
            WebSocketClient.this.handler.sendMessage(message);
        }

        public void onMessage(WebSocket webSocket, String text) {
            super.onMessage(webSocket, text);
            Log.d(WebSocketClient.TAG, "WebSocket message: " + text);
            Message message = Message.obtain();
            message.what = 3;
            message.obj = text;
            WebSocketClient.this.handler.sendMessage(message);
        }

        public void onMessage(WebSocket webSocket, ByteString bytes) {
            super.onMessage(webSocket, bytes);
        }

        public void onClosing(WebSocket webSocket, int code, String reason) {
            super.onClosing(webSocket, code, reason);
        }

        public void onClosed(WebSocket webSocket, int code, String reason) {
            super.onClosed(webSocket, code, reason);
            Log.d(WebSocketClient.TAG, "WebSocket closed... : " + reason);
            boolean unused = WebSocketClient.this.isConnected = false;
            Message message = Message.obtain();
            message.what = 4;
            message.obj = reason;
            WebSocketClient.this.handler.sendMessage(message);
        }

        public void onFailure(WebSocket webSocket, Throwable t, Response response) {
            super.onFailure(webSocket, t, response);
            Log.d(WebSocketClient.TAG, "WebSocket error: " + t.getMessage());
            boolean unused = WebSocketClient.this.isConnected = false;
            Message message = Message.obtain();
            message.what = 2;
            message.obj = t.getMessage();
            WebSocketClient.this.handler.sendMessage(message);
        }
    }
}
