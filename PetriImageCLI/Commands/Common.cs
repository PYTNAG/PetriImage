using System.Reflection;
using CLIHelper;

namespace PetriImageCLI.Commands;

[Command("common")]
internal sealed class Common : CommandBase
{
    private string _output = string.Empty;

    [HelpFlag]
    public void Help()
    {
        _output = _usage;
    } 

    public override void Execute()
    {
        Console.WriteLine(_output);
    }
}