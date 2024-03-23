namespace Terminal;

public static class ANSI {
    public const string ESC = "\x1B";
    public const string CSI = "\x9B";
    
    public const string EraseScreen = CSI+"2J";
    public const string EraseFromCursor = CSI+"0J";
    public const string EraseLine = CSI+"2K";
    public const string EraseLineFromCursor = CSI+"0K";
    /// <summary>
    /// Will return as CSI{row};{column}R <para/>
    /// Where CSI is CSI,
    /// {row} is the row,
    /// {column} is the column.
    /// </summary>
    public const string RequestCursorPosition = CSI+"6n";
    public const string CursorInvisible = CSI+"?25l";
    public const string CursorVisible = CSI+"?25h";

    public static class Styles {
        public const string ResetAll = CSI+"0m";

        /// <summary>
        /// Bold, interferes with <see cref="Faint"/>.
        /// </summary>
        public const string Bold = CSI+"1m"; // NOTE: In Windows Terminal you will need to set the 'Intense text style' in the profile to 'Bold font'.
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

        public const string Italic = CSI+"3m";
        public const string ResetItalic = CSI+"23m";

        public const string Underline = CSI+"4m";
        public const string ResetUnderline = CSI+"24m";

        public const string Blink = CSI+"5m";
        public const string ResetBlink = CSI+"25m";
        
        /// <summary>
        /// Inverse / reverse.
        /// </summary>
        public const string Inverse = CSI+"7m";
        public const string ResetInverse = CSI+"27m";

        /// <summary>
        /// Invisible / hidden.
        /// </summary>
        public const string Invisible = CSI+"8m";
        public const string ResetInvisible = CSI+"28m";

        /// <summary>
        /// Striketrough / linetrough.
        /// </summary>
        public const string Striketrough = CSI+"9m";
        public const string ResetStriketrough = CSI+"29m";

        /// <summary>
        /// non-specified, may only work in some terminals.
        /// </summary>
        public const string DoubleUnderLine = CSI+"21m";
        public const string ResetDoubleUnderLine = CSI+"21m";
    }
    public static string MoveCursor(int x, int y) {
        return CSI+y.ToString()+";"+x.ToString()+"H";
    }
    public static string BasicSetForegroundColor(byte color) {
        return CSI+color.ToString()+"m";
    }
    public static string BasicSetBackgroundColor(byte color) {
        return CSI+(color+10).ToString()+"m";
    }
    public static string TableBackgroundColor(byte color) {
        return CSI+"48;5;"+color.ToString()+"m";
    }
    public static string TableForegroundColor(byte color) {
        return CSI+"38;5;"+color.ToString()+"m";
    }
    public static string ForegroundTrueColor(byte r, byte g, byte b) {
        return CSI+"38;2;"+r.ToString()+";"+g.ToString()+";"+b.ToString()+"m";
    }
    public static string BackgroundTrueColor(byte r, byte g, byte b) {
        return CSI+"48;2;"+r.ToString()+";"+g.ToString()+";"+b.ToString()+"m";
    }
}
