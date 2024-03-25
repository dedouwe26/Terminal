using System.Reflection.Metadata.Ecma335;

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
    public string ID;
    public string Name;
    /// <summary>
    /// All the targets, key MUST BE nameof(...Target).
    /// </summary>
    public Dictionary<string, (ITarget target, bool enabled)> Targets;
    public Severity logLevel = Severity.Info;
    /// <summary>
    /// Creates a logger.
    /// </summary>
    /// <param name="id">The ID to identify this logger, like 'me.0xDED.MyProject' (if this ID is already registered it will throw an <see cref="ArgumentException"/> error).</param>
    /// <param name="name">The name of the logger, used in the log files and terminal.</param>
    /// <param name="severity">The log level of this logger.</param>
    /// <exception cref="ArgumentException"/>
    public Logger(string id, string name, Severity severity = Severity.Info, Dictionary<string, ITarget>? targets = null) {
        ID = id;
        Name = name;
        logLevel = severity;
        Targets = (targets ?? new Dictionary<string, ITarget>{{nameof(TerminalTarget), new TerminalTarget()}, {nameof(FileTarget),new FileTarget()}}).Select(target => new KeyValuePair<string, (ITarget, bool)>(target.Key, (target.Value, true))).ToDictionary();
        if (!Loggers.Register(this)) {
            throw new ArgumentException("A logger with this ID has already been registered.", nameof(id));
        }
    }
    /// <summary>
    /// Sets the log level.
    /// </summary>
    /// <param name="maxSeverity">The highest log level to log.</param>
    public void SetLevel(Severity maxSeverity) {
        logLevel = maxSeverity;
    }
    public bool SetTarget(string nameOf, bool enabled) {
        Targets.Set(nameOf, Targets.Get(nameOf).enabled =
    /// <summary>
    /// Returns the ANSI color corresponding to the severity.
    /// </summary>
    /// <param name="severity">The severity for the color.</param>
    /// <returns></returns>
    public static string GetColor(Severity severity) {
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
        if (((byte)severity) > ((byte)logLevel)) { return; 
        DateTime time = DateTime.Now;
        foreach ((ITarget target, bool enabled) target in Targets) {
            if (target.enabled) {
                target.target.Write(severity, time, this, text);
            }
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
    /// Disposes all targets, and unregisters this logger.
    /// </summary>
    public void Dispose() {
        Loggers.UnRegister(this);
        foreach ((ITarget target, bool enabled) target in Targets) {
            target.target.Dispose();
        }
        GC.SuppressFinalize(this);
    }
}

/// <summary>
/// Represents a target for logger outputs.
/// </summary>
public interface ITarget : IDisposable {
    /// <summary>
    /// The method to write to output
    /// </summary>
    /// <typeparam name="T">The type of the text.</typeparam>
    /// <param name="severity">The severity of the message.</param>
    /// <param name="time">The time when it has been logged.</param>
    /// <param name="logger">The logger.</param>
    /// <param name="text">The text to write (<see cref="object.ToString"/>).</param>
    public void Write<T>(Severity severity, DateTime time, Logger logger, T? text);
}
public class TerminalTarget : ITarget {
    public TextWriter Out;
    public TerminalTarget(TextWriter? terminalOut = null) {
        Out = (terminalOut ?? Terminal.Out);
    }
    public void Dispose() {
        GC.SuppressFinalize(this);
    }

    public void Write<T>(Severity severity, DateTime time, string name, string ID, T? text) {
        Out.WriteLine(Logger.GetColor(severity)+"["+Name+"]["+time.ToString()+"]["+ANSI.Styles.Bold+severity.ToString()+ANSI.Styles.ResetBold+"]: "+text?.ToString()+ANSI.Styles.ResetAll);
    }
}
public class FileTarget : ITarget
{
    public FileStream FileOut;
    public FileTarget(string path) {
        FileOut = File.OpenWrite(path).;
    }
    public void Dispose() {
        FileOut.Close();
        GC.SuppressFinalize(this);
    }

    public void Write<T>(Severity severity, DateTime time, string name, string ID, T? text) {
        FileOut.WriteLine("["+Name+"]["+time+"]["+severity.ToString()+"]: "+text?.ToString());
    }
}