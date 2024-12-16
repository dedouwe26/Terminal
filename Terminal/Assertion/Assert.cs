namespace OxDED.Terminal.Assertion;

public static class Assert {
    public static ValueAssertion<TA, TB> Is<TA, TB>(TA a, TB b) {
        return ValueAssertion<TA, TB>.Create(a, b);
    }
    public static ValueAssertion<object?, object?> Is(object? a, object? b) {
        return ValueAssertion<object?, object?>.Create(a, b);
    }
    public static ValueAssertion<bool, bool> IsTrue(bool boolean) {
        return new ValueAssertion<bool, bool>(boolean, boolean, true);
    }
    public static ValueAssertion<bool, bool> IsFalse(bool boolean) {
        return new ValueAssertion<bool, bool>(!boolean, boolean, false);
    }

    public static ReferenceAssertion<T> IsNull<T>(T? obj) {
        return ReferenceAssertion<T>.Create(obj, shouldBeNull:true);
    }
    public static ReferenceAssertion<T> IsNotNull<T>(T? obj) {
        return ReferenceAssertion<T>.Create(obj, shouldBeNull:false);
    }

    public static TypeAssertion<TValue, TMatch> IsType<TValue, TMatch>(TValue obj) {
        return TypeAssertion<TValue, TMatch>.Create(obj);
    }
    public static TypeAssertion<TValue> IsType<TValue>(TValue obj, Type type) {
        return TypeAssertion<TValue>.Create(obj, type);
    }
    public static TypeAssertion<object?, TMatch> IsType<TMatch>(object? obj) {
        return TypeAssertion<object?, TMatch>.Create(obj);
    }
    public static TypeAssertion<object?> IsType(object? obj, Type type) {
        return TypeAssertion<object?>.Create(obj, type);
    }
    
    public static ExceptionAssertion<T, TException> Throws<T, TException>(Func<T> emitter) where TException : Exception {
        return ExceptionAssertion<T, TException>.Create(emitter);
    }
    public static ExceptionAssertion<TException> Throws<TException>(Action emitter) where TException : Exception {
        return ExceptionAssertion<TException>.Create(emitter);
    }
    public static ExceptionAssertion<T, Exception> Throws<T>(Func<T> emitter) {
        return ExceptionAssertion<T, Exception>.Create(emitter);
    }
    public static ExceptionAssertion<Exception> Throws(Action emitter) {
        return ExceptionAssertion<Exception>.Create(emitter);
    }
}