FROM mcr.microsoft.com/dotnet/core/aspnet:5.0.0-buster-slim
WORKDIR /app
COPY out .
EXPOSE 80
ENTRYPOINT ["dotnet", "Distvisor.Web.dll"]
