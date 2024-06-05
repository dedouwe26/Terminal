namespace OxDED.Terminal.Arguments;

/// <summary>
/// Represents an optional argument (-f, --foo).
/// </summary>
public class Argument : ICloneable, IEquatable<Argument> {
    /// <summary>
    /// The keys of this argument (f, foo).
    /// </summary>
    public string[] keys;
    /// <summary>
    /// The parameters of this argument.
    /// </summary>
    public ArgumentParameter[] parameters;
    /// <summary>
    /// The description of this argument.
    /// </summary>
    public string? description = null;
    /// <summary>
    /// Creates an argument.
    /// </summary>
    /// <param name="key">The key of this argument.</param>
    /// <param name="description">The description of this argument (optional).</param>
    /// <param name="parameters">The parameters of this argument (default: empty).</param>
    public Argument(string key, string? description = null, IEnumerable<ArgumentParameter>? parameters = null) {
        keys = [key];
        this.description = description;
        this.parameters = parameters == null ? [] : [.. parameters];
    }
    /// <summary>
    /// Creates an argument.
    /// </summary>
    /// <param name="keys">The keys of this argument.</param>
    /// <param name="description">The description of this argument (optional).</param>
    /// <param name="parameters">The parameters of this argument (default: empty).</param>
    public Argument(IEnumerable<string> keys, string? description = null, IEnumerable<ArgumentParameter>? parameters = null) {
        this.keys = [.. keys];
        this.description = description;
        this.parameters = parameters == null ? [] : [.. parameters];
    }
    /// <summary>
    /// Sets the key of this argument.
    /// </summary>
    /// <param name="key">The new key.</param>
    /// <returns>This argument.</returns>
    public Argument Key(string key) {
        keys = [key];
        return this;
    }
    /// <summary>
    /// Sets the keys of this argument.
    /// </summary>
    /// <param name="keys">The new keys.</param>
    /// <returns>This argument.</returns>
    public Argument Keys(IEnumerable<string> keys) {
        this.keys = [.. keys];
        return this;
    }
    /// <summary>
    /// Sets the description of this argument.
    /// </summary>
    /// <param name="description">The new description.</param>
    /// <returns>This argument.</returns>
    public Argument Description(string? description) {
        this.description = description;
        return this;
    }
    /// <summary>
    /// Sets the parameters of this argument.
    /// </summary>
    /// <param name="parameters">The new parameters.</param>
    /// <returns>This argument.</returns>
    public Argument Parameters(IEnumerable<ArgumentParameter> parameters) {
        this.parameters = [.. parameters];
        return this;
    }
    /// <summary>
    /// Adds a parameter to this argument.
    /// </summary>
    /// <param name="parameter">The parameter to add.</param>
    /// <returns>This argument.</returns>
    public Argument AddParameter(ArgumentParameter parameter) {
        parameters = [.. parameters, parameter];
        return this;
    }
    /// <summary>
    /// If this argument's parameters have values (should be yes).
    /// </summary>
    public bool HasValue { get => parameters.All((ArgumentParameter parameter) => parameter.HasValue); }

    ///
    public static bool operator ==(Argument? left, Argument? right) {
        if (left is null && right is null) {
            return true;
        } else if (left is null) {
            return false;
        }
        return left.Equals(right);
    }
    /// 
    public static bool operator !=(Argument? left, Argument? right) {
        return !(left == right);
    }
    /// <inheritdoc/>
    /// <remarks>
    /// Checks if the that color is identical to this one.
    /// </remarks>
    public bool Equals(Argument? other) {
        if (other is null) {
            return false;
        }
        if (ReferenceEquals(this, other)) {
            return true;
        }
        if (GetType() != other.GetType()) {
            return false;
        }
        return keys == other.keys;
    }
    /// <inheritdoc/>
    /// <remarks>
    /// Checks if the that color is identical to this one.
    /// </remarks>
    public override bool Equals(object? obj) {
        return Equals(obj as Color);
    }
    /// <inheritdoc/>
    public override int GetHashCode() {
        return keys.GetHashCode();
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
        return new Argument(keys, description, parameters);
    }
}