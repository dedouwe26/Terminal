namespace OxDED.Terminal.Arguments;

public class PositionalArgument : ICloneable {
    public string name;
    public string? description;
    internal string? value;
    public bool HasValue { get => value != null; }
    public string Value { get {
        if (HasValue) {
            return value!;
        } else {
            throw new InvalidOperationException("This positional argument has not been parsed.");
        }
    } }
    public PositionalArgument(string name, string? description = null) {
        this.name = name;
        this.description = description;
    }
    public PositionalArgument Name(string name) {
        this.name = name;
        return this;
    }
    public PositionalArgument Description(string? description) {
        this.description = description;
        return this;
    }

    /// <inheritdoc/>
    /// <remarks>
    /// Calls <see cref="ClonePositionalArgument"/>.
    /// </remarks>
    public object Clone() {
        return ClonePositionalArgument();
    }

    /// <summary>
    /// Clones this color.
    /// </summary>
    /// <returns>The new copy of this color.</returns>
    /// <exception cref="InvalidOperationException"/>
    public PositionalArgument ClonePositionalArgument() {
        return new PositionalArgument(name, description);
    }
}