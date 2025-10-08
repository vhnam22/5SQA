package com.exampanle.ky_thuat.manualclient.apiclient.model;

import java.util.UUID;
import org.threeten.bp.OffsetDateTime;

public class ResultViewModel {
    public int cavity = 1;
    public OffsetDateTime created = null;
    public String createdBy = null;
    public String history = null;
    public UUID id = null;
    public String importantName = null;
    public Boolean isActivated = null;
    public String judge = null;
    public Double lower = null;
    public Double lsl = null;
    public String machineName = null;
    public String machineTypeName = null;
    public String measurementCode = null;
    public UUID measurementId = null;
    public OffsetDateTime modified = null;
    public String modifiedBy = null;
    public String name = null;
    public UUID requestId = null;
    public String result = null;
    public int sample = 1;
    public String staffName = null;
    public String unit = null;
    public Double upper = null;
    public Double usl = null;
    public String value = null;

    public String toString() {
        return this.measurementCode + (this.name == null ? "" : " (" + this.name + ")");
    }

    public UUID getId() {
        return this.id;
    }

    public void setId(UUID id2) {
        this.id = id2;
    }

    public UUID getRequestId() {
        return this.requestId;
    }

    public void setRequestId(UUID requestId2) {
        this.requestId = requestId2;
    }

    public UUID getMeasurementId() {
        return this.measurementId;
    }

    public void setMeasurementId(UUID measurementId2) {
        this.measurementId = measurementId2;
    }

    public String getMachineTypeName() {
        return this.machineTypeName;
    }

    public void setMachineTypeName(String machineTypeName2) {
        this.machineTypeName = machineTypeName2;
    }

    public String getMeasurementCode() {
        return this.measurementCode;
    }

    public void setMeasurementCode(String measurementCode2) {
        this.measurementCode = measurementCode2;
    }

    public String getName() {
        return this.name;
    }

    public void setName(String name2) {
        this.name = name2;
    }

    public String getImportantName() {
        return this.importantName;
    }

    public void setImportantName(String importantName2) {
        this.importantName = importantName2;
    }

    public String getValue() {
        return this.value;
    }

    public void setValue(String value2) {
        this.value = value2;
    }

    public String getUnit() {
        return this.unit;
    }

    public void setUnit(String unit2) {
        this.unit = unit2;
    }

    public Double getUpper() {
        return this.upper;
    }

    public void setUpper(Double upper2) {
        this.upper = upper2;
    }

    public Double getLower() {
        return this.lower;
    }

    public void setLower(Double lower2) {
        this.lower = lower2;
    }

    public Double getLSL() {
        return this.lsl;
    }

    public void setLSL(Double lsl2) {
        this.lsl = lsl2;
    }

    public Double getUSL() {
        return this.usl;
    }

    public void setUSL(Double usl2) {
        this.usl = usl2;
    }

    public String getResult() {
        return this.result;
    }

    public void setResult(String result2) {
        this.result = result2;
    }

    public String getJudge() {
        return this.judge;
    }

    public void setJudge(String judge2) {
        this.judge = judge2;
    }

    public String getMachineName() {
        return this.machineName;
    }

    public void setMachineName(String machineName2) {
        this.machineName = machineName2;
    }

    public String getStaffName() {
        return this.staffName;
    }

    public void setStaffName(String staffName2) {
        this.staffName = staffName2;
    }

    public String getHistory() {
        return this.history;
    }

    public void setHistory(String history2) {
        this.history = history2;
    }

    public OffsetDateTime getCreated() {
        return this.created;
    }

    public void setCreated(OffsetDateTime created2) {
        this.created = created2;
    }

    public OffsetDateTime getModified() {
        return this.modified;
    }

    public void setModified(OffsetDateTime modified2) {
        this.modified = modified2;
    }

    public String getCreatedBy() {
        return this.createdBy;
    }

    public void setCreatedBy(String createdBy2) {
        this.createdBy = createdBy2;
    }

    public String getModifiedBy() {
        return this.modifiedBy;
    }

    public void setModifiedBy(String modifiedBy2) {
        this.modifiedBy = modifiedBy2;
    }

    public Boolean getIsActivated() {
        return this.isActivated;
    }

    public void setIsActivated(Boolean isactivated) {
        this.isActivated = isactivated;
    }

    public int getSample() {
        return this.sample;
    }

    public void setSample(int sample2) {
        this.sample = sample2;
    }

    public int getCavity() {
        return this.cavity;
    }

    public void setCavity(int cavity2) {
        this.cavity = cavity2;
    }
}
