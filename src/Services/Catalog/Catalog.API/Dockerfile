FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src

# It's important to keep lines from here down to "COPY . ." identical in all Dockerfiles
# to take advantage of Docker's build cache, to speed up local container builds
COPY "Audiophile.sln" "Audiophile.sln"

COPY "Services/Basket/Basket.API/Basket.API.csproj" "Services/Basket/Basket.API/Basket.API.csproj"
COPY "Services/Identity/Identity.Server/Identity.Server.csproj" "Services/Identity/Identity.Server/Identity.Server.csproj"
COPY "Services/Catalog/Catalog.API/Catalog.API.csproj" "Services/Catalog/Catalog.API/Catalog.API.csproj"
COPY "Services/Catalog/Catalog.Core/Catalog.Core.csproj" "Services/Catalog/Catalog.Core/Catalog.Core.csproj"
COPY "Services/Catalog/Catalog.Infrastructure/Catalog.Infrastructure.csproj" "Services/Catalog/Catalog.Infrastructure/Catalog.Infrastructure.csproj"
COPY "Services/Catalog/Catalog.Tests.Integration/Catalog.API.Tests.Integration.csproj" "Services/Catalog/Catalog.Tests.Integration/Catalog.API.Tests.Integration.csproj"
COPY "Services/Service.Common/Service.Common.csproj" "Services/Service.Common/Service.Common.csproj"
COPY "docker-compose.dcproj" "docker-compose.dcproj"
COPY "Directory.Packages.props" "Directory.Packages.props"
COPY "NuGet.config" "NuGet.config"

RUN dotnet restore "Audiophile.sln"

COPY . .
WORKDIR /src/Services/Catalog/Catalog.API
RUN dotnet publish --no-restore -c Release -o /app

FROM build AS publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Catalog.API.dll"]