namespace OxDED.Terminal.Logging;

/// <summary>
/// A Logger Target for a log file.
/// </summary>
public class FileTarget : ITarget {
    /// <summary>
    /// The output stream to the file.
    /// </summary>
    public readonly TextWriter FileOut;
    /// <summary>
    /// The format to use for writing to the terminal (0: logger name, 1: logger ID, 2: time, 3: severity, 4: message).
    /// </summary>
    /// <remarks>
    /// Default:
    /// <c>[{0}][{2}][{3}]: {4}</c>
    /// </remarks>
    public string Format = "[{0}][{2}][{3}]: {4}";

    /// <summary>
    /// The format to use for creating names (0: parent name, 1: own name). <para/>
    /// Example for default:
    /// <c>{{App}: Sublogger}: sub-sublogger</c>
    /// </summary>
    /// <remarks>
    /// default:
    /// <c>{0}: {1}</c>
    /// </remarks>
    public string NameFormat = "{0}: {1}";

    /// <summary>
    /// Creates a target that targets a log file.
    /// </summary>
    /// <param name="path">The path to the log file (file doesn't need to exist).</param>
    /// <param name="format">The format to use for writing to a file ().</param>
    public FileTarget(string path, string? format = null) {
        if (format != null) {
            Format = format;
        }
        FileOut = new StreamWriter(File.OpenWrite(path));
    }
    /// <inheritdoc/>
    public void Dispose() {
        FileOut.Close();
        GC.SuppressFinalize(this);
    }

    private string GetName(Logger logger) {
        if (!logger.IsSubLogger) {
            return logger.Name;
        } else {
            return string.Format(NameFormat, GetName(logger.ParentLogger!), logger.Name);
        }
    }

    /// <inheritdoc/>
    /// <remarks>
    /// Writes a line.
    /// </remarks>
    public void Write<T>(Severity severity, DateTime time, Logger logger, T? text) {
        FileOut.WriteLine(string.Format(Format, GetName(logger), logger.ID, time.ToString(), severity.ToString(), text?.ToString()??""));
    }
}