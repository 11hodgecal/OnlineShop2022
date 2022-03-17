using OnlineShop2022.Data;
using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;

namespace OnlineShop2022.Models
{
    public class OrderRepository : IOrderRepository
    {
        private readonly AppDbContext _appDbContext;
        private readonly ShoppingCartModel _shoppingCart;

        public OrderRepository(AppDbContext appDbContext, ShoppingCartModel shoppingCart)
        {
            _appDbContext = appDbContext;
            _shoppingCart = shoppingCart;
        }

        public async void CreateOrder(OrderModel order, string UserId)
        {
            order.OrderPlaced = DateTime.Now;

            _appDbContext.Orders.Add(order);

            await _appDbContext.SaveChangesAsync();

            var shoppingCartItems = _shoppingCart.ShoppingCartItems;

            order.OrderTotal = 0;
            //attaches the userid to the order
            order.UserID = UserId;

            foreach (var shoppingCartItem in shoppingCartItems)
            {
                var orderDetail = new OrderDetailModel()
                {
                    Amount = shoppingCartItem.Amount,
                    ProductId = shoppingCartItem.Product.Id,
                    OrderId = order.OrderId,
                    Price = shoppingCartItem.Product.Price,
                    
                    
                };
                //adds the item price to the order total
                order.OrderTotal += shoppingCartItem.Product.Price;
                _appDbContext.OrderDetails.Add(orderDetail);
            }
            await _appDbContext.SaveChangesAsync();
        }
    }
}
