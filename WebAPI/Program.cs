using Application;
using Infrastructure;
using Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using System.Net;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services
    .AddInfrastructureServices(builder.Configuration)
    .AddApplicationServices();

builder.WebHost.ConfigureKestrel((context, serverOptions) =>
{
    serverOptions.Listen(IPAddress.Loopback, 8000);
    serverOptions.Listen(IPAddress.Loopback, 8001, listenOptions =>
    {
        listenOptions.UseHttps("aspnetcore.pfx", "testPassword");
    });
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp",
        builder =>
        {
            string react_url = Environment.GetEnvironmentVariable("REACT_URL") ?? "http://localhost:3000";
            string react_urls = Environment.GetEnvironmentVariable("REACT_URL_S") ?? "https://localhost:3000";
            builder.WithOrigins(react_url, react_urls)
                   .AllowAnyHeader()
                   .AllowAnyMethod()
                   .AllowCredentials();
        });
});
builder.Services.AddSwaggerGen();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
await MigrateAsync(app);
app.UseCors("AllowReactApp");
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.UseStaticFiles();
app.MapControllers();

app.Run();

static async Task MigrateAsync(WebApplication app)
{
    var scopeFactory = app.Services.GetRequiredService<IServiceScopeFactory>();
    var scope = scopeFactory.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    var result = await db.Database.GetPendingMigrationsAsync();
    if (result.Count() != 0)
    {
        await db.Database.MigrateAsync();
        await db.SaveChangesAsync();
    }
}