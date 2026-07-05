using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using NSwag.Generation.Processors.Security;
using Padaria.ProductService.Data;
using Padaria.ProductService.Repositories;
using Padaria.ProductService.Repositories.interfaces;
using Padaria.ProductService.Services;
using Padaria.ProductService.Services.interfaces;
using Padaria.ProductService.Storage;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
    .AddJsonOptions(opcoes =>
    {
        opcoes.JsonSerializerOptions.PropertyNamingPolicy =
            System.Text.Json.JsonNamingPolicy.CamelCase;
    });

builder.Services.AddDbContext<ContextoProduto>(opcoes =>
    opcoes.UseMySql(
        builder.Configuration.GetConnectionString("Padrao"),
        new MySqlServerVersion(new Version(8, 0, 0))
    ));

builder.Services.AddScoped<IRepositorioCategoria, RepositorioCategoria>();
builder.Services.AddScoped<IRepositorioProduto, RepositorioProduto>();
builder.Services.AddScoped<IServicoCategoria, ServicoCategoria>();
builder.Services.AddScoped<IServicoProduto, ServicoProduto>();
builder.Services.AddScoped<S3Service>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("PermitirAngular", policy =>
    {
        policy.WithOrigins("http://localhost:4200")
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(opcoes =>
    {
        opcoes.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime  = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer= builder.Configuration["Jwt:Emissor"],
            ValidAudience = builder.Configuration["Jwt:Audiencia"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Segredo"]!))
        };
    });

builder.Services.AddAuthorization();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApiDocument(c =>
{
    c.Title = "Padaria - Serviço de Produtos";
    c.Version  = "v1";
    c.Description = "Gerenciamento de produtos e categorias do cardápio";

    c.AddSecurity("Bearer", new NSwag.OpenApiSecurityScheme
    {
        Type  = NSwag.OpenApiSecuritySchemeType.Http,
        Scheme  = "bearer",
        BearerFormat = "JWT",
        Description = "Cole apenas o token JWT"
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
        c.Path = "/swagger";
        c.DocumentPath = "/swagger/v1/swagger.json";
    });
}

app.UseHttpsRedirection();
app.UseCors("PermitirAngular");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();