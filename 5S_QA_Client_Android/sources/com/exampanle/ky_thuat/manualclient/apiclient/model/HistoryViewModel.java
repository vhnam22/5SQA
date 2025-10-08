package com.exampanle.ky_thuat.manualclient.apiclient.model;

public class HistoryViewModel {
    private String Created = null;
    private String CreatedBy = null;
    private String MachineName = null;
    private String Value = null;

    public String getValue() {
        return this.Value;
    }

    public void setValue(String Value2) {
        this.Value = Value2;
    }

    public String getMachineName() {
        return this.MachineName;
    }

    public void setMachineName(String MachineName2) {
        this.MachineName = MachineName2;
    }

    public String getCreatedBy() {
        return this.CreatedBy;
    }

    public void setCreatedBy(String CreatedBy2) {
        this.CreatedBy = CreatedBy2;
    }

    public String getCreated() {
        return this.Created;
    }

    public void setCreated(String Created2) {
        this.Created = Created2;
    }
}
