using System;
using System.Threading.Tasks;
using DotNetCoreToolKit.Library.Tests.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using static DotNetCoreToolKit.Library.Models.Persistence.Enums;

namespace DotNetCoreToolKit.Library.Tests.DatabaseAccess.EntityFramework
{
    [TestClass]
    public class EfDataRepositoryTests
    {
        [TestMethod]
        public async Task Can_Save_And_Retrieve_Entity()
        {
            //Arrange
            var options = new DbContextOptionsBuilder<InMemoryDbContext>()
                .UseInMemoryDatabase(databaseName: "MyInMemoryDatabase")
                .Options;
            var inMemoryDbContext = new InMemoryDbContext(options);
            var customerRepo = new CustomerRepository(inMemoryDbContext);

            var customer = new Customer("Ernest", "Fakudze", Guid.NewGuid());
            customer.Id = 1;
            customer.ObjectState = ObjectState.Added;
            Guid custGuid = customer.Guid;

            //Act
            customerRepo.AddOrUpdateEntity(customer);
            await customerRepo.SaveChanges();
            var custInDb = await customerRepo.FindEntityByGuid(custGuid);

            //Assert
            Assert.IsNotNull(custInDb);
            Assert.AreEqual(custGuid, custInDb.Guid);
        }
    }
}
