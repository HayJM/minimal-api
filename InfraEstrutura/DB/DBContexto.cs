
using Microsoft.EntityFrameworkCore;
using minimal_api.Dominio.Entidades;

namespace minimal_api.InfraEstrutura.DB;

public class DBContexto : DbContext
{
    private readonly IConfiguration _configurationAppSettings; 
    public DBContexto( IConfiguration configurationAppSettings)
    {
        _configurationAppSettings = configurationAppSettings;
    }
    
    public DbSet<Administrador> Administradores { get; set; } = default!;

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if(!optionsBuilder.IsConfigured)
        {
            var stringConexao = _configurationAppSettings.GetConnectionString("mysql");
            if (!string.IsNullOrEmpty(stringConexao))
            {
                optionsBuilder.UseMySql(stringConexao, ServerVersion.AutoDetect(stringConexao));
            }
        }
        

        
    } 
}
