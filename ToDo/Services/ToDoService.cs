using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using ToDoTest.Models;
using ToDoTest.Repositories;

namespace ToDoTest.Services
{
    public class ToDoService : IToDoService
    {
        private readonly IHttpContextAccessor _accessor;
        private readonly IToDoRepository _repository;
        private readonly UserManager<ApplicationUser> _userManager;

        public ToDoService(IHttpContextAccessor accessor, IToDoRepository repository,
                UserManager<ApplicationUser> userManager)
        {
            _accessor = accessor;
            _repository = repository;
            _userManager = userManager;
        }
        public async Task<ToDo?> GetByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }
        public async Task<List<ToDo>> GetFilteredToDosAsync(Filters filters)
        {
            var user = await _userManager.GetUserAsync(_accessor.HttpContext.User);
            if(user == null)
            {
                return new List<ToDo>();
            }
            return await _repository.GetFilteredToDosAsync(filters, user.Id);
        }

        public async Task<List<Category>> GetCategoriesAsync()
            => await _repository.GetCategoriesAsync();

        public async Task<List<Status>> GetStatusesAsync()
            => await _repository.GetStatusesAsync();

        public async Task AddTaskAsync(ToDo task)
        {
            task.UserId = _accessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            await _repository.AddAsync(task);
            await _repository.SaveChangesAsync();
        }
        public async Task MarkTaskCompleteAsync(int id)
        {
            var task = await _repository.GetByIdAsync(id);
            if (task != null)
            {
                task.StatusId = "closed";
                await _repository.SaveChangesAsync();
            }
        }
        public async Task DeleteCompletedTasksAsync()
        {
            var completed = await _repository.GetCompletedTasksAsync();
            foreach (var task in completed)
            {
                _repository.Remove(task);
            }
            await _repository.SaveChangesAsync();
        }

        public async Task UpdateAsync(ToDo task)
        {
            await _repository.UpdateAsync(task);
            await _repository.SaveChangesAsync();
        }
        public async Task DeleteAsync(int id)
        {
            await _repository.DeleteAsync(id);
        }
        public async Task<List<ToDo>> GetUserToDosAsync(string userId)
        {
            return await _repository.GetUserToDosAsync(userId);
        }

        public async Task<ToDo> GetToDoByIdAsync(int id)
        {
            return await _repository.GetToDoByIdAsync(id);
        }

        public async Task AddToDoAsync(ToDo todo)
        {
            await _repository.AddToDoAsync(todo);
        }
    }
}
