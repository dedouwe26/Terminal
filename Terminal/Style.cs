namespace OxDED.Terminal;

/// <summary>
/// Contains the style decorations.
/// </summary>
public class Style : IEquatable<Style>, ICloneable {
    /// <summary>
    /// Interferes with <see cref="Faint"/>.
    /// </summary>
    public bool Bold = false;
    /// <summary>
    /// Interferes with <see cref="Bold"/>.
    /// </summary>
    public bool Faint = false;
    /// 
    public bool Italic = false;
    /// 
    public bool Underline = false;
    /// 
    public bool Blink = false;
    /// 
    public bool Inverse = false;
    /// 
    public bool Invisible = false;
    /// <summary>
    /// </summary>
    public bool Striketrough = false;
    /// <summary>
    /// May only work in some terminals.
    /// </summary>
    public bool DoubleUnderline = false;

    /// <summary>
    /// The text color.
    /// </summary>
    public Color ForegroundColor = Colors.Default;
    /// <summary>
    /// The color behind the text.
    /// </summary>
    public Color BackgroundColor = Colors.Default;

    /// <summary>
    /// Creates a basic style.
    /// </summary>
    public Style() {}

    /// <summary>
    /// Creates an ANSI coded string for the chosen decorations.
    /// </summary>
    /// <returns>The ANSI string.</returns>
    public string ToANSI() {
        return 
            (Bold ? ANSI.Styles.Bold : "") +
            (Faint ? ANSI.Styles.Faint : "") +
            ((!(Bold||Faint)) ? ANSI.Styles.ResetBold : "")+
            (Italic ? ANSI.Styles.Italic : ANSI.Styles.ResetItalic) +
            (Underline ? ANSI.Styles.Underline : "") +
            (Blink ? ANSI.Styles.Blink : ANSI.Styles.ResetBlink) +
            (Inverse ? ANSI.Styles.Inverse : ANSI.Styles.ResetInverse) +
            (Invisible ? ANSI.Styles.Invisible : ANSI.Styles.ResetInvisible) +
            (Striketrough ? ANSI.Styles.Striketrough : ANSI.Styles.ResetStriketrough) +
            (DoubleUnderline ? ANSI.Styles.DoubleUnderline : "") +
            ((!(Underline||DoubleUnderline)) ? ANSI.Styles.ResetUnderline : "") +
            BackgroundColor.ToBackgroundANSI() + ForegroundColor.ToForegroundANSI();
    }

    /// 
    public static bool operator ==(Style? left, Style? right) {
        if (left is null && right is null) {
            return true;
        } else if (left is null) {
            return false;
        }
        return left.Equals(right);
    }
    /// 
    public static bool operator !=(Style? left, Style? right) {
        return !(left == right);
    }
    /// <inheritdoc/>
    /// <remarks>
    /// Checks if the that color is identical to this one.
    /// </remarks>
    public bool Equals(Style? other) {
        if (other is null) {
            return false;
        }
        if (ReferenceEquals(this, other)) {
            return true;
        }
        if (GetType() != other.GetType()) {
            return false;
        }
        return (Bold == other.Bold) &&
               (Faint == other.Faint) &&
               (Italic == other.Italic) &&
               (Underline == other.Underline) &&
               (Blink == other.Blink) &&
               (Inverse == other.Inverse) &&
               (Invisible == other.Invisible) &&
               (Striketrough == other.Striketrough) &&
               (DoubleUnderline == other.DoubleUnderline) &&
               (ForegroundColor == other.ForegroundColor) &&
               (BackgroundColor == other.BackgroundColor);
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
        return (((((((Bold ? 1 : 0)
               << 1 ^ (Faint ? 1 : 0))
               << 1 ^ (Italic ? 1 : 0))
               << 1 ^ (Underline ? 1 : 0))
               << 1 ^ (Inverse ? 1 : 0))
               << 1 ^ (Invisible ? 1 : 0))
               << 1 ^ (Striketrough ? 1 : 0))
               << 1 ^ (DoubleUnderline ? 1 : 0)
                ^ ForegroundColor.GetHashCode()
                ^ ForegroundColor.GetHashCode();
    }
    /// <inheritdoc/>
    /// <remarks>
    /// Calls <see cref="CloneStyle"/>.
    /// </remarks>
    public object Clone() {
        return CloneStyle();
    }
    /// <summary>
    /// Clones this style.
    /// </summary>
    /// <returns>The new copy of this style.</returns>
    public Style CloneStyle() {
        return new Style {
            Bold = Bold,
            Faint = Faint,
            Italic = Italic,
            Underline = Underline,
            Blink = Blink,
            Inverse = Inverse,
            Invisible = Invisible,
            Striketrough = Striketrough,
            DoubleUnderline = DoubleUnderline,
            ForegroundColor = ForegroundColor.CloneColor(),
            BackgroundColor = BackgroundColor.CloneColor()
        };
    }
}