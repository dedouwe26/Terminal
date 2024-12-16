using OxDED.Terminal.Logging;

namespace OxDED.Terminal.Assertion;

public class ExceptionAssertion<TException> : Assertion where TException : Exception {
    public static ExceptionAssertion<TException> Create(Action emitter) {
        try {
            emitter.Invoke();
        } catch (TException e) {
            return new ExceptionAssertion<TException>(false, e);
        }
        return new ExceptionAssertion<TException>(true);
    }
    
    public readonly TException? exception;
    public ExceptionAssertion(bool success, TException? exception = null) : base(success ? AssertionResult.Success : AssertionResult.ExceptionCaught) {
        this.exception = exception;
    }
    public TException? GetException() {
        return exception;
    }

    public Asserter<TException?> ExceptionAsserter() {
        return new Asserter<TException?>(exception);
    }

    public override string ToString() {
        return $"{GetType().Name}: result = {result}, exception = {exception}";
    }
}

public class ExceptionAssertion<T, TException> : Assertion<T?> where TException : Exception {
    public static ExceptionAssertion<T, TException> Create(Func<T> emitter) {
        T value;
        try {
            value = emitter.Invoke();
        } catch (TException e) {
            return new ExceptionAssertion<T, TException>(false, exception:e);
        }
        return new ExceptionAssertion<T, TException>(true, value:value);
    }
    
    public readonly TException? exception;
    public ExceptionAssertion(bool success, T? value = default, TException? exception = null) : base(success ? AssertionResult.Success : AssertionResult.ExceptionCaught, value) {
        this.exception = exception;
    }
    public TException? GetException() {
        return exception;
    }

    public Asserter<Exception?> ExceptionAsserter() {
        return new Asserter<Exception?>(exception);
    }

    public override string ToString() {
        return $"{GetType().Name}: result = {result}, value = {value}, exception = {exception}";
    }
}