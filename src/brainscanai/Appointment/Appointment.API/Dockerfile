# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081


# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Appointment/Appointment.API/Appointment.API.csproj", "Appointment/Appointment.API/"]
COPY ["Appointment/Appointment.Application/Appointment.Application.csproj", "Appointment/Appointment.Application/"]
COPY ["BuildingBlocks/BuildingBlocks.Messaging/BuildingBlocks.Messaging.csproj", "BuildingBlocks/BuildingBlocks.Messaging/"]
COPY ["BuildingBlocks/BuildingBlocks/BuildingBlocks.csproj", "BuildingBlocks/BuildingBlocks/"]
COPY ["Appointment/Appointment.Domain/Appointment.Domain.csproj", "Appointment/Appointment.Domain/"]
COPY ["Appointment/Appointment.Infrastructure/Appointment.Infrastructure.csproj", "Appointment/Appointment.Infrastructure/"]
RUN dotnet restore "./Appointment/Appointment.API/Appointment.API.csproj"
COPY . .
WORKDIR "/src/Appointment/Appointment.API"
RUN dotnet build "./Appointment.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Appointment.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Appointment.API.dll"]