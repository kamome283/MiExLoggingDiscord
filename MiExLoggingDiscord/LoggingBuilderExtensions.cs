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
    var webhookClient = new DiscordWebhookClient(webhookUrl);
    builder.Services.AddSingleton(webhookClient);
    var embedsConstructors = additionalEmbedsConstructors.Concat(DefaultEmbedsConstructors.Instances);
    builder.Services.AddSingleton(embedsConstructors);
    builder.Services.AddSingleton<ILoggerProvider, DiscordLoggerProvider>();
    return builder;
  }
}
