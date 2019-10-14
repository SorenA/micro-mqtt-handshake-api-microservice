FROM mcr.microsoft.com/dotnet/core/aspnet:3.0-buster-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.0-buster AS build
WORKDIR /src
COPY ["src/MicroMQTT.Microservice.HandshakeAPI/MicroMQTT.Microservice.HandshakeAPI.csproj", "src/MicroMQTT.Microservice.HandshakeAPI/"]
RUN dotnet restore "src/MicroMQTT.Microservice.HandshakeAPI/MicroMQTT.Microservice.HandshakeAPI.csproj"
COPY . .
WORKDIR "/src/src/MicroMQTT.Microservice.HandshakeAPI"
RUN dotnet build "MicroMQTT.Microservice.HandshakeAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MicroMQTT.Microservice.HandshakeAPI.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MicroMQTT.Microservice.HandshakeAPI.dll"]