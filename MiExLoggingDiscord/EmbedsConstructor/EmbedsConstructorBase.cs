using Discord;
using Microsoft.Extensions.Logging.Abstractions;

namespace MiExLoggingDiscord.EmbedsConstructor;

internal abstract class EmbedsConstructorBase : IEmbedsConstructor
{
  public abstract IEnumerable<Embed>? Construct<TState>(LogEntry<TState> entry);
}
