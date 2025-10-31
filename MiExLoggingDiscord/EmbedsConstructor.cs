using System.Text;
using Discord;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace MiExLoggingDiscord;

public interface IEmbedsConstructor
{
  IEnumerable<Embed>? Construct<TState>(
    IExternalScopeProvider? scopeProvider,
    LogEntry<TState> entry
  );
}

public abstract class EmbedsConstructorBase : IEmbedsConstructor
{
  protected abstract EmbedsConstructorConfig Config { get; }

  public IEnumerable<Embed>? Construct<TState>(
    IExternalScopeProvider? scopeProvider,
    LogEntry<TState> entry)
  {
    if (entry.LogLevel != Config.TargetLogLevel) return null;
    var @params = new BuildActionParams<TState>(scopeProvider, entry, new EmbedBuilder());
    foreach (var buildAction in GetBuildActions<TState>())
    {
      buildAction(@params);
    }

    var embed = @params.Builder.Build();
    return [embed];
  }

  protected virtual Action<BuildActionParams<TState>>[] GetBuildActions<TState>()
  {
    return [SetTitle, SetColor, SetScopeField, SetDetailsField, SetExceptionsField];
  }

  private void SetTitle<TState>(BuildActionParams<TState> @params)
  {
    @params.Builder.Title = Config.Title;
  }

  private void SetColor<TState>(BuildActionParams<TState> @params)
  {
    @params.Builder.Color = Config.Color;
  }

  private static void SetScopeField<TState>(BuildActionParams<TState> @params)
  {
    var scopeStringBuilder = new StringBuilder().AppendLine(@params.Entry.Category);
    @params.ScopeProvider?.ForEachScope((scope, sb) =>
    {
      if (scope is null) return;
      sb.AppendLine($"=> {scope}");
    }, scopeStringBuilder);
    @params.Builder.AddField("Scope", scopeStringBuilder.ToString());
  }

  private static void SetDetailsField<TState>(BuildActionParams<TState> @params)
  {
    var entry = @params.Entry;
    var details = entry.Formatter(entry.State, entry.Exception);
    @params.Builder.AddField("Details", details);
  }

  private static void SetExceptionsField<TState>(BuildActionParams<TState> @params)
  {
    var ex = @params.Entry.Exception;
    if (ex is null) return;
    var builder = @params.Builder;
    builder.AddField("Error Message", ex.Message);
    builder.AddField("Error Source", ex.Source);
    builder.AddField("Error StackTrace", ex.StackTrace);
  }

  protected record BuildActionParams<TState>(
    IExternalScopeProvider? ScopeProvider,
    LogEntry<TState> Entry,
    EmbedBuilder Builder);
}

public record EmbedsConstructorConfig(LogLevel TargetLogLevel, string Title, Color Color);

public static class DefaultEmbedsConstructors
{
  public static readonly IEmbedsConstructor[] Instances =
  [
    new TraceEmbedsConstructor(),
    new DebugEmbedsConstructor(),
    new InformationEmbedsConstructor(),
    new WarningEmbedsConstructor(),
    new ErrorEmbedsConstructor(),
    new CriticalEmbedsConstructor()
  ];

  private class TraceEmbedsConstructor : EmbedsConstructorBase
  {
    protected override EmbedsConstructorConfig Config =>
      new(LogLevel.Trace, "Trace", Color.DarkGrey);
  }

  private class DebugEmbedsConstructor : EmbedsConstructorBase
  {
    protected override EmbedsConstructorConfig Config =>
      new(LogLevel.Debug, "Debug", Color.LightGrey);
  }

  private class InformationEmbedsConstructor : EmbedsConstructorBase
  {
    protected override EmbedsConstructorConfig Config =>
      new(LogLevel.Information, "Info", Color.Green);
  }

  private class WarningEmbedsConstructor : EmbedsConstructorBase
  {
    protected override EmbedsConstructorConfig Config =>
      new(LogLevel.Warning, "Warn", Color.Gold);
  }

  private class ErrorEmbedsConstructor : EmbedsConstructorBase
  {
    protected override EmbedsConstructorConfig Config =>
      new(LogLevel.Error, "ERROR", Color.Orange);
  }

  private class CriticalEmbedsConstructor : EmbedsConstructorBase
  {
    protected override EmbedsConstructorConfig Config =>
      new(LogLevel.Critical, "CRITICAL", Color.Red);
  }
}
