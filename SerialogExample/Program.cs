using Microsoft.OpenApi.Models;
using SerialogExample.Configuration;
using SerialogExample.Models;
using SerialogExample.Models.Exceptions;
using SerialogExample.Repositories;
using SerialogExample.Repositories.Interfaces;
using SerialogExample.Services;
using SerialogExample.Services.Interfaces;
using Serilog;
using Serilog.Context;
using Serilog.Events;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { 
        Title = "Users API", 
        Version = "v1",
        Description = "API para cadastro e consulta de usuários"
    });
});

LoggerConfig.ConfigureLogging(builder);

builder.Host.UseSerilog();

// Registro dos serviços
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Users API V1");
});

app.Use(async (context, next) =>
{
    var correlationId = context.Request.Headers["X-Correlation-ID"].FirstOrDefault() ?? Guid.NewGuid().ToString();
    using (LogContext.PushProperty("CorrelationId", correlationId))
    {
        context.Response.Headers["X-Correlation-ID"] = correlationId;
        await next();
    }
});

app.MapPost("/users", async (IUserService userService, User user) =>
    {
        try
        {
            Log.Information("Iniciando cadastro de usuário {@User}", user);
            var result = await userService.CreateUser(user);
            return Results.Created($"/users/{result.Name}", result);
        }
        catch (ValidationException ex)
        {
            Log.Warning(ex, "Validação falhou para usuário: {@User}", user);
            return Results.BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Erro ao cadastrar usuário: {@User}", user);
            return Results.StatusCode(500);
        }
    })
    .WithName("CreateUser")
        .WithDescription("Cadastra um novo usuário com nome e idade")
        .WithSummary("Criar novo usuário");

app.MapGet("/users", async (IUserService userService) =>
    {
        try
        {
            Log.Information("Consultando lista de usuários");
            var users = await userService.GetAllUsers();
            return Results.Ok(users);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Erro ao consultar usuários");
            return Results.StatusCode(500);
        }
    })
    .WithName("GetUsers");
    

// Middleware de logging para requisições HTTP
app.Use(async (context, next) =>
{
    var sw = System.Diagnostics.Stopwatch.StartNew();
    
    Log.Information("Iniciando requisição HTTP {Method} {Path}", 
        context.Request.Method, 
        context.Request.Path);

    await next();
    
    sw.Stop();
    
    Log.Information("Finalizando requisição HTTP {Method} {Path} - Status: {StatusCode} - Tempo: {ElapsedMs}ms",
        context.Request.Method,
        context.Request.Path,
        context.Response.StatusCode,
        sw.ElapsedMilliseconds);
});

app.Run();