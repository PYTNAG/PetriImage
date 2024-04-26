using CLIHelper;

namespace PetriImageCLI.Commands;

internal sealed class HelpFlag() : FlagAttribute("--help", [ "-h", "-?" ]) {}