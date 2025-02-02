FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /main
COPY ["Region.Query.Api/Region.Query.Api.csproj", "Region.Query.Api/"]
RUN dotnet restore "Region.Query.Api/Region.Query.Api.csproj"
COPY . .

RUN dotnet build "Region.Query.Api/Region.Query.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Region.Query.Api/Region.Query.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Region.Query.Api.dll"]
