namespace OxDED.Terminal.Arguments;

/// <summary>
/// Represents a required argument in a specific order.
/// </summary>
public class Argument : ICloneable {
    /// <summary>
    /// The name (key) of this argument.
    /// </summary>
    public string name;
    /// <summary>
    /// The description of this argument.
    /// </summary>
    public string? description;
    internal string? value;
    /// <summary>
    /// If this argument has a value (should be yes).
    /// </summary>
    public bool HasValue { get => value != null; }
    /// <summary>
    /// The value of this argument (error if it isn't parsed).
    /// </summary>
    /// <exception cref="InvalidOperationException"/>
    public string Value { get {
        if (HasValue) {
            return value!;
        } else {
            throw new InvalidOperationException("This argument has not yet been parsed");
        }
    } }

    /// <summary>
    /// Creates a argument.
    /// </summary>
    /// <param name="name">The name of this argument.</param>
    /// <param name="description">The description of this argument (optional).</param>
    public Argument(string name, string? description = null) {
        this.name = name;
        this.description = description;
    }
    /// <summary>
    /// Sets the name of this argument.
    /// </summary>
    /// <param name="name">The new name of this argument.</param>
    /// <returns>This argument.</returns>
    public Argument Name(string name) {
        this.name = name;
        return this;
    }
    /// <summary>
    /// Sets the description of this argument.
    /// </summary>
    /// <param name="description">The new description of this argument.</param>
    /// <returns>This argument.</returns>
    public Argument Description(string? description) {
        this.description = description;
        return this;
    }

    /// <inheritdoc/>
    /// <remarks>
    /// Calls <see cref="CloneArgument"/>.
    /// </remarks>
    
    public object Clone() {
        return CloneArgument();
    }

    /// <summary>
    /// Clones this color.
    /// </summary>
    /// <returns>The new copy of this color.</returns>
    /// <exception cref="InvalidOperationException"/>
    public Argument CloneArgument() {
        return new Argument(name, description);
    }
}