namespace Iustinsoft.ConsoleTUI.Extensions;

public static class ConsoleColorExtensions
{
    public static ConsoleColor ToConsoleColor(this string color) =>
        color switch
        {
            "Black" => ConsoleColor.Black,
            "DarkBlue" => ConsoleColor.DarkBlue,
            "DarkGreen" => ConsoleColor.DarkGreen,
            "DarkCyan" => ConsoleColor.DarkCyan,
            "DarkRed" => ConsoleColor.DarkRed,
            "DarkMagenta" => ConsoleColor.DarkMagenta,
            "DarkYellow" => ConsoleColor.DarkYellow,
            "Gray" => ConsoleColor.Gray,
            "DarkGray" => ConsoleColor.DarkGray,
            "Blue" => ConsoleColor.Blue,
            "Green" => ConsoleColor.Green,
            "Cyan" => ConsoleColor.Cyan,
            "Red" => ConsoleColor.Red,
            "Magenta" => ConsoleColor.Magenta,
            "Yellow" => ConsoleColor.Yellow,
            "White" => ConsoleColor.White,
            _ => throw new ArgumentOutOfRangeException(nameof(color), color, null),
        };
}