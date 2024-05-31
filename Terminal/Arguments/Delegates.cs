namespace OxDED.Terminal.Arguments;

public delegate void ArgumentCallback(Argument argument);
public delegate void PositionalArgumentCallback(PositionalArgument argument);
public delegate void InvalidFormatCallback(string message);