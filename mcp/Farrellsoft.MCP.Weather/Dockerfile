# Build stage
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY ["Farrellsoft.MCP.Weather.csproj", "/src/"]
RUN dotnet restore "Farrellsoft.MCP.Weather.csproj"

COPY . .
RUN dotnet publish "Farrellsoft.MCP.Weather.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app

EXPOSE 8080

COPY --from=build /app/publish .

ENV ASPNETCORE_URLS=http://+:8080

CMD ["dotnet", "Farrellsoft.MCP.Weather.dll"]
