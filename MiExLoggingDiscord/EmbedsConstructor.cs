using System.Text;
using Discord;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace MiExLoggingDiscord;

public interface IEmbedsConstructor
{
  IEnumerable<Embed>? Construct<TState>(
    IExternalScopeProvider? scopeProvider,
    LogEntry<TState> entry);
}

public abstract class EmbedsConstructorBase : IEmbedsConstructor
{
  protected abstract EmbedsConstructorConfig Config { get; }

  public IEnumerable<Embed>? Construct<TState>(
    IExternalScopeProvider? scopeProvider,
    LogEntry<TState> entry)
  {
    if (entry.LogLevel != Config.TargetLogLevel) return null;
    var scope = GetScope(scopeProvider, entry);
    var details = GetDetails(scopeProvider, entry);
    var embed = new EmbedBuilder
      {
        Title = Config.Title,
        Color = Config.Color,
      }
      .AddField("Scope", scope)
      .AddField("Details", details)
      .Build();
    return [embed];
  }

  protected virtual string GetScope<TState>(
    IExternalScopeProvider? scopeProvider,
    LogEntry<TState> entry)
  {
    var scopeStringBuilder = new StringBuilder().AppendLine(entry.Category);
    scopeProvider?.ForEachScope((scope, sb) =>
    {
      if (scope is null) return;
      sb.AppendLine($"=> {scope}");
    }, scopeStringBuilder);
    return scopeStringBuilder.ToString();
  }

  protected virtual string GetDetails<TState>(
    IExternalScopeProvider? _,
    LogEntry<TState> entry)
  {
    return entry.Formatter(entry.State, entry.Exception);
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
