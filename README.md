# Padaria Online

Sistema web para gerenciamento de uma padaria, permitindo a realização de pedidos online por clientes e o atendimento presencial através de uma interface de caixa. O projeto utiliza arquitetura de microserviços, comunicação assíncrona por mensageria e autenticação baseada em JWT.

---

# Sobre o projeto

Este projeto foi desenvolvido como atividade da 1ª unidade do curso de Análise e Desenvolvimento de Sistemas do IFPE Campus Paulista.

A aplicação foi dividida em dois módulos principais:

- **Portal do Cliente:** permite cadastro, autenticação e realização de pedidos online.
- **Sistema de Caixa:** destinado ao atendimento presencial e gerenciamento dos pedidos.

O backend foi desenvolvido seguindo uma arquitetura de microserviços, onde cada serviço possui sua própria responsabilidade e banco de dados, comunicando-se de forma assíncrona através do RabbitMQ utilizando MassTransit.

## Funcionalidades

- Cadastro e autenticação de usuários
- Autorização baseada em perfis (Cliente, Atendente e Administrador)
- Gerenciamento de produtos
- Realização de pedidos online
- Atendimento de pedidos no caixa
- Comunicação assíncrona entre microserviços
- Documentação das APIs com Swagger
- Execução completa utilizando Docker Compose

---

# Arquitetura

O backend é composto pelos seguintes microserviços:

- **Auth Service**
  - Autenticação
  - Cadastro de usuários
  - Geração de tokens JWT

- **Product Service**
  - Cadastro de produtos
  - Consulta de produtos

- **Order Service**
  - Criação e gerenciamento de pedidos

- **Notification Service**
  - Consumo de eventos publicados no RabbitMQ

Além disso, o sistema utiliza:

- RabbitMQ para troca de mensagens
- Bancos MySQL independentes para cada serviço
- Docker Compose para orquestração da aplicação

---

# Tecnologias utilizadas

## Backend

- .NET 10
- ASP.NET Core Web API
- C#
- Entity Framework Core
- MySQL
- RabbitMQ
- MassTransit
- JWT Bearer Authentication
- NSwag (Swagger)
- Docker
- Docker Compose

## Frontend

- Angular
- TypeScript
- Angular Material

---

# Estrutura do projeto

```
PadariaOnline
│
├── back
│   ├── src
│   │   ├── Padaria.AuthService
│   │   ├── Padaria.ProductService
│   │   ├── Padaria.OrderService
│   │   ├── Padaria.NotificationService
│   │   └── Shared
│   │
│   ├── scripts
│   └── docker-compose.yml
│
└── front
```

---

# Pré-requisitos

Para executar o projeto é necessário possuir instalado:

- Docker
- Docker Compose
- .NET 10 SDK (caso deseje executar sem Docker)
- MySQL Server (caso deseje executar sem Docker)

---

# Executando com Docker

## 1. Clone o repositório

```bash
git clone https://github.com/ItaloJCsilva/projetopadaria.git
```

## 2. Entre na pasta do backend

```bash
cd projetopadaria/back
```

## 3. Execute os containers

```bash
docker-compose up --build
```

Na primeira execução o processo pode levar alguns minutos devido à criação das imagens.

---

# Serviços disponíveis

| Serviço | URL |
|----------|-----|
| Auth Service | http://localhost:5001/swagger |
| Product Service | http://localhost:5002/swagger |
| Order Service | http://localhost:5003/swagger |
| RabbitMQ Management | http://localhost:15672 |

Credenciais do RabbitMQ:

```
Usuário: guest
Senha: guest
```

---

# Executando sem Docker

## 1. Criar os bancos de dados

Execute o script localizado em:

```
back/scripts/init.sql
```

Utilizando o MySQL Workbench ou outro cliente MySQL.

---

## 2. Configurar a string de conexão

Em cada arquivo `appsettings.json` configure:

```json
"ConnectionStrings": {
  "Padrao": "Server=localhost;Port=3306;Database=padaria_xxx;Uid=root;Pwd=suasenha;"
}
```

---

## 3. Executar os microserviços

Em terminais separados execute:

### Auth Service

```bash
cd back/src/Padaria.AuthService
dotnet run
```

### Product Service

```bash
cd back/src/Padaria.ProductService
dotnet run
```

### Order Service

```bash
cd back/src/Padaria.OrderService
dotnet run
```

### Notification Service

```bash
cd back/src/Padaria.NotificationService
dotnet run
```

---

# Autenticação

A autenticação é realizada utilizando JWT Bearer.

Após efetuar login no Auth Service, utilize o token retornado para acessar os endpoints protegidos das demais APIs.

---

# Documentação

Cada microserviço disponibiliza sua documentação Swagger através do NSwag.

Após iniciar a aplicação, acesse:

- http://localhost:5001/swagger
- http://localhost:5002/swagger
- http://localhost:5003/swagger

---

# Status do projeto

Atualmente o projeto encontra-se em desenvolvimento.

Funcionalidades implementadas:

- Autenticação de usuários
- Cadastro de produtos
- Gerenciamento de pedidos
- Comunicação entre microserviços
- Containerização com Docker
- Documentação das APIs

Em desenvolvimento:

- Interface web em Angular
- Melhorias na experiência do usuário
- Integração completa entre frontend e backend

---

# Autor

Desenvolvido por **Italo José** como projeto acadêmico do curso de Análise e Desenvolvimento de Sistemas do IFPE Campus Paulista.