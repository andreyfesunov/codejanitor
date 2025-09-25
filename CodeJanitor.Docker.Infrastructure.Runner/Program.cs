using System.Diagnostics;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace CodeJanitor.Docker.Infrastructure.Runner;

public static class Program
{
    public static async Task Main()
    {
        var repoUrl = Environment.GetEnvironmentVariable("REPOSITORY_URL") ??
                      throw new ArgumentException("REPOSITORY_URL is required");
        var accessToken = Environment.GetEnvironmentVariable("ACCESS_TOKEN") ??
                          throw new ArgumentException("ACCESS_TOKEN is required");
        var webhookUrl = Environment.GetEnvironmentVariable("WEBHOOK_URL") ??
                         throw new ArgumentException("WEBHOOK_URL is required");

        // TODO add adapter for different providers like github, gitlab, etc.
        var cloneUrl = repoUrl.Replace("https://", $"https://oauth2:{accessToken}@") + ".git";
        Console.WriteLine($"Cloning repository from {cloneUrl}...");

        var gitProcess = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = "git",
                Arguments = $"clone {cloneUrl}",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false
            }
        };

        gitProcess.Start();
        _ = await gitProcess.StandardOutput.ReadToEndAsync();
        var error = await gitProcess.StandardError.ReadToEndAsync();
        await gitProcess.WaitForExitAsync();

        if (gitProcess.ExitCode != 0)
        {
            Console.WriteLine($"Error cloning repo: {error}");
            return;
        }

        Console.WriteLine("Repository cloned successfully.");

        Console.WriteLine("Starting sleep for 25 seconds...");
        Thread.Sleep(25000);
        Console.WriteLine("Sleep completed.");

        var containerId = GetContainerId();
        Console.WriteLine($"Container ID: {containerId}");

        var payload = new { containerId };
        var jsonPayload = JsonSerializer.Serialize(payload);

        using var client = new HttpClient();
        var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");
        var response = await client.PostAsync(webhookUrl, content);

        Console.WriteLine(response.IsSuccessStatusCode
            ? "Payload sent successfully."
            : $"Error sending payload: {response.StatusCode}");
    }

    private static string? GetContainerId()
    {
        if (!File.Exists("/proc/self/mountinfo")) return null;

        var mountinfo = File.ReadAllText("/proc/self/mountinfo");
        var match = Regex.Match(mountinfo, "/docker/containers/([0-9a-f]{64})/");

        return match.Success ? match.Groups[1].Value : null;
    }
}