namespace OxDED.Terminal.Assertion;

/// <summary>
/// Represents an assertion that checks null references.
/// </summary>
/// <typeparam name="T">The type of the possible null reference.</typeparam>
public class ReferenceAssertion<T> : Assertion<T?> {
    /// <summary>
    /// Creates a new reference assertion.
    /// </summary>
    /// <param name="value">The object that can be null.</param>
    /// <param name="shouldBeNull">If the <paramref name="value"/> should be null.</param>
    /// <returns>The created reference assertion.</returns>
    public static ReferenceAssertion<T> Create(T? value, bool shouldBeNull = true) {
        return new ReferenceAssertion<T>(shouldBeNull ? (value is null) : (value is not null), value, shouldBeNull);
    }

    /// <summary>
    /// If <see cref="Assertion{T}.value"/> should be null.
    /// </summary>
    public readonly bool shouldBeNull;

    /// <summary>
    /// Creates a new reference assertion.
    /// </summary>
    /// <param name="success">If this assertion has succeeded.</param>
    /// <param name="value">The nullable object that has been checked.</param>
    /// <param name="shouldBeNull">If <paramref name="value"/> should be null.</param>
    public ReferenceAssertion(bool success, T? value, bool shouldBeNull) : base(success ? AssertionResult.Success : AssertionResult.UnexpectedReference, value) {
        this.shouldBeNull = shouldBeNull;
    }

    /// <summary>
    /// Checks if <see cref="Assertion{T}.value"/> should be null.
    /// </summary>
    /// <returns>True, if <see cref="Assertion{T}.value"/> should be null.</returns>
    public bool ShouldBeNull() {
        return shouldBeNull;
    }

    /// <summary>
    /// Creates an asserter of <see cref="shouldBeNull"/>.
    /// </summary>
    /// <returns>The <see cref="shouldBeNull"/> asserter.</returns>
    public Asserter<bool> ShouldBeNullAsserter() {
        return new Asserter<bool>(shouldBeNull);
    }
}