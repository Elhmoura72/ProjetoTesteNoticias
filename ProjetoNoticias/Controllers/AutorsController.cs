using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProjetoNoticias.Models;

namespace ProjetoNoticias.Controllers
{
    public class AutorsController : Controller
    {
        private readonly AppDbContext _context;

        public AutorsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Autors
        public async Task<IActionResult> Index()
        {
            return View(await _context.TbAutor.ToListAsync());
        }

        private bool AutorExiste(string nome,int Id = 0)
        {
            if (Id == 0)
            {
                return _context.TbAutor.Any(A => A.Nome.Trim().ToUpper() == nome.Trim().ToUpper());
            }
            else
            {
                return _context.TbAutor.Any(A => A.Nome.Trim().ToUpper() == nome.Trim().ToUpper() && A.Id != Id);
            }
        }

        // GET: Autors/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tbAutor = await _context.TbAutor
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tbAutor == null)
            {
                return NotFound();
            }

            return View(tbAutor);
        }

        // GET: Autors/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Autors/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nome")] TbAutor tbAutor)
        {
            if (AutorExiste(tbAutor.Nome) == false)
            {
                if (ModelState.IsValid)
                {
                    _context.Add(tbAutor);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }
            else
            {
                ModelState.AddModelError("Nome", "Nome de Autor Já Existe!");
            }
            return View(tbAutor);
        }

        // GET: Autors/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tbAutor = await _context.TbAutor.FindAsync(id);
            if (tbAutor == null)
            {
                return NotFound();
            }
            return View(tbAutor);
        }

        // POST: Autors/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nome")] TbAutor tbAutor)
        {
            if (id != tbAutor.Id)
            {
                return NotFound();
            }

            if (AutorExiste(tbAutor.Nome,tbAutor.Id) == false)
            {
                if (ModelState.IsValid)
                {
                    try
                    {
                        _context.Update(tbAutor);
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!TbAutorExists(tbAutor.Id))
                        {
                            return NotFound();
                        }
                        else
                        {
                            throw;
                        }
                    }
                    return RedirectToAction(nameof(Index));
                }
            }
            else
            {
                ModelState.AddModelError("Nome", "Nome de Autor Já Existe!");
            }
            return View(tbAutor);
        }

        private bool RelacionamentoNoticia(int Id)
        {
            return (from Noticia in _context.TbNoticias.AsEnumerable()
                    join Autor in _context.TbAutor.AsEnumerable()
                    on Noticia.AutorId equals Autor.Id
                    where Autor.Id == Id
                    select Noticia).Any();
        }

        // GET: Autors/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tbAutor = await _context.TbAutor
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tbAutor == null)
            {
                return NotFound();
            }

            return View(tbAutor);
        }

        // POST: Autors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (RelacionamentoNoticia(id) == false)
            {
                var tbAutor = await _context.TbAutor.FindAsync(id);
                _context.TbAutor.Remove(tbAutor);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            else
            {
                ModelState.AddModelError("Nome", "*Exclusão Inválida, Autor está em relacionamento com Notícia!");
                var NoticiaId = await _context.TbAutor.FirstOrDefaultAsync(m => m.Id == id);
                return View(NoticiaId);
            }
        }

        private bool TbAutorExists(int id)
        {
            return _context.TbAutor.Any(e => e.Id == id);
        }
    }
}
