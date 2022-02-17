using System;
using Xunit;
using OnlineShop2022;
using Microsoft.Extensions.Logging;
using OnlineShop2022.Controllers;
using OnlineShop2022.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using OnlineShop2022.Areas.Admin;
using OnlineShop2022.Models;
using OnlineShop2022.Helpers;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using Microsoft.AspNetCore.Http;
using Moq;

namespace TestProject
{
    public class ProductControllerTests
    {
        private ILogger<ProductController> _logger;
        private AppDbContext _db;
        private IWebHostEnvironment _webHostEnviroment;

        private void CreateMocDB()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>().UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;
            _db = new AppDbContext(options);
            _db.Database.EnsureCreated();
        }
        [Fact]
        public async void ProductUpdateReturnNull()
        {
            //arrange
            CreateMocDB();
            var _images = new Images(_webHostEnviroment);
            ProductController controller = new ProductController(_db,_webHostEnviroment, _images);
            ProductViewModel product = new ProductViewModel();
            product.Product = new ProductModel();
            product.Product.Description = "DFADFADFA";
            product.Product.Id = 1;
            //act 
            var result = await controller.Update(2, product) as NotFoundResult;
            //assert
            Assert.Equal("404", result.StatusCode.ToString());
            
        }


    }
}
