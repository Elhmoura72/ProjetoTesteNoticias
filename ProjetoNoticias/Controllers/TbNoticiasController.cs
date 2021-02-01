using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProjetoNoticias.Models;
using X.PagedList;

namespace ProjetoNoticias.Controllers
{
    public class TbNoticiasController : Controller
    {
        private readonly AppDbContext _context;
       
        public TbNoticiasController(AppDbContext context)
        {
            _context = context;
         
        }

        

        // GET: TbNoticias
        public async Task<IActionResult> Index(int pagina = 1)
        {
            var appDbContext = _context.TbNoticias.Include(t => t.Autor).Include(t => t.Categoria);
            ViewBag.Pagina = pagina;
            //TempData["pagina"] = pagina;
            return View(await appDbContext.ToPagedListAsync(pagina,1));
        }

        // GET: TbNoticias/Details/5
        public async Task<IActionResult> Details(int? id, int pagina)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tbNoticias = await _context.TbNoticias
                .Include(t => t.Autor)
                .Include(t => t.Categoria)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tbNoticias == null)
            {
                return NotFound();
            }

            ViewBag.Pagina = pagina;
            return View(tbNoticias);
        }

        private bool TituloExiste(string titulo, int Id = 0)
        {
            if (Id == 0)
            {
                return _context.TbNoticias.Any(A => A.Titulo.Trim().ToUpper() == titulo.Trim().ToUpper());
            }
            else
            {
                return _context.TbNoticias.Any(A => A.Titulo.Trim().ToUpper() == titulo.Trim().ToUpper() && A.Id != Id);
            }
        }

        private List<TbAutorUnique> AutorUnique()
        {
            return (from Autor in _context.TbAutor.AsEnumerable()
                    join Noticia in _context.TbNoticias.AsEnumerable()
                    on Autor.Id equals Noticia.AutorId into Joined
                    from GrupoAutor in Joined.DefaultIfEmpty()
                    select new TbAutorUnique
                    {
                        Id = Autor.Id,
                        Nome = Autor.Nome,
                        Autor_Id = GrupoAutor == null ? 0 : GrupoAutor.Id
                    }).Where(W => W.Autor_Id == 0).ToList();


        }

        // GET: TbNoticias/Create
        public IActionResult Create()
        {
           ViewData["Autor"] = new SelectList(AutorUnique(), "Id", "Nome");
           ViewData["Categoria"] = new SelectList(_context.TbCategoria, "Id", "Nome");
            return View();
        }

        // POST: TbNoticias/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Titulo,Descricao,Data,AutorId,CategoriaId")] TbNoticias tbNoticias)
        {
            if (TituloExiste(tbNoticias.Titulo) == false)
            {
                if (ModelState.IsValid)
                {
                    _context.Add(tbNoticias);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }
            else
            {
                ModelState.AddModelError("Titulo", "Título Já Existe!");
            }
            ViewData["Autor"] = new SelectList(AutorUnique(), "Id", "Nome", tbNoticias.AutorId);
            ViewData["Categoria"] = new SelectList(_context.TbCategoria, "Id", "Nome", tbNoticias.CategoriaId);
            return View(tbNoticias);
        }

        // GET: TbNoticias/Edit/5
        public async Task<IActionResult> Edit(int? id,int pagina)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tbNoticias = await _context.TbNoticias.FindAsync(id);
            if (tbNoticias == null)
            {
                return NotFound();
            }

            ViewBag.Pagina = pagina;
            ViewData["Autor"] = new SelectList(_context.TbAutor, "Id", "Nome", tbNoticias.AutorId);
            ViewData["Categoria"] = new SelectList(_context.TbCategoria, "Id", "Nome", tbNoticias.CategoriaId);
            return View(tbNoticias);
        }

        // POST: TbNoticias/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Titulo,Descricao,Data,AutorId,CategoriaId")] TbNoticias tbNoticias,int pagina)
        {
            if (id != tbNoticias.Id)
            {
                return NotFound();
            }

            if (TituloExiste(tbNoticias.Titulo, tbNoticias.Id) == false)
            {
                if (ModelState.IsValid)
                {
                    try
                    {
                        _context.Update(tbNoticias);
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!TbNoticiasExists(tbNoticias.Id))
                        {
                            return NotFound();
                        }
                        else
                        {
                            throw;
                        }
                    }
                    
                    return RedirectToAction("Index", new { pagina = pagina });
                }
            }
            else
            {
                ModelState.AddModelError("Titulo", "Título Já Existe!");
            }
            ViewBag.Pagina = pagina;
            ViewData["Autor"] = new SelectList(_context.TbAutor, "Id", "Nome", tbNoticias.AutorId);
            ViewData["Categoria"] = new SelectList(_context.TbCategoria, "Id", "Nome", tbNoticias.CategoriaId);
            return View(tbNoticias);
        }

        // GET: TbNoticias/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tbNoticias = await _context.TbNoticias
                .Include(t => t.Autor)
                .Include(t => t.Categoria)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tbNoticias == null)
            {
                return NotFound();
            }

            return View(tbNoticias);
        }

        // POST: TbNoticias/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var tbNoticias = await _context.TbNoticias.FindAsync(id);
            _context.TbNoticias.Remove(tbNoticias);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TbNoticiasExists(int id)
        {
            return _context.TbNoticias.Any(e => e.Id == id);
        }
    }
}
