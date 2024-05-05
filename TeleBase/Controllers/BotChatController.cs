using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TeleBase.Models;

namespace TeleBase.Controllers;

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

    public  IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("BotId, ChatId")] BotChat botChat)
    {
        if (ModelState.IsValid)
        {
            if (!BotChatExist(botChat.Id));
            await _dbContext.BotChats.AddAsync(botChat);
            await _dbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(botChat);
    }

    private bool BotChatExist(int id)
    {
        return _dbContext.BotChats.Any(x => x.Id == id);
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

        return View(botChat);
    }
    
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int? id, [Bind("BotId, ChatId")] BotChat botChat)
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
                throw;
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
        var botChat = await _dbContext.BotChats.FirstOrDefaultAsync(c => c.Id == id);
        _dbContext.BotChats.Remove(botChat);
        await _dbContext.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }
}