# FixedAssets

**FixedAssets** é um sistema de gerenciamento de ativos fixos desenvolvido utilizando **.NET 8** no backend e **Angular** no frontend, seguindo os princípios de **Clean Architecture** e **SOLID**. A aplicação gerencia usuários, produtos de renda fixa e operações de compra e venda. Além disso, está preparado para ser executado em contêineres Docker.

## Requisitos

Para rodar o projeto, você precisará ter as seguintes ferramentas instaladas:

### Backend:
- **.NET 8**
- **Entity Framework Core** (com suporte para **SQL Server**)
- **Clean Architecture**
- **SOLID Principles**
- **REST APIs**
- **Docker**

### Frontend:
- **Angular v18**
- **ESLint** para validação de código
- **TypeScript**
- **Bootstrap 

### Integração Contínua:
- **GitHub Actions** para pipeline CI/CD (.github/workflows/ci.yml)
  - Build e Teste automáticos para o backend e frontend
  - Linting automático para o frontend

## Estrutura da Aplicação

A estrutura do projeto segue os princípios de **Clean Architecture** e está dividida nas seguintes camadas:

- **Camada de Apresentação** (Frontend em Angular)
- **Camada de Aplicação** (Casos de uso e serviços)
- **Camada de Domínio** (Entidades principais)
- **Camada de Infraestrutura** (Acesso a dados e repositórios)
- 
### Backend - Pastas:
- **FixedAssets.Api**: Contém os controladores de API e o pipeline de requisições.
- **FixedAssets.Application**: Contém os casos de uso e interfaces para operações de negócios.
- **FixedAssets.Domain**: Contém as entidades de domínio e regras de negócios centrais.
- **FixedAssets.Infrastructure**: Contém as implementações de repositórios e outros serviços externos.

### Frontend - Pastas:
- **FixedAssetsWeb**: Contém o projeto Angular, com componentes para listagem de produtos, detalhes, e histórico de compras.
## Configuração do Ambiente

1. Clone este repositório:

```bash
git clone https://github.com/lucianaregi/FixedAssets.git
cd FixedAssets

```

1. Instale as dependências do frontend (Angular):

```bash
cd FixedAssetsWeb
npm install

```

1. Instale as dependências do backend (.NET):

```bash
cd ..
dotnet restore

```

## Rodando o Projeto Localmente

### Backend (.NET)

Para rodar o backend localmente, execute o comando:

```bash
dotnet run --project FixedAssets.Api

```

Isso iniciará o backend localmente, por padrão na URL `http://localhost:8000`.

### Frontend (Angular)

Para rodar o frontend (Angular), execute:

```bash
cd FixedAssetsWeb
ng serve

```

O frontend estará disponível em `http://localhost:4200`.

## Rodando com Docker

### Configuração do Docker

Para rodar a aplicação com Docker, siga os passos abaixo:

1. **Dockerfile** e **docker-compose.yml** já estão configurados no projeto.
2. No diretório raiz do projeto, rode o comando:

```bash
docker-compose up --build

```

Isso irá construir e iniciar os containers para o backend e o frontend. Os serviços estarão acessíveis nas seguintes portas:

- Backend: `http://localhost:8000`
- Frontend: `http://localhost:4200`
1. Para parar e remover os containers, execute:

```bash
docker-compose down

```

### Testando no Docker

Para testar a aplicação localmente dentro dos containers Docker:

```bash
# Verifique os logs dos containers
docker logs <nome_do_container>

# Ou entre no container:
docker exec -it <nome_do_container> /bin/bash

```

## Testes Automatizados

### Testes Unitários

Para rodar os testes unitários da aplicação:

```bash
dotnet test

```

### Testes de Integração

Os testes de integração estão configurados para rodar com o banco de dados **InMemory** para garantir que o fluxo completo de uma compra seja validado.


## APIs

### Endpoints Disponíveis

- **GET /api/products** - Retorna a lista de produtos.
- **POST /api/order** - Processa a compra de produtos.
- **GET /api/user/{id}** - Retorna os detalhes de um usuário.
- **PUT /api/user/{id}** - Atualiza as informações de um usuário.
- **GET /api/mosttradedassets** - Retorna os 5 ativos mais negociados.
- **PUT /api/mosttradedassets** - Atualiza o valor de um ativo.

### Exemplo de Chamadas com cURL:

- **Listar Produtos:**
    
    ```bash
    curl <http://localhost:5000/api/products>
    
    ```
    
- **Processar Compra:**
    
    ```bash
    curl -X POST <http://localhost:5000/api/order> \\
         -H "Content-Type: application/json" \\
         -d '{"userId": 1, "orderItems": [{"productId": 1, "quantity": 2}]}'
    
    ```
