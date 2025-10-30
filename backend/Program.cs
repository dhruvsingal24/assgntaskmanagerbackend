using WebApplication1.Services;

var builder = WebApplication.CreateBuilder(args);

// Configure to listen on Render's PORT
var port = Environment.GetEnvironmentVariable("PORT") ?? "5000";
builder.WebHost.UseUrls($"http://0.0.0.0:{port}");

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Register TaskService as Singleton (in-memory storage)
builder.Services.AddSingleton<TaskService>();

// Add CORS policy for frontend
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowAll");

// Remove HTTPS redirection for Render (Render handles SSL)
// app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();