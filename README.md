# Task Manager API 

Uma API RESTful para gerenciamento de tarefas e projetos desenvolvida em C# .NET 8.0 usando Domain-Driven Design.

## Arquitetura

- **Domain**: Entidades, Value Objects, Domain Events, Domain Services
- **Application**: Commands, Queries, Handlers, DTOs, Interfaces
- **Infrastructure**: Repositories, Data Context, External Services
- **API**: Controllers, Dependency Injection, Configuration

## Execução

```bash
# Com Docker
docker-compose up --build

# Localmente
dotnet run --project src/TaskManager.API