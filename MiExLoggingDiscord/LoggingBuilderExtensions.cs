using Discord.Webhook;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MiExLoggingDiscord.EmbedsConstructor;

namespace MiExLoggingDiscord;

public static class LoggingBuilderExtensions
{
  public static ILoggingBuilder AddDiscordLogger(
    this ILoggingBuilder builder,
    string webhookUrl,
    LogLevel mentionLogLevel = LogLevel.Error,
    params IEmbedConstructor[] additionalEmbedsConstructors)
  {
    var discordClient = new DiscordWebhookClient(webhookUrl);
    builder.Services.AddSingleton(discordClient);
    var embedsConstructors = additionalEmbedsConstructors.Concat(DefaultEmbedConstructors.GetInstances());
    builder.Services.AddSingleton<ILoggerProvider, DiscordLoggerProvider>(provider =>
    {
      var client = provider.GetRequiredService<DiscordWebhookClient>();
      return new DiscordLoggerProvider(embedsConstructors, mentionLogLevel, client);
    });
    return builder;
  }
}
