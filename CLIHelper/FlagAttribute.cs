namespace CLIHelper;

[AttributeUsage(AttributeTargets.Method)]
public class FlagAttribute(string name, params string[] aliases) : Attribute
{
    public string Name { get; } = name;
    public string[] Aliases { get; } = aliases;

    public bool IsNameOrAlias(string flag) => flag == Name || Aliases.Contains(flag);
}