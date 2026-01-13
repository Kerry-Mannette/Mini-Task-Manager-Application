using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mini_Task_Manager_Application.Data;
using Mini_Task_Manager_Application.Models;

namespace Mini_Task_Manager_Application.Controllers
{
    public class TasksController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly Microsoft.Extensions.Logging.ILogger<TasksController> _logger;

        public TasksController(ApplicationDbContext db, Microsoft.Extensions.Logging.ILogger<TasksController> logger)
        {
            _db = db;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var tasks = await _db.Tasks.OrderBy(t => t.IsCompleted).ThenBy(t => t.DueDate).ToListAsync();
                return View(tasks);
            }
            catch (Exception ex)
            {
                // Log full exception for diagnostics
                _logger.LogError(ex, "Failed to query Tasks from the database.");

                // Provide a user-friendly message in the UI
                ViewBag.DbError = "Cannot connect to the task database. Please check the connection string and network access.";

                // Return empty list so the view can render with the message
                return View(new List<Mini_Task_Manager_Application.Models.TaskItem>());
            }
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TaskItem taskItem)
        {
            if (!ModelState.IsValid) return View(taskItem);
            _db.Tasks.Add(taskItem);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var task = await _db.Tasks.FindAsync(id.Value);
            if (task == null) return NotFound();
            return View(task);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, TaskItem taskItem)
        {
            if (id != taskItem.Id) return BadRequest();
            if (!ModelState.IsValid) return View(taskItem);
            _db.Entry(taskItem).State = EntityState.Modified;
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();
            var task = await _db.Tasks.FirstOrDefaultAsync(t => t.Id == id.Value);
            if (task == null) return NotFound();
            return View(task);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var task = await _db.Tasks.FindAsync(id.Value);
            if (task == null) return NotFound();
            return View(task);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var task = await _db.Tasks.FindAsync(id);
            if (task != null)
            {
                _db.Tasks.Remove(task);
                await _db.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MarkComplete(int id)
        {
            var task = await _db.Tasks.FindAsync(id);
            if (task == null) return NotFound();
            task.IsCompleted = true;
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
