package com.exampanle.ky_thuat.manualclient.apiclient.model;

import java.util.UUID;

public class ReceiveResult {
    private String MachineName = null;
    private String MachineTypeName = null;
    private String Result = null;
    private UUID TabletId = null;
    private String Unit = null;

    public UUID getTabletId() {
        return this.TabletId;
    }

    public void setTabletId(UUID TabletId2) {
        this.TabletId = TabletId2;
    }

    public String getMachineName() {
        return this.MachineName;
    }

    public void setMachineName(String MachineName2) {
        this.MachineName = MachineName2;
    }

    public String getMachineTypeName() {
        return this.MachineTypeName;
    }

    public void setMachineTypeName(String MachineTypeName2) {
        this.MachineTypeName = MachineTypeName2;
    }

    public String getResult() {
        return this.Result;
    }

    public void setResult(String Result2) {
        this.Result = Result2;
    }

    public String getUnit() {
        return this.Unit;
    }

    public void setUnit(String Unit2) {
        this.Unit = Unit2;
    }
}
