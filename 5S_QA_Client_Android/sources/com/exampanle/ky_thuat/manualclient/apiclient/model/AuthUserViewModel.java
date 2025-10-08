package com.exampanle.ky_thuat.manualclient.apiclient.model;

import java.util.UUID;

public class AuthUserViewModel {
    private String fullName = null;
    private String token = null;
    private UUID userId = null;

    public String getFullName() {
        return this.fullName;
    }

    public void setFullName(String fullName2) {
        this.fullName = fullName2;
    }

    public String getRefreshToken() {
        return this.token;
    }

    public void setRefreshToken(String refreshToken) {
        this.token = refreshToken;
    }

    public UUID getId() {
        return this.userId;
    }

    public void setId(UUID id) {
        this.userId = id;
    }
}
