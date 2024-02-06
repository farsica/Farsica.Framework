namespace Farsica.Framework.Data
{
    public enum DataType : byte
    {
        Boolean = 0,
        Byte = 1,
        Short = 2,
        Int = 3,
        Long = 4,
        Decimal = 5,

        String = 6,
        UnicodeString = 7,
        MaxString = 8,
        UnicodeMaxString = 9,
        Char = 10,
        UnicodeChar = 11,

        Date = 12,
        Time = 13,
        DateTime = 14,
        DateTimeOffset = 15,

        Guid = 16,

        File = 17,

        Ulid = 18,
    }
}
