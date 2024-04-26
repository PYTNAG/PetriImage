using System.Reflection;
using System.Text;
using CLIHelper;

namespace PetriImageCLI.Commands;

internal abstract class CommandBase : Command
{
    // <flag> [, <alias>...] [: <description>]
    protected readonly string _flagsDescription;

    public CommandBase() : base()
    {
        StringBuilder description = new();
            
        foreach (var (_, flagMethod) in _flags)
        {
            FlagAttribute flag = flagMethod.GetCustomAttribute<FlagAttribute>()!;
            
            string flagAliases = string.Join(", ", [flag.Name, ..flag.Aliases]);
            string? flagDescription = flagMethod.GetCustomAttribute<DescriptionAttribute>()?.Text;

            description.Append($"{flagAliases}{(flagDescription is not null ? ": " + flagDescription : string.Empty)}\n");
        }

        _flagsDescription = description.ToString();
    }
}