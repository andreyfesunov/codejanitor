using System.Formats.Tar;
using CodeJanitor.Docker.Domain.Repositories;
using CodeJanitor.Docker.Infrastructure.Factories;
using Docker.DotNet;
using Docker.DotNet.Models;

namespace CodeJanitor.Docker.Infrastructure.Repositories;

public sealed class DockerRepository(
    IWebhookUrlFactory webhookFactory,
    IDockerFactory dockerFactory
) : IDockerRepository
{
    public async Task CreateBugFixContainerAsync(
        Guid id,
        string url,
        string token,
        string title,
        string description
    )
    {
        var image = await dockerFactory.CreateImageReference();
        var container = await dockerFactory.CreateContainerReference(id);

        using var client = new DockerClientConfiguration().CreateClient();

        try
        {
            await client.Images.InspectImageAsync(image);
        }
        catch (DockerImageNotFoundException)
        {
            await BuildImage(client, image);
        }

        var createResponse = await client.Containers.CreateContainerAsync(new CreateContainerParameters
        {
            Image = image,
            Name = container,
            Env = new List<string>
            {
                $"REPOSITORY_URL={url}",
                $"ACCESS_TOKEN={token}",
                $"WEBHOOK_URL={await webhookFactory.CreateDockerUrl()}"
            },
            HostConfig = new HostConfig
            {
                NetworkMode = "host",
                AutoRemove = false // can toggle this option to remove container after it finishes entrypoint script
            }
        });

        await client.Containers.StartContainerAsync(createResponse.ID, new ContainerStartParameters());
    }

    public async Task DeleteContainerAsync(string containerId)
    {
        using var client = new DockerClientConfiguration().CreateClient();

        await client.Containers.StopContainerAsync(containerId, new ContainerStopParameters());
        await client.Containers.RemoveContainerAsync(containerId, new ContainerRemoveParameters());
    }

    private async Task BuildImage(
        DockerClient client,
        string image
    )
    {
        // TODO improve building Dockerfile - i don't like using relative path to get Dockerfile and build

        var projectPath = await dockerFactory.CreateRunnerReference();

        var tarStream = new MemoryStream();
        await TarFile.CreateFromDirectoryAsync(projectPath, tarStream, false);
        tarStream.Position = 0;

        await client.Images.BuildImageFromDockerfileAsync(
            new ImageBuildParameters
            {
                Dockerfile = "Dockerfile",
                Tags = new List<string> { image }
            },
            tarStream,
            [],
            new Dictionary<string, string>(),
            new Progress<JSONMessage>()
        );
    }
}