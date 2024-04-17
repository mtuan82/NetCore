using Core.Configurations;
using Core.Providers.MSSQL;
using Core.Providers.PostgreSQL;
using Microsoft.EntityFrameworkCore;
using NetCoreWebAPI.Configurations;

var builder = WebApplication.CreateBuilder(args);

var env = new AppSettings(builder.Configuration);
env.SetupEnv();

ConfigureAuthentication.AddAuthentication(builder.Services, env.Identity);

//MongoDB Provider
ConfigureMongoDB.SetupDatabase(builder.Services, builder.Configuration);

//MYSQL Provider
ConfigureMySQL.SetupDatabase(builder.Services, env.MysqlDB);

//MSSQL Provider
builder.Services.AddDbContext<MSSQLProvider>(option =>
{
    option.UseLazyLoadingProxies()
            .UseSqlServer(env.MssqlDB.ConnectionString);
},ServiceLifetime.Scoped);

//PostgreSQL Provider
builder.Services.AddDbContext<PostgreSQLProvider>(option =>
{
    option.UseLazyLoadingProxies()
            .UseNpgsql(env.PostgresqlDB.ConnectionString);
}, ServiceLifetime.Singleton);

//Redis 
ConfigureRedis.RegisterRedis(builder.Services,env.Redis);

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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
