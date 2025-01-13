namespace OxDED.Terminal.Arguments;

/// <summary>
/// Represents a format for arguments and options.
/// </summary>
public partial class ArgumentFormatter {
    public bool shouldExitOnError = true;
    public string? name;
    public string? description;
    public string? version;
    
    /// <summary>
    /// The options of this format.
    /// </summary>
    public readonly List<OptionFormat> Options;
    /// <summary>
    /// The arguments of this format.
    /// </summary>
    public readonly  List<ArgumentFormat> Arguments;

    /// <summary>
    /// Creates a new argument format.
    /// </summary>
    public ArgumentFormatter(List<ArgumentFormat>? arguments = null, List<OptionFormat>? options = null) {
        Options = options ?? [];
        Arguments = arguments ?? [];
    }

    public OptionFormat Option() {
        return new(this);
    }
    public ArgumentFormat Argument() {
        return new(this);
    }

    public ArgumentFormatter ShouldExitOnError(bool shouldExit) {
        shouldExitOnError = shouldExit;
        return this;
    }
    public ArgumentFormatter Name(string name) {
        this.name = name;
        return this;
    }
    public ArgumentFormatter Description(string? description) {
        this.description = description;
        return this;
    }
    public ArgumentFormatter Version(string? version) {
        this.version = version;
        return this;
    }

    public HelpOptionFormat CurrentHelpOption { get; internal set; }

    public VersionOptionFormat CurrentVersionOption { get; internal set; }

    public HelpOptionFormat HelpOption() {
        return new HelpOptionFormat(this);
    }
    public VersionOptionFormat VersionOption() {
        return new VersionOptionFormat(this);
    }

    /// <summary>
    /// Finishes formatting and creates a parser.
    /// </summary>
    /// <returns>A new argument parser.</returns>
    public ArgumentParser Finish() {
        return new(this);
    }
}

public partial class ArgumentFormatter {
    /// <summary>
    /// Represents a format for an argument.
    /// </summary>
    public class ArgumentFormat {
        /// <summary>
        /// The description of this argument. Used by the help menu.
        /// </summary>
        public string description = "";
        
        /// <summary>
        /// The name of this argument. Used by the help menu.
        /// </summary>
        public string name = "";
        
        /// <summary>
        /// The parent format.
        /// </summary>
        public readonly ArgumentFormatter ArgumentFormatter;

        internal ArgumentFormat(ArgumentFormatter argumentFormatter) {
            ArgumentFormatter = argumentFormatter;
        }
        /// <summary>
        /// Sets the name of this argument.
        /// </summary>
        /// <param name="name">The new name.</param>
        /// <returns>This argument.</returns>
        public ArgumentFormat Name(string name) {
            this.name = name;
            return this;
        }
        /// <summary>
        /// Sets the description of this argument.
        /// </summary>
        /// <param name="description">The new description.</param>
        /// <returns>This argument.</returns>
        public ArgumentFormat Description(string description) {
            this.description = description;
            return this;
        }

        /// <summary>
        /// Finishes and saves this argument format.
        /// </summary>
        /// <returns>The parent.</returns>
        public ArgumentFormatter Finish() {
            ArgumentFormatter.Arguments.Add(this);
            return ArgumentFormatter;
        }
    }
}

public partial class ArgumentFormatter {
    /// <summary>
    /// Represents a format for an option.
    /// </summary>
    public class OptionFormat {
        /// <summary>
        /// All the keys of this option.
        /// </summary>
        public readonly List<string> keys = [];
        /// <summary>
        /// The description of this argument. Used by the help menu.
        /// </summary>
        public string description = "";
        /// <summary>
        /// All the parameters of this option.
        /// </summary>
        public readonly List<ParameterFormat> parameters = [];

        /// <summary>
        /// The parent format.
        /// </summary>
        public readonly ArgumentFormatter ArgumentFormatter;

        internal OptionFormat(ArgumentFormatter argumentFormatter) {
            ArgumentFormatter = argumentFormatter;
        }

        /// <summary>
        /// Adds a key to check for.
        /// </summary>
        /// <param name="key">The new key.</param>
        /// <returns>This option.</returns>
        public virtual OptionFormat Key(string key) {
            keys.Add(key);
            return this;
        }
        /// <summary>
        /// Adds keys to check for.
        /// </summary>
        /// <param name="keys">The new keys to add.</param>
        /// <returns>This option.</returns>
        public virtual OptionFormat Keys(IEnumerable<string> keys) {
            this.keys.AddRange(keys);
            return this;
        }
        /// <summary>
        /// Sets the description of this option.
        /// </summary>
        /// <param name="description">The new description.</param>
        /// <returns>This option.</returns>
        public virtual OptionFormat Description(string description) {
            this.description = description;
            return this;
        }
        /// <summary>
        /// Adds a new parameter to this option.
        /// </summary>
        /// <returns>The parameter formatter.</returns>
        public virtual ParameterFormat Parameter() {
            return new(this);
        }

        /// <summary>
        /// Finishes and saves this option format.
        /// </summary>
        /// <returns>The parent.</returns>
        public virtual ArgumentFormatter Finish() {
            ArgumentFormatter.Options.Add(this);
            return ArgumentFormatter;
        }
        /// <summary>
        /// Represents a format for an option's parameter.
        /// </summary>
        public class ParameterFormat {
            /// <summary>
            /// The name of this parameter. Used by the help menu.
            /// </summary>
            public string name = "";
            /// <summary>
            /// The description of this argument. Used by the help menu.
            /// </summary>
            public string description = "";

            /// <summary>
            /// The parent format.
            /// </summary>
            public readonly OptionFormat OptionFormat;

            internal ParameterFormat(OptionFormat optionBuilder) {
                OptionFormat = optionBuilder;
            }

            /// <summary>
            /// Sets the name of this parameter.
            /// </summary>
            /// <param name="name">The new name.</param>
            /// <returns>This parameter.</returns>
            public ParameterFormat Name(string name) {
                this.name = name;
                return this;
            }
            /// <summary>
            /// Sets the description of this parameter.
            /// </summary>
            /// <param name="description">The new description.</param>
            /// <returns>This parameter.</returns>
            public ParameterFormat Description(string description) {
                this.description = description;
                return this;
            }

            /// <summary>
            /// Finishes and saves this parameter format.
            /// </summary>
            /// <returns>The parent.</returns>
            public OptionFormat Finish() {
                OptionFormat.parameters.Add(this);
                return OptionFormat;
            }
        }
    }
}

/// <summary>
/// Represents a configurable help option.
/// </summary>
public class HelpOptionFormat : ArgumentFormatter.OptionFormat {
    internal HelpOptionFormat(ArgumentFormatter argumentFormatter) : base(argumentFormatter) {
        Keys(["h", "help"]).Description("Displays this help page.");
    }
    /// <summary>
    /// Whether the program should quit when this option is used or not.
    /// </summary>
    public bool quit = true;
    /// <summary>
    /// Sets whether the program should quit when this option is used or not.
    /// </summary>
    /// <param name="quit">Whether the program should quit when this option is used or not.</param>
    /// <returns>This option.</returns>
    public HelpOptionFormat Quit(bool quit) {
        this.quit = quit;
        return this;
    }
    /// <inheritdoc/>
    public override ArgumentFormatter Finish() {
        ArgumentFormatter.CurrentHelpOption = this;
        return base.Finish();
    }
}

/// <summary>
/// Represents a configurable version option.
/// </summary>
public class VersionOptionFormat : ArgumentFormatter.OptionFormat {
    internal VersionOptionFormat(ArgumentFormatter argumentFormatter) : base(argumentFormatter) {
        Keys(["v", "version"]).Description("Displays the version and description of this program.");
    }
    /// <summary>
    /// Whether the program should quit when this option is used or not.
    /// </summary>
    public bool quit = true;
    /// <summary>
    /// Sets whether the program should quit when this option is used or not.
    /// </summary>
    /// <param name="quit">Whether the program should quit when this option is used or not.</param>
    /// <returns>This option.</returns>
    public VersionOptionFormat Quit(bool quit) {
        this.quit = quit;
        return this;
    }
    /// <inheritdoc/>
    public override ArgumentFormatter Finish() {
        ArgumentFormatter.CurrentVersionOption = this;
        return base.Finish();
    }
}