using Microsoft.EntityFrameworkCore;
using WebApplication6.Models;

namespace WebApplication6.NewFolder1
{
    public class DBContextsol: DbContext
    {

        public DBContextsol(DbContextOptions<DBContextsol> options):base (options)
        {

        }
    
        public DbSet<Category> Categories { get; set; }    
    }
}
