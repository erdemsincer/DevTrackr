FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# 🔁 Projeleri tek tek kopyala (AuthService + Shared)
COPY ["AuthService/AuthService.csproj", "AuthService/"]
COPY ./Shared ./Shared
# ✅ Restore işlemi (dependency çöz)
RUN dotnet restore "AuthService/AuthService.csproj"

# 🔁 Tüm solution'ı kopyala (build için)
COPY . .

WORKDIR "/src/AuthService"
RUN dotnet build "AuthService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "AuthService.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AuthService.dll"]
