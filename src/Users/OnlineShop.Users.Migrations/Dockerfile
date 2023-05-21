FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/Users/OnlineShop.Users.Migrations/OnlineShop.Users.Migrations.csproj", "src/Users/OnlineShop.Users.Migrations/"]
COPY ["src/Users/OnlineShop.Users.Api/OnlineShop.Users.Api.csproj", "src/Users/OnlineShop.Users.Api/"]
RUN dotnet restore "src/Users/OnlineShop.Users.Migrations/OnlineShop.Users.Migrations.csproj"
COPY . .
WORKDIR "/src/src/Users/OnlineShop.Users.Migrations"
RUN dotnet build "OnlineShop.Users.Migrations.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "OnlineShop.Users.Migrations.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "OnlineShop.Users.Migrations.dll"]