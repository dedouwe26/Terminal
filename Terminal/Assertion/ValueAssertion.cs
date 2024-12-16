using System.Runtime.CompilerServices;

namespace OxDED.Terminal.Assertion;

public class ValueAssertion<T, TSecond> : Assertion<T> {
    private static bool P_Is(object? a, object? b) {
        if (a is null && b is null) return true;
        if (a is null ^ b is null) return false;

        if (a?.GetType() != b?.GetType()) return false;
        if (ReferenceEquals(a, b)) return true;

        // NOTE: A should not be possibly null here... Right?
        return a?.Equals(b) ?? (b is null);
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ValueAssertion<T, TSecond> Create(T a, TSecond b) {
        return new ValueAssertion<T, TSecond>(P_Is(a, b), a, b);
    }
    public readonly TSecond secondValue;
    public ValueAssertion(bool success, T value, TSecond secondValue) : base(success ? AssertionResult.Success : AssertionResult.UnexpectedValue, value) {
        this.secondValue = secondValue;
    }

    public TSecond? GetSecondValue() {
        return secondValue;
    }

    public Asserter<TSecond> SecondAsserter() {
        return new Asserter<TSecond>(secondValue);
    }

    public override string ToString() {
        return $"{GetType().Name}: result = {result}, value = {value}, secondValue = {secondValue}";
    }
}