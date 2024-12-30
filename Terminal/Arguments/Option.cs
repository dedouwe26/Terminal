namespace OxDED.Terminal.Arguments;

/// <summary>
/// Represents an optional argument (e.g. -f, --foo).
/// </summary>
public partial class Option : ICloneable, IEquatable<Option> {
    /// <summary>
    /// The keys of this option (e.g. f, foo).
    /// </summary>
    public string[] keys;
    /// <summary>
    /// The parameters of this option.
    /// </summary>
    public Parameter[] parameters;
    /// <summary>
    /// The description of this option.
    /// </summary>
    public string? description = null;

    /// <summary>
    /// Creates an option.
    /// </summary>
    public Option() {
        keys = [];
        parameters = [];
    }
    
    /// <summary>
    /// Creates an option.
    /// </summary>
    /// <param name="key">The key of this option.</param>
    /// <param name="description">The description of this option (optional).</param>
    /// <param name="parameters">The parameters of this option (default: empty).</param>
    public Option(string key, string? description = null, IEnumerable<Parameter>? parameters = null) {
        keys = [key];
        this.description = description;
        this.parameters = parameters == null ? [] : [.. parameters];
    }
    /// <summary>
    /// Creates an option.
    /// </summary>
    /// <param name="keys">The keys of this option.</param>
    /// <param name="description">The description of this option (optional).</param>
    /// <param name="parameters">The parameters of this option (default: empty).</param>
    public Option(IEnumerable<string> keys, string? description = null, IEnumerable<Parameter>? parameters = null) {
        this.keys = [.. keys];
        this.description = description;
        this.parameters = parameters == null ? [] : [.. parameters];
    }
    /// <summary>
    /// Sets the key of this option.
    /// </summary>
    /// <param name="key">The new key.</param>
    /// <returns>This option.</returns>
    public Option Key(string key) {
        keys = [key];
        return this;
    }
    /// <summary>
    /// Sets the keys of this option.
    /// </summary>
    /// <param name="keys">The new keys.</param>
    /// <returns>This option.</returns>
    public Option Keys(IEnumerable<string> keys) {
        this.keys = [.. keys];
        return this;
    }
    /// <summary>
    /// Sets the description of this option.
    /// </summary>
    /// <param name="description">The new description.</param>
    /// <returns>This option.</returns>
    public Option Description(string? description) {
        this.description = description;
        return this;
    }
    /// <summary>
    /// Sets the parameters of this option.
    /// </summary>
    /// <param name="parameters">The new parameters.</param>
    /// <returns>This option.</returns>
    public Option Parameters(IEnumerable<Parameter> parameters) {
        this.parameters = [.. parameters];
        return this;
    }
    /// <summary>
    /// Adds a parameter to this option.
    /// </summary>
    /// <param name="parameter">The parameter to add.</param>
    /// <returns>This option.</returns>
    public Option AddParameter(Parameter parameter) {
        parameters = [.. parameters, parameter];
        return this;
    }
    /// <summary>
    /// If this option's parameters have values (should be yes).
    /// </summary>
    public bool HasValue { get => parameters.All((Parameter parameter) => parameter.HasValue); }

    ///
    public static bool operator ==(Option? left, Option? right) {
        if (left is null && right is null) {
            return true;
        } else if (left is null) {
            return false;
        }
        return left.Equals(right);
    }
    /// 
    public static bool operator !=(Option? left, Option? right) {
        return !(left == right);
    }
    /// <inheritdoc/>
    /// <remarks>
    /// Checks if the that color is identical to this one.
    /// </remarks>
    public bool Equals(Option? other) {
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
    /// Calls <see cref="CloneOption"/>.
    /// </remarks>
    public object Clone() {
        return CloneOption();
    }

    /// <summary>
    /// Clones this color.
    /// </summary>
    /// <returns>The new copy of this color.</returns>
    /// <exception cref="InvalidOperationException"/>
    public Option CloneOption() {
        return new Option(keys, description, parameters);
    }
}

