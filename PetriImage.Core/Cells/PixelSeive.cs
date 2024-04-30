using Cells;

namespace PetriImage.Core.Cells;

public sealed class PixelSeive(uint x, uint y) : Seive(4)
{
    public (uint x, uint y) Position { get; } = (x, y);
}