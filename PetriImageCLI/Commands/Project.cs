using CLIHelper;

namespace PetriImageCLI.Commands;

[Command("proj")]
internal sealed class Project : CommandBase
{
    public override void Execute()
    {
        base.Execute();
    }

    [Command("new")]
    internal sealed class New : CommandBase
    {
        private string _source = string.Empty;
        private string _title = string.Empty;
        private string _outputFolder = string.Empty;

        [Flag("--source", "-s")]
        public void SetSource(string source) => _source = source;

        [Flag("--title", "-t")]
        public void SetTitle(string title) => _title = title;

        [Flag("--output", "-o")]
        public void SetOutput(string output) => _outputFolder = output;

        public override void Execute()
        {
            PetriImage.Core.Project.New(_title, _outputFolder, _source);

            _finalMessage = $"Project {_title} created at {_outputFolder}";

            base.Execute();
        }
    }
}