using minimal_api.Dominio.Entidades;
using minimal_api.Dominio.DTOs;

namespace minimal_api.Dominio.Servicos
{
    public interface IVeiculosServicos
    {
        List<Veiculo> Todos(int? page = 1, string? Nome = null, string? Marca = null);

        Veiculo? BuscaPorId(int id);

        void Incluir(Veiculo veiculo);
        void Atualizar(Veiculo veiculo);

        void Apagar(Veiculo veiculo);
    }
}