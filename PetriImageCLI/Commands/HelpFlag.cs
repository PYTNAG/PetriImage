using CLIHelper;

namespace PetriImageCLI.Commands;

internal sealed class HelpFlagAttribute() : FlagAttribute("--help", [ "-h", "-?" ]) {}