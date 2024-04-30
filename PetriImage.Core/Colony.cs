using PetriImage.Core.Cells;

namespace PetriImage.Core;

public sealed class Colony
{
    private HashSet<PixelSeive> _cells = [];
    private byte[]? _cachedCellsSerialization = null;
    private byte[] CachedCellsSerialization {
        get
        {
            _cachedCellsSerialization ??= _cells.SelectMany(c => c.ByteSerialization()).ToArray();
            return _cachedCellsSerialization;
        }
    }

    private Colony() { }

    public static Colony Random(int imageWidth, int imageHeight)
    {
        Colony colony = new()
        {
            _cells =
            [
                new(
                    (uint)System.Random.Shared.Next(imageWidth),
                    (uint)System.Random.Shared.Next(imageHeight)
                )
            ]
        };

        return colony;
    }

    public byte[] GetBytes() => CachedCellsSerialization;

    public int GetBytesCount() => CachedCellsSerialization.Length;
}