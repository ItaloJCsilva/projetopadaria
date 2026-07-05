using MassTransit;
using Padaria.NotificationService.Config;
using Padaria.NotificationService.Consumers;
using Padaria.NotificationService.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddScoped<IServicoEmail, ServicoEmail>();
builder.Services.Configure<ConfiguracaoEmail>(
    builder.Configuration.GetSection("Email"));
builder.Services.AddMassTransit(x =>
{

    x.AddConsumer<ConsumidorPedidoCriado>();
    x.AddConsumer<ConsumidorStatusPedidoAlterado>();
    x.AddConsumer<ConsumidorPedidoConcluido>();

    x.UsingRabbitMq((ctx, cfg) =>
    {
        cfg.Host(builder.Configuration["RabbitMQ:Host"], "/", h =>
        {
            h.Username(builder.Configuration["RabbitMQ:Usuario"]!);
            h.Password(builder.Configuration["RabbitMQ:Senha"]!);
        });
        cfg.ReceiveEndpoint("padaria-pedido-criado", e =>
        {
            e.ConfigureConsumer<ConsumidorPedidoCriado>(ctx);
            e.UseMessageRetry(r => r.Interval(3, TimeSpan.FromSeconds(5)));
        });
        cfg.ReceiveEndpoint("padaria-status-alterado", e =>
        {
            e.ConfigureConsumer<ConsumidorStatusPedidoAlterado>(ctx);
            e.UseMessageRetry(r => r.Interval(3, TimeSpan.FromSeconds(5)));
        });
        cfg.ReceiveEndpoint("padaria-pedido-concluido", e =>
        {
            e.ConfigureConsumer<ConsumidorPedidoConcluido>(ctx);
            e.UseMessageRetry(r =>
                r.Interval(3, TimeSpan.FromSeconds(5)));
        });
    });
});
var app = builder.Build();
app.Run();