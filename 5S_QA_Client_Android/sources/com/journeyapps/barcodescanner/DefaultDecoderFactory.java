package com.journeyapps.barcodescanner;

import com.google.zxing.BarcodeFormat;
import com.google.zxing.DecodeHintType;
import com.google.zxing.MultiFormatReader;
import java.util.Collection;
import java.util.EnumMap;
import java.util.Map;

public class DefaultDecoderFactory implements DecoderFactory {
    private String characterSet;
    private Collection<BarcodeFormat> decodeFormats;
    private Map<DecodeHintType, ?> hints;
    private int scanType;

    public DefaultDecoderFactory() {
    }

    public DefaultDecoderFactory(Collection<BarcodeFormat> decodeFormats2) {
        this.decodeFormats = decodeFormats2;
    }

    public DefaultDecoderFactory(Collection<BarcodeFormat> decodeFormats2, Map<DecodeHintType, ?> hints2, String characterSet2, int scanType2) {
        this.decodeFormats = decodeFormats2;
        this.hints = hints2;
        this.characterSet = characterSet2;
        this.scanType = scanType2;
    }

    public Decoder createDecoder(Map<DecodeHintType, ?> baseHints) {
        Map<DecodeHintType, Object> hints2 = new EnumMap<>(DecodeHintType.class);
        hints2.putAll(baseHints);
        Map<DecodeHintType, ?> map = this.hints;
        if (map != null) {
            hints2.putAll(map);
        }
        if (this.decodeFormats != null) {
            hints2.put(DecodeHintType.POSSIBLE_FORMATS, this.decodeFormats);
        }
        if (this.characterSet != null) {
            hints2.put(DecodeHintType.CHARACTER_SET, this.characterSet);
        }
        MultiFormatReader reader = new MultiFormatReader();
        reader.setHints(hints2);
        switch (this.scanType) {
            case 0:
                return new Decoder(reader);
            case 1:
                return new InvertedDecoder(reader);
            case 2:
                return new MixedDecoder(reader);
            default:
                return new Decoder(reader);
        }
    }
}
