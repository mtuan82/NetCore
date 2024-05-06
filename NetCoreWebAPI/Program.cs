using Core.Configurations;
using NetCoreWebAPI.Configurations;
using Swashbuckle.AspNetCore.Filters;

var builder = WebApplication.CreateBuilder(args);

var env = new AppSettings(builder.Configuration);
env.SetupEnv();

ConfigureAuthentication.AddAuthentication(builder.Services, env.Identity);

//MongoDB Provider
ConfigureMongoDB.SetupDatabase(builder.Services, builder.Configuration);

//MYSQL Provider
ConfigureMySQL.SetupDatabase(builder.Services, env.MysqlDB);

//MSSQL Provider
ConfigureMSSQL.SetupDatabase(builder.Services, env.MssqlDB);

//PostgreSQL Provider
ConfigurePostgreSQL.SetupDatabase(builder.Services, env.PostgresqlDB);

//Redis 
ConfigureRedis.RegisterRedis(builder.Services,env.Redis);

//RabbitMQ 
ConfigureRabbitMQ.SetupRabbitMQ(builder.Services, env.RabbitMQ);

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("oauth2", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey
    });
    options.OperationFilter<SecurityRequirementsOperationFilter>();
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
