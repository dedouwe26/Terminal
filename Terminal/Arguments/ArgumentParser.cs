namespace OxDED.Terminal.Arguments;

// TODO: add docs

/// <summary>
/// Helps you with parsing arguments.
/// </summary>
public class ArgumentParser
{
    /// <summary>
    /// This is the name of the application.
    /// </summary>
    public string? name = null;
    /// <summary>
    /// The description of the application.
    /// </summary>
    public string? description = null;
    private Argument? versionArgument = null;
    private Argument? helpArgument = null;
    /// <summary>
    /// The version of the application.
    /// </summary>
    public string? version = null;
    /// <summary>
    /// The arguments of the parser.
    /// </summary>
    public Dictionary<string, Argument> arguments = [];
    /// <summary>
    /// The positional arguments of the parser.
    /// </summary>
    public List<PositionalArgument> positionalArguments = [];
    /// <summary>
    /// Sets the help argument of the application parser.
    /// </summary>
    /// <param name="keys"></param>
    /// <param name="showDescription"></param>
    /// <param name="showVersion"></param>
    /// <param name="shouldExit"></param>
    public ArgumentParser Help(IEnumerable<string>? keys = null, bool showDescription = true, bool showVersion = false, bool shouldExit = true)
    {
        if (helpArgument != null)
        {
            RemoveArgument(helpArgument);
        }
        helpArgument = new Argument(keys ?? ["h", "help"], "Shows all the available arguments.");
        AddArgument(helpArgument, (Argument arg) =>
        {
            WriteHelp(showDescription, showVersion);
            if (shouldExit)
            {
                Environment.Exit(0);
            }
        });
        return this;
    }
    /// <summary>
    /// Sets the version of the application parser. And adds an argument.
    /// </summary>
    /// <param name="keys"></param>
    
    public ArgumentParser Version(string version, IEnumerable<string>? keys = null, bool shouldExit = true)
    {
        if (versionArgument != null)
        {
            RemoveArgument(versionArgument);
        }
        versionArgument = new Argument(keys ?? ["v", "version"], name == null ? "Shows the version of this application." : $"Shows the version of {name}."); // TODO: add documentation to first assign a name
        AddArgument(versionArgument, (Argument arg) =>
        {
            WriteVersion();
            if (shouldExit)
            {
                Environment.Exit(0);
            }
        });
        this.version = version;
        return this;
    }
    /// <summary>
    /// Sets the description of the application parser.
    /// </summary>
    public ArgumentParser Description(string? description)
    {
        this.description = description;
        return this;
    }
    /// <summary>
    /// Sets the name of the application parser.
    /// </summary>
    public ArgumentParser Name(string? name)
    {
        this.name = name;
        return this;
    }
    /// <summary>
    /// Removes a positional argument.
    /// </summary>
    public ArgumentParser RemovePositionalArgument(int position) {
        positionalArguments.RemoveAt(position);
        return this;
    }
    /// <summary>
    /// Removes a positional argument.
    /// </summary>
    public ArgumentParser RemovePositionalArgument(PositionalArgument argument) {
        positionalArguments.Remove(argument);
        return this;
    }
    /// <summary>
    /// Removes an argument.
    /// </summary>
    public ArgumentParser RemoveArgument(Argument argument)
    {
        foreach (string key in argument.keys)
        {
            arguments.Remove(key);
        }
        return this;
    }
    /// <summary>
    /// Adds an argument.
    /// </summary>
    public ArgumentParser AddArgument(Argument argument, ArgumentCallback? callback = null)
    {
        foreach (string key in argument.keys)
        {
            arguments.Add(key, argument);
        }
        if (callback != null)
        {
            OnArgument += (Argument arg) =>
            {
                if (arg.keys == argument.keys)
                {
                    callback?.Invoke(arg);
                }
            };
        }
        return this;
    }
    /// <summary>
    /// Adds a positional argument.
    /// </summary>
    public ArgumentParser AddPositionalArgument(PositionalArgument argument, PositionalArgumentCallback? callback = null)
    {
        positionalArguments.Add(argument);
        if (callback != null)
        {
            OnPositionalArgument += (PositionalArgument arg) =>
            {
                if (arg.name == argument.name)
                {
                    callback?.Invoke(arg);
                }
            };
        }
        return this;
    }
    /// <summary>
    /// Writes the help menu to the terminal.
    /// </summary>
    public void WriteHelp(bool showDescription = true, bool showVersion = false)
    {
        Terminal.WriteLine(GetHelp(showDescription, showVersion));
    }
    /// <summary>
    /// Writes the version to the terminal.
    /// </summary>
    public void WriteVersion()
    {
        Terminal.WriteLine(GetVersion());
    }
    private static string GetArgumentHelp(PositionalArgument arg, bool isRed = false)
    {
        return $"{(isRed ? Color.LightRed.ToForegroundANSI() : Color.Orange.ToForegroundANSI())}\u2520{ANSI.Styles.ResetAll}  {arg.name}: {arg.description}";
    }
    private static string GetArgumentHelpName(string[] keys)
    {
        string result = "";
        for (int i = 0; i < keys.Length; i++)
        {
            string key = keys[i];
            result += key.Length == 1 ? "-" : "--";
            result += key;
            if (i != keys.Length - 1)
            {
                result += ", ";
            }
        }
        return result;
    }
    private static string GetArgumentHelp(Argument arg)
    {
        string result = $"{Color.DarkGreen.ToForegroundANSI()}\u2520{ANSI.Styles.ResetAll}  {GetArgumentHelpName(arg.keys)}: {arg.description}\n";
        foreach (ArgumentParameter para in arg.parameters)
        {
            result += $"{Color.DarkGreen.ToForegroundANSI()}\u2503  \u2560{ANSI.Styles.ResetAll} {para.name}: {para.description}\n";
        }
        return result;
    }
    private string GetHelp(bool showDescription = true, bool showVersion = false, int positionalIndex = -1)
    {
        string result = $"{ANSI.Styles.Bold}{name}{ANSI.Styles.ResetBold} {((showVersion && version != null) ? version : "")}{((showDescription && description != null) ? '\n' + description : "")}\n\n";

        if (positionalArguments.Count > 0)
        {
            result += $"{Color.Orange.ToForegroundANSI()}\u250E\u2500\u2500{ANSI.Styles.ResetAll} Required Arguments\n";
            for (int i = 0; i < positionalArguments.Count; i++)
            {
                PositionalArgument arg = positionalArguments[i];
                if (positionalIndex <= i && positionalIndex != -1)
                {
                    result += GetArgumentHelp(arg, true);
                }
                else
                {
                    result += GetArgumentHelp(arg, false);
                }

            }
            result += "\n\n";
        }
        if (arguments.Count > 0 || helpArgument != null || versionArgument != null)
        {
            result += $"{Color.DarkGreen.ToForegroundANSI()}\u250E\u2500\u2500{ANSI.Styles.ResetAll} Arguments\n";
            if (helpArgument != null)
            {
                result += $"\u2520  {GetArgumentHelpName(helpArgument.keys)}: {helpArgument.description}\n";
            }
            if (versionArgument != null)
            {
                result += $"\u2520  {GetArgumentHelpName(versionArgument.keys)}: {versionArgument.description}\n";
            }
            if (arguments.Count > 0)
            {
                foreach (Argument argument in arguments.Values.Distinct())
                {
                    if (argument == versionArgument || argument == helpArgument)
                    {
                        continue;
                    }
                    result += GetArgumentHelp(argument);
                }
            }
        }
        return result;
    }
    private string GetVersion()
    {
        return $"{ANSI.Styles.Bold}{name}{ANSI.Styles.ResetBold} {version ?? ""}{(description != null ? '\n' + description : "")}";
    }
    /// <summary>
    /// An event that is called when an argument is parsed.
    /// </summary>
    public event ArgumentCallback? OnArgument;
    /// <summary>
    /// An event that is called when a positional argument is parsed.
    /// </summary>
    public event PositionalArgumentCallback? OnPositionalArgument;
    /// <summary>
    /// An event that is called when the format is invalid.
    /// </summary>
    public event InvalidFormatCallback? OnInvalidFormatCallback;
    private void WriteInvalid(string message)
    {
        WriteHelp(false);
        OnInvalidFormatCallback?.Invoke(message);
        Terminal.WriteLine("\n" + message, new Style { ForegroundColor = Color.Red });
        Environment.Exit(1);
    }
    private void WriteNoArgument(string message, int positionalIndex)
    {
        Terminal.WriteLine(GetHelp(false, false, positionalIndex));
        OnInvalidFormatCallback?.Invoke(message);
        Terminal.WriteLine("\n" + message, new Style { ForegroundColor = Color.Red });
        Environment.Exit(1);
    }
    /// <summary>
    /// Parses the arguments.
    /// </summary>
    public bool Parse(string arguments)
    {
        return Parse(arguments.Split(' '));
    }
    /// <summary>
    /// Parses the arguments.
    /// </summary>
    public bool Parse(string[] arguments)
    {
        int positionalArgumentIndex = 0;
        Argument? parsingArgument = null;
        List<string> parameters = [];
        bool isParsingArgument = false;
        foreach (string argument in arguments)
        {
            if (!isParsingArgument)
            {
                if (argument.StartsWith("--") && argument.Length > 2)
                {
                    parsingArgument = GetArgument(argument[2..]);
                    if (parsingArgument == null)
                    {
                        WriteInvalid("No such argument as: --" + argument[2..] + ".");
                        return false;
                    }
                    else
                    {
                        isParsingArgument = true;
                    }
                }
                else if (argument.StartsWith('-') && argument.Length > 1)
                {
                    if (argument.Length >= 3)
                    {
                        WriteInvalid("Invalid symbol usage (-): " + argument + ".\n Should it be (--)?");
                    }
                    parsingArgument = GetArgument(argument[1].ToString());
                    if (parsingArgument == null)
                    {
                        WriteInvalid("No such argument as: -" + argument[1] + ".");
                        return false;
                    }
                    else
                    {
                        isParsingArgument = true;
                    }

                }
                else
                {
                    PositionalArgument? positionalArgument = GetPositionalArgument(positionalArgumentIndex);
                    if (positionalArgument != null)
                    {
                        positionalArgument.value = argument;
                        OnPositionalArgument?.Invoke(positionalArgument);
                        positionalArgumentIndex++;
                    }
                    else
                    {
                        WriteInvalid("Too much positional arguments.");
                        return false;
                    }
                }
            }
            if (isParsingArgument)
            {
                if (!(parameters.Count - 1 >= parsingArgument!.parameters.Length))
                {
                    parameters.Add(argument);
                }
                if (parameters.Count - 1 >= parsingArgument!.parameters.Length)
                {
                    for (int j = 0; j < parsingArgument!.parameters.Length; j++)
                    {
                        parsingArgument!.parameters[j].value = parameters[j + 1];
                    }
                    OnArgument?.Invoke(parsingArgument);
                    isParsingArgument = false;
                    parameters = [];
                    parsingArgument = null;
                }
            }
        }
        if (isParsingArgument)
        {
            WriteInvalid("Invalid argument parameters for " + parameters[0] + ".");
            return false;
        }
        if (positionalArguments.Count != positionalArgumentIndex)
        {
            WriteNoArgument("Not enough positional arguments.", positionalArgumentIndex);
            return false;
        }

        return true;
    }
    /// <summary>
    /// Gets an argument that is registered (null if it isn't registered).
    /// </summary>
    public Argument? GetArgument(string key)
    {
        if (!arguments.TryGetValue(key, out Argument? value))
        {
            return null;
        }
        return value;
    }
    /// <summary>
    /// Gets a positional argument at a location (in order as registered) (if there is one).
    /// </summary>
    public PositionalArgument? GetPositionalArgument(int pos)
    {
        return positionalArguments.Count > pos ? positionalArguments[pos] : null;
    }
    /// <summary>
    /// True if an argument is registered (not used).
    /// </summary>
    public bool HasArgument(string key)
    {
        return GetArgument(key) != null;
    }
    /// <summary>
    /// Gets all the registered arguments (not used).
    /// </summary>
    public Argument[] GetArguments()
    {
        return [.. arguments.Values.Distinct()];
    }
    /// <summary>
    /// Gets all the registered positional arguments.
    /// </summary>
    public PositionalArgument[] GetPositionalArguments()
    {
        return [.. positionalArguments];
    }

}