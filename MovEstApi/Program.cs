using Infrastructure;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.OpenApi;
using MovEstApi.Controllers;
using MovEstApi.Middleware;
using Service;

var builder = WebApplication.CreateBuilder(args);


if(builder.Environment.IsDevelopment()){
    builder.Services.AddNpgsqlDataSource(Utilities.FormttedConnectionString(
        builder.Configuration["pgconnection"] ?? 
        "postgres://admin:admin@localhost:5431/stock_api"),
        dataSourceBuilder => dataSourceBuilder.EnableParameterLogging());
}

if(builder.Environment.IsProduction()){
    builder.Services.AddNpgsqlDataSource(Utilities.FormttedConnectionString(
        builder.Configuration["pgconnection"] ?? 
        "postgres://admin:admin@localhost:5431/stock_api"));
}

builder.Services.AddSingleton<ProductRepository>();
builder.Services.AddSingleton<ProductService>();
builder.Services.AddSingleton<MovementRepository>();
builder.Services.AddSingleton<MovementService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(options => {
    options.SetIsOriginAllowed(_ => true)
        .AllowAnyMethod()
        .AllowAnyHeader()
        .AllowCredentials();
});

app.MapControllers();
app.UseMiddleware<GlobalExceptionHandler>();
app.Run();
