namespace Gadgetron.Ps2
{
    public enum PineCommand : byte
    {
        Read8Bits = 0x00,
        Read16Bits = 0x01,
        Read32Bits = 0x02,
        Read64Bits = 0x03,
        Write8Bits = 0x04,
        Write16Bits = 0x05,
        Write32Bits = 0x06,
        Write64Bits = 0x07
    }
}
