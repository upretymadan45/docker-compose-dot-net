FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 7232

ENV ASPNETCORE_URLS=http://localhost:7232
ENV ASPNETCORE_ENVIRONMENT Development

USER app
FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG configuration=Release
WORKDIR /src
COPY ["UserManagement/UserManagement.csproj", "UserManagement/"]
RUN dotnet restore "UserManagement/UserManagement.csproj"
COPY . .
WORKDIR "/src/UserManagement"
RUN dotnet build "UserManagement.csproj" -c $configuration -o /app/build

FROM build AS publish
ARG configuration=Release
RUN dotnet publish "UserManagement.csproj" -c $configuration -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "UserManagement.dll"]
