using Discord;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace MiExLoggingDiscord.EmbedsConstructor;

public record BuildActionArgs<TState>(
  EmbedBuilder Builder,
  IExternalScopeProvider? ScopeProvider,
  LogLevel MentionLogLevel,
  LogEntry<TState> Entry);
