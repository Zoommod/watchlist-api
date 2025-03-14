using Microsoft.EntityFrameworkCore;
using Watchlist.Data;

var builder = WebApplication.CreateBuilder(args);

// Adiciona suporte a controladores
builder.Services.AddControllers();

// Configura o Swagger
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "Watchlist API", Version = "v1" });
});

// Configura o banco de dados SQLite
builder.Services.AddDbContext<WatchlistContext>(options =>
    options.UseSqlite("Data Source=watchlist.db"));

var app = builder.Build();

// Configura o pipeline de requisições HTTP
if (app.Environment.IsDevelopment())
{
    // Habilita o Swagger apenas em desenvolvimento
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Watchlist API v1"));
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();