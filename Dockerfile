# https://hub.docker.com/_/microsoft-dotnet
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /source

# copy and publish app and libraries
COPY . .
RUN dotnet publish -c Release -o /app

# final stage/image
FROM mcr.microsoft.com/dotnet/runtime:6.0-alpine
WORKDIR /app
COPY --from=build /app .
ENV mDNSRepeater_UCRListenPort=5354 mDNSRepeater_UCSDestPort=5354 mDNSRepeater_UCSDestAddresses=1.1.1.1
ENTRYPOINT ["dotnet", "MDNSRepeater.dll"]