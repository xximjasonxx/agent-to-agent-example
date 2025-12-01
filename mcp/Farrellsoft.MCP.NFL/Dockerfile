# Build stage
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy csproj and restore dependencies
COPY ["Farrellsoft.MCP.NFL.csproj", "/src/"]
RUN dotnet restore "Farrellsoft.MCP.NFL.csproj"

# Copy everything else and build
COPY . .
RUN dotnet publish "Farrellsoft.MCP.NFL.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app

# Copy published app
COPY --from=build /app/publish .

# Expose port
EXPOSE 8080

# Set environment variables
ENV ASPNETCORE_URLS=http://+:8080
ENV ASPNETCORE_ENVIRONMENT=Production

# Run the application
ENTRYPOINT ["dotnet", "Farrellsoft.MCP.NFL.dll"]
