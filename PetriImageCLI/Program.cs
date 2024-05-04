using PetriImageCLI.Commands;

Global global = new();

if (args.Length == 0)
{
    global.PrintHelp();
    return;
}

try
{
    global.ParseArgs(args);
}
catch (Exception e)
{
    Console.Write($"{e.Message}\n");
    global.PrintHelp();
}