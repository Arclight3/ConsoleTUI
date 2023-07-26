namespace Iustinsoft.ConsoleTUI.Models;

public record TextToken(string? Text)
{
    public TextToken(string? text, ConsoleColor? foregroundColor)
        : this(text) =>
        ForegroundColor = foregroundColor;

    public TextToken(string? text, ConsoleColor? foregroundColor, ConsoleColor? backgroundColor)
        : this(text) =>
        (ForegroundColor, BackgroundColor) = (foregroundColor, backgroundColor);

    public ConsoleColor? ForegroundColor { get; set; }
    public ConsoleColor? BackgroundColor { get; set; }
}