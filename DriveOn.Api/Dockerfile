# Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copia a solução inteira (todas as camadas)
COPY . .

# Restaura e publica
RUN dotnet restore DriveOn.Api/DriveOn.Api.csproj
RUN dotnet publish DriveOn.Api/DriveOn.Api.csproj -c Release -o /app/publish /p:UseAppHost=false

# Run
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/publish .
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080
ENTRYPOINT ["dotnet", "DriveOn.Api.dll"]
