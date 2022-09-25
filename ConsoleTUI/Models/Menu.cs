namespace Iustinsoft.ConsoleTUI.Models;

public record Menu
{
    public string Title { get; set; } = default!;
    public List<Option> Options { get; set; } = new();

    internal int TopStartPosition { get; set; }
    internal int LeftStartPosition { get; set; }
    internal int TopEndPosition { get; set; }
    internal int LeftEndPosition { get; set; }

    public static Menu Create(string title, List<Option> options)
    {
        var menu = new Menu
        {
            Title = title,
            Options = options
        };

        return menu;
    }

    public static Menu Create(string title, params Option[] options)
    {
        var menu = new Menu
        {
            Title = title,
            Options = options.ToList()
        };

        return menu;
    }

    public static Menu Create(string title, params string[] options)
    {
        var menu = new Menu
        {
            Title = title,
            Options = options.Select(Option.Create).ToList()
        };

        return menu;
    }
}