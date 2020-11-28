FROM mcr.microsoft.com/dotnet/aspnet:5.0.0-buster-slim-arm32v7
WORKDIR /app
COPY out .
EXPOSE 80
ENTRYPOINT ["dotnet", "Distvisor.Web.dll"]
