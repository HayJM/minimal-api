using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using minimal_api.Dominio.Servicos;
using Microsoft.AspNetCore.Hosting;
using minimal_api.Dominio.Interfaces;
using Test.Mocks;
using minimal_api.InfraEstrutura.DB;







namespace Test.Helpers
{
    public class Setup
    {
        public const string Port = "5001";
        public static TestContext testContext = default!;
        public static WebApplicationFactory<Startup> http = default!;
        public static HttpClient client = default!;

        public static void ClassInicial(TestContext testContext)
        {
            Setup.testContext = testContext;
            Setup.http = new WebApplicationFactory<Startup>();

            Setup.http = Setup.http.WithWebHostBuilder(builder =>
            {
                builder.UseSetting("http_port", Setup.Port).UseEnvironment("Testing");
                builder.ConfigureServices(static services =>
                {
                    //services.AddScoped<IBancoDeDadosServico<Cliente>, ClientesServicosMock>();
                    services.AddScoped<IAdministradorServicos, AdministradorServicosMock>();                 
                });

            });
            Setup.client = Setup.http.CreateClient();
        }
        public static void ClassCleanup()
        {
            Setup.client.Dispose();
        }
        
    }
}