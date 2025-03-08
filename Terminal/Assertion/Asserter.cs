using OxDED.Terminal.Logging;

namespace OxDED.Terminal.Assertion;

/// <summary>
/// Can assert a value that is contained in this class.
/// </summary>
/// <typeparam name="T">The type of the value to assert.</typeparam>
public class Asserter<T> {
    /// <summary>
    /// The value to assert.
    /// </summary>
    public readonly T value;
    /// <summary>
    /// Creates a new asserter.
    /// </summary>
    /// <param name="value">The value to assert.</param>
    public Asserter(T value) {
        this.value = value;
    }

    /// <summary>
    /// Gets the value to assert.
    /// </summary>
    /// <returns>The value to assert.</returns>
    public T? GetValue() {
        return value;
    }
    
    /// <summary>
    /// Checks if <see cref="value"/> is <paramref name="b"/>
    /// </summary>
    /// <param name="b">The second value.</param>
    /// <returns>A new value assertion.</returns>
    public ValueAssertion<T, object?> Is(object? b) {
        return Assert.Is(value, b);
    }
    /// <summary>
    /// Checks if <see cref="value"/> is <paramref name="b"/>
    /// </summary>
    /// <typeparam name="TMatch">Type of <paramref name="b"/>.</typeparam>
    /// <param name="b">The second value.</param>
    /// <returns>A new value assertion.</returns>
    public ValueAssertion<T, TMatch> Is<TMatch>(TMatch b) {
        return Assert.Is(value, b);
    }
    
    /// <summary>
    /// Checks if <see cref="value"/> is null.
    /// </summary>
    /// <returns>A new reference assertion.</returns>
    public ReferenceAssertion<T> IsNull() {
        return Assert.IsNull(value);
    }
    /// <summary>
    /// Checks if <see cref="value"/> is not null.
    /// </summary>
    /// <returns>A new reference assertion.</returns>
    public ReferenceAssertion<T> IsNotNull() {
        return Assert.IsNotNull(value);
    }
    
    /// <summary>
    /// Checks if <see cref="value"/> is of type <typeparamref name="TMatch"/>.
    /// </summary>
    /// <typeparam name="TMatch">The type <see cref="value"/> should be.</typeparam>
    /// <returns>A new type assertion.</returns>
    public TypeAssertion<T, TMatch> IsType<TMatch>() {
        return Assert.IsType<T, TMatch>(value);
    }
    /// <summary>
    /// Checks if <see cref="value"/> is of type <paramref name="type"/>.
    /// </summary>
    /// <param name="type">The type object should be.</param>
    /// <returns>A new type assertion.</returns>
    public TypeAssertion<T> IsType(Type type) {
        return Assert.IsType(value, type);
    }

    /// <summary>
    /// Tries to cast <see cref="value"/> to a new type.
    /// </summary>
    /// <typeparam name="TNew">The new type to try to cast to.</typeparam>
    /// <returns>A new asserter where the value is null if the cast failed (<see cref="InvalidCastException"/>).</returns>
    public Asserter<TNew?> As<TNew>() {
        try {
            return new Asserter<TNew?>((TNew?)(object?)value);
        } catch (InvalidCastException) {
            return new Asserter<TNew?>(default);
        }
    }

    /// <summary>
    /// Applies a function on <see cref="value"/>.
    /// </summary>
    /// <typeparam name="TOut">The new type of the value.</typeparam>
    /// <param name="function">The function to apply.</param>
    /// <returns>The new asserter where the function is applied on the <see cref="value"/>.</returns>
    public Asserter<TOut> Do<TOut>(Func<T, TOut> function) {
        return new Asserter<TOut>(function(value));
    }

    /// <summary>
    /// Applies a function on <see cref="value"/>.
    /// </summary>
    /// <param name="function">The function to apply.</param>
    /// <returns>The same asserter.</returns>
    public Asserter<T> Do(Action<T> function) {
        function(value);
        return this;
    }

    /// <summary>
    /// Logs the <see cref="value"/>.
    /// </summary>
    /// <param name="logger">The logger to log to.</param>
    /// <param name="severity">The severity to log.</param>
    /// <returns>The same asserter.</returns>
    public Asserter<T> Log(Logger logger, Severity severity = Severity.Debug) {
        logger.Log(severity, value);
        return this;
    }
}

/// <summary>
/// Asserter exstensions for assertions with specific types.
/// </summary>
public static class AsserterExtension {
    /// <summary>
    /// Checks if <see cref="Asserter{T}.value"/> is true.
    /// </summary>
    /// <returns>A new value assertion.</returns>
    public static ValueAssertion<bool, bool> IsTrue(this Asserter<bool> asserter) {
        return Assert.IsTrue(asserter.value);
    }
    /// <summary>
    /// Checks if <see cref="Asserter{T}.value"/> is true.
    /// </summary>
    /// <returns>A new value assertion.</returns>
    public static ValueAssertion<bool, bool> IsFalse(this Asserter<bool> asserter) {
        return Assert.IsFalse(asserter.value);
    }

    /// <summary>
    /// Checks if <see cref="Asserter{T}.value"/> throws <typeparamref name="TException"/>.
    /// </summary>
    /// <typeparam name="T">The return value of <see cref="Asserter{T}.value"/>.</typeparam>
    /// <typeparam name="TException">The exception to catch.</typeparam>
    /// <returns>A new exception assertion.</returns>
    public static ExceptionAssertion<T, TException> Throws<T, TException>(this Asserter<Func<T>> asserter) where TException : Exception {
        return Assert.Throws<T, TException>(asserter.value);
    }
    /// <summary>
    /// Checks if <see cref="Asserter{T}.value"/> throws <typeparamref name="TException"/>.
    /// </summary>
    /// <typeparam name="TException">The exception to catch.</typeparam>
    /// <returns>A new exception assertion.</returns>
    public static ExceptionAssertion<TException> Throws<TException>(this Asserter<Action> asserter) where TException : Exception {
        return Assert.Throws<TException>(asserter.value);
    }
    /// <summary>
    /// Checks if <see cref="Asserter{T}.value"/> throws an exception.
    /// </summary>
    /// <typeparam name="T">The return value of <see cref="Asserter{T}.value"/>.</typeparam>
    /// <returns>A new exception assertion.</returns>
    public static ExceptionAssertion<T, Exception> Throws<T>(this Asserter<Func<T>> asserter) {
        return Assert.Throws(asserter.value);
    }
    /// <summary>
    /// Checks if <see cref="Asserter{T}.value"/> throws an exception.
    /// </summary>
    /// <returns>A new exception assertion.</returns>
    public static ExceptionAssertion<Exception> Throws(this Asserter<Action> asserter) {
        return Assert.Throws(asserter.value);
    }
}