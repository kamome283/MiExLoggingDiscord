using Discord;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace MiExLoggingDiscord.EmbedsConstructor;

public interface IEmbedConstructor
{
  protected bool ShouldSkip(LogLevel logLevel);

  Embed? Construct<TState>(IExternalScopeProvider? scopeProvider, LogEntry<TState> entry);
}
