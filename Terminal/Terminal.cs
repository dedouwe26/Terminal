namespace Terminal;

public static class Terminal {
    public static TextWriter Out {get { return Console.Out; } set { Console.SetOut(value); }}
    public static TextReader In {get { return Console.In; } set { Console.SetIn(value); }}
    public static void Write<T>(T? text, Style? style) {
        Out.Write(style?.ToANSI()+text);
    }
    public static void WriteLine<T>(T? text, Style? style) {
        Out.WriteLine(style?.ToANSI()+text);
    }
    public static void Clear() {
        Out.Write(ANSI.EraseScreen);
    }

}
