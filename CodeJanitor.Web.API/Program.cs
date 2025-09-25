namespace CodeJanitor.Web.API;

public static class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Configuration
            .AddJsonFile("appsettings.json", false, true)
            .AddEnvironmentVariables();

        builder.Services.AddSwaggerGen();

        builder.Services.Configure(builder.Configuration);

        var app = builder.Build();

        if (builder.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.MapControllers();

        app.UseHttpsRedirection();

        app.Run();
    }
}