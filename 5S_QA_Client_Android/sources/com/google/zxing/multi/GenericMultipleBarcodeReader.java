package com.google.zxing.multi;

import com.google.zxing.BinaryBitmap;
import com.google.zxing.DecodeHintType;
import com.google.zxing.NotFoundException;
import com.google.zxing.Reader;
import com.google.zxing.ReaderException;
import com.google.zxing.Result;
import com.google.zxing.ResultPoint;
import java.util.ArrayList;
import java.util.Iterator;
import java.util.List;
import java.util.Map;

public final class GenericMultipleBarcodeReader implements MultipleBarcodeReader {
    private static final int MAX_DEPTH = 4;
    private static final int MIN_DIMENSION_TO_RECUR = 100;
    private final Reader delegate;

    public GenericMultipleBarcodeReader(Reader delegate2) {
        this.delegate = delegate2;
    }

    public Result[] decodeMultiple(BinaryBitmap image) throws NotFoundException {
        return decodeMultiple(image, (Map<DecodeHintType, ?>) null);
    }

    public Result[] decodeMultiple(BinaryBitmap image, Map<DecodeHintType, ?> hints) throws NotFoundException {
        ArrayList arrayList = new ArrayList();
        doDecodeMultiple(image, hints, arrayList, 0, 0, 0);
        if (!arrayList.isEmpty()) {
            return (Result[]) arrayList.toArray(new Result[arrayList.size()]);
        }
        throw NotFoundException.getNotFoundInstance();
    }

    private void doDecodeMultiple(BinaryBitmap binaryBitmap, Map<DecodeHintType, ?> map, List<Result> list, int i, int i2, int i3) {
        boolean z;
        int i4;
        int i5;
        float f;
        float f2;
        int i6;
        int i7;
        BinaryBitmap binaryBitmap2 = binaryBitmap;
        int i8 = i;
        int i9 = i2;
        int i10 = i3;
        if (i10 <= 4) {
            try {
                Result decode = this.delegate.decode(binaryBitmap2, map);
                Iterator<Result> it = list.iterator();
                while (true) {
                    if (it.hasNext()) {
                        if (it.next().getText().equals(decode.getText())) {
                            z = true;
                            break;
                        }
                    } else {
                        z = false;
                        break;
                    }
                }
                if (!z) {
                    list.add(translateResultPoints(decode, i8, i9));
                } else {
                    List<Result> list2 = list;
                }
                ResultPoint[] resultPoints = decode.getResultPoints();
                if (resultPoints != null && resultPoints.length != 0) {
                    int width = binaryBitmap.getWidth();
                    int height = binaryBitmap.getHeight();
                    float f3 = (float) width;
                    float f4 = (float) height;
                    float f5 = 0.0f;
                    float f6 = 0.0f;
                    for (ResultPoint resultPoint : resultPoints) {
                        if (resultPoint != null) {
                            float x = resultPoint.getX();
                            float y = resultPoint.getY();
                            if (x < f3) {
                                f3 = x;
                            }
                            if (y < f4) {
                                f4 = y;
                            }
                            if (x > f5) {
                                f5 = x;
                            }
                            if (y > f6) {
                                f6 = y;
                            }
                        }
                    }
                    if (f3 > 100.0f) {
                        f = f5;
                        f2 = f4;
                        i5 = height;
                        i4 = width;
                        doDecodeMultiple(binaryBitmap2.crop(0, 0, (int) f3, height), map, list, i, i2, i10 + 1);
                    } else {
                        f = f5;
                        f2 = f4;
                        i5 = height;
                        i4 = width;
                    }
                    if (f2 > 100.0f) {
                        int i11 = (int) f2;
                        i6 = i4;
                        doDecodeMultiple(binaryBitmap2.crop(0, 0, i6, i11), map, list, i, i2, i10 + 1);
                    } else {
                        i6 = i4;
                    }
                    float f7 = f;
                    if (f7 < ((float) (i6 - 100))) {
                        int i12 = (int) f7;
                        i7 = i5;
                        doDecodeMultiple(binaryBitmap2.crop(i12, 0, i6 - i12, i7), map, list, i8 + i12, i2, i10 + 1);
                    } else {
                        i7 = i5;
                    }
                    if (f6 < ((float) (i7 - 100))) {
                        int i13 = (int) f6;
                        doDecodeMultiple(binaryBitmap2.crop(0, i13, i6, i7 - i13), map, list, i, i9 + i13, i10 + 1);
                    }
                }
            } catch (ReaderException e) {
            }
        }
    }

    private static Result translateResultPoints(Result result, int xOffset, int yOffset) {
        ResultPoint[] resultPoints = result.getResultPoints();
        ResultPoint[] oldResultPoints = resultPoints;
        if (resultPoints == null) {
            return result;
        }
        ResultPoint[] newResultPoints = new ResultPoint[oldResultPoints.length];
        int i = 0;
        while (i < oldResultPoints.length) {
            ResultPoint resultPoint = oldResultPoints[i];
            ResultPoint oldPoint = resultPoint;
            if (resultPoint != null) {
                newResultPoints[i] = new ResultPoint(oldPoint.getX() + ((float) xOffset), oldPoint.getY() + ((float) yOffset));
            }
            i++;
        }
        Result newResult = new Result(result.getText(), result.getRawBytes(), result.getNumBits(), newResultPoints, result.getBarcodeFormat(), result.getTimestamp());
        int i2 = i;
        Result newResult2 = newResult;
        newResult.putAllMetadata(result.getResultMetadata());
        return newResult2;
    }
}
