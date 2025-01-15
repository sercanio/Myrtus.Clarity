FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
# Remove USER app line to run as root (needed for cert operations)
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
RUN dotnet restore "src/Myrtus.Clarity.WebAPI/Myrtus.Clarity.WebAPI.csproj"
COPY . .

RUN dotnet build "src/Myrtus.Clarity.WebAPI/Myrtus.Clarity.WebAPI.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "src/Myrtus.Clarity.WebAPI/Myrtus.Clarity.WebAPI.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

# Generate dev certificate
RUN apt-get update && \
    apt-get install -y libc6-dev openssl && \
    dotnet dev-certs https && \
    dotnet dev-certs https --trust

ENTRYPOINT ["dotnet", "Myrtus.Clarity.WebAPI.dll"]