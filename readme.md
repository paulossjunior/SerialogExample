# Minimal API com Logs Estruturados

Este projeto demonstra a implementação de uma API minimal em .NET 8 com logs estruturados usando Serilog e Seq.

## 🚀 Tecnologias

- .NET 8
- Serilog
- Seq
- Docker
- Docker Compose

## 📦 Pré-requisitos

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [Docker](https://www.docker.com/products/docker-desktop/)
- [Docker Compose](https://docs.docker.com/compose/install/)

## 🏗️ Estrutura do Projeto

```
MinimalUserApi/
├── Configuration/
│   └── LogEnrichers/
│       └── ClientIpEnricher.cs
├── Models/
│   └── User.cs
├── Services/
│   └── UserService.cs
├── Repositories/
│   └── UserRepository.cs
├── Dockerfile
├── docker-compose.yml
└── Program.cs
```

## 📝 Sobre o Logging

### Serilog
O projeto utiliza Serilog para logging estruturado. O Serilog oferece:

1. **Log Enrichers**
    - Machine Name
    - Environment Name
    - Thread ID
    - Client IP (customizado)

2. **Múltiplos Outputs (Sinks)**
   ```csharp
   Log.Logger = new LoggerConfiguration()
       .WriteTo.Console()       // Output no console
       .WriteTo.File()         // Output em arquivo
       .WriteTo.Seq()          // Output no Seq
       .CreateLogger();
   ```

### Seq
O Seq é uma plataforma para agregação e análise de logs. Características:

1. **Interface Web**
    - Acesse: http://localhost:5341
    - Dashboard interativo
    - Filtros avançados
    - Busca em tempo real

2. **Queries**
   Exemplos de queries no Seq:
   ```sql
   // Logs de erro
   @Level = 'Error'

   // Tempo de resposta alto
   ElapsedMilliseconds > 1000
   ```

## 🚀 Como Executar

1. **Clone o repositório**
   ```bash
   git clone [url-do-repositorio]
   cd MinimalUserApi
   ```

2. **Execute com Docker Compose**
   ```bash
   docker-compose up -d
   ```

3. **Acesse**
    - API: http://localhost:5000
    - Seq: http://localhost:5341

## 📊 Exemplos de Uso

### API Endpoints

1. **Criar Usuário**
   ```bash
   curl -X POST http://localhost:5000/users \
   -H "Content-Type: application/json" \
   -d '{"name": "João", "age": 30}'
   ```

2. **Listar Usuários**
   ```bash
   curl http://localhost:5000/users
   ```

### Visualizando Logs no Seq

1. **Filtrar por Nível**
    - Na interface do Seq, use o filtro: `@Level = 'Error'`

2. **Buscar por IP**
    - Use: `ClientIP = '192.168.1.100'`

3. **Monitorar Performance**
    - Use: `ElapsedMilliseconds > 500`

## 🔍 Logs Estruturados

Exemplo de log estruturado:
```json
{
  "@t": "2024-12-19T10:15:23.4561234Z",
  "@l": "Information",
  "@mt": "HTTP {RequestMethod} {RequestPath} responded {StatusCode} in {Elapsed:0.0000} ms",
  "RequestMethod": "POST",
  "RequestPath": "/users",
  "StatusCode": 201,
  "Elapsed": 45.2165,
  "UserAgent": "Mozilla/5.0...",
  "MachineName": "web-1"
}
```

## 🛠️ Configurações Adicionais

### Serilog em appsettings.json
```json
{
  "Serilog": {
    "MinimumLevel": {
      "Default": "Information",
      "Override": {
        "Microsoft": "Warning"
      }
    },
    "WriteTo": [
      {
        "Name": "Seq",
        "Args": {
          "serverUrl": "http://seq:5341"
        }
      }
    ]
  }
}
```
