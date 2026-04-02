FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY . .
RUN dotnet restore "src/ProjectionWorker/ProjectionWorker/ProjectionWorker.csproj"
WORKDIR "/src/src/ProjectionWorker/ProjectionWorker"
RUN dotnet build "ProjectionWorker.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ProjectionWorker.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ProjectionWorker.dll"]
