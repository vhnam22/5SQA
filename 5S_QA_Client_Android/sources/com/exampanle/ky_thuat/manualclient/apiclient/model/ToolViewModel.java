package com.exampanle.ky_thuat.manualclient.apiclient.model;

import java.util.UUID;

public class ToolViewModel {
    private UUID machineId = null;
    private String machineTypeName = null;
    private UUID tabletId = null;

    public UUID getTabletId() {
        return this.tabletId;
    }

    public void setTabletId(UUID tabletId2) {
        this.tabletId = tabletId2;
    }

    public String getMachineTypeName() {
        return this.machineTypeName;
    }

    public void setMachineTypeName(String machineTypeName2) {
        this.machineTypeName = machineTypeName2;
    }

    public UUID getMachineId() {
        return this.machineId;
    }

    public void setMachineId(UUID machineId2) {
        this.machineId = machineId2;
    }
}
