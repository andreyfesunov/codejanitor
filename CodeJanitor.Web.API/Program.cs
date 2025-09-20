namespace CodeJanitor.Web.API;

public static class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddOpenApi();

        builder.Services.Configure();

        builder.Configuration
            .AddJsonFile("appsettings.json", false, true)
            .AddEnvironmentVariables();

        var app = builder.Build();

        if (app.Environment.IsDevelopment()) app.MapOpenApi();

        app.UseHttpsRedirection();

        app.Run();
    }
}