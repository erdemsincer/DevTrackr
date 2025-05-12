using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using TaskService.Models;

namespace TaskService.Data
{
    public class TaskDbContext : DbContext
    {
        public TaskDbContext(DbContextOptions<TaskDbContext> options) : base(options) { }

        public DbSet<TaskItem> Tasks => Set<TaskItem>();
    }
}
