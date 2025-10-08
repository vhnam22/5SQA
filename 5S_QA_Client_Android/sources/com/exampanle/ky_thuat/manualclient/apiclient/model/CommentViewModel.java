package com.exampanle.ky_thuat.manualclient.apiclient.model;

import java.util.UUID;

public class CommentViewModel {
    private String content = null;
    private UUID id = null;
    private UUID requestId = null;

    public String getContent() {
        return this.content;
    }

    public void setContent(String content2) {
        this.content = content2;
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
}
