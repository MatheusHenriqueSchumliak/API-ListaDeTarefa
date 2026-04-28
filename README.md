# Descrição do Projeto

## Visão Geral do Projeto
Este é um projeto de API de Lista de Tarefas, onde os usuários podem gerenciar suas tarefas de forma eficaz.

## Tecnologias
- .NET 8
- ASP.NET Core Web API
- Swagger/Swashbuckle
- EF Core 8 (SQL Server)
- FluentValidation
- Arquitetura em camadas (Domain/Application/Infrastructure)

## Tarefa
- Criar uma API RESTful para gerenciamento de tarefas.
- Implementar CRUD: Criar, Ler, Atualizar e Deletar tarefas.
- Implementar documentação utilizando Swagger.

## Observação sobre o Banco de Dados
O projeto utiliza SQL Server LocalDB, configurado no arquivo appsettings.json.

## Endpoints Básicos
### CRUD Tarefas
- **POST** /tarefas: Criar uma nova tarefa
- **GET** /tarefas: Listar todas as tarefas
- **GET** /tarefas/{id}: Recuperar uma tarefa específica
- **GET** /tarefas/{descricao}: Busca por uma tarefa por descricao
- **PUT** /tarefas/{id}: Atualizar uma tarefa
- **DELETE** /tarefas/{id}: Deletar uma tarefa

### Valores de Status
- P: Pendente
- C: Concluída

## Arquitetura do projeto
- **Domain:** Contém as entidades e regras de negócio.
- **Application:** Contém a lógica de aplicação e os serviços.
- **Infrastructure:** Contém a implementação da base de dados e outras dependências externas.

## Como Executar a API
  - .NET 8 SDK instalado
  - SQL Server configurado
Para rodar a API, execute os seguintes comandos:
1. `dotnet restore`
2. `dotnet build`
3. `dotnet run`

## Comandos do EF Core
- `dotnet ef migrations add AdicionaClasseTarefa --startup-project ../ListaDeTarefa`
- `dotnet ef database update --startup-project ../ListaDeTarefa`

### URL do Swagger
Após executar a API, você pode acessar a documentação do Swagger em: `http://localhost:<porta>/swagger`
