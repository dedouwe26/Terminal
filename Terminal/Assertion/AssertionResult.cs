namespace OxDED.Terminal.Assertion;

/// <summary>
/// The possible results of an assertion.
/// </summary>
public enum AssertionResult {
    /// <summary>
    /// The assertion succceeded.
    /// </summary>
    Success,
    /// <summary>
    /// The value assertion failed.
    /// </summary>
    UnexpectedValue,
    /// <summary>
    /// The type assertion failed.
    /// </summary>
    UnexpectedType,
    /// <summary>
    /// The reference assertion failed.
    /// </summary>
    UnexpectedReference,
    /// <summary>
    /// The exception assertion failed.
    /// </summary>
    ExceptionCaught
}