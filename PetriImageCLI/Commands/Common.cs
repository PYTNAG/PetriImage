using CLIHelper;

namespace PetriImageCLI.Commands;

[Command("common")]
public sealed class Common : Command
{
    [Flag("--help", [ "-h", "-?" ])]
    public static void Help()
    {
        Console.WriteLine("this is --help -? -h");
    }

    [Flag("--test")]
    public static void Test()
    {
        Console.WriteLine("this is test");
    }
}