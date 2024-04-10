using System.Data;
using System;
using MySql.Data.MySqlClient;
using Microsoft.AspNetCore.Hosting.Server;
using Org.BouncyCastle.Bcpg;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Text;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddScoped<IUsuarioService, UsuarioService>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c => { 
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title="Sammi Games API",Version="v1"});
    var filePath = Path.Combine(System.AppContext.BaseDirectory, "TCC_SAMMI.Api.xml");
    c.IncludeXmlComments(filePath);
});
builder.Services.AddControllers();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(opt => {
        opt.SwaggerEndpoint("/swagger/v1/swagger.json", "Sammi Games");
    });
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();