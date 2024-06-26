﻿namespace CLIHelper;

[AttributeUsage(AttributeTargets.Class)]
public class CommandAttribute(string name) : Attribute
{
    public string Name { get; } = name;
}
