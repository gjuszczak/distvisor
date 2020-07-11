FROM mcr.microsoft.com/dotnet/core/aspnet:3.1.5-buster-slim
WORKDIR /app
COPY out .
EXPOSE 80
ENTRYPOINT ["dotnet", "Distvisor.Web.dll"]
