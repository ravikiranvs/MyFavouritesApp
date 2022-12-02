var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

// Serving static files from wwwroot
app.UseStaticFiles();

app.UseAuthorization();

app.MapControllers();

app.Run();

