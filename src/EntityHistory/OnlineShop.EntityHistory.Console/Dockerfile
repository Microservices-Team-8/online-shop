FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/EntityHistory/OnlineShop.EntityHistory.Console/OnlineShop.EntityHistory.Console.csproj", "src/EntityHistory/OnlineShop.EntityHistory.Console/"]
RUN dotnet restore "src/EntityHistory/OnlineShop.EntityHistory.Console/OnlineShop.EntityHistory.Console.csproj"
COPY . .
WORKDIR "/src/src/EntityHistory/OnlineShop.EntityHistory.Console"
RUN dotnet build "OnlineShop.EntityHistory.Console.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "OnlineShop.EntityHistory.Console.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "OnlineShop.EntityHistory.Console.dll"]
