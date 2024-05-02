namespace PetriImage.Core;

/// <summary>
/// CRC calculator translated from ISO C (ISO_9899) on https://www.w3.org/TR/png-3/#D-CRCAppendix
/// </summary>
internal static class CrcCalculator
{
    private static uint[]? _crcTable;

    private static void MakeCrcTable()
    {
        _crcTable = new uint[256];

        uint c;

        for (int n = 0; n < 256; ++n)
        {
            c = (uint)n;
            for (int k = 0; k < 8; ++k)
            {
                c = (c & 1) == 1 
                    ? (c >> 1) ^ 0xedb88320  
                    : (c >> 1);
            }

            _crcTable[n] = c;
        }
    }

    private static uint UpdateCRC(uint crc, byte[] buffer)
    {
        uint c = crc;

        if (_crcTable is null)
        {
            MakeCrcTable();
        }

        for (int n = 0; n < buffer.Length; ++n)
        {
            c = _crcTable![(c ^ buffer[n]) & 0xff] ^ (c >> 8);
        }

        return c;
    }

    public static uint CRC(byte[] buffer) => UpdateCRC(0xffffffff, buffer) ^ 0xffffffff;
}