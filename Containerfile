FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY ./src ./src
RUN dotnet restore ./src/NotificationService.API/NotificationService.API.csproj
RUN dotnet publish ./src/NotificationService.API/NotificationService.API.csproj -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .
EXPOSE 8080
ENTRYPOINT ["sh", "-c", "dotnet NotificationService.API.dll --urls http://0.0.0.0:${PORT:-8080}"]
