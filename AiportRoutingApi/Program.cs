var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Register DI Services
builder.Services.AddTransient<IAirportService, AirportService>();
builder.Services.AddTransient<IAirportApiClient, AirportApiClient>();

// Auto Mapper
builder.Services.AddAutoMapper(typeof(AirportMapper));

builder.Services.AddMemoryCache();

builder.Services.AddHttpClient<IAirportApiClient, AirportApiClient>(client =>
{
    client.BaseAddress = new Uri("https://api-cert.ezycommerce.sabre.com/api/v1/");
    client.DefaultRequestHeaders.Add("Accept", "text/plain");
    client.DefaultRequestHeaders.Add("Tenant-Identifier", "9d7d6eeb25cd6083e0df323a0fff258e59398a702fac09131275b6b1911e202d");
    client.DefaultRequestHeaders.Add("Channel", "web");
    client.DefaultRequestHeaders.Add("AppContext", "Ibe");
});

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .WriteTo.File(
        path: "Logs/log-.txt",
        rollingInterval: RollingInterval.Day,
        retainedFileCountLimit: 7,
        outputTemplate:
            "{Timestamp:yyyy-MM-dd HH:mm:ss} [{Level:u3}] {Message:lj}{NewLine}{Exception}")
    .CreateLogger();

builder.Host.UseSerilog();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseMiddleware<GlobalExceptionMiddleware>();

app.UseAuthorization();

app.MapControllers();

app.Run();