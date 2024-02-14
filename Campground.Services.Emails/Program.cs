using Campground.Services.Emails.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<MessageReceiverService>();
builder.Services.AddSingleton<EmailService>();

builder.Services.AddRouting(x => x.LowercaseUrls = true);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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

var messageReceiverService = app.Services.GetRequiredService<MessageReceiverService>();
await messageReceiverService.RegisterMessageHandlerAsync();

app.Run();
