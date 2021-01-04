using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using NewCoreConsoleApp.Models;
using System.IO;

namespace NewCoreConsoleApp.Data
{
    public class NewCoreConsoleAppContext : DbContext
    {
        //public NewCoreConsoleAppContext(DbContextOptions<NewCoreConsoleAppContext> options) : base(options)
        //{

        //}

        protected override void OnConfiguring(DbContextOptionsBuilder builder)
        {
            var configuration = new ConfigurationBuilder()
                                .SetBasePath(Directory.GetCurrentDirectory())
                                .AddJsonFile("appsettings.json", true, true)
                                .Build();

            var connectionString = configuration.GetConnectionString("NewCoreConsoleApp");

            builder.UseSqlServer(connectionString);
        }

        public DbSet<Person> People { get; set; }
    }
}
