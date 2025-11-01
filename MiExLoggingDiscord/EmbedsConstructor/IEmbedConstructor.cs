using Discord;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace MiExLoggingDiscord.EmbedsConstructor;

public interface IEmbedConstructor
{
  protected bool ShouldSkip(LogLevel logLevel);
  protected string? GetMentionAddress<TState>(BuildActionArgs<TState> args);

  (Embed embed, string? mentionAddress)? Construct<TState>(
    IExternalScopeProvider? scopeProvider,
    LogLevel mentionLogLevel,
    LogEntry<TState> entry);
}
