# Task Manager API 

Uma API RESTful para gerenciamento de tarefas e projetos desenvolvida em C# .NET 6.0 usando Domain-Driven Design.

## Arquitetura

- **Domain**: Entidades, Value Objects, Domain Events, Domain Services
- **Application**: Commands, Queries, Handlers, DTOs, Interfaces
- **Infrastructure**: Repositories, Data Context, External Services
- **API**: Controllers, Dependency Injection, Configuration

## Testes

# Na raiz da solução
dotnet test

# Com cobertura de código
dotnet test --collect:"XPlat Code Coverage"

# Testes específicos
dotnet test TaskManager.Domain.Tests
dotnet test TaskManager.Application.Tests  
dotnet test TaskManager.Infrastructure.Tests

# Com relatório de cobertura
dotnet test /p:CollectCoverage=true /p:CoverletOutputFormat=opencover

## Execução

```bash
# Com Docker
docker-compose up --build

# Localmente
dotnet run --project src/TaskManager.API

