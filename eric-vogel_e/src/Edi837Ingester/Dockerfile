FROM mcr.microsoft.com/dotnet/runtime:8.0 AS base
USER $APP_UID
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Edi837Ingester/Edi837Ingester.csproj", "Edi837Ingester/"]
RUN dotnet restore "Edi837Ingester/Edi837Ingester.csproj"
COPY . .
WORKDIR "/src/Edi837Ingester"
RUN dotnet build "./Edi837Ingester.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Edi837Ingester.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Edi837Ingester.dll"]
