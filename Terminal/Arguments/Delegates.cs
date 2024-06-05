namespace OxDED.Terminal.Arguments;

/// <summary>
/// An callback for when an argument has been parsed.
/// </summary>
/// <param name="argument">The argument that has been parsed.</param>
public delegate void ArgumentCallback(Argument argument);
/// <summary>
/// An callback for when an postitional argument has been parsed.
/// </summary>
/// <param name="argument">The positional argument that has been parsed.</param>
public delegate void PositionalArgumentCallback(PositionalArgument argument);
/// <summary>
/// An callback for when the parsing has failed.
/// </summary>
/// <param name="message">The message to describe what failed.</param>
public delegate void InvalidFormatCallback(string message);