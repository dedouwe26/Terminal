namespace OxDED.Terminal.Assertion;

public class ReferenceAssertion<T> : Assertion<T?> {
    public static ReferenceAssertion<T> Create(T? obj, bool shouldBeNull = true) {
        return new ReferenceAssertion<T>(shouldBeNull ? (obj is null) : (obj is not null), obj);
    }
    public ReferenceAssertion(bool success, T? value = default) : base(success ? AssertionResult.Success : AssertionResult.UnexpectedReference, value) { }
}