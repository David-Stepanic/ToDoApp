using ToDoTest.Models;

namespace ToDoTest.Services
{
    public interface IToDoService
    {
        Task<ToDo?> GetByIdAsync(int id);
        Task<List<ToDo>> GetFilteredToDosAsync(Filters filters);
        Task<List<Category>> GetCategoriesAsync();
        Task<List<Status>> GetStatusesAsync();
        Task AddTaskAsync(ToDo task);
        Task MarkTaskCompleteAsync(int id);
        Task DeleteCompletedTasksAsync();
        Task DeleteAsync(int id);
        Task UpdateAsync(ToDo task);
        Task<List<ToDo>> GetUserToDosAsync(string userId);
        Task<ToDo> GetToDoByIdAsync(int id);
        Task AddToDoAsync(ToDo todo);
    }
}
