using Iustinsoft.ConsoleTUI.Models;
using Iustinsoft.ConsoleTUI.Models.Themes;

namespace Iustinsoft.ConsoleTUI;

public class TextUserInterface
{
    private Menu? _currentMenu;
    private ITheme _theme = new DefaultTheme();

    // Theme
    public void SetTheme(ITheme theme) =>
        _theme = theme;

    public void ResetTheme() =>
        _theme = new DefaultTheme();

    // Menu Printing
    public void PrintMenu(Menu menu)
    {
        SetCurrentMenu(menu);

        (_currentMenu!.LeftStartPosition, _currentMenu.TopStartPosition) = NativeGetCursorPosition();
        PrintCurrentMenu();
        (_currentMenu.LeftEndPosition, _currentMenu.TopEndPosition) = NativeGetCursorPosition();

        SetCursor();
    }

    public void RefreshMenu()
    {
        if (_currentMenu is null) return;

        ClearMenu(true);
        PrintCurrentMenu(true);
    }

    public void ClearMenu(bool restoreCurrentCursorPosition = false)
    {
        if (_currentMenu is null) throw new Exception("Unable to clear current menu because it was not provided.");

        var currentCursorPositionBackup = NativeGetCursorPosition();

        var topCurrentPosition = _currentMenu.TopStartPosition;
        var topEndPosition = _currentMenu.TopEndPosition;

        while (topCurrentPosition < topEndPosition)
        {
            NativeSetCursorPosition(currentCursorPositionBackup.LeftPosition, topCurrentPosition);

            NativeClearCurrentLine();
            topCurrentPosition++;
        }

        if (restoreCurrentCursorPosition)
            NativeSetCursorPosition(currentCursorPositionBackup.LeftPosition, currentCursorPositionBackup.TopPosition);
        else
            NativeSetCursorPosition(_currentMenu.LeftStartPosition, _currentMenu.TopStartPosition);
    }

    public void ClearScreen()
    {
        var (_, topEndPosition) = NativeGetCursorPosition();

        var topCurrentPosition = 0;
        while (topCurrentPosition < topEndPosition)
        {
            NativeSetCursorPosition(0, topCurrentPosition);

            NativeClearCurrentLine();
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
    public void Print(string? text = null, ConsoleColor? textForegroundColor = null, ConsoleColor? textBackgroundColor = null)
    {
        SetCursor();
        NativePrint(text, textForegroundColor, textBackgroundColor);
    }

    public void PrintLine(string? text = null, ConsoleColor? textForegroundColor = null, ConsoleColor? textBackgroundColor = null)
    {
        SetCursor();
        NativePrintLine(text, textForegroundColor, textBackgroundColor);
    }

    public void PrintTokens(List<TextToken> textTokens)
    {
        SetCursor();

        foreach (var textToken in textTokens)
            NativePrint(textToken.Text, textToken.ForegroundColor, textToken.BackgroundColor);
    }

    public void PrintTokens(TextToken token1, TextToken token2) =>
        PrintTokens(new List<TextToken> { token1, token2 });

    public void PrintTokens(TextToken token1, TextToken token2, TextToken token3) =>
        PrintTokens(new List<TextToken> { token1, token2, token3 });

    public void PrintTokens(TextToken token1, TextToken token2, TextToken token3, TextToken token4) =>
        PrintTokens(new List<TextToken> { token1, token2, token3, token4 });

    public void PrintTokens(TextToken token1, TextToken token2, TextToken token3, TextToken token4, TextToken token5) =>
        PrintTokens(new List<TextToken> { token1, token2, token3, token4, token5 });

    public void PrintTokens(TextToken token1, TextToken token2, TextToken token3, TextToken token4, TextToken token5, TextToken token6) =>
        PrintTokens(new List<TextToken> { token1, token2, token3, token4, token5, token6 });

    public void PrintTokensLine(TextToken token1, TextToken token2)
    {
        PrintTokens(token1, token2);
        PrintLine();
    }

    public void PrintTokensLine(TextToken token1, TextToken token2, TextToken token3)
    {
        PrintTokens(token1, token2, token3);
        PrintLine();
    }

    public void PrintTokensLine(TextToken token1, TextToken token2, TextToken token3, TextToken token4)
    {
        PrintTokens(token1, token2, token3, token4);
        PrintLine();
    }

    public void PrintTokensLine(TextToken token1, TextToken token2, TextToken token3, TextToken token4, TextToken token5)
    {
        PrintTokens(token1, token2, token3, token4, token5);
        PrintLine();
    }

    public void PrintTokensLine(TextToken token1, TextToken token2, TextToken token3, TextToken token4, TextToken token5, TextToken token6)
    {
        PrintTokens(token1, token2, token3, token4, token5, token6);
        PrintLine();
    }

    public void ClearLastLine()
    {
        var currentCursorPosition = NativeGetCursorPosition();
        NativeSetCursorPosition(0, currentCursorPosition.TopPosition - 1);
        NativeClearCurrentLine();
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
            _currentMenu.Options.Add(new Option("Exit"));
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

        NativePrintLine(title, _theme.TitleForegroundColor, _theme.TitleBackgroundColor);
    }

    private void PrintOption(Option option)
    {
        PrintLeftMargin();

        ConsoleColor foregroundColor;
        ConsoleColor backgroundColor;
        if (option.IsActive)
        {
            foregroundColor = _theme.ActiveOptionForegroundColor;
            backgroundColor = _theme.ActiveOptionBackgroundColor;
        }
        else
        {
            foregroundColor = _theme.OptionsForegroundColor;
            backgroundColor = _theme.OptionsBackgroundColor;
        }

        var optionText = $"{_theme.OptionsIndicator} {option.Name}";

        NativePrintLine(optionText, foregroundColor, backgroundColor);
    }

    private void PrintLineSeparator(bool printNewLineBefore = false, bool printNewLineAfter = false)
    {
        if (printNewLineBefore)
            NativePrintLine();

        PrintLeftMargin();

        var lineSeparator = _theme.LineSeparator;
        if (_theme.RepeatLineSeparatorToFitWidth)
        {
            var repeatSeparatorCount = Console.WindowWidth / 2 - _theme.LeftMarginColumns;
            lineSeparator = string.Concat(Enumerable.Repeat(_theme.LineSeparator, repeatSeparatorCount));
        }
        NativePrintLine(lineSeparator, _theme.LineSeparatorForegroundColor, _theme.LineSeparatorBackgroundColor);

        if (printNewLineAfter)
            NativePrintLine();
    }

    private void SetCursor()
    {
        NativeSetCursorPosition(_theme.LeftMarginColumns, NativeGetCursorPosition().TopPosition);
        NativeDisplayCursor(_theme.DisplayCursor);
    }

    // Native methods
    private static void NativePrint(string? text = null, ConsoleColor? textForegroundColor = null, ConsoleColor? textBackgroundColor = null)
    {
        var backupColor = NativeGetTextColor();

        if (textForegroundColor is not null)
            NativeSetTextForegroundColor(textForegroundColor.Value);

        if (textBackgroundColor is not null)
            NativeSetTextBackgroundColor(textBackgroundColor.Value);

        Console.Write(text);

        NativeSetTextColor(backupColor.ForegroundColor, backupColor.BackgroundColor);
    }

    private static void NativePrintLine(string? text = null, ConsoleColor? textForegroundColor = null, ConsoleColor? textBackgroundColor = null)
    {
        var backupColor = NativeGetTextColor();

        if (textForegroundColor is not null)
            NativeSetTextForegroundColor(textForegroundColor.Value);

        if (textBackgroundColor is not null)
            NativeSetTextBackgroundColor(textBackgroundColor.Value);

        Console.WriteLine(text);

        NativeSetTextColor(backupColor.ForegroundColor, backupColor.BackgroundColor);
    }

    private static (int LeftPosition, int TopPosition) NativeGetCursorPosition() =>
        Console.GetCursorPosition();

    private static void NativeDisplayCursor(bool display) =>
        Console.CursorVisible = display;

    private static void NativeSetCursorPosition(int leftPosition, int topPosition) =>
        Console.SetCursorPosition(leftPosition, topPosition);

    private static (ConsoleColor ForegroundColor, ConsoleColor BackgroundColor) NativeGetTextColor() =>
        (Console.ForegroundColor, Console.BackgroundColor);

    private static void NativeSetTextColor(ConsoleColor textForegroundColor, ConsoleColor textBackgroundColor) =>
        (Console.ForegroundColor, Console.BackgroundColor) = (textForegroundColor, textBackgroundColor);

    private static void NativeSetTextForegroundColor(ConsoleColor textForegroundColor) =>
        Console.ForegroundColor = textForegroundColor;

    private static void NativeSetTextBackgroundColor(ConsoleColor textBackgroundColor) =>
        Console.BackgroundColor = textBackgroundColor;

    private static void NativeClearCurrentLine() =>
        Console.Write(new string(' ', Console.BufferWidth));

    private static void NativeResetColor() =>
        Console.ResetColor();
}