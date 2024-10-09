using FixedAssets.Application.Interfaces;
using FixedAssets.Application.Services;
using FixedAssets.Infrastructure.Interfaces;
using FixedAssets.Infrastructure.Persistence;
using FixedAssets.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Registrar o ApplicationDbContext com a string de conexão para SQL Server
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"),
    b => b.MigrationsAssembly("FixedAssets.Infrastructure")));

// Configuração explícita da URL
//builder.WebHost.UseUrls(builder.Configuration["ASPNETCORE_URLS"] ?? "http://+:8080");

// Adicionar CORS para permitir a comunicação entre frontend e backend
builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy",
        builder => builder
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader());
});

// Configurar a injeção de dependência para os serviços e repositórios
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IOrderItemService, OrderItemService>();
builder.Services.AddScoped<IOrderItemRepository, OrderItemRepository>();
builder.Services.AddScoped<IToroAccountRepository, ToroAccountRepository>();
builder.Services.AddScoped<IToroAccountService, ToroAccountService>();
builder.Services.AddScoped<IUserAssetRepository, UserAssetRepository>();
builder.Services.AddScoped<IMostTradedAssetService, MostTradedAssetService>();
builder.Services.AddScoped<IMostTradedAssetRepository, MostTradedAssetRepository>();


// Adicionar serviços de controle
builder.Services.AddControllers();

builder.Services.AddSwaggerGen(c =>
{
    c.EnableAnnotations();
});


var app = builder.Build();

// Configurações de pipeline de requisições HTTP
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.UseCors("CorsPolicy");

app.MapControllers();

app.Run();
