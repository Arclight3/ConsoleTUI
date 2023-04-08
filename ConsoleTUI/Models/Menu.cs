namespace Iustinsoft.ConsoleTUI.Models;

public record Menu
{
    public string Title { get; set; } = default!;
    public List<Option> Options { get; set; } = new();

    internal int TopStartPosition { get; set; }
    internal int LeftStartPosition { get; set; }
    internal int TopEndPosition { get; set; }
    internal int LeftEndPosition { get; set; }

    public static Menu Create(string title, List<Option> options) =>
        new()
        {
            Title = title,
            Options = options
        };

    public static Menu Create(string title, params Option[] options) =>
        new()
        {
            Title = title,
            Options = options.ToList()
        };

    public static Menu Create(string title, params string[] options) =>
        new()
        {
            Title = title,
            Options = options.Select(Option.Create).ToList()
        };
}