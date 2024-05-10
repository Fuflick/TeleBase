using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TeleBase.Models;

namespace TeleBase.Controllers
{
    public class BotChatController : Controller
    {
        private MyDbContext _dbContext;
        public BotChatController()
        {
            _dbContext = new MyDbContext();
        }
        
        public async Task<IActionResult> Index()
        {
            return View(await _dbContext.BotChats.ToListAsync());
        }

        public IActionResult Create()
        {
            ViewBag.BotList = new SelectList(_dbContext.Bots.ToList(), "Id", "Name");
            ViewBag.ChatList = new SelectList(_dbContext.Chats.ToList(), "Id", "Name");
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("BotId, ChatId")] BotChat botChat)
        {
            if (ModelState.IsValid)
            {
                _dbContext.Add(botChat);
                await _dbContext.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(botChat);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var botChat = await _dbContext.BotChats.FindAsync(id);
            if (botChat == null)
            {
                return NotFound();
            }

            ViewBag.BotList = new SelectList(_dbContext.Bots.ToList(), "Id", "Name", botChat.BotId);
            ViewBag.ChatList = new SelectList(_dbContext.Chats.ToList(), "Id", "Name", botChat.ChatId);

            return View(botChat);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,BotId,ChatId")] BotChat botChat)
        {
            if (id != botChat.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _dbContext.Update(botChat);
                    await _dbContext.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BotChatExists(botChat.Id))
                    {
                        await _dbContext.BotChats.AddAsync(botChat);
                        await _dbContext.SaveChangesAsync();
                        return RedirectToAction(nameof(Index));
                    }

                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(botChat);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var botChat = await _dbContext.BotChats
                .FirstOrDefaultAsync(m => m.Id == id);
            if (botChat == null)
            {
                return NotFound();
            }

            return View(botChat);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var botChat = await _dbContext.BotChats.FindAsync(id);
            _dbContext.BotChats.Remove(botChat);
            await _dbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BotChatExists(int id)
        {
            return _dbContext.BotChats.Any(e => e.Id == id);
        }
    }
}
