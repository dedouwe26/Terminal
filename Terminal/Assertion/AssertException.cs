namespace OxDED.Terminal.Assertion;

/// <summary>
/// Represents an exception thrown when an assertion failed.
/// </summary>
[Serializable]
public class AssertException<T> : Exception where T : Assertion {
    /// <summary>
    /// Creates a new assertion exception without any message.
    /// </summary>
    public AssertException() { }
    /// <summary>
    /// Creates a new assertion exception with a message.
    /// </summary>
    /// <param name="message">The message of this exception.</param>
    public AssertException(string message) : base(message) { }
    /// <summary>
    /// Creates a new assertion exception with a message and result.
    /// </summary>
    /// <param name="message">The message of this exception.</param>
    /// <param name="assertion">The assertion that occured.</param>
    public AssertException(string message, T assertion) : base(message+$" ({assertion})") { }
}