using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NewCore3xMVC.Models;
using RepositoryPattern.Models;
using NewCore3xMVC.Models.Views;

namespace NewCore3xMVC.Data
{
    public class NewCore3xMVCContext : DbContext
    {
        public NewCore3xMVCContext (DbContextOptions<NewCore3xMVCContext> options)
            : base(options)
        {
        }

        public DbSet<NewCore3xMVC.Models.Person> Person { get; set; }

        public DbSet<RepositoryPattern.Models.Employee> Employee { get; set; }

        public DbSet<NewCore3xMVC.Models.Views.PersonAddressView> PersonAddressView { get; set; }
    }
}
