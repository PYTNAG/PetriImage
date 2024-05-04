using CLIHelper;
using PetriImage.Core;

namespace PetriImageCLI.Commands;

[Command("new")]
[Description("Create new .ppng file with random colony")]
internal sealed class New : CommandBase
{
    private const string _capturesFolderName = "captures";
    private string? _sourceImagePath;

    [Flag("--source", "-s")]
    [Description("Path to source png-image (may be ppng)")]
    public void SetSource(string source)
    {
        string sourceExt = Path.GetExtension(source);

        if (sourceExt != ".png" && sourceExt != ".ppng")
        {
            throw new Exception("New petri-png must be created from png image (including ppng)");
        }
        
        _sourceImagePath = source;
    }
    public override void Execute()
    {
        if (_sourceImagePath is null)
        {
            throw new ArgumentNullException("Source image path must be specified (-s <pathToImage>)");
        }

        if (!Directory.Exists(_capturesFolderName))
        {
            Directory.CreateDirectory(_capturesFolderName);
        }

        PetriPng.NewFile(_sourceImagePath, _capturesFolderName, $"az@{DateTime.UtcNow:s}"); // az@yyyy-MM-ddThh:mm:ss
    }
}