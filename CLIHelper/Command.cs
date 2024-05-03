using System.Collections.ObjectModel;
using System.Reflection;

namespace CLIHelper;

public abstract class Command
{
    protected readonly ReadOnlyDictionary<Predicate<string>, MethodInfo> _flags;

    public Command()
    {
        Dictionary<Predicate<string>, MethodInfo> flags = [];

        foreach (var methodInfo in GetType().GetMethods())
        {
            FlagAttribute? flag = methodInfo.GetCustomAttribute<FlagAttribute>();
            if (flag is not null)
            {
                flags.Add(flag.IsNameOrAlias, methodInfo);
                continue;
            }
        }

        _flags = flags.AsReadOnly();
    }
    
    public void ParseFlags(string[] args)
    {
        if (args.Length == 0)
        {
            return;
        }

        if (!args[0].StartsWith('-'))
        {
            throw new ArgumentException("First argument must be flag and starts with hyphen");
        }

        int pointer = 0;
        while (pointer < args.Length)
        {
            string flagName = args[pointer++];

            List<string> flagParams = [];

            while (pointer < args.Length && !args[pointer].StartsWith('-'))
            {
                flagParams.Add(args[pointer++]);
            }

            _flags
                .First(flagInfo => flagInfo.Key(flagName)) // first flag with name <flagName>
                .Value.Invoke(this, [..flagParams]); // invoke flag method with given params
        }
    }

    public abstract void Execute();
} 