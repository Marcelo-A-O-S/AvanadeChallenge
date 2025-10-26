using StockService.Application.Interfaces;
using StockService.Domain.Entities;
using StockService.Domain.Interfaces;
using System.Linq.Expressions;

namespace StockService.Application.Services
{
    public class StockMovementServices : IStockMovementServices
    {
        private readonly IStockMovementRepository stockRepository;
        public StockMovementServices(IStockMovementRepository _stockRepository)
        {
            this.stockRepository = _stockRepository;
        }
        public async Task Delete(StockMovement entity)
        {
            await this.stockRepository.Delete(entity);
        }
        public async Task<StockMovement> FindBy(Expression<Func<StockMovement, bool>> predicate)
        {
            return await this.stockRepository.FindBy(predicate);
        }

        public async Task<StockMovement> GetById(long Id)
        {
            return await this.stockRepository.GetById(Id);
        }

        public async Task<List<StockMovement>> List()
        {
            return await this.stockRepository.List();
        }

        public async Task<List<StockMovement>> List(int? page)
        {
            return await this.stockRepository.List(page);
        }

        public async Task Save(StockMovement entity)
        {
            if(entity.Id == 0)
            {
                await this.stockRepository.Save(entity);
            }
            else
            {
                await this.stockRepository.Update(entity);
            }
        }

        public async Task Update(StockMovement entity)
        {
            await this.stockRepository.Update(entity);
        }
    }
}