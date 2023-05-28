FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/Orders/OnlineShop.Orders.Migrations/OnlineShop.Orders.Migrations.csproj", "src/Orders/OnlineShop.Orders.Migrations/"]
COPY ["src/Orders/OnlineShop.Orders.Api/OnlineShop.Orders.Api.csproj", "src/Orders/OnlineShop.Orders.Api/"]
RUN dotnet restore "src/Orders/OnlineShop.Orders.Migrations/OnlineShop.Orders.Migrations.csproj"
COPY . .
WORKDIR "/src/src/Orders/OnlineShop.Orders.Migrations"
RUN dotnet build "OnlineShop.Orders.Migrations.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "OnlineShop.Orders.Migrations.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "OnlineShop.Orders.Migrations.dll"]
