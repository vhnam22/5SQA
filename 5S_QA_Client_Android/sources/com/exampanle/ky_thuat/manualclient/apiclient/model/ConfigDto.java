package com.exampanle.ky_thuat.manualclient.apiclient.model;

public class ConfigDto {
    private String displayName;
    private boolean isShow;
    private String name;

    public ConfigDto(String name2, String displayName2, boolean isShow2) {
        this.name = name2;
        this.displayName = displayName2;
        this.isShow = isShow2;
    }

    public String getName() {
        return this.name;
    }

    public void setName(String name2) {
        this.name = name2;
    }

    public String getDisplayName() {
        return this.displayName;
    }

    public void setDisplayName(String displayName2) {
        this.displayName = displayName2;
    }

    public boolean isShow() {
        return this.isShow;
    }

    public void setShow(boolean show) {
        this.isShow = show;
    }
}
