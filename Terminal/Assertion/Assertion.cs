using OxDED.Terminal.Logging;

namespace OxDED.Terminal.Assertion;

public class Assertion {
    public readonly AssertionResult result;
    public Assertion(AssertionResult result) {
        this.result = result;
    }
    public AssertionResult GetResult() {
        return result;
    }
    public Asserter<AssertionResult> ResultAsserter() {
        return new Asserter<AssertionResult>(result);
    }
    public bool IsSuccess() {
        return result == AssertionResult.Success;
    }
    public bool IsFailure() {
        return result != AssertionResult.Success;
    }

    public Assertion OnSuccess(Action<Assertion> callback) {
        if (IsSuccess()) callback.Invoke(this);
        return this;
    }
    public Assertion OnFailure(Action<Assertion> callback) {
        if (IsFailure()) callback.Invoke(this);
        return this;
    }
    public Assertion Log(Logger logger, string? message = null, bool fatal = false) {
        logger.LogTrace($"Assertion done: {ToString()}");
        logger.Log(fatal ? Severity.Fatal : Severity.Error, message ?? $"Assertion failed with result: {result}");
        return this;
    }
    public Assertion Throw(Exception? exception = null) {
        return OnFailure((Assertion failedAssertion) => {
            throw exception ?? new AssertException<Assertion>("Assertion failed", this);
        });
    }

    public TNew? As<TNew>() where TNew : Assertion {
        return this as TNew;
    }

    public override string ToString() {
        return $"{GetType().Name}: result = {result}";
    }
}

public class Assertion<T> : Assertion {
    public readonly T value;
    public Assertion(AssertionResult result, T value) : base(result) {
        this.value = value;
    }

    public T GetValue() {
        return value;
    }
    public Asserter<T> Asserter() {
        return new Asserter<T>(value);
    }

    public override string ToString() {
        return $"{GetType().Name}: result = {result}, value = {value}";
    }
}