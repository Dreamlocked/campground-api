#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Campground.Services.Authentications/Campground.Services.Authentications.csproj", "Campground.Services.Authentications/"]
COPY ["Campground.Shared.Authentication/Campground.Shared.Authentication.csproj", "Campground.Shared.Authentication/"]
RUN dotnet restore "./Campground.Services.Authentications/Campground.Services.Authentications.csproj"
COPY . .
WORKDIR "/src/Campground.Services.Authentications"
RUN dotnet build "./Campground.Services.Authentications.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Campground.Services.Authentications.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Campground.Services.Authentications.dll"]