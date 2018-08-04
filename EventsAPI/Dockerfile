FROM microsoft/aspnetcore:2.0 AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/aspnetcore-build:2.0 AS build
WORKDIR /src
COPY EventsAPI/EventsAPI.csproj EventsAPI/
RUN dotnet restore EventsAPI/EventsAPI.csproj
COPY . .
WORKDIR /src/EventsAPI
RUN dotnet build EventsAPI.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish EventsAPI.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "EventsAPI.dll"]
