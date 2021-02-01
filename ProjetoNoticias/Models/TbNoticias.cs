using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ProjetoNoticias.Models
{
    public partial class TbNoticias
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "O Título é obrigatório")]
        [Display(Name = "Título")]
        public string Titulo { get; set; }

        [Required(ErrorMessage = "A Descrição é obrigatório")]
        [Display(Name = "Descrição")]
        public string Descricao { get; set; }

        [Display(Name = "Data da Publicação")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        [DataType(DataType.Date, ErrorMessage = "Formato de Data Inválido!")]
        public DateTime? Data { get; set; }

        [Required(ErrorMessage = "O Autor é obrigatório")]
        [Display(Name = "Autor")]
        public int? AutorId { get; set; }

        [Required(ErrorMessage = "A Categoria é obrigatório")]
        [Display(Name = "Categoria")]
        public int? CategoriaId { get; set; }

       public TbAutor Autor { get; set; }

        public TbCategoria Categoria { get; set; }
    }
}
