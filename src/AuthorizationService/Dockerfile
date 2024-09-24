FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src

COPY ["src/AuthorizationService/AuthorizationService.csproj", "AuthorizationService/"]
COPY ["src/Kernel/Kernel.csproj", "Kernel/"]

RUN dotnet restore "AuthorizationService"
COPY ./src .
WORKDIR "/src/AuthorizationService"
RUN dotnet build -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "AuthorizationService.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AuthorizationService.dll"]