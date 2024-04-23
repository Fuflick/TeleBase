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
    public class ChatController : Controller
    {
        public MyDbContext dbContext = new MyDbContext();
        
        public async Task<IActionResult> Index()
        {
            return View(await dbContext.Chats.ToListAsync());
        }

        public IActionResult Create()
        {
            return View();
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name")] Chat chat)
        {
            if (ModelState.IsValid)
            {
                await dbContext.Chats.AddAsync(chat);
                await dbContext.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(chat);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var chat = await dbContext.Chats.FindAsync(id);
            if (chat == null)
            {
                return NotFound();
            }

            return View(chat);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,[Bind("Id, Name")] Chat chat)
        {
            if (id != chat.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    dbContext.Update(chat);
                    await dbContext.SaveChangesAsync();
                }
                catch(DbUpdateConcurrencyException)
                {
                    if (!ChatExist(chat.Id))
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

            return View(chat);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var chat = await dbContext.Chats.FirstOrDefaultAsync(c => c.Id == id);
            if (chat == null)
            {
                return NotFound();
            }

            return View(chat);
        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var chat = await dbContext.Chats.FindAsync(id);
            dbContext.Chats.Remove(chat);
            await dbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ChatExist(int id)
        {
            return dbContext.Chats.Any(c => c.Id == id);
        }
    }
}