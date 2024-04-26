using CLIHelper;

namespace PetriImageCLI.Commands;

[Command("common")]
public sealed class Common : Command
{
    private static string _output = string.Empty;

    [Flag("--help", [ "-h", "-?" ])]
    public static void Help()
    {
        _output = "this is --help -? -h";
    }

    [Flag("--test")]
    public static void Test()
    {
        _output = "this is test";
    }

    public override void Execute()
    {
        Console.WriteLine(_output);
    }
}