using EFCoreExample.Infrastructure;
using EFCoreExample.Middlewares;
using EFCoreExample.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//Ignore cycle references like Order -> OrderItems -> Order -> OrderItems...
builder.Services.AddControllers().AddJsonOptions(o =>
{
    o.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
}
    );
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


//Database Configurations


builder.Services.AddDbContext<AmazonDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("Default"),
        sql => sql.EnableRetryOnFailure());
});

builder.Services.AddScoped<IUnitOfWork, EfUnitOfWork>();
builder.Services.AddScoped<ItemService>();
builder.Services.AddScoped<OrderService>();


var app = builder.Build();

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

app.MapControllers();

app.Run();
