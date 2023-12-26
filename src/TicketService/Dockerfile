FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src

COPY ["src/TicketService/TicketService.csproj", "TicketService/"]
COPY ["src/Kernel/Kernel.csproj", "Kernel/"]

RUN dotnet restore "/src/TicketService"
COPY /src .
WORKDIR "/src/TicketService"
RUN dotnet build -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TicketService.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TicketService.dll"]