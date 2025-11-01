using Discord;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace MiExLoggingDiscord.EmbedsConstructor;

public interface IEmbedConstructor
{
  protected bool ShouldSkip(LogLevel logLevel);

  (Embed embed, bool shouldMention)? Construct<TState>(
    IExternalScopeProvider? scopeProvider,
    LogLevel mentionLogLevel,
    LogEntry<TState> entry);
}
