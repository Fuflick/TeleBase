using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Humanizer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TeleBase.Models;

namespace TeleBase.Controllers
{
    public class CommandController : Controller
    {
        private MyDbContext _dbContext = new MyDbContext();
        public async Task<IActionResult> Index()
        {
            return View(await _dbContext.Commands.ToListAsync());
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name")] Command command)
        {
            if (ModelState.IsValid)
            {
                await _dbContext.Commands.AddAsync(command);
                await _dbContext.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(command);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var comnand = await _dbContext.Commands.FindAsync(id);
            if (comnand == null)
            {
                return NotFound();
            }

            return View(comnand);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, [Bind("Id, Name")] Command command)
        {
            if (id != command.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _dbContext.Update(command);
                    await _dbContext.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    throw;
                }

                return RedirectToAction(nameof(Index));
            }

            return View(command);
        }
        
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var command = await _dbContext.Commands
                .FirstOrDefaultAsync(m => m.Id == id);
            if (command == null)
            {
                return NotFound();
            }

            return View(command);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var command = await _dbContext.Commands.FirstOrDefaultAsync(c => c.Id == id);
            _dbContext.Commands.Remove(command);
            await _dbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        
        private bool UserExists(int id)
        {
            return _dbContext.Commands.Any(e => e.Id == id);
        }
    }
}