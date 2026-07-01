using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using NSwag.Generation.Processors.Security;
using Padaria.AuthService.Data;
using Padaria.AuthService.Repositories.interfaces;
using Padaria.AuthService.Repositorios;
using Padaria.AuthService.Services;
using Padaria.AuthService.Services.interfaces;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddCors(options =>
{
    options.AddPolicy("PermitirAngular", policy =>
    {
        policy.WithOrigins("http://localhost:4200") // URL do seu frontend
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

//builder.Services.AddControllers();
builder.Services.AddControllers()
    .AddJsonOptions(opcoes =>
        opcoes.JsonSerializerOptions.PropertyNamingPolicy = null); 
builder.Services.AddDbContext<ContextoAutenticacao>(opcoes =>
    opcoes.UseMySql(
        builder.Configuration.GetConnectionString("Padrao"),
        ServerVersion.AutoDetect(
            builder.Configuration.GetConnectionString("Padrao"))
    ));
builder.Services.AddScoped<IRepositorioUsuario, RepositorioUsuario>();
builder.Services.AddScoped<IServicoAutenticacao, ServicoAutenticacao>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(opcoes =>
    {
        opcoes.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Emissor"],
            ValidAudience = builder.Configuration["Jwt:Audiencia"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Segredo"]!))
        };
    });

builder.Services.AddAuthorization();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApiDocument(c =>
{
    c.Title = "Padaria - Serviço de Autenticação";
    c.Version = "v1";
    c.Description = "Cadastro, login e perfil de usuário";
    c.AddSecurity("Bearer", new NSwag.OpenApiSecurityScheme
    {
        Type = NSwag.OpenApiSecuritySchemeType.Http,
        Scheme = "bearer",
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