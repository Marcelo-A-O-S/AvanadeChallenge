FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY . .
RUN dotnet restore StockService.API/StockService.API.csproj
RUN dotnet publish StockService.API/StockService.API.csproj -c Release -o /app/publish
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .
RUN apt-get update && apt install -y curl
ENV ASPNETCORE_URLS=http://+:5003
EXPOSE 5003
ENTRYPOINT [ "dotnet", "StockService.API.dll"]