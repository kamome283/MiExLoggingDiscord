# MiExLoggingDiscord

[![NuGet](https://img.shields.io/nuget/v/kamome283.MiExLoggingDiscord.svg)](https://www.nuget.org/packages/kamome283.MiExLoggingDiscord/)

Logger and LoggerProvider Implementation for Discord in .NET 8

## How to use

Add a package reference to `kamome283.MiExLoggingDiscord` and `Microsoft.Extensions.Logging` if this
package is not implicitly referenced.
```shell
dotnet add package kamome283.MiExLoggingDiscord
dotnet add package Microsoft.Extensions.Logging # when it's not in your reference
```

and configure it in the builder.
```csharp
IHostBuilder someBuilder = new SOMEBUILDER();
someBuilder.ConfigureLogging(logging =>
{
    logging
        .AddDiscordLogger("YOUR WEBHOOK URL");
});
```