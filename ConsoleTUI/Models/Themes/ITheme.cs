namespace Iustinsoft.ConsoleTUI.Models.Themes;

public interface ITheme
{
    // Title
    public ConsoleColor TitleForegroundColor { get; set; }
    public ConsoleColor TitleBackgroundColor { get; set; }

    // Options
    public string OptionsIndicator { get; set; }
    public ConsoleColor OptionsForegroundColor { get; set; }
    public ConsoleColor OptionsBackgroundColor { get; set; }
    public bool AddExitOption { get; set; }

    // Active option
    public ConsoleColor ActiveOptionForegroundColor { get; set; }
    public ConsoleColor ActiveOptionBackgroundColor { get; set; }

    // Line Separator
    public string LineSeparator { get; set; }
    public bool RepeatLineSeparatorToFitWidth { get; set; }
    public ConsoleColor LineSeparatorForegroundColor { get; set; }
    public ConsoleColor LineSeparatorBackgroundColor { get; set; }

    // General
    public int LeftMarginColumns { get; set; }
    public int TopMarginLines { get; set; }
    public bool AllowCircularOptionsNavigation { get; set; }
    public bool DisplayCursor { get; set; }
}