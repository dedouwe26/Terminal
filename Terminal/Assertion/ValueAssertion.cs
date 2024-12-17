using System.Runtime.CompilerServices;

namespace OxDED.Terminal.Assertion;

/// <summary>
/// Represents an assertion that checks two values.
/// </summary>
/// <typeparam name="T">The type of the first value.</typeparam>
/// <typeparam name="TSecond">The type of the second value.</typeparam>
public class ValueAssertion<T, TSecond> : Assertion<T> {
    private static bool P_Is(object? a, object? b) {
        if (a is null && b is null) return true;
        if (a is null ^ b is null) return false;

        if (a?.GetType() != b?.GetType()) return false;
        if (ReferenceEquals(a, b)) return true;

        // NOTE: A should not be possibly null here... Right?
        return a?.Equals(b) ?? (b is null);
    }
    /// <summary>
    /// Creates a new value assertion.
    /// </summary>
    /// <param name="a">The value to check.</param>
    /// <param name="b">The value <paramref name="a"/> should have been.</param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ValueAssertion<T, TSecond> Create(T a, TSecond b) {
        return new ValueAssertion<T, TSecond>(P_Is(a, b), a, b);
    }

    /// <summary>
    /// The value <see cref="Assertion{T}.value"/> should have been.
    /// </summary>
    public readonly TSecond secondValue;

    /// <summary>
    /// Creates a new value assertion.
    /// </summary>
    /// <param name="success">If this assertion succeeded.</param>
    /// <param name="value">The checked value.</param>
    /// <param name="secondValue">The value <paramref name="value"/> should have been.</param>
    public ValueAssertion(bool success, T value, TSecond secondValue) : base(success ? AssertionResult.Success : AssertionResult.UnexpectedValue, value) {
        this.secondValue = secondValue;
    }

    /// <summary>
    /// Gets the value <see cref="Assertion{T}.value"/> should have been.
    /// </summary>
    /// <returns>The value <see cref="Assertion{T}.value"/> should have been.</returns>
    public TSecond? GetSecondValue() {
        return secondValue;
    }

    /// <summary>
    /// Creates an asserter of the value <see cref="Assertion{T}.value"/> should have been.
    /// </summary>
    /// <returns>The asserter of <see cref="secondValue"/>.</returns>
    public Asserter<TSecond> SecondAsserter() {
        return new Asserter<TSecond>(secondValue);
    }

    /// <inheritdoc/>
    public override string ToString() {
        return $"{GetType().Name}: result = {result}, value = {value}, secondValue = {secondValue}";
    }
}