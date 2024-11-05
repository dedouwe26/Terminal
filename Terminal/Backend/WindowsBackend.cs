using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Text;
using Microsoft.Win32.SafeHandles;

namespace OxDED.Terminal.Backend;

internal static class Utility {
    internal static Stream GetStream(WinAPI.StandardType type) {
        SafeFileHandle fileHandle = new(WinAPI.GetStdHandle((int)type), false);

        if (fileHandle.IsInvalid) {
            fileHandle.SetHandleAsInvalid();
            return Stream.Null;
        }

        FileStream stream = new(fileHandle, type != WinAPI.StandardType.Input ? FileAccess.Write : FileAccess.Read);

        return stream;
    }
    internal static WinAPI.CONSOLE_SCREEN_BUFFER_INFO GetBufferInfo(WindowsBackend backend) {
        if (backend.outHandle == WinAPI.INVALID_HANDLE_VALUE) {
            throw new Win32Exception("Invalid standard console output handle.");
        }

        bool succeeded = WinAPI.GetConsoleScreenBufferInfo(backend.outHandle, out WinAPI.CONSOLE_SCREEN_BUFFER_INFO csbi);
        if (!succeeded) {
            succeeded = WinAPI.GetConsoleScreenBufferInfo(backend.errHandle, out csbi);
            if (!succeeded)
                succeeded = WinAPI.GetConsoleScreenBufferInfo(backend.inHandle, out csbi);

            if (!succeeded) {
                int errorCode = Marshal.GetLastWin32Error();
                throw new Win32Exception(errorCode, "Tried to get the console screen buffer info.");
            }
        }

        return csbi;
    }
}

internal static partial class WinAPI {
    internal enum StandardType : int {
        Input = -10,
        Output = -11,
        Error = -12
    }
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    internal struct KEY_EVENT_RECORD {
        [MarshalAs(UnmanagedType.Bool)]
        internal bool bKeyDown;
        internal ushort wRepeatCount;
        internal ushort wVirtualKeyCode;
        internal ushort wVirtualScanCode;
        private ushort _uChar;
        internal uint dwControlKeyState;
        internal readonly char UChar => (char)_uChar;
    }
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    internal struct INPUT_RECORD {
        internal ushort EventType;
        internal KEY_EVENT_RECORD keyEvent;
    }
    [StructLayout(LayoutKind.Sequential)]
    internal struct COORD {
        internal short X;
        internal short Y;
    }
    [StructLayout(LayoutKind.Sequential)]
    internal struct SMALL_RECT {
        internal short Left; 
        internal short Top; 
        internal short Right; 
        internal short Bottom; 
    }
    [StructLayout(LayoutKind.Sequential)]
    internal struct CONSOLE_SCREEN_BUFFER_INFO {
        internal COORD dwSize; 
        internal COORD dwCursorPosition; 
        internal short wAttributes; 
        internal SMALL_RECT srWindow; 
        internal COORD dwMaximumWindowSize; 
    }
    internal const string KERNEL = "kernel32.dll";
    internal const nint INVALID_HANDLE_VALUE = -1;

    [LibraryImport(KERNEL, SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    internal static partial bool AllocConsole();

    [LibraryImport(KERNEL, SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    internal static partial bool FreeConsole();

    [LibraryImport(KERNEL)]
    internal static partial nint GetConsoleWindow();
    [LibraryImport(KERNEL, SetLastError = true)]
    internal static partial nint GetStdHandle(int nStdHandle);

    [LibraryImport(KERNEL, SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    internal static partial bool CloseHandle(nint handle);

    [LibraryImport(KERNEL, SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    internal static partial bool SetStdHandle(int nStdHandle, nint hConsoleOutput);

    [DllImport(KERNEL, CharSet = CharSet.Auto, BestFitMapping = false, SetLastError = true)]
    internal static extern SafeFileHandle CreateFile(string fileName, uint desiredAccess, int shareMode, nint securityAttributes, int creationDisposition, int flagsAndAttributes, nint templateFile);
    
    [DllImport(KERNEL, SetLastError = true, BestFitMapping = true, CharSet = CharSet.Unicode)]
    [return: MarshalAs(UnmanagedType.Bool)]
    internal static extern bool SetConsoleTitle(string title);

    [DllImport(KERNEL, SetLastError = true, BestFitMapping = true, CharSet = CharSet.Unicode)]
    internal static extern uint GetConsoleTitle(out string lpConsoleTitle, uint nSize);

    [DllImport(KERNEL, CharSet=CharSet.Auto, SetLastError=true)]
    internal static extern bool ReadConsoleInput(nint hConsoleInput, out INPUT_RECORD buffer, int numInputRecords_UseOne, out int numEventsRead);
    
    [LibraryImport(KERNEL, SetLastError=true)]
    internal static partial uint GetConsoleOutputCP();

    [LibraryImport(KERNEL, SetLastError=true)]
    internal static partial uint GetConsoleCP();

    [LibraryImport(KERNEL, SetLastError=true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    internal static partial bool SetConsoleOutputCP(uint codePage);

    [LibraryImport(KERNEL, SetLastError=true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    internal static partial bool SetConsoleCP(uint codePage);

    [LibraryImport(KERNEL, SetLastError =true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    internal static partial bool GetConsoleScreenBufferInfo(nint hConsoleOutput, out CONSOLE_SCREEN_BUFFER_INFO lpConsoleScreenBufferInfo);
}

/// <summary>
/// A wrapper for <see cref="ITerminalBackend"/> on Windows.
/// </summary>
public class WindowsBackend : TerminalBackend {
    /// <summary>
    /// Creates a new Windows terminal window.
    /// </summary>
    /// <exception cref="Win32Exception"></exception>
    public WindowsBackend() {
        outEnc = Encoding.UTF8;
        inEnc = Encoding.UTF8;
        outHandle = WinAPI.GetStdHandle((int)WinAPI.StandardType.Output);
        inHandle = WinAPI.GetStdHandle((int)WinAPI.StandardType.Input);
        errHandle = WinAPI.GetStdHandle((int)WinAPI.StandardType.Error);
        TextWriter outStream = TextWriter.Synchronized(new StreamWriter(Utility.GetStream(WinAPI.StandardType.Output), outEnc, 256, true));
        TextReader inStream = TextReader.Synchronized(new StreamReader(Utility.GetStream(WinAPI.StandardType.Input), inEnc, false, 256, true));
        TextWriter errStream = TextWriter.Synchronized(new StreamWriter(Utility.GetStream(WinAPI.StandardType.Error), outEnc, 256, true));
    }
    private TextWriter outStream;
    private Encoding outEnc;
    private TextReader inStream;
    private Encoding inEnc;
    private TextWriter errStream;

    internal nint outHandle;
    internal nint inHandle;
    internal nint errHandle;

    /// <inheritdoc/>
    public override (int Width, int Height) Size {
        get {
            WinAPI.CONSOLE_SCREEN_BUFFER_INFO csbi = Utility.GetBufferInfo(this);
            return (csbi.srWindow.Right - csbi.srWindow.Left + 1, csbi.srWindow.Bottom - csbi.srWindow.Top + 1);
        } set {
            throw new NotImplementedException();
        }
    }

    /// <inheritdoc/>
    public override Encoding InputEncoding { get => inEnc; 
        set {
            if (inEnc == value) return;

            if (!WinAPI.SetConsoleCP((uint)value.CodePage)) {
                throw new Win32Exception("Failed to set console output code page.");
            }
            inEnc = value;
        }
    }
    /// <inheritdoc/>
    public override Encoding OutputEncoding { get => outEnc; 
        set {
            if (outEnc == value) return;

            outStream.Flush();
            errStream.Flush();

            if (!WinAPI.SetConsoleOutputCP((uint)value.CodePage)) {
                throw new Win32Exception("Failed to set console output code page.");
            }
            outEnc = value;
        }
    }
    /// <inheritdoc/>
    public override Encoding ErrorEncoding { get => outEnc; set => OutputEncoding = value; }

    /// <inheritdoc/>
    public override TextReader StandardInput => inStream;

    /// <inheritdoc/>
    public override TextWriter StandardOutput => outStream;

    /// <inheritdoc/>
    public override TextWriter StandardError => errStream;

    /// <summary>
    /// An event for when a key is released.
    /// </summary>
    public event KeyPressCallback? OnKeyRelease;
    
    // /// <inheritdoc/>
    // /// <exception cref="Win32Exception"></exception>
    // /// <remarks>Gets the first 300 chars of the title.</remarks>
    // public override string Title {
    //     get {
    //         _ = WinAPI.GetConsoleTitle(out string title, 300);
    //         return title;
    //     }
    //     set {
    //         if (!WinAPI.SetConsoleTitle(value)) {
    //             throw new Win32Exception("Failed to set the title: "+Marshal.GetLastWin32Error());
    //         }
    //     }
    // }

    /// <inheritdoc/>
    /// <exception cref="Win32Exception"></exception>
    public override void Dispose() {
        
    }

    /// <inheritdoc/>
    /// <exception cref="Win32Exception"></exception>
    protected override void ListenForKeysMethod() {
        while (listenForKeys) {
            if (!WinAPI.ReadConsoleInput(consoleIn, out WinAPI.INPUT_RECORD ev, 1, out int eventsRead)) {
                throw new Win32Exception("Failed to read console inputs: "+Marshal.GetLastWin32Error());
            }

            bool isKeyDown = ev.EventType == 0x0001 && ev.keyEvent.bKeyDown != false;
            char ch = ev.keyEvent.UChar;
            ushort keyCode = ev.keyEvent.wVirtualKeyCode;

            if (!isKeyDown) {
                if (keyCode != 0x12)
                    continue;
            }
            if (ch == 0) {
                if ((keyCode >= 0x10 && keyCode <= 0x12) || keyCode == 0x14 || keyCode == 0x90 || keyCode == 0x91)
                    continue;
            }
            ControlKeyState state = (ControlKeyState)ev.keyEvent.dwControlKeyState;
            bool shift = (state & ControlKeyState.ShiftPressed) != 0;
            bool alt = (state & (ControlKeyState.LeftAltPressed | ControlKeyState.RightAltPressed)) != 0;
            bool control = (state & (ControlKeyState.LeftCtrlPressed | ControlKeyState.RightCtrlPressed)) != 0;
            if (isKeyDown) {
                KeyPress((ConsoleKey)keyCode, ch, alt, shift, control);
            } else {
                OnKeyRelease?.Invoke((ConsoleKey)keyCode, ch, alt, shift, control);
            }
        }
    }
    /// <inheritdoc/>
    public override (int x, int y) GetCursorPosition() {
        throw new NotImplementedException();
    }
    /// <inheritdoc/>
    /// <exception cref="Win32Exception"></exception>
    public override void WaitForKeyPress() {
        if (!WinAPI.ReadConsoleInput(consoleIn, out _, 1, out _)) {
            throw new Win32Exception("Failed to read console inputs: "+Marshal.GetLastWin32Error());
        }
    }
}