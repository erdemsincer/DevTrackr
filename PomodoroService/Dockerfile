FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["PomodoroService/PomodoroService.csproj", "PomodoroService/"]
COPY ./Shared ./Shared

RUN dotnet restore "PomodoroService/PomodoroService.csproj"
COPY . .
WORKDIR "/src/PomodoroService"
RUN dotnet build "PomodoroService.csproj" -c Release -o /app/build
FROM build AS publish
RUN dotnet publish "PomodoroService.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PomodoroService.dll"]
