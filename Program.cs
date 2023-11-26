using RecurringBitcoinPurchaseInstructions.Data;
using RecurringBitcoinPurchaseInstructions.Services;
using RecurringBitcoinPurchaseInstructions.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<InstructionDbContext>();

builder.Services.AddScoped<IInstructionService, InstructionService>();
builder.Services.AddScoped<INotificationService, NotificationService>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<InstructionDbContext>();
    dbContext.Database.EnsureCreated();

    dbContext.Users.ToList().ForEach(u =>
    {
        Console.WriteLine($"{u.Id} - {u.Username}");
    });
}

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
