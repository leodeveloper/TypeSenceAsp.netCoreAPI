using System.Configuration;
using TypeSenceApi.HraOpsApi.DapperUnitOfWork;
using TypeSenceApi.HraOpsApi.Model;
using TypeSenceApi.HraOpsApi.Repository;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings"));
string HcConnectionString = builder.Configuration.GetConnectionString("HCDatabase");

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddTransient<ITypeSenseRepository, TypeSenseRepository>();
builder.Services.AddTransient<IUnitOfWork, UnitOfWork>(service => new UnitOfWork(HcConnectionString));
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
