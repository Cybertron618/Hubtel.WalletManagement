using Microsoft.EntityFrameworkCore;
using Hubtel.WalletManagement.Api.Data;
using Microsoft.Extensions.DependencyInjection;
using Npgsql.EntityFrameworkCore.PostgreSQL;
using StackExchange.Redis;
using Hubtel.WalletManagement.Api.Services;
using Hubtel.WalletManagement.Api.Repositories;
using Hubtel.WalletManagement.Api.Interfaces;
using Hubtel.WalletManagement.Api.Operations;
using Microsoft.Extensions.Configuration;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<WalletDbContext>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

string redisConnectionString = builder.Configuration.GetConnectionString("Redis")
              ?? throw new InvalidOperationException("Redis connection string is missing or invalid.");

builder.Services.AddSingleton(ConnectionMultiplexer.Connect(redisConnectionString));

builder.Services.AddTransient<IWalletService, WalletService>();
builder.Services.AddTransient<IWalletOperations, WalletOperations>();
builder.Services.AddTransient<IWalletRepository, WalletRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
