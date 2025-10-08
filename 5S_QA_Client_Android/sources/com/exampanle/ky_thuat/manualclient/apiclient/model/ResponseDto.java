package com.exampanle.ky_thuat.manualclient.apiclient.model;

import java.util.List;

public class ResponseDto {
    private Integer count = null;
    private Object data = null;
    private List<ResponseMessage> messages = null;
    private Boolean success = null;

    public Object getData() {
        return this.data;
    }

    public void setData(Object data2) {
        this.data = data2;
    }

    public Boolean isSuccess() {
        return this.success;
    }

    public List<ResponseMessage> getMessages() {
        return this.messages;
    }

    public void setMessages(List<ResponseMessage> messages2) {
        this.messages = messages2;
    }

    public Integer getCount() {
        return this.count;
    }

    public void setCount(Integer count2) {
        this.count = count2;
    }
}
