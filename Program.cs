using minimal_api.InfraEstrutura.DB;
using minimal_api.Dominio.DTOs;
using minimal_api.Dominio.ModelViews;
using Microsoft.EntityFrameworkCore;
using System.Numerics;
using minimal_api.Dominio.Servicos;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IAdministradorServicos, AdministradorServicos>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<DBContexto>(opitions =>
{
    opitions.UseMySql(builder.Configuration.GetConnectionString("mysql"),
        ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("mysql"))
    );
} 
);

var app = builder.Build();


app.MapGet("/", () =>  Results.Json(new Home()));

app.MapPost ("/login",([FromBody]LoginDTD loginDTD, IAdministradorServicos administradorServicos) =>{
    if (administradorServicos.Login(loginDTD)!= null)
    {
        return Results.Ok("Login com successo");
    }
    else
    {
        return Results.Unauthorized();
    }
});

app.UseSwagger();
app.UseSwaggerUI();

app.Run();

