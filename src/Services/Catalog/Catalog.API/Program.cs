// using Service.Common.Extensions;
//
// var builder = WebApplication.CreateBuilder(args);
//
// // Add services to the container.
// builder.AddServiceDefaults();
// builder.Services.AddControllers();
// // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
// //builder.Services.AddEndpointsApiExplorer();
// //builder.Services.AddSwaggerGen();
//
// var app = builder.Build();
//
// // Configure the HTTP request pipeline.
// //if (app.Environment.IsDevelopment())
// //{
// //    app.UseSwagger();
// //    app.UseSwaggerUI();
// //}
// app.UseServiceDefaults();
// app.UseHttpsRedirection();
//
// //app.UseAuthorization();
//
// app.MapControllers();
//
// app.Run();
using Service.Common.Extensions;
using Catalog.Infrastructure.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.AddServiceDefaults();
builder.Services.AddControllers();

var configuration = builder.Configuration;

builder.Services.AddInfrastructure(configuration);

var app = builder.Build();

app.UseServiceDefaults();
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

public partial class Program { }