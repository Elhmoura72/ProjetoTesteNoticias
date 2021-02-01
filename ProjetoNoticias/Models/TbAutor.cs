using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ProjetoNoticias.Models
{
    public partial class TbAutor
    {
        public TbAutor()
        {
            TbNoticias = new HashSet<TbNoticias>();
        }

        public int Id { get; set; }

        [Required(ErrorMessage = "O nome é obrigatório")]
        [Display(Name = "Nome do Autor")]
        public string Nome { get; set; }

        public ICollection<TbNoticias> TbNoticias { get; set; }
    }
}
