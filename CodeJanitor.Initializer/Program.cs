using CodeJanitor.Initializer.Helpers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace CodeJanitor.Initializer;

internal static class Program
{
    public static async Task Main(string[] args)
    {
        try
        {
            using var host = CreateHostBuilder(args).Build();

            using var scope = host.Services.CreateScope();
            var services = scope.ServiceProvider;

            var commandExecutor = services.GetRequiredService<CommandExecutor>();
            await commandExecutor.RunAsync(args);

            await host.StopAsync();
        }
        catch (Exception ex)
        {
            ConsoleHelper.WriteLine(ConsoleColor.DarkRed, $"Stopped program because of exception: {ex}");

            throw;
        }
    }

    private static IHostBuilder CreateHostBuilder(string[] args)
    {
        var typedArgs = CommandArgs.Parse(args);

        return Host
            .CreateDefaultBuilder(args)
            .UseDefaultServiceProvider(options =>
                {
                    options.ValidateScopes = true;
                    options.ValidateOnBuild = true;
                }
            )
            .UseEnvironment(typedArgs.Environment)
            .ConfigureAppConfiguration((_, configurationBuilder) =>
                {
                    configurationBuilder.AddJsonFile("appsettings.json", false, true);
                    configurationBuilder.AddEnvironmentVariables();
                }
            ).ConfigureServices(Startup.Configure)
            .ConfigureLogging(logging =>
                {
                    logging.AddConsole();
                    logging.SetMinimumLevel(LogLevel.Warning);
                }
            );
    }
}