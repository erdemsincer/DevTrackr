using Microsoft.EntityFrameworkCore;
using PomodoroService.Models;
using System.Collections.Generic;

namespace PomodoroService.Data
{
    public class PomodoroDbContext(DbContextOptions<PomodoroDbContext> options) : DbContext(options)
    {
        public DbSet<PomodoroSession> Sessions { get; set; }
    }
}
