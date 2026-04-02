FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY . .
RUN dotnet restore "src/Command/Command.API/Command.API.csproj"
WORKDIR "/src/src/Command/Command.API"
RUN dotnet build "Command.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Command.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Command.API.dll"]
