using System.Text;
using Microsoft.Extensions.Logging;

namespace MiExLoggingDiscord.EmbedsConstructor;

public interface IDefaultEmbedConstructor : IMultiStepEmbedConstructor, IEmbedConstructorConfig
{
  bool IEmbedConstructor.ShouldSkip(LogLevel logLevel)
  {
    return logLevel != TargetLogLevel;
  }

  IEnumerable<Action<BuildActionArgs<TState>>> IMultiStepEmbedConstructor.GetBuildActions<TState>()
  {
    return [SetTitle, SetColor, SetDescription, SetScopeField, SetExceptionsField];
  }

  protected void SetTitle<TState>(BuildActionArgs<TState> args)
  {
    var eventId = args.Entry.EventId;
    var eventDescription = eventId.Name is not null ? $"{eventId.Name}: {eventId.Id}" : eventId.Id.ToString();
    args.Builder.Title = $"[{LogLevelExpr}] {args.Entry.Category} ({eventDescription})";
  }

  protected void SetColor<TState>(BuildActionArgs<TState> args)
  {
    args.Builder.Color = Color;
  }

  protected static void SetDescription<TState>(BuildActionArgs<TState> args)
  {
    var entry = args.Entry;
    var description = entry.Formatter(entry.State, entry.Exception);
    args.Builder.Description = description;
  }

  protected static void SetScopeField<TState>(BuildActionArgs<TState> args)
  {
    if (args.ScopeProvider is null) return;
    var stringBuilder = new StringBuilder();
    args.ScopeProvider?.ForEachScope((scope, sb) =>
    {
      if (scope is null) return;
      sb.Append($" => {scope}");
    }, stringBuilder);
    if (stringBuilder.Length <= 0) return;
    stringBuilder.Remove(0, 4);
    args.Builder.AddField("Scope", stringBuilder.ToString());
  }

  protected void SetExceptionsField<TState>(BuildActionArgs<TState> args)
  {
    var ex = args.Entry.Exception;
    if (ex is null) return;
    var builder = args.Builder;
    builder.AddField("Error Message", ex.Message);
    if (ex.Source is not null) builder.AddField("Error Source", ex.Source);
    if (ex.InnerException is not null) builder.AddField("Inner Exception", ex.InnerException);
    if (ex.StackTrace is not null) builder.AddField("Error StackTrace", ex.StackTrace);
  }
}
