var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.MapGet("/", (HttpContext context) =>
{
    var response = new
    {
        timestamp = DateTime.Now.ToString("O"),
        ip = context.Connection.RemoteIpAddress?.ToString() ?? "Unknown"
    };
    return response;
});

app.Run();
