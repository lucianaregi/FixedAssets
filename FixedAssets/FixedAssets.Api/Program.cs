using FixedAssets.Application.Interfaces;
using FixedAssets.Application.Services;
using FixedAssets.Infrastructure.Interfaces;
using FixedAssets.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Configurar a injeção de dependência
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();

// Adicionar CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy",
        builder => builder
            .AllowAnyOrigin() // Permite requisições de qualquer origem
            .AllowAnyMethod() // Permite todos os métodos HTTP
            .AllowAnyHeader() // Permite todos os cabeçalhos
    );
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();
app.UseAuthorization();

// Habilitar CORS no pipeline
app.UseCors("CorsPolicy");

app.MapControllers();

app.Run();

