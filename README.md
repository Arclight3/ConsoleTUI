# ConsoleTUI
A very simple but cool looking and customizable Text User Interface for your tools.

## Interface interaction

You only need **3** keys to interact with it: **Up**, **Down**, **Enter**.

Use the **Up** and **Down** arrow keys to navigate through options, then press **Enter** to select an option.

### Customization

If you are not satisfied with the default theme (```Iustinsoft.ConsoleTUI.Models.Themes.DefaultTheme```) you can tweak it's options, extend it or create a totally different Theme by implementing the ```Iustinsoft.ConsoleTUI.Models.Themes.ITheme``` interface.

Some of the customization options are:
- Colors (for title, options, currently selected option)
- Top margin
- Left margin
- Options indicator character
- Display/Hide cursor
- Loop navigation between options

## Code Examples

### Print a menu and return the selected option
```c#

// Declare a Console text user interface
var console = new TextUserInterface();

// Create a menu
var menu = Menu.Create("Test Menu", "Option 1", "Option 2", "Option 3");

// Print the menu
console.PrintMenu(menu);

// Get the user selected option
var option = console.ReadUserInput();

// Act on the selected option
if (option.Name is "Option 1") { // ... }

```

### Change default theme options
```c#

// Declare a Console text user interface
var console = new TextUserInterface();

// Declare a theme with custom options
var theme = new DefaultTheme
{
    TitleForegroundColor = ConsoleColor.DarkYellow
};

// Set the theme
console.SetTheme(theme);

// Create a menu
var menu = Menu.Create("Test Menu", "Option 1", "Option 2", "Option 3");

// Print the menu
console.PrintMenu(menu);

```
