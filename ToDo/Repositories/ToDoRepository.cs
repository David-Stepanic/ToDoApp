using Microsoft.EntityFrameworkCore;
using ToDoTest.Models;

namespace ToDoTest.Repositories
{
    public class ToDoRepository : IToDoRepository
    {
        private readonly ToDoContext _context;

        public ToDoRepository(ToDoContext context)
        {
            _context = context;
        }
        
        public async Task<List<Category>> GetCategoriesAsync()
            => await _context.Categories.ToListAsync();

        public async Task<List<Status>> GetStatusesAsync()
            => await _context.Statuses.ToListAsync();

        public async Task AddAsync(ToDo task)
            => await _context.ToDos.AddAsync(task);

        public async Task SaveChangesAsync()
            => await _context.SaveChangesAsync();

        public async Task<ToDo?> GetByIdAsync(int id)
        {
            return await _context.ToDos
                .Include(t => t.Category)
                .Include(t => t.Status)
                .FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<List<ToDo>> GetCompletedTasksAsync()
        {
            return await _context.ToDos
                .Where(t => t.StatusId == "closed")
                .ToListAsync();
        }
        public void Remove(ToDo task)
        {
            _context.ToDos.Remove(task);
        }

        public async Task UpdateAsync(ToDo task)
        {
            var existingTask = await _context.ToDos.FindAsync(task.Id);
            if (existingTask == null) return;

            existingTask.Description = task.Description;
            existingTask.CategoryId = task.CategoryId;
            existingTask.DueDate = task.DueDate;
            existingTask.StatusId = task.StatusId;
        }
        public async Task<List<ToDo>> GetFilteredToDosAsync(Filters filters, string userId)
        {
            var query = _context.ToDos
            .Include(t => t.Category)
            .Include(t => t.Status)
            .Where(t => t.UserId == userId)
            .AsQueryable();

            if (filters.HasCategory)
                query = query.Where(t => t.CategoryId == filters.CategoryId);

            if (filters.HasStatus)
                query = query.Where(t => t.StatusId == filters.StatusId);

            if (filters.HasDue)
            {
                var today = DateTime.Today;
                if (filters.IsPast)
                    query = query.Where(t => t.DueDate < today);
                else if (filters.IsFuture)
                    query = query.Where(t => t.DueDate > today);
                else if (filters.IsToday)
                    query = query.Where(t => t.DueDate == today);
            }

            return await query.OrderBy(t => t.DueDate).ToListAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var task = await _context.ToDos.FindAsync(id);
            if (task != null)
            {
                _context.ToDos.Remove(task);
                _context.SaveChangesAsync();
            }
        }
        public async Task<List<ToDo>> GetUserToDosAsync(string userId)
        {
            return await _context.ToDos
            .Where(t => t.UserId == userId)
            .ToListAsync();
        }

        public async Task<ToDo> GetToDoByIdAsync(int id)
        {
            return await _context.ToDos.FindAsync(id);
        }

        public async Task AddToDoAsync(ToDo todo)
        {
            _context.ToDos.Add(todo);
            await _context.SaveChangesAsync();
        }
    }
}
