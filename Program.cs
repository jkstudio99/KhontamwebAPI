using KhontamwebAPI;
using Microsoft.EntityFrameworkCore;
using KhontamwebAPI.Settings;
using KhontamwebAPI.Services;
using System.Reflection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.ComponentModel;
using KhontamwebAPI.Enums;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var services = builder.Services;
services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("DefaultConnection"))
    )
);
// add cross-origin resource sharing
services.AddCors(options =>
{
    options.AddPolicy("MyCors", config =>
    {
        config
        .WithOrigins(builder.Configuration.GetSection("AllowedOrigins")
        .Get<string[]>()!)
        .AllowAnyMethod().AllowAnyHeader()
        .AllowCredentials();

    });
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
services.AddEndpointsApiExplorer();
services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Khontamweb API", Version = "v1" });
    
    // เพิ่ม XML comments
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);
    
    // เพิ่ม enum descriptions
    c.SchemaFilter<EnumSchemaFilter>();
});

// Add Cloudinary configuration
services.Configure<CloudinarySettings>(
    builder.Configuration.GetSection("Cloudinary"));

// Register Cloudinary service
services.AddScoped<ICloudinaryService, CloudinaryService>();

// Add controllers
services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Use CORS policy
app.UseCors("MyCors");

// Map controllers
app.MapControllers();

app.Run();
