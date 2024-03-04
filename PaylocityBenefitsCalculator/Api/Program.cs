using Api.BusinessLayer;
using Api.PayrollCalculator;
using Api.Repository;
using Microsoft.AspNetCore.Authentication.Certificate;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.EnableAnnotations();
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "Employee Benefit Cost Calculation Api",
        Description = "Api to support employee benefit cost calculations"
    });
});

builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
builder.Services.AddScoped<IPayrollRepository, PayrollRepository>();
builder.Services.AddScoped<IPayrollBusinessLayer, PayrollBusinessLayer>();


builder.Services.AddScoped<BasePayrollDeductionCalculator, AddOnBenfitDeductionCalculator>();
builder.Services.AddScoped<BasePayrollDeductionCalculator, DependentsBenfitDeductionCalculator>();
builder.Services.AddScoped<BasePayrollDeductionCalculator, ElderlyBenfitDeductionCalculator>();
builder.Services.AddScoped<BasePayrollEarningsCalculator, StandardEarningCalculator>();
builder.Services.AddScoped<IDependentsRepository, DependentsRepository>();
builder.Services.AddScoped<IEmployeeBusinessLayer, EmployeeBusinessLayer>();



var allowLocalhost = "allow localhost";
builder.Services.AddCors(options =>
{
    options.AddPolicy(allowLocalhost,
        policy => { policy.WithOrigins("http://localhost:3000", "http://localhost"); });
});

var app = builder.Build();

var loggerFactory = app.Services.GetService<ILoggerFactory>();
loggerFactory.AddFile(builder.Configuration["Logging:LogFilePath"].ToString());

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(allowLocalhost);

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
