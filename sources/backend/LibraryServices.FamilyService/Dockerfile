FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS publish
WORKDIR /src
COPY ["LibraryServices.FamilyService/LibraryServices.FamilyService.csproj", "LibraryServices.FamilyService/"]
COPY ["LibraryServices.Domain/LibraryServices.Domain.csproj", "LibraryServices.Domain/"]
COPY ["LibraryServices.Infrastructure/LibraryServices.Infrastructure.csproj", "LibraryServices.Infrastructure/"]
COPY . .
WORKDIR "/src/LibraryServices.FamilyService"
RUN dotnet publish "LibraryServices.FamilyService.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
VOLUME ["/app/logs"]
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "LibraryServices.FamilyService.dll"]