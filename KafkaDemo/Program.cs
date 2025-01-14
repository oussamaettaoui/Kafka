using Confluent.Kafka;
using Shared;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var producerConfig = new ProducerConfig
{
    BootstrapServers = $"localhost:{Helper.GetKafkaBrokerPort(
        Directory.GetParent(Environment.CurrentDirectory)?.FullName!)}",
    ClientId = "order-producer"
};
builder.Services.AddSingleton(
    new ProducerBuilder<string, string>(producerConfig).Build());
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
