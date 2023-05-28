FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/Store/OnlineShop.Store.Migrations/OnlineShop.Store.Migrations.csproj", "src/Store/OnlineShop.Store.Migrations/"]
COPY ["src/Store/OnlineShop.Store.Api/OnlineShop.Store.Api.csproj", "src/Store/OnlineShop.Store.Api/"]
RUN dotnet restore "src/Store/OnlineShop.Store.Migrations/OnlineShop.Store.Migrations.csproj"
COPY . .
WORKDIR "/src/src/Store/OnlineShop.Store.Migrations"
RUN dotnet build "OnlineShop.Store.Migrations.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "OnlineShop.Store.Migrations.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "OnlineShop.Store.Migrations.dll"]
