namespace OxDED.Terminal.Assertion;

public class TypeAssertion<T> : Assertion<T> {
    public static TypeAssertion<T> Create(T obj, Type type) {
        return new TypeAssertion<T>(obj?.GetType() == type, type, obj);
    }
    public readonly Type matchedType;
    public TypeAssertion(bool success, Type matchedType, T value) : base(success ? AssertionResult.Success : AssertionResult.UnexpectedType, value) {
        
        this.matchedType = matchedType;
    }

    public Type GetMatchedType() {
        return matchedType;
    }

    public Asserter<Type> TypeAsserter() {
        return new Asserter<Type>(matchedType);
    }

    public override string ToString() {
        return $"{GetType().Name}: result = {result}, value = {value}, matchedType = {matchedType}";
    }
}

public class TypeAssertion<TValue, TMatch> : TypeAssertion<TValue> {
    public static TypeAssertion<TValue, TMatch> Create(TValue obj) {
        return new TypeAssertion<TValue, TMatch>(obj is TMatch, obj);
    }
    public TypeAssertion(bool success, TValue value) : base(success, typeof(TMatch), value) { }
}