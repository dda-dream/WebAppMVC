# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 5050
EXPOSE 5055


# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["WEBAPPMVC/WebAppMVC.csproj", "WebAppMVC/"]
COPY ["WebApp_DataAccess/WebApp_DataAccess.csproj", "WebApp_DataAccess/"]
COPY ["WebApp_Models/WebApp_Models.csproj", "WebApp_Models/"]
COPY ["WEBAPP_Utility/WebApp_Utility.csproj", "WEBAPP_Utility/"]
RUN dotnet restore "./WEBAPPMVC/WebAppMVC.csproj"
COPY . .
WORKDIR "/src/WebAppMVC"
RUN dotnet build "./WebAppMVC.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./WebAppMVC.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "WebAppMVC.dll"]