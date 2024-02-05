using ms_correo.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<MessageReceiverService>();
builder.Services.AddSingleton<EmailService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if(app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

// Obtén una instancia de MessageReceiverService e inicia el procesamiento de mensajes
var messageReceiverService = app.Services.GetRequiredService<MessageReceiverService>();
await messageReceiverService.RegisterMessageHandlerAsync();

app.Run();
