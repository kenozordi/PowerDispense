using PowerDispense.Factories;
using PowerDispense.Interfaces.IFactories;
using PowerDispense.Interfaces.IFactories.IRepoFactories;
using PowerDispense.Interfaces.IRepositories;
using PowerDispense.Interfaces.IServices;
using PowerDispense.Models.Config;
using PowerDispense.Repositories.Power;
using PowerDispense.Services;
using PowerDispense.Services.Workers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Register appsettings.json configurations
builder.Services.Configure<ConnectionStrings>(builder.Configuration.GetSection(nameof(ConnectionStrings)));

// Register DI services
builder.Services.AddSingleton<ConfigService>();
builder.Services.AddScoped<IMeterService, MeterService>();
builder.Services.AddScoped<IPowerProviderHealth, PowerProviderHealthService>();
builder.Services.AddScoped<IRaffleDrawService, RaffleDrawService>();

builder.Services.AddSingleton<IPowerServiceFactory, PowerServiceSwitch>();
builder.Services.AddSingleton<IPowerProviderFactory, ProviderFactory>();
builder.Services.AddSingleton<IPowerRepoFactory, PowerRepoFactory>();

builder.Services.AddScoped<AEDCService>();
builder.Services.AddScoped<EKEDCService>();

builder.Services.AddScoped<ITransactionProducer, TransactionProducer>();
builder.Services.AddSingleton<ITransactionConsumer, TransactionConsumer>();
builder.Services.AddSingleton<TransactionWorker>();

builder.Services.AddHostedService(provider => provider.GetRequiredService<TransactionWorker>());

builder.Services.AddScoped<RedisPowerRepo>();
builder.Services.AddScoped<FilePowerRepo>();


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

