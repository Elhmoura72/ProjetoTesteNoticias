using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ProjetoNoticias.Models
{
    public partial class TbCategoria
    {
        public TbCategoria()
        {
            TbNoticias = new HashSet<TbNoticias>();
        }

        public int Id { get; set; }

        [Required(ErrorMessage = "O nome é obrigatório")]
        [Display(Name = "Nome da Categoria")]
        public string Nome { get; set; }

        public ICollection<TbNoticias> TbNoticias { get; set; }
    }
}
