using minimal_api.InfraEstrutura.DB;
using minimal_api.Dominio.DTOs;
using minimal_api.Dominio.ModelViews;
using Microsoft.EntityFrameworkCore;
using minimal_api.Dominio.Entidades;
using System.Numerics;
using minimal_api.Dominio.Servicos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using minimal_api.Dominio.Enuns;

#region Buider
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IAdministradorServicos, AdministradorServicos>();
builder.Services.AddScoped<IVeiculosServicos, VeiculoServicos>();

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
#endregion

# region Home
app.MapGet("/", () =>  Results.Json(new Home())).WithTags("Home");
#endregion

#region  Administradores
app.MapPost("/Adminstradores/login", ([FromBody] LoginDTD loginDTD, IAdministradorServicos administradorServicos) =>
{
    if (administradorServicos.Login(loginDTD) != null)
    {
        return Results.Ok("Login com successo");
    }
    else
    {
        return Results.Unauthorized();
    }
}).WithTags("Administradores");

app.MapGet("/Adminstradores", ([FromQuery] int? pagina, IAdministradorServicos administradorServicos) =>
{
    var adms = new List<AdministradorModelView>();
    var administradores = administradorServicos.Todos(pagina);
    foreach (var adm in administradores)
    {
        adms.Add(new AdministradorModelView
        {
            Id = adm.Id,
            Email = adm.Email,
            Perfil = adm.Perfil
        }); 
        
    }

    return Results.Ok(adms);

}).WithTags("Administradores");

app.MapGet("/Administradores/{id}", ([FromRoute] int id, IAdministradorServicos administradorServicos) =>
{
    var administrador = administradorServicos.BuscaPorId(id);
    if (administrador == null)
    {
        return Results.NotFound();
    }
    return Results.Ok(new AdministradorModelView
        {
            Id = administrador.Id,
            Email = administrador.Email,
            Perfil = administrador.Perfil
        });

}).WithTags("Administradores");

app.MapPost ("/Adminstradores",([FromBody] AdministradorDTO administradorDTO, IAdministradorServicos administradorServicos) =>
{
    var validacao = new ErroDeValidacao
    {
        Menssagens = new List<string>()
    };

    if (string.IsNullOrEmpty(administradorDTO.Email))
        validacao.Menssagens.Add("O campo email não pode ser vasio.");
    if (string.IsNullOrEmpty(administradorDTO.Senha))
        validacao.Menssagens.Add("O campo Senha não pode estar vasio.");
    if (administradorDTO.Perfil == null)
        validacao.Menssagens.Add("O campo Perfil não pode ser vasio.");

    if (validacao.Menssagens.Count > 0)
        return Results.BadRequest(validacao);

    var administrador = new Administrador
    {
        Email = administradorDTO.Email,
        Senha = administradorDTO.Senha,
        Perfil = administradorDTO.Perfil.ToString() ?? Perfil.Editor.ToString()
    };
    administradorServicos.Incluir(administrador);

    return Results.Created($"/Adminstradores/{administrador.Id}",new AdministradorModelView
        {
            Id = administrador.Id,
            Email = administrador.Email,
            Perfil = administrador.Perfil
        });
  
}).WithTags("Administradores");
#endregion

#region Veiculos
ErroDeValidacao validaDTO(VeiculosDTO veiculosDTO)
{
    var validacao = new ErroDeValidacao
    {
        Menssagens = new List<string>()
    };

    if (string.IsNullOrEmpty(veiculosDTO.Nome))
    { validacao.Menssagens.Add("O nome do veiculo é obrigatório."); }
    if (string.IsNullOrEmpty(veiculosDTO.Marca))
    { validacao.Menssagens.Add("A marca do veiculo é obrigatório."); }
    if (veiculosDTO.Ano < 1950)
    { validacao.Menssagens.Add("O veiculo é muito antigo, aceito somente anos superiores a 1950"); }

    return validacao;    
}

app.MapPost("/veiculos", ([FromBody] VeiculosDTO veiculosDTD, IVeiculosServicos veiculosServicos) =>
{
    var validacao = validaDTO(veiculosDTD);
    if (validacao.Menssagens.Count > 0)
    {
        return Results.BadRequest(validacao);
    }
    
    var veiculo = new Veiculo
    {
        Nome = veiculosDTD.Nome,
        Marca = veiculosDTD.Marca,
        Ano = veiculosDTD.Ano
    };
    veiculosServicos.Incluir(veiculo);
    return Results.Created($"/veiculos/{veiculo.Id}", veiculo);

}).WithTags("Veiculos");

app.MapGet("/veiculos", ([FromQuery] int? pagina, IVeiculosServicos veiculosServicos) =>
{
    var veiculo = veiculosServicos.Todos(pagina);
    return Results.Ok(veiculo);

}).WithTags("Veiculos");

app.MapGet("/veiculos/{id}", ([FromRoute] int id, IVeiculosServicos veiculosServicos) =>
{
    var veiculo = veiculosServicos.BuscaPorId(id);
    if (veiculo == null)
    {
        return Results.NotFound();
    }
    return Results.Ok(veiculo);

}).WithTags("Veiculos");

app.MapPut("/veiculos/{id}", ([FromRoute] int id, VeiculosDTO veiculoDTO, IVeiculosServicos veiculosServicos) =>
{
    
    var veiculo = veiculosServicos.BuscaPorId(id);
    if (veiculo == null) {return Results.NotFound();}
    var validacao = validaDTO(veiculoDTO);
    if (validacao.Menssagens.Count > 0)
    {
        return Results.BadRequest(validacao);
    }
    veiculo.Nome = veiculoDTO.Nome;
    veiculo.Marca = veiculoDTO.Marca;
    veiculo.Ano = veiculoDTO.Ano;

    veiculosServicos.Atualizar(veiculo);
    return Results.Ok(veiculo);

}).WithTags("Veiculos");

app.MapDelete("/veiculos/{id}", ([FromRoute] int id, IVeiculosServicos veiculosServicos) =>
{
    var veiculo = veiculosServicos.BuscaPorId(id);
    if (veiculo == null)
    {
        return Results.NotFound();
    }
    veiculosServicos.Apagar(veiculo);
    return Results.NoContent();

}).WithTags("Veiculos");


#endregion

#region App
app.UseSwagger();
app.UseSwaggerUI();

app.Run();
#endregion

