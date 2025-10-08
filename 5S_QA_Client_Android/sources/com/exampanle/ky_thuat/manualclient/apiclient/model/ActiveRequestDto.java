package com.exampanle.ky_thuat.manualclient.apiclient.model;

import java.util.UUID;

public class ActiveRequestDto {
    private UUID id = null;
    private String status = null;

    public UUID getId() {
        return this.id;
    }

    public void setId(UUID id2) {
        this.id = id2;
    }

    public String getStatus() {
        return this.status;
    }

    public void setStatus(String status2) {
        this.status = status2;
    }
}
