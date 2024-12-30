namespace OxDED.Terminal.Arguments;

public class ArgumentParser {
    public ArgumentFormatter Register { get; private set; }
    public ArgumentParser(ArgumentFormatter register) {
        Register = register;
    }

    public void Parse(string[] args) {

    }
}