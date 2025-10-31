using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace MiExLoggingDiscord.Tests;

public class LoggerTests : IDisposable
{
  public LoggerTests()
  {
    var config = new ConfigurationBuilder()
      .AddUserSecrets(typeof(LoggerTests).Assembly)
      .Build();
    WebhookUrl = config["WebhookUrl"] ?? throw new KeyNotFoundException(nameof(WebhookUrl));
    LoggerConstructor = LoggerFactory.Create(builder =>
    {
      builder
        .AddDiscordLogger(WebhookUrl)
        .SetMinimumLevel(LogLevel.Trace);
    });
  }

  private Task Throttle => Task.Delay(1000);

  private string WebhookUrl { get; }
  private ILoggerFactory LoggerConstructor { get; }

  public void Dispose()
  {
    LoggerConstructor.Dispose();
    GC.SuppressFinalize(this);
  }

  [Test]
  [Category("WebRequest")]
  public async Task LoggerWorksAsDesired()
  {
    var logger = LoggerConstructor.CreateLogger("Default");
    logger.LogTrace("log at {Time}", DateTimeOffset.Now);
    await Throttle;
  }

  [Test]
  [Category("WebRequest")]
  public async Task GenericLoggerWorksAsDesired()
  {
    var logger = LoggerConstructor.CreateLogger<object>();
    logger.LogTrace("log from generic logger at {Time}", DateTimeOffset.Now);
    await Throttle;
  }

  [Test]
  [Category("WebRequest")]
  public async Task LoggingWithScopeWorksAsDesired()
  {
    var logger = LoggerConstructor.CreateLogger("With Scope");
    using var _ = logger.BeginScope("Scoped");
    logger.LogDebug("Log with scope at {Time}", DateTimeOffset.Now);
    await Throttle;
  }

  [Test]
  [Category("WebRequest")]
  public async Task LoggingWithEventIdWorksAsDesired()
  {
    var logger = LoggerConstructor.CreateLogger("With Event Id");
    logger.LogInformation(new EventId(1), "log with event id at {Time}", DateTimeOffset.Now);
    await Throttle;
  }

  [Test]
  [Category("WebRequest")]
  public async Task LoggingWithExceptionWorksAsDesired()
  {
    var logger = LoggerConstructor.CreateLogger("With Exception");
    var ex = new InvalidOperationException("Some Exception");
    logger.LogWarning(ex, "log with exception at {Time}", DateTimeOffset.Now);
    await Throttle;
  }

  [Test]
  [Category("WebRequest")]
  public async Task LoggingWithNestedExceptionWorksAsDesired()
  {
    var logger = LoggerConstructor.CreateLogger("With Nested Exception");
    var ex = new InvalidOperationException("Nested Exception", new KeyNotFoundException("Some Key"));
    logger.LogError(ex, "log with nested exception at {Time}", DateTimeOffset.Now);
    await Throttle;
  }

  [Test]
  [Category("WebRequest")]
  public async Task FullSpecLoggingWorksAsDesired()
  {
    var logger = LoggerConstructor.CreateLogger<object>();
    using var outerScope = logger.BeginScope("outer");
    using var innerScope = logger.BeginScope("inner");
    var ex = new InvalidOperationException("Nested Exception", new KeyNotFoundException("Some Key"));
    logger.LogCritical(new EventId(1, "Full"), ex, "Full spec logging at {Time}", DateTimeOffset.Now);
    await Throttle;
  }
}
