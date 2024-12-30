namespace OxDED.Terminal.Arguments;

/// <summary>
/// Represents a format for arguments and options.
/// </summary>
public partial class ArgumentFormatter {
    public string? name;
    public string? description;
    public string? version;
    
    /// <summary>
    /// The options of this format.
    /// </summary>
    public List<OptionFormat> Options { get; private set; }
    /// <summary>
    /// The arguments of this format.
    /// </summary>
    public List<ArgumentFormat> Arguments { get; private set; }

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

    public ArgumentFormatter AddHelpOption(bool quit = true, IEnumerable<string>? keys = null) {
        Option()
            .Keys(keys ?? ["-h", "--help"])
            .Description("Shows this help message.")
        .Finish();
        return this;
    }
    public ArgumentFormatter AddVersionOption(bool quit = true, IEnumerable<string>? keys = null) {
        Option()
            .Keys(keys ?? ["-v", "--version"])
            .Description("Shows the version of this application.")
        .Finish();
        return this;
    }

    // public ArgumentFormatter
}

public partial class ArgumentFormatter {
    public class ArgumentFormat {
        public string description;
        public string name;

        public ArgumentFormatter ArgumentFormatter { get; private set; }

        public ArgumentFormat(ArgumentFormatter argumentFormatter) {
            ArgumentFormatter = argumentFormatter;
        }

        public ArgumentFormat Name(string name) {
            this.name = name;
            return this;
        }
        public ArgumentFormat Description(string description) {
            this.description = description;
            return this;
        }

        public ArgumentFormatter Finish() {
            ArgumentFormatter.Arguments = [.. ArgumentFormatter.Arguments, this];
            return ArgumentFormatter;
        }
    }
}

public partial class ArgumentFormatter {
    public class OptionFormat {
        public string[] keys;
        public string description;
        public ParameterFormat[] parameters;

        public ArgumentFormatter ArgumentFormatter { get; private set; }

        public OptionFormat(ArgumentFormatter argumentFormatter) {
            ArgumentFormatter = argumentFormatter;
        }

        public OptionFormat Key(string key) {
            keys = [.. keys, key];
            return this;
        }
        public OptionFormat Keys(IEnumerable<string> keys) {
            this.keys = [.. this.keys, .. keys];
            return this;
        }
        public OptionFormat Description(string description) {
            this.description = description;
            return this;
        }
        public ParameterFormat Parameter() {
            return new(this);
        }

        public ArgumentFormatter Finish() {
            ArgumentFormatter.Options = [.. ArgumentFormatter.Options, this];
            return ArgumentFormatter;
        }

        public class ParameterFormat {
            public string name { get; set; }
            public string description { get; set; }
            public bool required { get; set; }

            public OptionFormat OptionFormat { get; private set; }

            public ParameterFormat(OptionFormat optionBuilder) {
                OptionFormat = optionBuilder;
            }

            public ParameterFormat Name(string name) {
                this.name = name;
                return this;
            }
            public ParameterFormat Description(string description) {
                this.description = description;
                return this;
            }
            public ParameterFormat Required(bool required) {
                this.required = required;
                return this;
            }

            public OptionFormat Finish() {
                OptionFormat.parameters = [.. OptionFormat.parameters, this];
                return OptionFormat;
            }
        }
    }
}

public class HelpOptionFormat : ArgumentFormatter.OptionFormat {
    public HelpOptionFormat(ArgumentFormatter argumentFormatter) : base(argumentFormatter) { }
    public bool quit;
    public HelpOptionFormat Quit(bool quit) {
        this.quit = quit;
        return this;
    }
}

public class VersionOptionFormat : ArgumentFormatter.OptionFormat {
    public VersionOptionFormat(ArgumentFormatter argumentFormatter) : base(argumentFormatter) { }
    public bool quit;
    public VersionOptionFormat Quit(bool quit) {
        this.quit = quit;
        return this;
    }
}