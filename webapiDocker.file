#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY *.sln .
COPY ./Core ./Core
COPY ./NetCoreWebAPI ./NetCoreWebAPI

RUN dotnet restore ./NetCoreWebAPI/NetCoreWebAPI.csproj


FROM build AS publish
RUN dotnet publish ./NetCoreWebAPI/NetCoreWebAPI.csproj -c Release -o /app/publish 

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

EXPOSE 7070
ENTRYPOINT ["dotnet", "NetCoreWebAPI/NetCoreWebAPI.dll"]