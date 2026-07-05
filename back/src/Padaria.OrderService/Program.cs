using System.Text;
using System.Text.Json.Serialization;
using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using NSwag.Generation.Processors.Security;
using Padaria.OrderService.Data;
using Padaria.OrderService.Messaging;
using Padaria.OrderService.Repositories;
using Padaria.OrderService.Repositories.interfaces;
using Padaria.OrderService.Services;
using Padaria.OrderService.Services.interfaces;



var builder = WebApplication.CreateBuilder(args);
builder.Services.AddCors(options =>
{
    options.AddPolicy("Angular", policy =>
    {
        policy
            .WithOrigins("http://localhost:4200")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});
builder.Services.AddControllers()
    .AddJsonOptions(opcoes =>
    {
        opcoes.JsonSerializerOptions.PropertyNamingPolicy =
            System.Text.Json.JsonNamingPolicy.CamelCase;
        opcoes.JsonSerializerOptions.Converters.Add(
            new JsonStringEnumConverter());
    });
builder.Services.AddDbContext<ContextoPedido>(opcoes =>
    opcoes.UseMySql(
        builder.Configuration.GetConnectionString("Padrao"),
        new MySqlServerVersion(new Version(8, 0, 0))
    ));
builder.Services.AddScoped<IRepositorioPedido, RepositorioPedido>();
builder.Services.AddScoped<IServicoPedido, ServicoPedido>();
builder.Services.AddScoped<PublicadorPedido>();
builder.Services.AddMassTransit(x =>
{
    x.UsingRabbitMq((ctx, cfg) =>
    {
        cfg.Host(builder.Configuration["RabbitMQ:Host"], "/", h =>
        {
            h.Username(builder.Configuration["RabbitMQ:Usuario"]!);
            h.Password(builder.Configuration["RabbitMQ:Senha"]!);
        });
        cfg.UseMessageRetry(r => r.Interval(2, TimeSpan.FromSeconds(5)));
        cfg.ConfigureEndpoints(ctx);
    });
});
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(opcoes =>
    {
        opcoes.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer  = true,
            ValidateAudience = true,
            ValidateLifetime   = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Emissor"],
            ValidAudience  = builder.Configuration["Jwt:Audiencia"],
            IssuerSigningKey  = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Segredo"]!))
        };
    });

builder.Services.AddAuthorization();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApiDocument(c =>
{
    c.Title= "Padaria - Serviço de Pedidos";
    c.Version = "v1";
    c.Description = "Criação e gerenciamento de pedidos online e do caixa";

    c.AddSecurity("Bearer", new NSwag.OpenApiSecurityScheme
    {
        Type  = NSwag.OpenApiSecuritySchemeType.Http,
        Scheme  = "bearer",
        BearerFormat = "JWT",
        Description  = "Cole apenas o token JWT, sem a palavra Bearer"
    });

    c.OperationProcessors.Add(
        new AspNetCoreOperationSecurityScopeProcessor("Bearer")
    );
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseOpenApi();
    app.UseSwaggerUi(c =>
    {
        c.Path         = "/swagger";
        c.DocumentPath = "/swagger/v1/swagger.json";
    });
}
app.UseHttpsRedirection();
app.UseCors("Angular");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();