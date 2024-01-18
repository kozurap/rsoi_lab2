FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 8060
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src

COPY ["src/FlightService/FlightService.csproj", "FlightService/"]
COPY ["src/Kernel/Kernel.csproj", "Kernel/"]

RUN dotnet restore "FlightService"
COPY /src .
WORKDIR "/src/FlightService"
RUN dotnet build -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "FlightService.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "FlightService.dll"]