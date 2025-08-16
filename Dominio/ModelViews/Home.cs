using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace minimal_api.Dominio.ModelViews
{
    public struct Home
    {
        public string Menssagem { get => "Bem vindo a API de Veiculos"; }
        public string Documentacao { get => "/swagger"; } 
    }
}