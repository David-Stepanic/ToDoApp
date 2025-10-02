using Microsoft.AspNetCore.Identity;

namespace ToDoTest.Models
{
    public class ApplicationUser : IdentityUser
    {
        public ICollection<ToDo> ToDos { get; set; }
    }
}
