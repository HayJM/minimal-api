using minimal_api.InfraEstrutura.DB;
using minimal_api.Dominio.DTOs;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<DBContexto>(opitions =>
{
    opitions.UseMySql(builder.Configuration.GetConnectionString("mysql"),
        ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("mysql"))
    );
} 
);

var app = builder.Build();


app.MapGet("/", () => "Hello World!");

app.MapPost ("/login",(minimal_api.Dominio.DTOs.LoginDTD loginDTD) =>{
    if (loginDTD.Email == "adm@teste.com" && loginDTD.Senha == "123456")
    {
        return Results.Ok("Login com successo");
    }
    else
    {
        return Results.Unauthorized();
    }
});

app.Run();

