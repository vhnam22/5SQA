package org.threeten.bp.jdk8;

public final class Jdk8Methods {
    private Jdk8Methods() {
    }

    public static <T> T requireNonNull(T value) {
        if (value != null) {
            return value;
        }
        throw new NullPointerException("Value must not be null");
    }

    public static <T> T requireNonNull(T value, String parameterName) {
        if (value != null) {
            return value;
        }
        throw new NullPointerException(parameterName + " must not be null");
    }

    public static boolean equals(Object a, Object b) {
        if (a == null) {
            if (b == null) {
                return true;
            }
            return false;
        } else if (b == null) {
            return false;
        } else {
            return a.equals(b);
        }
    }

    public static int compareInts(int a, int b) {
        if (a < b) {
            return -1;
        }
        if (a > b) {
            return 1;
        }
        return 0;
    }

    public static int compareLongs(long a, long b) {
        if (a < b) {
            return -1;
        }
        if (a > b) {
            return 1;
        }
        return 0;
    }

    public static int safeAdd(int a, int b) {
        int sum = a + b;
        if ((a ^ sum) >= 0 || (a ^ b) < 0) {
            return sum;
        }
        throw new ArithmeticException("Addition overflows an int: " + a + " + " + b);
    }

    public static long safeAdd(long a, long b) {
        long sum = a + b;
        if ((a ^ sum) >= 0 || (a ^ b) < 0) {
            return sum;
        }
        throw new ArithmeticException("Addition overflows a long: " + a + " + " + b);
    }

    public static int safeSubtract(int a, int b) {
        int result = a - b;
        if ((a ^ result) >= 0 || (a ^ b) >= 0) {
            return result;
        }
        throw new ArithmeticException("Subtraction overflows an int: " + a + " - " + b);
    }

    public static long safeSubtract(long a, long b) {
        long result = a - b;
        if ((a ^ result) >= 0 || (a ^ b) >= 0) {
            return result;
        }
        throw new ArithmeticException("Subtraction overflows a long: " + a + " - " + b);
    }

    public static int safeMultiply(int a, int b) {
        long total = ((long) a) * ((long) b);
        if (total >= -2147483648L && total <= 2147483647L) {
            return (int) total;
        }
        throw new ArithmeticException("Multiplication overflows an int: " + a + " * " + b);
    }

    public static long safeMultiply(long a, int b) {
        switch (b) {
            case -1:
                if (a != Long.MIN_VALUE) {
                    return -a;
                }
                throw new ArithmeticException("Multiplication overflows a long: " + a + " * " + b);
            case 0:
                return 0;
            case 1:
                return a;
            default:
                long total = ((long) b) * a;
                if (total / ((long) b) == a) {
                    return total;
                }
                throw new ArithmeticException("Multiplication overflows a long: " + a + " * " + b);
        }
    }

    public static long safeMultiply(long a, long b) {
        if (b == 1) {
            return a;
        }
        if (a == 1) {
            return b;
        }
        if (a == 0 || b == 0) {
            return 0;
        }
        long total = a * b;
        if (total / b == a && ((a != Long.MIN_VALUE || b != -1) && (b != Long.MIN_VALUE || a != -1))) {
            return total;
        }
        throw new ArithmeticException("Multiplication overflows a long: " + a + " * " + b);
    }

    public static int safeToInt(long value) {
        if (value <= 2147483647L && value >= -2147483648L) {
            return (int) value;
        }
        throw new ArithmeticException("Calculation overflows an int: " + value);
    }

    public static long floorDiv(long a, long b) {
        return a >= 0 ? a / b : ((a + 1) / b) - 1;
    }

    public static long floorMod(long a, long b) {
        return ((a % b) + b) % b;
    }

    public static int floorMod(long a, int b) {
        return (int) (((a % ((long) b)) + ((long) b)) % ((long) b));
    }

    public static int floorDiv(int a, int b) {
        return a >= 0 ? a / b : ((a + 1) / b) - 1;
    }

    public static int floorMod(int a, int b) {
        return ((a % b) + b) % b;
    }
}
