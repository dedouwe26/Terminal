namespace OxDED.Terminal.Arguments;

public class Argument : ICloneable, IEquatable<Argument> {
    public string[] keys;
    public ArgumentParameter[] parameters;
    public string? description = null;
    public Argument(string key, string? description = null, IEnumerable<ArgumentParameter>? parameters = null) {
        keys = [key];
        this.description = description;
        this.parameters = parameters == null ? [] : [.. parameters];
    }
    public Argument(IEnumerable<string> keys, string? description = null, IEnumerable<ArgumentParameter>? parameters = null) {
        this.keys = [.. keys];
        this.description = description;
        this.parameters = parameters == null ? [] : [.. parameters];
    }
    public Argument Key(string key) {
        keys = [key];
        return this;
    }
    public Argument Keys(IEnumerable<string> keys) {
        this.keys = [.. keys];
        return this;
    }
    public Argument Description(string? description) {
        this.description = description;
        return this;
    }
    public Argument Parameters(IEnumerable<ArgumentParameter> parameters) {
        this.parameters = [.. parameters];
        return this;
    }
    public Argument AddParameter(ArgumentParameter parameter) {
        parameters = [.. parameters, parameter];
        return this;
    }
    public bool HasValue { get => parameters.All((ArgumentParameter parameter) => parameter.HasValue); }

    //
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