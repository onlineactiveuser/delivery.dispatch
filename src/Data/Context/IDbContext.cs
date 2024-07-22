using Microsoft.EntityFrameworkCore;

namespace Data.Context
{
    public interface IDbContext
    {
        DbContext Context { get; }
    }
}
