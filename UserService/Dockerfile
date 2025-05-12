FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# ✅ Shared ve UserService projelerini doğru konumdan kopyala
COPY ./Shared ./Shared
COPY ["UserService/UserService.csproj", "UserService/"]

# 🔧 Restore işlemi
RUN dotnet restore "UserService/UserService.csproj"

# 🔁 Tüm kaynakları kopyala
COPY . .

# 🔨 Build işlemi
WORKDIR "/src/UserService"
RUN dotnet build "UserService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "UserService.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "UserService.dll"]
