using MassTransit;
using Padaria.NotificationService.Consumers;
using Padaria.NotificationService.Services;


var builder = WebApplication.CreateBuilder(args);

// 1. Serviço de email
builder.Services.AddScoped<IServicoEmail, ServicoEmail>();

// 2. RabbitMQ com MassTransit
//    Registra os consumidores e conecta no RabbitMQ
builder.Services.AddMassTransit(x =>
{
    // Registra os consumidores
    // O MassTransit cria as filas automaticamente no RabbitMQ
    x.AddConsumer<ConsumidorPedidoCriado>();
    x.AddConsumer<ConsumidorStatusPedidoAlterado>();

    x.UsingRabbitMq((ctx, cfg) =>
    {
        cfg.Host(builder.Configuration["RabbitMQ:Host"], "/", h =>
        {
            h.Username(builder.Configuration["RabbitMQ:Usuario"]!);
            h.Password(builder.Configuration["RabbitMQ:Senha"]!);
        });

        // Fila do evento PedidoCriadoEvento
        // Nome da fila gerado automaticamente pelo MassTransit
        cfg.ReceiveEndpoint("padaria-pedido-criado", e =>
        {
            e.ConfigureConsumer<ConsumidorPedidoCriado>(ctx);

            // Se der erro no consumo tenta 3 vezes
            // antes de mandar para a fila de erro
            e.UseMessageRetry(r => r.Interval(3, TimeSpan.FromSeconds(5)));
        });

        // Fila do evento StatusPedidoAlteradoEvento
        cfg.ReceiveEndpoint("padaria-status-alterado", e =>
        {
            e.ConfigureConsumer<ConsumidorStatusPedidoAlterado>(ctx);
            e.UseMessageRetry(r => r.Interval(3, TimeSpan.FromSeconds(5)));
        });
    });
});

var app = builder.Build();

app.Run();