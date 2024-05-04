using System.Reflection;
using System.Text;
using PetriImage.Core;

namespace PetriImageCLI.Repl;

internal static class Repl
{
    private static bool _isInterrupted = false;
    private static PetriPng? _petriPng;

    private static Dictionary<Predicate<string>, MethodInfo> _commands = [];

    private static void Eval(string input)
    {
        string[] tokens = input.Split(" ");

        MethodInfo? method = _commands.FirstOrDefault(commandInfo => commandInfo.Key(tokens[0])).Value;

        if (method is not null)
        {
            method.Invoke(null, tokens[1..]);
        }
        else
        {
            Console.WriteLine($"Unknown command: {tokens[0]}");
        }
    }

    public static void Loop(PetriPng petriPng)
    {
        _petriPng = petriPng;

        _isInterrupted = false;
        RegisterCommands();

        Console.WriteLine("Petri Image CLI :: REPL");

        while (!_isInterrupted)
        {
            Console.Write("\n> ");

            Eval(ReadLine());
        }

        _petriPng.Save();
    }

    /// <summary>
    /// Custom ReadLine method that works with Unix new line.
    /// </summary>
    /// <returns>User input as string</returns>
    private static string ReadLine()
    {
        StringBuilder input = new();

        while (true)
        {
            ConsoleKeyInfo keyInfo = Console.ReadKey();

            if (keyInfo.Key == ConsoleKey.Enter)
            {
                Console.Write("\n");
                break;
            }

            if (keyInfo.Key == ConsoleKey.Backspace)
            {
                input.Remove(input.Length - 1, 1);
                var (left, top) = Console.GetCursorPosition();
                Console.SetCursorPosition(left - 1, top);
                Console.Write(" ");
                Console.SetCursorPosition(left - 1, top);
                continue;
            }

            input.Append(keyInfo.KeyChar);
        }

        return input.ToString();
    }

    private static void RegisterCommands()
    {
        _commands = [];

        foreach (MethodInfo method in typeof(Repl).GetMethods(BindingFlags.NonPublic | BindingFlags.Static))
        {
            CommandAttribute? cmd = method.GetCustomAttribute<CommandAttribute>();

            if (cmd is null)
            {
                continue;
            }

            _commands.Add(cmd.IsNameOrAlias, method);
        }
    }

    [Command("quit", "q")]
    private static void Quit() => _isInterrupted = true;
}