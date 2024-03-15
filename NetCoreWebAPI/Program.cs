using DataCore.Configurations;
using DataCore.Providers.MSSQL;
using DataCore.Providers.PostgreSQL;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Web;
using NetCoreWebAPI.Configurations;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddMicrosoftIdentityWebApi(builder.Configuration.GetSection("AzureAd"));

var env = new AppSettings(builder.Configuration);
env.SetupEnv();

//MongoDB Provider
ConfigureMongoDB.SetupDatabase(builder.Services, env.MongoDB);

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
