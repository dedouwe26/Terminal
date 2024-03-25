namespace OxDEDTerm;

/// <summary>
/// A register for all loggers.
/// </summary>
public static class Loggers {
    public static readonly Dictionary<string, Logger> registeredLoggers = [];
    /// <summary>
    /// Registers a logger.
    /// </summary>
    /// <param name="logger">The logger to register.</param>
    /// <returns>False if there already is a logger with that ID.</returns>
    public static bool Register(Logger logger) {
        if (registeredLoggers.ContainsKey(logger.ID)) { return false; }
        registeredLoggers.Add(logger.ID, logger);
        return true;
    }
    /// <summary>
    /// Unregisters a logger
    /// </summary>
    /// <param name="logger">The logger to unregister.</param>
    /// <returns>True if it was successful, false if there is no logger with that ID.</returns>
    public static bool UnRegister(Logger logger) {
        return registeredLoggers.Remove(logger.ID);
    }
    /// <summary>
    /// Gets the logger corresponding to the ID.
    /// </summary>
    /// <param name="ID">The ID of the logger.</param>
    /// <returns>The logger (if there is one).</returns>
    public static Logger? Get(string ID) {
        registeredLoggers.TryGetValue(ID, out Logger? logger);
        return logger;
    }
}

/// <summary>
/// Logger severity.
/// </summary>
public enum Severity : byte {
    Fatal,
    Error,
    Warning,
    Message,
    Info,
    Debug,
    Trace
}

/// <summary>
/// Represents a logger for the terminal and log files.
/// </summary>
public class Logger : IDisposable {
    /// <summary>
    /// Options for a logger.
    /// </summary>
    public struct Options {
        public bool shouldWriteToTerminal = true;
        public bool shouldWriteToFile = true;
        public Options() { }
    }
    public string ID;
    public string Name;
    public TextWriter Out;
    public TextWriter? FileOut;
    public Options options;
    public Severity logLevel = Severity.Info;
    /// <summary>
    /// Creates a logger.
    /// </summary>
    /// <param name="id">The ID to identify this logger, like 'me.0xDED.MyProject' (if this ID is already registered it will throw an <see cref="ArgumentException"/> error).</param>
    /// <param name="name">The name of the logger, used in the log files and terminal.</param>
    /// <param name="filePath">The file path to the log file (required when <see cref="Options.shouldWriteToFile"/> is on).</param>
    /// <param name="severity">The log level of this logger.</param>
    /// <param name="terminalOut">The output stream to the terminal (default: <see cref="Console.Out"/>).</param>
    /// <param name="options">The options for logging.</param>
    /// <exception cref="ArgumentException"/>
    public Logger(string id, string name, string filePath = "./logs/latest.log", Severity severity = Severity.Info, Options? options = null, TextWriter? terminalOut = null) {
        ID = id;
        Name = name;
        Out = terminalOut ?? Console.Out;
        logLevel = severity;
        this.options = options ?? new();
        if (this.options.shouldWriteToFile) {
            FileOut = new StreamWriter(File.OpenWrite(Path.GetFullPath(filePath)));
        }
        if (!Loggers.Register(this)) {
            throw new ArgumentException("A logger with this ID has already been registered.", nameof(id));
        }
    }
    /// <summary>
    /// Changes the file to write to.
    /// </summary>
    /// <param name="filePath">The file path.</param>
    public void SetFile(string filePath) {
        FileOut = new StreamWriter(File.OpenWrite(Path.GetFullPath(filePath)));
    }
    /// <summary>
    /// Sets the log level.
    /// </summary>
    /// <param name="maxSeverity">The highest log level to log.</param>
    public void SetLevel(Severity maxSeverity) {
        logLevel = maxSeverity;
    }
    /// <summary>
    /// Returns the ANSI color corresponding to the severity.
    /// </summary>
    /// <param name="severity">The severity for the color.</param>
    /// <returns></returns>
    protected static string GetColor(Severity severity) {
        if (severity == Severity.Fatal) {
            return new Color(255, 0, 0).ToForegroundANSI();
        } else if (severity == Severity.Error) {
            return new Color(255, 80, 80).ToForegroundANSI();
        } else if (severity == Severity.Warning) {
            return new Color(255, 255, 0).ToForegroundANSI();
        } else if (severity == Severity.Message) {
            return ((Color)Colors.White).ToForegroundANSI();
        } else if (severity == Severity.Info) {
            return new Color(180, 180, 180).ToForegroundANSI();
        } else if (severity == Severity.Debug) {
            return new Color(255,160,0).ToForegroundANSI();
        } else if (severity == Severity.Trace) {
            return new Color(20, 200, 20).ToForegroundANSI();
        }
        return ((Color)Colors.White).ToForegroundANSI();
    }
    /// <summary>
    /// Logs something (<see cref="object.ToString"/>).
    /// </summary>
    /// <typeparam name="T">The type of the data.</typeparam>
    /// <param name="severity">The severity of the text.</param>
    /// <param name="text">The text to write (<see cref="object.ToString"/>).</param>
    public void Log<T>(Severity severity, T? text) {
        if (((byte)severity) > ((byte)logLevel)) { return; }
        string time = DateTime.Now.ToString();
        if (options.shouldWriteToTerminal) {
            Out.WriteLine(GetColor(severity)+"["+Name+"]["+time+"]["+ANSI.Styles.Bold+severity.ToString()+ANSI.Styles.ResetBold+"]: "+text?.ToString()+ANSI.Styles.ResetAll);
        }
        if (options.shouldWriteToFile && FileOut != null) {
            FileOut.WriteLine("["+Name+"]["+time+"]["+severity.ToString()+"]: "+text?.ToString());
        }
    }
    /// <summary>
    /// Logs something with Info severity.
    /// </summary>
    /// <typeparam name="T">The type of the data.</typeparam>
    /// <param name="text">The text to write (<see cref="object.ToString"/>).</param>
    public void LogInfo<T>(T? text) {
        Log(Severity.Info, text);
    }
    /// <summary>
    /// Logs something with Error severity.
    /// </summary>
    /// <typeparam name="T">The type of the data.</typeparam>
    /// <param name="text">The text to write (<see cref="object.ToString"/>).</param>
    public void LogError<T>(T? text) {
        Log(Severity.Error, text);
    }
    /// <summary>
    /// Logs something with Fatal severity.
    /// </summary>
    /// <typeparam name="T">The type of the data.</typeparam>
    /// <param name="text">The text to write (<see cref="object.ToString"/>).</param>
    public void LogFatal<T>(T? text) {
        Log(Severity.Fatal, text);
    }
    /// <summary>
    /// Logs something with Warning severity.
    /// </summary>
    /// <typeparam name="T">The type of the data.</typeparam>
    /// <param name="text">The text to write (<see cref="object.ToString"/>).</param>
    public void LogWarning<T>(T? text) {
        Log(Severity.Warning, text);
    }
    /// <summary>
    /// Logs something with Debug severity.
    /// </summary>
    /// <typeparam name="T">The type of the data.</typeparam>
    /// <param name="text">The text to write (<see cref="object.ToString"/>).</param>
    public void LogDebug<T>(T? text) {
        Log(Severity.Debug, text);
    }
    /// <summary>
    /// Logs something with Message severity.
    /// </summary>
    /// <typeparam name="T">The type of the data.</typeparam>
    /// <param name="text">The text to write (<see cref="object.ToString"/>).</param>
    public void LogMessage<T>(T? text) {
        Log(Severity.Message, text);
    }
    /// <summary>
    /// Logs something with Trace severity.
    /// </summary>
    /// <typeparam name="T">The type of the data.</typeparam>
    /// <param name="text">The text to write (<see cref="object.ToString"/>).</param>
    public void LogTrace<T>(T? text) {
        Log(Severity.Trace, text);
    }
    /// <summary>
    /// Closes / saves the log file, and unregisters this logger.
    /// </summary>
    public void Dispose() {
        Loggers.UnRegister(this);
        FileOut?.Close();
        GC.SuppressFinalize(this);
    }
}

/// <summary>
/// Represents a target for logger outputs.
/// </summary>
public interface ITarget : IDisposable {
    public bool enabled;
    /// <summary>
    /// The method to write to output
    /// </summary>
    /// <typeparam name="T">The type of the text.</typeparam>
    /// <param name="severity">The severity of the message.</param>
    /// <param name="time">The time when it has been logged.</param>
    /// <param name="name">The name of the logger.</param>
    /// <param name="ID">The ID of the logger.</param>
    /// <param name="text">The text to write (<see cref="object.ToString"/>).</param>
    public void Write<T>(Severity severity, DateTime time, string name, string ID, T? text);
}