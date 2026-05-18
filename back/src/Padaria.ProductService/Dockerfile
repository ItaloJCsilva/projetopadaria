FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY Padaria.ProductService/ Padaria.ProductService/
COPY Padaria.Shared/ Padaria.Shared/

RUN dotnet restore "Padaria.ProductService/Padaria.ProductService.csproj"

WORKDIR "/src/Padaria.ProductService"
RUN dotnet publish "Padaria.ProductService.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "Padaria.ProductService.dll"]