using Iustinsoft.ConsoleTUI;
using Iustinsoft.ConsoleTUI.Models;

var console = new TextUserInterface();

console.PrintLine("old message", ConsoleColor.Gray);

console.PrintMenu(Menu.Create("Question", "Yes", "No"));

await Task.Delay(1000);
console.ClearMenu();

console.PrintLine("new message", ConsoleColor.Green);

console.PrintTokensLine(
    new("Using Database connection: ", ConsoleColor.Gray),
    new("Migration", ConsoleColor.DarkYellow),
    new("Migration", ConsoleColor.DarkYellow),
    new("Migration", ConsoleColor.DarkYellow),
    new(" .", ConsoleColor.Gray));

console.PrintTokens(
    new("Using Database connection: ", ConsoleColor.Gray),
    new("Migration", ConsoleColor.DarkYellow),
    new("Migration", ConsoleColor.DarkYellow),
    new(" .", ConsoleColor.Gray));

Console.ReadKey();
