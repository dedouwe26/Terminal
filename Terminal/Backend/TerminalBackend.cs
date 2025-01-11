using System.Text;

namespace OxDED.Terminal.Backend;

/// <summary>
/// Represents an interface of common methods of a terminal.
/// </summary>
public interface ITerminalBackend : IDisposable {
    /// <summary>
    /// The data stream for reading from the terminal.
    /// </summary>
    public TextReader StandardInput { get; }
    /// <summary>
    /// The data stream for writing to the terminal.
    /// </summary>
    public TextWriter StandardOutput { get; }
    /// <summary>
    /// The data stream for writing errors to the terminal.
    /// </summary>
    public TextWriter StandardError { get; }
    /// <summary>
    /// The encoding used for the <see cref="StandardInput"/> stream (default: UTF-8).
    /// </summary>
    public Encoding InputEncoding { get; set; }
    /// <summary>
    /// The encoding used for the <see cref="StandardOutput"/> stream (default: UTF-8).
    /// </summary>
    public Encoding OutputEncoding { get; set; }
    /// <summary>
    /// The encoding used for the <see cref="StandardError"/> stream (default: UTF-8).
    /// </summary>
    public Encoding ErrorEncoding { get; set; }
    /// <summary>
    /// Hides the cursor in the terminal.
    /// </summary>
    public bool HideCursor { get; set; }
    /// <summary>
    /// True if this backend should listen for keypresses.
    /// </summary>
    public bool ListenForKeys { get; set; }
    /// <summary>
    /// An event for when a key is pressed.
    /// </summary>
    public static event KeyPressCallback? OnKeyPress;
    /// <summary>
    /// The width and the height (in characters) of the terminal.
    /// </summary>
    public (int Width, int Height) Size { get; set; }
    /// <summary>
    /// The position of the cursor in the terminal.
    /// </summary>
    public (int X, int Y) CursorPosition { get; set; }
    /// <summary>
    /// Waits until a key is pressed.
    /// </summary>
    public void WaitForKeyPress();
    /// <summary>
    /// Method in new thread that should call <see cref="OnKeyPress"/> when a key is pressed.
    /// </summary>
    protected void ListenForKeysMethod();
    /// <summary>
    /// Clears (resets) the whole screen.
    /// </summary>
    public void Clear();
    /// <summary>
    /// Clears screen from the position to end of the screen.
    /// </summary>
    /// <param name="start">The start position.</param>
    public void ClearFrom((int x, int y) start);
    /// <summary>
    /// Clears (deletes) a line.
    /// </summary>
    /// <param name="line">The y-axis of the line.</param>
    public void ClearLine(int line);
    /// <summary>
    /// Clears the line from the position to the end of the line.
    /// </summary>
    /// <param name="start">The start position.</param>
    public void ClearLineFrom((int x, int y) start);

}

/// <summary>
/// Represents an interface of common methods of a terminal.
/// </summary>
public abstract class TerminalBackend : ITerminalBackend {
    /// <inheritdoc/>
    public abstract TextReader StandardInput { get; }

    /// <inheritdoc/>
    public abstract TextWriter StandardOutput { get; }

    /// <inheritdoc/>
    public abstract TextWriter StandardError { get; }

    /// <inheritdoc/>
    public abstract Encoding InputEncoding { get; set; }

    /// <inheritdoc/>
    public abstract Encoding OutputEncoding { get; set; }

    /// <inheritdoc/>
    /// <remarks>
    /// Defaults to setting and getting the <see cref="OutputEncoding"/>.
    /// </remarks>
    public virtual Encoding ErrorEncoding { get { return OutputEncoding; } set { OutputEncoding = value; } }

    /// <inheritdoc/>
    public virtual (int Width, int Height) Size { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    
    /// <inheritdoc/>
    public abstract void Dispose();

    
    /// <summary>
    /// Writes something (<see cref="object.ToString"/>) to the terminal, with a style.
    /// </summary>
    /// <typeparam name="T">The type of what to write (<see cref="object.ToString"/>).</typeparam>
    /// <param name="text">The thing to write to the terminal.</param>
    /// <param name="style">The text decoration to use.</param>
    public virtual void Write<T>(T? text, Style? style = null) {
        if (text==null) return;
        StandardOutput.Write((style ?? new Style()).ToANSI()+text?.ToString()+ANSI.Styles.ResetAll);
    }
    /// <summary>
    /// Writes something (<see cref="object.ToString"/>) to the terminal, with a style.
    /// </summary>
    /// <typeparam name="T">The type of what to write (<see cref="object.ToString"/>).</typeparam>
    /// <param name="text">The thing to write to the terminal.</param>
    /// <param name="style">The text decoration to use.</param>
    public virtual void WriteLine<T>(T? text, Style? style = null) {
        if (text==null) return;
        StandardOutput.WriteLine((style ?? new Style()).ToANSI()+text?.ToString()+ANSI.Styles.ResetAll);
    }
    /// <summary>
    /// Writes something (<see cref="object.ToString"/>) to the error stream, with a style.
    /// </summary>
    /// <typeparam name="T">The type of what to write (<see cref="object.ToString"/>).</typeparam>
    /// <param name="text">The text to write to the error output stream.</param>
    /// <param name="style">The style to use (default: with red foreground).</param>
    public virtual void WriteErrorLine<T>(T? text, Style? style = null) {
        if (text==null) return;
        StandardError.WriteLine((style ?? new Style {ForegroundColor = Colors.Red}).ToANSI()+text?.ToString()+ANSI.Styles.ResetAll);
    }
    /// <summary>
    /// Writes something (<see cref="object.ToString"/>) to the error stream, with a style.
    /// </summary>
    /// <typeparam name="T">The type of what to write (<see cref="object.ToString"/>).</typeparam>
    /// <param name="text">The text to write to the error output stream.</param>
    /// <param name="style">The style to use (default: with red foreground).</param>
    public virtual void WriteError<T>(T? text, Style? style = null) {
        if (text==null) return;
        StandardError.Write((style ?? new Style {ForegroundColor = Colors.Red}).ToANSI()+text?.ToString()+ANSI.Styles.ResetAll);
    }
    /// <summary>
    /// Sets the something (<see cref="object.ToString"/>) at a <paramref name="pos"/>, with a <paramref name="style"/>.
    /// </summary>
    /// <typeparam name="T">The type of what to write.</typeparam>
    /// <param name="text">The thing to set at <paramref name="pos"/> to the terminal.</param>
    /// <param name="pos">The position to set <paramref name="text"/> at.</param>
    /// <param name="style">The text decoration to use.</param>
    public virtual void Set<T>(T? text, (int x, int y) pos, Style? style = null) {
        CursorPosition = pos;
        Write(text, style);
    }

    /// <summary>
    /// Sets the something in the error stream (<see cref="object.ToString"/>) at a <paramref name="pos"/>, with a <paramref name="style"/>.
    /// </summary>
    /// <typeparam name="T">The type of what to write.</typeparam>
    /// <param name="text">The thing to set at <paramref name="pos"/> to the terminal.</param>
    /// <param name="pos">The position to set <paramref name="text"/> at.</param>
    /// <param name="style">The text decoration to use.</param>
    public virtual void SetError<T>(T? text, (int x, int y) pos, Style? style = null) {
        CursorPosition = pos;
        WriteError(text, style);
    }
    /// <summary>
    /// Reads one character from the input stream.
    /// </summary>
    /// <returns>The character that has been read (-1 if everything has been read).</returns>
    public virtual int Read() {
        return StandardInput.Read();
    }
    /// <summary>
    /// Reads a line from the input stream.
    /// </summary>
    /// <returns>The line that has been read (null if everything has been read).</returns>
    public virtual string? ReadLine() {
        return StandardInput.ReadLine();
    }
    /// <summary>
    /// Waits until a key is pressed.
    /// </summary>
    public abstract void WaitForKeyPress();
    /// <summary>
    /// An event for when a key is pressed.
    /// </summary>
    public event KeyPressCallback? OnKeyPress;
    /// <summary>
    /// Raises an on keypress event.
    /// </summary>
    /// <param name="key">The key that is pressed.</param>
    /// <param name="keyChar">The corresponding char of the key (shift is used).</param>
    /// <param name="alt">If the alt key was pressed.</param>
    /// <param name="shift">If the shift key was pressed.</param>
    /// <param name="control">If the control key was pressed.</param>
    protected void KeyPress(ConsoleKey key, char keyChar, bool alt, bool shift, bool control) {
        OnKeyPress?.Invoke(key, keyChar, alt, shift, control);
    }

    /// <summary>
    /// Clears (resets) the whole screen.
    /// </summary>
    public virtual void Clear() {
        CursorPosition = (0, 0);
        StandardOutput.Write(ANSI.EraseScreenFromCursor);
    }
    /// <summary>
    /// Clears screen from the position to end of the screen.
    /// </summary>
    /// <param name="start">The start position.</param>
    public virtual void ClearFrom((int x, int y) start) {
        CursorPosition = start;
        StandardOutput.Write(ANSI.EraseLineFromCursor);
    }
    /// <summary>
    /// Clears (deletes) a line.
    /// </summary>
    /// <param name="line">The y-axis of the line.</param>
    public virtual void ClearLine(int line) {
        CursorPosition = (0, line);
        StandardOutput.Write(ANSI.EraseLine);
    }
    /// <summary>
    /// Clears the line from the position to the end of the line.
    /// </summary>
    /// <param name="start">The start position.</param>
    public virtual void ClearLineFrom((int x, int y) start) {
        CursorPosition = start;
        StandardOutput.Write(ANSI.EraseLineFromCursor);
    }
    /// <summary>
    /// The thread that is running <see cref="ListenForKeysMethod"/>.
    /// </summary>
    protected Thread? listenForKeysThread;
    /// <summary>
    /// If this window is currently listening to keys.
    /// </summary>
    protected bool listenForKeys = false;
    private (int X, int Y) CursorPosition { get; set; }

    /// <summary>
    /// If it should listen for keys.
    /// </summary>
    public virtual bool ListenForKeys {set {
        if (value && (!listenForKeys)) {
            listenForKeys = value;
            listenForKeysThread = new Thread(ListenForKeysMethod);
            listenForKeysThread.Start();
        } else {
            listenForKeys = value;
        }
    } get {
        return listenForKeys;
    }}

    public bool HideCursor { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

    /// <summary>
    /// Method in new thread that should call <see cref="OnKeyPress"/> when a key is pressed.
    /// </summary>
    protected abstract void ListenForKeysMethod();
}