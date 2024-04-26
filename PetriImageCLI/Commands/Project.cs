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
    private sealed class New : CommandBase
    {
        public override void Execute()
        {
            base.Execute();
        }
    }
}