namespace OxDED.Terminal.Arguments;

/// <summary>
/// Helps you with parsing arguments.
/// </summary>
public class ArgumentParserOld {
    /// <summary>
    /// This is the name of the application.
    /// </summary>
    public string? name = null;
    /// <summary>
    /// The description of the application.
    /// </summary>
    public string? description = null;
    private Option? versionArgument = null;
    private Option? helpArgument = null;
    /// <summary>
    /// The version of the application.
    /// </summary>
    public string? version = null;
    /// <summary>
    /// The arguments of the parser.
    /// </summary>
    public Dictionary<string, Option> options = [];
    /// <summary>
    /// The arguments of the parser.
    /// </summary>
    public List<Argument> arguments = [];
    /// <summary>
    /// Sets the help argument of the application parser.
    /// </summary>
    /// <param name="keys"></param>
    /// <param name="showDescription"></param>
    /// <param name="showVersion"></param>
    /// <param name="shouldExit"></param>
    public ArgumentParserOld Help(IEnumerable<string>? keys = null, bool showDescription = true, bool showVersion = false, bool shouldExit = true) {
        if (helpArgument != null) {
            RemoveOption(helpArgument);
        }
        helpArgument = new Option(keys ?? ["h", "help"], "Shows all the available arguments.");
        AddOption(helpArgument, (Option arg) => {
            WriteHelp(showDescription, showVersion);
            if (shouldExit) {
                Environment.Exit(0);
            }
        });
        return this;
    }
    /// <summary>
    /// Sets the version of the application parser. And adds an argument.
    /// </summary>
    /// <param name="version">The version of the application.</param>
    /// <param name="keys">The keys for the parameter (default: v, version).</param>
    /// <param name="shouldExit"></param>
    /// <returns></returns>
    
    public ArgumentParserOld Version(string version, IEnumerable<string>? keys = null, bool shouldExit = true) {
        if (versionArgument != null) {
            RemoveOption(versionArgument);
        }
        versionArgument = new Option(keys ?? ["v", "version"], name == null ? "Shows the version of this application." : $"Shows the version of {name}."); // TODO: add documentation to first assign a name
        AddOption(versionArgument, (Option arg) => {
            WriteVersion();
            if (shouldExit) {
                Environment.Exit(0);
            }
        });
        this.version = version;
        return this;
    }
    /// <summary>
    /// Sets the description of the application parser.
    /// </summary>
    public ArgumentParserOld Description(string? description) {
        this.description = description;
        return this;
    }
    /// <summary>
    /// Sets the name of the application parser.
    /// </summary>
    public ArgumentParserOld Name(string? name) {
        this.name = name;
        return this;
    }
    /// <summary>
    /// Removes a argument.
    /// </summary>
    public ArgumentParserOld RemoveArgument(int position) {
        arguments.RemoveAt(position);
        return this;
    }
    /// <summary>
    /// Removes a argument.
    /// </summary>
    public ArgumentParserOld RemoveArgument(Argument argument) {
        arguments.Remove(argument);
        return this;
    }
    /// <summary>
    /// Removes an argument.
    /// </summary>
    public ArgumentParserOld RemoveOption(Option argument) {
        foreach (string key in argument.keys) {
            options.Remove(key);
        }
        return this;
    }
    /// <summary>
    /// Adds an argument.
    /// </summary>
    public ArgumentParserOld AddOption(Option argument, OptionCallback? callback = null) {
        foreach (string key in argument.keys) {
            options.Add(key, argument);
        }
        if (callback != null) {
            OnOption += (Option arg) => {
                if (arg.keys == argument.keys) {
                    callback?.Invoke(arg);
                }
            };
        }
        return this;
    }
    /// <summary>
    /// Adds a argument.
    /// </summary>
    public ArgumentParserOld AddArgument(Argument argument, ArgumentCallback? callback = null) {
        arguments.Add(argument);
        if (callback != null) {
            OnArgument += (Argument arg) => {
                if (arg.name == argument.name) {
                    callback?.Invoke(arg);
                }
            };
        }
        return this;
    }
    /// <summary>
    /// Writes the help menu to the terminal.
    /// </summary>
    public void WriteHelp(bool showDescription = true, bool showVersion = false) {
        Terminal.WriteLine(GetHelp(showDescription, showVersion));
    }
    /// <summary>
    /// Writes the version to the terminal.
    /// </summary>
    public void WriteVersion() {
        Terminal.WriteLine(GetVersion());
    }
    private static string GetArgumentHelp(Argument arg, bool isRed = false) {
        return $"{(isRed ? Color.LightRed.ToForegroundANSI() : Color.Orange.ToForegroundANSI())}\u2520{ANSI.Styles.ResetAll}  {arg.name}{(arg.description == null ? "" : ": "+arg.description)}\n";
    }
    private static string GetArgumentHelpName(string[] keys) {
        string result = "";
        for (int i = 0; i < keys.Length; i++) {
            string key = keys[i];
            result += key.Length == 1 ? "-" : "--";
            result += key;
            if (i != keys.Length - 1) {
                result += ", ";
            }
        }
        return result;
    }
    private static string GetArgumentHelp(Option arg) {
        string result = $"{Color.DarkGreen.ToForegroundANSI()}\u2520{ANSI.Styles.ResetAll}  {GetArgumentHelpName(arg.keys)}{(arg.description == null ? "" : ": "+arg.description)}\n";
        foreach (OptionParameter para in arg.parameters) {
            result += $"{Color.DarkGreen.ToForegroundANSI()}\u2503  \u2560{ANSI.Styles.ResetAll} {para.name}{(para.description == null ? "" : ": "+para.description)}\n";
        }
        return result;
    }
    private string GetHelp(bool showDescription = true, bool showVersion = false, int positionalIndex = -1) {
        string result = $"{ANSI.Styles.Bold}{name}{ANSI.Styles.ResetBold} {((showVersion && version != null) ? version : "")}{((showDescription && description != null) ? '\n' + description : "")}\n\n";

        if (arguments.Count > 0) {
            result += $"{Color.Orange.ToForegroundANSI()}\u250E\u2500\u2500{ANSI.Styles.ResetAll} Required Arguments\n";
            for (int i = 0; i < arguments.Count; i++) {
                Argument arg = arguments[i];
                if (positionalIndex <= i && positionalIndex != -1) {
                    result += GetArgumentHelp(arg, true);
                }
                else {
                    result += GetArgumentHelp(arg, false);
                }

            }
            result += "\n";
        }
        if (options.Count > 0 || helpArgument != null || versionArgument != null) {
            result += $"{Color.DarkGreen.ToForegroundANSI()}\u250E\u2500\u2500{ANSI.Styles.ResetAll} Arguments\n";
            if (helpArgument != null) {
                result += $"\u2520  {GetArgumentHelpName(helpArgument.keys)}: {helpArgument.description}\n";
            }
            if (versionArgument != null) {
                result += $"\u2520  {GetArgumentHelpName(versionArgument.keys)}: {versionArgument.description}\n";
            }
            if (options.Count > 0) {
                foreach (Option argument in options.Values.Distinct()) {
                    if (argument == versionArgument || argument == helpArgument) {
                        continue;
                    }
                    result += GetArgumentHelp(argument);
                }
            }
        }
        return result;
    }
    private string GetVersion() {
        return $"{ANSI.Styles.Bold}{name}{ANSI.Styles.ResetBold} {version ?? ""}{(description != null ? '\n' + description : "")}";
    }
    /// <summary>
    /// An event that is called when an option is parsed.
    /// </summary>
    public event OptionCallback? OnOption;
    /// <summary>
    /// An event that is called when a argument is parsed.
    /// </summary>
    public event ArgumentCallback? OnArgument;
    /// <summary>
    /// An event that is called when the format is invalid.
    /// </summary>
    public event InvalidFormatCallback? OnInvalidFormatCallback;
    private void WriteInvalid(string message) {
        WriteHelp(false);
        OnInvalidFormatCallback?.Invoke(message);
        Terminal.WriteLine("\n" + message, new Style { ForegroundColor = Color.Red });
        Environment.Exit(1);
    }
    private void WriteNoArgument(string message, int positionalIndex) {
        Terminal.WriteLine(GetHelp(false, false, positionalIndex));
        OnInvalidFormatCallback?.Invoke(message);
        Terminal.WriteLine("\n" + message, new Style { ForegroundColor = Color.Red });
        Environment.Exit(1);
    }
    /// <summary>
    /// Parses the arguments.
    /// </summary>
    public bool Parse(string arguments) {
        return Parse(arguments.Split(' '));
    }
    /// <summary>
    /// Parses the arguments.
    /// </summary>
    public bool Parse(string[] arguments) {
        int argumentIndex = 0;
        Option? parsingOption = null;
        List<string> parameters = [];
        bool isParsingArgument = false;
        foreach (string raw in arguments) {
            if (!isParsingArgument) {
                if (raw.StartsWith("--") && raw.Length > 2) {
                    parsingOption = GetArgument(raw[2..]);
                    if (parsingOption == null) {
                        WriteInvalid("No such argument as: --" + raw[2..] + ".");
                        return false;
                    }
                    else {
                        isParsingArgument = true;
                    }
                }
                else if (raw.StartsWith('-') && raw.Length > 1) {
                    if (raw.Length >= 3) {
                        WriteInvalid("Invalid symbol usage (-): " + raw + ".\n Should it be (--)?");
                    }
                    parsingOption = GetArgument(raw[1].ToString());
                    if (parsingOption == null) {
                        WriteInvalid("No such argument as: -" + raw[1] + ".");
                        return false;
                    }
                    else {
                        isParsingArgument = true;
                    }

                }
                else {
                    Argument? argument = GetArgument(argumentIndex);
                    if (argument != null) {
                        argument.value = raw;
                        OnArgument?.Invoke(argument);
                        argumentIndex++;
                    }
                    else {
                        WriteInvalid("Too much arguments.");
                        return false;
                    }
                }
            }
            if (isParsingArgument) {
                if (!(parameters.Count - 1 >= parsingOption!.parameters.Length)) {
                    parameters.Add(raw);
                }
                if (parameters.Count - 1 >= parsingOption!.parameters.Length) {
                    for (int j = 0; j < parsingOption!.parameters.Length; j++) {
                        parsingOption!.parameters[j].value = parameters[j + 1];
                    }
                    OnOption?.Invoke(parsingOption);
                    isParsingArgument = false;
                    parameters = [];
                    parsingOption = null;
                }
            }
        }
        if (isParsingArgument) {
            WriteInvalid("Invalid option parameters for " + parameters[0] + ".");
            return false;
        }
        if (this.arguments.Count != argumentIndex) {
            WriteNoArgument("Not enough arguments.", argumentIndex);
            return false;
        }

        return true;
    }
    /// <summary>
    /// Gets an argument that is registered (null if it isn't registered).
    /// </summary>
    public Option? GetArgument(string key) {
        if (!options.TryGetValue(key, out Option? value)) {
            return null;
        }
        return value;
    }
    /// <summary>
    /// Gets a argument at a location (in order as registered) (if there is one).
    /// </summary>
    public Argument? GetArgument(int pos) {
        return arguments.Count > pos ? arguments[pos] : null;
    }
    /// <summary>
    /// True if an argument is registered (not used).
    /// </summary>
    public bool HasArgument(string key) {
        return GetArgument(key) != null;
    }
    /// <summary>
    /// Gets all the registered arguments (not used).
    /// </summary>
    public Option[] GetOptions() {
        return [.. options.Values.Distinct()];
    }
    /// <summary>
    /// Gets all the registered arguments.
    /// </summary>
    public Argument[] GetArguments() {
        return [.. arguments];
    }

}