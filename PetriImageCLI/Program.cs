using System.Reflection;
using CLIHelper;
using PetriImageCLI.Commands;

var commands = CommandsRegisterer.RegisterCommands();

if (args.Length == 0)
{
    Global cmd = new();
    cmd.PrintHelp();
    cmd.Execute();

    return;
}

if (args[0].StartsWith('-'))
{
    args = [
        typeof(Global).GetCustomAttribute<CommandAttribute>()!.Name, 
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
    Global cmd = new();
    cmd.PrintHelp();
    cmd.Execute();

    return;
}
