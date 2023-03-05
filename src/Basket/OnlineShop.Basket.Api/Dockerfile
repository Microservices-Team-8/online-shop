#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["OnlineShop.Basket.Api.csproj", "/src/Basket/OnlineShop.Basket.Api/"]
RUN dotnet restore "/src/Basket/OnlineShop.Basket.Api/OnlineShop.Basket.Api.csproj"
WORKDIR "/src/Basket/OnlineShop.Basket.Api"
COPY . .
RUN dotnet build "OnlineShop.Basket.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "OnlineShop.Basket.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "OnlineShop.Basket.Api.dll"]