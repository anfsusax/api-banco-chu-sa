FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src

# Copiar arquivos .csproj e restaurar dependências
COPY ["BanckChuSA.API/BanckChuSA.API.csproj", "BanckChuSA.API/"]
COPY ["BankChuSA.Application/BankChuSA.Application.csproj", "BankChuSA.Application/"]
COPY ["BankChuSA.Domain/BankChuSA.Domain.csproj", "BankChuSA.Domain/"]
COPY ["BankChuSA.Infrastructure/BankChuSA.Infrastructure.csproj", "BankChuSA.Infrastructure/"]

RUN dotnet restore "BanckChuSA.API/BanckChuSA.API.csproj"

# Copiar todo o código e compilar
COPY . .
WORKDIR "/src/BanckChuSA.API"
RUN dotnet build "BanckChuSA.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "BanckChuSA.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BanckChuSA.API.dll"]

