package com.exampanle.ky_thuat.manualclient.apiclient.model;

public class ArrayChartDto {
    private int count;
    private double value;

    public ArrayChartDto(double value2, int count2) {
        this.value = value2;
        this.count = count2;
    }

    public double getValue() {
        return this.value;
    }

    public void setValue(double value2) {
        this.value = value2;
    }

    public int getCount() {
        return this.count;
    }

    public void setCount(int count2) {
        this.count = count2;
    }
}
