FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/UI/OnlineShop.UI.Console/OnlineShop.UI.Console.csproj", "src/UI/OnlineShop.UI.Console/"]
RUN dotnet restore "src/UI/OnlineShop.UI.Console/OnlineShop.UI.Console.csproj"
COPY . .
WORKDIR "/src/UI/OnlineShop.UI.Console"
RUN dotnet build "OnlineShop.UI.Console.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "OnlineShop.UI.Console.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "OnlineShop.UI.Console.dll"]
