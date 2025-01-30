FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /main
COPY ["Tech.Challenge.Persistence.Api/Tech.Challenge.Persistence.Api.csproj", "Tech.Challenge.Persistence.Api/"]
RUN dotnet restore "Tech.Challenge.Persistence.Api/Tech.Challenge.Persistence.Api.csproj"
COPY . .

RUN dotnet build "Tech.Challenge.Persistence.Api/Tech.Challenge.Persistence.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Tech.Challenge.Persistence.Api/Tech.Challenge.Persistence.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Tech.Challenge.Persistence.Api.dll"]
