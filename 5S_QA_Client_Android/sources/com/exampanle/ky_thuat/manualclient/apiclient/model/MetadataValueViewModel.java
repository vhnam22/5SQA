package com.exampanle.ky_thuat.manualclient.apiclient.model;

import java.util.UUID;

public class MetadataValueViewModel {
    private String code = null;
    private UUID id = null;
    private String name = null;

    public String toString() {
        return this.name;
    }

    public UUID getId() {
        return this.id;
    }

    public void setId(UUID id2) {
        this.id = id2;
    }

    public String getCode() {
        return this.code;
    }

    public void setCode(String code2) {
        this.code = code2;
    }

    public String getName() {
        return this.name;
    }

    public void setName(String name2) {
        this.name = name2;
    }
}
