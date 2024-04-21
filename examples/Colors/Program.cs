using OxDEDTerm;

class Program {
    public static void Main(string[] args) {
        // Using style.
        Terminal.WriteLine("Red", new Style{foregroundColor = Color.Red});

        string defaultColor = ((Color)Colors.Default).ToForegroundANSI();
        byte[] randomBytes = new byte[1];
        Random.Shared.NextBytes(randomBytes);
        // In string formatting.
        Terminal.WriteLine(
            @$"{Color.Green.ToForegroundANSI()}Green{defaultColor}{"\t\t"}{ANSI.Styles.Blink}Blinking{ANSI.Styles.ResetBlink}
{Color.Blue.ToForegroundANSI()}Blue{defaultColor}{"\t\t"}{ANSI.Styles.Bold}Bold{ANSI.Styles.ResetBold}
{Color.LightRed.ToForegroundANSI()}LightRed{defaultColor}{"\t"}{ANSI.Styles.Faint}Faint{ANSI.Styles.ResetFaint}
{Color.DarkGreen.ToForegroundANSI()}DarkGreen{defaultColor}{"\t"}{ANSI.Styles.Underline}Underline{ANSI.Styles.ResetUnderline}
{Color.DarkBlue.ToForegroundANSI()}DarkBlue{defaultColor}{"\t"}{ANSI.Styles.DoubleUnderline}DoubleUnderLine?{ANSI.Styles.ResetDoubleUnderline}
{Color.Yellow.ToForegroundANSI()}Yellow{defaultColor}{"\t\t"}{ANSI.Styles.Striketrough}Striketrough{ANSI.Styles.ResetStriketrough}
{Color.Orange.ToForegroundANSI()}Orange{defaultColor}{"\t\t"}{ANSI.Styles.Inverse}Inverse{ANSI.Styles.ResetInverse}
{Color.White.ToForegroundANSI()}White{defaultColor}{"\t\t"}{ANSI.Styles.Invisible}Invisible, you dont see me!{ANSI.Styles.ResetInvisible}
{Color.Gray.ToForegroundANSI()}Gray{defaultColor}{"\t\t"}{ANSI.Styles.Italic}Italic{ANSI.Styles.ResetItalic}
{Color.Black.ToForegroundANSI()}Black{defaultColor}{"\t\t"}{new Color(randomBytes[0]).ToForegroundANSI()}Random color from the terminal-generated table.
            "
        );
        Terminal.WriteLine("Test for bold interference with faint", new Style{Bold = true});
        Terminal.WriteLine("Test for underline interference with double underline", new Style{Underline = true});

        Terminal.ListenForKeys = true;
        Terminal.WriteLine("Press X to clear terminal...");
        Terminal.onKeyPress += (ConsoleKey key, char keyChar, bool alt, bool shift, bool control) => {
            if (key == ConsoleKey.X) {
                Terminal.Clear();
            }
            Terminal.ListenForKeys = false;
        };
    }
}