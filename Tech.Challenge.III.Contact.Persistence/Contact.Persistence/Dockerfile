FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /main
COPY ["Contact.Persistence.Api/Contact.Persistence.Api.csproj", "Contact.Persistence.Api/"]
RUN dotnet restore "Contact.Persistence.Api/Contact.Persistence.Api.csproj"
COPY . .

RUN dotnet build "Contact.Persistence.Api/Contact.Persistence.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Contact.Persistence.Api/Contact.Persistence.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Contact.Persistence.Api.dll"]
