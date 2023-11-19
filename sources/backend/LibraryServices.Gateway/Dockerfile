FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS publish
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["LibraryServices.Gateway/LibraryServices.Gateway.csproj", "LibraryServices.Gateway/"]
COPY ["LibraryServices.Infrastructure/LibraryServices.Infrastructure.csproj", "LibraryServices.Infrastructure/"]
RUN dotnet restore "./LibraryServices.Gateway/./LibraryServices.Gateway.csproj"
COPY . .
WORKDIR "/src/LibraryServices.Gateway"
RUN dotnet publish "./LibraryServices.Gateway.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
VOLUME ["/app/logs"]
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "LibraryServices.Gateway.dll"]