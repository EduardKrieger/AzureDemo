FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /app

COPY SensorModul/SensorModul.csproj ./SensorModul/
RUN dotnet restore ./SensorModul/SensorModul.csproj

COPY SensorModul/ ./SensorModul/
RUN dotnet publish ./SensorModul/SensorModul.csproj -c Release -o /app/out


FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime

WORKDIR /app
COPY --from=build /app/out ./

ENTRYPOINT ["dotnet", "SensorModul.dll"]