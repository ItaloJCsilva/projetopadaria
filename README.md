#  Padaria Online

Sistema que simula uma padaria.


## Sobre o projeto

Projeto realizado como atividade para a 1ª unidade do curso de analise e desenvolvimento de sistemas do Ifpe campus Paulista.
O sistema possui duas interfaces distintas um site para o cliente realizar pedidos online onde o usuario podera fazer login, e uma interface de caixa para atendimento presencial. O backend é construído com arquitetura de microserviços com comunicação assíncrona via RabbitMQ e banco de dados para a persistencia.

## Tecnologias

**Backend**
- .NET 10  C# Web API
- Entity Framework Core  ORM
- MySQL  banco de dados
- RabbitMQ + MassTransit  mensageria Publish-Subscribe
- JWT Bearer  autenticação e autorização
- NSwag  documentação Swagger
- Docker + Docker Compose  containerização

**Frontend** (Em adamento)
- Angular 17
- TypeScript



## Pré-requisitos

- Docker e Docker Compose
- .NET 10 SDK (para desenvolvimento local)
- MySQL Workbench (opcional para visualizar o banco)

## Como executar

**1. Clonar o repositório**
```bash
git clone https://github.com/ItaloJCsilva/projetopadaria.git
cd projetopadaria
```

**2. Configurar as variáveis de ambiente**
```bash
cd back
```

**3. Subir todos os serviços**
```bash
docker-compose up --build
```

**4. Acessar**

| Serviço | URL |
|---|---|
| Auth API | http://localhost:5001/swagger |
| Produtos API | http://localhost:5002/swagger |
| Pedidos API | http://localhost:5003/swagger |
| RabbitMQ Painel | http://localhost:15672 |

Login RabbitMQ: `guest` / `guest`

## Executar sem Docker

**1. Criar os bancos no MySQL Workbench**

Execute o script `back/scripts/init.sql` no MySQL Workbench.

**2. Configurar a conexão** em cada `appsettings.json`:
```json
"ConnectionStrings": {
  "Padrao": "Server=localhost;Port=3306;Database=padaria_xxx;Uid=root;Pwd=suasenha;"
}
```

**3. Subir cada serviço em terminais separados**
```bash
cd back/src/Padaria.AuthService && dotnet run        # porta 5001
cd back/src/Padaria.ProductService && dotnet run     # porta 5002
cd back/src/Padaria.OrderService && dotnet run       # porta 5003
cd back/src/Padaria.NotificationService && dotnet run
```

