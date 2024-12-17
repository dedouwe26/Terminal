namespace OxDED.Terminal.Assertion;

/// <summary>
/// The base plate of an assertion.
/// </summary>
public static class Assert {
    /// <summary>
    /// Checks if <paramref name="a"/> is <paramref name="b"/>.
    /// </summary>
    /// <typeparam name="TA">Type of <paramref name="a"/>.</typeparam>
    /// <typeparam name="TB">Type of <paramref name="b"/>.</typeparam>
    /// <param name="a">The first value.</param>
    /// <param name="b">The second value.</param>
    /// <returns>A new value assertion.</returns>
    public static ValueAssertion<TA, TB> Is<TA, TB>(TA a, TB b) {
        return ValueAssertion<TA, TB>.Create(a, b);
    }
    /// <summary>
    /// Checks if <paramref name="a"/> is <paramref name="b"/>.
    /// </summary>
    /// <param name="a">The first value.</param>
    /// <param name="b">The second value.</param>
    /// <returns>A new value assertion.</returns>
    public static ValueAssertion<object?, object?> Is(object? a, object? b) {
        return ValueAssertion<object?, object?>.Create(a, b);
    }
    /// <summary>
    /// Checks if <paramref name="boolean"/> is true.
    /// </summary>
    /// <param name="boolean">The boolean that should be true.</param>
    /// <returns>A new value assertion.</returns>
    public static ValueAssertion<bool, bool> IsTrue(bool boolean) {
        return new ValueAssertion<bool, bool>(boolean, boolean, true);
    }
    /// <summary>
    /// Checks if <paramref name="boolean"/> is false.
    /// </summary>
    /// <param name="boolean">The boolean that should be false.</param>
    /// <returns>A new value assertion.</returns>
    public static ValueAssertion<bool, bool> IsFalse(bool boolean) {
        return new ValueAssertion<bool, bool>(!boolean, boolean, false);
    }

    /// <summary>
    /// Checks if <paramref name="value"/> is null.
    /// </summary>
    /// <typeparam name="T">The type of <paramref name="value"/>.</typeparam>
    /// <param name="value">The object to check.</param>
    /// <returns>A new reference assertion.</returns>
    public static ReferenceAssertion<T> IsNull<T>(T? value) {
        return ReferenceAssertion<T>.Create(value, shouldBeNull:true);
    }
    /// <summary>
    /// Checks if <paramref name="value"/> is not null.
    /// </summary>
    /// <typeparam name="T">The type of <paramref name="value"/>.</typeparam>
    /// <param name="value">The object to check.</param>
    /// <returns>A new reference assertion.</returns>
    public static ReferenceAssertion<T> IsNotNull<T>(T? value) {
        return ReferenceAssertion<T>.Create(value, shouldBeNull:false);
    }

    /// <summary>
    /// Checks if <paramref name="value"/> is of type <typeparamref name="TMatch"/>.
    /// </summary>
    /// <typeparam name="TValue">The type of <paramref name="value"/>.</typeparam>
    /// <typeparam name="TMatch">The type <paramref name="value"/> should be.</typeparam>
    /// <param name="value">The object to check.</param>
    /// <returns>A new type assertion.</returns>
    public static TypeAssertion<TValue, TMatch> IsType<TValue, TMatch>(TValue value) {
        return TypeAssertion<TValue, TMatch>.Create(value);
    }
    /// <summary>
    /// Checks if <paramref name="obj"/> is of type <paramref name="type"/>.
    /// </summary>
    /// <typeparam name="TValue">The type of <paramref name="obj"/>.</typeparam>
    /// <param name="type">The type object should be.</param>
    /// <param name="obj">The object to check.</param>
    /// <returns>A new type assertion.</returns>
    public static TypeAssertion<TValue> IsType<TValue>(TValue obj, Type type) {
        return TypeAssertion<TValue>.Create(obj, type);
    }
    /// <summary>
    /// Checks if <paramref name="value"/> is of type <typeparamref name="TMatch"/>.
    /// </summary>
    /// <typeparam name="TMatch">The type <paramref name="value"/> should be.</typeparam>
    /// <param name="value">The object to check.</param>
    /// <returns>A new type assertion.</returns>
    public static TypeAssertion<object?, TMatch> IsType<TMatch>(object? value) {
        return TypeAssertion<object?, TMatch>.Create(value);
    }
    /// <summary>
    /// Checks if <paramref name="value"/> is of type <paramref name="type"/>.
    /// </summary>
    /// <param name="value">The object to check.</param>
    /// <param name="type">The type object should be.</param>
    /// <returns>A new type assertion.</returns>
    public static TypeAssertion<object?> IsType(object? value, Type type) {
        return TypeAssertion<object?>.Create(value, type);
    }
    
    /// <summary>
    /// Checks if <paramref name="emitter"/> throws <typeparamref name="TException"/>.
    /// </summary>
    /// <typeparam name="T">The return value of <paramref name="emitter"/>.</typeparam>
    /// <typeparam name="TException">The exception to catch.</typeparam>
    /// <param name="emitter">The emitter to check.</param>
    /// <returns>A new exception assertion.</returns>
    public static ExceptionAssertion<T, TException> Throws<T, TException>(Func<T> emitter) where TException : Exception {
        return ExceptionAssertion<T, TException>.Create(emitter);
    }
    /// <summary>
    /// Checks if <paramref name="emitter"/> throws <typeparamref name="TException"/>.
    /// </summary>
    /// <typeparam name="TException">The exception to catch.</typeparam>
    /// <param name="emitter">The emitter to check.</param>
    /// <returns>A new exception assertion.</returns>
    public static ExceptionAssertion<TException> Throws<TException>(Action emitter) where TException : Exception {
        return ExceptionAssertion<TException>.Create(emitter);
    }
    /// <summary>
    /// Checks if <paramref name="emitter"/> throws an exception.
    /// </summary>
    /// <typeparam name="T">The return value of <paramref name="emitter"/>.</typeparam>
    /// <param name="emitter">The emitter to check.</param>
    /// <returns>A new exception assertion.</returns>
    public static ExceptionAssertion<T, Exception> Throws<T>(Func<T> emitter) {
        return ExceptionAssertion<T, Exception>.Create(emitter);
    }
    /// <summary>
    /// Checks if <paramref name="emitter"/> throws an exception.
    /// </summary>
    /// <param name="emitter">The emitter to check.</param>
    /// <returns>A new exception assertion.</returns>
    public static ExceptionAssertion<Exception> Throws(Action emitter) {
        return ExceptionAssertion<Exception>.Create(emitter);
    }
}