FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 7100

ENV ASPNETCORE_URLS=http://localhost:8232
ENV ASPNETCORE_ENVIRONMENT Development

USER app
FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG configuration=Release
WORKDIR /src
COPY ["OrderManagement/OrderManagement.csproj", "OrderManagement/"]
RUN dotnet restore "OrderManagement/OrderManagement.csproj"
COPY . .
WORKDIR "/src/OrderManagement"
RUN dotnet build "OrderManagement.csproj" -c $configuration -o /app/build

FROM build AS publish
ARG configuration=Release
RUN dotnet publish "OrderManagement.csproj" -c $configuration -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "OrderManagement.dll"]
