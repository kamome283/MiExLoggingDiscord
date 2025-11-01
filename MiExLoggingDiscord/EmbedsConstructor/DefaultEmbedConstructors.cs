using static MiExLoggingDiscord.EmbedsConstructor.DefaultEmbedConstructorConfigs;

namespace MiExLoggingDiscord.EmbedsConstructor;

public static class DefaultEmbedConstructors
{
  public static IEnumerable<IDefaultEmbedConstructor> GetInstances()
  {
    return
    [
      new TraceConstructor(),
      new DebugConstructor(),
      new InformationConstructor(),
      new WarningConstructor(),
      new ErrorConstructor(),
      new CriticalConstructor(),
    ];
  }

  public class TraceConstructor : IDefaultEmbedConstructor, ITraceConfig;
  public class DebugConstructor : IDefaultEmbedConstructor, IDebugConfig;
  public class InformationConstructor : IDefaultEmbedConstructor, IInformationConfig;
  public class WarningConstructor : IDefaultEmbedConstructor, IWarningConfig;
  public class ErrorConstructor : IDefaultEmbedConstructor, IErrorConfig;
  public class CriticalConstructor : IDefaultEmbedConstructor, ICriticalConfig;
}
