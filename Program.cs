using Five9.CodingAssessment.Data;
using Five9.CodingAssessment.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

//builder.Services.AddDbContext<CallCenterContext>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("PostgresConnection")));

var dbProvider = builder.Configuration.GetSection("DatabaseSettings:DbProvider").Value;

if (dbProvider == "PostgreSql")
{
    // Register PostgreSQL services
    builder.Services.AddDbContext<CallCenterContext>(options =>
        options.UseNpgsql(builder.Configuration.GetConnectionString("PostgresConnection")));
    builder.Services.AddScoped<IAgentRepository, PostgreSqlAgentRepository>();
}
else if (dbProvider == "MongoDb")
{
    // Register MongoDB services
    builder.Services.AddSingleton<IMongoClient>(s =>
        new MongoClient(builder.Configuration.GetConnectionString("MongoDbConnection")));

    builder.Services.AddScoped<IMongoDatabase>(s =>
        s.GetRequiredService<IMongoClient>().GetDatabase(builder.Configuration.GetSection("ConnectionStrings:MongoDbName").Value));

    builder.Services.AddScoped<IAgentRepository, MongoDbAgentRepository>();
}

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
