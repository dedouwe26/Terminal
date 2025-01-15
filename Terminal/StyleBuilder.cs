namespace OxDED.Terminal;

/// <summary>
/// Creates stylized text.
/// </summary>
public class StyleBuilder {
    private string text;

    /// <summary>
    /// Creates a new style builder.
    /// </summary>
    public StyleBuilder() {
        text = "";
    }
    /// <summary>
    /// Writes the text bold or not.
    /// </summary>
    /// <param name="isBold">Whether the text should be bold.</param>
    /// <returns>This style builder.</returns>
    public StyleBuilder Bold(bool isBold = true) {
        text += isBold ? ANSI.Styles.Bold : ANSI.Styles.ResetBold;
        return this;
    }
    /// <summary>
    /// Writes the text faint or not.
    /// </summary>
    /// <param name="isFaint">Whether the text should be faint.</param>
    /// <returns>This style builder.</returns>
    public StyleBuilder Faint(bool isFaint = true) {
        text += isFaint ? ANSI.Styles.Faint : ANSI.Styles.ResetFaint;
        return this;
    }
    /// <summary>
    /// Writes the text italic or not.
    /// </summary>
    /// <param name="isItalic">Whether the text should be italic.</param>
    /// <returns>This style builder.</returns>
    public StyleBuilder Italic(bool isItalic = true) {
        text += isItalic ? ANSI.Styles.Italic : ANSI.Styles.ResetItalic;
        return this;
    }
    /// <summary>
    /// Writes the text underlined or not.
    /// </summary>
    /// <param name="isUnderlined">Whether the text should be underlined.</param>
    /// <returns>This style builder.</returns>
    public StyleBuilder Underline(bool isUnderlined = true) {
        text += isUnderlined ? ANSI.Styles.Underline : ANSI.Styles.ResetUnderline;
        return this;
    }
    /// <summary>
    /// Writes the text blinking or not.
    /// </summary>
    /// <param name="isBlinking">Whether the text should be blinking.</param>
    /// <returns>This style builder.</returns>
    public StyleBuilder Blink(bool isBlinking = true) {
        text += isBlinking ? ANSI.Styles.Blink : ANSI.Styles.ResetBlink;
        return this;
    }
    /// <summary>
    /// Writes the text inversed or not.
    /// </summary>
    /// <param name="isInversed">Whether the text should be inversed.</param>
    /// <returns>This style builder.</returns>
    public StyleBuilder Inverse(bool isInversed = true) {
        text += isInversed ? ANSI.Styles.Inverse : ANSI.Styles.ResetInverse;
        return this;
    }
    /// <summary>
    /// Writes the text invisible or not.
    /// </summary>
    /// <param name="isInvisible">Whether the text should be invisible.</param>
    /// <returns>This style builder.</returns>
    public StyleBuilder Invisible(bool isInvisible = true) {
        text += isInvisible ? ANSI.Styles.Invisible : ANSI.Styles.ResetInvisible;
        return this;
    }
    /// <summary>
    /// Writes the text striketrough or not.
    /// </summary>
    /// <param name="striketrough">Whether the text should be striketrough.</param>
    /// <returns>This style builder.</returns>
    public StyleBuilder Striketrough(bool striketrough = true) {
        text += striketrough ? ANSI.Styles.Striketrough : ANSI.Styles.ResetStriketrough;
        return this;
    }
    /// <summary>
    /// Writes the text double underlined or not.
    /// </summary>
    /// <param name="isDoubleUnderlined">Whether the text should be double underlined.</param>
    /// <returns>This style builder.</returns>
    public StyleBuilder DoubleUnderline(bool isDoubleUnderlined = true) {
        text += isDoubleUnderlined ? ANSI.Styles.DoubleUnderline : ANSI.Styles.ResetDoubleUnderline;
        return this;
    }
    /// <summary>
    /// Resets the style of the upcoming text.
    /// </summary>
    /// <returns>This style builder.</returns>
    public StyleBuilder Reset() {
        text += ANSI.Styles.ResetAll;
        return this;
    }
    /// <summary>
    /// Resets the background color of the upcoming text.
    /// </summary>
    /// <returns>This style builder.</returns>
    public StyleBuilder ResetBackground() {
        return Background(Colors.Default);
    }
    /// <summary>
    /// Resets the text color of the upcoming text.
    /// </summary>
    /// <returns>This style builder.</returns>
    public StyleBuilder ResetForeground() {
        return Foreground(Colors.Default);
    }
    /// <summary>
    /// Sets the background color.
    /// </summary>
    /// <returns>This style builder.</returns>
    public StyleBuilder Background(Color backgroundColor) {
        text += backgroundColor.ToBackgroundANSI();
        return this;
    }
    /// <summary>
    /// Sets the text color.
    /// </summary>
    /// <returns>This style builder.</returns>
    public StyleBuilder Foreground(Color foregroundColor) {
        text += foregroundColor.ToForegroundANSI();
        return this;
    }
    /// <summary>
    /// Adds text.
    /// </summary>
    /// <returns>This style builder.</returns>
    public StyleBuilder Text(string text) {
        this.text += text;
        return this;
    }
    /// <summary>
    /// Adds a new line.
    /// </summary>
    /// <returns>This style builder.</returns>
    public StyleBuilder NewLine() {
        text += '\n';
        return this;
    }

    /// <summary>
    /// Returns the builded text.
    /// </summary>
    /// <returns>The builded text.</returns>
    public override string ToString() {
        return text;
    }
}