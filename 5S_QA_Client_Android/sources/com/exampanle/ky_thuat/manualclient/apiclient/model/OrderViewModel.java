package com.exampanle.ky_thuat.manualclient.apiclient.model;

import java.util.UUID;

public class OrderViewModel {
    private UUID id = null;
    private String name = null;
    private String orderNo = null;
    private UUID productId = null;
    private Integer quantity = null;
    private Integer remain = null;

    public String toString() {
        return this.name;
    }

    public UUID getId() {
        return this.id;
    }

    public void setId(UUID id2) {
        this.id = id2;
    }

    public UUID getProductId() {
        return this.productId;
    }

    public void setProductId(UUID productId2) {
        this.id = productId2;
    }

    public String getName() {
        return this.name;
    }

    public void setName(String name2) {
        this.name = name2;
    }

    public String getOrderNo() {
        return this.orderNo;
    }

    public void setOrderNo(String orderNo2) {
        this.orderNo = orderNo2;
    }

    public Integer getQuantity() {
        return this.quantity;
    }

    public void setQuantity(Integer quantity2) {
        this.quantity = quantity2;
    }

    public Integer getRemain() {
        return this.remain;
    }

    public void setRemain(Integer remain2) {
        this.remain = remain2;
    }
}
