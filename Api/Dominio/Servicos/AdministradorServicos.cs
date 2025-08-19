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
    public class AdministradorServicos : IAdministradorServicos
    {
        private readonly DBContexto _Contexto;

        public AdministradorServicos(DBContexto contexto)
        {
            _Contexto = contexto;
        }

        public Administrador? BuscaPorId(int id)
        {
            return _Contexto.Administradores.Where(v => v.Id == id).FirstOrDefault();
        }

        public Administrador Incluir(Administrador administrador)
        {
            _Contexto.Administradores.Add(administrador);
            _Contexto.SaveChanges();
            return administrador;
        }

        public Administrador? Login(LoginDTD loginDTD) {
            var adm = _Contexto.Administradores.Where(a => a.Email == loginDTD.Email && a.Senha == loginDTD.Senha).FirstOrDefault();
            return adm;
        }

        public List<Administrador> Todos(int? pagina)
        {
            var query = _Contexto.Administradores.AsQueryable();
                
            int ItensPorPagina = 10;

            if (pagina != null)
            {
                query = query.Skip(((int)pagina - 1) * ItensPorPagina).Take(ItensPorPagina);
            }
            
            return query.ToList();
        }
    }
}