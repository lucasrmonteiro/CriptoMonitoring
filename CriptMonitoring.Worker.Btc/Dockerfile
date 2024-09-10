FROM mcr.microsoft.com/dotnet/runtime:8.0 AS base
USER $APP_UID
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["CriptMonitoring.Worker/CriptMonitoring.Worker.Btc.csproj", "CriptMonitoring.Worker.Btc/"]
COPY ["CriptMonitoring.Application/CriptMonitoring.Application.csproj", "CriptMonitoring.Application/"]
RUN dotnet restore "CriptMonitoring.Worker.Btc/CriptMonitoring.Worker.Btc.csproj"
COPY . .
WORKDIR "/src/CriptMonitoring.Worker.Btc"
RUN dotnet build "CriptMonitoring.Worker.Btc.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "CriptMonitoring.Worker.Btc.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CriptMonitoring.Worker.Btc.dll"]
