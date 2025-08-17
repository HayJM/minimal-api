using minimal_api.Dominio.Entidades;
using minimal_api.Dominio.DTOs;

namespace minimal_api.Dominio.Servicos
{
    public interface IAdministradorServicos
    {
        
        Administrador? Login(LoginDTD loginDTD);
        Administrador Incluir(Administrador administrador);
        Administrador? BuscaPorId(int id);
        List<Administrador> Todos(int? pagina);
    }
}