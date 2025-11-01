using Discord;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace MiExLoggingDiscord.EmbedsConstructor;

public record BuildActionArgs<TState>(
  EmbedBuilder Builder,
  IExternalScopeProvider? ScopeProvider,
  LogEntry<TState> Entry);

public interface IMultiStepEmbedConstructor : IEmbedConstructor
{
  Embed? IEmbedConstructor.Construct<TState>(IExternalScopeProvider? scopeProvider, LogEntry<TState> entry)
  {
    if (ShouldSkip(entry.LogLevel)) return null;
    var builder = new EmbedBuilder();
    foreach (var buildAction in GetBuildActions<TState>())
    {
      buildAction(new BuildActionArgs<TState>(builder, scopeProvider, entry));
    }

    var embed = builder.Build();
    return embed;
  }

  protected IEnumerable<Action<BuildActionArgs<TState>>> GetBuildActions<TState>();
}
