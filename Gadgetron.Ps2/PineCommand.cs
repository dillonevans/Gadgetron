namespace Gadgetron.Ps2
{
    public enum PineCommand : byte
    {
        Read8Bits = 0,
        Read16Bits = 1,
        Read32Bits = 2,
        Read64Bits = 3,
        Write8Bits = 4,
        Write16Bits = 5,
        Write32Bits = 6,
        Write64Bits = 7
    }
}
