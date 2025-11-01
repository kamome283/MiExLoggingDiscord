using Discord;
using Microsoft.Extensions.Logging;

namespace MiExLoggingDiscord.EmbedsConstructor;

public interface IEmbedConstructorConfig
{
  LogLevel TargetLogLevel { get; }
  string LogLevelExpr { get; }
  Color Color { get; }
}
