using System.Collections.Concurrent;
using Discord.Webhook;
using Microsoft.Extensions.Logging;
using MiExLoggingDiscord.EmbedsConstructor;

namespace MiExLoggingDiscord;

public class DiscordLoggerProvider(
  IEnumerable<IEmbedConstructor> embedsConstructors,
  DiscordWebhookClient discordClient) : ILoggerProvider, ISupportExternalScope
{
  private readonly ConcurrentDictionary<string, DiscordLogger> _loggers = [];
  private IExternalScopeProvider? _scopeProvider;

  public void Dispose() => GC.SuppressFinalize(this);

  public ILogger CreateLogger(string categoryName) => _loggers.GetOrAdd(categoryName,
    new DiscordLogger(categoryName, _scopeProvider, LogLevel.Error, embedsConstructors,
      discordClient)); // TODO: make it customizable

  public void SetScopeProvider(IExternalScopeProvider scopeProvider)
  {
    _scopeProvider = scopeProvider;
    foreach (var (_, logger) in _loggers)
    {
      logger.ScopeProvider = _scopeProvider;
    }
  }
}
