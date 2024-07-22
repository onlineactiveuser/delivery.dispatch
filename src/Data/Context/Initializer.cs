using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;

namespace Data.Context
{
    [ExcludeFromCodeCoverage]
    public static class Initializer
    {
        public static void Initialize(PostgreDbContext context)
        {

            context.Database.Migrate();

            context.SaveChanges();
        }
    }
}
