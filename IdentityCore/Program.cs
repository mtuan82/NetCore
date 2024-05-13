using IdentityCore.Configuration;
using IdentityCore.Identity;
using IdentityCore.MiddleWare;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Swashbuckle.AspNetCore.Filters;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

ConfigureEncryption.SetupEncryption(builder.Services, builder.Configuration);

var connectionString = builder.Configuration.GetConnectionString("IdentityCoreContextConnection") ?? throw new InvalidOperationException("Connection string 'IdentityCoreContextConnection' not found.");
// Add services to the container.
var policy = new CorsPolicyBuilder()
            .AllowAnyHeader()
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .Build();
builder.Services.AddCors(c => c.AddPolicy("Cor", policy));
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddDbContext<CoreContext>(options => options.UseMySQL(connectionString));

//Identity
builder.Services.AddIdentity<IdentityCoreUser, IdentityRole>()
                .AddEntityFrameworkStores<CoreContext>()
                .AddSignInManager()
                .AddRoles<IdentityRole>();

//Authentication JWT
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ClockSkew = TimeSpan.Zero,
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        LifetimeValidator = TokenValidators.LifetimeValidator,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["JWT:ValidIssuer"],
        ValidAudience = builder.Configuration["JWT:ValidAudience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:SigningKey"]!))
    };
});

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
if (app.Environment.IsDevelopment() || app.Environment.IsEnvironment("Docker"))
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("Cor");
app.UseHttpsRedirection();
app.UseMiddleware<Encryption>();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
