using Microsoft.EntityFrameworkCore;

namespace aspnet5_base_ef.Models
{
    public class TodoContext : DbContext
    {
        public TodoContext(DbContextOptions<TodoContext> options)
            : base(options)
        {
        }

        public DbSet<TodoItem> TodoItems => Set<TodoItem>(); // fixes nullable type warning, EF will sort it.
    }
}