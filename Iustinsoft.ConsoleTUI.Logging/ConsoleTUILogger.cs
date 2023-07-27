using Iustinsoft.ConsoleTUI.Models;
using Iustinsoft.ConsoleTUI.Models.Themes;
using Microsoft.Extensions.Logging;

namespace Iustinsoft.ConsoleTUI.Logging;

public class ConsoleTUILogger : ILogger
{
    public LogLevel MinimumLogLevel { get; set; }

    private readonly MessagesTheme _messagesTheme; 
    private readonly TextUserInterface _console;

    public ConsoleTUILogger(MessagesTheme messagesTheme, TextUserInterface console)
    {
        _messagesTheme = messagesTheme ?? new();
        _console = console ?? new();

        MinimumLogLevel = LogLevel.Debug;
    }

    public IDisposable? BeginScope<TState>(TState state)
        where TState : notnull =>
        throw new NotImplementedException();

    public bool IsEnabled(LogLevel logLevel) =>
        logLevel is not LogLevel.None;

    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
    {
        if (IsEnabled(logLevel) is false) return;
        if (logLevel < MinimumLogLevel) return;

        if (formatter is null) throw new ArgumentNullException(nameof(formatter));

        var (messageTemplate, messageArguments) = ExtractMessageTemplateAndArguments(state);

        if (!string.IsNullOrEmpty(messageTemplate) || exception is not null)
            WriteMessage(logLevel, messageTemplate, messageArguments, exception);
    }

    private (string? MessageTemplate, Dictionary<string, object?> MessageArguments) ExtractMessageTemplateAndArguments<TState>(TState state)
    {
        var originalMessageArguments = state as IReadOnlyList<KeyValuePair<string, object?>>;
        _ = originalMessageArguments ?? throw new ArgumentNullException(nameof(originalMessageArguments));

        var messageTemplate = originalMessageArguments.First(x => x.Key == "{OriginalFormat}").Value?.ToString();
        var messageArguments = new Dictionary<string, object?>();

        foreach (var logArgument in originalMessageArguments)
        {
            if (logArgument.Key is "{OriginalFormat}") continue;

            messageArguments.Add(logArgument.Key, logArgument.Value);
        }

        return (messageTemplate, messageArguments);
    }

    private void WriteMessage(LogLevel logLevel, string? messageTemplate, Dictionary<string, object?> messageArguments, Exception? exception)
    {
        if (messageArguments.Count is 0)
            WriteBasicMessage(logLevel, messageTemplate, exception);
        else
            WriteAdvancedMessage(logLevel, messageTemplate, messageArguments, exception);
    }

    private void WriteBasicMessage(LogLevel logLevel, string? message, Exception? exception)
    {
        if (exception is not null)
            message = $"{message}{exception}";

        switch (logLevel)
        {
            case LogLevel.Trace:
                _console.PrintLine(message, _messagesTheme.TraceMessageColor);
                break;
            case LogLevel.Debug:
                _console.PrintLine(message, _messagesTheme.DebugMessageColor);
                break;
            case LogLevel.Information:
                _console.PrintLine(message, _messagesTheme.InformationalMessageColor);
                break;
            case LogLevel.Warning:
                _console.PrintLine(message, _messagesTheme.WarningMessageColor);
                break;
            case LogLevel.Error:
                _console.PrintLine(message, _messagesTheme.ErrorMessageColor);
                break;
            case LogLevel.Critical:
                _console.PrintLine(message, _messagesTheme.CriticalMessageColor);
                break;
            case LogLevel.None:
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(logLevel), logLevel, null);
        }
    }

    private void WriteAdvancedMessage(LogLevel logLevel, string? messageTemplate, Dictionary<string, object?> messageArguments, Exception? exception)
    {
        var messageTemplateParts = ExtractMessageTemplateParts(messageTemplate);

        var textColor = logLevel switch
        {
            LogLevel.Trace => _messagesTheme.TraceMessageColor,
            LogLevel.Debug => _messagesTheme.DebugMessageColor,
            LogLevel.Information => _messagesTheme.InformationalMessageColor,
            LogLevel.Warning => _messagesTheme.WarningMessageColor,
            LogLevel.Error => _messagesTheme.ErrorMessageColor,
            LogLevel.Critical => _messagesTheme.CriticalMessageColor,
            LogLevel.None => default,
            _ => throw new ArgumentOutOfRangeException(nameof(logLevel), logLevel, null)
        };

        var textTokens = new List<TextToken>();
        for (var charIndex = 0; charIndex < messageTemplateParts.Count; charIndex++)
        {
            var messageTemplatePart = messageTemplateParts[charIndex];

            if (messageTemplatePart.StartsWith('{'))
            {
                messageTemplatePart = messageTemplatePart[1..^1];

                messageArguments.TryGetValue(messageTemplatePart, out var messageTemplatePartValue);
                messageTemplatePart = messageTemplatePartValue?.ToString();

                textTokens.Add(new TextToken(messageTemplatePart, _messagesTheme.MessageTokenColor));
            }
            else
            {
                textTokens.Add(new TextToken(messageTemplatePart, textColor));
            }
        }

        _console.PrintTokensLine(textTokens.ToArray());
    }

    private static List<string> ExtractMessageTemplateParts(string? messageTemplate)
    {
        var messageTemplateParts = new List<string>();

        if (messageTemplate is not null)
        {
            var i = 0;
            var j = 0;
            while (j < messageTemplate.Length)
            {
                if (messageTemplate[j] == '{')
                {
                    // If the template doesn't start with a placeholder get the text part
                    if (j > i)
                    {
                        messageTemplateParts.Add(messageTemplate[i..j]);
                        i = j;
                    }

                    // Move right index to the end of the current placeholder
                    j = messageTemplate.IndexOf('}', j) + 1;

                    // Get current placeholder value
                    messageTemplateParts.Add(messageTemplate[i..j]);

                    // Move left index to the end of current placeholder
                    i = j;
                }
                else
                {
                    // Move right index to next character
                    j++;
                }

                // If right index reached end of template and is not a placeholder get the text part
                if (j == messageTemplate.Length && messageTemplate[j - 1] != '}')
                    messageTemplateParts.Add(messageTemplate[i..j]);
            }
        }

        return messageTemplateParts;
    }
}