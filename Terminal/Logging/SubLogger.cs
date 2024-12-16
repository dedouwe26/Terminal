namespace OxDED.Terminal.Logging;

/// <summary>
/// Represents a child of another logger.
/// </summary>
public class SubLogger : Logger {
    /// <summary>
    /// The parent logger if this logger is a sub logger.
    /// </summary>
    public Logger ParentLogger { get; private set; }

    /// <summary>
    /// The ID of the child.
    /// </summary>
    public readonly string childID;

    internal SubLogger(Logger parentLogger, string id = "Sublogger", string name = "Sublogger", string? registeredId = null, Severity severity = Severity.Info, Dictionary<Type, ITarget>? targets = null) : base(name, registeredId, severity, targets) {
        ParentLogger = parentLogger;
        childID = id;
    }
}