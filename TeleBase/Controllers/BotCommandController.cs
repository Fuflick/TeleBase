using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TeleBase;
using TeleBase.Models;

namespace test_app.Controllers
{
    public class BotCommandController : Controller
    {
        private readonly MyDbContext _context;

        public BotCommandController()
        {
            _context = new MyDbContext();
        }

        // GET: DocProcedure
        public async Task<IActionResult> Index()
        {
            return View(await _context.BotCommands.ToListAsync());
        }

        // GET: DocProcedure/Create
        public IActionResult Create()
        {
            // Заполнение списка врачей
            ViewBag.BotsList = new SelectList(_context.Bots.ToList(), "Id", "Name");

            // Заполнение списка диагнозов
            ViewBag.CommandsList = new SelectList(_context.Commands.ToList(), "Id", "Name");

            return View();
        }


        // POST: DocDiagnose/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,DocId,DiagId")] BotCommand botCommand)
        {
            if (ModelState.IsValid)
            {
                _context.Add(botCommand);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(botCommand);
        }


        // GET: DocProcedure/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var botCommand = await _context.BotCommands.FindAsync(id);
            if (botCommand == null)
            {
                return NotFound();
            }

            // Получите данные о врачах и диагнозах
            ViewBag.BotsList = new SelectList(_context.Bots, "Id", "Name");
            ViewBag.CommandsList = new SelectList(_context.Commands, "Id", "Name");

            return View(botCommand);
        }


        // POST: DocProcedure/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,BotId,CommandId")] BotCommand botCommand)
        {
            if (id != botCommand.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(botCommand);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BotCommandExists(botCommand.Id))
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
            return View(botCommand);
        }

        // GET: DocProcedure/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var botCommand = await _context.BotCommands
                .FirstOrDefaultAsync(m => m.Id == id);
            if (botCommand == null)
            {
                return NotFound();
            }

            return View(botCommand);
        }

        // POST: DocProcedure/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var botCommand = await _context.BotCommands.FindAsync(id);
            if (botCommand == null)
            {
                return NotFound();
            }

            // Проверка на наличие связей с Doctor и Procedure
            var hasDoctorLinks = _context.Bots.Any(d => d.Id == botCommand.BotId);
            var hasProcedureLinks = _context.Commands.Any(p => p.Id == botCommand.CommandId);
            if (hasDoctorLinks || hasProcedureLinks)
            {
                TempData["ErrorMessage"] = "Нельзя удалить процедуру, так как она связана с одним или несколькими врачами или процедурами.";
                return RedirectToAction(nameof(Index));
            }

            _context.BotCommands.Remove(botCommand);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        


        private bool BotCommandExists(int id)
        {
            return _context.BotCommands.Any(e => e.Id == id);
        }
    }
}