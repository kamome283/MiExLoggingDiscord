using Discord;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace MiExLoggingDiscord;

public interface IEmbedsConstructor
{
  IEnumerable<Embed>? Construct<TState>(LogEntry<TState> entry);
}

public abstract class EmbedsConstructorBase : IEmbedsConstructor
{
  protected abstract EmbedsConstructorConfig Config { get; }

  public IEnumerable<Embed>? Construct<TState>(LogEntry<TState> entry)
  {
    if (entry.LogLevel != Config.TargetLogLevel) return null;
    var message = entry.Formatter(entry.State, entry.Exception);
    var embed = new EmbedBuilder
    {
      Title = Config.Title,
      Description = message,
      Color = Config.Color,
    }.Build();
    return [embed];
  }
}

public record EmbedsConstructorConfig(LogLevel TargetLogLevel, string Title, Color Color);

public class TraceEmbedsConstructor : EmbedsConstructorBase
{
  protected override EmbedsConstructorConfig Config =>
    new(LogLevel.Trace, "Trace", Color.DarkGrey);
}

public class DebugEmbedsConstructor : EmbedsConstructorBase
{
  protected override EmbedsConstructorConfig Config =>
    new(LogLevel.Debug, "Debug", Color.LightGrey);
}

public class InformationEmbedsConstructor : EmbedsConstructorBase
{
  protected override EmbedsConstructorConfig Config =>
    new(LogLevel.Information, "Info", Color.Green);
}

public class WarningEmbedsConstructor : EmbedsConstructorBase
{
  protected override EmbedsConstructorConfig Config =>
    new(LogLevel.Warning, "Warn", Color.Gold);
}

public class ErrorEmbedsConstructor : EmbedsConstructorBase
{
  protected override EmbedsConstructorConfig Config =>
    new(LogLevel.Error, "ERROR", Color.Orange);
}

public class CriticalEmbedsConstructor : EmbedsConstructorBase
{
  protected override EmbedsConstructorConfig Config =>
    new(LogLevel.Critical, "CRITICAL", Color.Red);
}
