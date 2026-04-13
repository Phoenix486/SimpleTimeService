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
    // Get the real visitor IP from X-Forwarded-For header (set by load balancer/proxy)
    // Fall back to RemoteIpAddress if header not present
    var ip = "Unknown";
    
    if (context.Request.Headers.TryGetValue("X-Forwarded-For", out var forwardedFor))
    {
        // X-Forwarded-For can contain multiple IPs, the first one is the original client
        ip = forwardedFor.ToString().Split(',')[0].Trim();
        // Remove port if included (e.g., "192.168.1.1:8080" -> "192.168.1.1")
        if (ip.Contains(':'))
        {
            ip = ip.Split(':')[0];
        }
    }
    else if (context.Request.Headers.TryGetValue("X-Real-IP", out var realIp))
    {
        ip = realIp.ToString();
    }
    else
    {
        ip = context.Connection.RemoteIpAddress?.ToString() ?? "Unknown";
    }
    
    var response = new
    {
        timestamp = DateTime.Now.ToString("O"),
        ip = ip
    };
    return response;
});

app.Run();
