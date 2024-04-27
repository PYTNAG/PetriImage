using System.Reflection;
using System.Text;
using CLIHelper;

namespace PetriImageCLI.Commands;

internal abstract class CommandBase : Command
{
    protected string _finalMessage;

    // [<subcommand>...]
    protected readonly string _subcommandsList;

    // <flag> [, <alias>...] [: <description>]
    protected readonly string _flagsDescription;

    protected readonly string _usage;

    public CommandBase() : base()
    {
        _finalMessage = string.Empty;

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

        _usage = 
            $"usage: petrii [{GetType().GetCustomAttribute<CommandAttribute>()!.Name}] [<flag>...]\n\n" +
            $"{(_subcommandsList == string.Empty ? string.Empty : "subcommands:" + _subcommandsList + "\n\n")}" +
            $"flags:\n{_flagsDescription}";
    }

    [HelpFlag]
    public void Help()
    {
        _finalMessage = _usage;
    }
    
    public void PrintFinalMessage()
    {
        Console.WriteLine("\n" + _finalMessage + "\n");
    }
}