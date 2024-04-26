using System.Reflection;

namespace CLIHelper;

public static class CommandsRegisterer
{
    public static Dictionary<string, Type> RegisterCommands()
    {
        Dictionary<string, Type> commands = [];

        Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();

        foreach (Assembly assembly in assemblies)
        {
            foreach (Type type in assembly.GetTypes())
            {
                CommandAttribute? cmd = type.GetCustomAttribute<CommandAttribute>();
                if (cmd is not null)
                {
                    if (!type.GetConstructors().Any(ci => ci.GetParameters().Length == 0))
                    {
                        throw new Exception("Command type must have constructor with 0 parameters");
                        
                    }
                    
                    if (type.BaseType != typeof(Command))
                    {
                        throw new Exception("Command type must implement Command class");
                    }

                    commands.Add(cmd.Name, type);
                }
            }
        }

        return commands;
    }
}