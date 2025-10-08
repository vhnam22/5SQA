package com.exampanle.ky_thuat.manualclient.apiclient.model;

public class LoginDto {
    private String password = null;
    private String username = null;

    public String getUsername() {
        return this.username;
    }

    public void setUsername(String username2) {
        this.username = username2;
    }

    public String getPassword() {
        return this.password;
    }

    public void setPassword(String password2) {
        this.password = password2;
    }
}
