using Discord.Webhook;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using MiExLoggingDiscord.EmbedsConstructor;

namespace MiExLoggingDiscord;

public class DiscordLogger(
  string name,
  IExternalScopeProvider? scopeProvider,
  LogLevel mentionLogLevel,
  IEnumerable<IEmbedConstructor> embedsConstructors,
  DiscordWebhookClient discordClient) : ILogger
{
  internal IExternalScopeProvider? ScopeProvider { get; set; } = scopeProvider;

  public void Log<TState>(
    LogLevel logLevel,
    EventId eventId,
    TState state,
    Exception? exception,
    Func<TState, Exception?, string> formatter)
  {
    if (!IsEnabled(logLevel)) return;
    var logEntry = new LogEntry<TState>(logLevel, name, eventId, state, exception, formatter);
    var embeds = embedsConstructors
      .Select(constructor => constructor.Construct(ScopeProvider, mentionLogLevel, logEntry))
      .FirstOrDefault(embeds => embeds is not null);
    if (embeds is null) return;
    var (embed, shouldMention) = embeds.Value;
    if (shouldMention)
    {
      const string mentionText = "@everyone"; // TODO: make it customizable
      _ = discordClient.SendMessageAsync(mentionText, embeds: [embed]);
    }
    else
    {
      _ = discordClient.SendMessageAsync(embeds: [embed]);
    }
  }

  public bool IsEnabled(LogLevel logLevel) => logLevel != LogLevel.None;

  public IDisposable? BeginScope<TState>(TState state) where TState : notnull =>
    ScopeProvider?.Push(state);
}
