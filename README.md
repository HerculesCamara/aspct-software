# API para Gestão de Atividades para Crianças

<<<<<<< HEAD
Este projeto é uma API REST desenvolvida com .NET e Entity Framework Core, utilizando SQL Server Express como banco de dados. A aplicação tem como objetivo gerenciar atividades de crianças, seus pais e psicólogos.
=======
Este README fornece informações sobre a API desenvolvida em .NET com Entity Framework Core e SQL Server para gerenciar atividades de crianças, pais e psicólogos.
Repositório do frontend: https://github.com/HerculesCamara/aspct-frontend
>>>>>>> e57bfa5dc32097e21c2918b05bf35a286d3e52da

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

<<<<<<< HEAD
---
=======
    Certifique-se de que `"ConexaoPadrao"` corresponda ao nome da chave que você usou na seção `ConnectionStrings` do seu `appsettings.json`. `Configuration` (ou `builder.Configuration`) fornece acesso às configurações do seu aplicativo, incluindo as strings de conexão.

Com esses passos, seu projeto .NET estará configurado para utilizar o Entity Framework Core com o SQL Server, utilizando a string de conexão definida no seu arquivo `appsettings.json`. O próximo passo seria criar suas migrações para gerar o schema do banco de dados com base nos seus modelos.

### Como Executar a Aplicação

1.  **Pré-requisitos:**
    * .NET SDK instalado.
    * SQL Server instalado (local ou remoto).

2.  **Configurar a String de Conexão:**
    * Abra o arquivo `appsettings.json` (ou `appsettings.Development.json` para ambiente de desenvolvimento).
    * Localize a seção `ConnectionStrings` e configure a string de conexão para o seu servidor SQL Server conforme explicado na seção anterior. Certifique-se de que o nome do banco de dados (`Database=NOME_DO_SEU_BANCO`) esteja correto.

3.  **Executar Migrações:**
    * Abra o terminal na pasta raiz do projeto (onde o arquivo `.csproj` está localizado).
    * Execute os seguintes comandos para criar o banco de dados e aplicar as migrações:
        ```bash
        dotnet ef database update
        ```
        Certifique-se de ter as ferramentas do EF Core instaladas (`Microsoft.EntityFrameworkCore.Tools`). Se você estiver usando o Visual Studio, pode usar o **Console do Gerenciador de Pacotes** (Ferramentas -> NuGet Package Manager -> Package Manager Console) e executar o comando `Update-Database`.

4.  **Executar a Aplicação:**
    * No mesmo terminal, execute o seguinte comando:
        ```bash
        dotnet run
        ```
    * A API estará disponível em uma URL como `http://localhost:[porta]`, onde `[porta]` é especificada na configuração (geralmente 5000 ou 5001).

### Próximos Passos

* Implementação de autenticação e autorização para proteger os endpoints da API.
* Adição de documentação da API utilizando Swagger/OpenAPI.
* Implementação de testes unitários e de integração.
* Melhorias na validação dos dados de entrada.
* Implementação de paginação e filtragem para as listagens de recursos.
* Adição de tratamento de erros mais detalhado
>>>>>>> e57bfa5dc32097e21c2918b05bf35a286d3e52da
