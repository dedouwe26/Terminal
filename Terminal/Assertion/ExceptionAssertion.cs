using OxDED.Terminal.Logging;

namespace OxDED.Terminal.Assertion;

/// <summary>
/// Represents an assertion that catches exceptions.
/// </summary>
/// <typeparam name="TException">The type of the exception to catch.</typeparam>
public class ExceptionAssertion<TException> : Assertion where TException : Exception {
    /// <summary>
    /// Creates a new exception assertion.
    /// </summary>
    /// <param name="emitter">The possible emitter of an exception.</param>
    /// <returns>The created exception assertion.</returns>
    public static ExceptionAssertion<TException> Create(Action emitter) {
        try {
            emitter.Invoke();
        } catch (TException e) {
            return new ExceptionAssertion<TException>(false, e);
        }
        return new ExceptionAssertion<TException>(true);
    }
    
    /// <summary>
    /// The exception thrown.
    /// </summary>
    public readonly TException? exception;
    
    /// <summary>
    /// Creates a new exception assertion.
    /// </summary>
    /// <param name="success">If this assertion has succeeded.</param>
    /// <param name="exception">The optional exception that occured.</param>
    public ExceptionAssertion(bool success, TException? exception = null) : base(success ? AssertionResult.Success : AssertionResult.ExceptionCaught) {
        this.exception = exception;
    }

    /// <summary>
    /// Gets the exception of this exception assertion.
    /// </summary>
    /// <returns>The exception of this exception assertion.</returns>
    public TException? GetException() {
        return exception;
    }

    /// <summary>
    /// Creates an asserter of the exception.
    /// </summary>
    /// <returns>The exception asserter.</returns>
    public Asserter<TException?> ExceptionAsserter() {
        return new Asserter<TException?>(exception);
    }

    /// <inheritdoc/>
    public override string ToString() {
        return $"{GetType().Name}: result = {result}, exception = {exception}";
    }
}

/// <summary>
/// Represents an assertion that catches exceptions.
/// </summary>
/// <typeparam name="T">The return type of the emitter.</typeparam>
/// <typeparam name="TException">The type of the exception to catch.</typeparam>
public class ExceptionAssertion<T, TException> : Assertion<T?> where TException : Exception {
    /// <summary>
    /// Creates a new exception assertion.
    /// </summary>
    /// <param name="emitter">The possible emitter of an exception.</param>
    /// <returns>The created exception assertion.</returns>
    public static ExceptionAssertion<T, TException> Create(Func<T> emitter) {
        T value;
        try {
            value = emitter.Invoke();
        } catch (TException e) {
            return new ExceptionAssertion<T, TException>(false, exception:e);
        }
        return new ExceptionAssertion<T, TException>(true, value:value);
    }
    
    /// <summary>
    /// The exception thrown.
    /// </summary>
    public readonly TException? exception;

    /// <summary>
    /// Creates a new exception assertion.
    /// </summary>
    /// <param name="success">If this assertion succeeded.</param>
    /// <param name="value">The optional return value of the emitter.</param>
    /// <param name="exception">The optional exception that occured.</param>
    public ExceptionAssertion(bool success, T? value = default, TException? exception = null) : base(success ? AssertionResult.Success : AssertionResult.ExceptionCaught, value) {
        this.exception = exception;
    }

    /// <summary>
    /// Gets the exception of this exception assertion.
    /// </summary>
    /// <returns>The exception of this exception assertion.</returns>
    public TException? GetException() {
        return exception;
    }

    /// <summary>
    /// Creates an asserter of the exception.
    /// </summary>
    /// <returns>The exception asserter.</returns>
    public Asserter<TException?> ExceptionAsserter() {
        return new Asserter<TException?>(exception);
    }

    /// <inheritdoc/>
    public override string ToString() {
        return $"{GetType().Name}: result = {result}, value = {value}, exception = {exception}";
    }
}