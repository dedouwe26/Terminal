namespace OxDED.Terminal.Logging;

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