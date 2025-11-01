using Discord;
using Microsoft.Extensions.Logging;

namespace MiExLoggingDiscord.EmbedsConstructor;

public static class DefaultEmbedConstructorConfigs
{
  public interface ITraceConfig : IEmbedConstructorConfig
  {
    LogLevel IEmbedConstructorConfig.TargetLogLevel => LogLevel.Trace;
    string IEmbedConstructorConfig.LogLevelExpr => "Trace";
    Color IEmbedConstructorConfig.Color => Color.DarkGrey;
  }

  public interface IDebugConfig : IEmbedConstructorConfig
  {
    LogLevel IEmbedConstructorConfig.TargetLogLevel => LogLevel.Debug;
    string IEmbedConstructorConfig.LogLevelExpr => "Debug";
    Color IEmbedConstructorConfig.Color => Color.LightGrey;
  }

  public interface IInformationConfig : IEmbedConstructorConfig
  {
    LogLevel IEmbedConstructorConfig.TargetLogLevel => LogLevel.Information;
    string IEmbedConstructorConfig.LogLevelExpr => "Info";
    Color IEmbedConstructorConfig.Color => Color.Green;
  }

  public interface IWarningConfig : IEmbedConstructorConfig
  {
    LogLevel IEmbedConstructorConfig.TargetLogLevel => LogLevel.Warning;
    string IEmbedConstructorConfig.LogLevelExpr => "Warn";
    Color IEmbedConstructorConfig.Color => Color.Gold;
  }

  public interface IErrorConfig : IEmbedConstructorConfig
  {
    LogLevel IEmbedConstructorConfig.TargetLogLevel => LogLevel.Error;
    string IEmbedConstructorConfig.LogLevelExpr => "ERROR";
    Color IEmbedConstructorConfig.Color => Color.Orange;
  }

  public interface ICriticalConfig : IEmbedConstructorConfig
  {
    LogLevel IEmbedConstructorConfig.TargetLogLevel => LogLevel.Critical;
    string IEmbedConstructorConfig.LogLevelExpr => "CRITICAL";
    Color IEmbedConstructorConfig.Color => Color.Red;
  }
}
