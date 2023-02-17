FROM mcr.microsoft.com/dotnet/sdk:6.0-alpine AS build-env
WORKDIR /App

# Copy everything
COPY . ./
# Restore as distinct layers
RUN dotnet restore
# Build and publish a release
RUN dotnet publish -p:PublishProfile=LinuxARM64,DebugType=None -o out

# Build runtime image
FROM alpine
WORKDIR /App
COPY --from=build-env /App/out .
ENTRYPOINT ["MDNSRepeater"]