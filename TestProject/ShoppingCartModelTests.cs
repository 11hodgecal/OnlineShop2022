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
    public class ShoppingCartModelTests
    {
        private AppDbContext _db;

        private async Task CreateMocDBAsync()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>().UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;
            _db = new AppDbContext(options);
            ProductModel product = new ProductModel();
            product = new ProductModel();
            product.Description = "Test desc";
            product.Id = 1;
            product.Price = 5;
            


            ShoppingCartItemModel item = new ShoppingCartItemModel();
            item.Product = product;
            item.Amount = 2;
            item.ShoppingCartId = "Test";
            item.ShoppingCartItemId = product.Id;
            


            await _db.ShoppingCartItems.AddAsync(item);
            await _db.SaveChangesAsync();
            _db.Database.EnsureCreated();
        }
        private async Task CreateExtraProductandCartItem()
        {
            ProductModel product = new ProductModel();
            product = new ProductModel();
            product.Description = "Test desc2";
            product.Id = 2;
            product.Price = 5;

            ShoppingCartItemModel item2 = new ShoppingCartItemModel();
            item2.Product = product;
            item2.Amount = 1;
            item2.ShoppingCartId = "Test";
            item2.ShoppingCartItemId = product.Id;
            await _db.ShoppingCartItems.AddAsync(item2);
            await _db.SaveChangesAsync();
        }
        [Fact]
        public async void GetCartItemsSuccess()
        {
            bool NA = true;
            //arrange
            await CreateMocDBAsync();
            var cart = new ShoppingCartModel(_db);
            cart.ShoppingCartId = "Test";

            //act 
            cart.GetShoppingCartItems();

            //assert
            if(cart.ShoppingCartItems.Count != 0)
            {
                NA = false;
            }

            Assert.False(NA);


        }

        [Fact]
        public async void AddProductToCartSuccess()
        {
            bool Added = false;
            await CreateMocDBAsync();
            //arrange
            var cart = new ShoppingCartModel(_db);
            cart.ShoppingCartId = "Test";
            ProductModel product = new ProductModel();
            product = new ProductModel();
            product.Description = "Test desc";
            product.Id = 2;

            cart.AddToCart(product,2);
            cart.GetShoppingCartItems();

            //act 
            if(cart.ShoppingCartItems.Count == 2)
            {
                Added = true;
            }
            //assert
            Assert.True(Added);

        }
        [Fact]
        public async void RemoveProductFromCart_OutcomeRem1()
        {
            bool itemDeleted = false;
            await CreateMocDBAsync();
            //arrange

            var cart = new ShoppingCartModel(_db);
            cart.ShoppingCartId = "Test";
            ProductModel product = new ProductModel();
            product = new ProductModel();
            product.Description = "Test desc";
            product.Id = 1;
            
            cart.RemoveFromCart(product);

            var item = await _db.ShoppingCartItems.SingleOrDefaultAsync(
                s => s.ShoppingCartId == "Test");


            //act 
            if (item.Amount == 1)
            {
                itemDeleted = true;
            }


            //assert

            Assert.True(itemDeleted);
        }
        [Fact]
        public async void RemoveProductFromCart_OutcomeRemLast()
        {

            //arrange
            await CreateMocDBAsync();
            var cart = new ShoppingCartModel(_db);
            cart.ShoppingCartId = "Test";
            ProductModel product = new ProductModel();
            product = new ProductModel();
            product.Description = "Test desc";
            product.Id = 1;
            
            cart.RemoveFromCart(product);
            cart.RemoveFromCart(product);
            //act 

            var item = await _db.ShoppingCartItems.SingleOrDefaultAsync(
                s => s.ShoppingCartId == "Test");

            //assert
            Assert.Null(item);

        }
        [Fact]
        public async void ClearAllCartItemsSuccess()
        {

            //arrange
            await CreateMocDBAsync();
            var cart = new ShoppingCartModel(_db);
            cart.ShoppingCartId = "Test";

            await CreateExtraProductandCartItem();
            //act 
            cart.ClearCart();
            cart.GetShoppingCartItems();
            //assert
            Assert.Empty(cart.ShoppingCartItems);
            

        }
        [Fact]
        public async void GetCartTotalSuccess()
        {
            //Arrange
            await CreateMocDBAsync();
            await CreateExtraProductandCartItem();
            var cart = new ShoppingCartModel(_db);
            cart.ShoppingCartId = "Test";

            double expected = 0;

            var ShoppingCartItems = _db.ShoppingCartItems.Where(c => c.ShoppingCartId == "Test");

            foreach(var item in ShoppingCartItems)
            {
                
                expected = expected + (item.Product.Price * item.Amount);
            }

            //Act
            cart.GetShoppingCartItems();
            var result = cart.GetShoppingCartTotal();
            Console.WriteLine("");
            //Assert
            Assert.Equal(expected, result);
        }


    }

}
