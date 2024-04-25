namespace OxDED.Terminal;

/// <summary>
/// Contains all the ANSI codes.
/// </summary>
public static class ANSI {
    /// <summary>
    /// (1B)
    /// </summary>
    public const string ESC = "\x1B";
    /// <summary>
    /// CSI
    /// </summary>
    public const string CSI = ESC+"[";
    
    /// <summary>
    /// 
    /// </summary>
    public const string EraseScreen = CSI+"2J";
    /// <summary>
    /// 
    /// </summary>
    public const string EraseScreenFromCursor = CSI+"0J";
    /// <summary>
    /// 
    /// </summary>
    public const string EraseLine = CSI+"2K";
    /// <summary>
    /// 
    /// </summary>
    public const string EraseLineFromCursor = CSI+"0K";
    /// <summary>
    /// Will return as CSI{row};{column}R <para/>
    /// Where CSI is CSI,
    /// {row} is the row,
    /// {column} is the column.
    /// </summary>
    public const string RequestCursorPosition = CSI+"6n";
    /// <summary>
    /// 
    /// </summary>
    public const string CursorInvisible = CSI+"?25l";
    /// <summary>
    /// 
    /// </summary>
    public const string CursorVisible = CSI+"?25h";

    /// <summary>
    /// Contains all the ANSI codes for text decoration (excluding colors).
    /// </summary>
    public static class Styles {
        /// <summary>
        /// Resets all text decoration, including colors.
        /// </summary>
        public const string ResetAll = CSI+"0m";

        /// <summary>
        /// Bold, interferes with <see cref="Faint"/>. <para/>
        /// NOTE: In Windows Terminal you will need to set the 'Intense text style' in the profile to 'Bold font'.
        /// </summary>
        public const string Bold = CSI+"1m";
        /// <summary>
        /// Resets <see cref="Bold"/> or <see cref="Faint"/>.
        /// </summary>
        public const string ResetBold = CSI+"22m";

        /// <summary>
        /// Dim or faint, interferes with <see cref="Bold"/>.
        /// </summary>
        public const string Faint = CSI+"2m";
        /// <summary>
        /// Resets <see cref="Faint"/> or <see cref="Bold"/>.
        /// </summary>
        public const string ResetFaint = CSI+"22m";

        /// <summary>
        /// 
        /// </summary>
        public const string Italic = CSI+"3m";
        /// <summary>
        /// 
        /// </summary>
        public const string ResetItalic = CSI+"23m";

        /// <summary>
        /// Underlined, interferes with <see cref="DoubleUnderline"/>.
        /// </summary>
        public const string Underline = CSI+"4m";
        /// <summary>
        /// Resets <see cref="Underline"/> or <see cref="DoubleUnderline"/>.
        /// </summary>
        public const string ResetUnderline = CSI+"24m";

        /// <summary>
        /// 
        /// </summary>
        public const string Blink = CSI+"5m";
        /// <summary>
        /// 
        /// </summary>
        public const string ResetBlink = CSI+"25m";
        
        /// <summary>
        /// Inverse / reverse.
        /// </summary>
        public const string Inverse = CSI+"7m";
        /// <summary>
        /// 
        /// </summary>
        public const string ResetInverse = CSI+"27m";

        /// <summary>
        /// Invisible / hidden.
        /// </summary>
        public const string Invisible = CSI+"8m";
        /// <summary>
        /// 
        /// </summary>
        public const string ResetInvisible = CSI+"28m";

        /// <summary>
        /// Striketrough / linetrough.
        /// </summary>
        public const string Striketrough = CSI+"9m";
        /// <summary>
        /// 
        /// </summary>
        public const string ResetStriketrough = CSI+"29m";

        /// <summary>
        /// non-specified, may only work in some terminals.
        /// Interferes with <see cref="Underline"/>.
        /// </summary>
        public const string DoubleUnderline = CSI+"21m";
        /// <summary>
        /// Resets <see cref="DoubleUnderline"/> or <see cref="Underline"/>.
        /// </summary>
        public const string ResetDoubleUnderline = CSI+"24m";
    }
    /// <summary>
    /// Generates an ANSI code for moving the cursor.
    /// </summary>
    /// <param name="x">The x pos.</param>
    /// <param name="y">The y pos.</param>
    /// <returns>The ANSI code.</returns>
    public static string MoveCursor(int x, int y) {
        return CSI+y.ToString()+";"+x.ToString()+"H";
    }
    /// <summary>
    /// Generates an ANSI code for setting the foreground color.
    /// </summary>
    /// <param name="color">basic set index.</param>
    /// <returns>The ANSI code.</returns>
    public static string BasicSetForegroundColor(byte color) {
        return CSI+color.ToString()+"m";
    }
    /// <summary>
    /// Generates an ANSI code for setting the background color.
    /// </summary>
    /// <param name="color">basic set index.</param>
    /// <returns>The ANSI code.</returns>
    public static string BasicSetBackgroundColor(byte color) {
        return CSI+(color+10).ToString()+"m";
    }
    /// <summary>
    /// Generates an ANSI code for setting the background color.
    /// </summary>
    /// <param name="color">table index.</param>
    /// <returns>The ANSI code.</returns>
    public static string TableBackgroundColor(byte color) {
        return CSI+"48;5;"+color.ToString()+"m";
    }
    /// <summary>
    /// Generates an ANSI code for setting the foreground color.
    /// </summary>
    /// <param name="color">table index.</param>
    /// <returns>The ANSI code.</returns>
    public static string TableForegroundColor(byte color) {
        return CSI+"38;5;"+color.ToString()+"m";
    }
    
    /// <summary>
    /// Generates an ANSI code for setting the foreground color.
    /// </summary>
    /// <param name="r">R value.</param>
    /// <param name="g">G value.</param>
    /// <param name="b">B value.</param>
    /// <returns>The ANSI code.</returns>
    public static string ForegroundTrueColor(byte r, byte g, byte b) {
        return CSI+"38;2;"+r.ToString()+";"+g.ToString()+";"+b.ToString()+"m";
    }
    /// <summary>
    /// Generates an ANSI code for setting the background color.
    /// </summary>
    /// <param name="r">R value.</param>
    /// <param name="g">G value.</param>
    /// <param name="b">B value.</param>
    /// <returns>The ANSI code.</returns>
    public static string BackgroundTrueColor(byte r, byte g, byte b) {
        return CSI+"48;2;"+r.ToString()+";"+g.ToString()+";"+b.ToString()+"m";
    }
}
