package com.exampanle.ky_thuat.manualclient.apiclient.model;

import java.util.UUID;

public class RequestDetailViewModel {
    private String cavity = null;
    private UUID id = null;
    private Boolean isActivated = null;
    private String pinDate = null;
    private String produceNo = null;
    private UUID requestId = null;
    private int sample = 1;
    private String shotMold = null;

    public UUID getRequestId() {
        return this.requestId;
    }

    public void setRequestId(UUID requestId2) {
        this.requestId = requestId2;
    }

    public String getCavity() {
        return this.cavity;
    }

    public void setCavity(String cavity2) {
        this.cavity = cavity2;
    }

    public String getPinDate() {
        return this.pinDate;
    }

    public void setPinDate(String pinDate2) {
        this.pinDate = pinDate2;
    }

    public String getShotMold() {
        return this.shotMold;
    }

    public void setShotMold(String shotMold2) {
        this.shotMold = shotMold2;
    }

    public String getProduceNo() {
        return this.produceNo;
    }

    public void setProduceNo(String produceNo2) {
        this.produceNo = produceNo2;
    }

    public int getSample() {
        return this.sample;
    }

    public void setSample(int sample2) {
        this.sample = sample2;
    }

    public UUID getId() {
        return this.id;
    }

    public void setId(UUID id2) {
        this.id = id2;
    }

    public Boolean getActivated() {
        return this.isActivated;
    }

    public void setActivated(Boolean activated) {
        this.isActivated = activated;
    }
}
