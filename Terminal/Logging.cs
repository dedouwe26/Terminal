namespace OxDEDTerm;

/// <summary>
/// A register for all loggers.
/// </summary>
public static class Loggers {
    private static readonly Dictionary<string, Logger> registeredLoggers = [];
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
    /// Unregisters a logger.
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
    /// <summary>
    /// 
    /// </summary>
    Fatal,
    /// <summary>
    /// 
    /// </summary>
    Error,
    /// <summary>
    /// 
    /// </summary>
    Warning,
    /// <summary>
    /// 
    /// </summary>
    Message,
    /// <summary>
    /// 
    /// </summary>
    Info,
    /// <summary>
    /// 
    /// </summary>
    Debug,
    /// <summary>
    /// 
    /// </summary>
    Trace
}

/// <summary>
/// Represents a logger for the terminal and log files.
/// </summary>
public class Logger : IDisposable {
    /// <summary>
    /// 
    /// </summary>
    public string ID;
    /// <summary>
    /// 
    /// </summary>
    public string Name;
    /// <summary>
    /// All the targets, key MUST BE typeof(...Target) or ITarget.GetType() only when using .
    /// </summary>
    public Dictionary<Type, (ITarget target, bool enabled)> Targets;
    /// <summary>
    /// The current log severity max.
    /// </summary>
    public Severity logLevel = Severity.Info;
    /// <summary>
    /// Creates a logger.
    /// </summary>
    /// <param name="id">The ID to identify this logger, like 'me.0xDED.MyProject' (if this ID is already registered it will throw an <see cref="ArgumentException"/> error).</param>
    /// <param name="name">The name of the logger, used in the log files and terminal.</param>
    /// <param name="severity">The log level of this logger.</param>
    /// <param name="targets">The targets to add and enable (default: <see cref="TerminalTarget"/>, <see cref="FileTarget"/> with path "./latest.log").</param>
    /// <exception cref="ArgumentException"/>
    public Logger(string id, string name, Severity severity = Severity.Info, Dictionary<Type, ITarget>? targets = null) {
        ID = id;
        Name = name;
        logLevel = severity;
        Targets = targets != null ? targets.Select(target => new KeyValuePair<Type, (ITarget, bool)>(target.Key, (target.Value, true))).ToDictionary() : new Dictionary<Type, (ITarget, bool enabled)>{{typeof(FileTarget), (new TerminalTarget(), true)}, {typeof(FileTarget), (new FileTarget("./latest.log"), true)}};
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

    /// <summary>
    /// Sets if a target is enabled.
    /// </summary>
    /// <typeparam name="T">The type of the Target (e.g. TerminalTarget).</typeparam>
    /// <param name="enabled">True if enabled.</param>
    /// <returns>True if there is a Target with that type.</returns>
    public bool SetTarget<T>(bool enabled) where T : ITarget {
        return SetTarget(typeof(T), enabled);
    }

    /// <summary>
    /// Sets if a target is enabled.
    /// </summary>
    /// <param name="type">The type of the Target (e.g. typeof(TerminalTarget)).</param>
    /// <param name="enabled">True if enabled.</param>
    /// <returns>True if there is a Target with that type.</returns>
    public bool SetTarget(Type type, bool enabled) {
        if (Targets.TryGetValue(type, out (ITarget target, bool enabled) value)) {
            value.enabled = enabled;
            Targets[type] = value;
            return true;
        }
        return false;
    }

    /// <summary>
    /// Adds a target.
    /// </summary>
    /// <param name="target">The target to add (e.g. typeof(TerminalTarget)).</param>
    /// <param name="enabled">If it is enabled.</param>
    public void AddTarget(ITarget target, bool enabled = true) {
        Targets.Add(target.GetType(), (target, enabled));
    }

    /// <summary>
    /// Removes a Target.
    /// </summary>
    /// <typeparam name="T">The type of the target</typeparam>
    /// <returns>True if there was a target with that type.</returns>
    public bool RemoveTarget<T>() where T : ITarget {
        return RemoveTarget(typeof(T));
    }

    /// <summary>
    /// Removes a Target.
    /// </summary>
    /// <param name="type">The type of the target</param>
    /// <returns>True if there was a target with that type.</returns>
    public bool RemoveTarget(Type type) {
        return Targets.Remove(type);
    }
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
        if (((byte)severity) > ((byte)logLevel)) { return; }
        DateTime time = DateTime.Now;
        foreach (KeyValuePair<Type, (ITarget target, bool enabled)> target in Targets) {
            if (target.Value.enabled) {
                target.Value.target.Write(severity, time, this, text);
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
        foreach (KeyValuePair<Type, (ITarget target, bool enabled)> target in Targets) {
            target.Value.target.Dispose();
        }
        GC.SuppressFinalize(this);
    }
}

/// <summary>
/// Represents a target for logger outputs.
/// </summary>
public interface ITarget : IDisposable {
    /// <summary>
    /// The method to write to output.
    /// </summary>
    /// <typeparam name="T">The type of the text.</typeparam>
    /// <param name="severity">The severity of the message.</param>
    /// <param name="time">The time when it has been logged.</param>
    /// <param name="logger">The logger.</param>
    /// <param name="text">The text to write (<see cref="object.ToString"/>).</param>
    public void Write<T>(Severity severity, DateTime time, Logger logger, T? text);
}
/// <summary>
/// A Logger Target for the terminal.
/// </summary>
public class TerminalTarget : ITarget {
    /// <summary>
    /// The out stream to the terminal.
    /// </summary>
    public TextWriter Out;
    /// <summary>
    /// Creates a target that targets the terminal.
    /// </summary>
    /// <param name="terminalOut">The out stream (default: <see cref="Terminal.Out"/>).</param>
    public TerminalTarget(TextWriter? terminalOut = null) {
        Out = terminalOut ?? Terminal.Out;
    }
    /// <inheritdoc/>
    public void Dispose() {
        GC.SuppressFinalize(this);
    }

    /// <inheritdoc/>
    public void Write<T>(Severity severity, DateTime time, Logger logger, T? text) {
        Out.WriteLine(Logger.GetColor(severity)+"["+logger.Name+"]["+time.ToString()+"]["+ANSI.Styles.Bold+severity.ToString()+ANSI.Styles.ResetBold+"]: "+text?.ToString()+ANSI.Styles.ResetAll);
    }
}

/// <summary>
/// A Logger Target for a log file.
/// </summary>
public class FileTarget : ITarget
{
    /// <summary>
    /// The output stream to the file.
    /// </summary>
    public TextWriter FileOut;

    /// <summary>
    /// Creates a target that targets a log file.
    /// </summary>
    /// <param name="path">The path to the log file (file doesn't need to exist).</param>
    public FileTarget(string path) {
        FileOut = new StreamWriter(File.OpenWrite(path));
    }
    /// <inheritdoc/>
    public void Dispose() {
        FileOut.Close();
        GC.SuppressFinalize(this);
    }

    /// <inheritdoc/>
    public void Write<T>(Severity severity, DateTime time, Logger logger, T? text) {
        FileOut.WriteLine("["+logger.Name+"]["+time+"]["+severity.ToString()+"]: "+text?.ToString());
    }
}