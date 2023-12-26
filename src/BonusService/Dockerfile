FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src

COPY ["src/BonusService/PrivilegeService.csproj", "BonusService/"]
COPY ["src/Kernel/Kernel.csproj", "Kernel/"]

RUN dotnet restore "BonusService"
COPY /src .
WORKDIR "/src/BonusService"
RUN dotnet build -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "PrivilegeService.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PrivilegeService.dll"]