using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using TaskManagementAPI.Data;
using TaskManagementAPI.Services;
using TaskManagementAPI.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Configure Swagger
builder.Services.AddSwaggerGen(c =>
{
    // Define Swagger document and metadata
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Task Management API",
        Version = "v1",
        Description = "A simple API for managing tasks and user assignments",
        
    });

    // Add JWT Bearer token security to Swagger
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        Description = "Enter 'Bearer' followed by a space and then your JWT token."
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] { }
        }
    });
});

// Configure In-Memory Database for testing (or configure SQL Server as needed)
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseInMemoryDatabase("TaskDb"));

// Configure Authentication and JWT Bearer
var configuration = builder.Configuration;
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,

        ValidIssuer = configuration["Jwt:Issuer"],
        ValidAudience = configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]))
    };
});

// Add Authorization
builder.Services.AddAuthorization();

// Add the AuthService to handle authentication logic
builder.Services.AddScoped<AuthService>();

// CORS setup (optional, if frontend app needs to communicate with the API)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});

var app = builder.Build();

// Ensure the database is created and seed it with sample data
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.EnsureCreated(); // Ensures the database is created if not already present

    // Seeding sample task and comment data
    var task1 = new TaskItem { Title = "Sample Task 1", UserId = 1 };
    var task2 = new TaskItem { Title = "Sample Task 2", UserId = 2 };
    db.Tasks.Add(task1);
    db.Tasks.Add(task2);
    db.SaveChanges();

    db.TaskComments.Add(new TaskComment
    {
        Content = "Comment on Task 1",
        TaskItemId = task1.Id,
        UserId = 2
    });
    db.TaskComments.Add(new TaskComment
    {
        Content = "Comment on Task 2",
        TaskItemId = task2.Id,
        UserId = 1
    });
    db.SaveChanges();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();  // Enable Swagger
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Task Management API V1");  // API version
        c.RoutePrefix = string.Empty;  // Serve Swagger UI at the app's root (optional)
    });
}

app.UseHttpsRedirection();

// Enable CORS (if needed for cross-origin requests from frontend apps)
app.UseCors("AllowAll");

// Authentication & Authorization middleware
app.UseAuthentication();  // JWT token authentication
app.UseAuthorization();   // Authorization middleware

// Map controllers (APIs)
app.MapControllers();

app.Run();
