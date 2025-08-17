using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace minimal_api.Dominio.ModelViews
{
    public class ErroDeValidacao
    {
        public List<string> Menssagens { get; set; } = default!; 
    }
}