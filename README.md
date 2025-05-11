# API para Gestão de Atividades para Crianças

Este projeto é uma API REST desenvolvida com .NET e Entity Framework Core, utilizando SQL Server Express como banco de dados. A aplicação tem como objetivo gerenciar atividades de crianças, seus pais e psicólogos.

---

## 🛠 Tecnologias Utilizadas

- **.NET** – Framework de desenvolvimento backend.
- **Entity Framework Core** – ORM para manipulação do banco de dados.
- **SQL Server Express** – Banco de dados relacional gratuito.
- **Visual Studio / Visual Studio Code** – Ambiente de desenvolvimento.
- **Postman** – Testes de requisições HTTP.

---

## 📁 Estrutura do Projeto

```plaintext
ASPCTS/
│
├── Context/              --> ASPCTSContext (DbContext)
├── Controllers/          --> AtividadeController, CriancaController, etc.
├── Migrations/           --> Arquivos de migração do EF Core
├── Models/               --> Entidades: Atividade, Crianca, Usuario, etc.
├── Repositories/         --> Interfaces e classes para acesso ao banco
├── Services/             --> Regras de negócio
├── appsettings.json      --> Configurações de conexão
└── Program.cs / Startup.cs
```

---

## 🧠 Modelos de Dados

### Atividade
- `Id`: int
- `Titulo`: string
- `Descricao`: string
- `DataCriacao`: DateTime
- `DataConclusao`: DateTime?
- `Concluida`: bool
- `CriancaId`: int

### Crianca
- `Id`: int
- `Nome`: string
- `DataNascimento`: DateTime
- `Idade`: int (calculado)
- `PaiId`: int
- `PsicologoId`: int?

### Pai / Psicologo (herdam de Usuario)
- `Id`: int
- `Nome`, `Email`, `Senha`, `Telefone`, `CPF`, `DataNascimento`
- `PsicologoId`: int? (para Pai)
- `CRP`: string (para Psicologo)

---

## 📡 Endpoints da API

### 🔵 Atividade

| Método | Rota | Descrição |
|--------|------|-----------|
| GET    | `/api/atividade/buscar-atividades`          | Buscar todas as atividades |
| GET    | `/api/atividade/buscar-atividade-id/{id}`   | Buscar atividade por ID |
| POST   | `/api/atividade/adicionar-atividade`        | Adicionar nova atividade |
| PATCH  | `/api/atividade/atualizar-atividade/{id}`   | Atualizar atividade existente |
| DELETE | `/api/atividade/desativar-atividade/{id}`   | Desativar (soft delete) atividade |

### 🟢 Crianca

| Método | Rota | Descrição |
|--------|------|-----------|
| GET    | `/api/crianca/buscar-todas-criancas`         | Buscar todas as crianças |
| GET    | `/api/crianca/buscar-crianca-por-id/{id}`    | Buscar criança por ID |
| POST   | `/api/crianca/adicionar-crianca`             | Adicionar nova criança |
| PATCH  | `/api/crianca/atualizar-crianca/{id}`        | Atualizar dados da criança |
| DELETE | `/api/crianca/desativar-crianca/{id}`        | Desativar criança |

### 🟠 Pai

| Método | Rota | Descrição |
|--------|------|-----------|
| GET    | `/api/pai/buscar-todos-pais`          | Buscar todos os pais |
| GET    | `/api/pai/buscar-pai-por-id/{id}`     | Buscar pai por ID |
| POST   | `/api/pai/adicionar-pai`              | Adicionar novo pai |
| PATCH  | `/api/pai/atualizar-pai/{id}`         | Atualizar pai |
| DELETE | `/api/pai/desativar-pai/{id}`         | Desativar pai |

### 🟣 Psicologo

| Método | Rota | Descrição |
|--------|------|-----------|
| GET    | `/api/psicologo/buscar-todos-psicologos`         | Buscar todos os psicólogos |
| GET    | `/api/psicologo/buscar-psicologo-por-id/{id}`    | Buscar psicólogo por ID |
| POST   | `/api/psicologo/adicionar-psicologo`             | Adicionar novo psicólogo |
| PATCH  | `/api/psicologo/atualizar-psicologo/{id}`        | Atualizar psicólogo |
| DELETE | `/api/psicologo/desativar-psicologo/{id}`        | Desativar psicólogo |

---

## 🧱 Como Rodar o Projeto com SQL Server Express + EF Core

### ✅ Pré-requisitos

1. [.NET SDK](https://dotnet.microsoft.com/en-us/download)
2. [SQL Server Express Edition (gratuito)](https://www.microsoft.com/pt-br/sql-server/sql-server-downloads)
3. [SQL Server Management Studio (SSMS)](https://learn.microsoft.com/pt-br/sql/ssms/download-sql-server-management-studio-ssms)
4. Visual Studio / VS Code

### 🔧 Passos para Instalação e Configuração

#### 1. Instalar o SQL Server Express

- Execute o instalador e escolha o modo básico ou personalizado.
- Anote o nome da instância. Por padrão: `.\SQLEXPRESS`.

#### 2. Criar um banco de dados (opcional)

Você pode deixar o EF criar automaticamente ao aplicar a migration, ou criar manualmente via SSMS:

```sql
CREATE DATABASE NomeDoSeuBanco;
```

#### 3. Configurar o `appsettings.json`

```json
{
  "ConnectionStrings": {
    "ConexaoPadrao": "Server=localhost\SQLEXPRESS;Database=NomeDoSeuBanco;Trusted_Connection=True;TrustServerCertificate=True;"
  }
}
```

#### 4. Instalar os pacotes do Entity Framework Core

Execute os seguintes comandos no terminal da pasta do projeto:

```bash
dotnet add package Microsoft.EntityFrameworkCore
dotnet add package Microsoft.EntityFrameworkCore.SqlServer
dotnet add package Microsoft.EntityFrameworkCore.Tools
dotnet add package Microsoft.EntityFrameworkCore.Design
dotnet tool install --global dotnet-ef
```

#### 5. Configurar o `DbContext` no `Program.cs` (ou `Startup.cs`)

```csharp
builder.Services.AddDbContext<ASPCTSContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("ConexaoPadrao")));
```

#### 6. Gerar a migration e aplicar ao banco

```bash
dotnet ef migrations add Inicial
dotnet ef database update
```

---

## 🚀 Como Executar a API

```bash
dotnet run
```

Acesse via navegador ou Postman:

```
https://localhost:5001/
```

---

## 🧪 Testes

Você pode usar o Postman para testar cada uma das rotas listadas acima. Lembre-se de que as rotas usam os métodos corretos (GET, POST, PATCH, DELETE).

---
