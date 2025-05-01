var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Enable CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowExternalApp",
        builder => builder.WithOrigins("https://localhost:7236") // Replace with the URL of your MVC app
                          .AllowAnyMethod()
                          .AllowAnyHeader());
});

// Swagger setup
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Use Swagger in Development environment
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();

// Use CORS policy
app.UseCors("AllowExternalApp");

app.MapControllers();

app.Run();
