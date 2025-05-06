using Microsoft.EntityFrameworkCore;
using Store_app.Models;

namespace Store_app.services
{
    public class ApplicationDbcontext : DbContext
    {
        public ApplicationDbcontext(DbContextOptions options) : base(options)
        {

        }
        public DbSet<Product> Products { get; set; }
    }
}
