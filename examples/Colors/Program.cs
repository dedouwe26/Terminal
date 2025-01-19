// The namespace of the terminal.
using OxDED.Terminal;

class Program {
    public static void Main(string[] args) {
        // * Using style.
        Terminal.WriteLine("Red", new Style{ForegroundColor = Color.Red});

        // Random byte for terminal-generated color table.
        byte[] randomBytes = new byte[1];
        Random.Shared.NextBytes(randomBytes);

        // Printing out all the terminal-defined colors
        foreach (Colors color in Enum.GetValues<Colors>()) {
            Terminal.Write(color.ToString()+' ', new Style() { ForegroundColor = color});
        }

        Terminal.WriteLine();

        // Using Style builder for multiple styles in one go.
        Terminal.WriteLine(
            new StyleBuilder()
            .Foreground(Color.Green).Text("Green\t\t").ResetForeground()        .Blink().Text("Blinking").Blink(false).NewLine()
            .Foreground(Color.Blue).Text("Blue\t\t").ResetForeground()          .Bold().Text("Bold").Bold(false).NewLine()
            .Foreground(Color.LightRed).Text("LightRed\t").ResetForeground()    .Faint().Text("Faint").Faint(false).NewLine()
            .Foreground(Color.DarkGreen).Text("DarkGreen\t").ResetForeground()  .Underline().Text("Underline").Underline(false).NewLine()
            .Foreground(Color.DarkBlue).Text("DarkBlue\t").ResetForeground()    .DoubleUnderline().Text("DoubleUnderLine?").DoubleUnderline(false).NewLine()
            .Foreground(Color.Yellow).Text("Yellow\t\t").ResetForeground()      .Striketrough().Text("Striketrough").Striketrough(false).NewLine()
            .Foreground(Color.Orange).Text("Orange\t\t").ResetForeground()      .Inverse().Text("Inverse").Inverse(false).NewLine()
            .Foreground(Color.White).Text("White\t\t").ResetForeground()        .Invisible().Text("Invisible, you dont see me!").Invisible(false).NewLine()
            .Foreground(Color.Gray).Text("Gray\t\t").ResetForeground()          .Italic().Text("Italic").Italic(false).NewLine()
            .Foreground(Color.Black).Text("Black\t\t").ResetForeground()        .Foreground(new Color(randomBytes[0])).Text("Random color from the terminal-generated table.").Blink(false).NewLine()
        );

        // Tests Interference with other styles.
        Terminal.WriteLine("Test for bold interference with faint", new Style{Bold = true});
        Terminal.WriteLine("Test for underline interference with double underline", new Style{Underline = true});

        // Simple key listener (see: Keypresses example).
        Terminal.ListenForKeys = true;
        Terminal.WriteLine("Press X to clear terminal...");
        Terminal.OnKeyPress += (ConsoleKey key, char keyChar, bool alt, bool shift, bool control) => {
            // If X is pressed, clear terminal.
            if (key == ConsoleKey.X) {
                Terminal.Clear();
            }
            // Stops program.
            Terminal.ListenForKeys = false;
        };
    }
}