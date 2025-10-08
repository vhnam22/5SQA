package com.exampanle.ky_thuat.manualclient.apiclient.model;

import java.util.List;

public class QueryArgs {
    private Integer limit = null;
    private String order = null;
    private Integer page = null;
    private String predicate = null;
    private List<Object> predicateParameters = null;

    public String getPredicate() {
        return this.predicate;
    }

    public void setPredicate(String predicate2) {
        this.predicate = predicate2;
    }

    public List<Object> getPredicateParameters(String[] strings) {
        return this.predicateParameters;
    }

    public void setPredicateParameters(List<Object> predicateParameters2) {
        this.predicateParameters = predicateParameters2;
    }

    public String getOrder() {
        return this.order;
    }

    public void setOrder(String order2) {
        this.order = order2;
    }

    public Integer getPage() {
        return this.page;
    }

    public void setPage(Integer page2) {
        this.page = page2;
    }

    public Integer getLimit() {
        return this.limit;
    }

    public void setLimit(Integer limit2) {
        this.limit = limit2;
    }
}
