using EWasteManagement.API.Data;
using EWasteManagement.API.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using EWasteManagement.API.SeedData;


var builder = WebApplication.CreateBuilder(args);

// Add Controllers
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = null;
    });

// Add Swagger with better documentation
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "E-Waste Management API",
        Version = "v1",
        Description = "API for managing e-waste collection centers",
        Contact = new OpenApiContact
        {
            Name = "Admin Team",
            Email = "admin@ewaste.com"
        }
    });
});

// PostgreSQL Connection
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("DefaultConnection")
    )
);

// Register Services - Dependency Injection
builder.Services.AddScoped<ICenterService, CenterService>();

// CORS for React frontend
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend",
        policy =>
        {
            policy
                .AllowAnyOrigin()
                .AllowAnyHeader()
                .AllowAnyMethod();
        });
});

var app = builder.Build();

// Swagger
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "E-Waste API v1");
        c.RoutePrefix = "swagger";
    });
}

app.UseCors("AllowFrontend");

app.UseHttpsRedirection();

app.MapControllers();

// Ensure database is created on startup
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    dbContext.Database.EnsureCreated();
}

 await DbSeeder.SeedAsync(app.Services);

app.Run();