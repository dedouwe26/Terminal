using OxDED.Terminal.Logging;

namespace OxDED.Terminal.Assertion;

/// <summary>
/// Represents a base assertion with a result.
/// </summary>
public class Assertion {
    /// <summary>
    /// The result of this assertion.
    /// </summary>
    public readonly AssertionResult result;
    /// <summary>
    /// Creates a new assertion.
    /// </summary>
    /// <param name="result">The result of this assertion.</param>
    public Assertion(AssertionResult result) {
        this.result = result;
    }
    /// <summary>
    /// Gets the result of this assertion.
    /// </summary>
    /// <returns>The result of this assertion.</returns>
    public AssertionResult GetResult() {
        return result;
    }
    /// <summary>
    /// Creates an asserter of the result.
    /// </summary>
    /// <returns>The result asserter.</returns>
    public Asserter<AssertionResult> ResultAsserter() {
        return new Asserter<AssertionResult>(result);
    }
    /// <summary>
    /// Checks if this assertion succeeded.
    /// </summary>
    /// <returns>True if the result equals to <see cref="AssertionResult.Success"/>.</returns>
    public bool IsSuccess() {
        return result == AssertionResult.Success;
    }
    /// <summary>
    /// Checks if this assertion failed.
    /// </summary>
    /// <returns>True if the result does not equal to <see cref="AssertionResult.Success"/>.</returns>
    public bool IsFailure() {
        return result != AssertionResult.Success;
    }

    /// <summary>
    /// Calls <paramref name="callback"/> if this assertion succeeded.
    /// </summary>
    /// <param name="callback">The callback to call, if this assertion succeeded.</param>
    /// <returns>This assertion.</returns>
    public Assertion OnSuccess(Action<Assertion> callback) {
        if (IsSuccess()) callback.Invoke(this);
        return this;
    }
    /// <summary>
    /// Calls <paramref name="callback"/> if this assertion failed.
    /// </summary>
    /// <param name="callback">The callback to call, if this assertion failed.</param>
    /// <returns>This assertion.</returns>
    public Assertion OnFailure(Action<Assertion> callback) {
        if (IsFailure()) callback.Invoke(this);
        return this;
    }

    /// <summary>
    /// Logs this assertion.
    /// </summary>
    /// <param name="logger">The logger to log to.</param>
    /// <param name="severity">The severity to log with.</param>
    /// <returns>This assertion.</returns>
    public Assertion Log(Logger logger, Severity severity = Severity.Error) {
        logger.LogTrace($"Assertion done: {ToString()}");
        return OnFailure(failedAssertion => {
            logger.Log(severity, $"Assertion failed with result: {result}");
        });
    }

    /// <summary>
    /// Throws <paramref name="exception"/> or <see cref="AssertException{T}"/> upon failing.
    /// </summary>
    /// <param name="exception">The exception to throw (defaults to <see cref="AssertException{T}"/>).</param>
    /// <returns>This assertion.</returns>
    public Assertion Throw(Exception? exception = null) {
        return OnFailure(failedAssertion => {
            throw exception ?? new AssertException<Assertion>("Assertion failed", this);
        });
    }

    /// <summary>
    /// Converts back this assertion to another assertion.
    /// </summary>
    /// <typeparam name="TNew">The new assertion type.</typeparam>
    /// <returns>The new assertion if the cast was successful.</returns>
    public TNew? As<TNew>() where TNew : Assertion {
        return this as TNew;
    }

    /// <summary>
    /// Converts this assertion to a string.
    /// </summary>
    /// <returns>The string the assertion converted to.</returns>
    public override string ToString() {
        return $"{GetType().Name}: result = {result}";
    }
}

/// <summary>
/// Represents a base assertion with a result and a value.
/// </summary>
/// <typeparam name="T">The type of the value.</typeparam>
public class Assertion<T> : Assertion {
    /// <summary>
    /// The value of this assertion.
    /// </summary>
    public readonly T value;
    /// <summary>
    /// Creates a new assertion.
    /// </summary>
    /// <param name="result">The result of the assertion.</param>
    /// <param name="value">The value of the assertion.</param>
    public Assertion(AssertionResult result, T value) : base(result) {
        this.value = value;
    }

    /// <summary>
    /// Gets the value of the assertion.
    /// </summary>
    /// <returns>The value of this assertion.</returns>
    public T GetValue() {
        return value;
    }

    /// <summary>
    /// Creates an asserter of the <see cref="value"/>.
    /// </summary>
    /// <returns>The <see cref="value"/> asserter.</returns>
    public Asserter<T> Asserter() {
        return new Asserter<T>(value);
    }

    /// <inheritdoc/>
    public override string ToString() {
        return $"{GetType().Name}: result = {result}, value = {value}";
    }
}