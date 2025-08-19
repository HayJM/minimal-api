using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using minimal_api.Dominio.Entidades;
using minimal_api.InfraEstrutura.DB;
using minimal_api.Dominio.Servicos;
using System.Reflection;
namespace Test.Domain.Servicos;

[TestClass]
public class AdministradorServico
{

    private DBContexto CriarContextoTeste()
    {
        var pathAssembly = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        var path = Path.GetFullPath(Path.Combine(pathAssembly ?? "", "..", "..", ".."));
        // configurar o CofigurationBuilder 
        var builder = new ConfigurationBuilder()
                        .SetBasePath(path ?? Directory.GetCurrentDirectory())
                        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                        .AddEnvironmentVariables();

        var configuration = builder.Build();

        return new DBContexto(configuration);
    }
    [TestMethod]
    public void TestandoSalvarAdministrador()
    {

        // Arrange
        var context = CriarContextoTeste();
        context.Database.ExecuteSqlRaw("TRUNCATE TABLE Administradores");

        var adm = new Administrador();
        adm.Id = 1;
        adm.Email = "test@teste.com";
        adm.Senha = "senha123";
        adm.Perfil = "Admin";
        var administradorServico = new AdministradorServicos(context);
        //Act
        administradorServico.Incluir(adm);
        
        // Assert
        Assert.AreEqual(1,administradorServico.Todos(1).Count());
    }
    [TestMethod]
    public void TestandoBuscandoPorId()
    {

        // Arrange
        var context = CriarContextoTeste();
        context.Database.ExecuteSqlRaw("TRUNCATE TABLE Administradores");

        var adm = new Administrador();
        adm.Id = 1;
        adm.Email = "test@teste.com";
        adm.Senha = "senha123";
        adm.Perfil = "Admin";
        var administradorServico = new AdministradorServicos(context);
        //Act
        administradorServico.Incluir(adm);
        var admDoBanco = administradorServico.BuscaPorId(adm.Id);
        // Assert
        Assert.AreEqual(1, admDoBanco?.Id);
    }
    

}