namespace OxDED.Terminal.Arguments;

/// <summary>
/// A parameter for an <see cref="Argument"/>.
/// </summary>
public class ArgumentParameter {
    /// <summary>
    /// The name of this argument parameter.
    /// </summary>
    public string name;
    /// <summary>
    /// The description of this argument parameter.
    /// </summary>
    public string? description;
    internal string? value;
    /// <summary>
    /// If this argument parameter has a value (should be yes).
    /// </summary>
    public bool HasValue { get => value != null; }
    /// <summary>
    /// The value of this argument parameter (error if it isn't parsed).
    /// </summary>
    /// <exception cref="InvalidOperationException"/>
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