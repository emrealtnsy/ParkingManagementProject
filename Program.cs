using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.OpenApi.Models;
using ParkingManagement.Application.Helpers;
using ParkingManagement.Application.Interfaces;
using ParkingManagement.Infrastructure.Data;
using ParkingManagement.Infrastructure.Repositories;
using ParkingManagement.Infrastructure.UnitOfWorks;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ApplicationDbContext>(options =>
                                                            options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddOpenApi();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddScoped<ApplicationDbContext>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<IParkingHelper, ParkingHelper>();

builder.Services.AddMediatR(typeof(Program).Assembly);

builder.Services.AddSwaggerGen(c => { c.SwaggerDoc("v1", new OpenApiInfo { Title = "Parking API", Version = "v1" }); });

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

using var scope = app.Services.CreateScope();
var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

var databaseCreator = context.GetService<IRelationalDatabaseCreator>();
var isDatabaseExists = await databaseCreator.ExistsAsync();
if (!isDatabaseExists)
{
    await databaseCreator.CreateAsync();
}

var pendingMigrations = await context.Database.GetPendingMigrationsAsync();
if (pendingMigrations.Any())
{
    await context.Database.MigrateAsync();
}

SeedData.Initialize(context);

app.Run();
