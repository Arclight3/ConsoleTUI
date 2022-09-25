using ConsoleTUI;
using ConsoleTUI.Models;

var console = new TextUserInterface();

var menu = Menu.Create("Test Menu", "Option 1", "Option 2", "Option 3");

console.PrintMenu(menu);
var option = console.ReadUserInput();

console.ClearScreen();
console.PrintMenu(menu);

if (option.Name is "Option 1")
{
    console.PrintLine($"You have selected: {option.Name}");
}

if (option.Name is "Option 2")
{
    console.PrintLine($"You have selected: {option.Name}");
}

if (option.Name is "Option 3")
{
    console.PrintLine($"You have selected: {option.Name}");
}

console.PrintLine(string.Empty);
console.PrintLine("Press any key to exit...");
Console.ReadLine();
