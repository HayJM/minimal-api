using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using minimal_api.Dominio.DTOs;
using Test.Helpers;
using minimal_api.Dominio.Entidades;
using minimal_api.Dominio.ModelViews;

namespace Test.Requests;

[TestClass]
public class AdministradorRequestTest
{
    [ClassInitialize]
    public static void ClassInicial(TestContext testContext)
    {
        Setup.ClassInicial(testContext);
    }
    [ClassCleanup]
    public static void ClassCleanup()
    {
        // Cleanup resources after tests
        Setup.ClassCleanup();
    }

    [TestMethod]    
    public async Task TestGetPropriedades()
    {
        // Arrange
        var loginDTD = new LoginDTD
        {
            Email = "adm@teste.com",
            Senha = "123456"
        };

        var content = new StringContent(JsonSerializer.Serialize(loginDTD), Encoding.UTF8, "application/json");


        //Act

        var response = await Setup.client.PostAsync("/Administradores/login", content);

        // Assert

        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

        var resut = await response.Content.ReadAsStringAsync();
        var admLogado = JsonSerializer.Deserialize<AdministradorLogado>(resut, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        Assert.IsNotNull(admLogado?.Email ?? "");
        Assert.IsNotNull(admLogado?.Token ?? "");
        Assert.IsNotNull(admLogado?.Perfil ?? "");       

    }

}
