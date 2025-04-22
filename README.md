## README - API para Gestão de Atividades para Crianças

Este README fornece informações sobre a API desenvolvida em .NET com Entity Framework Core e SQL Server para gerenciar atividades de crianças, pais e psicólogos.

### Tecnologias Utilizadas

* **.NET:** Framework de desenvolvimento backend.
* **Entity Framework Core (EF Core):** ORM (Object-Relational Mapper) para interagir com o banco de dados.
* **SQL Server:** Banco de dados relacional utilizado para persistência dos dados.

### Estrutura do Projeto

O projeto segue uma estrutura comum de aplicações .NET, organizada da seguinte forma:

* **ASPCTS:** Projeto principal da API.
* **Context:** Contém a classe de contexto do Entity Framework (`ASPCTSContext`) responsável pela interação com o banco de dados.
* **Controllers:** Contém os controllers da API, responsáveis por receber as requisições HTTP e retornar as respostas.
    * `AtividadeController.cs`: Lida com as operações relacionadas às atividades.
    * `CriancaController.cs`: Lida com as operações relacionadas às crianças.
    * `PaiController.cs`: Lida com as operações relacionadas aos pais.
    * `PsicologoController.cs`: Lida com as operações relacionadas aos psicólogos.
    * `UsuarioController.cs`: Lida com as operações genéricas relacionadas aos usuários (pai e psicólogo herdam desta classe).
* **Migrations:** Contém os arquivos de migração do Entity Framework Core, utilizados para criar e atualizar o schema do banco de dados.
* **Models:** Contém as classes de modelo que representam as entidades do banco de dados e a lógica de domínio.
    * `Atividade.cs`: Representa uma atividade a ser realizada por uma criança.
    * `Crianca.cs`: Representa uma criança.
    * `Pai.cs`: Representa um pai, herda de `Usuario`.
    * `Psicologo.cs`: Representa um psicólogo, herda de `Usuario`.
    * `Usuario.cs`: Classe base abstrata para usuários do sistema.
* **Properties:** Contém configurações do projeto.
* **Repositories:** Contém as interfaces e implementações dos repositórios, responsáveis pela comunicação com o Entity Framework Core.
    * `IAtividadeRepository.cs` e `AtividadeRepository.cs`: Lida com a persistência de objetos `Atividade`.
    * `ICriancaRepository.cs` e `CriancaRepository.cs`: Lida com a persistência de objetos `Crianca`.
    * `IPaiRepository.cs` e `PaiRepository.cs`: Lida com a persistência de objetos `Pai`.
    * `IPsicologoRepository.cs` e `PsicologoRepository.cs`: Lida com a persistência de objetos `Psicologo`.
    * `IUsuarioRepository.cs` e `UsuarioRepository.cs`: Lida com a persistência de objetos `Usuario`.
* **Services:** Contém as interfaces e implementações dos serviços, que contêm a lógica de negócios da aplicação.
    * `IAtividadeService.cs` e `AtividadeService.cs`: Lida com a lógica de negócios relacionada às atividades.
    * `ICriancaService.cs` e `CriancaService.cs`: Lida com a lógica de negócios relacionada às crianças.
    * `IPaiService.cs` e `PaiService.cs`: Lida com a lógica de negócios relacionada aos pais.
    * `IPsicologoService.cs` e `PsicologoService.cs`: Lida com a lógica de negócios relacionada aos psicólogos.
    * `IUsuarioService.cs` e `UsuarioService.cs`: Lida com a lógica de negócios genérica relacionada aos usuários.
* `appsettings.Development.json` e `appsettings.json`: Arquivos de configuração da aplicação.

### Modelos de Dados

A seguir, um breve resumo dos modelos de dados:

* **Atividade:**
    * `Id` (int): Identificador único da atividade.
    * `Titulo` (string): Título da atividade.
    * `Descricao` (string): Descrição detalhada da atividade.
    * `DataCriacao` (DateTime): Data de criação da atividade (definida automaticamente como UTC).
    * `DataConclusao` (DateTime?): Data de conclusão da atividade (opcional).
    * `Concluida` (bool): Indica se a atividade foi concluída.
    * `CriancaId` (int): Chave estrangeira para a criança associada.
    * `Crianca` (`Crianca?`): Objeto de navegação para a criança associada.

* **Crianca:**
    * `Id` (int): Identificador único da criança.
    * `Nome` (string): Nome da criança.
    * `Idade` (int): Idade calculada da criança com base na data de nascimento.
    * `DataNascimento` (DateTimeOffset): Data de nascimento da criança (definida automaticamente como UTC).
    * `PaiId` (int): Chave estrangeira para o pai da criança.
    * `Pai` (`Pai?`): Objeto de navegação para o pai.
    * `PsicologoId` (int): Chave estrangeira para o psicólogo da criança (opcional).
    * `Psicologo` (`Psicologo?`): Objeto de navegação para o psicólogo (opcional).
    * `Atividades` (`ICollection<Atividade>`): Lista de atividades associadas à criança.

* **Pai:** Herda de `Usuario`.
    * `Criancas` (`ICollection<Crianca>`): Lista de crianças associadas ao pai.
    * `PsicologoId` (int?): Chave estrangeira para o psicólogo associado ao pai (opcional).
    * `Psicologo` (`Psicologo?`): Objeto de navegação para o psicólogo (opcional).

* **Psicologo:** Herda de `Usuario`.
    * `CRP` (string): Registro do Conselho Regional de Psicologia.
    * `Criancas` (`ICollection<Crianca>`): Lista de crianças associadas ao psicólogo.

* **Usuario:** Classe base abstrata.
    * `Id` (int): Identificador único do usuário.
    * `Name` (string): Nome do usuário.
    * `Email` (string): Endereço de e-mail do usuário.
    * `Password` (string): Senha do usuário.
    * `Phone` (string): Telefone do usuário.
    * `Tipo` (string): Tipo de usuário ("Psicologo", "Pai", etc.).
    * `CPF` (string): CPF do usuário.
    * `DataNascimento` (DateTimeOffset): Data de nascimento do usuário (definida automaticamente como UTC).

### Endpoints da API

A API expõe os seguintes endpoints para interagir com os recursos:

* **Atividades (`/api/Atividade`)**
    * `GET /BuscarTodasAtividades`: Retorna todas as atividades.
    * `GET /BuscarAtividadeId/{id}`: Retorna uma atividade específica pelo ID.
    * `POST /AdicionarAtividade`: Adiciona uma nova atividade.
    * `PUT /AtualizarAtividade/{id}`: Atualiza uma atividade existente.
    * `DELETE /DeletarAtividade/{id}`: Deleta uma atividade específica pelo ID.

* **Crianças (`/api/Crianca`)**
    * `GET /BuscarTodasCriancas`: Retorna todas as crianças (sem incluir a lista de atividades para otimização).
    * `GET /BuscarCriancaId/{id}`: Retorna uma criança específica pelo ID (incluindo idade).
    * `POST /AdicionarCrianca`: Adiciona uma nova criança.
    * `PUT /AtualizarCrianca/{id}`: Atualiza os dados de uma criança existente.
    * `DELETE /DeletarCrianca/{id}`: Deleta uma criança específica pelo ID.

* **Pais (`/api/Pai`)**
    * `GET /BuscarTodosPais`: Retorna todos os pais.
    * `GET /BuscarPorPaiId/{id}`: Retorna um pai específico pelo ID.
    * `POST /AdicionarPai`: Adiciona um novo pai. Valida se o CPF já existe.
    * `PUT /AtualizarPai/{id}`: Atualiza os dados de um pai existente.
    * `DELETE /DeletarPai/{id}`: Deleta um pai específico pelo ID.

* **Psicólogos (`/api/Psicologo`)**
    * `GET /BuscarTodosPsicologos`: Retorna todos os psicólogos.
    * `GET /BuscarPorPsicologoId/{id}`: Retorna um psicólogo específico pelo ID.
    * `POST /AdicionarPsicologo`: Adiciona um novo psicólogo. Valida se o CPF já existe.
    * `PUT /AtualizarPsicologo/{id}`: Atualiza os dados de um psicólogo existente.
    * `DELETE /DeletarPsicologo/{id}`: Deleta um psicólogo específico pelo ID.

* **Usuários (`/api/Usuario`)**
    * `GET /BuscarTodosUsuarios`: Retorna todos os usuários (pais e psicólogos).
    * `GET /BuscarPorUsuarioId/{id}`: Retorna um usuário específico pelo ID.
    * `POST /AdicionarUsuario`: Adiciona um novo usuário.
    * `PUT /AtualizarUsuario/{id}`: Atualiza os dados de um usuário existente.
    * `DELETE /DeletarUsuario/{id}`: Deleta um usuário específico pelo ID.

## Configuração do Entity Framework Core e SQL Server

Esta seção detalha os passos para configurar o Entity Framework Core (EF Core) e conectá-lo ao SQL Server para este projeto .NET.

### Pré-requisitos

* **.NET SDK:** Certifique-se de ter o .NET SDK instalado em sua máquina. Você pode baixá-lo em [https://dotnet.microsoft.com/download](https://dotnet.microsoft.com/download).
* **SQL Server:** Você precisará de uma instância do SQL Server instalada e em execução (local ou remota). Pode ser o SQL Server Express (gratuito), uma instância local do SQL Server Developer Edition ou um serviço de banco de dados na nuvem.

### 1. Instalação dos Pacotes NuGet do Entity Framework Core para SQL Server

O Entity Framework Core e seu provedor para SQL Server são distribuídos como pacotes NuGet. Você pode instalá-los utilizando o Gerenciador de Pacotes NuGet no Visual Studio ou através do .NET CLI.

**Usando o Gerenciador de Pacotes NuGet (Visual Studio):**

1.  Abra seu projeto no Visual Studio.
2.  Vá em **Ferramentas** -> **Gerenciador de Pacotes NuGet** -> **Gerenciar Pacotes NuGet para Solução**.
3.  Na janela do "Gerenciador de Pacotes NuGet", selecione a aba **Procurar**.
4.  Procure e instale os seguintes pacotes:
    * `Microsoft.EntityFrameworkCore`
    * `Microsoft.EntityFrameworkCore.SqlServer`
    * `Microsoft.EntityFrameworkCore.Tools` (necessário para comandos como `Add-Migration` e `Update-Database`)
    * Opcionalmente, instale `Microsoft.EntityFrameworkCore.Design` se você planeja usar comandos de scaffolding.

**Usando o .NET CLI:**

1.  Abra o terminal (Command Prompt ou PowerShell) na pasta raiz do seu projeto (onde o arquivo `.csproj` está localizado).
2.  Execute os seguintes comandos para adicionar os pacotes:

    ```bash
    dotnet add package Microsoft.EntityFrameworkCore
    dotnet add package Microsoft.EntityFrameworkCore.SqlServer
    dotnet add package Microsoft.EntityFrameworkCore.Tools
    dotnet add package Microsoft.EntityFrameworkCore.Design
    ```

### 2. Configuração da String de Conexão no `appsettings.json`

A string de conexão informa ao Entity Framework Core como se conectar ao seu banco de dados SQL Server. Ela é tipicamente armazenada no arquivo de configuração da aplicação, `appsettings.json`.

1.  Abra o arquivo `appsettings.json` no seu projeto. Se você tiver um arquivo `appsettings.Development.json`, você pode usá-lo para configurações específicas do ambiente de desenvolvimento.
2.  Adicione ou modifique a seção `ConnectionStrings` para incluir uma chave com o nome da sua conexão (por exemplo, `DefaultConnection`) e a string de conexão para o seu SQL Server.

    ```json
    {
      "Logging": {
        "LogLevel": {
          "Default": "Information",
          "Microsoft.AspNetCore": "Warning"
        }
      },
      "AllowedHosts": "*",
      "ConnectionStrings": {
        "ConexaoPadrao": "Server=SEU_SERVIDOR;Database=NOME_DO_SEU_BANCO;Trusted_Connection=True; TrustServerCertificate=True;"
      }
    }
    ```

    **Substitua os seguintes placeholders com as informações do seu banco de dados SQL Server:**

    * **`SEU_SERVIDOR`**: O nome ou endereço do seu servidor SQL Server. Se estiver rodando localmente, pode ser `.` ou `(local)` ou `localhost`. Para uma instância nomeada, use `SERVIDOR\NOME_DA_INSTANCIA`.
    * **`NOME_DO_SEU_BANCO`**: O nome do banco de dados que você deseja usar para este projeto. Se o banco de dados não existir, o Entity Framework Core pode criá-lo para você ao aplicar as migrações.
    * **`TrustServerCertificate=True`**: (Opcional, mas útil em ambientes de desenvolvimento) Indica que o certificado do servidor SQL Server não precisa ser totalmente validado. **Não use `True` em ambientes de produção sem entender as implicações de segurança.**


### 3. Configuração do `DbContext` (Classe de Contexto)

A classe de contexto (`DbContext`) serve como uma ponte entre seus modelos de domínio e o banco de dados. Você precisará configurar sua classe de contexto para usar a string de conexão definida no `appsettings.json`.

1.  Abra a classe do seu contexto (neste projeto, `ASPCTSContext.cs` dentro da pasta `Context`).
2.  No construtor da sua classe de contexto, você receberá um objeto `DbContextOptions<SuaClasseDeContexto>`. Essa opção é configurada na sua classe `Startup.cs` (ou `Program.cs` em .NET 6+) para ler a string de conexão do `appsettings.json`.

    No seu `Startup.cs` (ou `Program.cs`):

    ```csharp
    // Em Startup.cs (ConfigureServices):
    public void ConfigureServices(IServiceCollection services)
    {
        // ... outras configurações de serviço

        services.AddDbContext<ASPCTSContext>(options =>
            options.UseSqlServer(Configuration.GetConnectionString("ConexaoPadrao")));

        // ... outros serviços
    }

    // Em Program.cs (.NET 6+):
    var builder = WebApplication.CreateBuilder(args);

    // ... outras configurações do builder

    builder.Services.AddDbContext<ASPCTSContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("ConexaoPadrao")));

    // ... construir e executar o app
    ```

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