FROM mcr.microsoft.com/dotnet/core/aspnet:3.0.0-buster-slim-arm32v7
WORKDIR /app
COPY out .
ENTRYPOINT ["dotnet", "Distvisor.Web.dll"]