using ConsoleTUI.Models;
using ConsoleTUI.Models.Themes;

namespace ConsoleTUI;

public class TextUserInterface
{
    private Menu? _currentMenu = null;
    private ITheme _theme = new DefaultTheme();

    // Theme
    public void SetTheme(ITheme theme)
    {
        _theme = theme;
    }

    public void ResetTheme()
    {
        _theme = new DefaultTheme();
    }

    // Menu Printing
    public void PrintMenu(Menu menu)
    {
        SetCurrentMenu(menu);

        (_currentMenu.LeftStartPosition, _currentMenu.TopStartPosition) = NativeGetCursorPosition();
        PrintCurrentMenu();
        (_currentMenu.LeftEndPosition, _currentMenu.TopEndPosition) = NativeGetCursorPosition();

        SetCursor();
    }

    public void RefreshMenu()
    {
        if (_currentMenu is not null)
        {
            ClearCurrentMenu();

            PrintCurrentMenu(true);
        }
    }

    public void ClearScreen()
    {
        var (_, topEndPosition) = NativeGetCursorPosition();

        var topCurrentPosition = 0;
        while (topCurrentPosition < topEndPosition)
        {
            NativeSetCursorPosition(0, topCurrentPosition);

            Console.Write(new string(' ', Console.BufferWidth));
            topCurrentPosition++;
        }

        NativeSetCursorPosition(0, 0);
    }

    // User Input
    public Option ReadUserInput()
    {
        if (_currentMenu is null) throw new Exception("Unable to read user input because there is no active menu.");

        var input = Console.ReadKey(true);

        while (input.Key is not ConsoleKey.Enter)
        {
            if (input.Key is ConsoleKey.UpArrow)
                SelectPreviousOption();

            if (input.Key is ConsoleKey.DownArrow)
                SelectNextOption();

            input = Console.ReadKey(true);
        }

        var activeOption = _currentMenu.Options.FirstOrDefault(x => x.IsActive)!;

        // Handle default options
        if (activeOption.Name is "Exit")
            Environment.Exit(0);

        return activeOption;
    }

    // Other
    public void Print(string text)
    {
        SetCursor();
        NativePrint(text);
    }

    public void PrintLine(string text)
    {
        SetCursor();
        NativePrintLine(text);
    }

    // Private methods
    private void SetCurrentMenu(Menu menu)
    {
        var options = menu.Options.Select(x => x with { }).ToList();
        _currentMenu = menu with { Options = options };
        
        AddDefaultOptions();
    }

    private void SelectDefaultOptionIfNoneIsSelected()
    {
        if (_currentMenu is null) throw new Exception("Unable to select option because there is no active menu.");

        var optionChanged = false;
        var shouldSetDefaultOption = true;

        for (var i = 0; i < _currentMenu.Options.Count; i++)
        {
            if (_currentMenu.Options[i].IsActive)
            {
                shouldSetDefaultOption = false;
                break;
            }
        }

        if (shouldSetDefaultOption)
        {
            _currentMenu.Options[0].IsActive = true;
            optionChanged = true;
        }

        if (optionChanged)
            RefreshMenu();
    }

    private void AddDefaultOptions()
    {
        if (_currentMenu is null) throw new Exception("Unable to add default options because there is no active menu.");

        if (_theme.AddExitOption)
        {
            _currentMenu.Options.Add(new Option { Name = "Exit" });
        }
    }

    private void SelectNextOption()
    {
        if (_currentMenu is null) throw new Exception("Unable to select option because there is no active menu.");

        var optionChanged = false;

        for (var i = 0; i < _currentMenu.Options.Count; i++)
        {
            if (_currentMenu.Options[i].IsActive)
            {
                if (i < _currentMenu.Options.Count - 1)
                {
                    _currentMenu.Options[i].IsActive = false;
                    _currentMenu.Options[i + 1].IsActive = true;
                    optionChanged = true;
                }
                else
                {
                    if (_theme.AllowCircularOptionsNavigation)
                    {
                        _currentMenu.Options[i].IsActive = false;
                        _currentMenu.Options[0].IsActive = true;
                        optionChanged = true;
                    }
                }

                break;
            }
        }

        if (optionChanged)
            RefreshMenu();
    }

    private void SelectPreviousOption()
    {
        if (_currentMenu is null) throw new Exception("Unable to select option because there is no active menu.");

        var optionChanged = false;

        for (var i = 0; i < _currentMenu.Options.Count; i++)
        {
            if (_currentMenu.Options[i].IsActive)
            {
                if (i > 0)
                {
                    _currentMenu.Options[i].IsActive = false;
                    _currentMenu.Options[i - 1].IsActive = true;
                    optionChanged = true;
                }
                else
                {
                    if (_theme.AllowCircularOptionsNavigation)
                    {
                        _currentMenu.Options[i].IsActive = false;
                        _currentMenu.Options[^1].IsActive = true;
                        optionChanged = true;
                    }
                }

                break;
            }
        }

        if (optionChanged)
            RefreshMenu();
    }

    private void PrintTopMargin()
    {
        if (_theme.TopMarginLines > 0)
        {
            var currentLine = 0;

            while (currentLine != _theme.TopMarginLines)
            {
                NativePrintLine();
                currentLine++;
            }
        }
    }

    private void PrintLeftMargin()
    {
        if (_theme.LeftMarginColumns > 0)
        {
            var leftMargin = string.Concat(Enumerable.Repeat(" ", _theme.LeftMarginColumns));
            NativePrint(leftMargin);
        }
    }

    private void PrintCurrentMenu(bool restoreCurrentCursorPosition = false)
    {
        if (_currentMenu is null) throw new Exception("Unable to print current menu because it was not provided.");

        var currentCursorPositionBackup = NativeGetCursorPosition();

        NativeSetCursorPosition(_currentMenu.LeftStartPosition, _currentMenu.TopStartPosition);

        PrintTopMargin();

        PrintLineSeparator(false, true);
        PrintTitle(_currentMenu.Title);
        PrintLineSeparator(false, true);

        SelectDefaultOptionIfNoneIsSelected();
        foreach (var option in _currentMenu.Options)
            PrintOption(option);

        PrintLineSeparator();
        NativePrintLine();

        if (restoreCurrentCursorPosition)
            NativeSetCursorPosition(currentCursorPositionBackup.LeftPosition, currentCursorPositionBackup.TopPosition);
    }

    private void PrintTitle(string title)
    {
        PrintLeftMargin();

        NativeSetTextColor(_theme.TitleForegroundColor, _theme.TitleBackgroundColor);

        NativePrintLine(title);

        NativeResetColor();
    }

    private void PrintOption(Option option)
    {
        PrintLeftMargin();

        if (option.IsActive)
            NativeSetTextColor(_theme.ActiveOptionForegroundColor, _theme.ActiveOptionBackgroundColor);
        else
            NativeSetTextColor(_theme.OptionsForegroundColor, _theme.OptionsBackgroundColor);

        var optionText = $"{_theme.OptionsIndicator} {option.Name}";
        NativePrintLine(optionText);

        NativeResetColor();
    }

    private void PrintLineSeparator(bool printNewLineBefore = false, bool printNewLineAfter = false)
    {
        if (printNewLineBefore)
            NativePrintLine();

        PrintLeftMargin();

        NativeSetTextColor(_theme.LineSeparatorForegroundColor, _theme.LineSeparatorBackgroundColor);

        var lineSeparator = _theme.LineSeparator;
        if (_theme.RepeatLineSeparatorToFitWidth)
        {
            var repeatSeparatorCount = Console.WindowWidth / 2 - _theme.LeftMarginColumns;
            lineSeparator = string.Concat(Enumerable.Repeat(_theme.LineSeparator, repeatSeparatorCount));
        }
        NativePrintLine(lineSeparator);

        NativeResetColor();

        if (printNewLineAfter)
            NativePrintLine();
    }

    private void SetCursor()
    {
        NativeSetCursorPosition(_theme.LeftMarginColumns, NativeGetCursorPosition().TopPosition);
        NativeDisplayCursor(_theme.DisplayCursor);
    }

    private void ClearCurrentMenu()
    {
        if (_currentMenu is null) throw new Exception("Unable to clear current menu because it was not provided.");

        var currentCursorPositionBackup = NativeGetCursorPosition();

        var topCurrentPosition = _currentMenu.TopStartPosition;
        var topEndPosition = _currentMenu.TopEndPosition;

        while (topCurrentPosition < topEndPosition)
        {
            NativeSetCursorPosition(currentCursorPositionBackup.LeftPosition, topCurrentPosition);

            Console.Write(new string(' ', Console.BufferWidth));
            topCurrentPosition++;
        }

        NativeSetCursorPosition(currentCursorPositionBackup.LeftPosition, currentCursorPositionBackup.TopPosition);
    }

    // Native methods

    private static void NativePrint(string? text = null)
    {
        Console.Write(text);
    }

    private static void NativePrintLine(string? text = null)
    {
        Console.WriteLine(text);
    }

    private static (int LeftPosition, int TopPosition) NativeGetCursorPosition()
    {
        var cursorPosition = Console.GetCursorPosition();
        return cursorPosition;
    }

    private static void NativeDisplayCursor(bool display)
    {
        Console.CursorVisible = display;
    }

    private static void NativeSetCursorPosition(int leftPosition, int topPosition)
    {
        Console.SetCursorPosition(leftPosition, topPosition);
    }

    private static void NativeSetTextColor(ConsoleColor foregroundColor, ConsoleColor backgroundColor)
    {
        Console.ForegroundColor = foregroundColor;
        Console.BackgroundColor = backgroundColor;
    }

    private static void NativeResetColor()
    {
        Console.ResetColor();
    }
}