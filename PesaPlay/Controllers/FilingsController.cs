using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SecData;
using SecData.Models.Db;

namespace PesaPlay.Controllers
{
    public class FilingsController : Controller
    {
        private readonly FilingsDbContext _context;

        public FilingsController(FilingsDbContext context)
        {
            _context = context;
        }

        // GET: Filings
        public async Task<IActionResult> Index()
        {
            return View(await _context.Filings.ToListAsync());
        }

        // GET: Filings/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var filing = await _context.Filings
                .FirstOrDefaultAsync(m => m.Id == id);
            if (filing == null)
            {
                return NotFound();
            }

            return View(filing);
        }

        // GET: Filings/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Filings/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Cik,FileDate,FormType")] Filing filing)
        {
            if (ModelState.IsValid)
            {
                _context.Add(filing);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(filing);
        }

        // GET: Filings/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var filing = await _context.Filings.FindAsync(id);
            if (filing == null)
            {
                return NotFound();
            }
            return View(filing);
        }

        // POST: Filings/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Cik,FileDate,FormType")] Filing filing)
        {
            if (id != filing.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(filing);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FilingExists(filing.Id))
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
            return View(filing);
        }

        // GET: Filings/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var filing = await _context.Filings
                .FirstOrDefaultAsync(m => m.Id == id);
            if (filing == null)
            {
                return NotFound();
            }

            return View(filing);
        }

        // POST: Filings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var filing = await _context.Filings.FindAsync(id);
            _context.Filings.Remove(filing);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FilingExists(int id)
        {
            return _context.Filings.Any(e => e.Id == id);
        }
    }
}
