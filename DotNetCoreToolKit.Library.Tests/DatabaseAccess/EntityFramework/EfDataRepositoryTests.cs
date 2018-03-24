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
            var customerRepo = new CustomerRepository(inMemoryDbContext, null);

            var customer = new Customer("Ernest", "Fakudze", Guid.NewGuid());

            customer.ObjectState = ObjectState.Added;
            Guid custGuid = customer.Id;

            //Act
            await customerRepo.SaveEntity(customer);
            var custInDb = await customerRepo.FindById(custGuid);

            //Assert
            Assert.IsNotNull(custInDb);
            Assert.AreEqual(custGuid, custInDb.Id);
        }
    }
}
