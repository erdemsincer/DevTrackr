FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["ActivityService/ActivityService.csproj", "ActivityService/"]
RUN dotnet restore "ActivityService/ActivityService.csproj"

COPY . .
WORKDIR "/src/ActivityService"
RUN dotnet build "ActivityService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ActivityService.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ActivityService.dll"]
