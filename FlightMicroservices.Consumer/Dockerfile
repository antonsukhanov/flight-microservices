FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["FlightMicroservices.Consumer.csproj", "FlightMicroservices.Consumer/"]
RUN dotnet restore "FlightMicroservices.Consumer/FlightMicroservices.Consumer.csproj"
COPY . FlightMicroservices.Consumer/
WORKDIR "/src/FlightMicroservices.Consumer"
RUN ls -la
RUN dotnet build "FlightMicroservices.Consumer.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "FlightMicroservices.Consumer.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "FlightMicroservices.Consumer.dll"]
