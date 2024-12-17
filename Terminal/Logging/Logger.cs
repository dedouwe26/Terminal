using System.Collections.ObjectModel;
using OxDED.Terminal.Logging.Targets;

namespace OxDED.Terminal.Logging;

/// <summary>
/// Represents a logger for the terminal and log files.
/// </summary>
public class Logger : IDisposable, IEquatable<Logger> {
    /// <summary>
    /// The ID of this logger.
    /// </summary>
    public readonly string? ID;
    /// <summary>
    /// The name of this logger.
    /// </summary>
    public readonly string Name;

    /// <summary>
    /// Checks if this is a sub logger.
    /// </summary>
    public bool IsSubLogger => this is SubLogger;

    /// <summary>
    /// All the sub loggers of this logger.
    /// </summary>
    public ReadOnlyDictionary<string, Logger> SubLoggers { get { return subLoggers.AsReadOnly(); } }

    private readonly Dictionary<string, Logger> subLoggers = [];

    /// <summary>
    /// All the targets, key MUST BE typeof(...Target) or ITarget.GetType() only when using.
    /// </summary>
    public Dictionary<Type, (ITarget target, bool enabled)> Targets;
    /// <summary>
    /// The current log severity max.
    /// </summary>
    public Severity logLevel = Severity.Info;
    /// <summary>
    /// An event for when something is logged.
    /// </summary>
    public event LogCallback? OnLog;


    /// <summary>
    /// Creates a logger.
    /// </summary>
    /// <param name="id">The optional ID to identify this logger, like 'me.0xDED.MyProject'. It won't register if the ID is null (if this ID is already registered it will throw an <see cref="ArgumentException"/> error).</param>
    /// <param name="name">The name of the logger, used in the log files and terminal.</param>
    /// <param name="severity">The log level of this logger.</param>
    /// <param name="targets">The targets to add and enable (default: <see cref="TerminalTarget"/>, <see cref="FileTarget"/> with path "./latest.log").</param>
    /// <exception cref="ArgumentException"/>
    public Logger(string name = "Logger", string? id = null, Severity severity = Severity.Info, Dictionary<Type, ITarget>? targets = null) {
        ID = id;
        Name = name;
        logLevel = severity;
        Targets = targets != null ? targets.Select(target => new KeyValuePair<Type, (ITarget, bool)>(target.Key, (target.Value, true))).ToDictionary() : new Dictionary<Type, (ITarget, bool enabled)> { { typeof(TerminalTarget), (new TerminalTarget(), true) }, { typeof(FileTarget), (new FileTarget("./latest.log"), true) } };
        if (id != null) {
            if (!Loggers.Register(this)) {
                throw new ArgumentException("A logger with this ID has already been registered.", nameof(id));
            }
        }
        
    }

    /// <summary>
    /// Checks if this logger is registered.
    /// </summary>
    /// <returns></returns>
    public bool IsRegistered() {
        return ID is not null;
    }

    /// <summary>
    /// Sets the log level.
    /// </summary>
    /// <param name="maxSeverity">The highest log level to log.</param>
    public void SetLevel(Severity maxSeverity) {
        logLevel = maxSeverity;
    }

    /// <summary>
    /// Checks if this logger has a target of that type.
    /// </summary>
    /// <param name="type">The target type.</param>
    /// <returns>True if it has a target of that type.</returns>
    public bool HasTarget(Type type) {
        return Targets.ContainsKey(type);
    }

    /// <summary>
    /// Checks if this logger has a target of that type.
    /// </summary>
    /// <typeparam name="T">The type of the target.</typeparam>
    /// <returns>True if it has a target of that type.</returns>
    public bool HasTarget<T>() where T : ITarget {
        return HasTarget(typeof(T));
    }

    /// <summary>
    /// Gets the target from type.
    /// </summary>
    /// <param name="type">The target type.</param>
    /// <returns>The target if there is one.</returns>
    public ITarget? GetTarget(Type type) {
        if (!Targets.TryGetValue(type, out (ITarget target, bool enabled) target)) {
            return null;
        }
        return target.target;
    }
    /// <summary>
    /// Gets a target.
    /// </summary>
    /// <typeparam name="T">The type of the target.</typeparam>
    /// <returns>The target if there is one.</returns>
    /// <exception cref="ArgumentException"/>
    public T GetTarget<T>() where T : ITarget {
        ITarget? target = GetTarget(typeof(T)) ?? throw new ArgumentException("No target found.", nameof(T));
        return (T)target!;
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
    /// Checks if this logger has a sub logger with that ID.
    /// </summary>
    /// <param name="childID">The child ID of the sub logger.</param>
    /// <returns>True if this logger has a sub logger with that ID.</returns>
    public bool HasSubLogger(string childID) {
        return subLoggers.ContainsKey(childID);
    }
    /// <summary>
    /// Checks if this logger has this logger as a sub logger.
    /// </summary>
    /// <param name="logger">The sub logger.</param>
    /// <returns>True if this logger has that sub logger.</returns>
    public bool HasSubLogger(SubLogger logger) {
        return ReferenceEquals(this, logger.ParentLogger);
    }

    /// <summary>
    /// Gets the sub logger from an ID.
    /// </summary>
    /// <remarks>
    /// Please use <see cref="Loggers.Get(string)"/>, if you want to get a logger with that ID.
    /// Or use <see cref="HasSubLogger(string)"/> if you want to know if it has a sub logger with that ID.
    /// </remarks>
    /// <param name="childID">The child ID of the sub logger.</param>
    /// <returns>The sub logger.</returns>
    public Logger? GetSubLogger(string childID) {
        subLoggers.TryGetValue(childID, out Logger? logger);
        return logger;
    }
    /// <summary>
    /// Logs something (<see cref="object.ToString"/>).
    /// </summary>
    /// <typeparam name="T">The type of the data.</typeparam>
    /// <param name="severity">The severity of the text.</param>
    /// <param name="text">The text to write (<see cref="object.ToString"/>).</param>
    public void Log<T>(Severity severity, T? text) {
        DateTime time = DateTime.Now;
        OnLog?.Invoke(this, text?.ToString()??"", severity, time);
        if (((byte)severity) > ((byte)logLevel)) { return; }
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
    /// Creates a sub logger.
    /// </summary>
    /// <param name="name">The sub name of the logger.</param>
    /// <param name="id">The sub ID. Full ID will be: parent ID + '.' + subID = child ID).</param>
    /// <param name="shouldRegister">If the sublogger should be registered, if the parent logger is also registered.</param>
    /// <param name="severity">The log level of the new sub logger.</param>
    /// <param name="targets">The targets of the new sub logger (default: Targets of parent).</param>
    /// <returns>The created sub logger.</returns>
    /// <exception cref="ArgumentException" />
    public SubLogger CreateSubLogger(string id, string name = "Sublogger", bool shouldRegister = true, Severity severity = Severity.Info, Dictionary<Type, ITarget>? targets = null) {
        SubLogger subLogger = new(this, id, name, (ID != null && shouldRegister) ? ID+'.'+id : null, severity, targets);
        subLoggers.Add(id, subLogger);
        return subLogger;
    }

    /// <inheritdoc/>
    /// <remarks>
    /// Disposes all targets, and unregisters this logger.
    /// </remarks>
    public void Dispose() {
        Loggers.UnRegister(this);
        foreach (KeyValuePair<Type, (ITarget target, bool enabled)> target in Targets) {
            target.Value.target.Dispose();
        }

        foreach (KeyValuePair<string, Logger> subLogger in subLoggers) {
            subLogger.Value.Dispose();
        }

        GC.SuppressFinalize(this);
    }

    /// 
    public static bool operator ==(Logger? left, Logger? right) {
        if (left is null && right is null) {
            return true;
        } else if (left is null) {
            return false;
        }
        return left.Equals(right);
    }
    /// 
    public static bool operator !=(Logger? left, Logger? right) {
        return !(left == right);
    }
    /// <inheritdoc/>
    /// <remarks>
    /// Checks if the that color is identical to this one.
    /// </remarks>
    public bool Equals(Logger? other) {
        if (other is null) {
            return false;
        }
        if (ReferenceEquals(this, other)) {
            return true;
        }
        if (GetType() != other.GetType()) {
            return false;
        }
        if (ID!=null) {
            return ID == other.ID;
        }
        return false;
    }
    /// <inheritdoc/>
    /// <remarks>
    /// Checks if the that color is identical to this one.
    /// </remarks>
    public override bool Equals(object? obj) {
        return Equals(obj as Color);
    }
    /// <inheritdoc/>
    public override int GetHashCode() {
        return ID == null ? Name.GetHashCode() ^ subLoggers.GetHashCode() ^ Targets.GetHashCode() : ID.GetHashCode();
    }
}