FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/Baskets/OnlineShop.Baskets.Migrations/OnlineShop.Baskets.Migrations.csproj", "src/Baskets/OnlineShop.Baskets.Migrations/"]
COPY ["src/Baskets/OnlineShop.Baskets.Api/OnlineShop.Baskets.Api.csproj", "src/Baskets/OnlineShop.Baskets.Api/"]
RUN dotnet restore "src/Baskets/OnlineShop.Baskets.Migrations/OnlineShop.Baskets.Migrations.csproj"
COPY . .
WORKDIR "/src/src/Baskets/OnlineShop.Baskets.Migrations"
RUN dotnet build "OnlineShop.Baskets.Migrations.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "OnlineShop.Baskets.Migrations.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "OnlineShop.Baskets.Migrations.dll"]
