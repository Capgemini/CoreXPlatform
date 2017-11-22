FROM microsoft/dotnet:latest as build-env

WORKDIR /app

# copy the csproj to create a layer with dependencies only
COPY ./src/CoreXPlatform.API/*.csproj ./src/CoreXPlatform.API/

RUN dotnet restore ./src/CoreXPlatform.API/CoreXPlatform.API.csproj

# copy everything else and publish
COPY . ./

RUN dotnet publish ./src/CoreXPlatform.API/CoreXPlatform.API.csproj -c Release -o ../../out /p:LinkDuringPublish=true

# build the runtime image
FROM microsoft/aspnetcore:latest

WORKDIR /app

COPY --from=build-env /app/out ./

EXPOSE 5000

ENTRYPOINT [ "dotnet", "CoreXPlatform.API.dll" ]