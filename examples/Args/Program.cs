using OxDED.Terminal;
using OxDED.Terminal.Arguments;

class Program {
    public static void Main(string[] args) {
        // Create the argument parser.
        ArgumentParser parser = new ArgumentParser()
            .Name("Parser Example") // Sets a name.
            .Description("Shows how you can you the ArgumentParser from 0xDED.Terminal.") // Sets the description.
            .Version("1.0.0") // Sets the version (can set the custom keys for that).
            .Help() // Adds a help argument (can set the custom keys for that too) (-h, --help).
            
            .AddArgument( // Adds an example argument (--test).
                new("test", "Just an example."),
                (Argument arg) => { // Adds a callback for when it is used.
                    Terminal.WriteLine("--test argument has been used.");
                }
            )
            .AddArgument( // Adds an example argument with multiple aliases (-f, --foo).
                new(["f", "foo"], "Argument with Multiple keys.")
            )
            .AddArgument( // Adds an example argument with parameters ()
                new("bar", "This has parameters.", [
                    new("para1", "This is the first parameter of --bar."),
                    new("para2", "This is the second parameter of --bar.")
                ])
            )
            .AddPositionalArgument( // Adds an required argument (at the first position).
                new("value", "This is a positional argument (required).")
            );
        
        parser.OnArgument += (Argument arg) => { // Adds an event for when an argument is used (same with OnPositionalArgument).
            if (arg.keys.SequenceEqual(["f", "foo"])) { // Checks for a -f or --foo argument.
                Terminal.WriteLine("The --foo, -f argument has been used.");
            }
        };
        
        parser.Parse(args); // Parses the arguments.
        Argument? arg = parser.GetArgument("bar"); // Gets the --bar argument.

        if (arg != null) { // If there is a --bar argument.
            if (arg.HasValue) { // If that agrument is also given.
                Terminal.WriteLine($"Bar argument has been used ({arg.parameters[0].Value}, {arg.parameters[1].Value})."); // Gets the parameters of that argument.
            }
        }

        Terminal.WriteLine(parser.GetPositionalArgument(0)!.Value); // Writes the postional argument

    }
}