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
    public class OrderRepositoryTest
    {
        private AppDbContext _db;

        
        private async Task CreateMocDBAsync()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>().UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;
            _db = new AppDbContext(options);
            

            await _db.SaveChangesAsync();
            _db.Database.EnsureCreated();
        }
        

        [Fact]
        public async void OrderCreateSuccess()
        {
            //arrange
            CreateMocDBAsync();
            
            ProductModel product = new ProductModel();
            product = new ProductModel();
            product.Description = "Test desc";
            product.Id = 1;
            product.Price = 5;
            
            await _db.Products.AddAsync(product);

            var cart = new ShoppingCartModel(_db);
            cart.ShoppingCartId = "Test";
            cart.AddToCart(product, 1);
            cart.GetShoppingCartItems();

            var repositiory = new OrderRepository(_db,cart);
            

            var Order = new OrderModel {AddressLine1 = "test", AddressLine2 = "test",FirstName = "test", LastName = "test", 
                City ="test", Postcode = "test", Country="uk",Email = "test1234@gmail.com", OrderId = 1 };

            var expected = 1;
            //act
            repositiory.CreateOrder(Order);

            //assert
            
            Assert.Equal(expected, Order.OrderLines.Count);
            

        }
        [Fact]
        public async void OrderTotalCalculated()
        {
            //arrange
            CreateMocDBAsync();

            ProductModel product = new ProductModel();
            product = new ProductModel();
            product.Description = "Test desc";
            product.Id = 1;
            product.Price = 5;

            await _db.Products.AddAsync(product);

            var cart = new ShoppingCartModel(_db);
            cart.ShoppingCartId = "Test";
            cart.AddToCart(product, 1);
            cart.GetShoppingCartItems();

            var repositiory = new OrderRepository(_db, cart);


            var Order = new OrderModel { AddressLine1 = "test", AddressLine2 = "test", FirstName = "test", LastName = "test", City = "test", 
                Postcode = "test", Country = "uk", Email = "test1234@gmail.com", OrderId = 1 };

            double expected = 0.0;

            //act
            repositiory.CreateOrder(Order);

            foreach (var item in Order.OrderLines)
            {
                expected += item.Price;
            }
            //assert

            Assert.Equal(expected, Order.OrderTotal);


        }
    }
}
