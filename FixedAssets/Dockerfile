# Usar uma imagem oficial do .NET SDK para build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copiar os arquivos de projeto e restaurar dependências
COPY *.sln .
COPY FixedAssets.Api/*.csproj ./FixedAssets.Api/
COPY FixedAssets.Application/*.csproj ./FixedAssets.Application/
COPY FixedAssets.Domain/*.csproj ./FixedAssets.Domain/
COPY FixedAssets.Infrastructure/*.csproj ./FixedAssets.Infrastructure/
COPY FixedAssets.Application.Tests/*.csproj ./FixedAssets.Application.Tests/

RUN dotnet restore

# Copiar o restante dos arquivos do projeto
COPY . .

# Compilar o projeto
RUN dotnet publish FixedAssets.Api/FixedAssets.Api.csproj -c Release -o /app/publish

# Usar uma imagem mais leve para execução
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

# Expor a porta da aplicação
EXPOSE 80

# Comando para rodar a aplicação
ENTRYPOINT ["dotnet", "FixedAssets.Api.dll"]

