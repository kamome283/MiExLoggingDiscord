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
    params IEmbedsConstructor[] additionalEmbedsConstructors)
  {
    var discordClient = new DiscordWebhookClient(webhookUrl);
    builder.Services.AddSingleton(discordClient);
    var embedsConstructors = additionalEmbedsConstructors.Concat(DefaultEmbedsConstructors.Instances);
    builder.Services.AddSingleton<ILoggerProvider, DiscordLoggerProvider>(provider =>
    {
      var client = provider.GetRequiredService<DiscordWebhookClient>();
      return new DiscordLoggerProvider(embedsConstructors, client);
    });
    return builder;
  }
}
