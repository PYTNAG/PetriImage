using System.Reflection;
using System.Text;
using CLIHelper;

namespace PetriImageCLI.Commands;

internal abstract class CommandBase : Command
{
    // [<subcommand>...]
    protected readonly string _subcommandsList;

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

        List<string> subcommands = [];

        foreach (var nestedType in GetType().GetNestedTypes())
        {
            CommandAttribute? cmd = nestedType.GetCustomAttribute<CommandAttribute>();
            if (cmd is not null)
            {
                subcommands.Add(cmd.Name);
            }
        }

        _subcommandsList = string.Join(", ", subcommands);
    }
}