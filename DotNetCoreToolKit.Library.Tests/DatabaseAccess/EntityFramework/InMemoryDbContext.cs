using DotNetCoreToolKit.Library.Tests.Models;
using Microsoft.EntityFrameworkCore;

namespace DotNetCoreToolKit.Library.Tests.DatabaseAccess.EntityFramework
{
    public class InMemoryDbContext : DbContext 
    {
        public InMemoryDbContext()
        { }

        public InMemoryDbContext(DbContextOptions<InMemoryDbContext> options)
        : base(options)
        { }

        public DbSet<Customer> Customers { get; set; } 
    }
}
