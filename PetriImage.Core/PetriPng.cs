using System.Text;

namespace PetriImage.Core;

public sealed class PetriPng
{
    private static readonly string _pngExtension = ".png";
    private static readonly byte[] _pngSignature = [0x89, 0x50, 0x4e, 0x47, 0x0d, 0x0a, 0x1a, 0x0a];

    public static readonly string PpngExtension = ".ppng";
    private static readonly string _workInProgressSuffix = "~";

    private const string _colonyChunkType = "clNy";

    private readonly string _filepath;
    private readonly Colony _colony;

    public PetriPng(string filepath)
    {
        _filepath = filepath;

        int width, height;

        using (FileStream file = File.OpenRead(_filepath))
        {
            try
            {
                byte[] signatureBuffer = new byte[8];

                file.ReadExactly(signatureBuffer);
                if (!signatureBuffer.SequenceEqual(_pngSignature))
                {
                    throw new Exception("Bad png signature");
                }

                byte[] 
                    length = new byte[4],
                    type = new byte[4];
                
                while (true)
                {
                    if (file.Position == file.Length)
                    {
                        throw new Exception("Png file doesn't have IHDR chunk");
                    }

                    file.ReadExactly(length);
                    file.ReadExactly(type);

                    if (type.SequenceEqual("IHDR"u8.ToArray()))
                    {
                        byte[] sizeBuffer = new byte[8];

                        file.ReadExactly(sizeBuffer);

                        var sizeSpan = sizeBuffer.AsSpan();

                        width = BitConverter.ToInt32(sizeSpan[..5]);
                        height = BitConverter.ToInt32(sizeSpan[5..]);

                        break;
                    }
                    else
                    {
                        file.Seek(BitConverter.ToUInt32(length) + 4, SeekOrigin.Current);
                    }
                }
            }
            catch (EndOfStreamException)
            {
                throw new EndOfStreamException($"Corrupted file ({_filepath}). Ragged png signature.");
            }
        }

        if (!TryParseColony(out _colony!))
        {
            _colony = Colony.Random(width, height);
        }
    }

    private bool TryParseColony(out Colony? colony)
    {
        using FileStream file = File.OpenRead(_filepath);

        file.Seek(8, SeekOrigin.Begin);

        byte[] 
            length = new byte[4],
            type = new byte[4],
            crc = new byte[4];

        file.ReadExactly(length);
        file.ReadExactly(type);

        if (Encoding.ASCII.GetString(type) != _colonyChunkType)
        {
            colony = null;
            return false;
        }

        // TODO : what if length is big (ex. uint.MaxValue)
        byte[] data = new byte[BitConverter.ToUInt32(length)];

        file.ReadExactly(data);
        file.ReadExactly(crc);

        if (CrcCalculator.CRC([..type, ..data]) != BitConverter.ToUInt32(crc))
        {
            throw new Exception("Colony chunk corrupted");
        }

        colony = null;
        return false;

        // TODO : parse colony from data
    }

    public void Save()
    {
        string wipFile = _filepath + _workInProgressSuffix;

        using FileStream ppng = File.Create(wipFile);
        {
            using FileStream source = File.OpenRead(_filepath);

            ppng.Write(_pngSignature);

            // copy IHDR chunk
            byte[] 
                length = new byte[4],
                type = new byte[4],
                crc = new byte[4];
            
            source.ReadExactly(length);

            byte[] data = new byte[BitConverter.ToUInt32(length)];

            source.ReadExactly(type);
            source.ReadExactly(data);
            source.ReadExactly(crc);

            ppng.Write([..length, ..type, ..data, ..crc]);

            // skip existing colony chunk
            byte[] lengthBytes = new byte[4];
            byte[] typeBytes = new byte[4];

            source.ReadExactly(lengthBytes);
            source.ReadExactly(typeBytes);

            // skip existing colony chunk
            if (Encoding.ASCII.GetString(typeBytes) == _colonyChunkType)
            {
                source.Seek(BitConverter.ToUInt32(lengthBytes) + 3, SeekOrigin.Current);
            }
            else
            {
                source.Seek(-8, SeekOrigin.Current);
            }

            // add new colony chunk
            byte[] chunkLength = BitConverter.GetBytes(_colony.GetBytesCount());
            byte[] chunkType = Encoding.ASCII.GetBytes(_colonyChunkType);
            byte[] chunkData = _colony.GetBytes();
            byte[] chunkCrc = BitConverter.GetBytes(CrcCalculator.CRC(chunkData));

            ppng.Write([..chunkLength, ..chunkType, ..chunkData, ..chunkCrc]);

            byte[] buffer = new byte[256];
            // copy other file data
            while (source.Position < source.Length)
            {
                source.Read(buffer);
                ppng.Write(buffer);
            }
        }
        
        File.Move(wipFile, _filepath, overwrite: true);
    }

    public static string NewFile(string sourcePngPath, string outputFolderPath, string outputFilename)
    {
        string sourceExt = Path.GetExtension(sourcePngPath);
        if (sourceExt != _pngExtension && sourceExt != PpngExtension)
        {
            throw new Exception($"Source file should have {_pngExtension} or {PpngExtension} extension");
        }

        string newFile = $"{outputFolderPath}{Path.DirectorySeparatorChar}{outputFilename}{PpngExtension}";

        File.Copy(sourcePngPath, newFile);

        return newFile;
    }
}