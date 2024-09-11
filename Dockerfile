FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

COPY SensorModul/SensorModul.csproj ./SensorModul/

WORKDIR /app/SensorModul
RUN dotnet restore


COPY SensorModul/ ./SensorModul/
RUN dotnet publish -c Release -o /app/out

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/out ./


ENTRYPOINT ["dotnet", "SensorModul.dll"]
