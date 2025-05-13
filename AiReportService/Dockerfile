FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["AiReportService/AiReportService.csproj", "AiReportService/"]

RUN dotnet restore "AiReportService/AiReportService.csproj"
COPY . .
WORKDIR "/src/AiReportService"
RUN dotnet build "AiReportService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "AiReportService.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AiReportService.dll"]
