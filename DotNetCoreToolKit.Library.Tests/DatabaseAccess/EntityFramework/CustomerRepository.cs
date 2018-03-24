using DotNetCoreToolKit.Library.Tests.Models;
using DotNetCoreToolKit.Library.Implementations.Repositories;
using MediatR;

namespace DotNetCoreToolKit.Library.Tests.DatabaseAccess.EntityFramework
{
    public class CustomerRepository : EfDataRepository<Customer, InMemoryDbContext>
    {
        public CustomerRepository(InMemoryDbContext dbContext, IMediator mediatr):base(dbContext,mediatr)
        {

        }
    }
}
