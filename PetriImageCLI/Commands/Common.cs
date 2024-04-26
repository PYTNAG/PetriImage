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
        _output = 
            $"usage: petrii [{GetType().GetCustomAttribute<CommandAttribute>()!.Name}] [<flag>...]\n\n"
            + $"{(_subcommandsList == string.Empty ? string.Empty : "subcommands:" + _subcommandsList + "\n\n")}"
            + $"flags:\n{_flagsDescription}";
    }    

    public override void Execute()
    {
        Console.WriteLine(_output);
    }
}