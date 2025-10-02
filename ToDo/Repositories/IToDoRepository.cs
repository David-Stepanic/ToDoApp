using ToDoTest.Models;

namespace ToDoTest.Repositories
{
    public interface IToDoRepository
    {
        Task<ToDo?> GetByIdAsync(int id);
        Task<List<ToDo>> GetFilteredToDosAsync(Filters filters, string userId);
        Task<List<Category>> GetCategoriesAsync();
        Task<List<Status>> GetStatusesAsync();
        Task AddAsync(ToDo task);
        Task SaveChangesAsync();
        Task<List<ToDo>> GetCompletedTasksAsync();
        void Remove(ToDo task);
        Task DeleteAsync(int id);
        Task UpdateAsync(ToDo task);
        Task<List<ToDo>> GetUserToDosAsync(string userId);
        Task<ToDo> GetToDoByIdAsync(int id);
        Task AddToDoAsync(ToDo todo);
    }
}
