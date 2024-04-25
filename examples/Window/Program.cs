// The namespace of the terminal and terminal windowing.
using OxDED.Terminal;
using OxDED.Terminal.Window;

class Program {
    public static void Main(string[] args) {
        TerminalWindow window = Terminal.CreateWindow("test");
        window.WriteLine("lesgo");
        Terminal.WriteLine("lesgo2");
        window.WaitForKeyPress();
    }
}