# Base runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["EnersoftSensorsManagementSystem.API/EnersoftSensorsManagementSystem.API.csproj", "EnersoftSensorsManagementSystem.API/"]
RUN dotnet restore "./EnersoftSensorsManagementSystem.API/EnersoftSensorsManagementSystem.API.csproj"
COPY . .
WORKDIR "/src/EnersoftSensorsManagementSystem.API"
RUN dotnet build "./EnersoftSensorsManagementSystem.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Publish stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./EnersoftSensorsManagementSystem.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Final stage
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENV POSTGRES_CONNECTION="Host=postgres-db;Port=5432;Database=sensors;Username=postgres;Password=postgres"
ENV MYSQL_CONNECTION="Server=mysql-db;Port=3306;Database=sensors;User=root;Password=root"
ENV MSSQL_CONNECTION="Server=mssql-db,1433;Database=sensors;User Id=sa;Password=StrongPassword123!"
ENTRYPOINT ["dotnet", "EnersoftSensorsManagementSystem.API.dll"]

# Healthcheck
HEALTHCHECK --interval=30s --timeout=10s --retries=3 CMD curl --fail http://localhost:8080/health || exit 1
