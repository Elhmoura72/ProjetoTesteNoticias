using Microsoft.AspNetCore.Mvc;
using ProjetoNoticias.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjetoNoticias.Components
{
    public class Login: ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            string[] nomeLogin;
            if(Modulo._login != null)
            {
                nomeLogin = Modulo._login.Nome.Split(" ");
                string nome = "Olá, " + nomeLogin[0];
                return View((object)nome);
            }
            else
            {
                return View((object)"Login");
            }
        }
    }
}
