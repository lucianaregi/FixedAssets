# Usar uma imagem oficial do .NET SDK para build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copiar os arquivos de projeto e restaurar dependências
COPY ["FixedAssets.Api/FixedAssets.Api.csproj", "FixedAssets.Api/"]
COPY ["FixedAssets.Application/FixedAssets.Application.csproj", "FixedAssets.Application/"]
COPY ["FixedAssets.Domain/FixedAssets.Domain.csproj", "FixedAssets.Domain/"]
COPY ["FixedAssets.Infrastructure/FixedAssets.Infrastructure.csproj", "FixedAssets.Infrastructure/"]

RUN dotnet restore "FixedAssets.Api/FixedAssets.Api.csproj"

# Copiar o restante dos arquivos do projeto
COPY . .

# Compilar o projeto
WORKDIR /src/FixedAssets.Api
RUN dotnet build "FixedAssets.Api.csproj" -c Release -o /app/build

# Publicar o projeto
RUN dotnet publish "FixedAssets.Api.csproj" -c Release -o /app/publish

# Usar uma imagem mais leve para execução
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

# Expor a porta da aplicação
EXPOSE 80

# Comando para rodar a aplicação
ENTRYPOINT ["dotnet", "FixedAssets.Api.dll"]