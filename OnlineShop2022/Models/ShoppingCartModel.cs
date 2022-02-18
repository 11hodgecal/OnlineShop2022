using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using OnlineShop2022.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineShop2022.Models
{
    public class ShoppingCartModel
    {
        private readonly AppDbContext _appDbContext;

        public ShoppingCartModel(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public string ShoppingCartId { get; set; }

        public List<ShoppingCartItemModel> ShoppingCartItems { get; set; }

        public static ShoppingCartModel GetCart(IServiceProvider services)
        {
            ISession session = services.GetRequiredService<IHttpContextAccessor>()?.HttpContext.Session;

            var context = services.GetService<AppDbContext>();
            string cartId = session.GetString("CartId") ?? Guid.NewGuid().ToString();

            session.SetString("CartId", cartId);

            return new ShoppingCartModel(context) { ShoppingCartId = cartId };
        }

        public async void AddToCart(ProductModel Product, int qty)
        {
            var shoppingCartItem = _appDbContext.ShoppingCartItems.SingleOrDefault(
                s => s.Product.Id == Product.Id && s.ShoppingCartId == ShoppingCartId);

            if(shoppingCartItem == null)
            {
                shoppingCartItem = new ShoppingCartItemModel
                {
                    ShoppingCartId = ShoppingCartId,
                    Product = Product,
                    Amount = qty
                };

                _appDbContext.ShoppingCartItems.Add(shoppingCartItem);
            }
            else
            {
                shoppingCartItem.Amount++;
            }
            await _appDbContext.SaveChangesAsync();
        }

        public int RemoveFromCart(ProductModel Product)
        {
            var shoppingCartItem = _appDbContext.ShoppingCartItems.SingleOrDefault(
                s => s.Product.Id == Product.Id && s.ShoppingCartId == ShoppingCartId);
            var localAmount = 0;

            if (shoppingCartItem != null)
            {
                if(shoppingCartItem.Amount>1)
                {
                    shoppingCartItem.Amount--;
                    localAmount = shoppingCartItem.Amount;
                }
                else
                {
                    _appDbContext.ShoppingCartItems.Remove(shoppingCartItem);
                }
            }

            _appDbContext.SaveChanges();

            return localAmount;
        }

        public List<ShoppingCartItemModel> GetShoppingCartItems()
        {
            return ShoppingCartItems ??
                (ShoppingCartItems = _appDbContext.ShoppingCartItems.Where(c => c.ShoppingCartId == ShoppingCartId)
                .Include(s => s.Product)
                .ToList());
        }

        public async void ClearCart()
        {
            var cartItems = _appDbContext
                .ShoppingCartItems
                .Where(cart => cart.ShoppingCartId == ShoppingCartId);

            _appDbContext.ShoppingCartItems.RemoveRange(cartItems);

            await _appDbContext.SaveChangesAsync();
        }

        public double GetShoppingCartTotal()
        {
            var total = _appDbContext.ShoppingCartItems.Where(c => c.ShoppingCartId == ShoppingCartId)
                .Select(c => c.Product.Price * c.Amount).Sum();
            return total;
        }
    }
}
