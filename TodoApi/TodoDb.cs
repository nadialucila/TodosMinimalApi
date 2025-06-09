using Microsoft.EntityFrameworkCore;

namespace TodoApi;

public class TodoDb : DbContext
{
    public TodoDb(DbContextOptions options) : base(options)
    {
    }

    public DbSet<TodoItem> Todos { get; set; }

    protected TodoDb()
    {
    }
}
