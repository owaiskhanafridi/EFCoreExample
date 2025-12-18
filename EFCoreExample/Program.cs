using EFCoreExample.Infrastructure;
using EFCoreExample.Middlewares;
using EFCoreExample.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Hybrid;
using Serilog;
using System.Reflection;
using System.Threading.RateLimiting;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//Ignore cycle references like Order -> OrderItems -> Order -> OrderItems...
builder.Services.AddControllers().AddJsonOptions(o =>
{
    o.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
}
    );

builder.Services.AddOutputCache(options =>
{
    options.DefaultExpirationTimeSpan = TimeSpan.FromSeconds(10);
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath, includeControllerXmlComments: true);
});


//Database Configurations


builder.Services.AddDbContext<AmazonDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("Default"),
        sql => sql.EnableRetryOnFailure());
});

builder.Services.AddScoped<IUnitOfWork, EfUnitOfWork>();
builder.Services.AddScoped<ItemService>();
builder.Services.AddScoped<OrderService>();

builder.Services.AddRateLimiter(options =>
{
    options.AddFixedWindowLimiter("fixed", o =>
    {
        o.QueueLimit = 0; // no queuing; reject when limit reached
        o.PermitLimit = 2; // limits 2 requests
        o.Window = TimeSpan.FromSeconds(5); // limits 2 requests in 5 second window
        o.QueueProcessingOrder = QueueProcessingOrder.OldestFirst; // In case of throtling, some request may still is saved in the queue to be processed
        //later. The QueueProcessingOrder decides the que discipline. In this case, it's first-come, first-served.
    });

    // Optional: status code when limited
    options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;

});

//builder.Services.AddApiVersioning(o =>
// {
//     o.ApiVersionReader = new HeaderApiVersionReader("RM-Api-Version");

//     //o.DefaultApiVersion = new ApiVersion(1, 0);
//     //o.AssumeDefaultVersionWhenUnspecified = true;   // ✅ prevents the error
//     o.ReportApiVersions = true;
// });

builder.Host.UseSerilog((ctx, services, lc) =>
    lc.ReadFrom.Configuration(ctx.Configuration)
      .ReadFrom.Services(services)
      .Enrich.FromLogContext()
      .WriteTo.Console()
);


var app = builder.Build();
Log.Information("Serilog is wired up");


app.UseMiddleware<ErrorHandlerMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment() ||
    builder.Configuration.GetValue<bool>("Swagger:Enable"))

{
    app.UseSwagger();
    app.UseSwaggerUI();
}

Console.WriteLine("Application Started");
Console.WriteLine("Application Running");


app.UseHttpsRedirection();

app.UseAuthorization();

app.UseOutputCache();
app.UseRateLimiter();
app.UseSerilogRequestLogging();

//Use the following in place of app.UseSerilogRequestLogging() to use without adding Serilog
//app.Use(async (ctx, next) =>
//{
//    var path = ctx.Request.Path;
//    var method = ctx.Request.Method;

//    await next();

//    var status = ctx.Response.StatusCode;
//    ctx.RequestServices.GetRequiredService<ILoggerFactory>()
//        .CreateLogger("Http")
//        .LogInformation("HTTP {Method} {Path} -> {Status}", method, path, status);
//});


app.MapControllers();

app.Run();
