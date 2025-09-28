FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["ComplexTechnologicalCalculationsWebApp/ComplexTechnologicalCalculationsWebApp.csproj", "ComplexTechnologicalCalculationsWebApp/"]
RUN dotnet restore "ComplexTechnologicalCalculationsWebApp/ComplexTechnologicalCalculationsWebApp.csproj"
COPY . .
WORKDIR "/src/ComplexTechnologicalCalculationsWebApp"
RUN dotnet build "./ComplexTechnologicalCalculationsWebApp.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./ComplexTechnologicalCalculationsWebApp.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ComplexTechnologicalCalculationsWebApp.dll"]
