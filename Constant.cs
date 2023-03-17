namespace SnakeDotNet;

public static class Constant
{
    public const int GridWidth = 15;
    public const int GridHeight = 15;
    
    public enum GridType
    {
        None = 0,
        Head = 1,
        Item = 2,
        Body = 3,
    }

    public const string Head = "■";
    public const string None = "□";
    public const string Item = "●";
    public const string Body = "▦";
    public const int UpdateDelay = 150;
}