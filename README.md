# **ASPCTS - Acompanhamento e Suporte Terapêutico para Crianças Autistas**
## **Resumo do Projeto**
O **ASPCTS** (Acompanhamento e Suporte Terapêutico para Crianças Autistas) é uma plataforma digital robusta e inovadora, projetada para otimizar a comunicação e o acompanhamento terapêutico de crianças autistas com nível de suporte elevado. Ao centralizar informações e promover a continuidade do cuidado, esta ferramenta se torna um recurso indispensável para **psicólogos, pais e escolas**.

O aplicativo visa preencher a lacuna na comunicação entre os diversos envolvidos no processo terapêutico. Ele oferece funcionalidades cruciais como a **geração automatizada de relatórios diários e semanais**, **estatísticas de progresso personalizadas** e uma **interface de usuário intuitiva e acessível**. Desenvolvido com foco em **segurança e conformidade regulatória**, utilizando **tecnologias modernas e escaláveis**, o ASPCTS garante a acessibilidade e a eficiência necessárias para monitorar e impulsionar o desenvolvimento das crianças em diferentes ambientes, como casa e escola.
## **Arquitetura da Aplicação**
A aplicação ASPCTS é construída seguindo os princípios de uma arquitetura em camadas (ou N-Tier), promovendo a separação de responsabilidades, alta coesão e baixo acoplamento, o que resulta em um código mais manutenível, testável e escalável.
### **Estrutura de Pastas Detalhada**
ASPCTS/
├── bin/                             # Binários compilados do projeto.
├── Context/                         # Camada de Acesso a Dados (parte do Entity Framework Core).
│   └── ApplicationDbContext.cs      # Define o contexto do banco de dados, DbSets e configurações de mapeamento.
├── Controllers/                     # Camada de Apresentação (API REST).
│   ├── AtividadeController.cs       # Endpoints para gerenciamento de atividades.
│   ├── AuthController.cs            # Endpoints para autenticação (login, registro).
│   ├── CriancaController.cs         # Endpoints para gerenciamento de crianças.
│   ├── PsicologoController.cs       # Endpoints para gerenciamento de psicólogos.
│   ├── RelatorioController.cs       # Endpoints para gerenciamento de relatórios.
│   └── ResponsavelController.cs     # Endpoints para gerenciamento de responsáveis.
├── DTOs/                            # Data Transfer Objects.
│   ├── Atividade/                   # DTOs específicos para a entidade Atividade.
│   │   ├── AtividadeCreateDTO.cs    # DTO para criação de atividades.
│   │   ├── AtividadeDTO.cs          # DTO para representação de atividades.
│   │   └── AtividadeUpdateDTO.cs    # DTO para atualização de atividades.
│   ├── Crianca/                     # DTOs específicos para a entidade Criança.
│   │   ├── CriancaCreateDTO.cs      # DTO para criação de crianças.
│   │   ├── CriancaDTO.cs            # DTO para representação de crianças.
│   │   ├── CriancaUpdateDTO.cs      # DTO para atualização de crianças.
│   │   └── CriancaVinculoDTO.cs     # DTO para vincular crianças.
│   ├── Login/                       # DTOs para o processo de login.
│   │   └── LoginDTO.cs              # DTO para credenciais de login.
│   ├── Psicologo/                   # DTOs específicos para a entidade Psicólogo.
│   │   ├── PsicologoCreateDTO.cs    # DTO para criação de psicólogos.
│   │   ├── PsicologoDTO.cs          # DTO para representação de psicólogos.
│   │   └── PsicologoUpdateDTO.cs    # DTO para atualização de psicólogos.
│   ├── Register/                    # DTOs para o processo de registro.
│   │   └── UsuarioRegisterDTO.cs    # DTO para registro de novos usuários.
│   ├── Relatorio/                   # DTOs específicos para a entidade Relatório.
│   │   ├── RelatorioCreateDTO.cs    # DTO para criação de relatórios.
│   │   ├── RelatorioDTO.cs          # DTO para representação de relatórios.
│   │   └── RelatorioUpdateDTO.cs    # DTO para atualização de relatórios.
│   └── Responsavel/                 # DTOs específicos para a entidade Responsável.
│       ├── ResponsavelCreateDTO.cs  # DTO para criação de responsáveis.
│       ├── ResponsavelDTO.cs        # DTO para representação de responsáveis.
│       ├── ResponsavelUpdateDTO.cs  # DTO para atualização de responsáveis.
│       └── ResponsavelUpdatePasswordDTO.cs # DTO para atualização de senha de responsáveis.
├── Mappings/                        # Configurações do AutoMapper.
│   └── MappingProfile.cs            # Define os mapeamentos entre Modelos e DTOs.
├── Migrations/                      # Arquivos de migração do Entity Framework Core.
├── Models/                          # Modelos de Domínio/Entidades do banco de dados.
│   ├── Atividade.cs                 # Representa uma atividade terapêutica.
│   ├── Crianca.cs                   # Representa uma criança autista.
│   ├── Psicologo.cs                 # Representa um usuário do tipo Psicólogo.
│   ├── Relatorio.cs                 # Representa um relatório de progresso da criança.
│   ├── Responsavel.cs               # Representa um usuário do tipo Responsável (pai/mãe).
│   └── Usuario.cs                   # Classe base para usuários (Psicólogo, Responsável).
├── obj/                             # Objetos intermediários da compilação.
├── Properties/                      # Propriedades do projeto.
│   └── launchSettings.json          # Configurações de inicialização e depuração.
├── Repositories/                    # Camada de Acesso a Dados (abstração de persistência).
│   ├── Atividade/
│   │   ├── AtividadeRepository.cs   # Implementação do repositório de atividades.
│   │   └── IAtividadeRepository.cs  # Interface para o repositório de atividades.
│   ├── Crianca/
│   │   ├── CriancaRepository.cs     # Implementação do repositório de crianças.
│   │   └── ICriancaRepository.cs    # Interface para o repositório de crianças.
│   ├── Psicologo/
│   │   ├── IPsicologoRepository.cs  # Interface para o repositório de psicólogos.
│   │   └── PsicologoRepository.cs   # Implementação do repositório de psicólogos.
│   ├── Relatorio/
│   │   ├── IRelatorioRepository.cs  # Interface para o repositório de relatórios.
│   │   └── RelatorioRepository.cs   # Implementação do repositório de relatórios.
│   ├── Responsavel/
│   │   ├── IResponsavelRepository.cs# Interface para o repositório de responsáveis.
│   │   └── ResponsavelRepository.cs # Implementação do repositório de responsáveis.
│   └── Usuario/
│       ├── IUsuarioRepository.cs    # Interface para o repositório de usuários.
│       └── UsuarioRepository.cs     # Implementação do repositório de usuários.
├── Services/                        # Camada de Lógica de Negócio.
│   ├── Atividade/
│   │   ├── AtividadeService.cs      # Lógica de negócio para atividades.
│   │   └── IAtividadeService.cs     # Interface para o serviço de atividades.
│   ├── Crianca/
│   │   ├── CriancaService.cs        # Lógica de negócio para crianças.
│   │   └── ICriancaService.cs       # Interface para o serviço de crianças.
│   ├── Jwt/                         # Serviço de Geração de JWT.
│   │   ├── IJwtService.cs           # Interface para o serviço JWT.
│   │   └── JwtService.cs            # Implementação do serviço JWT.
│   ├── Psicologo/
│   │   ├── IPsicologoService.cs     # Interface para o serviço de psicólogos.
│   │   └── PsicologoService.cs      # Lógica de negócio para psicólogos.
│   ├── Relatorio/
│   │   ├── IRelatorioService.cs     # Interface para o serviço de relatórios.
│   │   └── RelatorioService.cs      # Lógica de negócio para relatórios.
│   ├── Responsavel/
│   │   ├── IResponsavelService.cs   # Interface para o serviço de responsáveis.
│   │   └── ResponsavelService.cs    # Lógica de negócio para responsáveis.
│   └── Usuario/
│       ├── IUsuarioService.cs       # Interface para o serviço de usuários.
│       └── UsuarioService.cs        # Lógica de negócio para usuários.
├── appsettings.Development.json     # Configurações específicas para o ambiente de desenvolvimento.
├── appsettings.json                 # Configurações globais da aplicação.
├── ASPCTS.csproj                    # Arquivo de projeto C#.
├── ASPCTS.http                      # Arquivo de requisições HTTP (para VS Code).
├── Program.cs                       # Ponto de entrada e configuração da aplicação (injeção de dependências, pipeline HTTP).
├── ASPCTS.sln                       # Arquivo de solução do Visual Studio.
└── README.md                        # Este arquivo.
### **Detalhes dos Componentes e Fluxo de Dados**
1. **appsettings.json / appsettings.Development.json**:
   1. **Configurações Essenciais**: Contém a string de conexão para o banco de dados ("ConexaoPadrao": "Server=localhost\\SQLEXPRESS;Database=ASPCTSDB;Trusted\_Connection=True;TrustServerCertificate=True;") e as configurações para a geração e validação de JWTs (chave, emissor, audiência, tempo de expiração).
   1. **Logging**: Define os níveis de log para diferentes categorias da aplicação.
1. **Program.cs**:
   1. **Injeção de Dependências (DI)**: É o coração da inicialização da aplicação. Todos os serviços e repositórios são registrados aqui (builder.Services.AddScoped<Interface, Implementacao>();), garantindo que suas dependências sejam resolvidas automaticamente pelo contêiner de DI. Isso promove o baixo acoplamento e a testabilidade.
   1. **Configuração do Database Context**: ApplicationDbContext é configurado para usar SQL Server.
   1. **Autenticação JWT Bearer**: Configura o middleware de autenticação, definindo como os tokens JWT serão validados (quem pode emitir, para quem se destina, validade, chave de assinatura). O RoleClaimType e NameClaimType são definidos para que o ASP.NET Core possa extrair informações de função e identificador do usuário do token.
   1. **Swagger/OpenAPI**: Configura o Swagger para gerar uma documentação interativa da API, incluindo a configuração de segurança para tokens Bearer, permitindo testes de endpoints autenticados diretamente na interface.
   1. **AutoMapper**: Registra o AutoMapper e seus perfis de mapeamento.
   1. **CORS**: Define políticas de Cross-Origin Resource Sharing para controlar quais origens podem acessar a API.
1. **Models**:
   1. Representam as entidades do domínio e o esquema do banco de dados. Por exemplo, Usuario é uma classe base para Psicologo e Responsavel, utilizando a herança Table-Per-Hierarchy (TPH) no Entity Framework Core, onde todos os tipos da hierarquia são mapeados para uma única tabela no banco de dados, com uma coluna discriminadora (Tipo) para identificar o tipo real da linha.
1. **Context/ApplicationDbContext.cs**:
   1. **Mapeamento Objeto-Relacional**: Define como suas classes Model se relacionam com as tabelas do banco de dados.
   1. **Relacionamentos de Chave Estrangeira**: Configura relacionamentos HasOne/WithMany e HasForeignKey para garantir a integridade referencial, com OnDelete(DeleteBehavior.Restrict) para evitar exclusões em cascata indesejadas que poderiam corromper dados.
   1. **Conversores de Valor**: Para tipos complexos como List<string> (MarcosAlcancados), um Value Converter é usado para serializar a lista em uma string JSON para armazenamento no banco de dados e deserializá-la de volta ao recuperar.
1. **Repositories**:
   1. **Abstração de Persistência**: Define contratos (IRepository) e implementações (Repository) para as operações de CRUD. Eles encapsulam a lógica de acesso ao banco de dados e são a única camada que interage diretamente com o DbContext.
1. **Services**:
   1. **Lógica de Negócio**: Esta é a camada onde as regras de negócio complexas e validações são aplicadas *antes* que os dados sejam persistidos. Por exemplo:
      1. CriancaService: Valida se uma criança tem pelo menos um pai ou mãe válido(a) e do gênero correto ao ser adicionada.
      1. AtividadeService: Garante que uma atividade só possa ser desativada se não estiver concluída.
      1. PsicologoService e UsuarioService: Verificam a unicidade de CPF e e-mail para evitar duplicações.
      1. ResponsavelService: Orquestra operações complexas envolvendo responsáveis e suas crianças, atividades e relatórios, utilizando o AutoMapper para conversão de DTOs.
   1. **JWT Generation**: JwtService é uma implementação de um serviço específico para geração de tokens JWT, desacoplando a lógica de autenticação dos controladores.
1. **DTOs**:
   1. **Separação de Preocupações**: Projetados para serem usados na camada de API. Eles evitam que os modelos de domínio sejam expostos diretamente, protegendo o esquema do banco de dados e permitindo que a API tenha um contrato diferente dos modelos internos.
## **Tecnologias Utilizadas**
- **ASP.NET Core 8.0 (com C#)**: Framework robusto e de alto desempenho para construção de APIs web modernas e escaláveis.
- **Entity Framework Core 9.0.5**: ORM (Object-Relational Mapper) que facilita a interação com o banco de dados, permitindo que você trabalhe com objetos C# em vez de SQL puro.
- **SQL Server**: Sistema de Gerenciamento de Banco de Dados Relacional (SGBD) utilizado para persistir os dados da aplicação.
- **JWT (JSON Web Tokens)**: Padrão para criação de tokens de acesso seguros, compactos e autossuficientes, usados para autenticação e autorização sem estado.
- **AutoMapper 12.0.1**: Uma biblioteca de mapeamento de objetos que reduz a necessidade de escrever código de mapeamento repetitivo entre diferentes camadas de objetos.
- **BCrypt.Net-Next 4.0.3**: Biblioteca para hash de senhas de forma segura, utilizando o algoritmo bcrypt.
- **Swashbuckle.AspNetCore 8.1.2**: Ferramentas para gerar, documentar e testar APIs RESTful de forma interativa diretamente do navegador (Swagger/OpenAPI).
- **Microsoft.AspNetCore.JsonPatch 9.0.4**: Para aplicar operações de PATCH em recursos de forma eficiente.
## **Requisitos de Sistema**
Para executar este projeto, você precisará ter os seguintes softwares instalados:

- [**.NET SDK 8.0 ou superior**](https://dotnet.microsoft.com/download): Certifique-se de instalar a versão 8.0 do SDK.
- [**SQL Server Express**](https://www.microsoft.com/en-us/sql-server/sql-server-downloads) (ou qualquer outra edição do SQL Server): O banco de dados para a persistência dos dados.
  - [**SQL Server Management Studio (SSMS)**](https://docs.microsoft.com/en-us/sql/ssms/download-sql-server-management-studio-ssms) (Opcional, mas recomendado para gerenciar o banco de dados).
- [**Visual Studio 2022**](https://visualstudio.microsoft.com/vs/) (Community, Professional ou Enterprise) ou [**Visual Studio Code**](https://code.visualstudio.com/): IDEs recomendadas para desenvolvimento C#.
  - Para VS Code: Extensão **C#** (por Microsoft) e **SQL Server** (por Microsoft) são úteis.
## **Instalação e Execução (Passo a Passo Completo)**
Siga estes passos para configurar e executar a API ASPCTS em seu ambiente local:
### **1. Clonar o Repositório**
Primeiro, clone o repositório para o seu ambiente local usando Git:

git clone <URL\_DO\_SEU\_REPOSITORIO>
cd ASPCTS

*Substitua <URL\_DO\_SEU\_REPOSITORIO> pela URL real do seu repositório Git (e.g., https://github.com/seu-usuario/ASPCTS.git).*
### **2. Instalar as Dependências (Pacotes NuGet)**
A aplicação utiliza diversos pacotes NuGet para sua funcionalidade. As versões exatas estão especificadas no seu arquivo ASPCTS.csproj. Para instalá-los, execute o comando de restauração de pacotes NuGet no terminal, dentro da pasta raiz do projeto (ASPCTS/):

dotnet restore

Este comando fará o download e instalará todos os pacotes listados no ItemGroup do seu csproj, incluindo:

- **AutoMapper (12.0.1)**: Biblioteca de mapeamento de objetos.
  - Instalação Manual (se necessário): dotnet add package AutoMapper --version 12.0.1
- **AutoMapper.Extensions.Microsoft.DependencyInjection (12.0.1)**: Integração do AutoMapper com o sistema de injeção de dependência do .NET.
  - Instalação Manual: dotnet add package AutoMapper.Extensions.Microsoft.DependencyInjection --version 12.0.1
- **BCrypt.Net-Next (4.0.3)**: Biblioteca para hash de senhas seguro.
  - Instalação Manual: dotnet add package BCrypt.Net-Next --version 4.0.3
- **Microsoft.AspNetCore.JsonPatch (9.0.4)**: Para aplicar operações JSON Patch.
  - Instalação Manual: dotnet add package Microsoft.AspNetCore.JsonPatch --version 9.0.4
- **Microsoft.EntityFrameworkCore (9.0.5)**: Pacote principal do Entity Framework Core.
  - Instalação Manual: dotnet add package Microsoft.EntityFrameworkCore --version 9.0.5
- **Microsoft.EntityFrameworkCore.Design (9.0.5)**: Ferramentas de tempo de design para migrações do EF Core.
  - Instalação Manual: dotnet add package Microsoft.EntityFrameworkCore.Design --version 9.0.5
- **Microsoft.EntityFrameworkCore.SqlServer (9.0.5)**: Provedor de banco de dados SQL Server para EF Core.
  - Instalação Manual: dotnet add package Microsoft.EntityFrameworkCore.SqlServer --version 9.0.5
- **Microsoft.EntityFrameworkCore.Tools (9.0.5)**: Ferramentas de linha de comando para EF Core (usadas para migrações).
  - Instalação Manual: dotnet add package Microsoft.EntityFrameworkCore.Tools --version 9.0.5
- **Swashbuckle.AspNetCore (8.1.2)**: Geração de documentação Swagger/OpenAPI.
  - Instalação Manual: dotnet add package Swashbuckle.AspNetCore --version 8.1.2
- **Microsoft.AspNetCore.Authentication.JwtBearer (8.0.5)**: Suporte para autenticação JWT Bearer.
  - Instalação Manual: dotnet add package Microsoft.AspNetCore.Authentication.JwtBearer --version 8.0.5
### **3. Configurar o Banco de Dados**
3\.1. Verificar e Configurar a String de Conexão:

\* Abra o arquivo appsettings.json na raiz do projeto.

\* Localize a seção ConnectionStrings. A string de conexão padrão é configurada para uma instância local do SQL Server Express:

json "ConnectionStrings": { "ConexaoPadrao": "Server=localhost\\SQLEXPRESS;Database=ASPCTSDB;Trusted\_Connection=True;TrustServerCertificate=True;" }

\* Crucial: Certifique-se de que o nome do servidor (localhost\\SQLEXPRESS neste exemplo) e a configuração de autenticação (Trusted\_Connection=True para autenticação Windows integrada) correspondem à sua configuração local do SQL Server.

\* Se você estiver usando SQL Server com autenticação de usuário/senha, a string de conexão precisará ser adaptada (e.g., User ID=seu\_usuario;Password=sua\_senha;).

\* Certificado do Servidor: TrustServerCertificate=True é usado para permitir conexões sem validação de certificado TLS/SSL, o que é comum em ambientes de desenvolvimento local. Em ambientes de produção, é recomendado configurar a validação de certificado.

3\.2. Aplicar Migrações do Entity Framework Core:

\* As migrações são arquivos de código que representam as alterações no esquema do seu banco de dados ao longo do tempo. Elas são geradas automaticamente pelo Entity Framework Core com base nas suas classes de modelo (Models).

\* Abrir um terminal (Prompt de Comando, PowerShell ou Terminal integrado do VS Code/Visual Studio) na raiz do projeto ASPCTS (onde o arquivo ASPCTS.csproj está localizado).

\* Execute o seguinte comando para aplicar todas as migrações pendentes e criar o banco de dados ASPCTSDB (se ele ainda não existir) com as tabelas correspondentes aos seus modelos. Este comando também irá atualizar o esquema do banco de dados se houver novas migrações que foram adicionadas:

bash dotnet ef database update

\* Comandos Adicionais para Migrações (para Desenvolvedores):

\* Para criar uma nova migração (após alterar seus modelos):

bash dotnet ef migrations add NomeDaSuaNovaMigracao

Isso gerará um novo arquivo na pasta Migrations detalhando as alterações no esquema.

\* Para remover a última migração aplicada (cuidado):

bash dotnet ef migrations remove

\* Para reverter para uma migração específica (cuidado):

bash dotnet ef database update NomeDaMigracaoAnterior

Para reverter todas as migrações e retornar o banco de dados a um estado inicial (sem tabelas), use:

bash dotnet ef database update 0
### **4. Executar a Aplicação**
4\.1. No Visual Studio (Recomendado para Desenvolvimento):

\* Abra o arquivo de solução ASPCTS.sln no Visual Studio.

\* O Visual Studio detectará e restaurará automaticamente os pacotes NuGet.

\* Pressione F5 ou clique no botão Run (geralmente um triângulo verde com "IIS Express" ou o nome do seu projeto) na barra de ferramentas. Isso compilará o projeto, iniciará o servidor Kestrel (ou IIS Express) e abrirá o navegador na URL configurada para o Swagger.

4\.2. No Visual Studio Code ou Terminal (Alternativo):

\* Abrir o terminal na raiz do projeto ASPCTS.

\* Execute o seguinte comando para compilar e iniciar a aplicação:

bash dotnet run

\* O terminal exibirá as URLs onde a aplicação está ouvindo (geralmente https://localhost:7060 e http://localhost:5023). Copie e cole a URL HTTPS no seu navegador.
### **5. Acessar a API e Documentação Swagger**
Após a execução bem-sucedida, a API estará acessível em:

- **URL da API (HTTPS):** https://localhost:7060
- **URL da API (HTTP - apenas para desenvolvimento):** http://localhost:5023
- **Documentação Swagger UI:** https://localhost:7060/ (ou http://localhost:5023/ para HTTP)
  - A página do Swagger UI é a interface recomendada para explorar e testar todos os endpoints da API de forma interativa. O Program.cs está configurado para exibir o Swagger UI na raiz (RoutePrefix = string.Empty;) quando em ambiente de desenvolvimento, facilitando o acesso.
## **Autenticação**
A API utiliza autenticação baseada em JWT. Para interagir com endpoints protegidos (que exigem autenticação), você deve seguir estes passos:

1. **Registrar um Usuário:** Utilize o endpoint POST /api/Auth/register para criar um novo usuário (Psicólogo ou Responsável).
1. **Fazer Login:** Utilize o endpoint POST /api/Auth/login com as credenciais do usuário registrado. A resposta incluirá um token JWT.
1. **Autorizar no Swagger UI:**
   1. Na interface do Swagger UI, clique no botão **"Authorize"** (geralmente no canto superior direito).
   1. No campo de valor, insira o token JWT obtido no login, prefixado com Bearer (com um espaço no final). Exemplo: Bearer eyJhbGciOiJIUzI1NiIsIn....
   1. Clique em Authorize. Agora, todas as suas requisições feitas através do Swagger UI incluirão este token no cabeçalho Authorization.
1. **Em Requisições HTTP (Exemplo cURL/Postman):**
   1. Para requisições fora do Swagger UI, inclua o cabeçalho Authorization com o token:
      Authorization: Bearer <SEU\_TOKEN\_JWT>
## **Endpoints da API**
A API ASPCTS expõe os seguintes grupos de rotas (endpoints). Utilize a documentação Swagger UI para detalhes sobre parâmetros, modelos de requisição/resposta e códigos de status HTTP.
### **Autenticação (/api/Auth)**
- POST /api/Auth/register: Registra um novo usuário (Psicólogo ou Responsável).
- POST /api/Auth/login: Realiza o login do usuário e retorna um token JWT.
### **Atividades (/api/Atividade) - *Requer autenticação***
- GET /api/Atividade: Obtém todas as atividades cadastradas (filtrando por crianças ativas).
- GET /api/Atividade/{id}: Obtém uma atividade específica pelo ID.
- GET /api/Atividade/crianca/{criancaId}: Obtém todas as atividades de uma criança específica.
- POST /api/Atividade: Adiciona uma nova atividade.
- PUT /api/Atividade/{id}: Atualiza uma atividade existente pelo ID.
- DELETE /api/Atividade/{id}: Desativa (marca como inativa) uma atividade pelo ID (somente se não estiver concluída).
### **Crianças (/api/Crianca) - *Requer autenticação***
- GET /api/Crianca: Obtém todas as crianças cadastradas.
- GET /api/Crianca/{id}: Obtém uma criança específica pelo ID.
- GET /api/Crianca/by-user-access: Obtém as crianças que o usuário autenticado tem permissão para visualizar.
- POST /api/Crianca: Adiciona uma nova criança.
- PUT /api/Crianca/{id}: Atualiza uma criança existente pelo ID.
- DELETE /api/Crianca/{id}: Desativa (marca como inativa) uma criança pelo ID.
### **Psicólogos (/api/Psicologo) - *Requer autenticação***
- GET /api/Psicologo: Obtém todos os psicólogos cadastrados.
- GET /api/Psicologo/{id}: Obtém um psicólogo específico pelo ID.
- POST /api/Psicologo: Adiciona um novo psicólogo.
- PUT /api/Psicologo/{id}: Atualiza um psicólogo existente pelo ID.
- DELETE /api/Psicologo/{id}: Desativa (marca como inativa) um psicólogo pelo ID.
### **Relatórios (/api/Relatorio) - *Requer autenticação***
- GET /api/Relatorio: Obtém todos os relatórios cadastrados.
- GET /api/Relatorio/{id}: Obtém um relatório específico pelo ID.
- GET /api/Relatorio/crianca/{criancaId}: Obtém todos os relatórios de uma criança específica.
- POST /api/Relatorio: Adiciona um novo relatório.
- PUT /api/Relatorio/{id}: Atualiza um relatório existente pelo ID.
- DELETE /api/Relatorio/{id}: Desativa (marca como inativa) um relatório pelo ID.
### **Responsáveis (/api/Responsavel) - *Requer autenticação***
- GET /api/Responsavel/{id}/criancas: Obtém as crianças associadas a um responsável.
- GET /api/Responsavel/{id}/atividades-das-criancas: Obtém as atividades de todas as crianças associadas a um responsável.
- GET /api/Responsavel/{id}/relatorios-das-criancas: Obtém os relatórios de todas as crianças associadas a um responsável.
- GET /api/Responsavel/{id}/psicologo-das-criancas: Obtém o psicólogo associado às crianças de um responsável.
- GET /api/Responsavel/por-psicologo/{psicologoId}: Obtém os responsáveis associados a um psicólogo específico.
- PUT /api/Responsavel/{id}: Atualiza um responsável existente pelo ID.
- PUT /api/Responsavel/psicologo/{psicologoId}/responsavel/{responsavelId}: Atualiza um responsável específico por um psicólogo.
- DELETE /api/Responsavel/{id}: Desativa (marca como inativa) um responsável pelo ID.
## **Estratégia de Branching**
Recomendamos uma estratégia de branching como **Git Flow** para gerenciamento de código, que organiza o desenvolvimento em branches de propósito específico:

- main: Branch de código de produção, sempre estável e pronto para deploy.
- develop: Branch de integração principal para novas funcionalidades e correções.
- feature/nome-da-funcionalidade: Branches criadas a partir de develop para o desenvolvimento isolado de novas funcionalidades.
- bugfix/nome-do-bug: Branches criadas a partir de develop para correção de bugs no ambiente de desenvolvimento.
- hotfix/nome-do-hotfix: Branches criadas a partir de main para correções urgentes em produção.
## **Contribuição**
Contribuições são **extremamente bem-vindas** e valorizadas! Se você deseja colaborar com o projeto, siga estas diretrizes:

1. Faça um fork do repositório principal no GitHub.
1. Clone o seu fork para a sua máquina local.
1. Crie uma nova branch para sua funcionalidade ou correção. Use nomes descritivos, como feature/adicionar-relatorio-personalizado ou bugfix/erro-no-login:
   git checkout develop
   git pull origin develop # Garanta que sua develop esteja atualizada
   git checkout -b feature/minha-nova-funcionalidade
1. Faça suas alterações no código.
1. Comite suas alterações com mensagens claras e concisas que descrevam o que foi feito:
   git add .
   git commit -m "feat: Adiciona funcionalidade X para Y"
1. Envie suas alterações para o seu fork no GitHub:
   git push origin feature/minha-nova-funcionalidade
1. Abra um Pull Request (PR) do seu fork para a branch develop do repositório principal.
1. No PR, forneça uma descrição detalhada das suas alterações, explicando o problema que resolve ou a funcionalidade que implementa.
1. Aguarde a revisão do código. Esteja preparado para feedbacks e possíveis solicitações de alterações.
## **Licença**
Este projeto é de código aberto e está licenciado sob a [Nome da Licença, por exemplo, MIT License](https://opensource.org/licenses/MIT) (se houver um arquivo LICENSE.md). Por favor, consulte o arquivo LICENSE.md na raiz do repositório para mais detalhes.
