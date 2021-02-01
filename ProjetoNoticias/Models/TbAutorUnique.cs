using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjetoNoticias.Models
{
    public class TbAutorUnique
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public int Autor_Id { get; set; }

    }
}
