FROM --platform=linux/amd64 mcr.microsoft.com/dotnet/aspnet:5.0
WORKDIR /app
COPY out .
EXPOSE 80
ENTRYPOINT ["dotnet", "Distvisor.Web.dll"]
