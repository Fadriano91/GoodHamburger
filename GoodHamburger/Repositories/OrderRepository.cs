using GoodHamburger.Data;
using GoodHamburger.Models;
using GoodHamburger.Repositories.interfaces;
using Microsoft.EntityFrameworkCore;

namespace GoodHamburger.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly GoodHamburgerDbContext _dbContext;

        public OrderRepository(GoodHamburgerDbContext goodHamburgerDbContext)
        {
            _dbContext = goodHamburgerDbContext;   
        }
        public async Task<OrderModel> CreateOrder(OrderModel order)
        {
            await _dbContext.Orders.AddAsync(order);
            await _dbContext.SaveChangesAsync();

            return order;

        }

        public async Task<List<OrderModel>> SearchAllOrders()
        {
            return await _dbContext.Orders.ToListAsync();
        }

        public async Task<OrderModel> SearchOrderById(int id)
        {
            return await _dbContext.Orders.FirstAsync(x => x.Id == id) ;
        }

        public async Task<OrderModel> UpdateOrder(OrderModel order)
        {
            OrderModel orderForId = await SearchOrderById(order.Id);
            if(orderForId == null)
            {
                throw new Exception($"Order for Id: {order.Id} is not found!");
            }

            orderForId.Sandwich = order.Sandwich;
            orderForId.Extras = order.Extras;
            orderForId.Total = order.Total;

            _dbContext.Orders.Update(orderForId);
            await _dbContext.SaveChangesAsync();

            return orderForId;
        }
        
        public async Task<bool> RemoveOrder(int id)
        {
            OrderModel orderForId = await SearchOrderById(id);

            if (orderForId == null)
            {
                throw new Exception($"Order for Id: {id} is not found!");
            }

            _dbContext.Orders.Remove(orderForId);
            await _dbContext.SaveChangesAsync();
            return true;
        }

    }
}
