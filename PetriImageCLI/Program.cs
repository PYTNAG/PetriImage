using PetriImageCLI.Commands;

Global global = new();

if (args.Length == 0)
{
    global.PrintHelp();
    return;
}

try
{
    global.ParseFlags(args);
}
catch (Exception e)
{
    Console.Write($"{e.Message}\n");
    global.PrintHelp();
}