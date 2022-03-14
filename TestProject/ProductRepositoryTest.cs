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
using System.Threading.Tasks;
using System.Linq;

namespace TestProject
{
    public class ProductRepositoryTest
    {
        private AppDbContext _db;

        
        private async Task CreateMocDBAsync()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>().UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;
            _db = new AppDbContext(options);
            var Product = new ProductModel();
            Product.Description = "Test1";
            Product.Id = 1;
            Product.Price = 5;
            await _db.Products.AddAsync(Product);

            var Product2 = new ProductModel();
            Product2.Description = "Test2";
            Product2.Id = 2;
            Product2.Price = 5;
            await _db.Products.AddAsync(Product2);

            await _db.SaveChangesAsync();
            _db.Database.EnsureCreated();
        }

        [Fact]
        public async void ProductProductsReturnsList()
        {
            //arrange
            CreateMocDBAsync();
            var productrepository = new ProductRepository(_db);
            //act 

            var result = productrepository.Products;
            

            //assert
            Assert.NotNull(result);

        }

        [Fact]
        public async void GetsAllProducts()
        {
            //arrange
            CreateMocDBAsync();
            var productrepository = new ProductRepository(_db);
            //act 

            var result = productrepository.GetAllProducts();
            var expected = 2;

            //assert
            Assert.Equal(expected, result.Count());

        }
        [Fact]
        public async void GetsOneProduct()
        {
            //arrange
            CreateMocDBAsync();
            var productrepository = new ProductRepository(_db);
            //act 

            var result = productrepository.GetProductById(1);
            var expected = "Test1";

            //assert
            Assert.Equal(expected, result.Description);

        }

    }
}
