FROM mcr.microsoft.com/dotnet/aspnet:5.0.2 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:5.0.102-1 AS build
WORKDIR /src
COPY src/RabbitMQ.csproj RabbitMQ.csproj
RUN dotnet restore RabbitMQ.csproj
COPY src/. .
RUN dotnet build RabbitMQ.csproj -c Release -o /app/build
RUN dotnet publish RabbitMQ.csproj -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "RabbitMQ.dll"]