using Microsoft.AspNetCore.RateLimiting;
using System.Threading.RateLimiting;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddRateLimiter(_ => _
    .AddFixedWindowLimiter(policyName: "RateLimiterFixed", options =>
    {
        options.PermitLimit = 4;
        options.Window = TimeSpan.FromSeconds(30);
        options.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
        options.QueueLimit = 2;
    }));

builder.Services.AddRateLimiter(_ => _
    .AddSlidingWindowLimiter(policyName: "RateLimiterSliding", options =>
    {
        options.PermitLimit = 3;
        options.Window = TimeSpan.FromSeconds(15);
        options.QueueProcessingOrder= QueueProcessingOrder.OldestFirst;
        options.SegmentsPerWindow = 3;        
        options.QueueLimit = 2;
    }));


var app = builder.Build();

app.UseRateLimiter();

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
