# API Banco Chu S.A.

## Sobre o Projeto

Este projeto foi desenvolvido pela **BMPTec Tecnologia** para o **Banco Chu S.A.**, com o objetivo de criar uma API de integração para gerenciar contas bancárias e realizar transferências.

Durante o desenvolvimento, nossa equipe técnica realizou entrevistas com o cliente para definir os requisitos e funcionalidades necessárias. A aplicação foi projetada para ser consumida tanto por outras aplicações via REST quanto por operadores internos através de uma interface gráfica.

## Endpoints Principais

A API oferece os seguintes endpoints:

- **Cadastrar Contas** - Criação de novas contas bancárias
- **Realizar Transferências** - Transferências entre contas (somente em dias úteis)
- **Gerar Extrato por Período** - Consulta de movimentações em um intervalo de datas

A validação de dias úteis é feita através da integração com a API BrasilAPI: [https://brasilapi.com.br/api/feriados/v1/2025](https://brasilapi.com.br/api/feriados/v1/2025)

## Requisitos Atendidos

- ✅ **ASP.NET Core 6** - Framework utilizado no desenvolvimento
- ✅ **Testes Unitários** - Cobertura de testes implementada
- ✅ **Containerização (Docker)** - Preparado para containerização
- ✅ **Cache** - Implementação de cache para otimização
- ✅ **Git** - Controle de versão
- ✅ **Versionamento de API** - API versionada (v1)
- ✅ **Interação com Banco de Dados** - SQL Server com Entity Framework Core
- ✅ **Autenticação e Autorização** - JWT Bearer Token
- ✅ **Swagger** - Documentação interativa da API
- ✅ **Fluent Validation** - Validação de dados

## Estrutura do Projeto

```
APIBancoChuSA/
├── backend/
│   └── BankChuSA/
│       ├── BanckChuSA.API/          # Camada de apresentação
│       ├── BankChuSA.Application/  # Regras de negócio e serviços
│       ├── BankChuSA.Domain/        # Entidades e enums
│       ├── BankChuSA.Infrastructure/ # Acesso a dados e repositórios
│       └── BankChuSA.Tests/        # Testes unitários
└── frontend/
    └── bmptecbank-frontend/         # Interface Angular
```

## Pré-requisitos

Antes de executar o projeto, certifique-se de ter instalado:

- [.NET 6.0 SDK](https://dotnet.microsoft.com/download/dotnet/6.0)
- [SQL Server](https://www.microsoft.com/sql-server/sql-server-downloads) ou SQL Server Express
- [Node.js](https://nodejs.org/) (para o frontend Angular)
- [Docker](https://www.docker.com/) (opcional, para containerização)

## Como Executar o Projeto

### Backend (API)

1. **Clone o repositório:**
```bash
git clone https://github.com/anfsusax/api-banco-chu-sa.git
cd api-banco-chu-sa/backend/BankChuSA
```

2. **Configure a connection string:**

Edite o arquivo `BanckChuSA.API/appsettings.json` e ajuste a connection string do banco de dados:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Data Source=SEU_SERVIDOR;Initial Catalog=BankChuSA;Integrated Security=True;Encrypt=False"
  },
  "Jwt": {
    "Key": "SUA_CHAVE_SECRETA_AQUI_MINIMO_32_CARACTERES",
    "Issuer": "BankChuSA",
    "Audience": "BankChuSA"
  }
}
```

**Importante:** Para produção, use User Secrets ou variáveis de ambiente para armazenar a chave JWT.

3. **Restaure as dependências:**
```bash
dotnet restore
```

4. **Execute as migrations:**
```bash
cd BanckChuSA.API
dotnet ef database update
```

Ou simplesmente execute a aplicação - as migrations serão aplicadas automaticamente na primeira execução.

5. **Execute a API:**
```bash
dotnet run
```

A API estará disponível em:
- HTTPS: `https://localhost:7263`
- HTTP: `http://localhost:5079`

6. **Acesse o Swagger:**
Abra no navegador: `https://localhost:7263/swagger`

### Frontend (Angular)

1. **Navegue até a pasta do frontend:**
```bash
cd ../../frontend/bmptecbank-frontend
```

2. **Instale as dependências:**
```bash
npm install
```

3. **Execute o frontend:**
```bash
npm start
```

O frontend estará disponível em: `http://localhost:4200`

## Autenticação

A API utiliza autenticação JWT. Para obter um token:

1. Faça uma requisição POST para `/api/v1/auth/login`:
```json
{
  "username": "admin",
  "password": "admin123"
}
```

2. Use o token retornado no header das requisições:
```
Authorization: Bearer {seu_token}
```

**Usuário padrão criado automaticamente:**
- Username: `admin`
- Password: `admin123`

⚠️ **Altere a senha padrão em ambiente de produção!**

## Endpoints Detalhados

### Autenticação

**POST /api/v1/auth/login**
- Autentica um usuário e retorna token JWT
- Não requer autenticação

### Contas

**POST /api/v1/accounts**
- Cria uma nova conta bancária
- Requer autenticação
- Body: `{ "ownerName": "string", "documentNumber": "string", "initialBalance": 0 }`

**GET /api/v1/accounts/{accountNumber}**
- Consulta uma conta pelo número
- Requer autenticação

### Transferências

**POST /api/v1/transfers**
- Realiza transferência entre contas
- Requer autenticação
- Apenas em dias úteis (validação automática via BrasilAPI)
- Body: `{ "fromAccountNumber": "string", "toAccountNumber": "string", "amount": 0, "description": "string" }`

### Extratos

**GET /api/v1/statements/{accountNumber}?startDate=2025-01-01&endDate=2025-01-31**
- Consulta extrato por período
- Requer autenticação
- Parâmetros: `startDate` e `endDate` (formato ISO)

## Executando os Testes

Para executar os testes unitários:

```bash
cd backend/BankChuSA
dotnet test
```

## Docker (Containerização)

Para executar com Docker, você pode criar um `Dockerfile` na raiz do projeto ou usar docker-compose. Exemplo básico:

```dockerfile
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["BanckChuSA.API/BanckChuSA.API.csproj", "BanckChuSA.API/"]
RUN dotnet restore "BanckChuSA.API/BanckChuSA.API.csproj"
COPY . .
WORKDIR "/src/BanckChuSA.API"
RUN dotnet build "BanckChuSA.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "BanckChuSA.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BanckChuSA.API.dll"]
```

## Tecnologias Utilizadas

- **Backend:**
  - ASP.NET Core 6.0
  - Entity Framework Core 6.0.32
  - SQL Server
  - JWT Bearer Authentication
  - FluentValidation
  - Serilog (Logging)
  - Swagger/OpenAPI

- **Frontend:**
  - Angular 20
  - TypeScript
  - RxJS

- **Testes:**
  - xUnit
  - Moq

## Cache

A aplicação implementa cache em memória para otimizar consultas frequentes, especialmente para:
- Validação de feriados (BrasilAPI)
- Consultas de contas

## Logs

Os logs são gerados automaticamente e salvos em:
- Console (durante desenvolvimento)
- Arquivo: `BanckChuSA.API/logs/bankchusa-YYYYMMDD.txt`

## Observações Importantes

- As transferências só podem ser realizadas em dias úteis (segunda a sexta, exceto feriados)
- A validação de feriados é feita através da integração com BrasilAPI
- Todas as operações financeiras são transacionais (Unit of Work)
- A API possui rate limiting para proteção contra abuso
- HTTPS é obrigatório em produção

## Suporte

Para dúvidas ou suporte, entre em contato:

**BMPTec Tecnologia**  
Rua Abdon Batista, 342 – Centro  
Joinville/SC - CEP: 89201-010  
[moneyp.com.br](https://moneyp.com.br)

---

**Desenvolvido por BMPTec Tecnologia para Banco Chu S.A.**

