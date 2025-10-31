using Discord;
using Microsoft.Extensions.Logging.Abstractions;

namespace MiExLoggingDiscord.EmbedsConstructor;

public interface IEmbedsConstructor
{
  IEnumerable<Embed>? Construct<TState>(LogEntry<TState> entry);
}
