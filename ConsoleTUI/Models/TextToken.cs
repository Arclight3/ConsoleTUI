using Iustinsoft.ConsoleTUI.Models.Themes;

namespace Iustinsoft.ConsoleTUI.Models;

public class TextToken
{
    public string Text { get; set; } = default!;
    public ConsoleColor? ForegroundColor { get; set; }
    public ConsoleColor? BackgroundColor { get; set; }
}