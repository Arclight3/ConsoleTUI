using Iustinsoft.ConsoleTUI;

var console = new TextUserInterface();

console.PrintLine("old message", ConsoleColor.Gray);

console.PrintLine("pending", ConsoleColor.DarkGray);

await Task.Delay(2000);

console.ClearLastLine();
console.PrintLine("success", ConsoleColor.Green);

console.PrintLine("new message", ConsoleColor.Gray);
