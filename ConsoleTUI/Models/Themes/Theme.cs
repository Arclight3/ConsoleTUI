namespace Iustinsoft.ConsoleTUI.Models.Themes;

public class Theme
{
    // General
    public int LeftMarginColumns { get; set; } = 2;
    public int TopMarginLines { get; set; } = 0;

    // Menu
    public MenuTheme MenuTheme { get; set; } = new();
    public MessagesTheme MessagesTheme { get; set; } = new();
}