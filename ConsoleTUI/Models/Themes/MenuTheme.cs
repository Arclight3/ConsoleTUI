namespace Iustinsoft.ConsoleTUI.Models.Themes;

public class MenuTheme
{
    // Title
    public ConsoleColor TitleForegroundColor { get; set; } = ConsoleColor.Green;
    public ConsoleColor TitleBackgroundColor { get; set; } = ConsoleColor.Black;

    // Active option
    public ConsoleColor ActiveOptionForegroundColor { get; set; } = ConsoleColor.Blue;
    public ConsoleColor ActiveOptionBackgroundColor { get; set; } = ConsoleColor.Black;

    // Options
    public string OptionsIndicator { get; set; } = ">";
    public ConsoleColor OptionsForegroundColor { get; set; } = ConsoleColor.Gray;
    public ConsoleColor OptionsBackgroundColor { get; set; } = ConsoleColor.Black;
    public bool AddExitOption { get; set; } = true;
    public bool AllowCircularOptionsNavigation { get; set; } = true;

    // Line Separator
    public string LineSeparator { get; set; } = "__";
    public bool RepeatLineSeparatorToFitWidth { get; set; } = true;
    public ConsoleColor LineSeparatorForegroundColor { get; set; } = ConsoleColor.White;
    public ConsoleColor LineSeparatorBackgroundColor { get; set; } = ConsoleColor.Black;

    public bool DisplayCursor { get; set; } = false;
}