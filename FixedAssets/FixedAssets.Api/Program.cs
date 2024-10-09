using FixedAssets.Application.Interfaces;
using FixedAssets.Application.Services;
using FixedAssets.Infrastructure.Interfaces;
using FixedAssets.Infrastructure.Persistence;
using FixedAssets.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Registrar o ApplicationDbContext com a string de conex�o para SQL Server
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"),
    b => b.MigrationsAssembly("FixedAssets.Infrastructure")));

// Configura��o expl�cita da URL
//builder.WebHost.UseUrls(builder.Configuration["ASPNETCORE_URLS"] ?? "http://+:8080");

// Adicionar CORS para permitir a comunica��o entre frontend e backend
builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy",
        builder => builder
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader());
});

// Configurar a inje��o de depend�ncia para os servi�os e reposit�rios
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


// Adicionar servi�os de controle
builder.Services.AddControllers();

builder.Services.AddSwaggerGen(c =>
{
    c.EnableAnnotations();
});


var app = builder.Build();

// Configura��es de pipeline de requisi��es HTTP
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.UseCors("CorsPolicy");

app.MapControllers();

app.Run();
