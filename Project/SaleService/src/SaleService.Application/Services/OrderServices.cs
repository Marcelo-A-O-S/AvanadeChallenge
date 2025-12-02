using SaleService.Application.Interfaces;
using SaleService.Domain.Entities;
using SaleService.Domain.Interfaces;
using System.Linq.Expressions;

namespace SaleService.Application.Services
{
    public class OrderServices : IOrderServices
    {
        private readonly IOrderRepository orderRepository;
        public OrderServices(IOrderRepository _orderRepository)
        {
            this.orderRepository = _orderRepository;
        }
        public async Task Delete(Order entity)
        {
            await this.orderRepository.Delete(entity);
        }

        public async Task<List<Order>> FindAllBy(Expression<Func<Order, bool>> predicate)
        {
            return await this.orderRepository.FindAllBy(predicate);
        }

        public async Task<Order> FindBy(Expression<Func<Order, bool>> predicate)
        {
            return await this.orderRepository.FindBy(predicate);
        }

        public async Task<List<Order>> GetAllByUserId(long userId, int page = 1, int itemsPage = 10)
        {
            return await this.orderRepository.GetAllByUserId(userId, page, itemsPage);
        }

        public async Task<Order> GetById(int Id)
        {
            return await this.orderRepository.GetById(Id);
        }

        public async Task<int> GetQuantity()
        {
            return await this.orderRepository.GetQuantity();
        }

        public async Task<int> GetQuantityByUserId(long userId)
        {
            return await this.orderRepository.GetQuantityByUserId(userId);
        }

        public async Task<List<Order>> List()
        {
            return await this.orderRepository.List();
        }

        public async Task<List<Order>> List(int page)
        {
            return await this.orderRepository.List(page);
        }

        public async Task Save(Order entity)
        {
            if (entity.Id == 0)
            {
                await this.orderRepository.Save(entity);
            }
            else
            {
                await this.orderRepository.Update(entity);
            }
        }
        public async Task Update(Order entity)
        {
            await this.orderRepository.Update(entity);
        }
    }
}
