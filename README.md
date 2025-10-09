# ASP.NET Core Web API (.NET 8 / .NET 9)

Este repositório contém os projetos e exercícios do curso **Web API ASP.NET Core** ministrado por [Jose Carlos Macoratti](https://github.com/macoratti), abordando a criação de APIs REST usando .NET 8 e .NET 9.

## 📚 Principais Tópicos

- **Fundamentos:** REST, JSON, HTTP, Entity Framework Core
- **Recursos Essenciais:** Roteamento, filtros, async/await, logging, paginação
- **Padrões:** Repository, Unit of Work, AutoMapper, DTOs
- **Segurança:** JWT, autorização, CORS
- **Documentação:** Swagger/OpenAPI, versionamento
- **Consumo de APIs:** Angular, Windows Forms, React, OData, GraphQL
- **Bônus:** Minimal APIs e microsserviços


# APICatalogo — Guia rápido e simples (Português BR)

Este README mostra o menor conjunto de passos para clonar, configurar, criar/popular o banco e executar a API. Faça exatamente na ordem indicada. Tudo em .NET 8 + MySQL. 

> ⚠️ **Importante:** Não comite o arquivo `.env`

---

## Pré-requisitos

- .NET 8 SDK instalado
- MySQL Server acessível (local ou remoto)
- Terminal (PowerShell no Windows; Bash no Linux/Mac/WSL)

---

## 1) Clonar repositório

```bash
git clone https://github.com/mapompeo/aspnetcore-webapi.git
cd aspnetcore-webapi/APICatalogo
```

## 2) Criar o arquivo de configuração local (.env)

### Windows (PowerShell)

```powershell
Copy-Item .env.template .env
notepad .env
```

### Linux / Mac / WSL

```bash
cp .env.template .env
nano .env
```

No `.env` ajuste apenas a string de conexão MySQL (`ConnectionStrings__DefaultConnection`) com seu servidor, usuário e senha. Salve e feche.

**Exemplo:**

```env
ConnectionStrings__DefaultConnection="Server=localhost;Database=catalogodb;Uid=root;Pwd=suasenha"
LOG_PATH="APICatalogo/log/log.txt"
```

## 3) Comando único para instalar dependências, restaurar pacotes e inicializar o banco (migrations/seed)

### Windows (PowerShell)

Cole e execute as linhas em sequência:

```powershell
dotnet add APICatalogo package DotNetEnv
dotnet add APICatalogo package Pomelo.EntityFrameworkCore.MySql
dotnet add APICatalogo package Microsoft.EntityFrameworkCore.Design
dotnet restore
.\scripts\init-db.ps1
```

### Linux / Mac / WSL (Bash)

```bash
dotnet add APICatalogo package DotNetEnv
dotnet add APICatalogo package Pomelo.EntityFrameworkCore.MySql
dotnet add APICatalogo package Microsoft.EntityFrameworkCore.Design
dotnet restore
chmod +x ./scripts/init-db.sh
./scripts/init-db.sh
```

O script `init-db` irá criar/aplicar migrations (se houver) ou criar o esquema e inserir os dados de exemplo (cardápio de restaurante) caso as tabelas estejam vazias.

## 4) Executar a API

```bash
dotnet run --project APICatalogo
```

Abra o endereço HTTPS informado no console (ex.: `https://localhost:7160`) e acesse o Swagger.

---

## 📝 Estrutura do Projeto

```
aspnetcore-webapi/
└── APICatalogo/
    ├── .env.template
    ├── .env (não comitar)
    ├── scripts/
    │   ├── init-db.ps1
    │   └── init-db.sh
    └── ...
```

## 🔧 Solução de Problemas

- **Erro de conexão MySQL:** Verifique se o servidor MySQL está rodando e se as credenciais no `.env` estão corretas
- **Erro de permissão no script:** No Linux/Mac, certifique-se de ter executado `chmod +x ./scripts/init-db.sh`
- **Porta em uso:** Se a porta padrão estiver ocupada, o .NET escolherá outra automaticamente

## 📚 Próximos Passos

Após iniciar a API com sucesso:

1. Explore os endpoints disponíveis no Swagger
2. Teste as operações CRUD de categorias e produtos
3. Consulte a documentação da API para mais detalhes

---

**Desenvolvido com .NET 8 e MySQL** 🚀