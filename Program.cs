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
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.ComponentModel.DataAnnotations;
using Microsoft.OpenApi.Models;
using System.Security.Cryptography.Xml;
using Microsoft.AspNetCore.Authorization;

#region Buider
var builder = WebApplication.CreateBuilder(args);

var key = builder.Configuration.GetSection("Jwt").ToString();
if (string.IsNullOrEmpty(key)) key = "123456";

builder.Services.AddAuthentication(option =>
{
    option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(option =>
{
    option.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateLifetime = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),

        ValidateIssuer = false,
        ValidateAudience = false,

    };
});
builder.Services.AddAuthorization();

builder.Services.AddScoped<IAdministradorServicos, AdministradorServicos>();
builder.Services.AddScoped<IVeiculosServicos, VeiculoServicos>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(opcions =>
{
    opcions.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Insira o token JWT aqui"
    });
    opcions.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }

            },
            new string[] { }
        }
    });

});

builder.Services.AddDbContext<DBContexto>(opitions =>
{
    opitions.UseMySql(builder.Configuration.GetConnectionString("MySql"),
        ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("MySql"))
    );
} 
);

var app = builder.Build();
#endregion

# region Home
app.MapGet("/", () =>  Results.Json(new Home())).AllowAnonymous().WithTags("Home");
#endregion

#region  Administradores

string GerarTokenJwt(Administrador administrador)
{
    if (string.IsNullOrEmpty(key))
        return string.Empty;
    var SecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
    var credenciais = new SigningCredentials(SecurityKey, SecurityAlgorithms.HmacSha256);
    var claims = new List<Claim>
    {
        new Claim("Email",administrador.Email),
        new Claim("Perfil", administrador.Perfil),
        new Claim(ClaimTypes.Role, administrador.Perfil),
    };
    var token = new JwtSecurityToken(
        claims: claims,
        expires: DateTime.Now.AddDays(1),
        signingCredentials: credenciais
    );   
    return new JwtSecurityTokenHandler().WriteToken(token);
}

app.MapPost("/Adminstradores/login", ([FromBody] LoginDTD loginDTD, IAdministradorServicos administradorServicos) =>
{
    var adm = administradorServicos.Login(loginDTD);
    if (adm != null)
    {
        string token = GerarTokenJwt(adm);
        return Results.Ok(new AdministradorLogado
        {
            Email =  adm.Email,
            Perfil = adm.Perfil,
            Token = token
        });
    }
    else
    {
        return Results.Unauthorized();
    }
}).AllowAnonymous().WithTags("Administradores");

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

})
.RequireAuthorization()
.RequireAuthorization(new AuthorizeAttribute{Roles = "Adm"})
.WithTags("Administradores");

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

})
.RequireAuthorization()
.RequireAuthorization(new AuthorizeAttribute{Roles = "Adm"})
.WithTags("Administradores");

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
  
})
.RequireAuthorization()
.RequireAuthorization(new AuthorizeAttribute{Roles = "Adm"})
.WithTags("Administradores");
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

}).RequireAuthorization()
.RequireAuthorization(new AuthorizeAttribute{Roles = "Adm, Editor"})
.WithTags("Veiculos");

app.MapGet("/veiculos", ([FromQuery] int? pagina, IVeiculosServicos veiculosServicos) =>
{
    var veiculo = veiculosServicos.Todos(pagina);
    return Results.Ok(veiculo);

}).RequireAuthorization().WithTags("Veiculos");

app.MapGet("/veiculos/{id}", ([FromRoute] int id, IVeiculosServicos veiculosServicos) =>
{
    var veiculo = veiculosServicos.BuscaPorId(id);
    if (veiculo == null)
    {
        return Results.NotFound();
    }
    return Results.Ok(veiculo);

}).RequireAuthorization()
.RequireAuthorization(new AuthorizeAttribute{Roles = "Adm, Editor"})
.WithTags("Veiculos");

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

}).RequireAuthorization()
.RequireAuthorization(new AuthorizeAttribute{Roles = "Adm"})
.WithTags("Veiculos");

app.MapDelete("/veiculos/{id}", ([FromRoute] int id, IVeiculosServicos veiculosServicos) =>
{
    var veiculo = veiculosServicos.BuscaPorId(id);
    if (veiculo == null)
    {
        return Results.NotFound();
    }
    veiculosServicos.Apagar(veiculo);
    return Results.NoContent();

}).RequireAuthorization()
.RequireAuthorization(new AuthorizeAttribute{Roles = "Adm"})
.WithTags("Veiculos");


#endregion

#region App
app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthentication();
app.UseAuthorization();

app.Run();
#endregion

