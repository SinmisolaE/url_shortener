FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081



FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY ["URLShort.API/URLShort.API.csproj", "URLShort.API/"]
COPY ["URLShort.Core/URLShort.Core.csproj", "URLShort.Core/"]
COPY ["URLShort.Infrastructure/URLShort.Infrastructure.csproj", "URLShort.Infrastructure/"]
RUN dotnet restore "URLShort.API/URLShort.API.csproj"

COPY . .

WORKDIR "/src/URLShort.API"
RUN dotnet build "URLShort.API.csproj" -c Release -o /app/build



FROM build AS publish
RUN dotnet publish "URLShort.API.csproj" -c Release -o /app/publish /p:UseAppHost=false



FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "URLShort.API.dll"]

