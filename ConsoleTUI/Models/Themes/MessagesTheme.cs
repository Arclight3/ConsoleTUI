namespace Iustinsoft.ConsoleTUI.Models.Themes;

public class MessagesTheme
{
    public ConsoleColor TraceMessageColor { get; set; } = ConsoleColor.DarkGray;
    public ConsoleColor DebugMessageColor { get; set; } = ConsoleColor.DarkGray;
    public ConsoleColor InformationalMessageColor { get; set; } = ConsoleColor.Green;
    public ConsoleColor WarningMessageColor { get; set; } = ConsoleColor.Yellow;
    public ConsoleColor ErrorMessageColor { get; set; } = ConsoleColor.DarkRed;
    public ConsoleColor CriticalMessageColor { get; set; } = ConsoleColor.Red;

    public ConsoleColor MessageTokenColor { get; set; } = ConsoleColor.DarkYellow;
}