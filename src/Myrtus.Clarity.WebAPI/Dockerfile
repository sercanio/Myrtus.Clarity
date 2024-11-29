FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 5000
EXPOSE 5001

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/Myrtus.CMS.WebAPI/Myrtus.CMS.WebAPI.csproj", "src/Myrtus.CMS.WebAPI/"]
COPY ["src/Myrtus.CMS.Application/Myrtus.CMS.Application.csproj", "src/Myrtus.CMS.Application/"]
COPY ["src/Myrtus.CMS.Domain/Myrtus.CMS.Domain.csproj", "src/Myrtus.CMS.Domain/"]
COPY ["src/Myrtus.CMS.Infrastructure/Myrtus.CMS.Infrastructure.csproj", "src/Myrtus.CMS.Infrastructure/"]
RUN dotnet restore "./src/Myrtus.CMS.WebAPI/./Myrtus.CMS.WebAPI.csproj"
COPY . .
WORKDIR "/src/src/Myrtus.CMS.WebAPI"
RUN dotnet build "./Myrtus.CMS.WebAPI.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Myrtus.CMS.WebAPI.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Myrtus.CMS.WebAPI.dll"]