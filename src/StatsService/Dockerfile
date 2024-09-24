FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src

COPY ["src/StatsService/StatsService.csproj", "StatsService/"]
COPY ["src/Kernel/Kernel.csproj", "Kernel/"]

RUN dotnet restore "StatsService"
COPY ./src .
WORKDIR "/src/StatsService"
RUN dotnet build -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "StatsService.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "StatsService.dll"]