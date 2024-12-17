namespace OxDED.Terminal.Assertion;

/// <summary>
/// Represents an assertion that checks the types.
/// </summary>
/// <typeparam name="T">The type of value to check.</typeparam>
public class TypeAssertion<T> : Assertion<T> {
    /// <summary>
    /// Creates a new type assertion.
    /// </summary>
    /// <param name="obj">The object to check.</param>
    /// <param name="type">The type <paramref name="obj"/> should be.</param>
    /// <returns>The created type assertion.</returns>
    public static TypeAssertion<T> Create(T obj, Type type) {
        return new TypeAssertion<T>(obj?.GetType() == type, type, obj);
    }

    /// <summary>
    /// The type that <see cref="Assertion{T}.value"/> should have been.
    /// </summary><see cref="Assertion{T}.value"/>
    public readonly Type matchedType;

    /// <summary>
    /// Creates a new type assertion.
    /// </summary>
    /// <param name="success">If this assertion has succeeded.</param>
    /// <param name="matchedType">The type that <paramref name="value"/> should have been.</param>
    /// <param name="value">The value that should have been <paramref name="matchedType"/>.</param>
    public TypeAssertion(bool success, Type matchedType, T value) : base(success ? AssertionResult.Success : AssertionResult.UnexpectedType, value) {
        
        this.matchedType = matchedType;
    }

    /// <summary>
    /// Gets the type that the object should have been.
    /// </summary>
    /// <returns>The type that the object should have been.</returns>
    public Type GetMatchedType() {
        return matchedType;
    }

    /// <summary>
    /// Creates an asserter of the type that <see cref="Assertion{T}.value"/> should have been.
    /// </summary>
    /// <returns>The type asserter.</returns>
    public Asserter<Type> TypeAsserter() {
        return new Asserter<Type>(matchedType);
    }

    /// <inheritdoc/>
    public override string ToString() {
        return $"{GetType().Name}: result = {result}, value = {value}, matchedType = {matchedType}";
    }
}

/// <summary>
/// Represents an assertion that checks the types.
/// </summary>
/// <typeparam name="TValue">The type of value to check.</typeparam>
/// <typeparam name="TMatch">The type <see cref="Assertion{T}.value"/> should have been.</typeparam>
public class TypeAssertion<TValue, TMatch> : TypeAssertion<TValue> {
    /// <summary>
    /// Creates a new type assertion.
    /// </summary>
    /// <param name="obj">The object to check.</param>
    /// <returns>The created type assertion.</returns>
    public static TypeAssertion<TValue, TMatch> Create(TValue obj) {
        return new TypeAssertion<TValue, TMatch>(obj is TMatch, obj);
    }

    /// <summary>
    /// Creates a new type assertion.
    /// </summary>
    /// <param name="success">If this assertion has succeeded.</param>
    /// <param name="value">The value that should have been <typeparamref name="TMatch"/>.</param>
    public TypeAssertion(bool success, TValue value) : base(success, typeof(TMatch), value) { }
}