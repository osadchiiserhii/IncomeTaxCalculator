using IncomeTaxCalculator.DAL;
using IncomeTaxCalculator.Domain.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.ConfigureDAL(builder.Configuration);
builder.Services.ConfigureDomain(builder.Configuration);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder.WithOrigins("http://localhost:4200")
               .AllowAnyHeader()
               .AllowAnyMethod();
    });
});

var app = builder.Build();

var serviceScope = app.Services.CreateScope();
var dataContext = serviceScope.ServiceProvider.GetService<DataContext>();
dataContext?.Database.EnsureCreated();

app.UseSwagger();
app.UseSwaggerUI();
app.UseCors();
app.MapControllers();
app.Run();