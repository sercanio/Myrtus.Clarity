FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER app
WORKDIR /app
EXPOSE 5000
EXPOSE 5001

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/Myrtus.Clarity.WebAPI/Myrtus.Clarity.WebAPI.csproj", "src/Myrtus.Clarity.WebAPI/"]
COPY ["src/Myrtus.Clarity.Application/Myrtus.Clarity.Application.csproj", "src/Myrtus.Clarity.Application/"]
COPY ["src/Myrtus.Clarity.Domain/Myrtus.Clarity.Domain.csproj", "src/Myrtus.Clarity.Domain/"]
COPY ["src/Myrtus.Clarity.Infrastructure/Myrtus.Clarity.Infrastructure.csproj", "src/Myrtus.Clarity.Infrastructure/"]
RUN dotnet restore "./src/Myrtus.Clarity.WebAPI/./Myrtus.Clarity.WebAPI.csproj"
COPY . .
WORKDIR "/src/Myrtus.Clarity.WebAPI"
RUN dotnet build "./Myrtus.Clarity.WebAPI.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Myrtus.Clarity.WebAPI.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Myrtus.Clarity.WebAPI.dll"]