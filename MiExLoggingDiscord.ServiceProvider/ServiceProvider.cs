using Discord.Webhook;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace MiExLoggingDiscord.ServiceProvider;

internal class DiscordLoggerProvider(
  IExternalScopeProvider? scopeProvider,
  IEnumerable<IEmbedsConstructor> embedsConstructors,
  DiscordWebhookClient discordClient) : ILoggerProvider
{
  public void Dispose() => GC.SuppressFinalize(this);

  public ILogger CreateLogger(string categoryName) =>
    new DiscordLogger(categoryName, scopeProvider, embedsConstructors, discordClient);
}

public static class DiscordLoggerServiceProvider
{
  public static ILoggingBuilder AddDiscordLogger(this ILoggingBuilder builder, string webhookUrl)
  {
    var webhookClient = new DiscordWebhookClient(webhookUrl);
    builder.Services.AddSingleton(webhookClient);
    builder.Services.AddSingleton<ILoggerProvider, DiscordLoggerProvider>();
    return builder;
  }
}
