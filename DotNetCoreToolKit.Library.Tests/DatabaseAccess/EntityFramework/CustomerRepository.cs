using DotNetCoreToolKit.Library.Tests.Models;
using DotNetCoreToolKit.Library.Implementations.Repositories;

namespace DotNetCoreToolKit.Library.Tests.DatabaseAccess.EntityFramework
{
    public class CustomerRepository : EfDataRepository<Customer, InMemoryDbContext>
    {
        public CustomerRepository(InMemoryDbContext dbContext):base(dbContext)
        {

        }
    }
}
