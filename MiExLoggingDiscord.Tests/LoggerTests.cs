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
    LoggerConstructor = LoggerFactory.Create(builder => { builder.AddDiscordLogger(WebhookUrl); });
  }

  private string WebhookUrl { get; }
  private ILoggerFactory LoggerConstructor { get; }

  public void Dispose()
  {
    LoggerConstructor.Dispose();
    GC.SuppressFinalize(this);
  }

  [Test]
  public void CanConstructLogger()
  {
    Assert.DoesNotThrow(() => { _ = LoggerConstructor.CreateLogger("Default"); });
  }

  [Test]
  public void CanConstructGenericLogger()
  {
    Assert.DoesNotThrow(() => { _ = LoggerConstructor.CreateLogger<object>(); });
  }
}
