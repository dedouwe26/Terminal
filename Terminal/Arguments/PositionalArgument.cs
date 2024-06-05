namespace OxDED.Terminal.Arguments;

/// <summary>
/// Represents a required argument in a specific order.
/// </summary>
public class PositionalArgument : ICloneable {
    /// <summary>
    /// The name (key) of this positional argument.
    /// </summary>
    public string name;
    /// <summary>
    /// The description of this positional argument.
    /// </summary>
    public string? description;
    internal string? value;
    /// <summary>
    /// If this argument has a value (should be yes).
    /// </summary>
    public bool HasValue { get => value != null; }
    /// <summary>
    /// The value of this positional argument (error if it isn't parsed).
    /// </summary>
    /// <exception cref="InvalidOperationException"/>
    public string Value { get {
        if (HasValue) {
            return value!;
        } else {
            throw new InvalidOperationException("This positional argument has not been parsed.");
        }
    } }
    /// <summary>
    /// Creates a positional argument.
    /// </summary>
    /// <param name="name">The name of this positional argument.</param>
    /// <param name="description">The description of this positional argument (optional).</param>
    public PositionalArgument(string name, string? description = null) {
        this.name = name;
        this.description = description;
    }
    /// <summary>
    /// Sets the name of this positional argument.
    /// </summary>
    /// <param name="name">The new name of this positional argument.</param>
    /// <returns>This positional argument.</returns>
    public PositionalArgument Name(string name) {
        this.name = name;
        return this;
    }
    /// <summary>
    /// Sets the description of this positional argument.
    /// </summary>
    /// <param name="description">The new description of this positional argument.</param>
    /// <returns>This positional argument.</returns>
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