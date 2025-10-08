package com.exampanle.ky_thuat.manualclient.apiclient.model;

import android.os.Parcel;
import android.os.Parcelable;
import java.util.UUID;
import org.threeten.bp.OffsetDateTime;

public class RequestViewModel implements Parcelable {
    public static final Parcelable.Creator<RequestViewModel> CREATOR = new Parcelable.Creator<RequestViewModel>() {
        public RequestViewModel createFromParcel(Parcel in) {
            return new RequestViewModel(in);
        }

        public RequestViewModel[] newArray(int size) {
            return new RequestViewModel[size];
        }
    };
    private String code;
    private OffsetDateTime created;
    private String createdBy;
    private OffsetDateTime date;
    private UUID id;
    private String intention;
    private Boolean isActivated;
    private String line;
    private String lot;
    private OffsetDateTime modified;
    private String modifiedBy;
    private String name;
    private UUID productId;
    private String productImageUrl;
    private String productName;
    private String productStage;
    private Integer quantity;
    private int sample;
    private String status;
    private String type;

    public RequestViewModel() {
        this.code = null;
        this.name = null;
        this.productId = null;
        this.productStage = null;
        this.productName = null;
        this.productImageUrl = null;
        this.date = null;
        this.sample = 1;
        this.type = null;
        this.status = null;
        this.intention = null;
        this.line = null;
        this.lot = null;
        this.quantity = null;
        this.id = null;
        this.created = null;
        this.createdBy = null;
        this.modified = null;
        this.modifiedBy = null;
        this.isActivated = null;
    }

    protected RequestViewModel(Parcel in) {
        Boolean bool = null;
        this.code = null;
        this.name = null;
        this.productId = null;
        this.productStage = null;
        this.productName = null;
        this.productImageUrl = null;
        this.date = null;
        boolean z = true;
        this.sample = 1;
        this.type = null;
        this.status = null;
        this.intention = null;
        this.line = null;
        this.lot = null;
        this.quantity = null;
        this.id = null;
        this.created = null;
        this.createdBy = null;
        this.modified = null;
        this.modifiedBy = null;
        this.isActivated = null;
        this.code = in.readString();
        this.name = in.readString();
        this.productId = (UUID) in.readSerializable();
        this.productStage = in.readString();
        this.productName = in.readString();
        this.productImageUrl = in.readString();
        this.date = (OffsetDateTime) in.readSerializable();
        this.sample = in.readInt();
        this.type = in.readString();
        this.status = in.readString();
        this.intention = in.readString();
        this.line = in.readString();
        this.lot = in.readString();
        this.quantity = Integer.valueOf(in.readInt());
        this.id = (UUID) in.readSerializable();
        this.created = (OffsetDateTime) in.readSerializable();
        this.createdBy = in.readString();
        this.modified = (OffsetDateTime) in.readSerializable();
        this.modifiedBy = in.readString();
        byte tmpIsActivated = in.readByte();
        if (tmpIsActivated != 0) {
            bool = Boolean.valueOf(tmpIsActivated != 1 ? false : z);
        }
        this.isActivated = bool;
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

    public UUID getProductId() {
        return this.productId;
    }

    public void setProductId(UUID productId2) {
        this.productId = productId2;
    }

    public String getProductStage() {
        return this.productStage;
    }

    public void setProductStage(String productStage2) {
        this.productStage = productStage2;
    }

    public String getProductName() {
        return this.productName;
    }

    public void setProductName(String productName2) {
        this.productName = productName2;
    }

    public String getProductImageUrl() {
        return this.productImageUrl;
    }

    public void setProductImageUrl(String productImageUrl2) {
        this.productImageUrl = productImageUrl2;
    }

    public OffsetDateTime getDate() {
        return this.date;
    }

    public void setDate(OffsetDateTime date2) {
        this.date = date2;
    }

    public int getSample() {
        return this.sample;
    }

    public void setSample(int sample2) {
        this.sample = sample2;
    }

    public String getType() {
        return this.type;
    }

    public void setType(String type2) {
        this.type = type2;
    }

    public String getStatus() {
        return this.status;
    }

    public void setStatus(String status2) {
        this.status = status2;
    }

    public String getIntention() {
        return this.intention;
    }

    public void setIntention(String intention2) {
        this.intention = intention2;
    }

    public String getLine() {
        return this.line;
    }

    public void setLine(String line2) {
        this.line = line2;
    }

    public String getLot() {
        return this.lot;
    }

    public void setLot(String lot2) {
        this.lot = lot2;
    }

    public Integer getQuantity() {
        return this.quantity;
    }

    public void setQuantity(Integer quantity2) {
        this.quantity = quantity2;
    }

    public UUID getId() {
        return this.id;
    }

    public void setId(UUID id2) {
        this.id = id2;
    }

    public OffsetDateTime getCreated() {
        return this.created;
    }

    public void setCreated(OffsetDateTime created2) {
        this.created = created2;
    }

    public String getCreatedBy() {
        return this.createdBy;
    }

    public void setCreatedBy(String createdBy2) {
        this.createdBy = createdBy2;
    }

    public OffsetDateTime getModified() {
        return this.modified;
    }

    public void setModified(OffsetDateTime modified2) {
        this.modified = modified2;
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

    public void setIsActivated(Boolean isActivated2) {
        this.isActivated = isActivated2;
    }

    public int describeContents() {
        return 0;
    }

    public void writeToParcel(Parcel parcel, int i) {
        parcel.writeString(this.code);
        parcel.writeString(this.name);
        parcel.writeSerializable(this.productId);
        parcel.writeString(this.productStage);
        parcel.writeString(this.productName);
        parcel.writeString(this.productImageUrl);
        parcel.writeSerializable(this.date);
        parcel.writeInt(this.sample);
        parcel.writeString(this.type);
        parcel.writeString(this.status);
        parcel.writeString(this.intention);
        parcel.writeString(this.line);
        parcel.writeString(this.lot);
        Integer num = this.quantity;
        int i2 = 0;
        parcel.writeInt(num == null ? 0 : num.intValue());
        parcel.writeSerializable(this.id);
        parcel.writeSerializable(this.created);
        parcel.writeString(this.createdBy);
        parcel.writeSerializable(this.modified);
        parcel.writeString(this.modifiedBy);
        Boolean bool = this.isActivated;
        if (bool != null) {
            i2 = bool.booleanValue() ? 1 : 2;
        }
        parcel.writeByte((byte) i2);
    }
}
