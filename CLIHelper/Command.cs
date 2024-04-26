using System.Reflection;

namespace CLIHelper;

public abstract class Command
{
    private readonly Dictionary<Predicate<string>, MethodInfo> _flags;

    public Command()
    {
        _flags = [];

        foreach (var methodInfo in GetType().GetMethods())
        {
            FlagAttribute? flag = methodInfo.GetCustomAttribute<FlagAttribute>();
            if (flag is not null)
            {
                _flags.Add(flag.IsNameOrAlias, methodInfo);
                continue;
            }
        }
    }
    
    public void Parse(string[] args)
    {
        if (args.Length == 0)
        {
            return;
        }

        if (!args[0].StartsWith('-'))
        {
            throw new ArgumentException("first argument must be flag and starts with hyphen");
        }

        int pointer = 0;
        while (pointer < args.Length)
        {
            string flag = args[pointer];

            ++pointer;
            List<string> flagArgs = [];
            while (pointer < args.Length && !args[pointer].StartsWith('-'))
            {
                flagArgs.Add(args[pointer]);
                ++pointer;
            }

            _flags.First(f => f.Key.Invoke(flag)).Value.Invoke(this, [.. flagArgs]);
        }
    }
} 