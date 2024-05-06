using Core.Configurations;
using HangfireJob.Configurations;

var builder = WebApplication.CreateBuilder(args);

var env = new AppSettings(builder.Configuration);
env.SetupEnv();

ConfigureAppService.AddAppServices(builder.Services);

ConfigureHangfire.AddHangfireServices(builder.Services, env.Redis);

var app = builder.Build();

app.UseHangfire(env.Hangfire);
app.UseRouting();

app.Run();

