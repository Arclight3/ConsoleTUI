namespace Iustinsoft.ConsoleTUI.Models;

public record Option(string Name)
{
    public bool IsActive { get; set; }

    public static Option Create(string name) => new(name);
}