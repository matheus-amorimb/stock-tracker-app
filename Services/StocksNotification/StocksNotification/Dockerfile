FROM mcr.microsoft.com/dotnet/runtime:8.0 AS base
USER $APP_UID
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Services/StocksNotification/StocksNotification/StocksNotification.csproj", "Services/StocksNotification/StocksNotification/"]
RUN dotnet restore "Services/StocksNotification/StocksNotification/StocksNotification.csproj"
COPY . .
WORKDIR "/src/Services/StocksNotification/StocksNotification"
RUN dotnet build "StocksNotification.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "StocksNotification.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "StocksNotification.dll"]
