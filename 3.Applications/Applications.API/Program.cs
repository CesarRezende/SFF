using DryIoc.Microsoft.DependencyInjection;
using SFF.Infra.IoC;
using Serilog;
using SFF.Infra.Web.Startup;
using DryIoc;
//Não remover esse using
using Microsoft.Extensions.Logging;


var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((hostContext, loggerConfiguration) => loggerConfiguration.ReadFrom.Configuration(hostContext.Configuration, "Serilog"));

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddResponseCompression();

builder.Host.UseServiceProviderFactory(new DryIocServiceProviderFactory(ContainerManager.CreateContainer()));

var container = ContainerManager.GetContainer().AddDbConfigurations(builder.Configuration).AddDefaults();

//Config ILogger
var loggerConfiguration = new LoggerConfiguration().ReadFrom.Configuration(builder.Configuration);
container.UseInstance(loggerConfiguration);
var loggerFactoryMethod = typeof(LoggerConfiguration).GetMethod("CreateLogger");
container.Register(typeof(ILogger<>), made: Made.Of(
    req => loggerFactoryMethod,
    ServiceInfo.Of<LoggerConfiguration>()));

//builder.Services.AddLogging(loggingBuilder =>
//{
//    loggingBuilder.AddSerilog(logger);
//});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseResponseCompression();

app.UseHttpsRedirection();
app.UseHttpLogging();

app.UseAuthorization();

app.MapControllers();

app.Run();