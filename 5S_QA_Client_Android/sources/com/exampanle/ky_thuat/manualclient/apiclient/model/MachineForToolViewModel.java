package com.exampanle.ky_thuat.manualclient.apiclient.model;

import java.util.UUID;

public class MachineForToolViewModel {
    private UUID id = null;
    private Boolean isHasTool = null;
    private String machineTypeName = null;
    private String name = null;

    public String getName() {
        return this.name;
    }

    public void setName(String name2) {
        this.name = name2;
    }

    public String getMachineTypeName() {
        return this.machineTypeName;
    }

    public void setMachineTypeName(String machineTypeName2) {
        this.machineTypeName = machineTypeName2;
    }

    public Boolean getHasTool() {
        return this.isHasTool;
    }

    public void setHasTool(Boolean hasTool) {
        this.isHasTool = hasTool;
    }

    public UUID getId() {
        return this.id;
    }

    public void setId(UUID id2) {
        this.id = id2;
    }
}
