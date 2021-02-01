using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ProjetoNoticias.Models;

namespace ProjetoNoticias.Controllers
{
    public class LoginController : Controller
    {
        private readonly AppDbContext _context;

        public LoginController(AppDbContext context)
        {
            _context = context;

        }

        public IActionResult Logar()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Logar(TbLogin login)
        {
            if (ModelState.IsValid)
            {
                login.Senha = Modulo.GerarHashMd5(login.Senha);

                var User = _context.TbLogin.Where(W => W.Username == login.Username && W.Senha == login.Senha);
                if(User.Any())
                {
                    Modulo._login = User.FirstOrDefault();
                    return RedirectToAction("Index", "TbNoticias");
                }
                else
                {
                    ModelState.AddModelError("Senha", "Usuario ou Senha Inválido!");
                    Modulo._login = null;
                }

               
            }

            return View(login);

        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TbLogin login)
        {
            if (ModelState.IsValid)
            {
                login.Senha = Modulo.GerarHashMd5(login.Senha);

                _context.Add(login);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index","TbNoticias");
            }

            return View(login);
           
        }

        public IActionResult Logout()
        {
            Modulo._login = null;
            return RedirectToAction("Index", "TbNoticias");
        }
    }
}