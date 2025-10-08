package com.exampanle.ky_thuat.manualclient.apiclient.model;

public class UIDto {
    private String code;
    private boolean isShow;
    private String name;

    public UIDto(String name2, String code2, boolean isShow2) {
        this.name = name2;
        this.code = code2;
        this.isShow = isShow2;
    }

    public String getName() {
        return this.name;
    }

    public void setName(String name2) {
        this.name = name2;
    }

    public String getCode() {
        return this.code;
    }

    public void setCode(String code2) {
        this.code = code2;
    }

    public boolean isShow() {
        return this.isShow;
    }

    public void setShow(boolean show) {
        this.isShow = show;
    }
}
