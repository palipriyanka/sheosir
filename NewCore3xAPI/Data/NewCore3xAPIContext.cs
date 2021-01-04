using Microsoft.EntityFrameworkCore;
using NewCore3xAPI.Models;

namespace NewCore3xAPI.Data
{
    public class NewCore3xAPIContext : DbContext
    {
        public NewCore3xAPIContext(DbContextOptions<NewCore3xAPIContext> options)
           : base(options)
        {
        }

        public DbSet<Person> People { get; set; }

        public DbSet<UserDetail> UserDetails { get; set; }

    }
}
