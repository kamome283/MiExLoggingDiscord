using Discord;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace MiExLoggingDiscord.EmbedsConstructor;

public interface IMultiStepEmbedConstructor : IEmbedConstructor
{
  (Embed embed, string? mentionAddress)? IEmbedConstructor.Construct<TState>(
    IExternalScopeProvider? scopeProvider,
    LogLevel mentionLogLevel,
    LogEntry<TState> entry)
  {
    if (ShouldSkip(entry.LogLevel)) return null;
    var builder = new EmbedBuilder();
    var args = new BuildActionArgs<TState>(builder, scopeProvider, mentionLogLevel, entry);
    foreach (var buildAction in GetBuildActions<TState>())
    {
      buildAction(args);
    }

    var mentionAddress = GetMentionAddress(args);
    var embed = builder.Build();

    return (embed, mentionAddress);
  }

  protected IEnumerable<Action<BuildActionArgs<TState>>> GetBuildActions<TState>();
}
