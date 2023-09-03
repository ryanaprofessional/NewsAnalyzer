FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS publish
WORKDIR /src
COPY NewsAnalyzer.csproj .
RUN dotnet restore "NewsAnalyzer.csproj"
COPY . .
RUN dotnet publish "NewsAnalyzer.csproj" -c RELEASE -o /app/publish /p:UseAppHost=false

FROM base as final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "NewsAnalyzer.dll"]
