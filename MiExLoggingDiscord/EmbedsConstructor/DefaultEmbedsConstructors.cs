using Discord;
using Microsoft.Extensions.Logging;

namespace MiExLoggingDiscord.EmbedsConstructor;

public static class DefaultEmbedsConstructors
{
  public static readonly IEnumerable<EmbedsConstructor> Instances =
    Configs.AllConfigs.Select(config => new EmbedsConstructor { Config = config });

  public static class Configs
  {
    public static readonly EmbedsConstructorConfig TraceConfig =
      new(LogLevel.Trace, "Trace", Color.DarkGrey);

    public static readonly EmbedsConstructorConfig DebugConfig =
      new(LogLevel.Debug, "Debug", Color.LightGrey);

    public static readonly EmbedsConstructorConfig InfoConfig =
      new(LogLevel.Information, "Info", Color.Green);

    public static readonly EmbedsConstructorConfig WarnConfig =
      new(LogLevel.Warning, "Warn", Color.Gold);

    public static readonly EmbedsConstructorConfig ErrorConfig =
      new(LogLevel.Error, "ERROR", Color.Orange);

    public static readonly EmbedsConstructorConfig CriticalConfig =
      new(LogLevel.Critical, "CRITICAL", Color.Red);

    public static EmbedsConstructorConfig[] AllConfigs =>
      [TraceConfig, DebugConfig, InfoConfig, WarnConfig, ErrorConfig, CriticalConfig];
  }
}
