using Discord.Webhook;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace MiExLoggingDiscord;

public class DiscordLogger(
  string name,
  IExternalScopeProvider? scopeProvider,
  IEnumerable<IEmbedsConstructor> embedsConstructors,
  DiscordWebhookClient discordClient) : ILogger
{
  public void Log<TState>(
    LogLevel logLevel,
    EventId eventId,
    TState state,
    Exception? exception,
    Func<TState, Exception?, string> formatter)
  {
    if (IsEnabled(logLevel)) return;
    var logEntry = new LogEntry<TState>(logLevel, name, eventId, state, exception, formatter);
    var embeds = embedsConstructors
      .Select(constructor => constructor.Construct(logEntry))
      .FirstOrDefault(embeds => embeds is not null);
    if (embeds is null) return;
    _ = discordClient.SendMessageAsync(embeds: embeds);
  }

  public bool IsEnabled(LogLevel logLevel) => logLevel != LogLevel.None;

  public IDisposable? BeginScope<TState>(TState state) where TState : notnull =>
    scopeProvider?.Push(state);
}
