FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
RUN apt-get update && apt-get install -y git
WORKDIR /src
COPY . .
RUN dotnet restore
RUN dotnet publish -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/runtime:9.0
RUN apt-get update && apt-get install -y git
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "CodeJanitor.Docker.Infrastructure.Runner.dll"]