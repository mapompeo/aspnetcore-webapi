using System;
using DotNetEnv;
using APICatalogo.Context;
using APICatalogo.Filters;
using APICatalogo.Logging;
using APICatalogo.Repositories;
using APICatalogo.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

Env.Load(); // carrega .env na raiz do repositório (não comitar .env)
Console.WriteLine($"[DEBUG] LOG_PATH => {Environment.GetEnvironmentVariable("LOG_PATH")}");

var builder = WebApplication.CreateBuilder(args);

// Configuração de logging customizado
builder.Logging.ClearProviders();
builder.Logging.AddProvider(new CustomLoggerProvider(
    new CustomLoggerProviderConfiguration
    {
        LogLevel = LogLevel.Information
    })
);

// Controllers + filtros + JSON
builder.Services.AddControllers(options =>
{
    options.Filters.Add(typeof(ApiExceptionFilter));
})
.AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
})
.AddNewtonsoftJson();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// String de conexão (prioriza variável de ambiente)
string? envConnection = Environment.GetEnvironmentVariable("ConnectionStrings__DefaultConnection");
string mySqlConnection = envConnection ?? builder.Configuration.GetConnectionString("DefaultConnection")!;

// Caminho de log
builder.Configuration["LogPath"] = Environment.GetEnvironmentVariable("LOG_PATH") ?? "APICatalogo/log/log.txt";

// Configuração do DbContext
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(mySqlConnection, ServerVersion.AutoDetect(mySqlConnection)));

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddTransient<IMeuServico, MeuServico>();

builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.DisableImplicitFromServicesParameters = true;
});

var app = builder.Build();

// Inicialização opcional do banco de dados
if (args.Contains("--init-db"))
{
    using var scope = app.Services.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    await DbInitializer.InitializeAsync(db);
    Console.WriteLine("Banco de dados inicializado e populado com sucesso!");
    return;
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.Run();
