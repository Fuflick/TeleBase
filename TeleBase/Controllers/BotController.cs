using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TeleBase.Models;

namespace TeleBase.Controllers
{
    public class BotController : Controller
    {
        public MyDbContext dbContext = new MyDbContext();
        
        public async Task<IActionResult> Index()
        {
            return View(await dbContext.Bots.ToListAsync());
        }

        public IActionResult Create()
        {
            return View();
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name")] Bot bot)
        {
            if (ModelState.IsValid)
            {
                await dbContext.Bots.AddAsync(bot);
                await dbContext.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(bot);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bot = await dbContext.Bots.FindAsync(id);
            if (bot == null)
            {
                return NotFound();
            }

            return View(bot);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,[Bind("Id, Name")] Bot bot)
        {
            if (id != bot.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    dbContext.Update(bot);
                    await dbContext.SaveChangesAsync();
                }
                catch(DbUpdateConcurrencyException)
                {
                    if (!BotExist(bot.Id))
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

            return View(bot);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bot = await dbContext.Bots.FirstOrDefaultAsync(b => b.Id == id);
            if (bot == null)
            {
                return NotFound();
            }

            return View(bot);
        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var bot = await dbContext.Bots.FindAsync(id);
            dbContext.Bots.Remove(bot);
            await dbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BotExist(int id)
        {
            return dbContext.Bots.Any(b => b.Id == id);
        }
    }
}