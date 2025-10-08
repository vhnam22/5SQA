package com.exampanle.ky_thuat.manualclient.apiclient;

import com.google.gson.Gson;
import com.google.gson.GsonBuilder;
import com.google.gson.JsonParseException;
import com.google.gson.TypeAdapter;
import com.google.gson.internal.bind.util.ISO8601Utils;
import com.google.gson.stream.JsonReader;
import com.google.gson.stream.JsonToken;
import com.google.gson.stream.JsonWriter;
import io.gsonfire.GsonFireBuilder;
import java.io.IOException;
import java.io.StringReader;
import java.lang.reflect.Type;
import java.text.DateFormat;
import java.text.ParseException;
import java.text.ParsePosition;
import java.util.Date;
import org.threeten.bp.LocalDate;
import org.threeten.bp.OffsetDateTime;
import org.threeten.bp.format.DateTimeFormatter;

public class JSON {
    private DateTypeAdapter dateTypeAdapter = new DateTypeAdapter();
    private Gson gson = createGson().registerTypeAdapter(Date.class, this.dateTypeAdapter).registerTypeAdapter(java.sql.Date.class, this.sqlDateTypeAdapter).registerTypeAdapter(OffsetDateTime.class, this.offsetDateTimeTypeAdapter).registerTypeAdapter(LocalDate.class, this.localDateTypeAdapter).create();
    private boolean isLenientOnJson = false;
    private LocalDateTypeAdapter localDateTypeAdapter = new LocalDateTypeAdapter(this);
    private OffsetDateTimeTypeAdapter offsetDateTimeTypeAdapter = new OffsetDateTimeTypeAdapter();
    private SqlDateTypeAdapter sqlDateTypeAdapter = new SqlDateTypeAdapter();

    public static GsonBuilder createGson() {
        return new GsonFireBuilder().createGsonBuilder();
    }

    public String serialize(Object obj) {
        return this.gson.toJson(obj);
    }

    public <T> T deserialize(String body, Type returnType) {
        try {
            if (!this.isLenientOnJson) {
                return this.gson.fromJson(body, returnType);
            }
            JsonReader jsonReader = new JsonReader(new StringReader(body));
            jsonReader.setLenient(true);
            return this.gson.fromJson(jsonReader, returnType);
        } catch (JsonParseException e) {
            if (returnType.equals(String.class)) {
                return body;
            }
            throw e;
        }
    }

    public static class OffsetDateTimeTypeAdapter extends TypeAdapter<OffsetDateTime> {
        private DateTimeFormatter formatter;

        public OffsetDateTimeTypeAdapter() {
            this(DateTimeFormatter.ISO_OFFSET_DATE_TIME);
        }

        public OffsetDateTimeTypeAdapter(DateTimeFormatter formatter2) {
            this.formatter = formatter2;
        }

        public void setFormat(DateTimeFormatter dateFormat) {
            this.formatter = dateFormat;
        }

        public void write(JsonWriter out, OffsetDateTime date) throws IOException {
            if (date == null) {
                out.nullValue();
            } else {
                out.value(this.formatter.format(date));
            }
        }

        public OffsetDateTime read(JsonReader in) throws IOException {
            switch (AnonymousClass1.$SwitchMap$com$google$gson$stream$JsonToken[in.peek().ordinal()]) {
                case 1:
                    in.nextNull();
                    return null;
                default:
                    String date = in.nextString();
                    if (date.endsWith("+0000")) {
                        date = date.substring(0, date.length() - 5) + "Z";
                    }
                    return OffsetDateTime.parse(date, this.formatter);
            }
        }
    }

    /* renamed from: com.exampanle.ky_thuat.manualclient.apiclient.JSON$1  reason: invalid class name */
    static /* synthetic */ class AnonymousClass1 {
        static final /* synthetic */ int[] $SwitchMap$com$google$gson$stream$JsonToken;

        static {
            int[] iArr = new int[JsonToken.values().length];
            $SwitchMap$com$google$gson$stream$JsonToken = iArr;
            try {
                iArr[JsonToken.NULL.ordinal()] = 1;
            } catch (NoSuchFieldError e) {
            }
        }
    }

    public class LocalDateTypeAdapter extends TypeAdapter<LocalDate> {
        private DateTimeFormatter formatter;

        public LocalDateTypeAdapter(JSON this$02) {
            this(DateTimeFormatter.ISO_LOCAL_DATE);
        }

        public LocalDateTypeAdapter(DateTimeFormatter formatter2) {
            this.formatter = formatter2;
        }

        public void setFormat(DateTimeFormatter dateFormat) {
            this.formatter = dateFormat;
        }

        public void write(JsonWriter out, LocalDate date) throws IOException {
            if (date == null) {
                out.nullValue();
            } else {
                out.value(this.formatter.format(date));
            }
        }

        public LocalDate read(JsonReader in) throws IOException {
            switch (AnonymousClass1.$SwitchMap$com$google$gson$stream$JsonToken[in.peek().ordinal()]) {
                case 1:
                    in.nextNull();
                    return null;
                default:
                    return LocalDate.parse(in.nextString(), this.formatter);
            }
        }
    }

    public static class SqlDateTypeAdapter extends TypeAdapter<java.sql.Date> {
        private DateFormat dateFormat;

        public SqlDateTypeAdapter() {
        }

        public SqlDateTypeAdapter(DateFormat dateFormat2) {
            this.dateFormat = dateFormat2;
        }

        public void setFormat(DateFormat dateFormat2) {
            this.dateFormat = dateFormat2;
        }

        public void write(JsonWriter out, java.sql.Date date) throws IOException {
            String value;
            if (date == null) {
                out.nullValue();
                return;
            }
            DateFormat dateFormat2 = this.dateFormat;
            if (dateFormat2 != null) {
                value = dateFormat2.format(date);
            } else {
                value = date.toString();
            }
            out.value(value);
        }

        public java.sql.Date read(JsonReader in) throws IOException {
            switch (AnonymousClass1.$SwitchMap$com$google$gson$stream$JsonToken[in.peek().ordinal()]) {
                case 1:
                    in.nextNull();
                    return null;
                default:
                    String date = in.nextString();
                    try {
                        if (this.dateFormat != null) {
                            return new java.sql.Date(this.dateFormat.parse(date).getTime());
                        }
                        return new java.sql.Date(ISO8601Utils.parse(date, new ParsePosition(0)).getTime());
                    } catch (ParseException e) {
                        throw new JsonParseException((Throwable) e);
                    }
            }
        }
    }

    public static class DateTypeAdapter extends TypeAdapter<Date> {
        private DateFormat dateFormat;

        public DateTypeAdapter() {
        }

        public DateTypeAdapter(DateFormat dateFormat2) {
            this.dateFormat = dateFormat2;
        }

        public void setFormat(DateFormat dateFormat2) {
            this.dateFormat = dateFormat2;
        }

        public void write(JsonWriter out, Date date) throws IOException {
            String value;
            if (date == null) {
                out.nullValue();
                return;
            }
            DateFormat dateFormat2 = this.dateFormat;
            if (dateFormat2 != null) {
                value = dateFormat2.format(date);
            } else {
                value = ISO8601Utils.format(date, true);
            }
            out.value(value);
        }

        public Date read(JsonReader in) throws IOException {
            try {
                switch (AnonymousClass1.$SwitchMap$com$google$gson$stream$JsonToken[in.peek().ordinal()]) {
                    case 1:
                        in.nextNull();
                        return null;
                    default:
                        String date = in.nextString();
                        DateFormat dateFormat2 = this.dateFormat;
                        if (dateFormat2 != null) {
                            return dateFormat2.parse(date);
                        }
                        return ISO8601Utils.parse(date, new ParsePosition(0));
                }
            } catch (ParseException e) {
                throw new JsonParseException((Throwable) e);
            } catch (IllegalArgumentException e2) {
                throw new JsonParseException((Throwable) e2);
            }
            throw new JsonParseException((Throwable) e2);
        }
    }
}
