using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AspNetCoreCrudUsingStoredProcedures.Data;
using AspNetCoreCrudUsingStoredProcedures.Models;

namespace AspNetCoreCrudUsingStoredProcedures.Controllers
{
    public class BooksController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BooksController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Books
        public async Task<IActionResult> Index()
        {
              return _context.BookViewModel != null ? 
                          View(await _context.BookViewModel.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.BookViewModel'  is null.");
        }

        // GET: Books/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.BookViewModel == null)
            {
                return NotFound();
            }

            var bookViewModel = await _context.BookViewModel
                .FirstOrDefaultAsync(m => m.bookId == id);
            if (bookViewModel == null)
            {
                return NotFound();
            }

            return View(bookViewModel);
        }

        // GET: Books/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Books/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("bookId,title,author,price")] BookViewModel bookViewModel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(bookViewModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(bookViewModel);
        }

        // GET: Books/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.BookViewModel == null)
            {
                return NotFound();
            }

            var bookViewModel = await _context.BookViewModel.FindAsync(id);
            if (bookViewModel == null)
            {
                return NotFound();
            }
            return View(bookViewModel);
        }

        // POST: Books/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("bookId,title,author,price")] BookViewModel bookViewModel)
        {
            if (id != bookViewModel.bookId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(bookViewModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BookViewModelExists(bookViewModel.bookId))
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
            return View(bookViewModel);
        }

        // GET: Books/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.BookViewModel == null)
            {
                return NotFound();
            }

            var bookViewModel = await _context.BookViewModel
                .FirstOrDefaultAsync(m => m.bookId == id);
            if (bookViewModel == null)
            {
                return NotFound();
            }

            return View(bookViewModel);
        }

        // POST: Books/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.BookViewModel == null)
            {
                return Problem("Entity set 'ApplicationDbContext.BookViewModel'  is null.");
            }
            var bookViewModel = await _context.BookViewModel.FindAsync(id);
            if (bookViewModel != null)
            {
                _context.BookViewModel.Remove(bookViewModel);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BookViewModelExists(int id)
        {
          return (_context.BookViewModel?.Any(e => e.bookId == id)).GetValueOrDefault();
        }
    }
}
