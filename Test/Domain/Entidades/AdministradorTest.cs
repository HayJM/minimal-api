using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using minimal_api.Dominio.Entidades;

namespace Test.Domain
{
    public class AdministradorTest
    {
        [TestMethod]
        public void TestGetPropriedades()
        {
            // Arrange
            var adm = new Administrador();

            //Act
            adm.Id = 1;
            adm.Email = "test@teste.com";
            adm.Senha = "senha123";
            adm.Perfil = "Admin";

            // Assert
            Assert.AreEqual(1, adm.Id);
            Assert.AreEqual("test@teste.com", adm.Email);
            Assert.AreEqual("senha123", adm.Senha);
            Assert.AreEqual("Admin", adm.Perfil);
        }
        
    }
}