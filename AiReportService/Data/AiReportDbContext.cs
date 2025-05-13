using AiReportService.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace AiReportService.Data
{
    public class AiReportDbContext(DbContextOptions<AiReportDbContext> options) : DbContext(options)
    {
        public DbSet<AiReport> Reports => Set<AiReport>();
    }
}
