using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Data.Context
{
    public class PostgreDbContext : DbContext
    {
        public PostgreDbContext(DbContextOptions<PostgreDbContext> dbContextOptions) : base(dbContextOptions)
        {

        }

        public DbSet<Vehicle>? Vehicles { get; set; }
    }
}
