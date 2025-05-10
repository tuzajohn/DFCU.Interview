using DFCU.Interview.API.Data;
using DFCU.Interview.API.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System.Text.Json;
using System.Threading.RateLimiting;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContextPool<PaymentDbContext>(options =>
{
    var sss = builder.Configuration.GetConnectionString(DConstants.ConnectionString);
    options.UseSqlServer(builder.Configuration.GetConnectionString(DConstants.ConnectionString), (e) =>
    {
        e.EnableRetryOnFailure(10, TimeSpan.FromSeconds(10), Enumerable.Range(6, 10));
        e.CommandTimeout(20);
    });

    options.EnableDetailedErrors();
    options.LogTo(Console.WriteLine, LogLevel.Information, DbContextLoggerOptions.DefaultWithLocalTime);
});

builder.Services.AddRateLimiter(options =>
{
    options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(httpContext =>
        RateLimitPartition.GetFixedWindowLimiter(
            partitionKey: httpContext.Connection.RemoteIpAddress?.ToString() ?? DConstants.LimiterKey,
            factory: partition => new FixedWindowRateLimiterOptions
            {
                AutoReplenishment = true,
                PermitLimit = DConstants.PermitLimit,
                QueueLimit = 0,
                Window = TimeSpan.FromSeconds(DConstants.WindowTime)
            }));

    options.OnRejected = async (context, cancellationToken) =>
    {
        context.HttpContext.Response.StatusCode = StatusCodes.Status429TooManyRequests;
        context.HttpContext.Response.Headers["Retry-After"] = $"{DConstants.WindowTime}";
        context.HttpContext.Response.ContentType = "application/json";

        await context.HttpContext.Response.WriteAsync(JsonSerializer.Serialize(new
        {
            code = 429,
            message = "Rate limit exceeded. Please try again later. after: " + DConstants.WindowTime + "s",
        }), cancellationToken);

    };
});


builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddLogging(logBuilder =>
{
    logBuilder.AddConsole();
    logBuilder.AddConfiguration(builder.Configuration.GetSection("Logging"));
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();


    using var scope = app.Services.CreateScope();
    var dbContext = scope.ServiceProvider.GetRequiredService<PaymentDbContext>();
    dbContext.Database.Migrate();

}

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseRateLimiter();

app.MapControllers();

app.Run();
