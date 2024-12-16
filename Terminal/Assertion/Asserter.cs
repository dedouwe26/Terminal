using OxDED.Terminal.Logging;

namespace OxDED.Terminal.Assertion;

public class Asserter<T> {
    public readonly T value;
    public Asserter(T value) {
        this.value = value;
    }

    public T? GetValue() {
        return value;
    }
    
    public ValueAssertion<T, object?> Is(object? b) {
        return Assert.Is(value, b);
    }
    public ValueAssertion<T, TMatch?> Is<TMatch>(TMatch? b) {
        return Assert.Is(value, b);
    }
    
    public ReferenceAssertion<T> IsNull() {
        return Assert.IsNull(value);
    }
    public ReferenceAssertion<T> IsNotNull() {
        return Assert.IsNotNull(value);
    }
    
    public TypeAssertion<T, TMatch> IsType<TMatch>() {
        return Assert.IsType<T, TMatch>(value);
    }
    public TypeAssertion<T> IsType(Type type) {
        return Assert.IsType(value, type);
    }

    public Asserter<TNew?> As<TNew>() {
        try {
            return new Asserter<TNew?>((TNew?)(object?)value);
        } catch (InvalidCastException) {
            return new Asserter<TNew?>(default);
        }
    }

    public Asserter<TOut> Do<TOut>(Func<T, TOut> function) {
        return new Asserter<TOut>(function(value));
    }

    public Asserter<T> Log(Logger logger, Severity severity = Severity.Debug) {
        logger.Log(severity, value);
        return this;
    }
}

public static class AsserterExtension {
    public static ValueAssertion<bool, bool> IsTrue(this Asserter<bool> asserter) {
        return Assert.IsTrue(asserter.value);
    }
    public static ValueAssertion<bool, bool> IsFalse(this Asserter<bool> asserter) {
        return Assert.IsFalse(asserter.value);
    }

    public static ExceptionAssertion<T, TException> Throws<T, TException>(this Asserter<Func<T>> asserter) where TException : Exception {
        return Assert.Throws<T, TException>(asserter.value);
    }
    public static ExceptionAssertion<TException> Throws<TException>(this Asserter<Action> asserter) where TException : Exception {
        return Assert.Throws<TException>(asserter.value);
    }
    public static ExceptionAssertion<T, Exception> Throws<T>(this Asserter<Func<T>> asserter) {
        return Assert.Throws(asserter.value);
    }
    public static ExceptionAssertion<Exception> Throws(this Asserter<Action> asserter) {
        return Assert.Throws(asserter.value);
        
    }
}