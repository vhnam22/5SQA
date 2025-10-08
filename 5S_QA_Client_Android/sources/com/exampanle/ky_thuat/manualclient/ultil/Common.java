package com.exampanle.ky_thuat.manualclient.ultil;

import android.content.Context;
import android.content.SharedPreferences;

public class Common {
    private Context mContext;
    private SharedPreferences sharedpreferences;

    public Common(Context context) {
        this.mContext = context;
        this.sharedpreferences = context.getSharedPreferences(Constant.MyPREFERENCES, 0);
    }

    public void clearAllData() {
        SharedPreferences.Editor editor = this.sharedpreferences.edit();
        editor.clear();
        editor.commit();
    }

    public Object getData(String mark, int type) {
        Object result = new Object();
        switch (type) {
            case 1:
                return this.sharedpreferences.getString(mark, "");
            case 2:
                return Boolean.valueOf(this.sharedpreferences.getBoolean(mark, false));
            case 3:
                return Integer.valueOf(this.sharedpreferences.getInt(mark, 20));
            default:
                return result;
        }
    }

    public Object getData(String mark, Object value, int type) {
        Object result = new Object();
        switch (type) {
            case 1:
                return this.sharedpreferences.getString(mark, (String) value);
            case 2:
                return Boolean.valueOf(this.sharedpreferences.getBoolean(mark, ((Boolean) value).booleanValue()));
            case 3:
                return Integer.valueOf(this.sharedpreferences.getInt(mark, ((Integer) value).intValue()));
            default:
                return result;
        }
    }

    public void setData(String mark, Object obj, int type) {
        SharedPreferences.Editor editor = this.sharedpreferences.edit();
        switch (type) {
            case 1:
                editor.putString(mark, (String) obj);
                break;
            case 2:
                editor.putBoolean(mark, ((Boolean) obj).booleanValue());
                break;
            case 3:
                editor.putInt(mark, ((Integer) obj).intValue());
                break;
        }
        editor.commit();
    }
}
