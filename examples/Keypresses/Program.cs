// The namespace of the terminal.
using OxDEDTerm;

class Program {
    public static sbyte countdown = 5;
    public static void Main(string[] args) {
        // Will listen for keys.
        Terminal.ListenForKeys = true;
        // Adds an event.
        Terminal.onKeyPress += OnKey;
    }
    public static void OnKey(ConsoleKey key, char keyChar, bool alt, bool shift, bool control) {
        // Says what key, and countdown
        Terminal.WriteLine(key.ToString() + countdown.ToString());

        // lowers the countdown and checks if it is at zero.
        if ((--countdown)==0) {
            // Stops listening for keys.
            Terminal.ListenForKeys = false;

            Terminal.WriteLine("Waiting for input...", new Style{Bold = true});
            //Waits for key press.
            Terminal.WaitForKeyPress();
            // End of program
        }
    }
}