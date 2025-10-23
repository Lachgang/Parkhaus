# Unter https://aka.ms/customizecontainer erfahren Sie, wie Sie Ihren Debugcontainer anpassen und wie Visual Studio dieses Dockerfile verwendet, um Ihre Images für ein schnelleres Debuggen zu erstellen.

# Diese Stufe wird verwendet, wenn sie von VS im Schnellmodus ausgeführt wird (Standardeinstellung für Debugkonfiguration).
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 80


# Diese Stufe wird zum Erstellen des Dienstprojekts verwendet.
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY ["ParkhausAPI/ParkhausAPI.csproj", "ParkhausAPI/"]
RUN dotnet restore "./ParkhausAPI/ParkhausAPI.csproj"
COPY . .
WORKDIR "/src/ParkhausAPI"
RUN dotnet build "./ParkhausAPI.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Diese Stufe wird verwendet, um das Dienstprojekt zu veröffentlichen, das in die letzte Phase kopiert werden soll.
FROM build AS publish
RUN dotnet publish "./ParkhausAPI.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Diese Stufe wird in der Produktion oder bei Ausführung von VS im regulären Modus verwendet (Standard, wenn die Debugkonfiguration nicht verwendet wird).
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ParkhausAPI.dll"]