using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ToDoTest.Models;
using ToDoTest.Services;

namespace ToDoTest.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly IToDoService _service;

        public HomeController(IToDoService service)
        {
            _service = service;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Welcome()
        {
            return View();
        }

        public async Task<IActionResult> Index(string id)
        {
            var filters = new Filters(id);

            ViewBag.Filters = filters;
            ViewBag.Categories = await _service.GetCategoriesAsync();
            ViewBag.Statuses = await _service.GetStatusesAsync();
            ViewBag.DueFilters = Filters.DueFilterValues;

            var tasks = await _service.GetFilteredToDosAsync(filters);

            return View(tasks);
        }

        [HttpGet]
        public async Task<IActionResult> Add()
        {
            ViewBag.Categories = await _service.GetCategoriesAsync();
            ViewBag.Statuses = await _service.GetStatusesAsync();
            var task = new ToDo { StatusId = "open" };
            return View(task);
        }
        [HttpPost]
        public async Task<IActionResult> Add(ToDo task)
        {
            if (ModelState.IsValid)
            {
                await _service.AddTaskAsync(task);
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.Categories = await _service.GetCategoriesAsync();
                ViewBag.Statuses = await _service.GetStatusesAsync();
                return View(task);
            }
        }

        [HttpPost]
        public IActionResult Filter(string[] filter)
        {
            string id = string.Join('-', filter);
            return RedirectToAction("Index", new { ID = id });
        }

        [HttpPost]
        public async Task<IActionResult> MarkComplete([FromRoute] string id, ToDo selected)
        {
            await _service.MarkTaskCompleteAsync(selected.Id);
            return RedirectToAction("Index", new { ID = id });
        }

        [HttpPost]
        public async Task<IActionResult> DeleteComplete(string id)
        {
            await _service.DeleteCompletedTasksAsync();
            return RedirectToAction("Index", new { ID = id });
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var task = await _service.GetByIdAsync(id);
            if (task == null) return NotFound();

            ViewBag.Categories = await _service.GetCategoriesAsync();
            ViewBag.Statuses = await _service.GetStatusesAsync();

            return PartialView("EditPartial", task); 
        }

        [HttpPost]
        public async Task<IActionResult> Edit(ToDo task)
        {
            if (!ModelState.IsValid)
            {
                return Json(new { success = false });
            }

            await _service.UpdateAsync(task);

            var updatedTask = await _service.GetByIdAsync(task.Id);

            return Json(new
            {
                success = true,
                id = updatedTask.Id,
                description = updatedTask.Description,
                category = updatedTask.Category?.Name ?? "",
                dueDate = updatedTask.DueDate?.ToString("yyyy-MM-dd"),
                status = updatedTask.Status?.Name ?? "",
                showMarkComplete = updatedTask.StatusId == "open"
            });
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var task = await _service.GetByIdAsync(id);
            if (task == null)
            {
                return Json(new { success = false });
            }

            await _service.DeleteAsync(id);
            return Json(new { success = true, id });
        }

    }
}
