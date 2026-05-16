var builder = WebApplication.CreateBuilder(args);

// 1. Controllers
builder.Services.AddControllers();

// 2. NSwag — funciona com .NET 10
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApiDocument(c =>
{
    c.Title   = "Padaria - Product Service";
    c.Version = "v1";
    c.Description = "Gerenciamento de produtos e categorias da padaria";
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    // Gera o JSON da documentação em /swagger/v1/swagger.json
    app.UseOpenApi();

    // Interface visual em /swagger
    app.UseSwaggerUi(c =>
    {
        c.Path = "/swagger";
        c.DocumentPath = "/swagger/v1/swagger.json";
    });
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();