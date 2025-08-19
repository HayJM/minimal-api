# Minimal API

Este projeto é uma API minimalista desenvolvida em .NET 8, utilizando Entity Framework Core e MySQL, com autenticação JWT e testes automatizados.

## Funcionalidades
- Cadastro, login e consulta de administradores
- Cadastro, consulta, atualização e remoção de veículos
- Autenticação via JWT
- Testes automatizados com mocks
- Documentação automática via Swagger

## Estrutura do Projeto
```
minimal-api/
├── Api/                # Código principal da API
├── Dominio/            # Entidades, DTOs, interfaces e serviços
├── InfraEstrutura/     # Contexto do banco de dados
├── Test/               # Testes automatizados e mocks
├── appsettings.json    # Configurações gerais
├── Program.cs          # Configuração da API
├── Startup.cs          # Configuração de serviços
└── README.md           # Este arquivo
```

## Como rodar
1. Instale o .NET 8 SDK e o MySQL.
2. Configure a string de conexão em `appsettings.json`.
3. Execute as migrations:
   ```bash
   dotnet ef database update
   ```
4. Rode a API:
   ```bash
   dotnet run --project Api
   ```
5. Acesse o Swagger em `http://localhost:5000/swagger`.

## Como rodar os testes
```bash
cd Test
 dotnet test
```

## Endpoints principais
- `POST /Adminstradores/login` — Login de administrador
- `POST /Adminstradores` — Cadastro de administrador
- `GET /Adminstradores` — Listagem paginada de administradores
- `GET /veiculos` — Listagem paginada de veículos
- `POST /veiculos` — Cadastro de veículo
- `PUT /veiculos/{id}` — Atualização de veículo
- `DELETE /veiculos/{id}` — Remoção de veículo

## Autenticação
- Utilize o endpoint de login para obter o token JWT.
- Envie o token no header `Authorization: Bearer <token>` para acessar endpoints protegidos.

## Observações
- O projeto utiliza mocks para facilitar testes automatizados.
- O ambiente de testes pode ser configurado com banco em memória ou mocks.

## Licença
MIT
