using Chains;

namespace PetriImage.Core.Cells;

internal static class Serializator
{
    private static readonly byte[] separator = [];

    public static byte[] GetBytes(PixelSeive seive)
    {
        List<byte> bytes = [];

        byte[] positionBytes = [
            ..BitConverter.GetBytes(seive.Position.x),
            ..BitConverter.GetBytes(seive.Position.y)
        ];

        byte[] freeChains = seive.FreeChains
            .SelectMany<Chain, byte>(chain => [..SerializeChain(chain), ..separator])
            .ToArray();
        
        byte[] genotype = [..BitConverter.GetBytes(seive.Genotype.Length), ..seive.Genotype];

        byte[] enegy = [..BitConverter.GetBytes(seive.Energy.Count()), ..(seive.Energy.SelectMany(SerializeEnergy))];

        return [..bytes];
    }

    private static byte[] SerializeChain(Chain chain)
    {
        List<byte> bytes = [];

        bytes.AddRange(BitConverter.GetBytes(chain.Head.Length));
        bytes.AddRange(chain.Head);
        bytes.AddRange(BitConverter.GetBytes(chain.Body.Length));
        bytes.AddRange(chain.Body);

        return [..bytes];
    }

    private static byte[] SerializeEnergy(Energy energy)
    {
        List<byte> bytes = [];
        
        bytes.Add(energy.Type);
        bytes.AddRange(BitConverter.GetBytes(energy.Value));

        return [..bytes];
    }
}