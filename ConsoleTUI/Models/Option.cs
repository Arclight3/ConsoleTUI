namespace ConsoleTUI.Models;

public record Option
{
    public string Name { get; set; } = default!;
    public bool IsActive { get; set; }

    public static Option Create(string name)
    {
        return new Option { Name = name };
    }
}