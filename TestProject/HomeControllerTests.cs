using System;
using Xunit;
using OnlineShop2022;
using Microsoft.Extensions.Logging;
using OnlineShop2022.Controllers;
using OnlineShop2022.Data;
using Microsoft.EntityFrameworkCore;

namespace TestProject
{
    public class HomeControllerTests
    {
        private ILogger<HomeController> _logger;
        private AppDbContext _db;

        private void CreateMocDB()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>().UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;
            _db = new AppDbContext(options);
            _db.Database.EnsureCreated();
        }
        [Fact]
        public void HomeControllerIndexNotNull()
        {
            //Arrange
            CreateMocDB();
            var home = new HomeController(_logger, _db);

            //Act
            var result = home.Index();
            //Assert
            Assert.NotNull(result);
        }

        [Fact]
        public void PrivacyNotNull()
        {
            //Arrange
            CreateMocDB();
            var home = new HomeController(_logger, _db);

            //Act
            var result = home.Privacy();
            //Assert
            Assert.NotNull(result);

        }

        [Fact]
        public void ProductsNotNull()
        {
            //Arrange
            CreateMocDB();
            var home = new HomeController(_logger, _db);

            //Act
            var result = home.Products("dfadfa31141fadfa");
            //Assert
            Assert.NotNull(result);

        }
    }
}
