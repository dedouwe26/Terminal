using OxDED.Terminal.Backend;

namespace OxDED.Terminal.Window;

/// <summary>
/// Represents a terminal window.
/// </summary>
public interface IWindow : ITerminalBackend {
    /// <summary>
    /// The title of the terminal window.
    /// </summary>
    public string Title { get; set; }
}