using CLIHelper;

namespace PetriImageCLI.Commands;

[Command("global")]
internal sealed class Global : CommandBase
{
    private readonly Dictionary<string, Type> _commands;

    public Global()
    {
        _commands = CommandsRegisterer.RegisterCommands();
        _subcommandsList.AddRange(_commands.Select(c => c.Key));
    }

    public override void Execute() { }

    public void ParseArgs(params string[] args)
    {
        Type executableCommandType = typeof(Global);

        int argPointer = 0;
        for (;argPointer < args.Length; ++argPointer)
        {
            if (args[argPointer].StartsWith('-'))
            {
                break;
            }

            if (_commands.TryGetValue(args[argPointer], out Type? cmdType))
            {
                executableCommandType = cmdType;
            }
            else
            {
                throw new ArgumentException($"unknown command {args[argPointer]}");
            }
        }

        Command cmd = (Activator.CreateInstance(executableCommandType) as Command)!;
        
        cmd.ParseFlags(args[argPointer..]);
        cmd.Execute();
    }
}