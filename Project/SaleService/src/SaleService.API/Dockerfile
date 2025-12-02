FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY . .
RUN dotnet restore SaleService.API/SaleService.API.csproj
RUN dotnet publish SaleService.API/SaleService.API.csproj -c Release -o /app/publish
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .
RUN apt-get update && apt install -y curl
ENV ASPNETCORE_URLS=http://+:5002
EXPOSE 5002
ENTRYPOINT [ "dotnet","SaleService.API.dll" ]