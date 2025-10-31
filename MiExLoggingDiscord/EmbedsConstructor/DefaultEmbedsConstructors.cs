using Discord;
using Microsoft.Extensions.Logging;

namespace MiExLoggingDiscord.EmbedsConstructor;

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
