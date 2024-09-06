# Use the official .NET SDK image for building your application
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copy the project file for SensorModul and restore dependencies
COPY SensorModul/SensorModul.csproj ./SensorModul/
WORKDIR /app/SensorModul
RUN dotnet restore

# Copy the remaining source code and build the application
COPY SensorModul/ ./SensorModul/
RUN dotnet publish -c Release -o /app/out

# Build the runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/out ./

# Set the entry point for the application
ENTRYPOINT ["dotnet", "SensorModul.dll"]