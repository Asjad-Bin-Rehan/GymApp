using CSAPI.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add CORS policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend",
        policy =>
        {
            policy.WithOrigins("http://localhost:5173")
                  .AllowAnyHeader()
                  .AllowAnyMethod()
                  .AllowCredentials();
        });
});

// Register all services including JWT Authentication & Authorization
builder.Services.RegisterService(builder.Configuration);

var app = builder.Build();

// Apply CORS policy
app.UseCors("AllowFrontend");

// Configure middleware pipeline (includes Authentication & Authorization)
await app.Configure();

app.Run();
