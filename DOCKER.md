# Guia de Execução com Docker

Este guia explica como executar a aplicação API Banco Chu S.A. usando Docker e Docker Compose.

## Pré-requisitos

- [Docker](https://www.docker.com/get-started) instalado (versão 20.10 ou superior)
- [Docker Compose](https://docs.docker.com/compose/install/) instalado (versão 2.0 ou superior)

## Execução Rápida (Desenvolvimento)

Para executar toda a aplicação com um único comando:

```bash
docker-compose up -d
```

Isso irá:
- Criar e iniciar o SQL Server
- Criar e iniciar a API backend
- Criar e iniciar o frontend Angular
- Configurar a rede entre os containers

### Acessar a aplicação:

- **API Backend:** http://localhost:5000
- **Swagger:** http://localhost:5000/swagger
- **Frontend:** http://localhost:4200

## Parar os containers

```bash
docker-compose down
```

Para remover também os volumes (dados do banco):

```bash
docker-compose down -v
```

## Execução em Produção

Para executar em modo produção com configurações otimizadas:

1. Crie um arquivo `.env` na raiz do projeto:

```env
SA_PASSWORD=SuaSenhaSeguraAqui
JWT_KEY=SuaChaveJWTSecretMuitoSeguraComPeloMenos32Caracteres
JWT_ISSUER=BankChuSA
JWT_AUDIENCE=BankChuSA
API_PORT=5000
FRONTEND_PORT=4200
```

2. Execute com o arquivo de produção:

```bash
docker-compose -f docker-compose.prod.yml --env-file .env up -d
```

## Comandos Úteis

### Ver logs dos containers

```bash
# Todos os containers
docker-compose logs -f

# Apenas API
docker-compose logs -f api

# Apenas Frontend
docker-compose logs -f frontend

# Apenas SQL Server
docker-compose logs -f sqlserver
```

### Reconstruir as imagens

Se você fez alterações no código e precisa reconstruir:

```bash
docker-compose build --no-cache
docker-compose up -d
```

### Executar comandos dentro dos containers

```bash
# Acessar o container da API
docker-compose exec api bash

# Executar migrations manualmente (se necessário)
docker-compose exec api dotnet ef database update
```

### Verificar status dos containers

```bash
docker-compose ps
```

## Estrutura dos Containers

- **sqlserver:** SQL Server 2022 Express
  - Porta: 1433
  - Usuário: sa
  - Senha padrão: BankChuSA@2025! (altere em produção)

- **api:** Backend .NET 6.0
  - Porta: 5000 (mapeada para 80 no container)
  - Aguarda o SQL Server estar saudável antes de iniciar

- **frontend:** Frontend Angular servido por Nginx
  - Porta: 4200 (mapeada para 80 no container)
  - Aguarda a API estar disponível

## Solução de Problemas

### Container não inicia

Verifique os logs:
```bash
docker-compose logs nome-do-container
```

### Erro de conexão com banco de dados

Certifique-se de que o SQL Server está saudável:
```bash
docker-compose ps sqlserver
```

O healthcheck deve mostrar como "healthy".

### Porta já em uso

Se as portas 5000, 4200 ou 1433 já estiverem em uso, altere no `docker-compose.yml`:

```yaml
ports:
  - "5001:80"  # Altere 5000 para outra porta
```

### Limpar tudo e começar do zero

```bash
docker-compose down -v
docker system prune -a
docker-compose up -d --build
```

## Variáveis de Ambiente

As principais variáveis que podem ser configuradas:

- `SA_PASSWORD`: Senha do SQL Server
- `JWT_KEY`: Chave secreta para JWT (mínimo 32 caracteres)
- `JWT_ISSUER`: Emissor do token JWT
- `JWT_AUDIENCE`: Audiência do token JWT
- `API_PORT`: Porta externa da API
- `FRONTEND_PORT`: Porta externa do frontend

## Notas Importantes

- As migrations são executadas automaticamente quando a API inicia
- Os dados do banco são persistidos em volumes Docker
- Os logs da API são salvos em `backend/BankChuSA/BanckChuSA.API/logs/`
- Em produção, sempre use variáveis de ambiente para senhas e chaves secretas

## Suporte

Para mais informações, consulte a documentação principal no README.md

