using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using TeleBase.Models;

namespace TeleBase.Controllers
{
    public class UserChatController : Controller
    {
        private MyDbContext _dbContext;

        public UserChatController()
        {
            _dbContext = new MyDbContext();
        }

        public async Task<IActionResult> Index()
        {
            return View(await _dbContext.UserChats.ToListAsync());
        }

        public IActionResult Create()
        {
            ViewBag.UserList = new SelectList(_dbContext.Users.ToList(), "Id", "Name");
            ViewBag.ChatList = new SelectList(_dbContext.Chats.ToList(), "Id", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UserId, ChatId")] UserChat userChat)
        {
            if (ModelState.IsValid)
            {
                _dbContext.Add(userChat);
                await _dbContext.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewBag.UsersList = new SelectList(_dbContext.Users.ToList(), "Id", "Name", userChat.UserId);
            ViewBag.ChatList = new SelectList(_dbContext.Chats.ToList(), "Id", "Name", userChat.ChatId);
            return View(userChat);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userChat = await _dbContext.UserChats.FindAsync(id);
            if (userChat == null)
            {
                return NotFound();
            }

            ViewBag.UserList = new SelectList(_dbContext.Users.ToList(), "Id", "Name", userChat.UserId);
            ViewBag.ChatList = new SelectList(_dbContext.Chats.ToList(), "Id", "Name", userChat.ChatId);

            return View(userChat);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,UserId,ChatId")] UserChat userChat)
        {
            if (id != userChat.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _dbContext.Update(userChat);
                    await _dbContext.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(userChat);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userChat = await _dbContext.UserChats.FirstOrDefaultAsync(m => m.Id == id);
            if (userChat == null)
            {
                return NotFound();
            }

            return View(userChat);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var userChat = await _dbContext.UserChats.FindAsync(id);
            _dbContext.UserChats.Remove(userChat);
            await _dbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserChatExists(int id)
        {
            return _dbContext.UserChats.Any(e => e.Id == id);
        }
    }
}
