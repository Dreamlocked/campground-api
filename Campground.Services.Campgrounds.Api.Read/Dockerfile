#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Campground.Services.Campgrounds.Api.Read/Campground.Services.Campgrounds.Api.Read.csproj", "Campground.Services.Campgrounds.Api.Read/"]
COPY ["Campground.Services.Campgrounds.Infrastructure/Campground.Services.Campgrounds.Infrastructure.csproj", "Campground.Services.Campgrounds.Infrastructure/"]
COPY ["Campground.Services.Campgrounds.Domain/Campground.Services.Campgrounds.Domain.csproj", "Campground.Services.Campgrounds.Domain/"]
RUN dotnet restore "./Campground.Services.Campgrounds.Api.Read/Campground.Services.Campgrounds.Api.Read.csproj"
COPY . .
WORKDIR "/src/Campground.Services.Campgrounds.Api.Read"
RUN dotnet build "./Campground.Services.Campgrounds.Api.Read.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Campground.Services.Campgrounds.Api.Read.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Campground.Services.Campgrounds.Api.Read.dll"]