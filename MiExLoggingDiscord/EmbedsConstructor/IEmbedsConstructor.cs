using Discord;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace MiExLoggingDiscord.EmbedsConstructor;

public interface IEmbedsConstructor
{
  IEnumerable<Embed>? Construct<TState>(
    IExternalScopeProvider? scopeProvider,
    LogEntry<TState> entry
  );
}
