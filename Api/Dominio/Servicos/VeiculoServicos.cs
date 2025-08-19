using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using minimal_api.Dominio.DTOs;
using minimal_api.Dominio.Entidades;
using minimal_api.Dominio.Interfaces;
using minimal_api.InfraEstrutura.DB;

namespace minimal_api.Dominio.Servicos
{
    public class VeiculoServicos : IVeiculosServicos
    {
        private readonly DBContexto _Contexto;

        public VeiculoServicos(DBContexto contexto)
        {
            _Contexto = contexto;
        }
        public void Apagar(Veiculo veiculo)
        {
            _Contexto.Veiculos.Remove(veiculo);
            _Contexto.SaveChanges();
        }

        public void Atualizar(Veiculo veiculo)
        {
            _Contexto.Veiculos.Update(veiculo);
            _Contexto.SaveChanges();
        }

        public Veiculo? BuscaPorId(int id)
        {
            return _Contexto.Veiculos.Where(v => v.Id == id).FirstOrDefault();
        }


        public void Incluir(Veiculo veiculo)
        {
            _Contexto.Veiculos.Add(veiculo);
            _Contexto.SaveChanges();
        }

        public List<Veiculo> Todos(int? pagina = 1, string? Nome = null, string? Marca = null)
        {
            var query = _Contexto.Veiculos.AsQueryable();
            if (!string.IsNullOrEmpty(Nome))
            {
                query = query.Where(v => EF.Functions.Like(v.Nome.ToLower(), $"%{Nome}%"));
            }
            int ItensPorPagina = 10;

            if (pagina != null)
            {
                query = query.Skip(((int)pagina - 1) * ItensPorPagina).Take(ItensPorPagina);
            }
            
            return query.ToList();
        }
    }

    
}