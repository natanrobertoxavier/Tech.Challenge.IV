FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /main
COPY ["Tech.Challenge.Api.Gateway/Tech.Challenge.Api.Gateway.csproj", "Tech.Challenge.Api.Gateway/"]
RUN dotnet restore "Tech.Challenge.Api.Gateway/Tech.Challenge.Api.Gateway.csproj"
COPY . .

RUN dotnet build "Tech.Challenge.Api.Gateway/Tech.Challenge.Api.Gateway.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Tech.Challenge.Api.Gateway/Tech.Challenge.Api.Gateway.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Tech.Challenge.Api.Gateway.dll"]