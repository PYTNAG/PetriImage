namespace CLIHelper;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class DescriptionAttribute(string text) : Attribute
{
    public string Text { get; } = text;
}