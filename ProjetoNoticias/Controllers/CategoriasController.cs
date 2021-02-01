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
    public class CategoriasController : Controller
    {
        private readonly AppDbContext _context;

        public CategoriasController(AppDbContext context)
        {
            _context = context;
        }

        private bool CategoriaExiste(string nome, int Id = 0)
        {
            if (Id == 0)
            {
                return _context.TbCategoria.Any(A => A.Nome.Trim().ToUpper() == nome.Trim().ToUpper());
            }
            else
            {
                return _context.TbCategoria.Any(A => A.Nome.Trim().ToUpper() == nome.Trim().ToUpper() && A.Id != Id);
            }
        }

        // GET: Categorias
        public async Task<IActionResult> Index()
        {
            return View(await _context.TbCategoria.ToListAsync());
        }

        // GET: Categorias/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tbCategoria = await _context.TbCategoria
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tbCategoria == null)
            {
                return NotFound();
            }

            return View(tbCategoria);
        }

        // GET: Categorias/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Categorias/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nome")] TbCategoria tbCategoria)
        {
            if (CategoriaExiste(tbCategoria.Nome) == false)
            {
                if (ModelState.IsValid)
                {
                    _context.Add(tbCategoria);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }
            else
            {
                ModelState.AddModelError("Nome", "Categoria Já Existe!");
            }
            return View(tbCategoria);
        }

        // GET: Categorias/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tbCategoria = await _context.TbCategoria.FindAsync(id);
            if (tbCategoria == null)
            {
                return NotFound();
            }
            return View(tbCategoria);
        }

        // POST: Categorias/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nome")] TbCategoria tbCategoria)
        {
            if (id != tbCategoria.Id)
            {
                return NotFound();
            }

            if (CategoriaExiste(tbCategoria.Nome, tbCategoria.Id) == false)
            {
                if (ModelState.IsValid)
                {
                    try
                    {
                        _context.Update(tbCategoria);
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!TbCategoriaExists(tbCategoria.Id))
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
                ModelState.AddModelError("Nome", "Categoria Já Existe!");
            }
            return View(tbCategoria);
        }

        private bool RelacionamentoNoticia(int Id)
        {
            return (from Noticia in _context.TbNoticias.AsEnumerable()
                    join Categoria in _context.TbCategoria.AsEnumerable()
                    on Noticia.CategoriaId equals Categoria.Id
                    where Categoria.Id == Id
                    select Noticia).Any();
        }

        // GET: Categorias/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tbCategoria = await _context.TbCategoria
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tbCategoria == null)
            {
                return NotFound();
            }

            return View(tbCategoria);
        }

        // POST: Categorias/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (RelacionamentoNoticia(id) == false)
            {

                var tbCategoria = await _context.TbCategoria.FindAsync(id);
                _context.TbCategoria.Remove(tbCategoria);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            else
            {
                ModelState.AddModelError("Nome", "*Exclusão Inválida, Categoria está em relacionamento com Notícia!");
                var NoticiaId = await _context.TbCategoria.FirstOrDefaultAsync(m => m.Id == id);
                return View(NoticiaId);
            }
        }

        private bool TbCategoriaExists(int id)
        {
            return _context.TbCategoria.Any(e => e.Id == id);
        }
    }
}
