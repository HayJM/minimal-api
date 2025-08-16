using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using minimal_api.Dominio.DTOs;
using minimal_api.Dominio.Entidades;
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

        public Administrador? Login(LoginDTD loginDTD) {
            var adm = _Contexto.Administradores.Where(a => a.Email == loginDTD.Email && a.Senha == loginDTD.Senha).FirstOrDefault();
            return adm;
        }

    }
}