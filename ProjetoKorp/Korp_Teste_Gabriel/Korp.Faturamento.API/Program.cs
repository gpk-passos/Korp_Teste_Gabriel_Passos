using Korp.Faturamento.API.Data;
using Korp.Faturamento.API.Models;
using Korp.Faturamento.API.Repositorios;
using Korp.Faturamento.API.Repositorios.Interfaces;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite("Data Source=faturamento.db"));

builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();

builder.Services.AddHttpClient();

builder.Services.AddScoped<INotaFiscalRepositorio, NotaFiscalRepositorio>();

builder.Services.AddCors(options => {
    options.AddPolicy("AllowAll", policy => {
        policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
    });
});




var app = builder.Build();

//app.UseSwagger();
//app.UseSwaggerUI();

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.UseCors("AllowAll");

app.Run();