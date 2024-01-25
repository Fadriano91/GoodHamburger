using GoodHamburger.Models;

namespace GoodHamburger.Repositories.interfaces
{
    public interface IOrderRepository
    {
        Task<OrderModel> SearchOrderById(int id);
        Task<List<OrderModel>> SearchAllOrders();
        Task<OrderModel> CreateOrder(OrderModel order);
        Task<OrderModel> UpdateOrder(OrderModel order);
        Task<bool> RemoveOrder(int id);
    }
}
