using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ProjetoNoticias.Models
{
    public partial class TbLogin
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Usuario é obrigatório")]
        [Display(Name = "Usuario")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Senha é obrigatório")]
        [Display(Name = "Senha")]
        public string Senha { get; set; }

        //[Required(ErrorMessage = "O nome é obrigatório")]
        [Display(Name = "Nome")]
        public string Nome { get; set; }
    }
}
