FROM mcr.microsoft.com/dotnet/runtime:8.0 AS base
USER $APP_UID
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["CriptMonitoring.Worker.Eth/CriptMonitoring.Worker.Eth.csproj", "CriptMonitoring.Worker.Eth/"]
COPY ["CriptMonitoring.Application/CriptMonitoring.Application.csproj", "CriptMonitoring.Application/"]
COPY ["CriptMonitoring.Data/CriptMonitoring.Data.csproj", "CriptMonitoring.Data/"]
COPY ["CriptMonitoring.Domain/CriptMonitoring.Domain.csproj", "CriptMonitoring.Domain/"]
COPY ["CriptMonitoring.Infra/CriptMonitoring.Infra.csproj", "CriptMonitoring.Infra/"]
RUN dotnet restore "CriptMonitoring.Worker.Eth/CriptMonitoring.Worker.Eth.csproj"
COPY . .
WORKDIR "/src/CriptMonitoring.Worker.Eth"
RUN dotnet build "CriptMonitoring.Worker.Eth.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "CriptMonitoring.Worker.Eth.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CriptMonitoring.Worker.Eth.dll"]
