using minimal_api.Dominio.Entidades;
using minimal_api.Dominio.DTOs;

namespace minimal_api.Dominio.Servicos
{
    public interface IAdministradorServicos
    {
        Administrador? Login(LoginDTD loginDTD);
    }
}