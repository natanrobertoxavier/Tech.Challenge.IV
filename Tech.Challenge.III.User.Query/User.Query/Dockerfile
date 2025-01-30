FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /main
COPY ["User.Query.Api/User.Query.Api.csproj", "User.Query.Api/"]
RUN dotnet restore "User.Query.Api/User.Query.Api.csproj"
COPY . .

RUN dotnet build "User.Query.Api/User.Query.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "User.Query.Api/User.Query.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "User.Query.Api.dll"]
