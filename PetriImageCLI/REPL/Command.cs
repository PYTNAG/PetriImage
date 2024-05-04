namespace PetriImageCLI.Repl;

[AttributeUsage(AttributeTargets.Method)]
internal sealed class CommandAttribute(string name, params string[] aliases) : Attribute
{
    public string Name { get; } = name;
    public string[] Aliases { get; } = aliases;

    public bool IsNameOrAlias(string name) => name == Name || Aliases.Contains(name);
}