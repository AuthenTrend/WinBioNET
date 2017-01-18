namespace WinBioNET.Enums
{
    public enum WinBioComponentType : uint
    {
        sensor = 1,
        engine = 2,
        storage = 3
    }

    public enum SensorLEDType : byte
    {
        NO_LED_MODE = 8,
        STEADY_RED,
        STEADY_GREEN,
        STEADY_BLUE,
        ONE_SEC_RED,
        ONE_SEC_GREEN,
        ONE_SEC_BLUE,
        FLASHING_RED,
        FLASHING_GREEN,
        FLASHING_BLUE
    }
}