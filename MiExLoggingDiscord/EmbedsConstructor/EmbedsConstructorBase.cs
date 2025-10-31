using System.Text;
using Discord;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace MiExLoggingDiscord.EmbedsConstructor;

public record EmbedsConstructorConfig(LogLevel TargetLogLevel, string Title, Color Color);

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
    return [SetTitle, SetColor, SetDescription, SetScopeField, SetExceptionsField];
  }

  protected void SetTitle<TState>(BuildActionParams<TState> @params)
  {
    var eventId = @params.Entry.EventId;
    var eventDescription = eventId.Name is not null ? $"{eventId.Name}: {eventId.Id}" : eventId.Id.ToString();
    @params.Builder.Title = $"[{Config.Title}] {@params.Entry.Category} ({eventDescription})";
  }

  protected void SetColor<TState>(BuildActionParams<TState> @params)
  {
    @params.Builder.Color = Config.Color;
  }

  protected static void SetDescription<TState>(BuildActionParams<TState> @params)
  {
    var entry = @params.Entry;
    var description = entry.Formatter(entry.State, entry.Exception);
    @params.Builder.Description = description;
  }

  protected static void SetScopeField<TState>(BuildActionParams<TState> @params)
  {
    if (@params.ScopeProvider is null) return;
    var stringBuilder = new StringBuilder();
    @params.ScopeProvider?.ForEachScope((scope, sb) =>
    {
      if (scope is null) return;
      sb.Append($" => {scope}");
    }, stringBuilder);
    if (stringBuilder.Length <= 0) return;
    stringBuilder.Remove(0, 4);
    @params.Builder.AddField("Scope", stringBuilder.ToString());
  }

  protected static void SetExceptionsField<TState>(BuildActionParams<TState> @params)
  {
    var ex = @params.Entry.Exception;
    if (ex is null) return;
    var builder = @params.Builder;
    builder.AddField("Error Message", ex.Message);
    if (ex.Source is not null) builder.AddField("Error Source", ex.Source);
    if (ex.InnerException is not null) builder.AddField("Inner Exception", ex.InnerException);
    if (ex.StackTrace is not null) builder.AddField("Error StackTrace", ex.StackTrace);
  }

  protected record BuildActionParams<TState>(
    IExternalScopeProvider? ScopeProvider,
    LogEntry<TState> Entry,
    EmbedBuilder Builder);
}
