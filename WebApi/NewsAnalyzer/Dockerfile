FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443
COPY . .
RUN rm Dockerfile
ENTRYPOINT ["dotnet", "NewsAnalyzer.dll"]
