// The namespace of terminal logging.
using OxDED.Terminal.Logging;

class Program {
    public const string LoggerID = "me.0xDED.Terminal.examples";

    // Creates a logger with an ID and name and with severity Trace (lowest).
    public static Logger logger = new(LoggerID, "Logging Example", Severity.Trace);
    public static void Main() {
        logger.LogMessage("Hello, world!");
        logger.LogInfo("This is the start of the program!");
        logger.LogWarning("This is a warning.");
        logger.LogDebug("Let's debug that.");
        logger.LogTrace("It came from there.");
        // Gets logger with logger id.
        Loggers.Get(LoggerID)?.LogError("It happend again.");
        Loggers.Get(LoggerID)?.LogFatal("Bye");
        // Do not forget to dispose the logger.
        logger.Dispose();
    }
}