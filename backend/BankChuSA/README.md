# BankChuSA API

API REST para gerenciamento bancário desenvolvida em .NET 6.0, implementando operações de contas, transferências e extratos com autenticação JWT.

## 📋 Índice

- [Características](#características)
- [Tecnologias](#tecnologias)
- [Arquitetura](#arquitetura)
- [Pré-requisitos](#pré-requisitos)
- [Instalação](#instalação)
- [Configuração](#configuração)
- [Executando a Aplicação](#executando-a-aplicação)
- [Endpoints](#endpoints)
- [Autenticação](#autenticação)
- [Testes](#testes)
- [Estrutura do Projeto](#estrutura-do-projeto)
- [Segurança](#segurança)
- [Logging](#logging)
- [Contribuindo](#contribuindo)

## 🚀 Características

- ✅ Autenticação e autorização com JWT
- ✅ Gerenciamento de contas bancárias
- ✅ Transferências entre contas com validação de dias úteis
- ✅ Consulta de extratos por período
- ✅ Validação de dados com FluentValidation
- ✅ Logging estruturado com Serilog
- ✅ Tratamento de erros padronizado
- ✅ Rate Limiting para segurança
- ✅ Transações de banco de dados com Unit of Work
- ✅ Versionamento de API
- ✅ Documentação Swagger/OpenAPI

## 🛠 Tecnologias

- **.NET 6.0** - Framework principal
- **Entity Framework Core 6.0.32** - ORM para acesso a dados
- **SQL Server** - Banco de dados
- **JWT Bearer Authentication** - Autenticação
- **FluentValidation** - Validação de dados
- **Serilog** - Logging estruturado
- **Swagger/OpenAPI** - Documentação da API
- **xUnit** - Framework de testes
- **Moq** - Mocking para testes
- **FluentAssertions** - Assertions legíveis

## 🏗 Arquitetura

O projeto segue uma arquitetura em camadas (Clean Architecture):

```
BankChuSA/
├── BankChuSA.API/              # Camada de apresentação (Controllers)
├── BankChuSA.Application/      # Camada de aplicação (Services, DTOs, Validators)
├── BankChuSA.Domain/           # Camada de domínio (Entities, Enums)
├── BankChuSA.Infrastructure/   # Camada de infraestrutura (DbContext, Repositories)
└── BankChuSA.Tests/            # Testes unitários e de integração
```

### Princípios

- **Separação de Responsabilidades**: Cada camada tem uma responsabilidade específica
- **Dependency Inversion**: Dependências apontam para abstrações (interfaces)
- **Unit of Work**: Padrão para gerenciar transações de banco de dados
- **Repository Pattern**: Abstração do acesso a dados

## 📦 Pré-requisitos

- [.NET 6.0 SDK](https://dotnet.microsoft.com/download/dotnet/6.0)
- [SQL Server](https://www.microsoft.com/sql-server/sql-server-downloads) ou SQL Server Express
- [Visual Studio 2022](https://visualstudio.microsoft.com/) ou [VS Code](https://code.visualstudio.com/)

## 🔧 Instalação

1. Clone o repositório:
```bash
git clone https://github.com/seu-usuario/BankChuSA.git
cd BankChuSA
```

2. Restaure as dependências:
```bash
dotnet restore
```

3. Configure a connection string no `appsettings.json` ou use User Secrets (recomendado)

## ⚙️ Configuração

### User Secrets (Recomendado)

Para configurações sensíveis, use User Secrets:

```bash
cd BanckChuSA.API
dotnet user-secrets init
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Data Source=SEU_SERVIDOR;Initial Catalog=BankChuSA;Integrated Security=True;Encrypt=False"
dotnet user-secrets set "Jwt:Key" "SUA_CHAVE_SECRETA_SUPER_SEGURA_AQUI"
dotnet user-secrets set "Jwt:Issuer" "BankChuSA"
dotnet user-secrets set "Jwt:Audience" "BankChuSA"
```

### Variáveis de Ambiente

Alternativamente, você pode usar variáveis de ambiente:

```bash
export ConnectionStrings__DefaultConnection="Data Source=SEU_SERVIDOR;Initial Catalog=BankChuSA;Integrated Security=True;Encrypt=False"
export Jwt__Key="SUA_CHAVE_SECRETA_SUPER_SEGURA_AQUI"
export Jwt__Issuer="BankChuSA"
export Jwt__Audience="BankChuSA"
```

### appsettings.json

Copie o arquivo `appsettings.Example.json` para `appsettings.json` e configure:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Data Source=SEU_SERVIDOR;Initial Catalog=BankChuSA;Integrated Security=True;Encrypt=False"
  },
  "Jwt": {
    "Key": "USE_USER_SECRETS_OR_ENVIRONMENT_VARIABLES",
    "Issuer": "BankChuSA",
    "Audience": "BankChuSA"
  }
}
```

## 🚀 Executando a Aplicação

1. Execute as migrations:
```bash
cd BanckChuSA.API
dotnet ef database update
```

Ou a aplicação executará as migrations automaticamente na inicialização.

2. Execute a aplicação:
```bash
dotnet run
```

3. Acesse o Swagger UI:
```
https://localhost:5001/swagger
```

## 📡 Endpoints

### Autenticação

#### POST /api/v1/auth/login
Autentica um usuário e retorna um token JWT.

**Request:**
```json
{
  "username": "admin",
  "password": "admin123"
}
```

**Response:**
```json
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9..."
}
```

### Contas

#### POST /api/v1/accounts
Cria uma nova conta bancária.

**Headers:**
```
Authorization: Bearer {token}
```

**Request:**
```json
{
  "ownerName": "João Silva",
  "documentNumber": "12345678901",
  "initialBalance": 1000.00
}
```

#### GET /api/v1/accounts/{accountNumber}
Consulta uma conta pelo número.

**Headers:**
```
Authorization: Bearer {token}
```

### Transferências

#### POST /api/v1/transfers
Realiza uma transferência entre contas.

**Headers:**
```
Authorization: Bearer {token}
```

**Request:**
```json
{
  "fromAccountNumber": "123456",
  "toAccountNumber": "654321",
  "amount": 100.00,
  "description": "Transferência de pagamento"
}
```

**Regras:**
- Transferências só podem ser realizadas em dias úteis
- A conta de origem deve ter saldo suficiente
- As contas devem estar ativas

### Extratos

#### GET /api/v1/statements/{accountNumber}?startDate=2024-01-01&endDate=2024-01-31
Consulta o extrato de uma conta no período especificado.

**Headers:**
```
Authorization: Bearer {token}
```

## 🔐 Autenticação

A API usa autenticação JWT Bearer. Para acessar endpoints protegidos:

1. Faça login em `/api/v1/auth/login`
2. Copie o token retornado
3. Inclua no header das requisições:
```
Authorization: Bearer {seu_token}
```

O token expira em 8 horas.

### Usuário Padrão

Ao inicializar a aplicação, um usuário admin é criado automaticamente:
- **Username:** admin
- **Password:** admin123

⚠️ **IMPORTANTE:** Altere a senha padrão em produção!

## 🧪 Testes

Execute os testes unitários:

```bash
dotnet test
```

Para ver a cobertura de código:

```bash
dotnet test /p:CollectCoverage=true /p:CoverletOutputFormat=opencover
```

## 📁 Estrutura do Projeto

```
BankChuSA/
├── BanckChuSA.API/
│   ├── Controllers/
│   │   └── v1/
│   │       ├── AccountsController.cs
│   │       ├── AuthController.cs
│   │       ├── StatementsController.cs
│   │       └── TransfersController.cs
│   ├── Middleware/
│   │   └── ErrorHandlingMiddleware.cs
│   ├── Program.cs
│   └── appsettings.json
│
├── BankChuSA.Application/
│   ├── DTOs/
│   ├── Interfaces/
│   ├── Resources/
│   ├── Services/
│   └── Validators/
│
├── BankChuSA.Domain/
│   ├── Entities/
│   ├── Enums/
│   └── Common/
│
├── BankChuSA.Infrastructure/
│   ├── Data/
│   │   ├── BankDbContext.cs
│   │   └── SeedData.cs
│   └── Repositories/
│       ├── Repository.cs
│       ├── AccountRepository.cs
│       ├── TransactionRepository.cs
│       └── UnitOfWork.cs
│
└── BankChuSA.Tests/
    └── Services/
```

## 🔒 Segurança

- ✅ Autenticação JWT
- ✅ Rate Limiting (100 requisições/minuto para anônimos, 200 para autenticados)
- ✅ Validação de dados com FluentValidation
- ✅ Configurações sensíveis em User Secrets
- ✅ HTTPS obrigatório
- ✅ CORS configurado
- ✅ Tratamento de erros padronizado

## 📝 Logging

A aplicação usa Serilog para logging estruturado. Os logs são salvos em:
- Console (durante desenvolvimento)
- Arquivo: `logs/bankchusa-YYYYMMDD.txt`

### Níveis de Log

- **Information**: Operações normais
- **Warning**: Avisos
- **Error**: Erros que não interrompem a aplicação
- **Fatal**: Erros críticos que interrompem a aplicação

## 🚧 Melhorias Futuras

- [ ] Validação de CPF/CNPJ
- [ ] Validação de horário comercial para transferências
- [ ] Suporte a múltiplos bancos
- [ ] Notificações de transações
- [ ] Histórico de auditoria
- [ ] Cache com Redis
- [ ] Health checks
- [ ] Métricas com Application Insights

## 🤝 Contribuindo

1. Faça um fork do projeto
2. Crie uma branch para sua feature (`git checkout -b feature/AmazingFeature`)
3. Commit suas mudanças (`git commit -m 'Add some AmazingFeature'`)
4. Push para a branch (`git push origin feature/AmazingFeature`)
5. Abra um Pull Request

## 📄 Licença

Este projeto está sob a licença MIT. Veja o arquivo `LICENSE` para mais detalhes.

## 👥 Autores

- **Seu Nome** - *Desenvolvimento inicial* - [SeuGitHub](https://github.com/seu-usuario)

## 🙏 Agradecimentos

- [BrasilAPI](https://brasilapi.com.br/) - API de feriados brasileiros
- Comunidade .NET

---

**Desenvolvido com ❤️ usando .NET 6.0**

