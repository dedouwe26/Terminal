namespace Terminal;

public class Style {
    public bool Bold = false;
    public bool Faint = false;
    public bool Italic = false;
    public bool Underline = false;
    public bool Blink = false;
    public bool Inverse = false;
    public bool Invisible = false;
    public bool Striketrough = false;
    /// <summary>
    /// May only work in some terminals.
    /// </summary>
    public bool DoubleUnderLine = false;

    public required Color foregroundColor;
    public required Color backgroundColor;
    public string ToANSI() {
        return 
            (Bold ? ANSI.Styles.Bold : ANSI.Styles.ResetBold) +
            (Faint ? ANSI.Styles.Faint : ANSI.Styles.ResetFaint) +
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