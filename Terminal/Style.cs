namespace OxDEDTerm;

/// <summary>
/// Contains the style decorations.
/// </summary>
public struct Style {
    /// <summary>
    /// Interferes with <see cref="Faint"/>.
    /// </summary>
    public bool Bold = false;
    /// <summary>
    /// Interferes with <see cref="Bold"/>.
    /// </summary>
    public bool Faint = false;
    /// <summary>
    /// 
    /// </summary>
    public bool Italic = false;
    /// <summary>
    /// 
    /// </summary>
    public bool Underline = false;
    /// <summary>
    /// 
    /// </summary>
    public bool Blink = false;
    /// <summary>
    /// 
    /// </summary>
    public bool Inverse = false;
    /// <summary>
    /// 
    /// </summary>
    public bool Invisible = false;
    /// <summary>
    /// </summary>
    public bool Striketrough = false;
    /// <summary>
    /// May only work in some terminals.
    /// </summary>
    public bool DoubleUnderLine = false;

    /// <summary>
    /// The text color.
    /// </summary>
    public Color foregroundColor = Colors.Default;
    /// <summary>
    /// The color behind the text.
    /// </summary>
    public Color backgroundColor = Colors.Default;

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
            (Bold ? ANSI.Styles.Bold : ANSI.Styles.ResetBold) +
            (Faint ? ANSI.Styles.Faint : (Bold ? ANSI.Styles.ResetFaint : "")) +
            (Italic ? ANSI.Styles.Italic : ANSI.Styles.ResetItalic) +
            (Underline ? ANSI.Styles.Underline : ANSI.Styles.ResetUnderline) +
            (Blink ? ANSI.Styles.Blink : ANSI.Styles.ResetBlink) +
            (Inverse ? ANSI.Styles.Inverse : ANSI.Styles.ResetInverse) +
            (Invisible ? ANSI.Styles.Invisible : ANSI.Styles.ResetInvisible) +
            (Striketrough ? ANSI.Styles.Striketrough : ANSI.Styles.ResetStriketrough) +
            (DoubleUnderLine ? ANSI.Styles.DoubleUnderLine : ANSI.Styles.ResetDoubleUnderLine) +
            backgroundColor.ToBackgroundANSI() + foregroundColor.ToForegroundANSI();
    }
}