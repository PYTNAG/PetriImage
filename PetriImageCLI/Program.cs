using System.Reflection;
using CLIHelper;
using PetriImageCLI.Commands;

var commands = CommandsRegisterer.RegisterCommands();

if (args.Length == 0)
{
    Common cmd = new();
    cmd.Help();
    cmd.Execute();

    return;
}

if (args[0].StartsWith('-'))
{
    args = [
        typeof(Common).GetCustomAttribute<CommandAttribute>()!.Name, 
        ..args
    ];
}

if (commands.TryGetValue(args[0], out Type? commandType))
{
    string[] commandArgs = args[1..];
    Command command = (Activator.CreateInstance(commandType) as Command)!;

    command.Parse(commandArgs);
    command.Execute();
}
else
{
    Common cmd = new();
    cmd.Help();
    cmd.Execute();

    return;
}
