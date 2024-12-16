namespace OxDED.Terminal.Logging;

/// <summary>
/// A Logger Target for the terminal.
/// </summary>
public class TerminalTarget : ITarget {
    /// <summary>
    /// The out stream to the terminal.
    /// </summary>
    public TextWriter Out;
    /// <summary>
    /// The error stream to the terminal.
    /// </summary>
    public TextWriter Error;
    /// <summary>
    /// The format to use for writing to the terminal (0: name see <see cref="NameFormat"/>, 1: logger ID, 2: time, 3: severity, 4: message, 5: color ANSI).
    /// </summary>
    /// <remarks>
    /// Default:
    /// <c>{5}[{0}][{2}][BOLD{3}RESETBOLD]: {4}RESETALL</c>
    /// </remarks>
    public string Format = "{5}[{0}][{2}]["+ANSI.Styles.Bold+"{3}"+ANSI.Styles.ResetBold+"]: {4}"+ANSI.Styles.ResetAll;
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
    /// The colors of the severities (index: 0: Fatal, 1: Error, 2: Warning, 3: Message, 4: Info, 5: Debug, 6: Trace).
    /// </summary>
    public readonly List<Color> SeverityColors = [new Color(255, 0, 0), new Color(255, 80, 80), new Color(255, 255, 0), ((Color)Colors.White), new Color(180, 180, 180), new Color(255,160,0), new Color(20, 200, 20)];
    /// <summary>
    /// Creates a target that targets the terminal.
    /// </summary>
    /// <param name="format">The format to write to the terminal (default, more info: <see cref="Format"/>).</param>
    /// <param name="terminalOut">The out stream (default: <see cref="Terminal.Out"/>).</param>
    /// <param name="terminalError">The error stream (default: <see cref="Terminal.Error"/>).</param>
    public TerminalTarget(string? format = null, TextWriter? terminalOut = null, TextWriter? terminalError = null) {
        if (format != null) {
            Format = format;
        }
        Out = terminalOut ?? Terminal.Out;
        Error = terminalError ?? Terminal.Error;
    }
    /// <inheritdoc/>
    public void Dispose() {
        GC.SuppressFinalize(this);
    }

    private string GetName(Logger logger) {
        if (!logger.IsSubLogger) {
            return logger.Name;
        } else {
            return string.Format(NameFormat, GetName((logger as SubLogger)!.ParentLogger), logger.Name);
        }
    }

    /// <inheritdoc/>
    /// <remarks>
    /// Writes a line.
    /// </remarks>
    public void Write<T>(Severity severity, DateTime time, Logger logger, T? text) {
        if (((byte)severity) < 2) {
            Error.WriteLine(string.Format(Format, GetName(logger), logger.ID, time.ToString(), severity.ToString(), text?.ToString()??"", SeverityColors[(byte)severity].ToForegroundANSI()));
        } else {
            Out.WriteLine(string.Format(Format, GetName(logger), logger.ID, time.ToString(), severity.ToString(), text?.ToString()??"", SeverityColors[(byte)severity].ToForegroundANSI()));
        }
        
    }
}