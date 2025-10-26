FROM mcr.microsoft.com/dotnet/sdk:8.0 AS Build
WORKDIR /src
COPY *.csproj .
RUN dotnet restore
COPY . .
RUN dotnet publish -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS Runtime
WORKDIR /app
COPY --from=Build /app/publish .
RUN apt-get update && apt install -y curl
ENV ASPNETCORE_URLS=http://+:5002
EXPOSE 5002
ENTRYPOINT [ "dotnet","SaleService.API.dll" ]