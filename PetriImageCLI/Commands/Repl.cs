using CLIHelper;
using REPL = PetriImageCLI.Repl.Repl;

namespace PetriImageCLI.Commands;

[Command("repl")]
internal sealed class Repl : CommandBase
{
    private string? _inputFile;

    [Flag("--input", "-i")]
    public void SetInput(string input)
    {
        if (Path.GetExtension(input) != ".ppng")
        {
            throw new ArgumentException("Input file must have .ppng extension");
        }

        _inputFile = input;
    }

    public override void Execute() 
    {
        if (_inputFile is null)
        {
            throw new ArgumentNullException("Input file must be specified with --input | -i flag");
        }

        REPL.Loop(new(_inputFile));
    }
}