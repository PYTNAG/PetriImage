using System.Reflection;
using System.Threading.Tasks.Dataflow;
using CLIHelper;
using PetriImageCLI.Commands;

var commands = CommandsRegisterer.RegisterCommands();

Type? commandType = null;
string[] commandArgs = [];

if (args.Length == 0)
{
    Common.Help();
    return;
}

if (args[0].StartsWith('-'))
{
    args = [
        typeof(Common).GetCustomAttribute<CommandAttribute>()?.Name, 
        ..args
    ];
}

if (commands.TryGetValue(args[0], out commandType))
{
    commandArgs = args[1..];
    Command command = Activator.CreateInstance(commandType) as Command ?? throw new InvalidCastException("Command type doesn't inherit Command class");

    command.Parse(commandArgs);
}
else
{
    Common.Help();
    return;
}
