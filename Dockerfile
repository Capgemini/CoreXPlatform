FROM microsoft/dotnet:latest as build-env

WORKDIR /app

COPY ./src/CoreXPlatform.API/*.csproj ./src/CoreXPlatform.API/

RUN dotnet restore ./src/CoreXPlatform.API/CoreXPlatform.API.csproj

