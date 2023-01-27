FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /source

COPY *.sln .
COPY SendColorBot/ ./SendColorBot
RUN dotnet restore -r linux-musl-x64

WORKDIR /source/SendColorBot
RUN dotnet publish -c release -o /app -r linux-musl-x64 --self-contained false --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:7.0-alpine AS final
WORKDIR /app
COPY --from=build /app ./

ENTRYPOINT ["./SendColorBot"]