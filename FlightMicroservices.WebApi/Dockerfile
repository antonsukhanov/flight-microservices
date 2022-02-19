FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["FlightMicroservices.WebApi/FlightMicroservices.WebApi.csproj", "FlightMicroservices.WebApi/"]
RUN dotnet restore "FlightMicroservices.WebApi/FlightMicroservices.WebApi.csproj"
COPY . .
WORKDIR "/src/FlightMicroservices.WebApi"
RUN dotnet build "FlightMicroservices.WebApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "FlightMicroservices.WebApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "FlightMicroservices.WebApi.dll"]
