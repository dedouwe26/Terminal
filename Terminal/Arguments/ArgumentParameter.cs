namespace OxDED.Terminal.Arguments;

public class ArgumentParameter {
    public string name;
    public string? description;
    internal string? value;
    public bool HasValue { get => value != null; }
    public string Value { get {
        if (HasValue) {
            return value!;
        } else {
            throw new InvalidOperationException("This argument parameter has not been parsed.");
        }
    } }
    public ArgumentParameter(string name, string? description = null) {
        this.name = name;
        this.description = description;
    }
    public ArgumentParameter Name(string name) {
        this.name = name;
        return this;
    }
    public ArgumentParameter Description(string? description) {
        this.description = description;
        return this;
    }
}