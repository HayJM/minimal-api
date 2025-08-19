using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Forms;
using minimal_api.Dominio.DTOs;
using minimal_api.Dominio.Entidades;
using minimal_api.Dominio.Interfaces;
using minimal_api.Dominio.Servicos;
using Test.Domain.Servicos;


namespace Test.Mocks
{
    public class AdministradorServicosMock : IAdministradorServicos
    {
        private static List<Administrador> administradores = new List<Administrador>(){

            new Administrador {
                Id = 1,
                Email = "adm@teste.com",
                Senha = "123456",
                Perfil = "Adm",
            },
            new Administrador {
                Id = 2,
                Email = "edtor@teste.com",
                Senha = "editor",
                Perfil = "Editor",
            }
        };
        public Administrador? BuscaPorId(int id)
        {
            return administradores.Find(a => a.Id == id);
        }

        public Administrador Incluir(Administrador administrador)
        {
            administrador.Id = administradores.Count + 1; // Simulating auto-increment ID
            administradores.Add(administrador);
            return administrador;
        }

        public Administrador? Login(LoginDTD loginDTD)
        {
            return administradores.Find(a => a.Email == loginDTD.Email && a.Senha == loginDTD.Senha);
        }

        public List<Administrador> Todos(int? pagina)
        {
            return administradores;
        }
    }

    
}